namespace Server.KnowledgeGraph
{
    partial class KnowledgeGraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button recalculateLayoutButton;
        private System.Windows.Forms.Splitter splitter1;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gViewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.recalculateLayoutButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "No object is selected";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gViewer);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Location = new System.Drawing.Point(2, 54);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 661);
            this.panel1.TabIndex = 4;
            // 
            // gViewer
            // 
            this.gViewer.AsyncLayout = false;
            this.gViewer.AutoScroll = true;
            this.gViewer.BackwardEnabled = false;
            this.gViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gViewer.ForwardEnabled = false;
            this.gViewer.Graph = null;
            this.gViewer.Location = new System.Drawing.Point(0, 0);
            this.gViewer.MouseHitDistance = 0.05D;
            this.gViewer.Name = "gViewer";
            this.gViewer.NavigationVisible = true;
            this.gViewer.PanButtonPressed = false;
            this.gViewer.SaveButtonVisible = true;
            this.gViewer.Size = new System.Drawing.Size(694, 661);
            this.gViewer.TabIndex = 4;
            this.gViewer.ZoomF = 1D;
            this.gViewer.ZoomFraction = 0.5D;
            this.gViewer.ZoomWindowThreshold = 0.05D;
            this.gViewer.SelectionChanged += new System.EventHandler(this.gViewer_SelectionChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(694, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 661);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(697, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(298, 661);
            this.propertyGrid1.TabIndex = 1;
            // 
            // recalculateLayoutButton
            // 
            this.recalculateLayoutButton.Location = new System.Drawing.Point(2, 27);
            this.recalculateLayoutButton.Name = "recalculateLayoutButton";
            this.recalculateLayoutButton.Size = new System.Drawing.Size(106, 23);
            this.recalculateLayoutButton.TabIndex = 5;
            this.recalculateLayoutButton.Text = "Recalclulate Layout";
            this.recalculateLayoutButton.UseVisualStyleBackColor = true;
            this.recalculateLayoutButton.Click += new System.EventHandler(this.recalculateLayoutButton_Click);
            // 
            // OperationalBranching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 687);
            this.Controls.Add(this.recalculateLayoutButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "OperationalBranching";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operational Branching for LV Distribution Network";
            this.Load += new System.EventHandler(this.OperationalBranching_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Microsoft.Glee.GraphViewerGdi.GViewer gViewer;
        #endregion
    }
}