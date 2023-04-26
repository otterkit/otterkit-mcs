using System.Text;

namespace Otterkit.MessageTags;

// Message tag exchange, first request:

// MCS:oRPibtpJ90WdDsVLTwxw1g METHOD:SEND    ORIGIN:{URL/LOCAL-SERVER}...
// MCS:oRPibtpJ90WdDsVLTwxw1g METHOD:RECEIVE ORIGIN:{URL/LOCAL-SERVER}...
// MCS:oRPibtpJ90WdDsVLTwxw1g METHOD:CONNECT ORIGIN:{URL/LOCAL-SERVER}...

// Null message tag:

// MCS:NULL                   METHOD:NULL    ORIGIN:NULL

// Data body exchange, second request:

// BODY:{REQUEST-DATA}

public readonly struct MessageTag
{
    private static readonly MessageTag NullInstance = new(false);
    public static MessageTag NullTag => NullInstance;

    public readonly Memory<byte> Message { get; init; }

    public MessageTag(ReadOnlySpan<byte> mcs, ReadOnlySpan<byte> method, ReadOnlySpan<byte> origin)
    {
        Message = new byte[43 + origin.Length];

        Message.Span.Slice(0, 42).Fill(32);

        mcs.CopyTo(Message.Span);

        method.CopyTo(Message.Span.Slice(27));

        origin.CopyTo(Message.Span.Slice(42));

        Message.Span[Message.Length - 1] = 0;
    }

    public MessageTag(ReadOnlySpan<byte> message)
    {
        Message = new byte[message.Length];

        message.CopyTo(Message.Span);
    }

    public MessageTag(Memory<byte> message)
    {
        Message = message;
    }

    public MessageTag(bool useStaticInstance)
    {
        if (useStaticInstance)
        {
            this = NullTag;
            return;
        }

        this = new("MCS:NULL"u8, "METHOD:NULL"u8, "ORIGIN:NULL"u8);
    }

    public MessageTag()
    {
        this = NullTag;
    }

    public ReadOnlySpan<byte> Mcs
    {
        get => Message.Span.Slice(0, 26);
    }

    public ReadOnlySpan<byte> Method
    {
        get => Message.Span.Slice(27, 14);
    }

    public ReadOnlySpan<byte> Origin
    {
        get => Message.Span.Slice(42);
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(Message.Span);
    }

    public string ToString(ReadOnlySpan<char> part)
    {
        if (part.Equals("MCS", StringComparison.OrdinalIgnoreCase))
        {
            return Encoding.UTF8.GetString(Mcs);
        }

        if (part.Equals("METHOD", StringComparison.OrdinalIgnoreCase))
        {
            return Encoding.UTF8.GetString(Method);
        }

        if (part.Equals("ORIGIN", StringComparison.OrdinalIgnoreCase))
        {
            return Encoding.UTF8.GetString(Origin);
        }

        throw new ArgumentOutOfRangeException(nameof(part), "Only valid parts are: MCS, METHOD and ORIGIN");
    }
}
