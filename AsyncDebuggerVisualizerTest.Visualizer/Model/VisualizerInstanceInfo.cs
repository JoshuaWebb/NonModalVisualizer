namespace AsyncDebuggerVisualizerTest.Visualizer.Model
{
    public struct VisualizerInstanceInfo
    {
        public int ProcessId { get; }
        public int TcpPort { get; }

        public VisualizerInstanceInfo(int processId, int tcpPort)
        {
            ProcessId = processId;
            TcpPort = tcpPort;
        }

        public override string ToString()
        {
            return $"Port: {TcpPort} - Process: {ProcessId}";
        }
    }
}
