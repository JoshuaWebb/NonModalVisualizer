using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncDebuggerVisualizerTest.Visualizer;
using AsyncDebuggerVisualizerTest.Visualizer.Helpers;
using Message = AsyncDebuggerVisualizerTest.Visualizer.Model.Message;

namespace AsyncDebuggerVisualizerTest
{
    public partial class Form1 : Form
    {
        private WaitHandle WaitHandle { get; set; }
        private DataListener Listener { get; set; }
        private TcpClient Client { get; set; }
        private int MyPort { get; set; }

        public Form1()
        {
            InitializeComponent();
            Listener = new DataListener();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            WaitHandle?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Listener.StartListening(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            Listener.StopListening();
            WaitHandle?.Dispose();
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
            Invoke(new Action(
                () =>
                {
                    Text = $"Port: {port}";
                    SetLabel($"Port: {port}");
                    listeningForDataToolStripMenuItem.Checked = true;
                }));

            WaitHandle?.Dispose();
            WaitHandle = CommunicationHelper.LetDudeKnowWereListeningOnSomePort(Process.GetCurrentProcess().Id);
        }

        private void listeningForDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var isListening = listeningForDataToolStripMenuItem.Checked;
            if (isListening)
            {
                Listener.StopListening();
                listeningForDataToolStripMenuItem.Checked = false;
            }
            else
            {
                Listener.StartListening(this);
            }
        }
    }
}
