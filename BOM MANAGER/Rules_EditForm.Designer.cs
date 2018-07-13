namespace BOM_MANAGER
{
    partial class Rules_EditForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ParameterName_comboBox = new System.Windows.Forms.ComboBox();
            this.CategoryName_comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Ok_EditRulesButton = new System.Windows.Forms.Button();
            this.Cancel_EditRulesButton = new System.Windows.Forms.Button();
            this.FilterRuleEdit_comboBox = new System.Windows.Forms.ComboBox();
            this.QtyEdit_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PartName_textBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.PACAF_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ProductCode_textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QtyEdit_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(509, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(503, 245);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(503, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = "Edit Rules";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(503, 209);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.ParameterName_comboBox, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.CategoryName_comboBox, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label8, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.Ok_EditRulesButton, 2, 4);
            this.tableLayoutPanel4.Controls.Add(this.Cancel_EditRulesButton, 3, 4);
            this.tableLayoutPanel4.Controls.Add(this.FilterRuleEdit_comboBox, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.QtyEdit_numericUpDown, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.PartName_textBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.PACAF_textBox, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.ProductCode_textBox, 1, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(497, 161);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // ParameterName_comboBox
            // 
            this.ParameterName_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ParameterName_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ParameterName_comboBox.FormattingEnabled = true;
            this.ParameterName_comboBox.Location = new System.Drawing.Point(350, 37);
            this.ParameterName_comboBox.Name = "ParameterName_comboBox";
            this.ParameterName_comboBox.Size = new System.Drawing.Size(144, 21);
            this.ParameterName_comboBox.TabIndex = 21;
            this.ParameterName_comboBox.SelectedIndexChanged += new System.EventHandler(this.ParameterName_comboBox_SelectedIndexChanged);
            // 
            // CategoryName_comboBox
            // 
            this.CategoryName_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CategoryName_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryName_comboBox.FormattingEnabled = true;
            this.CategoryName_comboBox.Location = new System.Drawing.Point(350, 5);
            this.CategoryName_comboBox.Name = "CategoryName_comboBox";
            this.CategoryName_comboBox.Size = new System.Drawing.Size(144, 21);
            this.CategoryName_comboBox.TabIndex = 20;
            this.CategoryName_comboBox.SelectionChangeCommitted += new System.EventHandler(this.CategoryName_comboBox_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(253, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "Category Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(253, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 22);
            this.label5.TabIndex = 4;
            this.label5.Text = "Parameter Name:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Location = new System.Drawing.Point(5, 101);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 22);
            this.label6.TabIndex = 5;
            this.label6.Text = "Quantity:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label8.Location = new System.Drawing.Point(253, 69);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 22);
            this.label8.TabIndex = 7;
            this.label8.Text = "Filter Rule:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ok_EditRulesButton
            // 
            this.Ok_EditRulesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok_EditRulesButton.Location = new System.Drawing.Point(251, 131);
            this.Ok_EditRulesButton.Name = "Ok_EditRulesButton";
            this.Ok_EditRulesButton.Size = new System.Drawing.Size(93, 27);
            this.Ok_EditRulesButton.TabIndex = 12;
            this.Ok_EditRulesButton.Text = "OK";
            this.Ok_EditRulesButton.UseVisualStyleBackColor = true;
            this.Ok_EditRulesButton.Click += new System.EventHandler(this.Ok_EditRulesButton_Click);
            // 
            // Cancel_EditRulesButton
            // 
            this.Cancel_EditRulesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_EditRulesButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_EditRulesButton.Location = new System.Drawing.Point(350, 131);
            this.Cancel_EditRulesButton.Name = "Cancel_EditRulesButton";
            this.Cancel_EditRulesButton.Size = new System.Drawing.Size(144, 27);
            this.Cancel_EditRulesButton.TabIndex = 13;
            this.Cancel_EditRulesButton.Text = "Cancel";
            this.Cancel_EditRulesButton.UseVisualStyleBackColor = true;
            // 
            // FilterRuleEdit_comboBox
            // 
            this.FilterRuleEdit_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterRuleEdit_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterRuleEdit_comboBox.FormattingEnabled = true;
            this.FilterRuleEdit_comboBox.Location = new System.Drawing.Point(350, 69);
            this.FilterRuleEdit_comboBox.Name = "FilterRuleEdit_comboBox";
            this.FilterRuleEdit_comboBox.Size = new System.Drawing.Size(144, 21);
            this.FilterRuleEdit_comboBox.TabIndex = 14;
            // 
            // QtyEdit_numericUpDown
            // 
            this.QtyEdit_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.QtyEdit_numericUpDown.Location = new System.Drawing.Point(102, 102);
            this.QtyEdit_numericUpDown.Name = "QtyEdit_numericUpDown";
            this.QtyEdit_numericUpDown.Size = new System.Drawing.Size(143, 20);
            this.QtyEdit_numericUpDown.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "Part Name: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PartName_textBox
            // 
            this.PartName_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PartName_textBox.Enabled = false;
            this.PartName_textBox.Location = new System.Drawing.Point(102, 6);
            this.PartName_textBox.Name = "PartName_textBox";
            this.PartName_textBox.ReadOnly = true;
            this.PartName_textBox.Size = new System.Drawing.Size(143, 20);
            this.PartName_textBox.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(5, 69);
            this.label11.Margin = new System.Windows.Forms.Padding(5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 22);
            this.label11.TabIndex = 10;
            this.label11.Text = "PACAF Id:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PACAF_textBox
            // 
            this.PACAF_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PACAF_textBox.Enabled = false;
            this.PACAF_textBox.Location = new System.Drawing.Point(102, 70);
            this.PACAF_textBox.Name = "PACAF_textBox";
            this.PACAF_textBox.ReadOnly = true;
            this.PACAF_textBox.Size = new System.Drawing.Size(143, 20);
            this.PACAF_textBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(5, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "Product Code:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ProductCode_textBox
            // 
            this.ProductCode_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProductCode_textBox.Enabled = false;
            this.ProductCode_textBox.Location = new System.Drawing.Point(102, 38);
            this.ProductCode_textBox.Name = "ProductCode_textBox";
            this.ProductCode_textBox.Size = new System.Drawing.Size(143, 20);
            this.ProductCode_textBox.TabIndex = 11;
            // 
            // Rules_EditForm
            // 
            this.AcceptButton = this.Ok_EditRulesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.Cancel_EditRulesButton;
            this.ClientSize = new System.Drawing.Size(509, 251);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Rules_EditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rules Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QtyEdit_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ProductCode_textBox;
        private System.Windows.Forms.Button Ok_EditRulesButton;
        private System.Windows.Forms.Button Cancel_EditRulesButton;
        private System.Windows.Forms.TextBox PACAF_textBox;
        private System.Windows.Forms.ComboBox FilterRuleEdit_comboBox;
        private System.Windows.Forms.NumericUpDown QtyEdit_numericUpDown;
        private System.Windows.Forms.ComboBox ParameterName_comboBox;
        private System.Windows.Forms.ComboBox CategoryName_comboBox;
        private System.Windows.Forms.TextBox PartName_textBox;
    }
}