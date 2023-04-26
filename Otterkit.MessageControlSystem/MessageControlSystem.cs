using Otterkit.MessageHandling;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build(); 

app.UseWebSockets();

app.Map("/mcs", async context => await MessageHandler.HandleConnection(context));

app.Run();
