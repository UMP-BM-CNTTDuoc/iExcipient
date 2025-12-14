using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iExcipient_Form.Forms.Danhmuc
{
    public partial class DanhMucChucNang : Form
    {
        public DanhMucChucNang()
        {
            InitializeComponent();
        }

        private void DanhMucChucNang_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            textBoxIDChucNang.Clear();
            textBoxChucNang.Clear();
            textBoxMoTa.Clear();
            textBoxChucNang.Focus();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }

        private void textBoxChucNang_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxChucNang.Text);
        }
    }
}
