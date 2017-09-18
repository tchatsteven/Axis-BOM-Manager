namespace BOM_Data_Manager
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.eventLogTextBox = new System.Windows.Forms.TextBox();
            this.selectionDataTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.AssembliesListDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.DeleteAssemblyButton = new System.Windows.Forms.Button();
            this.createNewAssemblyButton = new System.Windows.Forms.Button();
            this.EditAssemblyButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.showRootAssembliesCheck = new System.Windows.Forms.CheckBox();
            this.assembliesFilterIsCaseSensitive = new System.Windows.Forms.CheckBox();
            this.assembliesListFilterTextBox = new System.Windows.Forms.TextBox();
            this.PartsListDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.partsListFilterTextBox = new System.Windows.Forms.TextBox();
            this.partsFilterIsCaseSensitive = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.deletePartButton = new System.Windows.Forms.Button();
            this.editPartButton = new System.Windows.Forms.Button();
            this.createNewPartButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.showManageAssembliesButton = new System.Windows.Forms.Button();
            this.showManagePartsButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssembliesListDataGridView)).BeginInit();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartsListDataGridView)).BeginInit();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.45975F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.54025F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1840, 924);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.tableLayoutPanel1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.eventLogTextBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.selectionDataTextBox, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 761);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(8);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1824, 155);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event Log";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(915, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current Selection Info";
            // 
            // eventLogTextBox
            // 
            this.eventLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventLogTextBox.Location = new System.Drawing.Point(5, 18);
            this.eventLogTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.eventLogTextBox.Multiline = true;
            this.eventLogTextBox.Name = "eventLogTextBox";
            this.eventLogTextBox.Size = new System.Drawing.Size(902, 132);
            this.eventLogTextBox.TabIndex = 2;
            // 
            // selectionDataTextBox
            // 
            this.selectionDataTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectionDataTextBox.Location = new System.Drawing.Point(917, 18);
            this.selectionDataTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.selectionDataTextBox.Multiline = true;
            this.selectionDataTextBox.Name = "selectionDataTextBox";
            this.selectionDataTextBox.Size = new System.Drawing.Size(902, 132);
            this.selectionDataTextBox.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.SteelBlue;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1828, 74);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::BOM_Data_Manager.Properties.Resources.ordering_code_manager_logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1818, 64);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1170F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 89);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1828, 661);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.SteelBlue;
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(658, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1167, 661);
            this.tableLayoutPanel7.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(1167, 34);
            this.label5.TabIndex = 0;
            this.label5.Text = "Assemble Assemblies";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.SteelBlue;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(203, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(452, 661);
            this.tableLayoutPanel5.TabIndex = 0;
            this.tableLayoutPanel5.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel5_Paint);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(452, 34);
            this.label3.TabIndex = 0;
            this.label3.Text = "Create/Edit Parts and Assemblies";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.AssembliesListDataGridView, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel13, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel12, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.PartsListDataGridView, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 37);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.78767F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.21233F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(446, 621);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // AssembliesListDataGridView
            // 
            this.AssembliesListDataGridView.AllowUserToResizeColumns = false;
            this.AssembliesListDataGridView.AllowUserToResizeRows = false;
            this.AssembliesListDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssembliesListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.AssembliesListDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.AssembliesListDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AssembliesListDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.AssembliesListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssembliesListDataGridView.EnableHeadersVisualStyles = false;
            this.AssembliesListDataGridView.Location = new System.Drawing.Point(226, 102);
            this.AssembliesListDataGridView.Name = "AssembliesListDataGridView";
            this.AssembliesListDataGridView.RowHeadersVisible = false;
            this.AssembliesListDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AssembliesListDataGridView.Size = new System.Drawing.Size(217, 516);
            this.AssembliesListDataGridView.TabIndex = 2;
            this.AssembliesListDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssembliesListDataGridView_CellContentClick);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.AutoSize = true;
            this.tableLayoutPanel13.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel13.ColumnCount = 3;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.Controls.Add(this.DeleteAssemblyButton, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.createNewAssemblyButton, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.EditAssemblyButton, 1, 0);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(226, 3);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(217, 31);
            this.tableLayoutPanel13.TabIndex = 3;
            // 
            // DeleteAssemblyButton
            // 
            this.DeleteAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteAssemblyButton.AutoSize = true;
            this.DeleteAssemblyButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DeleteAssemblyButton.BackColor = System.Drawing.Color.Gainsboro;
            this.DeleteAssemblyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteAssemblyButton.Location = new System.Drawing.Point(196, 3);
            this.DeleteAssemblyButton.Name = "DeleteAssemblyButton";
            this.DeleteAssemblyButton.Size = new System.Drawing.Size(102, 25);
            this.DeleteAssemblyButton.TabIndex = 5;
            this.DeleteAssemblyButton.Text = "Delete Assembly";
            this.DeleteAssemblyButton.UseVisualStyleBackColor = false;
            this.DeleteAssemblyButton.Click += new System.EventHandler(this.DeleteAssemblyButton_Click);
            // 
            // createNewAssemblyButton
            // 
            this.createNewAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createNewAssemblyButton.AutoSize = true;
            this.createNewAssemblyButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.createNewAssemblyButton.BackColor = System.Drawing.Color.Gainsboro;
            this.createNewAssemblyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNewAssemblyButton.Location = new System.Drawing.Point(3, 3);
            this.createNewAssemblyButton.Name = "createNewAssemblyButton";
            this.createNewAssemblyButton.Size = new System.Drawing.Size(92, 25);
            this.createNewAssemblyButton.TabIndex = 3;
            this.createNewAssemblyButton.Text = "New Assembly";
            this.createNewAssemblyButton.UseVisualStyleBackColor = false;
            this.createNewAssemblyButton.Click += new System.EventHandler(this.createNewAssemblyButton_Click);
            // 
            // EditAssemblyButton
            // 
            this.EditAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditAssemblyButton.AutoSize = true;
            this.EditAssemblyButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EditAssemblyButton.BackColor = System.Drawing.Color.Gainsboro;
            this.EditAssemblyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditAssemblyButton.Location = new System.Drawing.Point(101, 3);
            this.EditAssemblyButton.Name = "EditAssemblyButton";
            this.EditAssemblyButton.Size = new System.Drawing.Size(89, 25);
            this.EditAssemblyButton.TabIndex = 4;
            this.EditAssemblyButton.Text = "Edit Assembly";
            this.EditAssemblyButton.UseVisualStyleBackColor = false;
            this.EditAssemblyButton.Click += new System.EventHandler(this.EditAssemblyButton_Click);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.tableLayoutPanel8, 1, 1);
            this.tableLayoutPanel12.Controls.Add(this.assembliesListFilterTextBox, 1, 0);
            this.tableLayoutPanel12.Location = new System.Drawing.Point(226, 40);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(217, 56);
            this.tableLayoutPanel12.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(145, 28);
            this.label8.TabIndex = 0;
            this.label8.Text = "Assemblies Table Filtering: ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.74112F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.25888F));
            this.tableLayoutPanel8.Controls.Add(this.showRootAssembliesCheck, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.assembliesFilterIsCaseSensitive, 0, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(151, 28);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(66, 28);
            this.tableLayoutPanel8.TabIndex = 2;
            // 
            // showRootAssembliesCheck
            // 
            this.showRootAssembliesCheck.AutoSize = true;
            this.showRootAssembliesCheck.ForeColor = System.Drawing.Color.White;
            this.showRootAssembliesCheck.Location = new System.Drawing.Point(24, 3);
            this.showRootAssembliesCheck.Name = "showRootAssembliesCheck";
            this.showRootAssembliesCheck.Size = new System.Drawing.Size(39, 17);
            this.showRootAssembliesCheck.TabIndex = 3;
            this.showRootAssembliesCheck.Text = "Show Root Assemblies";
            this.showRootAssembliesCheck.UseVisualStyleBackColor = true;
            this.showRootAssembliesCheck.CheckedChanged += new System.EventHandler(this.showRootAssembliesCheck_CheckedChanged);
            // 
            // assembliesFilterIsCaseSensitive
            // 
            this.assembliesFilterIsCaseSensitive.AutoSize = true;
            this.assembliesFilterIsCaseSensitive.ForeColor = System.Drawing.Color.White;
            this.assembliesFilterIsCaseSensitive.Location = new System.Drawing.Point(3, 3);
            this.assembliesFilterIsCaseSensitive.Name = "assembliesFilterIsCaseSensitive";
            this.assembliesFilterIsCaseSensitive.Size = new System.Drawing.Size(15, 17);
            this.assembliesFilterIsCaseSensitive.TabIndex = 2;
            this.assembliesFilterIsCaseSensitive.Text = "Case Sensitive";
            this.assembliesFilterIsCaseSensitive.UseVisualStyleBackColor = true;
            this.assembliesFilterIsCaseSensitive.CheckedChanged += new System.EventHandler(this.assembliesFilterIsCaseSensitive_CheckedChanged);
            // 
            // assembliesListFilterTextBox
            // 
            this.assembliesListFilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.assembliesListFilterTextBox.Location = new System.Drawing.Point(154, 3);
            this.assembliesListFilterTextBox.Name = "assembliesListFilterTextBox";
            this.assembliesListFilterTextBox.Size = new System.Drawing.Size(60, 22);
            this.assembliesListFilterTextBox.TabIndex = 1;
            this.assembliesListFilterTextBox.TextChanged += new System.EventHandler(this.assembliesListFilterTextBox_TextChanged);
            // 
            // PartsListDataGridView
            // 
            this.PartsListDataGridView.AllowUserToResizeColumns = false;
            this.PartsListDataGridView.AllowUserToResizeRows = false;
            this.PartsListDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartsListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.PartsListDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.PartsListDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PartsListDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.PartsListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PartsListDataGridView.EnableHeadersVisualStyles = false;
            this.PartsListDataGridView.Location = new System.Drawing.Point(3, 102);
            this.PartsListDataGridView.Name = "PartsListDataGridView";
            this.PartsListDataGridView.RowHeadersVisible = false;
            this.PartsListDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PartsListDataGridView.Size = new System.Drawing.Size(217, 516);
            this.PartsListDataGridView.TabIndex = 0;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.partsListFilterTextBox, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.partsFilterIsCaseSensitive, 1, 1);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 40);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(217, 56);
            this.tableLayoutPanel10.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 28);
            this.label7.TabIndex = 0;
            this.label7.Text = "Parts Table Filtering: ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // partsListFilterTextBox
            // 
            this.partsListFilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.partsListFilterTextBox.Location = new System.Drawing.Point(123, 3);
            this.partsListFilterTextBox.Name = "partsListFilterTextBox";
            this.partsListFilterTextBox.Size = new System.Drawing.Size(91, 22);
            this.partsListFilterTextBox.TabIndex = 1;
            this.partsListFilterTextBox.TextChanged += new System.EventHandler(this.partsListFilterTextBox_TextChanged);
            // 
            // partsFilterIsCaseSensitive
            // 
            this.partsFilterIsCaseSensitive.AutoSize = true;
            this.partsFilterIsCaseSensitive.ForeColor = System.Drawing.Color.White;
            this.partsFilterIsCaseSensitive.Location = new System.Drawing.Point(123, 31);
            this.partsFilterIsCaseSensitive.Name = "partsFilterIsCaseSensitive";
            this.partsFilterIsCaseSensitive.Size = new System.Drawing.Size(91, 17);
            this.partsFilterIsCaseSensitive.TabIndex = 2;
            this.partsFilterIsCaseSensitive.Text = "Case Sensitive";
            this.partsFilterIsCaseSensitive.UseVisualStyleBackColor = true;
            this.partsFilterIsCaseSensitive.CheckedChanged += new System.EventHandler(this.partsFilterIsCaseSensitive_CheckedChanged);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.AutoSize = true;
            this.tableLayoutPanel11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.Controls.Add(this.deletePartButton, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.editPartButton, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.createNewPartButton, 0, 0);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.Size = new System.Drawing.Size(217, 31);
            this.tableLayoutPanel11.TabIndex = 2;
            // 
            // deletePartButton
            // 
            this.deletePartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deletePartButton.AutoSize = true;
            this.deletePartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deletePartButton.BackColor = System.Drawing.Color.Gainsboro;
            this.deletePartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deletePartButton.Location = new System.Drawing.Point(142, 3);
            this.deletePartButton.Name = "deletePartButton";
            this.deletePartButton.Size = new System.Drawing.Size(75, 25);
            this.deletePartButton.TabIndex = 5;
            this.deletePartButton.Text = "Delete Part";
            this.deletePartButton.UseVisualStyleBackColor = false;
            this.deletePartButton.Click += new System.EventHandler(this.deletePartButton_Click);
            // 
            // editPartButton
            // 
            this.editPartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editPartButton.AutoSize = true;
            this.editPartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editPartButton.BackColor = System.Drawing.Color.Gainsboro;
            this.editPartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editPartButton.Location = new System.Drawing.Point(74, 3);
            this.editPartButton.Name = "editPartButton";
            this.editPartButton.Size = new System.Drawing.Size(62, 25);
            this.editPartButton.TabIndex = 4;
            this.editPartButton.Text = "Edit Part";
            this.editPartButton.UseVisualStyleBackColor = false;
            this.editPartButton.Click += new System.EventHandler(this.editPartButton_Click);
            // 
            // createNewPartButton
            // 
            this.createNewPartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createNewPartButton.AutoSize = true;
            this.createNewPartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.createNewPartButton.BackColor = System.Drawing.Color.Gainsboro;
            this.createNewPartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createNewPartButton.Location = new System.Drawing.Point(3, 3);
            this.createNewPartButton.Name = "createNewPartButton";
            this.createNewPartButton.Size = new System.Drawing.Size(65, 25);
            this.createNewPartButton.TabIndex = 3;
            this.createNewPartButton.Text = "New Part";
            this.createNewPartButton.UseVisualStyleBackColor = false;
            this.createNewPartButton.Click += new System.EventHandler(this.createNewPartButton_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.SteelBlue;
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.showManageAssembliesButton, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.showManagePartsButton, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(200, 661);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // showManageAssembliesButton
            // 
            this.showManageAssembliesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showManageAssembliesButton.BackColor = System.Drawing.Color.Gainsboro;
            this.showManageAssembliesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showManageAssembliesButton.Location = new System.Drawing.Point(3, 67);
            this.showManageAssembliesButton.Name = "showManageAssembliesButton";
            this.showManageAssembliesButton.Size = new System.Drawing.Size(194, 24);
            this.showManageAssembliesButton.TabIndex = 3;
            this.showManageAssembliesButton.Text = "Assemble Assemblies";
            this.showManageAssembliesButton.UseVisualStyleBackColor = false;
            this.showManageAssembliesButton.Click += new System.EventHandler(this.showManageAssembliesButton_Click);
            // 
            // showManagePartsButton
            // 
            this.showManagePartsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showManagePartsButton.BackColor = System.Drawing.Color.Gainsboro;
            this.showManagePartsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showManagePartsButton.Location = new System.Drawing.Point(3, 37);
            this.showManagePartsButton.Name = "showManagePartsButton";
            this.showManagePartsButton.Size = new System.Drawing.Size(194, 24);
            this.showManagePartsButton.TabIndex = 0;
            this.showManagePartsButton.Text = "Create/Edit Parts and Assemblies";
            this.showManagePartsButton.UseVisualStyleBackColor = false;
            this.showManagePartsButton.Click += new System.EventHandler(this.showManagePartsButton_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(200, 34);
            this.label4.TabIndex = 2;
            this.label4.Text = "Main Menu";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1840, 924);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AXIS BOM Data Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssembliesListDataGridView)).EndInit();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartsListDataGridView)).EndInit();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox eventLogTextBox;
        private System.Windows.Forms.TextBox selectionDataTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button showManageAssembliesButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button showManagePartsButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.DataGridView PartsListDataGridView;
        private System.Windows.Forms.TextBox partsListFilterTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button createNewPartButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Button deletePartButton;
        private System.Windows.Forms.Button editPartButton;
        private System.Windows.Forms.CheckBox partsFilterIsCaseSensitive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox assembliesListFilterTextBox;
        private System.Windows.Forms.CheckBox assembliesFilterIsCaseSensitive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Button DeleteAssemblyButton;
        private System.Windows.Forms.Button createNewAssemblyButton;
        private System.Windows.Forms.Button EditAssemblyButton;
        private System.Windows.Forms.DataGridView AssembliesListDataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.CheckBox showRootAssembliesCheck;
    }
}

