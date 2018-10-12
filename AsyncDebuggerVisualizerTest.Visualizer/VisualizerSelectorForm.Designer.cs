using System.Windows.Forms;

namespace AsyncDebuggerVisualizerTest.Visualizer
{
    partial class VisualizerSelectorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VisualizerListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // VisualizerListBox
            // 
            this.VisualizerListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VisualizerListBox.FormattingEnabled = true;
            this.VisualizerListBox.Location = new System.Drawing.Point(13, 13);
            this.VisualizerListBox.Name = "VisualizerListBox";
            this.VisualizerListBox.Size = new System.Drawing.Size(259, 82);
            this.VisualizerListBox.TabIndex = 0;
            // 
            // VisualizerSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 109);
            this.Controls.Add(this.VisualizerListBox);
            this.Name = "VisualizerSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisualizerSelectorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox VisualizerListBox;
    }
}