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
    public partial class DanhMucChucNang : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();


        public DanhMucChucNang()
        {
            InitializeComponent();
        }

        private void DanhMucChucNang_Load(object sender, EventArgs e)
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
            textBoxIDChucNang.Clear();
            textBoxChucNang.Clear();
            textBoxMoTa.Clear();
            textBoxChucNang.Focus();
        }

        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = true;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }

        private void textBoxChucNang_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxChucNang.Text);
        }

        private void refreshDatagrid()
        {
            grid1.DataSource = getdata.GetDSChucNang();
            dataGridView1.AutoResizeColumns();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked row is valid (not header row)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate textboxes with selected row data
                textBoxIDChucNang.Text = row.Cells["IDChucnang"].Value.ToString();
                textBoxChucNang.Text = row.Cells["Tenchucnang"].Value.ToString();
                textBoxMoTa.Text = row.Cells["Motachucnang"].Value.ToString();

                // Enable buttons after selection
                buttonXoa.Enabled = true;
                buttonSua.Enabled = true;
            }
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(textBoxChucNang.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên chức năng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxChucNang.Focus();
                    return;
                }

                ChucNang item = new ChucNang
                {
                    Tenchucnang = textBoxChucNang.Text.Trim(),
                    Motachucnang = textBoxMoTa.Text.Trim()
                };

                // Insert new record using KetnoiDB.InsertData
                if (insertdata.InsertChucNang(item))
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại! Chức năng có thể đã tồn tại.", "Lỗi",
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
                // Validate ID
                if (string.IsNullOrWhiteSpace(textBoxIDChucNang.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức năng cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idChucNang = int.Parse(textBoxIDChucNang.Text);
                string tenChucNang = textBoxChucNang.Text.Trim();

                // BƯỚC 1: Kiểm tra có bao nhiêu thành phần đang dùng chức năng này
                int soLuongQuanHe = deletedata.GetRelatedCountChucNang(idChucNang);

                // BƯỚC 2: Tạo thông báo phù hợp
                string confirmMessage = "";
                MessageBoxIcon icon = MessageBoxIcon.Question;

                if (soLuongQuanHe > 0)
                {
                    // Có quan hệ - cảnh báo nghiêm trọng hơn
                    confirmMessage = string.Format(
                        "CẢNH BÁO: Chức năng '{0}' đang được sử dụng bởi {1} thành phần.\n\n" +
                        "Nếu xóa, tất cả {1} quan hệ này sẽ BỊ XÓA VĨNH VIỄN.\n\n" +
                        "Bạn có CHẮC CHẮN muốn xóa?",
                        tenChucNang,
                        soLuongQuanHe
                    );
                    icon = MessageBoxIcon.Warning;
                }
                else
                {
                    // Không có quan hệ - xác nhận bình thường
                    confirmMessage = string.Format(
                        "Bạn có chắc chắn muốn xóa chức năng '{0}'?",
                        tenChucNang
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

                // BƯỚC 4: Thực hiện xóa nếu user chọn Yes
                if (confirm == DialogResult.Yes)
                {
                    if (deletedata.DeleteChucNang(idChucNang))
                    {
                        string successMsg = soLuongQuanHe > 0
                            ? string.Format("Đã xóa chức năng '{0}' và {1} quan hệ liên quan!", tenChucNang, soLuongQuanHe)
                            : string.Format("Đã xóa chức năng '{0}' thành công!", tenChucNang);

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
                if (string.IsNullOrWhiteSpace(textBoxIDChucNang.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức năng cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(textBoxChucNang.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên chức năng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxChucNang.Focus();
                    return;
                }

                // Update record using KetnoiDB.UpdateData
                int idChucNang = int.Parse(textBoxIDChucNang.Text);

                if (updatedata.UpdateChucNang(idChucNang, textBoxChucNang.Text.Trim(), textBoxMoTa.Text.Trim()))
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
                    Title = "Chọn file để import (Cột 1: TenChucNang, Cột 2: MoTaChucNang)"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    List<ChucNang> listChucNang = new List<ChucNang>();

                    ImportFromCSV(filePath, listChucNang);

                    if (listChucNang.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listChucNang.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertChucNang(listChucNang))
                            {
                                MessageBox.Show("Import thành công " + listChucNang.Count.ToString() + " bản ghi!", "Thông báo",
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
        private void ImportFromCSV(string filePath, List<ChucNang> listChucNang)
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

                    // Skip header row
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    if (values.Length >= 1 && !string.IsNullOrWhiteSpace(values[0]))
                    {
                        ChucNang cn = new ChucNang
                        {
                            Tenchucnang = values[0].Trim(),
                            Motachucnang = values.Length > 1 ? values[1].Trim() : ""
                        };
                        listChucNang.Add(cn);
                    }
                }
            }
        }

    }
}
