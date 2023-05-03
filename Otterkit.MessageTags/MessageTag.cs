namespace Otterkit.MessageTags;

public readonly struct MessageTag
{
    private static readonly MessageTag NullInstance = new(false);
    public static MessageTag Null => NullInstance;

    public readonly byte[] Message { get; init; }

    public MessageTag(ReadOnlySpan<byte> message)
    {
        Message = new byte[message.Length];

        message.CopyTo(Message);
    }

    public MessageTag(bool useStaticInstance)
    {
        if (useStaticInstance)
        {
            this = Null;
            return;
        }

        this = new("NULL"u8);
    }

    public MessageTag()
    {
        this = Null;
    }
}
