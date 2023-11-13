using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WsSecureClient
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;
        static String CommonUrlBmm = "wss://bmmwdo.org";
        static String CommonUrlLocal = "ws://localhost:3000";

        Uri Wsurl = new Uri(CommonUrlLocal);
        ClientWebSocket client = new ClientWebSocket();

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new ChildForms.TallyCheck();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private async void MDIParent1_Load(object sender, EventArgs e)
        {
            try
            {
                await client.ConnectAsync(Wsurl, CancellationToken.None);

                string msg = "New Connection New Socket Client From Desktop";
                SendMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)));

                if (client.State == WebSocketState.Open)
                {
                    MessageBox.Show("Success");
                }
                //SendSystemInfo();
                await ForWebSocket.Incoming.receiveTask(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        private void SendSystemInfo()
        {
            try
            {

                var macAddr = (
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
                //listBox1.Items.Add("error " + error);
            };

        }




    }
}
