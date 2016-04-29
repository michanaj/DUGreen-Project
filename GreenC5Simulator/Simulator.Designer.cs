namespace GreenC5Simulator
{
    partial class SimulatorWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatorWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.display = new GraphLib.PlotterDisplayEx();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.programExecutionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cbK2 = new System.Windows.Forms.ComboBox();
            this.cbK1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbNormCorrelation = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTotalExecution = new System.Windows.Forms.Label();
            this.gbEnergyConsumption = new System.Windows.Forms.GroupBox();
            this.lbTotalTime = new System.Windows.Forms.Label();
            this.lbAvgPower = new System.Windows.Forms.Label();
            this.lbExecJoule = new System.Windows.Forms.Label();
            this.lbWattsHour = new System.Windows.Forms.Label();
            this.lbEnergy = new System.Windows.Forms.Label();
            this.cbInfiniteExecution = new System.Windows.Forms.CheckBox();
            this.butClear = new System.Windows.Forms.Button();
            this.butStart = new System.Windows.Forms.Button();
            this.lbThreadCount = new System.Windows.Forms.Label();
            this.butAddNewThread = new System.Windows.Forms.Button();
            this.lbWattsUpPowerReading = new System.Windows.Forms.Label();
            this.lbIntelPowerReading = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbEnergyConsumption.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.display);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1278, 1028);
            this.splitContainer1.SplitterDistance = 454;
            this.splitContainer1.TabIndex = 0;
            // 
            // display
            // 
            this.display.AccessibleDescription = "";
            this.display.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.display.BackColor = System.Drawing.Color.Transparent;
            this.display.BackgroundColorBot = System.Drawing.Color.White;
            this.display.BackgroundColorTop = System.Drawing.Color.White;
            this.display.DashedGridColor = System.Drawing.Color.DarkGray;
            this.display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display.DoubleBuffering = true;
            this.display.Location = new System.Drawing.Point(0, 0);
            this.display.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.display.Name = "display";
            this.display.PlaySpeed = 0.5F;
            this.display.Size = new System.Drawing.Size(1278, 454);
            this.display.SolidGridColor = System.Drawing.Color.DarkGray;
            this.display.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.programExecutionPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.cbK2);
            this.splitContainer2.Panel2.Controls.Add(this.cbK1);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.lbNormCorrelation);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.lbTotalExecution);
            this.splitContainer2.Panel2.Controls.Add(this.gbEnergyConsumption);
            this.splitContainer2.Panel2.Controls.Add(this.cbInfiniteExecution);
            this.splitContainer2.Panel2.Controls.Add(this.butClear);
            this.splitContainer2.Panel2.Controls.Add(this.butStart);
            this.splitContainer2.Panel2.Controls.Add(this.lbThreadCount);
            this.splitContainer2.Panel2.Controls.Add(this.butAddNewThread);
            this.splitContainer2.Panel2.Controls.Add(this.lbWattsUpPowerReading);
            this.splitContainer2.Panel2.Controls.Add(this.lbIntelPowerReading);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2MinSize = 300;
            this.splitContainer2.Size = new System.Drawing.Size(1278, 570);
            this.splitContainer2.SplitterDistance = 819;
            this.splitContainer2.TabIndex = 0;
            // 
            // programExecutionPanel
            // 
            this.programExecutionPanel.AutoScroll = true;
            this.programExecutionPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.programExecutionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.programExecutionPanel.Location = new System.Drawing.Point(0, 0);
            this.programExecutionPanel.Name = "programExecutionPanel";
            this.programExecutionPanel.Size = new System.Drawing.Size(819, 570);
            this.programExecutionPanel.TabIndex = 1;
            // 
            // cbK2
            // 
            this.cbK2.FormattingEnabled = true;
            this.cbK2.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "90",
            "100"});
            this.cbK2.Location = new System.Drawing.Point(336, 297);
            this.cbK2.Name = "cbK2";
            this.cbK2.Size = new System.Drawing.Size(65, 28);
            this.cbK2.TabIndex = 15;
            this.cbK2.SelectedIndexChanged += new System.EventHandler(this.cbK2_SelectedIndexChanged);
            // 
            // cbK1
            // 
            this.cbK1.FormattingEnabled = true;
            this.cbK1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbK1.Location = new System.Drawing.Point(265, 297);
            this.cbK1.Name = "cbK1";
            this.cbK1.Size = new System.Drawing.Size(65, 28);
            this.cbK1.TabIndex = 14;
            this.cbK1.SelectedIndexChanged += new System.EventHandler(this.cbK1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Decision Making Criteria (k1, k2): ";
            // 
            // lbNormCorrelation
            // 
            this.lbNormCorrelation.AutoSize = true;
            this.lbNormCorrelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNormCorrelation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbNormCorrelation.Location = new System.Drawing.Point(298, 11);
            this.lbNormCorrelation.Name = "lbNormCorrelation";
            this.lbNormCorrelation.Size = new System.Drawing.Size(66, 30);
            this.lbNormCorrelation.TabIndex = 12;
            this.lbNormCorrelation.Text = "0.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(251, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Graph Normalized Correlation:";
            // 
            // lbTotalExecution
            // 
            this.lbTotalExecution.AutoSize = true;
            this.lbTotalExecution.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbTotalExecution.Location = new System.Drawing.Point(238, 252);
            this.lbTotalExecution.Name = "lbTotalExecution";
            this.lbTotalExecution.Size = new System.Drawing.Size(143, 20);
            this.lbTotalExecution.TabIndex = 10;
            this.lbTotalExecution.Text = "Total Executions: 0";
            // 
            // gbEnergyConsumption
            // 
            this.gbEnergyConsumption.Controls.Add(this.lbTotalTime);
            this.gbEnergyConsumption.Controls.Add(this.lbAvgPower);
            this.gbEnergyConsumption.Controls.Add(this.lbExecJoule);
            this.gbEnergyConsumption.Controls.Add(this.lbWattsHour);
            this.gbEnergyConsumption.Controls.Add(this.lbEnergy);
            this.gbEnergyConsumption.Location = new System.Drawing.Point(20, 335);
            this.gbEnergyConsumption.Name = "gbEnergyConsumption";
            this.gbEnergyConsumption.Size = new System.Drawing.Size(422, 209);
            this.gbEnergyConsumption.TabIndex = 9;
            this.gbEnergyConsumption.TabStop = false;
            this.gbEnergyConsumption.Text = "Power/Energy Consumption (Intel Power Gadget)";
            // 
            // lbTotalTime
            // 
            this.lbTotalTime.AutoSize = true;
            this.lbTotalTime.Location = new System.Drawing.Point(26, 66);
            this.lbTotalTime.Name = "lbTotalTime";
            this.lbTotalTime.Size = new System.Drawing.Size(153, 20);
            this.lbTotalTime.TabIndex = 4;
            this.lbTotalTime.Text = "Runtime: 0 Seconds";
            // 
            // lbAvgPower
            // 
            this.lbAvgPower.AutoSize = true;
            this.lbAvgPower.Location = new System.Drawing.Point(26, 35);
            this.lbAvgPower.Name = "lbAvgPower";
            this.lbAvgPower.Size = new System.Drawing.Size(151, 20);
            this.lbAvgPower.TabIndex = 3;
            this.lbAvgPower.Text = "Avg. Power: 0 Watts";
            // 
            // lbExecJoule
            // 
            this.lbExecJoule.AutoSize = true;
            this.lbExecJoule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExecJoule.Location = new System.Drawing.Point(26, 168);
            this.lbExecJoule.Name = "lbExecJoule";
            this.lbExecJoule.Size = new System.Drawing.Size(262, 20);
            this.lbExecJoule.TabIndex = 2;
            this.lbExecJoule.Text = "Productivity: 0 Executions/Joule";
            // 
            // lbWattsHour
            // 
            this.lbWattsHour.AutoSize = true;
            this.lbWattsHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWattsHour.Location = new System.Drawing.Point(26, 135);
            this.lbWattsHour.Name = "lbWattsHour";
            this.lbWattsHour.Size = new System.Drawing.Size(219, 20);
            this.lbWattsHour.TabIndex = 1;
            this.lbWattsHour.Text = "Energy (WH): 0 Watt-Hour";
            // 
            // lbEnergy
            // 
            this.lbEnergy.AutoSize = true;
            this.lbEnergy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEnergy.Location = new System.Drawing.Point(26, 102);
            this.lbEnergy.Name = "lbEnergy";
            this.lbEnergy.Size = new System.Drawing.Size(168, 20);
            this.lbEnergy.TabIndex = 0;
            this.lbEnergy.Text = "Energy (J): 0 Joules";
            // 
            // cbInfiniteExecution
            // 
            this.cbInfiniteExecution.AutoSize = true;
            this.cbInfiniteExecution.Location = new System.Drawing.Point(241, 192);
            this.cbInfiniteExecution.Name = "cbInfiniteExecution";
            this.cbInfiniteExecution.Size = new System.Drawing.Size(201, 24);
            this.cbInfiniteExecution.TabIndex = 8;
            this.cbInfiniteExecution.Text = "Infinite Execution Mode";
            this.cbInfiniteExecution.UseVisualStyleBackColor = true;
            this.cbInfiniteExecution.CheckedChanged += new System.EventHandler(this.cbInfiniteExecution_CheckedChanged);
            // 
            // butClear
            // 
            this.butClear.Location = new System.Drawing.Point(20, 180);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(78, 47);
            this.butClear.TabIndex = 7;
            this.butClear.Text = "Clear";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // butStart
            // 
            this.butStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butStart.Location = new System.Drawing.Point(20, 233);
            this.butStart.Name = "butStart";
            this.butStart.Size = new System.Drawing.Size(194, 47);
            this.butStart.TabIndex = 6;
            this.butStart.Text = "Start";
            this.butStart.UseVisualStyleBackColor = true;
            this.butStart.Visible = false;
            this.butStart.Click += new System.EventHandler(this.butStart_Click);
            // 
            // lbThreadCount
            // 
            this.lbThreadCount.AutoSize = true;
            this.lbThreadCount.Location = new System.Drawing.Point(237, 140);
            this.lbThreadCount.Name = "lbThreadCount";
            this.lbThreadCount.Size = new System.Drawing.Size(123, 20);
            this.lbThreadCount.TabIndex = 5;
            this.lbThreadCount.Text = "Thread Count: 0";
            // 
            // butAddNewThread
            // 
            this.butAddNewThread.Location = new System.Drawing.Point(20, 127);
            this.butAddNewThread.Name = "butAddNewThread";
            this.butAddNewThread.Size = new System.Drawing.Size(197, 47);
            this.butAddNewThread.TabIndex = 4;
            this.butAddNewThread.Text = "Add New Thread";
            this.butAddNewThread.UseVisualStyleBackColor = true;
            this.butAddNewThread.Click += new System.EventHandler(this.butAddNewThread_Click);
            // 
            // lbWattsUpPowerReading
            // 
            this.lbWattsUpPowerReading.AutoSize = true;
            this.lbWattsUpPowerReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWattsUpPowerReading.Location = new System.Drawing.Point(236, 87);
            this.lbWattsUpPowerReading.Name = "lbWattsUpPowerReading";
            this.lbWattsUpPowerReading.Size = new System.Drawing.Size(85, 29);
            this.lbWattsUpPowerReading.TabIndex = 3;
            this.lbWattsUpPowerReading.Text = "0.00W";
            // 
            // lbIntelPowerReading
            // 
            this.lbIntelPowerReading.AutoSize = true;
            this.lbIntelPowerReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIntelPowerReading.Location = new System.Drawing.Point(236, 47);
            this.lbIntelPowerReading.Name = "lbIntelPowerReading";
            this.lbIntelPowerReading.Size = new System.Drawing.Size(85, 29);
            this.lbIntelPowerReading.TabIndex = 2;
            this.lbIntelPowerReading.Text = "0.00W";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Watts Up? Power Reading:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intel Power Gadget Reading:";
            // 
            // SimulatorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 1028);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimulatorWindow";
            this.Text = "GreenC5 Simulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.gbEnergyConsumption.ResumeLayout(false);
            this.gbEnergyConsumption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GraphLib.PlotterDisplayEx display;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.FlowLayoutPanel programExecutionPanel;
        private System.Windows.Forms.Label lbWattsUpPowerReading;
        private System.Windows.Forms.Label lbIntelPowerReading;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butAddNewThread;
        private System.Windows.Forms.Label lbThreadCount;
        private System.Windows.Forms.Button butClear;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.CheckBox cbInfiniteExecution;
        private System.Windows.Forms.GroupBox gbEnergyConsumption;
        private System.Windows.Forms.Label lbWattsHour;
        private System.Windows.Forms.Label lbEnergy;
        private System.Windows.Forms.Label lbExecJoule;
        private System.Windows.Forms.Label lbTotalTime;
        private System.Windows.Forms.Label lbAvgPower;
        private System.Windows.Forms.Label lbTotalExecution;
        private System.Windows.Forms.Label lbNormCorrelation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbK1;
        private System.Windows.Forms.ComboBox cbK2;
    }
}

