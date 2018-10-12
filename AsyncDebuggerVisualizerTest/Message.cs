using System;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    [Serializable]
    public class Message
    {
        public int Port { get; set; }
        public string Data { get; set; }
    }
}