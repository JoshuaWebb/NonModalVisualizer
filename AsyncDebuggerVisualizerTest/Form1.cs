using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncDebuggerVisualizerTest.Visualizer;
using Message = AsyncDebuggerVisualizerTest.Visualizer.Message;

namespace AsyncDebuggerVisualizerTest
{
    public partial class Form1 : Form
    {
        private DataListener Listener { get; set; }
        private TcpClient Client { get; set; }
        private int MyPort { get; set; }

        public Form1()
        {
            InitializeComponent();
            Listener = new DataListener();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Listener.StartListening(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            Listener.StopListening();
            base.OnClosed(e);
        }

        public void SetLabel(string label)
        {
            label1.Text = label;
        }

        public void AddMessage(Message message)
        {
            textBox1.Text += $"{message.Port}: {message.Data}{Environment.NewLine}";
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (Client == null)
                return;

            if (messageToSend.Text == "")
                return;

            var message = new Message
            {
                Port = MyPort,
                Data = messageToSend.Text
            };

            AddMessage(message);

            var networkStream = Client.GetStream();
            using (var streamWriter = new BinaryWriter(networkStream, Encoding.Default, true))
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, message);
                streamWriter.Write(memoryStream.Length);

                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(networkStream);
                await networkStream.FlushAsync();
                messageToSend.Text = null;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (portBox.Text == "")
                return;

            int port;
            if (int.TryParse(portBox.Text, out port))
            {
                if (Client != null)
                {
                    // TODO: send a hangup message explicitly?
                    Client.Client.Disconnect(false);
                    Client.Dispose();
                }

                Client = new TcpClient();
                Client.Connect(IPAddress.Loopback, port);
            }
        }

        public void SetPort(int port)
        {
            MyPort = port;
            Invoke(new Action(() => SetLabel($"Port: {port}")));
        }
    }
}
