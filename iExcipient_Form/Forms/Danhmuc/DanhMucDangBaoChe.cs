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
    public partial class DanhMucDangBaoChe : Form
    {
        public DanhMucDangBaoChe()
        {
            InitializeComponent();
        }

        private void DanhMucDangBaoChe_Load(object sender, EventArgs e)
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
            textBoxIDDangBaoChe.Clear();
            textBoxDangBaoChe.Clear();
            textBoxDangBaoChe.Focus();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }

        private void textBoxDangBaoChe_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxDangBaoChe.Text);
        }
    }
}
