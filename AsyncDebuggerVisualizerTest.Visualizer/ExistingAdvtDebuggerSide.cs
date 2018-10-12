using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.DebuggerVisualizers;
using AsyncDebuggerVisualizerTest.Visualizer;
using AsyncDebuggerVisualizerTest.Visualizer.Helpers;

[assembly: DebuggerVisualizer(
    typeof(ExistingAdvtDebuggerSide),
    typeof(AdvtObjectSource),
    Target = typeof(string),
    Description = "Existing ADVT")]

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public class ExistingAdvtDebuggerSide : BaseDebuggerSide
    {
        protected override async void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var initialVisualizerInstances = FindVisualizerInstances();

            using (var messageData = objectProvider.GetData())
            {
                if (initialVisualizerInstances.Count == 0)
                {
                    var launchedInstance = LaunchNewInstance();
                    await CommunicationHelper.SendToInstance(launchedInstance.TcpPort, messageData);
                }
                else if (initialVisualizerInstances.Count == 1)
                {
                    await CommunicationHelper.SendToInstance(initialVisualizerInstances[0].TcpPort, messageData);
                }
                else
                {
                    var selector = new VisualizerSelectorForm(initialVisualizerInstances, messageData);
                    windowService.ShowDialog(selector);
                }
            }
        }
    }
}
