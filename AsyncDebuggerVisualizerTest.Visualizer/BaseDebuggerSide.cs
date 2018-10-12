using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AsyncDebuggerVisualizerTest.Visualizer.Helpers;
using AsyncDebuggerVisualizerTest.Visualizer.Model;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public abstract class BaseDebuggerSide : DialogDebuggerVisualizer
    {
        protected VisualizerInstanceInfo LaunchNewInstance()
        {
            // TODO: don't hardcode this / configure this somehow?
            var exePath = @"C:\Users\joshua.webb\Documents\Visual Studio 2015\Projects\AsyncDebuggerVisualizerTest\AsyncDebuggerVisualizerTest\bin\Debug\AsyncDebuggerVisualizerTest.App.exe";
            var process = Process.Start(exePath);
            CommunicationHelper.WaitForProcessToStartListeningOnAPort(process.Id);

            var newInstances = FindVisualizerInstances();
            var launchedInstance = newInstances.Single(i => i.ProcessId == process.Id);
            return launchedInstance;
        }

        protected static List<VisualizerInstanceInfo> FindVisualizerInstances()
        {
            var visualizerProcessName = "AsyncDebuggerVisualizerTest.App";
            var connections = PortSniffer.GetAllTCPConnections();
            var candidateProcesses = Process.GetProcessesByName(visualizerProcessName);
            var matchedConnectionInfos = new List<VisualizerInstanceInfo>();
            foreach (var connection in connections)
            {
                foreach (var candidateProcess in candidateProcesses)
                {
                    if (connection.ProcessId == candidateProcess.Id)
                    {
                        matchedConnectionInfos.Add(new VisualizerInstanceInfo((int)connection.ProcessId, connection.LocalPort));
                    }
                }
            }

            return matchedConnectionInfos;
        }
    }
}
