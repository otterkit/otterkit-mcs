namespace Otterkit.MessageControlSystem;

public static class MainThread
{
    static string OtterSock = $"{Path.GetTempPath()}otter.sock";

    public static void Main(string[] args)
    {
        var unixSocket = new UnixSocketThread(OtterSock);

        unixSocket.StartThread();

        Console.WriteLine("[Otterkit MCS]: Press (ESC) to stop server...");

        var waitkey = Console.ReadKey(true);

        while (waitkey.Key != ConsoleKey.Escape)
        {
            waitkey = Console.ReadKey(true);
        }

        unixSocket.StopThread().AwaitResult();
    }
}
