using Otterkit.MessageTags;

namespace Otterkit.MessageControlSystem;

public static class StatusMessages
{
    public static readonly MessageTag MessageSent = new("MCS STATUS:MESSAGE-TAG-SENT"u8);

    public static readonly MessageTag InvalidTag = new("MCS STATUS:EC-MCS-INVALID-TAG"u8);
}
