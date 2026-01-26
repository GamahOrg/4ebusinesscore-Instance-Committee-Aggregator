using _4EPlatform.PermissionsGroupingChecker;
using _4EPlatform.PremessionRegister;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using System.Net;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7350, o => o.Protocols = HttpProtocols.Http2);

    // ADDED THIS LINE to fix the problem
    options.Listen(IPAddress.Any, 5399, o => o.Protocols = HttpProtocols.Http1);
    //  options.Listen(IPAddress.Any, 5041, o => o.Protocols = HttpProtocols.Http1);
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCPAggregatorGrpcClients(builder.Configuration["GrpcServiceSettings:CPAggregatorUrl"]);
builder.Services.AddCommitteeGrpcClients(builder.Configuration["GrpcServiceSettings:CommitteesUrl"]);
builder.Services.AddUserGrpcClients(builder.Configuration["GrpcServiceSettings:UserManagmentUrl"]);
builder.Services.AddDynamicWorkfloGrpcClients(builder.Configuration["GrpcServiceSettings:DynamicWorkflow"]);

builder.Host.UseSerilog((context, configuration) => configuration
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext().
        ReadFrom.Configuration(context.Configuration));

builder.Services.AddScoped<PermissionsGroupingCheckerMiddleware, PermissionsGroupingCheckerMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors",
        builder =>
        {
            builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
        });
});

builder.Services.AddRateLimiter(options =>
    {
        var rateLimitSection = builder.Configuration.GetSection("RateLimit");
        var permitLimit = rateLimitSection.GetValue<int>("PermitLimit");
        var queueLimit = rateLimitSection.GetValue<int>("QueueLimit");
        var windowSeconds = rateLimitSection.GetValue<int>("WindowSeconds");
        options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
        {
            opt.Window = TimeSpan.FromSeconds(windowSeconds);
            opt.PermitLimit = permitLimit;
            opt.QueueLimit = queueLimit;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        }).RejectionStatusCode = 429;
    });



builder.Services.AddAuthentication("Bearer")
      .AddJwtBearer("Bearer", options =>
      {
          // the name of your api resources   
          options.Audience = "UserManagementServer";
          /// identity server url                    
          options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
          options.RequireHttpsMetadata = false;
      });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceOwnerPolicy", policy =>
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireClaim("scope", "4EMicroServices"); // Require scope associated with Resource Owner Password token
    });

    options.AddPolicy("ClientCredentialsPolicy", policy =>
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireClaim("scope", "CPAggregatorAPI"); // Require scope associated with Client Credentials token
    });
});
// Add grpc config to serviceCollection
builder.Services.AddGrpc();

builder.Services.AddServices();

#region Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CommitteesManagement.Aggregator APIs Reference",
    });

    c.AddSecurityDefinition(
    "token",
    new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization
    }
        );
    c.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "token"
                            },
                        },
                        Array.Empty<string>()
                    }
    }
        );
});
#endregion

var app = builder.Build();
//app.MapGrpcService<AggregatorService>();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseCors("AllowCors");
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<PermissionsCheckerMiddleware>();
app.MapControllers();
app.UseMiddleware<PermissionsGroupingCheckerMiddleware>();
try
{
    app.Run();
}
catch (Exception ex)
{
    throw ex;
}
