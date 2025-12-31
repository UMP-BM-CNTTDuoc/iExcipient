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
    public partial class QuanHeThanhPhan_ChatLienQuan : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private BindingList<ChatLienQuan> _listLienKet;
        ThanhPhan workingTP;

        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        private bool capnhat = false; // Flag to track unsaved changes
        public QuanHeThanhPhan_ChatLienQuan()
        {
            InitializeComponent();
        }

        private void QuanHeThanhPhan_ChatLienQuan_Load(object sender, EventArgs e)
        {

            LoadThanhPhan();
            LoadListLienKet();

            comboBoxThanhPhan.SelectedIndex = -1;
            comboBoxChatLienQuan.SelectedIndex = -1;

            LoadLinkedList();
        }
        private void buttonThoat_Click(object sender, EventArgs e)
        {
            if (capnhat)
            {
                DialogResult result = MessageBox.Show(
                    "Có thay đổi chưa được lưu. Bạn có muốn lưu trước khi thoát?",
                    "Xác nhận",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    buttonSua_Click(sender, e);
                    if (capnhat) return; // If save failed, don't close
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Don't close
                }
            }

            this.Close();
        }

        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (workingTP == null)
                {
                    MessageBox.Show("Vui lòng chọn Thành Phần trước.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                deletedata.DeleteThanhPhan_ChatLienQuan_ByThanhPhan(workingTP.IDThanhphan);

                foreach (ChatLienQuan i in _listLienKet)
                {
                    insertdata.InsertChatLienQuan(i);
                }

                capnhat = false; // Reset flag after successful save
                MessageBox.Show("Cập nhật thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (Forms.Thietlap.Import_ThanhPhan_ChatLienQuan formcon = new Forms.Thietlap.Import_ThanhPhan_ChatLienQuan())
            {
                formcon.ShowDialog();
                // Reload data after import
                LoadThanhPhan();
                if (workingTP != null)
                {
                    comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan;
                }
            }
        }

        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhan().OrderBy(tp => tp.Ten_INN).ToList();

            // Load for main component ComboBox
            comboBoxThanhPhan.DataSource = _listThanhPhan.ToList();
            comboBoxThanhPhan.DisplayMember = "Ten_INN";
            comboBoxThanhPhan.ValueMember = "IDThanhphan";

            // Load for related component ComboBox
            comboBoxChatLienQuan.DataSource = _listThanhPhan.ToList();
            comboBoxChatLienQuan.DisplayMember = "Ten_INN";
            comboBoxChatLienQuan.ValueMember = "IDThanhphan";
        }

        private void LoadListLienKet()
        {
            _listLienKet = new BindingList<ChatLienQuan>();
        }

        private void comboBoxThanhPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxThanhPhan.SelectedValue == null || !(comboBoxThanhPhan.SelectedValue is int))
                    return;

                // Check for unsaved changes before switching
                if (capnhat)
                {
                    DialogResult result = MessageBox.Show(
                        "Có thay đổi chưa được lưu. Bạn có muốn lưu trước khi chuyển sang Thành Phần khác?",
                        "Xác nhận",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        buttonSua_Click(sender, e);
                        if (capnhat) return; // If save failed, don't switch
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        // Revert to previous selection
                        if (workingTP != null)
                        {
                            comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan;
                        }
                        return;
                    }
                    else // DialogResult.No
                    {
                        capnhat = false; // Discard changes
                    }
                }

                int idThanhPhan = (int)comboBoxThanhPhan.SelectedValue;
                workingTP = getdata.GetThanhPhan(idThanhPhan);

                _listLienKet.Clear();

                if (workingTP != null && workingTP.dsThanhPhanLienQuan != null)
                {
                    foreach (ThanhPhan tpLienQuan in workingTP.dsThanhPhanLienQuan)
                    {
                        _listLienKet.Add(new ChatLienQuan
                        {
                            IDThanhphan = idThanhPhan,
                            IDThanhphanLienquan = tpLienQuan.IDThanhphan
                        });
                    }
                }

                LoadLinkedList();
                capnhat = false; // Reset flag after loading new data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLinkedList()
        {
            if (comboBoxThanhPhan.SelectedValue == null) return;

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
            dataGridView1.DataSource = grid1;
            dataGridView1.Refresh();
        }

        private string GetTenThanhPhan(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.Ten_INN : "";
        }

        private string GetCASNo(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.CAS_No : "";
        }

        private void buttonThemCLQ_Click(object sender, EventArgs e)
        {
            if (comboBoxThanhPhan.SelectedValue == null || comboBoxChatLienQuan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cả Thành Phần và Chất Liên Quan.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idThanhPhan = (int)comboBoxThanhPhan.SelectedValue;
            int idThanhPhanLienQuan = (int)comboBoxChatLienQuan.SelectedValue;

            // Check if selecting the same component
            if (idThanhPhan == idThanhPhanLienQuan)
            {
                MessageBox.Show("Không thể liên kết Thành Phần với chính nó.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_listLienKet.Any(l => l.IDThanhphan == idThanhPhan &&
                                     l.IDThanhphanLienquan == idThanhPhanLienQuan))
            {
                MessageBox.Show("Liên kết này đã tồn tại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _listLienKet.Add(new ChatLienQuan
            {
                IDThanhphan = idThanhPhan,
                IDThanhphanLienquan = idThanhPhanLienQuan
            });

            capnhat = true; // Mark as having unsaved changes
            LoadLinkedList();
        }

        private void buttonXoaCLQ_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa liên kết này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var selectedRows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList();

                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row.DataBoundItem != null)
                    {
                        var item = row.DataBoundItem;
                        int idThanhPhan = (int)item.GetType().GetProperty("IDThanhphan").GetValue(item);
                        int idThanhPhanLienQuan = (int)item.GetType().GetProperty("IDThanhphanLienquan").GetValue(item);

                        var linkToRemove = _listLienKet.FirstOrDefault(l =>
                            l.IDThanhphan == idThanhPhan &&
                            l.IDThanhphanLienquan == idThanhPhanLienQuan);

                        if (linkToRemove != null)
                        {
                            _listLienKet.Remove(linkToRemove);
                        }
                    }
                }

                capnhat = true; // Mark as having unsaved changes
                LoadLinkedList();
            }
        }
    }
}
