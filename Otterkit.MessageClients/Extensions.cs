using System.Runtime.CompilerServices;

namespace Otterkit.MessageClients;

internal static class HelperExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ConfiguredTaskAwaitable SetupContext(this Task task)
    {
        return task.ConfigureAwait(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ConfiguredTaskAwaitable<T> SetupContext<T>(this Task<T> task)
    {
        return task.ConfigureAwait(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ConfiguredValueTaskAwaitable SetupContext(this ValueTask task)
    {
        return task.ConfigureAwait(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ConfiguredValueTaskAwaitable<T> SetupContex<T>(this ValueTask<T> task)
    {
        return task.ConfigureAwait(false);
    }
}
