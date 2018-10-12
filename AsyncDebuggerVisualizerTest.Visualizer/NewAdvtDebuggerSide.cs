using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using AsyncDebuggerVisualizerTest.Visualizer;
using AsyncDebuggerVisualizerTest.Visualizer.Helpers;

[assembly: DebuggerVisualizer(
    typeof(NewAdvtDebuggerSide),
    typeof(AdvtObjectSource),
    Target = typeof(string),
    Description = "New ADVT")]

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public class NewAdvtDebuggerSide : BaseDebuggerSide
    {
        protected override async void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var launchedInstance = LaunchNewInstance();
            await CommunicationHelper.SendToInstance(launchedInstance.TcpPort, objectProvider.GetData());
        }
    }
}
