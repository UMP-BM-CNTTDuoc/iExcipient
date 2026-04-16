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

namespace iExcipient_Form.Forms.Danhmuc
{
    public partial class DanhMucQuyDinh : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        public DanhMucQuyDinh()
        {
            InitializeComponent();
        }

        private void DanhMucQuyDinh_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            dataGridView1.DataSource = grid1;
            LoadComboBoxThanhPhan();
            refreshDatagrid();

        }
        private void LoadComboBoxThanhPhan()
        {
            try
            {
                List<ThanhPhan> dsThanhPhan = getdata.GetDSThanhPhan();
                comboBoxTenThanhPhan.DataSource = dsThanhPhan;
                comboBoxTenThanhPhan.DisplayMember = "Ten_INCI";
                comboBoxTenThanhPhan.ValueMember = "IDThanhphan";
                comboBoxTenThanhPhan.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thành phần: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            textBoxGhiChu.Clear();
            comboBoxTenThanhPhan.SelectedIndex = -1;
            checkBoxAnnexII.Checked = false;
            checkBoxAnnexIII.Checked = false;
            checkBoxAnnexIV.Checked = false;
            checkBoxAnnexV.Checked = false;
            checkBoxAnnexVI.Checked = false;
            comboBoxTenThanhPhan.Focus();
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
                // Get data from database
                List<QuyDinh> dsQuyDinh = getdata.GetDSQuyDinh();

                // Create display list with ThanhPhan name
                var displayList = dsQuyDinh.Select(qd => new
                {
                    qd.IDQuydinh,
                    qd.IDThanhphan,
                    TenThanhPhan = GetTenThanhPhan(qd.IDThanhphan),
                    qd.AnnexII,
                    qd.AnnexIII,
                    qd.AnnexIV,
                    qd.AnnexV,
                    qd.AnnexVI,
                    qd.DieuKienSuDung,
                    qd.Ghichu
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
        private string GetTenThanhPhan(int idThanhPhan)
        {
            try
            {
                ThanhPhan tp = getdata.GetThanhPhan(idThanhPhan);
                return tp != null ? tp.Ten_INCI : "";
            }
            catch
            {
                return "";
            }
        }
        private void comboBoxTenThanhPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id;

            if (comboBoxTenThanhPhan.SelectedValue != null &&
                int.TryParse(comboBoxTenThanhPhan.SelectedValue.ToString(), out id))
            {
                textBoxIDThanhPhan.Text = id.ToString();
                buttonThem.Enabled = true;
            }
            else
            {
                textBoxIDThanhPhan.Clear();
                buttonThem.Enabled = false;
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate textboxes
                textBoxIDQuyDinh.Text = row.Cells["IDQuydinh"].Value != null
                    ? row.Cells["IDQuydinh"].Value.ToString()
                    : "";

                textBoxIDThanhPhan.Text = row.Cells["IDThanhphan"].Value != null
                    ? row.Cells["IDThanhphan"].Value.ToString()
                    : "";

                textBoxDieuKienSuDung.Text = row.Cells["DieuKienSuDung"].Value != null
                    ? row.Cells["DieuKienSuDung"].Value.ToString()
                    : "";

                textBoxGhiChu.Text = row.Cells["Ghichu"].Value != null
                    ? row.Cells["Ghichu"].Value.ToString()
                    : "";

                // Set combobox by IDThanhphan
                if (row.Cells["IDThanhphan"].Value != null &&
                    row.Cells["IDThanhphan"].Value != DBNull.Value)
                {
                    int idThanhPhan = Convert.ToInt32(row.Cells["IDThanhphan"].Value);
                    comboBoxTenThanhPhan.SelectedValue = idThanhPhan;
                }
                else
                {
                    comboBoxTenThanhPhan.SelectedIndex = -1;
                }

                // Set checkboxes
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

                // Enable buttons
                buttonXoa.Enabled = true;
                buttonSua.Enabled = true;
            }
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTenThanhPhan.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn thành phần!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTenThanhPhan.Focus();
                    return;
                }

                QuyDinh item = new QuyDinh
                {
                    IDThanhphan = int.Parse(textBoxIDThanhPhan.Text),
                    AnnexII = checkBoxAnnexII.Checked ? (bool?)true : null,
                    AnnexIII = checkBoxAnnexIII.Checked ? (bool?)true : null,
                    AnnexIV = checkBoxAnnexIV.Checked ? (bool?)true : null,
                    AnnexV = checkBoxAnnexV.Checked ? (bool?)true : null,
                    AnnexVI = checkBoxAnnexVI.Checked ? (bool?)true : null,
                    DieuKienSuDung = textBoxDieuKienSuDung.Text.Trim(),
                    Ghichu = textBoxGhiChu.Text.Trim()
                };

                if (insertdata.InsertQuyDinh(item))
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
                if (string.IsNullOrWhiteSpace(textBoxIDQuyDinh.Text))
                {
                    MessageBox.Show("Vui lòng chọn quy định cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idQuyDinh = int.Parse(textBoxIDQuyDinh.Text);
                string tenThanhPhan = comboBoxTenThanhPhan.Text.Trim();

                // BƯỚC 1: Kiểm tra quan hệ liên quan
                int soLuongQuanHe = deletedata.GetRelatedCountQuyDinh(idQuyDinh);

                // BƯỚC 2: Tạo thông báo phù hợp
                string confirmMessage = "";
                MessageBoxIcon icon;

                if (soLuongQuanHe > 0)
                {
                    confirmMessage = string.Format(
                        "CẢNH BÁO: Quy định của thành phần '{0}' đang được sử dụng bởi {1} thành phần.\n\n" +
                        "Nếu xóa, tất cả {1} quan hệ này sẽ BỊ XÓA VĨNH VIỄN.\n\n" +
                        "Bạn có CHẮC CHẮN muốn xóa?",
                        tenThanhPhan,
                        soLuongQuanHe
                    );
                    icon = MessageBoxIcon.Warning;
                }
                else
                {
                    confirmMessage = string.Format(
                        "Bạn có chắc chắn muốn xóa quy định của thành phần '{0}'?",
                        tenThanhPhan
                    );
                    icon = MessageBoxIcon.Question;
                }

                // BƯỚC 3: Hiển thị dialog xác nhận
                DialogResult confirm = MessageBox.Show(
                    confirmMessage,
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    icon
                );

                // BƯỚC 4: Thực hiện xóa
                if (confirm == DialogResult.Yes)
                {
                    if (deletedata.DeleteQuyDinh(idQuyDinh))
                    {
                        string successMsg = soLuongQuanHe > 0
                            ? string.Format("Đã xóa quy định của thành phần '{0}' và {1} quan hệ liên quan!", tenThanhPhan, soLuongQuanHe)
                            : string.Format("Đã xóa quy định của thành phần '{0}' thành công!", tenThanhPhan);

                        MessageBox.Show(successMsg, "Thành công",
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
                // Validate ID
                if (string.IsNullOrWhiteSpace(textBoxIDQuyDinh.Text))
                {
                    MessageBox.Show("Vui lòng chọn quy định cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate thành phần
                if (comboBoxTenThanhPhan.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn thành phần!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTenThanhPhan.Focus();
                    return;
                }

                int idQuyDinh = int.Parse(textBoxIDQuyDinh.Text);

                QuyDinh item = new QuyDinh
                {
                    IDQuydinh = idQuyDinh,
                    IDThanhphan = int.Parse(textBoxIDThanhPhan.Text),
                    AnnexII = checkBoxAnnexII.Checked ? (bool?)true : null,
                    AnnexIII = checkBoxAnnexIII.Checked ? (bool?)true : null,
                    AnnexIV = checkBoxAnnexIV.Checked ? (bool?)true : null,
                    AnnexV = checkBoxAnnexV.Checked ? (bool?)true : null,
                    AnnexVI = checkBoxAnnexVI.Checked ? (bool?)true : null,
                    DieuKienSuDung = textBoxDieuKienSuDung.Text.Trim(),
                    Ghichu = textBoxGhiChu.Text.Trim()
                };

                if (updatedata.UpdateQuyDinh(
                    idQuyDinh,
                    int.Parse(textBoxIDThanhPhan.Text),
                    checkBoxAnnexII.Checked ? (bool?)true : null,
                    checkBoxAnnexIII.Checked ? (bool?)true : null,
                    checkBoxAnnexIV.Checked ? (bool?)true : null,
                    checkBoxAnnexV.Checked ? (bool?)true : null,
                    checkBoxAnnexVI.Checked ? (bool?)true : null,
                    textBoxDieuKienSuDung.Text.Trim(),
                    textBoxGhiChu.Text.Trim()))
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
                    Title = "Chọn file để import (Các cột: IDThanhPhan, AnnexII, AnnexIII, AnnexIV, AnnexV, AnnexVI, DieuKienSuDung, GhiChu)"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    List<QuyDinh> listQuyDinh = new List<QuyDinh>();

                    ImportFromCSV(filePath, listQuyDinh);

                    if (listQuyDinh.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listQuyDinh.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertQuyDinh(listQuyDinh))
                            {
                                MessageBox.Show("Import thành công " + listQuyDinh.Count.ToString() + " bản ghi!",
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

        private void ImportFromCSV(string filePath, List<QuyDinh> listQuyDinh)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                bool isFirstRow = true;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    string[] values = line.Split(',');

                    if (values.Length >= 1 && !string.IsNullOrWhiteSpace(values[0]))
                    {
                        QuyDinh qd = new QuyDinh
                        {
                            IDThanhphan = int.Parse(values[0].Trim().Trim('"')),
                            AnnexII = values.Length > 1 && values[1].Trim().Trim('"').ToLower() == "true" ? (bool?)true : null,
                            AnnexIII = values.Length > 2 && values[2].Trim().Trim('"').ToLower() == "true" ? (bool?)true : null,
                            AnnexIV = values.Length > 3 && values[3].Trim().Trim('"').ToLower() == "true" ? (bool?)true : null,
                            AnnexV = values.Length > 4 && values[4].Trim().Trim('"').ToLower() == "true" ? (bool?)true : null,
                            AnnexVI = values.Length > 5 && values[5].Trim().Trim('"').ToLower() == "true" ? (bool?)true : null,
                            DieuKienSuDung = values.Length > 6 ? values[6].Trim().Trim('"') : "",
                            Ghichu = values.Length > 7 ? values[7].Trim().Trim('"') : ""
                        };
                        listQuyDinh.Add(qd);
                    }
                }
            }
        }

    }
}
