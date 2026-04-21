using Microsoft.AspNetCore.Server.Kestrel.Core;
using TableNet.WebApi.GrpcServer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(o =>
    {
        o.ListenAnyIP(5000, l =>
        {
            l.Protocols = HttpProtocols.Http2;
        });
    });

builder.Services
    .AddScoped<MessageService>();

WebApplication app = builder.Build();

app.MapGrpcService<MessageService>();
    
await app.RunAsync();