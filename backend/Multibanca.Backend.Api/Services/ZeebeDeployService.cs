using GatewayProtocol;
using Grpc.Core;
using System.Net.Sockets;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;

public class ZeebeDeployService
{
    private readonly IZeebeClient _client;
    private readonly ILogger<ZeebeDeployService> _logger;

    public ZeebeDeployService(
        IZeebeClient client,
        ILogger<ZeebeDeployService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IDeployResourceResponse> DeployWithRetryAsync(
        string bpmnFilePath,
        CancellationToken ct = default)
    {
        var delays = new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(8),
            TimeSpan.FromSeconds(16)
        };

        Exception? last = null;

        for (var attempt = 1; attempt <= delays.Length; attempt++)
        {
            try
            {
                _logger.LogInformation(
                    "Deploy BPMN intento {Attempt}: {Path}",
                    attempt,
                    bpmnFilePath);

                var bytes = await File.ReadAllBytesAsync(bpmnFilePath, ct);
                var fileName = Path.GetFileName(bpmnFilePath);

                return await _client.NewDeployCommand()
                    .AddResourceBytes(bytes, fileName)
                    .Send(ct);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
            {
                // 👇 ESTE ES EL BLOQUE QUE PREGUNTABAS
                _logger.LogError(ex,
                    "Zeebe exige Authorization. El gateway tiene security activo o el cliente está mal configurado."+ex.Message);

                throw new InvalidOperationException(
                    "Zeebe exige Authorization (gateway security está habilitado). " +
                    "Revisa docker-compose o la configuración del ZeebeClient." + ex.Message,
                    ex);
            }
            catch (Exception ex) when (IsTransient(ex))
            {
                last = ex;
                _logger.LogWarning(
                    ex,
                    "Zeebe aún no listo. Reintentando en {Delay}s…",
                    delays[attempt - 1].TotalSeconds);

                await Task.Delay(delays[attempt - 1], ct);
            }
        }

        throw new InvalidOperationException(
            "No se pudo desplegar el BPMN tras varios intentos",
            last);
    }

    private static bool IsTransient(Exception ex)
    {
        if (ex is RpcException rpc)
        {
            // Transitorios típicos de arranque/red
            return rpc.StatusCode is StatusCode.Unavailable
                or StatusCode.DeadlineExceeded
                or StatusCode.ResourceExhausted;
        }

        return ex.InnerException is SocketException;
    }

}
