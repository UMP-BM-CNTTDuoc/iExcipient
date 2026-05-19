namespace iExcipient_Form.Forms.Danhmuc
{
    partial class DanhMucThanhPhanCosing
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
            this.buttonXoa = new System.Windows.Forms.Button();
            this.buttonThem = new System.Windows.Forms.Button();
            this.buttonSua = new System.Windows.Forms.Button();
            this.buttonThoat = new System.Windows.Forms.Button();
            this.buttonXoatrang = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxTen_INCI = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIDThanhphan_Cosing = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEC_No = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxCAS_No = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTongSo = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonXoa
            // 
            this.buttonXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXoa.Location = new System.Drawing.Point(610, 9);
            this.buttonXoa.Name = "buttonXoa";
            this.buttonXoa.Size = new System.Drawing.Size(130, 43);
            this.buttonXoa.TabIndex = 13;
            this.buttonXoa.Text = "Xóa";
            this.buttonXoa.UseVisualStyleBackColor = true;
            this.buttonXoa.Click += new System.EventHandler(this.buttonXoa_Click);
            // 
            // buttonThem
            // 
            this.buttonThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonThem.Location = new System.Drawing.Point(462, 9);
            this.buttonThem.Name = "buttonThem";
            this.buttonThem.Size = new System.Drawing.Size(130, 43);
            this.buttonThem.TabIndex = 12;
            this.buttonThem.Text = "Thêm";
            this.buttonThem.UseVisualStyleBackColor = true;
            this.buttonThem.Click += new System.EventHandler(this.buttonThem_Click);
            // 
            // buttonSua
            // 
            this.buttonSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSua.Location = new System.Drawing.Point(758, 9);
            this.buttonSua.Name = "buttonSua";
            this.buttonSua.Size = new System.Drawing.Size(130, 43);
            this.buttonSua.TabIndex = 14;
            this.buttonSua.Text = "Sửa";
            this.buttonSua.UseVisualStyleBackColor = true;
            this.buttonSua.Click += new System.EventHandler(this.buttonSua_Click);
            // 
            // buttonThoat
            // 
            this.buttonThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonThoat.Location = new System.Drawing.Point(906, 9);
            this.buttonThoat.Name = "buttonThoat";
            this.buttonThoat.Size = new System.Drawing.Size(130, 43);
            this.buttonThoat.TabIndex = 15;
            this.buttonThoat.Text = "Thoát";
            this.buttonThoat.UseVisualStyleBackColor = true;
            this.buttonThoat.Click += new System.EventHandler(this.buttonThoat_Click);
            // 
            // buttonXoatrang
            // 
            this.buttonXoatrang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXoatrang.Location = new System.Drawing.Point(314, 9);
            this.buttonXoatrang.Name = "buttonXoatrang";
            this.buttonXoatrang.Size = new System.Drawing.Size(130, 43);
            this.buttonXoatrang.TabIndex = 11;
            this.buttonXoatrang.Text = "Xóa trắng";
            this.buttonXoatrang.UseVisualStyleBackColor = true;
            this.buttonXoatrang.Click += new System.EventHandler(this.buttonXoatrang_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(166, 9);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(130, 43);
            this.buttonImport.TabIndex = 10;
            this.buttonImport.Text = "Import...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonImport);
            this.panel2.Controls.Add(this.buttonXoatrang);
            this.panel2.Controls.Add(this.buttonThoat);
            this.panel2.Controls.Add(this.buttonSua);
            this.panel2.Controls.Add(this.buttonThem);
            this.panel2.Controls.Add(this.buttonXoa);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 471);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel2.Size = new System.Drawing.Size(1034, 55);
            this.panel2.TabIndex = 22;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 174);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1010, 291);
            this.dataGridView1.TabIndex = 16;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelTongSo);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel3.Size = new System.Drawing.Size(1034, 471);
            this.panel3.TabIndex = 23;
            // 
            // textBoxTen_INCI
            // 
            this.textBoxTen_INCI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTen_INCI.Location = new System.Drawing.Point(473, 28);
            this.textBoxTen_INCI.Name = "textBoxTen_INCI";
            this.textBoxTen_INCI.Size = new System.Drawing.Size(549, 31);
            this.textBoxTen_INCI.TabIndex = 0;
            this.textBoxTen_INCI.TextChanged += new System.EventHandler(this.textBoxTen_INCI_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(473, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tên INCI";
            // 
            // textBoxIDThanhphan_Cosing
            // 
            this.textBoxIDThanhphan_Cosing.Enabled = false;
            this.textBoxIDThanhphan_Cosing.Location = new System.Drawing.Point(12, 28);
            this.textBoxIDThanhphan_Cosing.Name = "textBoxIDThanhphan_Cosing";
            this.textBoxIDThanhphan_Cosing.ReadOnly = true;
            this.textBoxIDThanhphan_Cosing.Size = new System.Drawing.Size(455, 31);
            this.textBoxIDThanhphan_Cosing.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Mã thành phần CosIng";
            // 
            // textBoxEC_No
            // 
            this.textBoxEC_No.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEC_No.Location = new System.Drawing.Point(473, 78);
            this.textBoxEC_No.Name = "textBoxEC_No";
            this.textBoxEC_No.Size = new System.Drawing.Size(549, 31);
            this.textBoxEC_No.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(473, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 25);
            this.label12.TabIndex = 23;
            this.label12.Text = "Mã EC";
            // 
            // textBoxCAS_No
            // 
            this.textBoxCAS_No.Location = new System.Drawing.Point(12, 78);
            this.textBoxCAS_No.Name = "textBoxCAS_No";
            this.textBoxCAS_No.Size = new System.Drawing.Size(455, 31);
            this.textBoxCAS_No.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 25);
            this.label11.TabIndex = 25;
            this.label11.Text = "Mã CAS";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.textBoxCAS_No);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.textBoxEC_No);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxIDThanhphan_Cosing);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxTen_INCI);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(1034, 137);
            this.panel1.TabIndex = 21;
            // 
            // labelTongSo
            // 
            this.labelTongSo.AutoSize = true;
            this.labelTongSo.Location = new System.Drawing.Point(12, 146);
            this.labelTongSo.Name = "labelTongSo";
            this.labelTongSo.Size = new System.Drawing.Size(0, 25);
            this.labelTongSo.TabIndex = 17;
            // 
            // DanhMucThanhPhanCosing
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1034, 526);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DanhMucThanhPhanCosing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DanhMucThanhPhanCosing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DanhMucThanhPhanCosing_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonXoa;
        private System.Windows.Forms.Button buttonThem;
        private System.Windows.Forms.Button buttonSua;
        private System.Windows.Forms.Button buttonThoat;
        private System.Windows.Forms.Button buttonXoatrang;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBoxTen_INCI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIDThanhphan_Cosing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEC_No;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxCAS_No;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTongSo;
    }
}