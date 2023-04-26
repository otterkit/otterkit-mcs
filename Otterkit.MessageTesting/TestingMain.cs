using Otterkit.MessageTags;
using System.Text;

var tag = new MessageTag();

var serverTag = new MessageTag("MCS:oRPibtpJ90WdDsVLTwxw1g METHOD:RECEIVE ORIGIN:local-server-name"u8);

Console.WriteLine(tag.ToString("MCS"));
Console.WriteLine(tag.ToString("METHOD"));
Console.WriteLine(tag.ToString("ORIGIN"));

Console.WriteLine(serverTag.ToString("MCS"));
Console.WriteLine(serverTag.ToString("METHOD"));
Console.WriteLine(serverTag.ToString("ORIGIN"));

