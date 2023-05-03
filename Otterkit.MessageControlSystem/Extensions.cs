using System.IO.Pipelines;
using System.Buffers;

namespace Otterkit.MessageControlSystem;

internal static class HelperExtensions
{
    public static async ValueTask<ReadOnlyMemory<byte>> ReadAllAsync(this PipeReader reader)
    {
        while (true)
        {
            var readAsync = await reader.ReadAsync();

            if (readAsync.IsCompleted)
            {
                var result = readAsync.Buffer.ToArray();

                reader.AdvanceTo(readAsync.Buffer.End);
                
                return result;
            }

            reader.AdvanceTo(readAsync.Buffer.Start, readAsync.Buffer.End);
        }
    }
}
