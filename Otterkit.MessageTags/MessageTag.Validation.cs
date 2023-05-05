namespace Otterkit.MessageTags;

public readonly partial struct MessageTag
{
    public MessageMethod ValidateMethod()
    {
        return Message.Span switch
        {
            // MCS bytes => 0x4D 0x43 0x53
            [0x4D, 0x43, 0x53, .. _] => MessageMethod.Mcs,

            // SEND bytes => 0x53 0x45 0x4E 0x44
            [0x53, 0x45, 0x4E, 0x44, .. _] => MessageMethod.Send,

            // RECEIVE bytes => 0x52 0x45 0x43 0x45 0x49 0x56 0x45
            [0x52, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, .. _] => MessageMethod.Receive,

            // None of the above methods =>
            [..] => MessageMethod.None
        };
    }
}
