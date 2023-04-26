using System.Net.WebSockets;
using Otterkit.MessageTags;

namespace Otterkit.MessageHandling;

public static class MessageHandler
{
    public static async ValueTask HandleConnection(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            await ReceiveTag(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async ValueTask ReceiveTag(WebSocket socket)
    {
        var buffer = new Memory<byte>(new byte[1024 * 4]);

        var receiveResult = await ReceiveAsync(socket, buffer);
        
        while (!socket.CloseStatus.HasValue)
        {   
            var tag = new MessageTag(buffer);

            Console.WriteLine($"MCS Received: [{tag.ToString("MCS")}]");
            Console.WriteLine($"METHOD Received: [{tag.ToString("METHOD")}]");
            Console.WriteLine($"ORIGIN Received: [{tag.ToString("ORIGIN")}]");

            buffer.Span.Clear();

            receiveResult = await ReceiveAsync(socket, buffer);
        }

        await CloseAsync(socket, receiveResult);
    }

    private static async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(WebSocket socket, Memory<byte> buffer)
    {
        return await socket.ReceiveAsync(buffer, CancellationToken.None);
    }

    private static async ValueTask SendAsync(WebSocket socket, Memory<byte> buffer, ValueWebSocketReceiveResult result)
    {
        await socket.SendAsync(buffer, result.MessageType, result.EndOfMessage, CancellationToken.None);
    }

    private static async ValueTask CloseAsync(WebSocket socket, ValueWebSocketReceiveResult result)
    {
        if (socket.CloseStatus.HasValue)
        {
            await socket.CloseAsync(socket.CloseStatus.Value, socket.CloseStatusDescription, CancellationToken.None);
        }
        else
        {
            await socket.CloseAsync(WebSocketCloseStatus.ProtocolError, socket.CloseStatusDescription, CancellationToken.None);
        }
    }
}
