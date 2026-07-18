using Data.Repository.Implementations;
using Framework.WorkFlow.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Multibanca.Automapper.FuncTransversal;
using Multibanca.Automapper.Multibanca;
using Multibanca.Automapper.Security;
using Multibanca.Common.Settings;
using Multibanca.Register.IoC.FuncTransversal;
using Multibanca.Register.IoC.Multibanca;
using Multibanca.Register.IoC.Security;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.WebSso;
using System.Security.Cryptography.X509Certificates;
using System.Text;

DotNetEnv.Env.TraversePath().Load();
var builder = WebApplication.CreateBuilder(args);

foreach (var item in builder.Configuration.AsEnumerable())
{
    if (item.Key.Contains("Saml"))
        Console.WriteLine($"{item.Key} = {item.Value}");
}

builder.Services.AddControllers().AddNewtonsoftJson();
IoCRegisterSecurity.AddRegistration(builder.Services);
IoCRegisterFuncTransversal.AddRegistration(builder.Services);
IoCRegisterMultibanca.AddRegistration(builder.Services);
builder.Services.AddEndpointsApiExplorer();


builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<AccesoTemporalSettings>(builder.Configuration.GetSection("AccesoTemporal"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Multibanca API",
        Version = "v1"
    });
});

// var hash = EncryptHelper.GenerateHash("Admin2026*");

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'Default'.");

builder.Services.AddDbContext<MultibancaDBContext>(options =>
    options.UseNpgsql(connectionString));

var connectionStringWF = builder.Configuration.GetConnectionString("dbWorkFlow")
    ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'dbWorkFlow'.");

builder.Services.AddDbContext<WorkFlowDBContext>(options =>
    options.UseNpgsql(connectionStringWF));

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfileSecurity());
    cfg.AddProfile(new AutoMapperProfileFuncTransversal());
    cfg.AddProfile(new AutoMapperProfileMultibanca());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors(o =>
{
    o.AddPolicy("Frontend", p =>
        p.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5174",
            "http://localhost:3000",
            "http://localhost:3000",
            "https://devbbvalegalizacion.cibergestion.com",
            "http://multibanca.local")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddMemoryCache();

// ── SAML2 + JWT ────────────────────────────────────────────────────
var samlConfig = builder.Configuration.GetSection("Saml2");

var authBuilder = builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie("Saml2.Cookies");

if (!string.IsNullOrEmpty(samlConfig["SpCertificatePath"]) &&
    !string.IsNullOrEmpty(samlConfig["IdpEntityId"]) &&
    !string.IsNullOrEmpty(samlConfig["IdpSsoUrl"]))
{
    var certPath = samlConfig["SpCertificatePath"]!;
    if (!Path.IsPathRooted(certPath))
        certPath = Path.Combine(builder.Environment.ContentRootPath, certPath);
    var spCert = new X509Certificate2(certPath, samlConfig["SpCertificatePassword"]!);

    authBuilder.AddSaml2(options =>
    {

        var entityId = samlConfig["EntityId"];
        var returnUrl = samlConfig["ReturnUrl"];
        var idpSsoUrl = samlConfig["IdpSsoUrl"];
        var idpEntityId = new EntityId(samlConfig["IdpEntityId"]!);

        Console.WriteLine($"EntityId='{entityId}'");
        Console.WriteLine($"ReturnUrl='{returnUrl}'");
        Console.WriteLine($"IdpSsoUrl='{idpSsoUrl}'");

        if (string.IsNullOrWhiteSpace(entityId))
            throw new Exception("EntityId vacío");

        if (string.IsNullOrWhiteSpace(returnUrl))
            throw new Exception("ReturnUrl vacío");

        if (string.IsNullOrWhiteSpace(idpSsoUrl))
            throw new Exception("IdpSsoUrl vacío");

        options.SPOptions.EntityId = new EntityId(entityId);
        options.SPOptions.ReturnUrl = new Uri(returnUrl);

        var idp = new IdentityProvider(idpEntityId, options.SPOptions)
        {
            Binding = Saml2BindingType.HttpPost,
            SingleSignOnServiceUrl = new Uri(idpSsoUrl)
        };

        options.SPOptions.EntityId = new EntityId(samlConfig["EntityId"]);
        options.SPOptions.ReturnUrl = new Uri(samlConfig["ReturnUrl"] ?? samlConfig["EntityId"]!);
        options.SPOptions.ServiceCertificates.Add(spCert);

        

        var idpSigningCertB64 = samlConfig["IdpCertificateSigning"]!;
        var idpSigningCert = new X509Certificate2(Convert.FromBase64String(idpSigningCertB64));
        idp.SigningKeys.AddConfiguredKey(idpSigningCert);

        options.IdentityProviders.Add(idp);
    });
}

authBuilder.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        LifetimeValidator = TokenLifetimeValidator.Validate,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.FromMinutes(2)
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
    options.UseSecurityTokenValidators = true;
});
// ── FIN AUTH ────────────────────────────────────────────────────────

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NxYtFisQPR08Cit/VkV+XU9AclRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3hTcEZjWHhcdXZXQmZcVk9yXg==");

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}" }
            };
        });
    });

    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public static class TokenLifetimeValidator
{
    public static bool Validate(
        DateTime? notBefore,
        DateTime? expires,
        SecurityToken tokenToValidate,
        TokenValidationParameters @param
    )
    {
        return (expires != null && expires > DateTime.UtcNow);
    }
}
