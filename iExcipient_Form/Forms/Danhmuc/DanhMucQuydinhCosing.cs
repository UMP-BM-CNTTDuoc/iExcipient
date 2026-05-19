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
    public partial class DanhMucQuydinhCosing : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();

        public DanhMucQuydinhCosing()
        {
            InitializeComponent();
        }

        private void DanhMucQuydinhCosing_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            dataGridView1.DataSource = grid1;
            LoadComboBoxThanhPhanCosing();
            refreshDatagrid();
        }

        private void LoadComboBoxThanhPhanCosing()
        {
            try
            {
                List<ThanhPhanCosing> dsThanhPhanCosing = getdata.GetDSThanhPhanCosing();
                comboBoxTenThanhPhanCosing.DataSource = dsThanhPhanCosing;
                comboBoxTenThanhPhanCosing.DisplayMember = "Ten_INCI";
                comboBoxTenThanhPhanCosing.ValueMember = "IDThanhphan_Cosing";
                comboBoxTenThanhPhanCosing.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thành phần Cosing: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            textBoxIDQuydinhCosing.Clear();
            textBoxIDThanhPhanCosing.Clear();
            comboBoxTenThanhPhanCosing.SelectedIndex = -1;
            checkBoxAnnexII.Checked = false;
            checkBoxAnnexIII.Checked = false;
            checkBoxAnnexIV.Checked = false;
            checkBoxAnnexV.Checked = false;
            checkBoxAnnexVI.Checked = false;
            comboBoxTenThanhPhanCosing.Focus();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = true;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            buttonThem.Enabled = false;
            ClearTextBoxes();
        }

        private void refreshDatagrid()
        {
            try
            {
                List<QuydinhCosing> dsQuydinhCosing = getdata.GetDSQuydinhCosing();

                var displayList = dsQuydinhCosing.Select(qd => new
                {
                    qd.IDQuydinh_Cosing,
                    qd.IDThanhphan_Cosing,
                    TenThanhPhanCosing = GetTenThanhPhanCosing(qd.IDThanhphan_Cosing),
                    qd.AnnexII,
                    qd.AnnexIII,
                    qd.AnnexIV,
                    qd.AnnexV,
                    qd.AnnexVI
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

        private string GetTenThanhPhanCosing(int idThanhPhanCosing)
        {
            try
            {
                ThanhPhanCosing tp = getdata.GetThanhPhanCosing(idThanhPhanCosing);
                return tp != null ? tp.Ten_INCI : "";
            }
            catch
            {
                return "";
            }
        }

        private void comboBoxTenThanhPhanCosing_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBoxIDQuydinhCosing.Text = row.Cells["IDQuydinh_Cosing"].Value != null
                    ? row.Cells["IDQuydinh_Cosing"].Value.ToString()
                    : "";

                textBoxIDThanhPhanCosing.Text = row.Cells["IDThanhphan_Cosing"].Value != null
                    ? row.Cells["IDThanhphan_Cosing"].Value.ToString()
                    : "";

                if (row.Cells["IDThanhphan_Cosing"].Value != null &&
                    row.Cells["IDThanhphan_Cosing"].Value != DBNull.Value)
                {
                    int idThanhPhanCosing = Convert.ToInt32(row.Cells["IDThanhphan_Cosing"].Value);
                    comboBoxTenThanhPhanCosing.SelectedValue = idThanhPhanCosing;
                }
                else
                {
                    comboBoxTenThanhPhanCosing.SelectedIndex = -1;
                }

                checkBoxAnnexII.Checked = row.Cells["AnnexII"].Value != null &&
                    row.Cells["AnnexII"].Value != DBNull.Value &&
                    Convert.ToBoolean(row.Cells["AnnexII"].Value);

                checkBoxAnnexIII.Checked = row.Cells["AnnexIII"].Value != null &&
                    row.Cells["AnnexIII"].Value != DBNull.Value &&
                    Convert.ToBoolean(row.Cells["AnnexIII"].Value);

                checkBoxAnnexIV.Checked = row.Cells["AnnexIV"].Value != null &&
                    row.Cells["AnnexIV"].Value != DBNull.Value &&
                    Convert.ToBoolean(row.Cells["AnnexIV"].Value);

                checkBoxAnnexV.Checked = row.Cells["AnnexV"].Value != null &&
                    row.Cells["AnnexV"].Value != DBNull.Value &&
                    Convert.ToBoolean(row.Cells["AnnexV"].Value);

                checkBoxAnnexVI.Checked = row.Cells["AnnexVI"].Value != null &&
                    row.Cells["AnnexVI"].Value != DBNull.Value &&
                    Convert.ToBoolean(row.Cells["AnnexVI"].Value);

                buttonXoa.Enabled = true;
                buttonSua.Enabled = true;
            }
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTenThanhPhanCosing.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn thành phần Cosing!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTenThanhPhanCosing.Focus();
                    return;
                }

                QuydinhCosing item = new QuydinhCosing
                {
                    IDThanhphan_Cosing = int.Parse(textBoxIDThanhPhanCosing.Text),
                    AnnexII = checkBoxAnnexII.Checked ? (bool?)true : null,
                    AnnexIII = checkBoxAnnexIII.Checked ? (bool?)true : null,
                    AnnexIV = checkBoxAnnexIV.Checked ? (bool?)true : null,
                    AnnexV = checkBoxAnnexV.Checked ? (bool?)true : null,
                    AnnexVI = checkBoxAnnexVI.Checked ? (bool?)true : null,
                };

                if (insertdata.InsertQuydinhCosing(item))
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại!", "Lỗi",
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
                if (string.IsNullOrWhiteSpace(textBoxIDQuydinhCosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn quy định Cosing cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idQuydinhCosing = int.Parse(textBoxIDQuydinhCosing.Text);
                string tenThanhPhanCosing = comboBoxTenThanhPhanCosing.Text.Trim();

                DialogResult confirm = MessageBox.Show(
                    string.Format("Bạn có chắc chắn muốn xóa quy định Cosing của thành phần '{0}'?",
                        tenThanhPhanCosing),
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    if (deletedata.DeleteQuydinhCosing(idQuydinhCosing))
                    {
                        MessageBox.Show(
                            string.Format("Đã xóa quy định Cosing của thành phần '{0}' thành công!", tenThanhPhanCosing),
                            "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearTextBoxes();
                        refreshDatagrid();
                        buttonXoa.Enabled = false;
                        buttonSua.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại! Vui lòng thử lại.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                if (string.IsNullOrWhiteSpace(textBoxIDQuydinhCosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn quy định Cosing cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxTenThanhPhanCosing.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn thành phần Cosing!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTenThanhPhanCosing.Focus();
                    return;
                }

                int idQuydinhCosing = int.Parse(textBoxIDQuydinhCosing.Text);

                if (updatedata.UpdateQuydinhCosing(
                    idQuydinhCosing,
                    int.Parse(textBoxIDThanhPhanCosing.Text),
                    checkBoxAnnexII.Checked ? (bool?)true : null,
                    checkBoxAnnexIII.Checked ? (bool?)true : null,
                    checkBoxAnnexIV.Checked ? (bool?)true : null,
                    checkBoxAnnexV.Checked ? (bool?)true : null,
                    checkBoxAnnexVI.Checked ? (bool?)true : null))
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
                    Title = "Chọn file để import (Các cột: IDThanhPhan_Cosing, AnnexII, AnnexIII, AnnexIV, AnnexV, AnnexVI)"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    List<QuydinhCosing> listQuydinhCosing = new List<QuydinhCosing>();

                    ImportFromCSV(filePath, listQuydinhCosing);

                    if (listQuydinhCosing.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listQuydinhCosing.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertQuydinhCosing(listQuydinhCosing))
                            {
                                MessageBox.Show("Import thành công " + listQuydinhCosing.Count.ToString() + " bản ghi!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                refreshDatagrid();
                            }
                            else
                            {
                                MessageBox.Show("Import thất bại!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ImportFromCSV(string filePath, List<QuydinhCosing> listQuydinhCosing)
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
                        QuydinhCosing qd = new QuydinhCosing
                        {
                            IDThanhphan_Cosing = int.Parse(values[0].Trim()),
                            AnnexII = values.Length > 1 && values[1].Trim().ToLower() == "true" ? (bool?)true : null,
                            AnnexIII = values.Length > 2 && values[2].Trim().ToLower() == "true" ? (bool?)true : null,
                            AnnexIV = values.Length > 3 && values[3].Trim().ToLower() == "true" ? (bool?)true : null,
                            AnnexV = values.Length > 4 && values[4].Trim().ToLower() == "true" ? (bool?)true : null,
                            AnnexVI = values.Length > 5 && values[5].Trim().ToLower() == "true" ? (bool?)true : null
                        };
                        listQuydinhCosing.Add(qd);
                    }
                }
            }
        }
    }
}