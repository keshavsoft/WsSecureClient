using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WsSecureClient.ChildForms
{
    public partial class TallyCheck : Form
    {
        public TallyCheck()
        {
            InitializeComponent();
        }

        private async void TallyCheck_Load(object sender, EventArgs e)
        {
            WriteToFile("Service is recall at " + await HttpGetTallyAsync());
        }

        private async Task<string> HttpGetTallyAsync()
        {
            try
            {

                var url = "http://localhost:9000/";

                var client = new HttpClient();

                var response = await client.GetAsync(url);

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception error)
            {
                return null;
            };
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }


    }
}
