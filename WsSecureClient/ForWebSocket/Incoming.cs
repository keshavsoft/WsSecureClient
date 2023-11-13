using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WsSecureClient.ForWebSocket
{
    public class Incoming
    {
        public static async Task receiveTask(ClientWebSocket inClient)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await inClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                ToLog.TodayText.WriteToFile(message);
                
                //listBox1.Items.Add("message : " + message);

                if (message == "Accounts")
                {
                    //listBox1.Items.Add("from ws : " + message);
                    string msg = "Accounts data from tally";

                }

                if (message == "Ledgers")
                {
                    //listBox1.Items.Add("Ledgers : " + message);

                }

            }

        }
    }

}
