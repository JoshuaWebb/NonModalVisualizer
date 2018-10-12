using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AsyncDebuggerVisualizerTest.Visualizer.Model;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public class AdvtObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var targetString = (string) target;
            var message = new Message
            {
                Data = targetString,
                Port = -1
            };

            var formatter = new BinaryFormatter();
            formatter.Serialize(outgoingData, message);
        }
    }
}
