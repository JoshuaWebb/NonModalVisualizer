using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncDebuggerVisualizerTest.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ssss = "Some string";
            var ttttt = "some other string";

            var developmentHost = new VisualizerDevelopmentHost(
                ssss,
                typeof(ExistingAdvtDebuggerSide),
                typeof(AdvtObjectSource)
            );

            developmentHost.ShowVisualizer();
        }
    }
}
