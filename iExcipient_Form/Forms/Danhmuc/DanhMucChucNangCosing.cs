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
    public partial class DanhMucChucNangCosing : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();

        public DanhMucChucNangCosing()
        {
            InitializeComponent();
        }

        private void DanhMucChucNangCosing_Load(object sender, EventArgs e)
        {
            buttonThem.Enabled = false;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            dataGridView1.DataSource = grid1;
            refreshDatagrid();
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            textBoxIDChucNangCosing.Clear();
            textBoxChucNangCosing.Clear();
            textBoxMoTa.Clear();
            textBoxChucNangCosing.Focus();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = true;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }

        private void textBoxChucNangCosing_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxChucNangCosing.Text);
        }

        private void refreshDatagrid()
        {
            grid1.DataSource = getdata.GetDSChucNangCosing();
            dataGridView1.AutoResizeColumns();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxIDChucNangCosing.Text = row.Cells["IDChucnangcosing"].Value.ToString();
                textBoxChucNangCosing.Text = row.Cells["Tenchucnangcosing"].Value.ToString();
                textBoxMoTa.Text = row.Cells["Motachucnangcosing"].Value.ToString();
                buttonXoa.Enabled = true;
                buttonSua.Enabled = true;
            }
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxChucNangCosing.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên chức năng cosing!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxChucNangCosing.Focus();
                    return;
                }

                ChucNangCosing item = new ChucNangCosing
                {
                    Tenchucnangcosing = textBoxChucNangCosing.Text.Trim(),
                    Motachucnangcosing = textBoxMoTa.Text.Trim()
                };

                if (insertdata.InsertChucNangCosing(item))
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại! Chức năng cosing có thể đã tồn tại.", "Lỗi",
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
                if (string.IsNullOrWhiteSpace(textBoxIDChucNangCosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức năng cosing cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idChucNangCosing = int.Parse(textBoxIDChucNangCosing.Text);
                string tenChucNangCosing = textBoxChucNangCosing.Text.Trim();

                int soLuongQuanHe = deletedata.GetRelatedCountChucNangCosing(idChucNangCosing);

                string confirmMessage = "";
                MessageBoxIcon icon = MessageBoxIcon.Question;

                if (soLuongQuanHe > 0)
                {
                    confirmMessage = string.Format(
                        "CẢNH BÁO: Chức năng cosing '{0}' đang được sử dụng bởi {1} thành phần.\n\n" +
                        "Nếu xóa, tất cả {1} quan hệ này sẽ BỊ XÓA VĨNH VIỄN.\n\n" +
                        "Bạn có CHẮC CHẮN muốn xóa?",
                        tenChucNangCosing, soLuongQuanHe);
                    icon = MessageBoxIcon.Warning;
                }
                else
                {
                    confirmMessage = string.Format(
                        "Bạn có chắc chắn muốn xóa chức năng cosing '{0}'?",
                        tenChucNangCosing);
                }

                DialogResult confirm = MessageBox.Show(confirmMessage, "Xác nhận xóa",
                    MessageBoxButtons.YesNo, icon);

                if (confirm == DialogResult.Yes)
                {
                    if (deletedata.DeleteChucNangCosing(idChucNangCosing))
                    {
                        string successMsg = soLuongQuanHe > 0
                            ? string.Format("Đã xóa chức năng cosing '{0}' và {1} quan hệ liên quan!", tenChucNangCosing, soLuongQuanHe)
                            : string.Format("Đã xóa chức năng cosing '{0}' thành công!", tenChucNangCosing);

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
                if (string.IsNullOrWhiteSpace(textBoxIDChucNangCosing.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức năng cosing cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxChucNangCosing.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên chức năng cosing!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxChucNangCosing.Focus();
                    return;
                }

                int idChucNangCosing = int.Parse(textBoxIDChucNangCosing.Text);

                if (updatedata.UpdateChucNangCosing(idChucNangCosing, textBoxChucNangCosing.Text.Trim(), textBoxMoTa.Text.Trim()))
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
                    Title = "Chọn file để import (Cột 1: TenChucNangCosing, Cột 2: MoTaChucNangCosing)"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    List<ChucNangCosing> listChucNangCosing = new List<ChucNangCosing>();
                    ImportFromCSV(filePath, listChucNangCosing);

                    if (listChucNangCosing.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listChucNangCosing.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertChucNangCosing(listChucNangCosing))
                            {
                                MessageBox.Show("Import thành công " + listChucNangCosing.Count.ToString() + " bản ghi!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ImportFromCSV(string filePath, List<ChucNangCosing> listChucNangCosing)
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
                    if (isFirstRow) { isFirstRow = false; continue; }

                    if (values.Length >= 1 && !string.IsNullOrWhiteSpace(values[0]))
                    {
                        ChucNangCosing cn = new ChucNangCosing
                        {
                            Tenchucnangcosing = values[0].Trim(),
                            Motachucnangcosing = values.Length > 1 ? values[1].Trim() : ""
                        };
                        listChucNangCosing.Add(cn);
                    }
                }
            }
        }
    }
}