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
    public partial class Import_ThanhPhan_ChucNangCosing : Form
    {
        private List<ThanhPhanCosing> _listThanhPhanCosing;
        private List<ChucNangCosing> _listChucNangCosing;
        private List<ThanhPhan_ChucNangCosing> _listLienKet;
        private List<ThanhPhan_ChucNangCosing> _listTong;

        BindingSource grid1 = new BindingSource();
        BindingSource gridTong = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        public Import_ThanhPhan_ChucNangCosing()
        {
            InitializeComponent();
        }

        private void Import_ThanhPhan_ChucNangCosing_Load(object sender, EventArgs e)
        {
            LoadThanhPhanCosing();
            LoadChucNangCosing();
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

                StringBuilder errors = new StringBuilder();
                int errorCount = 0;

                for (int i = 0; i < _listLienKet.Count; i++)
                {
                    ThanhPhan_ChucNangCosing item = _listLienKet[i];

                    if (!_listThanhPhanCosing.Any(tp => tp.IDThanhphan_Cosing == item.IDThanhphan_Cosing))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDThanhphan " + item.IDThanhphan_Cosing.ToString() + " không tồn tại");
                        errorCount++;
                    }

                    if (!_listChucNangCosing.Any(cn => cn.IDChucnangcosing == item.IDChucnangcosing))
                    {
                        errors.AppendLine("Dòng " + (i + 1).ToString() + ": IDChucnangcosing " + item.IDChucnangcosing.ToString() + " không tồn tại");
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
                    "Xác nhận import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (bulkInsert.BulkInsertThanhPhan_ChucNangCosing(_listLienKet))
                    {
                        MessageBox.Show("Import thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _listLienKet.Clear();
                        grid1.DataSource = null;
                        grid1.DataSource = _listLienKet;

                        if (_listTong != null && _listTong.Count > 0)
                            buttonGetTong_Click(null, null);
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
                    LoadListLienKet();
                    ImportFromCSV(filePath);
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
            _listLienKet = new List<ThanhPhan_ChucNangCosing>();
        }

        private void DisplayImportedData()
        {
            var displayList = _listLienKet.Select(l => new
            {
                IDThanhphan = l.IDThanhphan_Cosing,
                TenThanhPhan = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                CAS_No = GetCASNo(l.IDThanhphan_Cosing),
                IDChucnangcosing = l.IDChucnangcosing,
                TenChucNangCosing = GetTenChucNangCosing(l.IDChucnangcosing),
                MoTaChucNangCosing = GetMoTaChucNangCosing(l.IDChucnangcosing)
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
                    if (isFirstRow) { isFirstRow = false; continue; }
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        string[] values = line.Split(',');
                        if (values.Length >= 2 &&
                            !string.IsNullOrWhiteSpace(values[0]) &&
                            !string.IsNullOrWhiteSpace(values[1]))
                        {
                            ThanhPhan_ChucNangCosing item = new ThanhPhan_ChucNangCosing();
                            item.IDThanhphan_Cosing = Convert.ToInt32(values[0].Trim().Trim('"'));
                            item.IDChucnangcosing = Convert.ToInt32(values[1].Trim().Trim('"'));
                            _listLienKet.Add(item);
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

        private void LoadThanhPhanCosing()
        {
            _listThanhPhanCosing = getdata.GetDSThanhPhanCosing().OrderBy(tp => tp.Ten_INCI).ToList();
        }

        private void LoadChucNangCosing()
        {
            _listChucNangCosing = getdata.GetDSChucNangCosing().OrderBy(cn => cn.Tenchucnangcosing).ToList();
        }

        private string GetTenThanhPhanCosing(int id)
        {
            ThanhPhanCosing tp = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tp != null ? tp.Ten_INCI : "(Không tìm thấy ID: " + id.ToString() + ")";
        }

        private string GetCASNo(int id)
        {
            ThanhPhanCosing tp = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tp != null ? tp.CAS_No : "";
        }

        private string GetTenChucNangCosing(int id)
        {
            ChucNangCosing cn = _listChucNangCosing.FirstOrDefault(c => c.IDChucnangcosing == id);
            return cn != null ? cn.Tenchucnangcosing : "(Không tìm thấy ID: " + id.ToString() + ")";
        }

        private string GetMoTaChucNangCosing(int id)
        {
            ChucNangCosing cn = _listChucNangCosing.FirstOrDefault(c => c.IDChucnangcosing == id);
            return cn != null ? cn.Motachucnangcosing : "";
        }

        private void buttonGetTong_Click(object sender, EventArgs e)
        {
            try
            {
                _listTong = getdata.GetDSThanhPhan_ChucNangCosing();

                if (_listTong == null || _listTong.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong cơ sở dữ liệu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridTong.DataSource = null;
                    return;
                }

                var displayList = _listTong.Select(l => new
                {
                    IDThanhphan_Cosing = l.IDThanhphan_Cosing,
                    TenThanhPhanCosing = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                    CAS_No = GetCASNo(l.IDThanhphan_Cosing),
                    IDChucnangcosing = l.IDChucnangcosing,
                    TenChucNangCosing = GetTenChucNangCosing(l.IDChucnangcosing),
                    MoTaChucNangCosing = GetMoTaChucNangCosing(l.IDChucnangcosing)
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
                    FileName = "ThanhPhan_ChucNangCosing_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        sw.WriteLine("IDThanhphan_Cosing,TenThanhPhanCosing,CAS_No,IDChucnangcosing,TenChucNangCosing,MoTaChucNangCosing");

                        foreach (ThanhPhan_ChucNangCosing item in _listTong)
                        {
                            string tenTP = GetTenThanhPhanCosing(item.IDThanhphan_Cosing);
                            string casTP = GetCASNo(item.IDThanhphan_Cosing);
                            string tenCN = GetTenChucNangCosing(item.IDChucnangcosing);
                            string motaCN = GetMoTaChucNangCosing(item.IDChucnangcosing);

                            sw.WriteLine(string.Format("{0},\"{1}\",\"{2}\",{3},\"{4}\",\"{5}\"",
                                item.IDThanhphan_Cosing,
                                tenTP.Replace("\"", "\"\""),
                                casTP.Replace("\"", "\"\""),
                                item.IDChucnangcosing,
                                tenCN.Replace("\"", "\"\""),
                                motaCN.Replace("\"", "\"\"")));
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