namespace Otterkit.MessageControlSystem;

public static class MainThread
{
    static string OtterSock = $"{Path.GetTempPath()}otter.sock";

    public static void Main(string[] args)
    {
        using var mutexLock = HandleMutexLock("Otterkit MCS");

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

    private static Mutex HandleMutexLock(string mutexName)
    {
        var mutex = new Mutex(false, $"""Global\{mutexName}""");

        if (!mutex.WaitOne(0))
        {
            Console.WriteLine("[Otterkit MCS]: An MCS instance is already running on this machine.");
            Environment.Exit(0);
        }

        return mutex;
    }
}
