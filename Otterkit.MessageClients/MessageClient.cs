using System.Net.Sockets;
using Otterkit.MessageTags;

namespace Otterkit.MessageClients;

public class MessageClient
{
    static string OtterSock = $"{Path.GetTempPath()}otter.sock";

    readonly HttpClient internalClient;

    public MessageClient(string? baseAddress = null)
    {
        var socketHandler  = new SocketsHttpHandler
        {
            ConnectCallback = static async (context, token) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

                var endpoint = new UnixDomainSocketEndPoint(OtterSock);

                await socket.ConnectAsync(endpoint).SetupContext();

                return new NetworkStream(socket, false);
            }
        };

        internalClient = new HttpClient(socketHandler, true);

        if (baseAddress is not null)
        {
            internalClient.BaseAddress = new Uri(baseAddress);
        }
    }

    public async ValueTask<Memory<byte>> ReceiveAsync(string endpoint)
    {
        var response = await internalClient.GetAsync(endpoint).SetupContext();
        
        return await response.Content.ReadAsByteArrayAsync().SetupContext();
    }

    public async ValueTask<Memory<byte>> SendAsync(string endpoint, MessageTag tag)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

        request.Content = new ByteArrayContent(tag.Message);

        var post = await internalClient.SendAsync(request);

        return await post.Content.ReadAsByteArrayAsync().SetupContext();
    }
}
