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
    public partial class QuanHeThanhPhan_ChucNang : Form
    {
        public QuanHeThanhPhan_ChucNang()
        {
            InitializeComponent();
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (Forms.Thietlap.Import_ThanhPhan_ChucNang formcon = new Thietlap.Import_ThanhPhan_ChucNang())
            {
                formcon.ShowDialog();
            }
        }
    }
}
