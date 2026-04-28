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

namespace iExcipient_Form.Forms.Thietlap
{
    public partial class ChiTietThanhPhan : Form
    {
        BindingSource grid1 = new BindingSource();

        private int _idDangChon = -1;

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        public ChiTietThanhPhan()
        {
            InitializeComponent();
        }
        private void ChiTietThanhPhan_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = grid1;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            SetPanelEnabled(false);
            refreshDatagrid();

            if (dataGridView1.Columns["IDThanhphan"] != null)
                dataGridView1.Columns["IDThanhphan"].Visible = false;
        }
        private void refreshDatagrid()
        {
            try
            {
                List<ThanhPhan> ds = getdata.GetDSThanhPhan();

                var displayList = ds.Select(tp => new
                {
                    tp.IDThanhphan,
                    tp.Ten_INN,
                    tp.Ten_INCI,
                    tp.Ten_IUPAC,
                    tp.CAS_No
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

        private void SetPanelEnabled(bool enabled)
        {
            textBoxCongThucHoaHoc.Enabled = enabled;
            textBoxKhoiLuongPhanTu.Enabled = enabled;
            textBoxCauTrucPhanTu.Enabled = enabled;
            textBoxTinhChatVatLy.Enabled = enabled;
            textBoxMoTa.Enabled = enabled;
            textBoxBaoQuan.Enabled = enabled;
            textBoxTLTK.Enabled = enabled;
            buttonSua.Enabled = enabled;
            buttonXoatrang.Enabled = enabled;
            // textBoxChatLienQuan ReadOnly
        }
        private void ClearTextBoxes()
        {
            textBoxCongThucHoaHoc.Clear();
            textBoxKhoiLuongPhanTu.Clear();
            textBoxCauTrucPhanTu.Clear();
            textBoxTinhChatVatLy.Clear();
            textBoxMoTa.Clear();
            textBoxBaoQuan.Clear();
            textBoxTLTK.Clear();
            textBoxChatLienQuan.Clear();
            _idDangChon = -1;
        }
        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            KhoiLuongPhanTu = values.Length > 5 ? values[5].Trim().Trim('"') : "",
                            CauTrucPhanTu = values.Length > 6 ? values[6].Trim().Trim('"') : "",
                            TinhChatVatLy = values.Length > 7 ? values[7].Trim().Trim('"') : "",
                            MoTa = values.Length > 8 ? values[8].Trim().Trim('"') : "",
                            BaoQuan = values.Length > 9 ? values[9].Trim().Trim('"') : "",
                            TLTK = values.Length > 11 ? values[11].Trim().Trim('"') : "",
                            UngDung = values.Length > 12 ? values[12].Trim().Trim('"') : "",
                            TuongKy = values.Length > 13 ? values[13].Trim().Trim('"') : "",
                            NgayTao = DateTime.Now,
                            NgayCapNhat = DateTime.Now
                        };
                        listThanhPhan.Add(tp);
                    }
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row.Cells["IDThanhphan"].Value == null) return;

            int id = int.Parse(row.Cells["IDThanhphan"].Value.ToString());
            _idDangChon = id;

            try
            {
                ThanhPhan tp = getdata.GetThanhPhan(id);
                if (tp == null) return;
                textBoxIDThanhphan.Text = row.Cells["IDThanhphan"].Value != null
                    ? row.Cells["IDThanhphan"].Value.ToString()
                    : "";
                textBoxCongThucHoaHoc.Text = tp.CongThucHoaHoc;
                textBoxKhoiLuongPhanTu.Text = tp.KhoiLuongPhanTu;
                textBoxCauTrucPhanTu.Text = tp.CauTrucPhanTu;
                textBoxTinhChatVatLy.Text = tp.TinhChatVatLy;
                textBoxMoTa.Text = tp.MoTa;
                textBoxBaoQuan.Text = tp.BaoQuan;
                textBoxTLTK.Text = tp.TLTK;
                textBoxUngDung.Text = tp.UngDung;
                textBoxTuongKy.Text = tp.TuongKy;
                textBoxNgayTao.Text = tp.NgayTao != null
                    ? tp.NgayTao.Value.ToString("dd/MM/yyyy HH:mm:ss")
                    : "";
                textBoxNgayCapNhat.Text = tp.NgayCapNhat != null
                    ? tp.NgayCapNhat.Value.ToString("dd/MM/yyyy HH:mm:ss")
                    : "";

                List<ThanhPhan> dsCLQ = getdata.GetThanhPhanLienQuan(id);
                textBoxChatLienQuan.Text = string.Join("; ", dsCLQ.Select(x => x.Ten_INN));

                SetPanelEnabled(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSua_Click(object sender, EventArgs e)
        {
            if (_idDangChon < 0)
            {
                MessageBox.Show("Vui lòng chọn thành phần cần cập nhật!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double khoiLuong = 0;
                if (!string.IsNullOrWhiteSpace(textBoxKhoiLuongPhanTu.Text))
                {
                    if (!double.TryParse(textBoxKhoiLuongPhanTu.Text, out khoiLuong))
                    {
                        MessageBox.Show("Khối lượng phân tử phải là số!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxKhoiLuongPhanTu.Focus();
                        return;
                    }
                }

                ThanhPhan current = getdata.GetThanhPhan(_idDangChon);
                if (current == null) return;

                if (updatedata.UpdateThanhPhan(
                    _idDangChon,
                    current.Ten_INN,                          // giữ nguyên
                    current.Ten_INCI,                         // giữ nguyên
                    current.Ten_IUPAC,                        // giữ nguyên
                    current.CAS_No,                           // giữ nguyên
                    textBoxCongThucHoaHoc.Text.Trim(),        
                    textBoxKhoiLuongPhanTu.Text.Trim(),                                
                    textBoxCauTrucPhanTu.Text.Trim(),         
                    textBoxTinhChatVatLy.Text.Trim(),         
                    textBoxMoTa.Text.Trim(),                  
                    textBoxBaoQuan.Text.Trim(),               
                    textBoxTLTK.Text.Trim(),
                    textBoxUngDung.Text.Trim(),
                    textBoxTuongKy.Text.Trim()

                ))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            SetPanelEnabled(false);
            dataGridView1.ClearSelection();
        }
    }
}
