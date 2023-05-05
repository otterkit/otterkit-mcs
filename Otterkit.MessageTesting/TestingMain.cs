using System.Text;
using System.Net.Sockets;

await Task.WhenAll(RunClient(args[0]), RunClient(args[0]), RunClient(args[0]), RunClient(args[0]));

async Task RunClient(string message)
{
    using Socket client = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

    var endpoint = new UnixDomainSocketEndPoint($"{Path.GetTempPath()}otter.sock");

    await client.ConnectAsync(endpoint);
    while (true)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        Console.WriteLine($"Ping message: \"{message}\"");

        // Receive ack.
        var buffer = new byte[1024];

        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);

        if (response.EndsWith("EOM"))
        {
            Console.WriteLine($"Pong: \"{response}\"");
            break;
        }
        else
        {
            Console.WriteLine($"Error: \"{response}\"");
            break;
        }
        
    }

    client.Shutdown(SocketShutdown.Both);
}
