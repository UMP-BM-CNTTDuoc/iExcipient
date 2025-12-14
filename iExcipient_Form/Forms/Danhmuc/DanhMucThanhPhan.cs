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
    public partial class DanhMucThanhPhan : Form
    {
        public DanhMucThanhPhan()
        {
            InitializeComponent();
        }

        private void DanhMucThanhPhan_Load(object sender, EventArgs e)
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
            textBoxIDThanhphan.Clear();
            textBoxTen_INN.Clear();
            textBoxTen_INCI.Clear();
            textBoxTen_IUPAC.Clear();
            textBoxCAS_No.Clear();
            textBoxCongThucHoaHoc.Clear();
            //textBoxCauTrucPhanTu
            textBoxKhoiLuongPhanTu.Clear();
            textBoxTinhChatVatLy.Clear();
            textBoxMoTa.Clear();
            textBoxBaoQuan.Clear();
            //textBoxChatLienQuan.Clear();
            textBoxTLTK.Clear();
            //dateTimePickerNgayCapNhat
            //dateTimePickerNgayTao
            textBoxTen_INN.Focus();
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
