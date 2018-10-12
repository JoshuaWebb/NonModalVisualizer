using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncDebuggerVisualizerTest.Visualizer.Model;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public class DataListener
    {
        private TcpListener TcpListener { get; set; }
        private Thread ListenThread { get; set; }
        private Form1 Form { get; set; }

        public void StartListening(Form1 form)
        {
            Form = form;

            TcpListener = new TcpListener(IPAddress.Loopback, 0);
            ListenThread = new Thread(ListenForClients);
            ListenThread.Start();
        }

        public void StopListening()
        {
            TcpListener.Stop();
        }

        private void ListenForClients()
        {
            TcpListener.Start();

            var port = ((IPEndPoint)TcpListener.LocalEndpoint).Port;
            Form.SetPort(port);

            while (true)
            {
                // wait for a new client
                TcpClient client;
                try
                {
                    client = TcpListener.AcceptTcpClient();
                }
                catch (SocketException se) when (se.SocketErrorCode == SocketError.Interrupted)
                {
                    break;
                }

                ThreadPool.QueueUserWorkItem(delegate { HandleClientComm(client); });
            }
        }

        private void HandleClientComm(TcpClient tcpClient)
        {
            using (tcpClient)
            using (var networkStream = tcpClient.GetStream())
            {
                while (true)
                {
                    using (var streamReader = new BinaryReader(networkStream, Encoding.Default, true))
                    using (var memoryStream = new MemoryStream())
                    {
                        var buffer = new byte[4096];

                        try
                        {
                            var messageLength = streamReader.ReadInt64();
                            var totalRead = 0;
                            while (totalRead < messageLength)
                            {
                                var bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                                totalRead += bytesRead;
                                memoryStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        catch (EndOfStreamException)
                        {
                            // client hung up
                            break;
                        }
                        catch (IOException)
                        {
                            // client hung up
                            if (!tcpClient.Connected)
                                break;

                            throw;
                        }

                        memoryStream.Position = 0;
                        var binaryFormatter = new BinaryFormatter();
                        var message = (Message) binaryFormatter.Deserialize(memoryStream);
                        Form.Invoke(new Action(() => Form.AddMessage(message)));
                    }
                }
            }
        }
    }
}
