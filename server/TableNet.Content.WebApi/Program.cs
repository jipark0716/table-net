using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TableNet.WebApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(o => o.Interceptors.Add<AuthInterceptor>());
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(o =>
{
    o.RegisterAssemblyTypes(Assembly.GetEntryAssembly()!)
        .Where(t => typeof(ISingleton).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
        .AsImplementedInterfaces()
        .SingleInstance();

    o.RegisterAssemblyTypes(Assembly.GetEntryAssembly()!)
        .Where(t => typeof(IScoped).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
});

builder.WebHost.ConfigureKestrel(o =>
    {
        o.ListenAnyIP(5000, l =>
        {
            l.Protocols = HttpProtocols.Http2;
        });
    });

WebApplication app = builder.Build();



MethodInfo method = typeof(GrpcEndpointRouteBuilderExtensions)
    .GetMethods()
    .First(m => m is { Name: "MapGrpcService", IsGenericMethod: true });

foreach (Type type in Assembly.GetEntryAssembly()!
             .GetTypes()
             .Where(t => typeof(IGrpcServer).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false }))
{
    method.MakeGenericMethod(type).Invoke(null, [app]);
}

await app.RunAsync();