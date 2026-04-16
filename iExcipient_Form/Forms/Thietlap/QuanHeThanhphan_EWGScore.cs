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
    public partial class QuanHeThanhphan_EWGScore : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private int _idDangChon = -1;
        private bool _daCoEWG = false;

        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        public QuanHeThanhphan_EWGScore()
        {
            InitializeComponent();
        }



        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QuanHeThanhphan_EWGScore_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = grid1;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            LoadThanhPhan();
            comboBoxThanhPhan.SelectedIndex = -1;

            RefreshDatagrid();
        }
        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhan().OrderBy(tp => tp.Ten_INN).ToList();

            comboBoxThanhPhan.DataSource = _listThanhPhan.ToList();
            comboBoxThanhPhan.DisplayMember = "Ten_INN";
            comboBoxThanhPhan.ValueMember = "IDThanhphan";

            textBoxScoreFrom.Left = label5.Right + 10;
            label6.Left = textBoxScoreFrom.Right + 10;
            textBoxScoreTo.Left = label6.Right + 10;
        }
        private void RefreshDatagrid()
        {
            try
            {
                List<Thanhphan_EWGScore> ds = getdata.GetDSEWGScore();

                var displayList = ds.Select(e2 => new
                {
                    e2.IDThanhphan,
                    TenThanhPhan = GetTenThanhPhan(e2.IDThanhphan),
                    EWG_Score_from = e2.EWG_Score_from,
                    EWG_Score_to = e2.EWG_Score_to,
                    DoAnToan = e2.EWG_Score,
                    MucDoBangChung = e2.EWG_DataAvailability
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
        private void comboBoxThanhPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxThanhPhan.SelectedValue == null ||
                    !(comboBoxThanhPhan.SelectedValue is int))
                    return;

                int id = (int)comboBoxThanhPhan.SelectedValue;
                _idDangChon = id;
                textBoxIDThanhphan.Text = id.ToString();

                // Load EWG Score của thành phần đó
                Thanhphan_EWGScore ewg = getdata.GetEWGScoreByThanhPhan(id);
                if (ewg != null)
                {
                    _daCoEWG = true;
                    textBoxScoreFrom.Text = ewg.EWG_Score_from.HasValue
                                                   ? ewg.EWG_Score_from.Value.ToString() : "";
                    textBoxScoreTo.Text = ewg.EWG_Score_to.HasValue
                                                   ? ewg.EWG_Score_to.Value.ToString() : "";
                    textBoxDataAvailability.Text = ewg.EWG_DataAvailability ?? "";
                    textBoxDoAnToan.Text = Thanhphan_EWGScore
                                                   .PhanLoaiDoAnToan(ewg.EWG_Score_to);
                }
                else
                {
                    // Chưa có bản ghi → xóa trắng
                    _daCoEWG = false;
                    textBoxScoreFrom.Text = "";
                    textBoxScoreTo.Text = "";
                    textBoxDoAnToan.Text = "";
                    textBoxDataAvailability.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearTextBoxes()
        {
            textBoxIDThanhphan.Clear();
            textBoxScoreFrom.Clear();
            textBoxScoreTo.Clear();
            textBoxDoAnToan.Clear();
            textBoxDataAvailability.Clear();
            _idDangChon = -1;
            _daCoEWG = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            // Lấy IDThanhphan từ grid để đồng bộ combobox
            if (row.Cells["IDThanhphan"].Value == null) return;

            int id;
            if (!int.TryParse(row.Cells["IDThanhphan"].Value.ToString(), out id)) return;

            // Đặt combobox — SelectedIndexChanged tự load dữ liệu
            comboBoxThanhPhan.SelectedValue = id;
        }
        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idDangChon < 0)
                {
                    MessageBox.Show("Vui lòng chọn thành phần!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? scoreFrom = null, scoreTo = null;
                int tmp;

                if (!string.IsNullOrWhiteSpace(textBoxScoreFrom.Text))
                {
                    if (!int.TryParse(textBoxScoreFrom.Text.Trim(), out tmp))
                    {
                        MessageBox.Show("Điểm an toàn từ phải là số nguyên!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxScoreFrom.Focus();
                        return;
                    }
                    scoreFrom = tmp;
                }

                if (!string.IsNullOrWhiteSpace(textBoxScoreTo.Text))
                {
                    if (!int.TryParse(textBoxScoreTo.Text.Trim(), out tmp))
                    {
                        MessageBox.Show("Điểm an toàn đến phải là số nguyên!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxScoreTo.Focus();
                        return;
                    }
                    scoreTo = tmp;
                }

                // Nếu chỉ nhập 1 ô thì tự đồng bộ
                if (scoreFrom.HasValue && !scoreTo.HasValue)
                {
                    scoreTo = scoreFrom;
                }
                else if (!scoreFrom.HasValue && scoreTo.HasValue)
                {
                    scoreFrom = scoreTo;
                }

                // Kiểm tra khoảng hợp lệ
                if (scoreFrom.HasValue && scoreTo.HasValue && scoreFrom.Value > scoreTo.Value)
                {
                    MessageBox.Show("Điểm từ không được lớn hơn điểm đến!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool ketqua;
                if (_daCoEWG)
                {
                    ketqua = updatedata.UpdateThanhphan_EWGScore(
                        _idDangChon,
                        scoreFrom,
                        scoreTo,
                        textBoxDoAnToan.Text.Trim(),
                        textBoxDataAvailability.Text.Trim());
                }
                else
                {
                    Thanhphan_EWGScore newEWG = new Thanhphan_EWGScore
                    {
                        IDThanhphan = _idDangChon,
                        EWG_Score_from = scoreFrom,
                        EWG_Score_to = scoreTo,
                        EWG_Score = textBoxDoAnToan.Text.Trim(),
                        EWG_DataAvailability = textBoxDataAvailability.Text.Trim()
                    };
                    ketqua = insertdata.InsertThanhPhanEWGScore(newEWG);
                }

                if (ketqua)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _daCoEWG = true;   // sau lần đầu insert, các lần sau là update
                    RefreshDatagrid();
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
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "CSV Files|*.csv",
                    Title = "Chọn file CSV để import EWG Score"
                };

                if (ofd.ShowDialog() != DialogResult.OK) return;

                List<Thanhphan_EWGScore> list = new List<Thanhphan_EWGScore>();
                ImportFromCSV(ofd.FileName, list);

                if (list.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu trong file!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Tìm thấy " + list.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                    "Xác nhận import",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (bulkInsert.BulkInsertThanhphan_EWGScore(list))
                    {
                        MessageBox.Show("Import thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDatagrid();
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
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ImportFromCSV(string filePath, List<Thanhphan_EWGScore> list)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                bool isFirstRow = true;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (isFirstRow) { isFirstRow = false; continue; }

                    string[] values = line.Split(',');
                    if (values.Length < 1) continue;

                    int idTP;
                    if (!int.TryParse(values[0].Trim().Trim('"'), out idTP)) continue;

                    int? from = null, to = null;
                    int tmpInt;
                    if (values.Length > 1 && int.TryParse(values[1].Trim().Trim('"'), out tmpInt))
                        from = tmpInt;
                    if (values.Length > 2 && int.TryParse(values[2].Trim().Trim('"'), out tmpInt))
                        to = tmpInt;

                    string dataAvail = values.Length > 3
                                       ? values[3].Trim().Trim('"') : "";

                    list.Add(new Thanhphan_EWGScore
                    {
                        IDThanhphan = idTP,
                        EWG_Score_from = from,
                        EWG_Score_to = to,
                        EWG_Score = Thanhphan_EWGScore.PhanLoaiDoAnToan(to),
                        EWG_DataAvailability = dataAvail
                    });
                }
            }
        }
        private string GetTenThanhPhan(int id)
        {
            if (_listThanhPhan == null) return "";
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.Ten_INN : "";
        }
        private void textBoxScoreTo_TextChanged(object sender, EventArgs e)
        {
            CapNhatDoAnToan();
        }
        private void textBoxScoreFrom_TextChanged(object sender, EventArgs e)
        {
            CapNhatDoAnToan();
        }
        private void CapNhatDoAnToan()
        {
            int val;

            if (int.TryParse(textBoxScoreTo.Text, out val))
            {
                textBoxDoAnToan.Text = Thanhphan_EWGScore.PhanLoaiDoAnToan(val);
            }
            else if (int.TryParse(textBoxScoreFrom.Text, out val))
            {
                textBoxDoAnToan.Text = Thanhphan_EWGScore.PhanLoaiDoAnToan(val);
            }
            else
            {
                textBoxDoAnToan.Text = "";
            }
        }
    }
}
