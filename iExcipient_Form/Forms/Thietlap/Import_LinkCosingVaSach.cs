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
    public partial class Import_LinkCosingVaSach : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private List<ThanhPhanCosing> _listThanhPhanCosing;
        private List<LinkCosingVaSach> _listLienKet;
        private List<LinkCosingVaSach> _listTong;

        BindingSource grid1 = new BindingSource();
        BindingSource gridTong = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        public Import_LinkCosingVaSach()
        {
            InitializeComponent();
        }

        private void Import_LinkCosingVaSach_Load(object sender, EventArgs e)
        {
            LoadThanhPhan();
            LoadThanhPhanCosing();
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
                    LinkCosingVaSach item = _listLienKet[i];

                    // Check if ThanhPhan exists
                    if (!_listThanhPhan.Any(tp => tp.IDThanhphan == item.IDThanhphan))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDThanhphan " + item.IDThanhphan.ToString() + " không tồn tại");
                        errorCount++;
                    }

                    // Check if ThanhPhanCosing exists
                    if (!_listThanhPhanCosing.Any(tc => tc.IDThanhphan_Cosing == item.IDThanhphan_Cosing))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDThanhphan_Cosing " + item.IDThanhphan_Cosing.ToString() + " không tồn tại");
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
                    if (bulkInsert.BulkInsertLinkCosingVaSach(_listLienKet))
                    {
                        MessageBox.Show("Import thành công!", "Thông báo",
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
                        MessageBox.Show("Import thất bại hoặc tất cả đã tồn tại!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _listLienKet = new List<LinkCosingVaSach>();
        }

        private void DisplayImportedData()
        {
            var displayList = _listLienKet.Select(l => new
            {
                IDThanhphan = l.IDThanhphan,
                TenThanhPhan = GetTenThanhPhan(l.IDThanhphan),
                CAS_No = GetCASNo(l.IDThanhphan),
                IDThanhphan_Cosing = l.IDThanhphan_Cosing,
                TenThanhPhanCosing = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                CAS_No_Cosing = GetCASNoCosing(l.IDThanhphan_Cosing)
            }).ToList();

            grid1.DataSource = null;
            grid1.DataSource = displayList;
            dataGridView1.AutoResizeColumns();
        }

        private void ImportFromCSV(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
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
                            LinkCosingVaSach lk = new LinkCosingVaSach();
                            lk.IDThanhphan = Convert.ToInt32(values[0].Trim().Trim('"'));
                            lk.IDThanhphan_Cosing = Convert.ToInt32(values[1].Trim().Trim('"'));
                            _listLienKet.Add(lk);
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

        //Tạm load n hàng để tránh đơ app
        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhanTop(5000).OrderBy(tp => tp.Ten_INCI).ToList();
        }

        private void LoadThanhPhanCosing()
        {
            _listThanhPhanCosing = getdata.GetDSThanhPhanCosingTop(30100).OrderBy(tc => tc.Ten_INCI).ToList();
        }

        private string GetTenThanhPhan(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.Ten_INCI : "(Không tìm thấy ID: " + id.ToString() + ")";
        }

        private string GetCASNo(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.CAS_No : "";
        }

        private string GetTenThanhPhanCosing(int id)
        {
            ThanhPhanCosing tpc = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tpc != null ? tpc.Ten_INCI : "(Không tìm thấy ID: " + id.ToString() + ")";
        }

        private string GetCASNoCosing(int id)
        {
            ThanhPhanCosing tpc = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tpc != null ? tpc.CAS_No : "";
        }

        private void buttonGetTong_Click(object sender, EventArgs e)
        {
            try
            {
                // Get all existing relationships from database
                _listTong = getdata.GetDSLinkCosingVaSach();

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
                    IDThanhphan_Cosing = l.IDThanhphan_Cosing,
                    TenThanhPhanCosing = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                    CAS_No_Cosing = GetCASNoCosing(l.IDThanhphan_Cosing)
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
                    FileName = "LinkCosingVaSach_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.Default))
                    {
                        // Write header
                        sw.WriteLine("IDThanhphan,TenThanhPhan,CAS_No,IDThanhphan_Cosing,TenThanhPhanCosing,CAS_No_Cosing");

                        // Write data
                        foreach (LinkCosingVaSach item in _listTong)
                        {
                            string tenTP = GetTenThanhPhan(item.IDThanhphan);
                            string casTP = GetCASNo(item.IDThanhphan);
                            string tenTPC = GetTenThanhPhanCosing(item.IDThanhphan_Cosing);
                            string casTPC = GetCASNoCosing(item.IDThanhphan_Cosing);

                            sw.WriteLine(string.Format("{0},\"{1}\",\"{2}\",{3},\"{4}\",\"{5}\"",
                                item.IDThanhphan,
                                tenTP.Replace("\"", "\"\""),
                                casTP.Replace("\"", "\"\""),
                                item.IDThanhphan_Cosing,
                                tenTPC.Replace("\"", "\"\""),
                                casTPC.Replace("\"", "\"\"")));
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