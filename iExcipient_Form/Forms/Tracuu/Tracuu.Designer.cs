namespace iExcipient_Form.Forms.Tracuu
{
    partial class Tracuu
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
            this.pnlResults = new System.Windows.Forms.Panel();
            this.flpResults = new System.Windows.Forms.FlowLayoutPanel();
            this.cboItemsPerPage = new System.Windows.Forms.ComboBox();
            this.lblTotalResults = new System.Windows.Forms.Label();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.buttonThoat = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlResults.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlResults
            // 
            this.pnlResults.Controls.Add(this.flpResults);
            this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResults.Location = new System.Drawing.Point(0, 151);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlResults.Size = new System.Drawing.Size(1034, 320);
            this.pnlResults.TabIndex = 19;
            this.pnlResults.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlResults_Paint);
            // 
            // flpResults
            // 
            this.flpResults.AutoScroll = true;
            this.flpResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpResults.Location = new System.Drawing.Point(0, 0);
            this.flpResults.Name = "flpResults";
            this.flpResults.Size = new System.Drawing.Size(1034, 320);
            this.flpResults.TabIndex = 0;
            this.flpResults.Paint += new System.Windows.Forms.PaintEventHandler(this.flpResults_Paint);
            // 
            // cboItemsPerPage
            // 
            this.cboItemsPerPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboItemsPerPage.FormattingEnabled = true;
            this.cboItemsPerPage.Location = new System.Drawing.Point(586, 16);
            this.cboItemsPerPage.Name = "cboItemsPerPage";
            this.cboItemsPerPage.Size = new System.Drawing.Size(121, 28);
            this.cboItemsPerPage.TabIndex = 20;
            this.cboItemsPerPage.SelectedIndexChanged += new System.EventHandler(this.cboItemsPerPage_SelectedIndexChanged);
            // 
            // lblTotalResults
            // 
            this.lblTotalResults.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalResults.AutoSize = true;
            this.lblTotalResults.Location = new System.Drawing.Point(440, 29);
            this.lblTotalResults.Name = "lblTotalResults";
            this.lblTotalResults.Size = new System.Drawing.Size(60, 20);
            this.lblTotalResults.TabIndex = 19;
            this.lblTotalResults.Text = "label11";
            this.lblTotalResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTotalResults.Click += new System.EventHandler(this.lblTotalResults_Click);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(440, 6);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(60, 20);
            this.lblPageInfo.TabIndex = 18;
            this.lblPageInfo.Text = "label11";
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPageInfo.Click += new System.EventHandler(this.lblPageInfo_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNextPage.Location = new System.Drawing.Point(527, 6);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(53, 43);
            this.btnNextPage.TabIndex = 17;
            this.btnNextPage.Text = ">>>";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboItemsPerPage);
            this.panel2.Controls.Add(this.lblTotalResults);
            this.panel2.Controls.Add(this.lblPageInfo);
            this.panel2.Controls.Add(this.btnNextPage);
            this.panel2.Controls.Add(this.btnPrevPage);
            this.panel2.Controls.Add(this.buttonThoat);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 471);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel2.Size = new System.Drawing.Size(1034, 55);
            this.panel2.TabIndex = 18;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPrevPage.Location = new System.Drawing.Point(348, 6);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(53, 43);
            this.btnPrevPage.TabIndex = 16;
            this.btnPrevPage.Text = "<<<";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // buttonThoat
            // 
            this.buttonThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonThoat.Location = new System.Drawing.Point(879, 9);
            this.buttonThoat.Name = "buttonThoat";
            this.buttonThoat.Size = new System.Drawing.Size(143, 43);
            this.buttonThoat.TabIndex = 15;
            this.buttonThoat.Text = "Thoát";
            this.buttonThoat.UseVisualStyleBackColor = true;
            this.buttonThoat.Click += new System.EventHandler(this.buttonThoat_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlSearch.Size = new System.Drawing.Size(1034, 151);
            this.pnlSearch.TabIndex = 17;
            this.pnlSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSearch_Paint);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Danh sách chỉ định";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Tracuu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1034, 526);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Tracuu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tracuu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlResults.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlResults;
        private System.Windows.Forms.FlowLayoutPanel flpResults;
        private System.Windows.Forms.ComboBox cboItemsPerPage;
        private System.Windows.Forms.Label lblTotalResults;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button buttonThoat;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label label4;
    }
}