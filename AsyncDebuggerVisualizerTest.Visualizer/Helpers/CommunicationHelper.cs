using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDebuggerVisualizerTest.Visualizer.Helpers
{
    public static class CommunicationHelper
    {
        public static void WaitForProcessToStartListeningOnAPort(int processId)
        {
            // TODO: there might be some issues with this if one of the sides was launched
            //       as admin but the other was not.
            bool created;
            using (var wh = new EventWaitHandle(false, EventResetMode.AutoReset, $"ADVT{processId}", out created))
                wh.WaitOne();
        }

        public static EventWaitHandle LetDudeKnowWereListeningOnSomePort(int processId)
        {
            bool created;
            var wh = new EventWaitHandle(false, EventResetMode.AutoReset, $"ADVT{processId}", out created);
            wh.Set();
            return wh;
        }

        public static async Task SendToInstance(int port, Stream stream)
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(IPAddress.Loopback, port);

                var networkStream = tcpClient.GetStream();
                using (var streamWriter = new BinaryWriter(networkStream, Encoding.Default, true))
                using (var messageData = stream)
                {
                    streamWriter.Write(messageData.Length);

                    messageData.Position = 0;
                    await messageData.CopyToAsync(networkStream);
                    await networkStream.FlushAsync();
                }
            }
        }
    }
}
