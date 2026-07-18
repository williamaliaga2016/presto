using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;
using System.Linq;


public sealed class BearerTokenInterceptor : Interceptor
{
    private readonly Func<Task<string>> _getToken;

    public BearerTokenInterceptor(Func<Task<string>> getToken)
        => _getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        // OJO: esto bloquea el hilo. Normalmente OK porque el token supplier cachea.
        // Si tu token supplier realmente hace red aquí, luego te paso versión async completa.
        var token = _getToken().GetAwaiter().GetResult();

        var headers = context.Options.Headers ?? new Metadata();

        // En gRPC metadata la key debe ir en minúsculas
        var existingAuth = headers.FirstOrDefault(h => h.Key == "authorization");

        if (existingAuth != null)
        {
            headers.Remove(existingAuth);
        }

        headers.Add(new Metadata.Entry("authorization", $"Bearer {token}"));

        var newOptions = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            newOptions);

        return continuation(request, newContext);
    }
}
