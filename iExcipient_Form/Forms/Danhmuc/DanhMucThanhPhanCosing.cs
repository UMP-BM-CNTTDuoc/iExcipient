using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClassLibraryIE;
using Microsoft.VisualBasic.FileIO;

namespace iExcipient_Form.Forms.Danhmuc
{
    public partial class DanhMucThanhPhanCosing : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();

        public DanhMucThanhPhanCosing()
        {
            InitializeComponent();
        }

        private void DanhMucThanhPhanCosing_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            dataGridView1.DataSource = grid1;

            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            refreshDatagrid();
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            textBoxIDThanhphan_Cosing.Clear();
            textBoxTen_INCI.Clear();
            textBoxCAS_No.Clear();
            textBoxEC_No.Clear();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = true;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            buttonThem.Enabled = true;
            ClearTextBoxes();
        }

        private void refreshDatagrid()
        {
            try
            {
                int tongSo = getdata.CountThanhPhanCosing();
                labelTongSo.Text = string.Format("Tổng: {0} thành phần", tongSo);

                List<ThanhPhanCosing> dsThanhPhanCosing = getdata.GetDSThanhPhanCosingTop(30100);

                var displayList = dsThanhPhanCosing.Select(tp => new
                {
                    tp.IDThanhphan_Cosing,
                    tp.Ten_INCI,
                    tp.CAS_No,
                    tp.EC_No
                }).ToList();

                grid1.DataSource = displayList;
                dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBoxIDThanhphan_Cosing.Text = row.Cells["IDThanhphan_Cosing"].Value != null
                    ? row.Cells["IDThanhphan_Cosing"].Value.ToString()
                    : "";

                textBoxTen_INCI.Text = row.Cells["Ten_INCI"].Value != null
                    ? row.Cells["Ten_INCI"].Value.ToString()
                    : "";

                textBoxCAS_No.Text = row.Cells["CAS_No"].Value != null
                    ? row.Cells["CAS_No"].Value.ToString()
                    : "";

                textBoxEC_No.Text = row.Cells["EC_No"].Value != null
                    ? row.Cells["EC_No"].Value.ToString()
                    : "";

                buttonXoa.Enabled = true;
                buttonSua.Enabled = true;
            }
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxTen_INCI.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên INCI!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxTen_INCI.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxCAS_No.Text))
                {
                    MessageBox.Show("Vui lòng nhập CAS No!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxCAS_No.Focus();
                    return;
                }

                ThanhPhanCosing item = new ThanhPhanCosing
                {
                    Ten_INCI = textBoxTen_INCI.Text.Trim(),
                    CAS_No = textBoxCAS_No.Text.Trim(),
                    EC_No = textBoxEC_No.Text.Trim()
                };

                if (insertdata.InsertThanhPhanCosing(item))
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại! CAS No có thể đã tồn tại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxIDThanhphan_Cosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn thành phần Cosing cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(textBoxIDThanhphan_Cosing.Text);

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa thành phần Cosing này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                if (deletedata.DeleteThanhPhanCosing(id))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                    buttonXoa.Enabled = false;
                    buttonSua.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxIDThanhphan_Cosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn thành phần Cosing cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxTen_INCI.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên INCI!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxTen_INCI.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxCAS_No.Text))
                {
                    MessageBox.Show("Vui lòng nhập CAS No!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxCAS_No.Focus();
                    return;
                }

                int id = int.Parse(textBoxIDThanhphan_Cosing.Text);

                if (updatedata.UpdateThanhPhanCosing(
                    id,
                    textBoxTen_INCI.Text.Trim(),
                    textBoxCAS_No.Text.Trim(),
                    textBoxEC_No.Text.Trim()))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                    buttonXoa.Enabled = false;
                    buttonSua.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "CSV Files|*.csv",
                    Title = "Chọn file để import thành phần Cosing"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    List<ThanhPhanCosing> listThanhPhanCosing = new List<ThanhPhanCosing>();

                    ImportFromCSV(filePath, listThanhPhanCosing);

                    if (listThanhPhanCosing.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listThanhPhanCosing.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertThanhPhanCosing(listThanhPhanCosing))
                            {
                                MessageBox.Show("Import thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                refreshDatagrid();
                            }
                            else
                            {
                                MessageBox.Show("Import thất bại hoặc tất cả đã tồn tại!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu trong file!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportFromCSV(string filePath, List<ThanhPhanCosing> listThanhPhanCosing)
        {
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser =
                   new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                bool isFirstRow = true;
                while (!parser.EndOfData)
                {
                    string[] values = parser.ReadFields();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    if (values.Length >= 1 && !string.IsNullOrWhiteSpace(values[0]))
                    {
                        ThanhPhanCosing tp = new ThanhPhanCosing
                        {
                            Ten_INCI = values.Length > 0 ? values[0].Trim() : "",
                            CAS_No = values.Length > 1 ? values[1].Trim() : "",
                            EC_No = values.Length > 2 ? values[2].Trim() : ""
                        };
                        listThanhPhanCosing.Add(tp);
                    }
                }
            }
        }

        private void textBoxTen_INCI_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxTen_INCI.Text);
        }
    }
}