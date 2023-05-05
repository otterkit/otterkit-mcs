using System.Text;
using System.Net.Sockets;

await Task.WhenAll(RunClient(args[0]), RunClient(args[0]), RunClient(args[0]), RunClient(args[0]));

async Task RunClient(string message)
{
    using Socket client = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

    var endpoint = new UnixDomainSocketEndPoint($"{Path.GetTempPath()}otter.sock");

    await client.ConnectAsync(endpoint);

    var messageBytes = Encoding.UTF8.GetBytes(message);

    await client.SendAsync(messageBytes, CancellationToken.None);

    Console.WriteLine($"Ping message: \"{message}\"");

    var buffer = new byte[4096];

    var received = await client.ReceiveAsync(buffer, CancellationToken.None);

    var memory = buffer.AsMemory().Slice(0, received);

    var response = Encoding.UTF8.GetString(memory.Span);

    if (response.EndsWith("EOM"))
    {
        Console.WriteLine($"Pong: \"{response}\"");
    }
    else
    {
        Console.WriteLine($"Error: \"{response}\"");
    }
    
    client.Shutdown(SocketShutdown.Both);

    client.Dispose();
}
