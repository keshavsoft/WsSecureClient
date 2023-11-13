using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;

namespace WsSecureClient
{
    public partial class Form1 : Form
    {
        static String CommonUrlSecure = "wss://tallyws12.keshavsoft.net";

        static String CommonUrl = "ws://tallyws12.keshavsoft.net";

        static String CommonUrlLocal = "ws://localhost:3000";

        static String CommonUrlwash = "ws://washtex7.keshavsoft.net";

        static String CommonUrlBmm = "wss://bmmwdo.org";


        Uri Wsurl = new Uri(CommonUrlBmm);
        ClientWebSocket client = new ClientWebSocket();


        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            await client.ConnectAsync(Wsurl, CancellationToken.None);

            string msg = "hello0123456789123456789123456789123456789123456789123456789";
            SendMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)));


            SendSystemInfo();

            await receiveTask();

        }


        private void SendSystemInfo()
        {
            try
            {

                var macAddr =
    (
        from nic in NetworkInterface.GetAllNetworkInterfaces()
        where nic.OperationalStatus == OperationalStatus.Up
        select nic.GetPhysicalAddress().ToString()
    ).FirstOrDefault();

                var my_jsondata = new
                {
                    From = "Service",
                    SysMac = macAddr,
                    Type = "SysInfo"
                };

                string json_data = JsonConvert.SerializeObject(my_jsondata);

                SendMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(json_data)));
            }
            catch (Exception error)
            {
                listBox1.Items.Add("error " + error);
            };

        }


        private async Task receiveTask()
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                listBox1.Items.Add("message : " + message);

                if (message == "Accounts")
                {
                    listBox1.Items.Add("from ws : " + message);
                    string msg = "Accounts data from tally";
          
                }

                if (message == "Ledgers")
                {
                    listBox1.Items.Add("Ledgers : " + message);

                }

            }

        }


        private async void SendMessage(ArraySegment<byte> inDataToSend)
        {
            try
            {
                // restricted to 5 iteration only
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(inDataToSend, WebSocketMessageType.Text,
                                         true, CancellationToken.None);
                }
            }
            catch (Exception error)
            {
                //   WriteToFile("error " + error);
            };
        }

    }
}