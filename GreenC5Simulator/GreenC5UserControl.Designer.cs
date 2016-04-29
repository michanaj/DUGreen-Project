namespace GreenC5Simulator
{
    partial class GreenC5UserControl
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
            this.lbName = new System.Windows.Forms.Label();
            this.cbStartDataStructure = new System.Windows.Forms.ComboBox();
            this.cbPrograms = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDataStructureGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCurrentDataStructure = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbTransCount = new System.Windows.Forms.Label();
            this.lbNumExecution = new System.Windows.Forms.Label();
            this.lbPrediction = new System.Windows.Forms.Label();
            this.cbRunMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(4, 4);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(33, 20);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "ID:";
            // 
            // cbStartDataStructure
            // 
            this.cbStartDataStructure.FormattingEnabled = true;
            this.cbStartDataStructure.Location = new System.Drawing.Point(81, 120);
            this.cbStartDataStructure.Name = "cbStartDataStructure";
            this.cbStartDataStructure.Size = new System.Drawing.Size(221, 28);
            this.cbStartDataStructure.TabIndex = 2;
            this.cbStartDataStructure.SelectedIndexChanged += new System.EventHandler(this.cbStartDataStructure_SelectedIndexChanged);
            // 
            // cbPrograms
            // 
            this.cbPrograms.FormattingEnabled = true;
            this.cbPrograms.Location = new System.Drawing.Point(6, 178);
            this.cbPrograms.Name = "cbPrograms";
            this.cbPrograms.Size = new System.Drawing.Size(298, 28);
            this.cbPrograms.TabIndex = 4;
            this.cbPrograms.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Program:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbRunMode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbDataStructureGroup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbPrograms);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbStartDataStructure);
            this.groupBox1.Location = new System.Drawing.Point(8, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 216);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Start DS:";
            // 
            // cbDataStructureGroup
            // 
            this.cbDataStructureGroup.FormattingEnabled = true;
            this.cbDataStructureGroup.Location = new System.Drawing.Point(6, 42);
            this.cbDataStructureGroup.Name = "cbDataStructureGroup";
            this.cbDataStructureGroup.Size = new System.Drawing.Size(298, 28);
            this.cbDataStructureGroup.TabIndex = 6;
            this.cbDataStructureGroup.SelectedIndexChanged += new System.EventHandler(this.cbDataStructureGroup_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Data Structure Group and Worst DS:";
            // 
            // lbCurrentDataStructure
            // 
            this.lbCurrentDataStructure.AutoSize = true;
            this.lbCurrentDataStructure.Location = new System.Drawing.Point(8, 63);
            this.lbCurrentDataStructure.Name = "lbCurrentDataStructure";
            this.lbCurrentDataStructure.Size = new System.Drawing.Size(146, 20);
            this.lbCurrentDataStructure.TabIndex = 8;
            this.lbCurrentDataStructure.Text = "Current : LinkedList";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(8, 34);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(102, 20);
            this.lbStatus.TabIndex = 7;
            this.lbStatus.Text = "Status: Idle";
            // 
            // lbTransCount
            // 
            this.lbTransCount.AutoSize = true;
            this.lbTransCount.Location = new System.Drawing.Point(8, 129);
            this.lbTransCount.Name = "lbTransCount";
            this.lbTransCount.Size = new System.Drawing.Size(145, 20);
            this.lbTransCount.TabIndex = 9;
            this.lbTransCount.Text = "Transform Count: 0";
            // 
            // lbNumExecution
            // 
            this.lbNumExecution.AutoSize = true;
            this.lbNumExecution.Location = new System.Drawing.Point(167, 34);
            this.lbNumExecution.Name = "lbNumExecution";
            this.lbNumExecution.Size = new System.Drawing.Size(74, 20);
            this.lbNumExecution.TabIndex = 10;
            this.lbNumExecution.Text = "# Exec: 0";
            // 
            // lbPrediction
            // 
            this.lbPrediction.AutoSize = true;
            this.lbPrediction.Location = new System.Drawing.Point(8, 93);
            this.lbPrediction.Name = "lbPrediction";
            this.lbPrediction.Size = new System.Drawing.Size(111, 20);
            this.lbPrediction.TabIndex = 11;
            this.lbPrediction.Text = "Prediction: N/a";
            // 
            // cbRunMode
            // 
            this.cbRunMode.FormattingEnabled = true;
            this.cbRunMode.Location = new System.Drawing.Point(112, 79);
            this.cbRunMode.Name = "cbRunMode";
            this.cbRunMode.Size = new System.Drawing.Size(190, 28);
            this.cbRunMode.TabIndex = 9;
            this.cbRunMode.SelectedIndexChanged += new System.EventHandler(this.cbRunMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Run Mode:";
            // 
            // GreenC5UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbPrediction);
            this.Controls.Add(this.lbNumExecution);
            this.Controls.Add(this.lbTransCount);
            this.Controls.Add(this.lbCurrentDataStructure);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbName);
            this.Name = "GreenC5UserControl";
            this.Size = new System.Drawing.Size(329, 377);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ComboBox cbStartDataStructure;
        private System.Windows.Forms.ComboBox cbPrograms;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbDataStructureGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTransCount;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbCurrentDataStructure;
        private System.Windows.Forms.Label lbNumExecution;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbPrediction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbRunMode;
    }
}
