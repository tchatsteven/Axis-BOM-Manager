namespace BeamCartridgePicker
{
    partial class CartridgePickerTesterForm
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
            this.RequestedLengthTextBox = new System.Windows.Forms.TextBox();
            this.Solve = new System.Windows.Forms.Button();
            this.MainTextArea = new System.Windows.Forms.RichTextBox();
            this.SplitGapCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.TextInExcelButton = new System.Windows.Forms.Button();
            this.TestProgressBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ApplicationLog = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RequestedLengthTextBox
            // 
            this.RequestedLengthTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RequestedLengthTextBox.Location = new System.Drawing.Point(232, 3);
            this.RequestedLengthTextBox.Name = "RequestedLengthTextBox";
            this.RequestedLengthTextBox.Size = new System.Drawing.Size(116, 20);
            this.RequestedLengthTextBox.TabIndex = 0;
            this.RequestedLengthTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RequestedLengthTextBox_KeyDown);
            // 
            // Solve
            // 
            this.Solve.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Solve.FlatAppearance.BorderSize = 0;
            this.Solve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Solve.ForeColor = System.Drawing.Color.White;
            this.Solve.Location = new System.Drawing.Point(3, 26);
            this.Solve.Name = "Solve";
            this.Solve.Size = new System.Drawing.Size(160, 23);
            this.Solve.TabIndex = 1;
            this.Solve.Text = "Show Best Solution";
            this.Solve.UseVisualStyleBackColor = false;
            this.Solve.Click += new System.EventHandler(this.Solve_Click);
            // 
            // MainTextArea
            // 
            this.MainTextArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTextArea.Location = new System.Drawing.Point(10, 240);
            this.MainTextArea.Margin = new System.Windows.Forms.Padding(10);
            this.MainTextArea.Name = "MainTextArea";
            this.MainTextArea.Size = new System.Drawing.Size(1325, 302);
            this.MainTextArea.TabIndex = 3;
            this.MainTextArea.Text = "";
            // 
            // SplitGapCheckBox
            // 
            this.SplitGapCheckBox.AutoSize = true;
            this.SplitGapCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SplitGapCheckBox.ForeColor = System.Drawing.Color.White;
            this.SplitGapCheckBox.Location = new System.Drawing.Point(3, 3);
            this.SplitGapCheckBox.Name = "SplitGapCheckBox";
            this.SplitGapCheckBox.Size = new System.Drawing.Size(140, 17);
            this.SplitGapCheckBox.TabIndex = 4;
            this.SplitGapCheckBox.Text = "Divide gap to both sides";
            this.SplitGapCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(3, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Show All For Comparison";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TextInExcelButton
            // 
            this.TextInExcelButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.TextInExcelButton.Enabled = false;
            this.TextInExcelButton.FlatAppearance.BorderSize = 0;
            this.TextInExcelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TextInExcelButton.ForeColor = System.Drawing.Color.White;
            this.TextInExcelButton.Location = new System.Drawing.Point(3, 84);
            this.TextInExcelButton.Name = "TextInExcelButton";
            this.TextInExcelButton.Size = new System.Drawing.Size(160, 23);
            this.TextInExcelButton.TabIndex = 6;
            this.TextInExcelButton.Text = "Test In Excel";
            this.TextInExcelButton.UseVisualStyleBackColor = false;
            this.TextInExcelButton.Click += new System.EventHandler(this.TextInExcelButton_Click);
            // 
            // TestProgressBar
            // 
            this.TestProgressBar.Location = new System.Drawing.Point(169, 84);
            this.TestProgressBar.Name = "TestProgressBar";
            this.TestProgressBar.Size = new System.Drawing.Size(195, 23);
            this.TestProgressBar.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MainTextArea, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ApplicationLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1345, 702);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // ApplicationLog
            // 
            this.ApplicationLog.AcceptsTab = true;
            this.ApplicationLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplicationLog.BackColor = System.Drawing.Color.Black;
            this.ApplicationLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ApplicationLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationLog.ForeColor = System.Drawing.Color.White;
            this.ApplicationLog.Location = new System.Drawing.Point(10, 562);
            this.ApplicationLog.Margin = new System.Windows.Forms.Padding(10);
            this.ApplicationLog.Name = "ApplicationLog";
            this.ApplicationLog.Size = new System.Drawing.Size(1325, 130);
            this.ApplicationLog.TabIndex = 9;
            this.ApplicationLog.Text = "";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.15152F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.84848F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.RequestedLengthTextBox, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 83);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(654, 144);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 26);
            this.label4.TabIndex = 10;
            this.label4.Text = "Input Test Length (In Inches)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.Solve, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.button1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.SplitGapCheckBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.TextInExcelButton, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.TestProgressBar, 1, 3);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(232, 29);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(367, 110);
            this.tableLayoutPanel3.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::CartridgeSolver.Properties.Resources.Cartridge_Solver;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // CartridgePickerTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(65)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1345, 702);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CartridgePickerTesterForm";
            this.Text = "Cartridge Picker Tester";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox RequestedLengthTextBox;
        private System.Windows.Forms.Button Solve;
        private System.Windows.Forms.RichTextBox MainTextArea;
        private System.Windows.Forms.CheckBox SplitGapCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button TextInExcelButton;
        private System.Windows.Forms.ProgressBar TestProgressBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox ApplicationLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

