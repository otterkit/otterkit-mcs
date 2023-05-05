using System.Buffers;
using System.Net.Sockets;

namespace Otterkit.MessageControlSystem;

public class UnixSocketThread
{
    private static ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

    private readonly CancellationTokenSource Cancellation;
    private readonly Socket UnixSocket;
    private ValueTask ThreadTask;

    public UnixSocketThread(string unixSocketPath)
    {
        if (File.Exists(unixSocketPath)) File.Delete(unixSocketPath);

        Socket socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

        var endpoint = new UnixDomainSocketEndPoint(unixSocketPath);

        socket.Bind(endpoint);
        socket.Listen(100);

        UnixSocket = socket;
        Cancellation = new();
    }

    public void StartThread()
    {
        ThreadTask = StartThreadAsync(Cancellation.Token);
    }

    private async ValueTask StartThreadAsync(CancellationToken token)
    {
        Console.WriteLine($"[Otterkit MCS]: Listening on UDS otter.sock...");

        try
        {
            while (true)
            {
                var client = await UnixSocket.AcceptAsync(token);

                await HandleMessageAsync(client, token);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[Otterkit MCS]: Unix socket thread canceled...");
        }
    }

    private async ValueTask HandleMessageAsync(Socket socket, CancellationToken token)
    {
        var buffer = Pool.Rent(4096);

        try
        {
            var received = await socket.ReceiveAsync(buffer.AsMemory(), token);

            var response = buffer.AsMemory().Slice(0, received);

            if (response.Span.EndsWith("EOM"u8))
            {
                await socket.SendAsync(response, token);
            }
            else
            {
                var invalid = StatusMessages.InvalidTag;

                await socket.SendAsync(invalid.Message, token);
            }
        }
        finally
        {
            Pool.Return(buffer);
            socket.Dispose();
        }
    }

    public async ValueTask StopThread()
    {
        Cancellation.Cancel();

        await ThreadTask;

        Cancellation.Dispose();
        UnixSocket.Dispose();
    }
}
