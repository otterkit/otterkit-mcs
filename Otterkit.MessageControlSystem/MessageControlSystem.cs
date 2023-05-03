using Microsoft.AspNetCore.Server.Kestrel.Core;
using Otterkit.MessageControlSystem;
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
        listenOptions.UseHttps();
    });
});

var mcs = builder.Build();

if (mcs.Environment.IsDevelopment())
{
    mcs.UseDeveloperExceptionPage();
}

var mcsTag = new MessageTag("MCS STATUS:ONLINE"u8);

mcs.MapGet("/", async (HttpResponse response) => 
{
    var result = await response.BodyWriter.WriteAsync(mcsTag.Message);
});

mcs.MapPost("/send", async (HttpRequest request, HttpResponse response) => 
{
    var readResult = await request.BodyReader.ReadAllAsync();

    var result = await response.BodyWriter.WriteAsync(readResult);
});

mcs.Run();
