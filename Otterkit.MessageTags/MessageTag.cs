namespace Otterkit.MessageTags;

public readonly partial struct MessageTag
{
    public static readonly MessageTag Null = new("NULL"u8);

    public readonly ReadOnlyMemory<byte> Message { get; init; }

    public MessageTag(ReadOnlySpan<byte> message)
    {
        Message = message.ToArray();
    }

    public MessageTag(ReadOnlyMemory<byte> message)
    {
        Message = message;
    }

    public MessageTag()
    {
        this = Null;
    }
}
