using Microsoft.AspNetCore.Server.Kestrel.Core;

string OtterSock = $"{Path.GetTempPath()}otter.sock";

if (File.Exists(OtterSock)) File.Delete(OtterSock);

var builder = WebApplication.CreateBuilder();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5114, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });

    options.ListenUnixSocket(OtterSock, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

var mcs = builder.Build();

if (mcs.Environment.IsDevelopment())
{
    mcs.UseDeveloperExceptionPage();
}

mcs.MapGet("/", () => "Hello from both IP and Unix sockets!");

mcs.Run();
