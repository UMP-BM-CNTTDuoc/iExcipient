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
    public partial class DanhMucQuyDinh : Form
    {
        public DanhMucQuyDinh()
        {
            InitializeComponent();
        }

        private void DanhMucQuyDinh_Load(object sender, EventArgs e)
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
            textBoxIDQuyDinh.Clear();
            textBoxIDThanhPhan.Clear();
            textBoxDieuKienSuDung.Clear();
            textBoxDieuKienSuDung.Clear();
            //comboboxTenThanhPhan
            //checkBoxAnnexII
            //checkBoxAnnexIII
            //checkBoxAnnexIV
            //checkBoxAnnexV
            //checkBoxAnnexVI
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }
    }
}
