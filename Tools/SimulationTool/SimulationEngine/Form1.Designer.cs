namespace SimulationEngine
{
    partial class Form1
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Step-1: Select Folder";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(730, 587);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(32, 213);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(202, 41);
            this.button3.TabIndex = 3;
            this.button3.Text = "Create Monitors";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Step-3: Create Monitors";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(388, 15);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(402, 147);
            this.listBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Step1: Select and Read OpenDSS file";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(32, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(173, 29);
            this.button5.TabIndex = 9;
            this.button5.Text = "Step-2: Parse DSS Files";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Step2: Parse OpenDSS file";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(409, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Step-5: Simulate";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(588, 219);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(202, 39);
            this.button4.TabIndex = 11;
            this.button4.Text = "Simulate";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 388);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 13;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(588, 171);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(202, 39);
            this.button6.TabIndex = 14;
            this.button6.Text = "Create MatlabAnalysis";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(409, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(151, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Step-4: Create Matlab Analysis";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(588, 276);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(204, 37);
            this.button7.TabIndex = 16;
            this.button7.Text = "Move Files";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(409, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Step-6: Move all Data Files";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(409, 343);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Step-7: Validate all Data Files";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(588, 330);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(202, 36);
            this.button8.TabIndex = 18;
            this.button8.Text = "Validate Simulated Files";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(292, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Step-2.1: Submit OpenDSS config as Individuals to Ontology";
            this.label10.Visible = false;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(32, 213);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(173, 37);
            this.button9.TabIndex = 20;
            this.button9.Text = "Step-2.1: Submit as Individuals";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 626);
            this.ControlBox = false;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Simulation Engine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button9;
    }
}

