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
    public partial class Import_ThanhPhan_ChatLienQuan : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private List<ChatLienQuan> _listLienKet;
        private List<ChatLienQuan> _listTong;

        BindingSource grid1 = new BindingSource();
        BindingSource gridTong = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        public Import_ThanhPhan_ChatLienQuan()
        {
            InitializeComponent();
        }

        private void Import_ThanhPhan_ChatLienQuan_Load(object sender, EventArgs e)
        {
            LoadThanhPhan();
            LoadListLienKet();

            dataGridView1.DataSource = grid1;
            dataGridViewTong.DataSource = gridTong;
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (_listLienKet == null || _listLienKet.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để import!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate data before import
                StringBuilder errors = new StringBuilder();
                int errorCount = 0;

                for (int i = 0; i < _listLienKet.Count; i++)
                {
                    ChatLienQuan item = _listLienKet[i];

                    // Check if ThanhPhan exists
                    if (!_listThanhPhan.Any(tp => tp.IDThanhphan == item.IDThanhphan))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDThanhphan " + item.IDThanhphan.ToString() + " không tồn tại");
                        errorCount++;
                    }

                    // Check if ThanhPhanLienQuan exists
                    if (!_listThanhPhan.Any(tp => tp.IDThanhphan == item.IDThanhphanLienquan))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDThanhphanLienquan " + item.IDThanhphanLienquan.ToString() + " không tồn tại");
                        errorCount++;
                    }

                    // Check if linking to itself
                    if (item.IDThanhphan == item.IDThanhphanLienquan)
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": Không thể liên kết Thành Phần với chính nó");
                        errorCount++;
                    }
                }

                if (errorCount > 0)
                {
                    MessageBox.Show("Tìm thấy " + errorCount.ToString() + " lỗi:\n\n" + errors.ToString(),
                        "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Tìm thấy " + _listLienKet.Count.ToString() + " dòng dữ liệu hợp lệ.\n\n" +
                    "Lưu ý: Các liên kết trùng lặp sẽ bị bỏ qua.\n\n" +
                    "Bạn có muốn import?",
                    "Xác nhận import",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (bulkInsert.BulkInsertChatLienQuan(_listLienKet))
                    {
                        MessageBox.Show("Import thành công " + _listLienKet.Count.ToString() + " bản ghi!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the import grid
                        _listLienKet.Clear();
                        grid1.DataSource = null;
                        grid1.DataSource = _listLienKet;

                        // Refresh the total grid if it's loaded
                        if (_listTong != null && _listTong.Count > 0)
                        {
                            buttonGetTong_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Import thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                    Title = "Chọn file để import"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Clear previous data
                    LoadListLienKet();

                    ImportFromCSV(filePath);

                    // Display imported data with names
                    DisplayImportedData();

                    MessageBox.Show("Đã đọc " + _listLienKet.Count.ToString() + " dòng dữ liệu từ file.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadListLienKet()
        {
            _listLienKet = new List<ChatLienQuan>();
        }

        private void DisplayImportedData()
        {
            var displayList = _listLienKet.Select(l => new
            {
                IDThanhphan = l.IDThanhphan,
                TenThanhPhan = GetTenThanhPhan(l.IDThanhphan),
                CAS_No = GetCASNo(l.IDThanhphan),
                IDThanhphanLienquan = l.IDThanhphanLienquan,
                TenThanhPhanLienQuan = GetTenThanhPhan(l.IDThanhphanLienquan),
                CAS_No_LienQuan = GetCASNo(l.IDThanhphanLienquan)
            }).ToList();

            grid1.DataSource = null;
            grid1.DataSource = displayList;
            dataGridView1.AutoResizeColumns();
        }

        private void ImportFromCSV(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                bool isFirstRow = true;
                int lineNumber = 0;

                while (!sr.EndOfStream)
                {
                    lineNumber++;
                    string line = sr.ReadLine();

                    // Skip header row
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        string[] values = line.Split(',');

                        if (values.Length >= 2 &&
                            !string.IsNullOrWhiteSpace(values[0]) &&
                            !string.IsNullOrWhiteSpace(values[1]))
                        {
                            ChatLienQuan clq = new ChatLienQuan();
                            clq.IDThanhphan = Convert.ToInt32(values[0].Trim().Trim('"'));
                            clq.IDThanhphanLienquan = Convert.ToInt32(values[1].Trim().Trim('"'));
                            _listLienKet.Add(clq);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi tại dòng " + lineNumber.ToString() + ": " + ex.Message,
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhan().OrderBy(tp => tp.Ten_INN).ToList();
        }

        private string GetTenThanhPhan(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.Ten_INN : "(Không tìm thấy ID: " + id.ToString() + ")";
        }

        private string GetCASNo(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.CAS_No : "";
        }

        private void buttonGetTong_Click(object sender, EventArgs e)
        {
            try
            {
                // Get all existing relationships from database
                _listTong = getdata.GetDSChatLienQuan();

                if (_listTong == null || _listTong.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong cơ sở dữ liệu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridTong.DataSource = null;
                    return;
                }

                // Display with names
                var displayList = _listTong.Select(l => new
                {
                    IDThanhphan = l.IDThanhphan,
                    TenThanhPhan = GetTenThanhPhan(l.IDThanhphan),
                    CAS_No = GetCASNo(l.IDThanhphan),
                    IDThanhphanLienquan = l.IDThanhphanLienquan,
                    TenThanhPhanLienQuan = GetTenThanhPhan(l.IDThanhphanLienquan),
                    CAS_No_LienQuan = GetCASNo(l.IDThanhphanLienquan)
                }).ToList();

                gridTong.DataSource = null;
                gridTong.DataSource = displayList;
                dataGridViewTong.AutoResizeColumns();

                MessageBox.Show("Đã tải " + _listTong.Count.ToString() + " bản ghi từ cơ sở dữ liệu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExportCSVTong_Click(object sender, EventArgs e)
        {
            try
            {
                if (_listTong == null || _listTong.Count == 0)
                {
                    MessageBox.Show("Vui lòng nhấn 'Lấy dữ liệu tổng' trước khi xuất file.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    Title = "Lưu file CSV",
                    FileName = "ThanhPhan_ChatLienQuan_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        // Write header
                        sw.WriteLine("IDThanhphan,TenThanhPhan,CAS_No,IDThanhphanLienquan,TenThanhPhanLienQuan,CAS_No_LienQuan");

                        // Write data
                        foreach (ChatLienQuan item in _listTong)
                        {
                            string tenTP = GetTenThanhPhan(item.IDThanhphan);
                            string casTP = GetCASNo(item.IDThanhphan);
                            string tenTPLQ = GetTenThanhPhan(item.IDThanhphanLienquan);
                            string casTPLQ = GetCASNo(item.IDThanhphanLienquan);

                            sw.WriteLine(string.Format("{0},\"{1}\",\"{2}\",{3},\"{4}\",\"{5}\"",
                                item.IDThanhphan,
                                tenTP.Replace("\"", "\"\""), // Escape quotes
                                casTP.Replace("\"", "\"\""),
                                item.IDThanhphanLienquan,
                                tenTPLQ.Replace("\"", "\"\""),
                                casTPLQ.Replace("\"", "\"\"")));
                        }
                    }

                    MessageBox.Show("Đã xuất " + _listTong.Count.ToString() + " bản ghi ra file CSV!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}