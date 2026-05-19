using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iExcipient_Form
{
    public partial class Main : Form
    {
        private Form activeForm = null;
        public Main()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void chứcNăngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucChucNang DMChucnang = new Forms.Danhmuc.DanhMucChucNang();
            openChildForm(DMChucnang);
        }

        private void openChildForm(Form childForm) //Mở các giao diện con
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(childForm);
            panelContainer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void quyĐịnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucQuyDinh DMQuydinh = new Forms.Danhmuc.DanhMucQuyDinh();
            openChildForm(DMQuydinh);
        }

        private void thànhPhầnChứcNăngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.QuanHeThanhPhan_ChucNang QuanHeThanhphan_Chucnang = new Forms.Danhmuc.QuanHeThanhPhan_ChucNang();
            openChildForm(QuanHeThanhphan_Chucnang);
        }

        private void thànhPhầnChấtLiênQuaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.QuanHeThanhPhan_ChatLienQuan QuanHeThanhphan_Chatlienquan = new Forms.Danhmuc.QuanHeThanhPhan_ChatLienQuan();
            openChildForm(QuanHeThanhphan_Chatlienquan);
        }

        private void traCứuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Tracuu.Tracuu TraCuu = new Forms.Tracuu.Tracuu();
            openChildForm(TraCuu);
        }

        private void thànhPhầnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucThanhPhan DanhMucThanhphan = new Forms.Danhmuc.DanhMucThanhPhan();
            openChildForm(DanhMucThanhphan);
        }

        private void điểmEWGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Thietlap.QuanHeThanhphan_EWGScore QuanHeThanhphan_EWGScore = new Forms.Thietlap.QuanHeThanhphan_EWGScore();
            openChildForm(QuanHeThanhphan_EWGScore);
        }

        private void chứcNăngCosingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucChucNangCosing DMChucnangcosing = new Forms.Danhmuc.DanhMucChucNangCosing();
            openChildForm(DMChucnangcosing);
        }

        private void chứcNăngCosIngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.QuanHeThanhPhan_ChucNangCosing QuanHeThanhphan_ChucnangCosing = new Forms.Danhmuc.QuanHeThanhPhan_ChucNangCosing();
            openChildForm(QuanHeThanhphan_ChucnangCosing);
        }

        private void thànhPhầnCosIngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucThanhPhanCosing DanhMucThanhphancosing = new Forms.Danhmuc.DanhMucThanhPhanCosing();
            openChildForm(DanhMucThanhphancosing);
        }

        private void quyĐịnhCosIngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Danhmuc.DanhMucQuydinhCosing DanhMucQuydinhcosing = new Forms.Danhmuc.DanhMucQuydinhCosing();
            openChildForm(DanhMucQuydinhcosing);
        }

        private void quanHệCosIngHOPEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Thietlap.QuanHeLinkCosingVaSach QuanHeLinkcosingVasach = new Forms.Thietlap.QuanHeLinkCosingVaSach();
            openChildForm(QuanHeLinkcosingVasach);
        }

    }
}
