using System.Text;
using Otterkit.MessageTags;
using Otterkit.MessageClients;

var client = new MessageClient("https://localhost");

var receive = await client.ReceiveAsync("/");

var tag = new MessageTag("SEND ORIGIN:LOCAL ROUTE:ping DATA:MCS PING!"u8);

var send = await client.SendAsync("/send", tag);

Console.WriteLine(Encoding.UTF8.GetString(receive.Span));

Console.WriteLine(Encoding.UTF8.GetString(send.Span));


