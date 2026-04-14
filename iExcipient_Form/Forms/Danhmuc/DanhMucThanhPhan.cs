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
    public partial class DanhMucThanhPhan : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();

        public DanhMucThanhPhan()
        {
            InitializeComponent();
        }

        private void DanhMucThanhPhan_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            dataGridView1.DataSource = grid1;

            // Thiết lập DateTimePicker
            dateTimePickerNgayTao.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayTao.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dateTimePickerNgayCapNhat.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayCapNhat.CustomFormat = "dd/MM/yyyy HH:mm:ss";

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
            textBoxIDThanhphan.Clear();
            textBoxTen_INN.Clear();
            textBoxTen_INCI.Clear();
            textBoxTen_IUPAC.Clear();
            textBoxCAS_No.Clear();
            dateTimePickerNgayTao.Value = DateTime.Now;
            dateTimePickerNgayCapNhat.Value = DateTime.Now;
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
                List<ThanhPhan> dsThanhPhan = getdata.GetDSThanhPhan();

                var displayList = dsThanhPhan.Select(tp => new
                {
                    tp.IDThanhphan,
                    tp.Ten_INN,
                    tp.Ten_INCI,
                    tp.Ten_IUPAC,
                    tp.CAS_No,
                    tp.NgayTao,
                    tp.NgayCapNhat
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

                textBoxIDThanhphan.Text = row.Cells["IDThanhphan"].Value != null
                    ? row.Cells["IDThanhphan"].Value.ToString()
                    : "";

                textBoxTen_INN.Text = row.Cells["Ten_INN"].Value != null
                    ? row.Cells["Ten_INN"].Value.ToString()
                    : "";

                textBoxTen_INCI.Text = row.Cells["Ten_INCI"].Value != null
                    ? row.Cells["Ten_INCI"].Value.ToString()
                    : "";

                textBoxTen_IUPAC.Text = row.Cells["Ten_IUPAC"].Value != null
                    ? row.Cells["Ten_IUPAC"].Value.ToString()
                    : "";

                textBoxCAS_No.Text = row.Cells["CAS_No"].Value != null
                    ? row.Cells["CAS_No"].Value.ToString()
                    : "";

                if (row.Cells["NgayTao"].Value != null && row.Cells["NgayTao"].Value != DBNull.Value)
                {
                    dateTimePickerNgayTao.Value = Convert.ToDateTime(row.Cells["NgayTao"].Value);
                }
                else
                {
                    dateTimePickerNgayTao.Value = DateTime.Now;
                }

                if (row.Cells["NgayCapNhat"].Value != null && row.Cells["NgayCapNhat"].Value != DBNull.Value)
                {
                    dateTimePickerNgayCapNhat.Value = Convert.ToDateTime(row.Cells["NgayCapNhat"].Value);
                }
                else
                {
                    dateTimePickerNgayCapNhat.Value = DateTime.Now;
                }

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


                ThanhPhan item = new ThanhPhan
                {
                    Ten_INN = textBoxTen_INN.Text.Trim(),
                    Ten_INCI = textBoxTen_INCI.Text.Trim(),
                    Ten_IUPAC = textBoxTen_IUPAC.Text.Trim(),
                    CAS_No = textBoxCAS_No.Text.Trim(),
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now
                };

                if (insertdata.InsertThanhPhan(item))
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
                if (string.IsNullOrWhiteSpace(textBoxIDThanhphan.Text))
                {
                    MessageBox.Show("Vui lòng chọn thành phần cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idThanhPhan = int.Parse(textBoxIDThanhphan.Text);
                string tenThanhPhan = textBoxTen_INCI.Text;

                // Kiểm tra số lượng quan hệ
                KetnoiDB.DeleteData.ThanhPhanRelationCount relationCount =
                    deletedata.GetRelatedCountThanhPhan(idThanhPhan);

                if (relationCount.TotalCount > 0)
                {
                    string message = string.Format(
                        "Thành phần '{0}' có {1} quan hệ liên kết:\n" +
                        "- Chức năng: {2}\n" +
                        "- Dạng bào chế: {3}\n" +
                        "- Chất liên quan: {4}\n" +
                        "- Là chất liên quan của: {5}\n" +
                        "- Quy định: {6}\n\n" +
                        "Bạn có chắc chắn muốn xóa? Tất cả quan hệ sẽ bị xóa!",
                        tenThanhPhan,
                        relationCount.TotalCount,
                        relationCount.ChucNangCount,
                        relationCount.DangBaoCheCount,
                        relationCount.ChatLienQuanCount,
                        relationCount.ChatLienQuanAsRelatedCount,
                        relationCount.QuyDinhCount
                    );

                    DialogResult confirm = MessageBox.Show(message, "Cảnh báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes)
                        return;
                }
                else
                {
                    DialogResult confirm = MessageBox.Show(
                        string.Format("Bạn có chắc chắn muốn xóa thành phần '{0}'?", tenThanhPhan),
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirm != DialogResult.Yes)
                        return;
                }

                if (deletedata.DeleteThanhPhan(idThanhPhan))
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
                if (string.IsNullOrWhiteSpace(textBoxIDThanhphan.Text))
                {
                    MessageBox.Show("Vui lòng chọn thành phần cần sửa!", "Thông báo",
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

                int idThanhPhan = int.Parse(textBoxIDThanhphan.Text);

                // Lấy dữ liệu chi tiết hiện tại để giữ nguyên các field không sửa
                ThanhPhan current = getdata.GetThanhPhanByID(idThanhPhan);
                if (current == null) return;

                if (updatedata.UpdateThanhPhan(
                    idThanhPhan,
                    textBoxTen_INN.Text.Trim(),
                    textBoxTen_INCI.Text.Trim(),
                    textBoxTen_IUPAC.Text.Trim(),
                    textBoxCAS_No.Text.Trim(),
                    current.CongThucHoaHoc,     // giữ nguyên
                    current.KhoiLuongPhanTu,    // giữ nguyên
                    current.CauTrucPhanTu,      // giữ nguyên
                    current.TinhChatVatLy,      // giữ nguyên
                    current.MoTa,               // giữ nguyên
                    current.BaoQuan,            // giữ nguyên
                    current.TLTK                // giữ nguyên
                ))
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
                    Title = "Chọn file để import thành phần"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    List<ThanhPhan> listThanhPhan = new List<ThanhPhan>();

                    ImportFromCSV(filePath, listThanhPhan);

                    if (listThanhPhan.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listThanhPhan.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertThanhPhan(listThanhPhan))
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

        private void ImportFromCSV(string filePath, List<ThanhPhan> listThanhPhan)
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

                    if (values.Length >= 3 && !string.IsNullOrWhiteSpace(values[2]))
                    {
                        double khoiLuong = 0;
                        if (values.Length > 5 && !string.IsNullOrWhiteSpace(values[5]))
                        {
                            double.TryParse(values[5].Trim().Trim('"'), out khoiLuong);
                        }

                        ThanhPhan tp = new ThanhPhan
                        {
                            Ten_INN = values.Length > 0 ? values[0].Trim().Trim('"') : "",
                            Ten_INCI = values.Length > 1 ? values[1].Trim().Trim('"') : "",
                            Ten_IUPAC = values.Length > 2 ? values[2].Trim().Trim('"') : "",
                            CAS_No = values.Length > 3 ? values[3].Trim().Trim('"') : "",
                            CongThucHoaHoc = values.Length > 4 ? values[4].Trim().Trim('"') : "",
                            KhoiLuongPhanTu = khoiLuong,
                            CauTrucPhanTu = values.Length > 6 ? values[6].Trim().Trim('"') : "",
                            TinhChatVatLy = values.Length > 7 ? values[7].Trim().Trim('"') : "",
                            MoTa = values.Length > 8 ? values[8].Trim().Trim('"') : "",
                            BaoQuan = values.Length > 9 ? values[9].Trim().Trim('"') : "",
                            TLTK = values.Length > 11 ? values[11].Trim().Trim('"') : "",
                            NgayTao = DateTime.Now,
                            NgayCapNhat = DateTime.Now
                        };
                        listThanhPhan.Add(tp);
                    }
                }
            }
        }

        private void textBoxTen_INN_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxTen_INN.Text);
        }

        private void buttonChitiet_Click(object sender, EventArgs e)
        {
            using (Forms.Thietlap.ChiTietThanhPhan formcon = new Forms.Thietlap.ChiTietThanhPhan())
            {
                if (formcon.ShowDialog() == DialogResult.OK)
                {
                    refreshDatagrid(); // Refresh nếu form con có thay đổi dữ liệu
                }
            }
        }
    }
}