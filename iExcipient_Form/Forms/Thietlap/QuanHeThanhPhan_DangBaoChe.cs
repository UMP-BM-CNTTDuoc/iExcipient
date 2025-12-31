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
    public partial class QuanHeThanhPhan_DangBaoChe : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private List<DangBaoChe> _listDangBaoChe;
        private BindingList<ThanhPhan_DangBaoChe> _listLienKet;
        ThanhPhan workingTP;

        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        private bool capnhat = false; // Flag to track unsaved changes

        public QuanHeThanhPhan_DangBaoChe()
        {
            InitializeComponent();
        }

        private void QuanHeThanhPhan_DangBaoChe_Load(object sender, EventArgs e)
        {
            LoadThanhPhan();
            LoadDangBaoChe();
            LoadListLienKet();

            comboBoxThanhPhan.SelectedIndex = -1;
            comboBoxDangBaoChe.SelectedIndex = -1;

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

                // Xóa tất cả quan hệ cũ của thành phần này
                deletedata.DeleteThanhPhan_DangBaoChe_ByThanhPhan(workingTP.IDThanhphan);

                // Thêm lại các quan hệ mới
                foreach (ThanhPhan_DangBaoChe i in _listLienKet)
                {
                    insertdata.InsertThanhPhan_DangBaoChe(i);
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
            using (Forms.Thietlap.Import_ThanhPhan_DangBaoChe formcon = new Forms.Thietlap.Import_ThanhPhan_DangBaoChe())
            {
                formcon.ShowDialog();
                // Reload data after import
                LoadThanhPhan();
                LoadDangBaoChe();
                if (workingTP != null)
                {
                    comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan;
                }
            }
        }

        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhan().OrderBy(tp => tp.Ten_INCI).ToList();

            // Load for main component ComboBox
            comboBoxThanhPhan.DataSource = _listThanhPhan.ToList();
            comboBoxThanhPhan.DisplayMember = "Ten_INCI";
            comboBoxThanhPhan.ValueMember = "IDThanhphan";
        }

        private void LoadDangBaoChe()
        {
            _listDangBaoChe = getdata.GetDSDangBaoChe().OrderBy(dbc => dbc.TenDangbaoche).ToList();

            // Load for DangBaoChe ComboBox
            comboBoxDangBaoChe.DataSource = _listDangBaoChe.ToList();
            comboBoxDangBaoChe.DisplayMember = "TenDangbaoche";
            comboBoxDangBaoChe.ValueMember = "IDDangbaoche";
        }

        private void LoadListLienKet()
        {
            _listLienKet = new BindingList<ThanhPhan_DangBaoChe>();
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

                if (workingTP != null && workingTP.dsDangBaoChe != null)
                {
                    foreach (DangBaoChe dbc in workingTP.dsDangBaoChe)
                    {
                        _listLienKet.Add(new ThanhPhan_DangBaoChe
                        {
                            IDThanhphan = idThanhPhan,
                            IDDangbaoche = dbc.IDDangbaoche
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
                IDDangbaoche = l.IDDangbaoche,
                TenDangBaoChe = GetTenDangBaoChe(l.IDDangbaoche)
            }).ToList();

            grid1.DataSource = null;
            grid1.DataSource = displayList;
            dataGridView1.DataSource = grid1;
            dataGridView1.Refresh();
        }

        private string GetTenThanhPhan(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.Ten_INCI : "";
        }

        private string GetCASNo(int id)
        {
            ThanhPhan tp = _listThanhPhan.FirstOrDefault(t => t.IDThanhphan == id);
            return tp != null ? tp.CAS_No : "";
        }

        private string GetTenDangBaoChe(int id)
        {
            DangBaoChe dbc = _listDangBaoChe.FirstOrDefault(d => d.IDDangbaoche == id);
            return dbc != null ? dbc.TenDangbaoche : "";
        }

        private void buttonThemDangBaoChe_Click(object sender, EventArgs e)
        {
            if (comboBoxThanhPhan.SelectedValue == null || comboBoxDangBaoChe.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cả Thành Phần và Dạng Bào Chế.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idThanhPhan = (int)comboBoxThanhPhan.SelectedValue;
            int idDangBaoChe = (int)comboBoxDangBaoChe.SelectedValue;

            // Check if link already exists
            if (_listLienKet.Any(l => l.IDThanhphan == idThanhPhan &&
                                     l.IDDangbaoche == idDangBaoChe))
            {
                MessageBox.Show("Liên kết này đã tồn tại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _listLienKet.Add(new ThanhPhan_DangBaoChe
            {
                IDThanhphan = idThanhPhan,
                IDDangbaoche = idDangBaoChe
            });

            capnhat = true; // Mark as having unsaved changes
            LoadLinkedList();
        }

        private void buttonXoaDangBaoChe_Click(object sender, EventArgs e)
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
                        int idDangBaoChe = (int)item.GetType().GetProperty("IDDangbaoche").GetValue(item);

                        var linkToRemove = _listLienKet.FirstOrDefault(l =>
                            l.IDThanhphan == idThanhPhan &&
                            l.IDDangbaoche == idDangBaoChe);

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