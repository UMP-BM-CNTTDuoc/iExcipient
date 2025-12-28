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
    public partial class DanhMucDangBaoChe : Form
    {
        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.UpdateData updatedata = new KetnoiDB.UpdateData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();
        KetnoiDB.BulkInsertData bulkInsert = new KetnoiDB.BulkInsertData();
        public DanhMucDangBaoChe()
        {
            InitializeComponent();
        }

        private void DanhMucDangBaoChe_Load(object sender, EventArgs e)
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
            textBoxIDDangBaoChe.Clear();
            textBoxDangBaoChe.Clear();
            textBoxDangBaoChe.Focus();
        }

        private void refreshDatagrid()
        {
            grid1.DataSource = getdata.GetDSDangBaoChe();
            dataGridView1.AutoResizeColumns();
        }
        private void buttonXoatrang_Click(object sender, EventArgs e)
        {
            buttonImport.Enabled = true;
            buttonXoa.Enabled = false;
            buttonSua.Enabled = false;
            ClearTextBoxes();
        }

        private void textBoxDangBaoChe_TextChanged(object sender, EventArgs e)
        {
            buttonThem.Enabled = !string.IsNullOrWhiteSpace(textBoxDangBaoChe.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked row is valid (not header row)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate textboxes with selected row data
                textBoxIDDangBaoChe.Text = row.Cells["IDDangbaoche"].Value.ToString();
                textBoxDangBaoChe.Text = row.Cells["TenDangbaoche"].Value.ToString();

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
                if (string.IsNullOrWhiteSpace(textBoxDangBaoChe.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên dạng bào chế!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDangBaoChe.Focus();
                    return;
                }

                DangBaoChe item = new DangBaoChe
                {
                    TenDangbaoche = textBoxDangBaoChe.Text.Trim()
                };

                // Insert new record using KetnoiDB.InsertData
                if (insertdata.InsertDangBaoChe(item))
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTextBoxes();
                    refreshDatagrid();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại! Dạng bào chế có thể đã tồn tại.", "Lỗi",
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
                if (string.IsNullOrWhiteSpace(textBoxIDDangBaoChe.Text))
                {
                    MessageBox.Show("Vui lòng chọn dạng bào chế cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idDangBaoChe = int.Parse(textBoxIDDangBaoChe.Text);
                string tenDangBaoChe = textBoxDangBaoChe.Text.Trim();

                // BƯỚC 1: Kiểm tra có bao nhiêu thành phần đang dùng dạng bào chế này
                int soLuongQuanHe = deletedata.GetRelatedCountDangBaoChe(idDangBaoChe);

                // BƯỚC 2: Tạo thông báo phù hợp
                string confirmMessage = "";
                MessageBoxIcon icon = MessageBoxIcon.Question;

                if (soLuongQuanHe > 0)
                {
                    // Có quan hệ - cảnh báo nghiêm trọng hơn
                    confirmMessage = string.Format(
                        "CẢNH BÁO: Dạng bào chế '{0}' đang được sử dụng bởi {1} thành phần.\n\n" +
                        "Nếu xóa, tất cả {1} quan hệ này sẽ BỊ XÓA VĨNH VIỄN.\n\n" +
                        "Bạn có CHẮC CHẮN muốn xóa?",
                        tenDangBaoChe,
                        soLuongQuanHe
                    );
                    icon = MessageBoxIcon.Warning;
                }
                else
                {
                    // Không có quan hệ - xác nhận bình thường
                    confirmMessage = string.Format(
                        "Bạn có chắc chắn muốn xóa dạng bào chế '{0}'?",
                        tenDangBaoChe
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
                    if (deletedata.DeleteDangBaoChe(idDangBaoChe))
                    {
                        string successMsg = soLuongQuanHe > 0
                            ? string.Format("Đã xóa dạng bào chế '{0}' và {1} quan hệ liên quan!", tenDangBaoChe, soLuongQuanHe)
                            : string.Format("Đã xóa dạng bào chế '{0}' thành công!", tenDangBaoChe);

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
                if (string.IsNullOrWhiteSpace(textBoxIDDangBaoChe.Text))
                {
                    MessageBox.Show("Vui lòng chọn dạng bào chế cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(textBoxDangBaoChe.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên dạng bào chế!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxDangBaoChe.Focus();
                    return;
                }

                // Update record using KetnoiDB.UpdateData
                int idDangBaoChe = int.Parse(textBoxIDDangBaoChe.Text);

                if (updatedata.UpdateDangBaoChe(idDangBaoChe, textBoxDangBaoChe.Text.Trim()))
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
                    Title = "Chọn file để import (Cột 1: TenDangBaoChe)"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    List<DangBaoChe> listDangBaoChe = new List<DangBaoChe>();

                    ImportFromCSV(filePath, listDangBaoChe);

                    if (listDangBaoChe.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Tìm thấy " + listDangBaoChe.Count.ToString() + " dòng dữ liệu. Bạn có muốn import?",
                            "Xác nhận import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            if (bulkInsert.BulkInsertDangBaoChe(listDangBaoChe))
                            {
                                MessageBox.Show("Import thành công " + listDangBaoChe.Count.ToString() + " bản ghi!", "Thông báo",
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

        private void ImportFromCSV(string filePath, List<DangBaoChe> listDangBaoChe)
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                bool isFirstRow = true;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    // Skip header row
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    string[] values = line.Split(',');

                    if (values.Length >= 1 && !string.IsNullOrWhiteSpace(values[0]))
                    {
                        DangBaoChe dbc = new DangBaoChe
                        {
                            TenDangbaoche = values[0].Trim().Trim('"')
                        };
                        listDangBaoChe.Add(dbc);
                    }
                }
            }
        }
    }
}
