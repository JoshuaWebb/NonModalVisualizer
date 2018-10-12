using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncDebuggerVisualizerTest.Visualizer.Helpers;
using AsyncDebuggerVisualizerTest.Visualizer.Model;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    public partial class VisualizerSelectorForm : Form
    {
        private MemoryStream MemoryStreamToVisualize { get; }

        public VisualizerSelectorForm(IEnumerable<VisualizerInstanceInfo> instances, Stream memoryStreamToVisualize)
        {
            InitializeComponent();
            MemoryStreamToVisualize = new MemoryStream();

            foreach (var instance in instances)
                VisualizerListBox.Items.Add(instance);

            memoryStreamToVisualize.CopyTo(MemoryStreamToVisualize);

            VisualizerListBox.MouseDoubleClick += VisualizerListBoxOnMouseDoubleClick;
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

            MemoryStreamToVisualize?.Dispose();

            base.Dispose(disposing);
        }

        private void VisualizerListBoxOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            // TODO: prevent sending to multiple instances at the same time.
            int index = VisualizerListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var selectedInstance = (VisualizerInstanceInfo)VisualizerListBox.Items[index];
                SendToSelectedInstance(selectedInstance);
                Close();
            }
        }

        private async void SendToSelectedInstance(VisualizerInstanceInfo instance)
        {
            Trace.WriteLine($"Send to: {instance}");
            await CommunicationHelper.SendToInstance(instance.TcpPort, MemoryStreamToVisualize);
        }
    }
}
