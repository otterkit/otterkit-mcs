using Microsoft.AspNetCore.Server.Kestrel.Core;
using Otterkit.MessageTags;

string OtterSock = $"{Path.GetTempPath()}otter.sock";

if (File.Exists(OtterSock)) File.Delete(OtterSock);

var builder = WebApplication.CreateBuilder();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5151, listenOptions =>
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

var tag = MessageTag.Null;

mcs.MapGet("/", async (HttpResponse response) => 
{
    var result = await response.BodyWriter.WriteAsync(tag.Message);
});

mcs.Run();
