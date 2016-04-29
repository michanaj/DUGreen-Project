namespace GreenC5Simulator
{
    partial class ThreadUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbThreadName = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.butAddNewGreenC5 = new System.Windows.Forms.Button();
            this.greenC5FlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lbCount = new System.Windows.Forms.Label();
            this.lbTotalExecution = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbThreadName
            // 
            this.lbThreadName.AutoSize = true;
            this.lbThreadName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbThreadName.Location = new System.Drawing.Point(12, 11);
            this.lbThreadName.Name = "lbThreadName";
            this.lbThreadName.Size = new System.Drawing.Size(33, 20);
            this.lbThreadName.TabIndex = 0;
            this.lbThreadName.Text = "ID:";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(220, 11);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(90, 20);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "Status: Idle";
            // 
            // butAddNewGreenC5
            // 
            this.butAddNewGreenC5.Location = new System.Drawing.Point(16, 43);
            this.butAddNewGreenC5.Name = "butAddNewGreenC5";
            this.butAddNewGreenC5.Size = new System.Drawing.Size(167, 31);
            this.butAddNewGreenC5.TabIndex = 2;
            this.butAddNewGreenC5.Text = "Add New GreenC5";
            this.butAddNewGreenC5.UseVisualStyleBackColor = true;
            this.butAddNewGreenC5.Click += new System.EventHandler(this.butAddNewGreenC5_Click);
            // 
            // greenC5FlowPanel
            // 
            this.greenC5FlowPanel.AutoScroll = true;
            this.greenC5FlowPanel.Location = new System.Drawing.Point(16, 81);
            this.greenC5FlowPanel.Name = "greenC5FlowPanel";
            this.greenC5FlowPanel.Size = new System.Drawing.Size(689, 403);
            this.greenC5FlowPanel.TabIndex = 4;
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Location = new System.Drawing.Point(189, 48);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(138, 20);
            this.lbCount.TabIndex = 5;
            this.lbCount.Text = "GreenC5 Count: 0";
            // 
            // lbTotalExecution
            // 
            this.lbTotalExecution.AutoSize = true;
            this.lbTotalExecution.Location = new System.Drawing.Point(465, 11);
            this.lbTotalExecution.Name = "lbTotalExecution";
            this.lbTotalExecution.Size = new System.Drawing.Size(224, 20);
            this.lbTotalExecution.TabIndex = 6;
            this.lbTotalExecution.Text = "Total Completed Executions: 0";
            // 
            // ThreadUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbTotalExecution);
            this.Controls.Add(this.lbCount);
            this.Controls.Add(this.greenC5FlowPanel);
            this.Controls.Add(this.butAddNewGreenC5);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbThreadName);
            this.Name = "ThreadUserControl";
            this.Size = new System.Drawing.Size(723, 499);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbThreadName;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button butAddNewGreenC5;
        private System.Windows.Forms.FlowLayoutPanel greenC5FlowPanel;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Label lbTotalExecution;
    }
}
