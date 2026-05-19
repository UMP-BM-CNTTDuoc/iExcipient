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
    public partial class QuanHeLinkCosingVaSach : Form
    {
        private List<ThanhPhan> _listThanhPhan;
        private List<ThanhPhanCosing> _listThanhPhanCosing;
        private BindingList<LinkCosingVaSach> _listLienKet;
        ThanhPhan workingTP;

        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        private bool capnhat = false; // Flag to track unsaved changes

        public QuanHeLinkCosingVaSach()
        {
            InitializeComponent();
        }

        private void QuanHeLinkCosingVaSach_Load(object sender, EventArgs e)
        {
            LoadThanhPhan();
            LoadThanhPhanCosing();
            LoadListLienKet();

            comboBoxThanhPhan.SelectedIndex = -1;
            comboBoxThanhPhanCosing.SelectedIndex = -1;

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

                // Xóa tất cả quan hệ cũ của Sách này theo từng IDLink
                List<LinkCosingVaSach> dsCu = getdata.GetDSLinkCosingVaSach()
                    .Where(x => x.IDThanhphan == workingTP.IDThanhphan)
                    .ToList();

                foreach (LinkCosingVaSach old in dsCu)
                {
                    deletedata.DeleteLinkCosingVaSach(old.IDLink);
                }

                // Thêm lại các quan hệ mới
                foreach (LinkCosingVaSach i in _listLienKet)
                {
                    insertdata.InsertLinkCosingVaSach(i);
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

        private void LoadThanhPhan()
        {
            _listThanhPhan = getdata.GetDSThanhPhanTop(400).OrderBy(tp => tp.Ten_INCI).ToList();

            comboBoxThanhPhan.DataSource = _listThanhPhan.ToList();
            comboBoxThanhPhan.DisplayMember = "Ten_INN";
            comboBoxThanhPhan.ValueMember = "IDThanhphan";
        }

        private void LoadThanhPhanCosing()
        {
            _listThanhPhanCosing = getdata.GetDSThanhPhanCosingTop(200).OrderBy(tc => tc.Ten_INCI).ToList();

            comboBoxThanhPhanCosing.DataSource = _listThanhPhanCosing.ToList();
            comboBoxThanhPhanCosing.DisplayMember = "Ten_INCI";
            comboBoxThanhPhanCosing.ValueMember = "IDThanhphan_Cosing";
        }

        private void LoadListLienKet()
        {
            _listLienKet = new BindingList<LinkCosingVaSach>();
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
                workingTP = _listThanhPhan.FirstOrDefault(tp => tp.IDThanhphan == idThanhPhan);

                _listLienKet.Clear();

                // Tải các liên kết hiện có từ DB theo IDThanhphan (Sách)
                List<LinkCosingVaSach> dshienco = getdata.GetDSLinkCosingVaSach()
                    .Where(x => x.IDThanhphan == idThanhPhan)
                    .ToList();

                foreach (LinkCosingVaSach lk in dshienco)
                {
                    _listLienKet.Add(lk);
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
                IDLink = l.IDLink,
                IDThanhphan = l.IDThanhphan,
                TenThanhPhan = GetTenThanhPhan(l.IDThanhphan),
                CAS_No = GetCASNo(l.IDThanhphan),
                IDThanhphan_Cosing = l.IDThanhphan_Cosing,
                TenThanhPhanCosing = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                CAS_No_Cosing = GetCASNoCosing(l.IDThanhphan_Cosing)
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

        private string GetTenThanhPhanCosing(int id)
        {
            ThanhPhanCosing tpc = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tpc != null ? tpc.Ten_INCI : "";
        }

        private string GetCASNoCosing(int id)
        {
            ThanhPhanCosing tpc = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tpc != null ? tpc.CAS_No : "";
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (Forms.Thietlap.Import_LinkCosingVaSach formcon = new Forms.Thietlap.Import_LinkCosingVaSach())
            {
                formcon.ShowDialog();
                // Reload data after import
                LoadThanhPhan();
                LoadThanhPhanCosing();
                if (workingTP != null)
                {
                    comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan;
                }
            }
        }

        private void buttonThemThanhPhanCosing_Click(object sender, EventArgs e)
        {
            if (comboBoxThanhPhan.SelectedValue == null || comboBoxThanhPhanCosing.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cả Thành Phần và Thành Phần Cosing.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idThanhPhan = (int)comboBoxThanhPhan.SelectedValue;
            int idThanhPhanCosing = (int)comboBoxThanhPhanCosing.SelectedValue;

            // Check if link already exists
            if (_listLienKet.Any(l => l.IDThanhphan == idThanhPhan &&
                                     l.IDThanhphan_Cosing == idThanhPhanCosing))
            {
                MessageBox.Show("Liên kết này đã tồn tại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _listLienKet.Add(new LinkCosingVaSach
            {
                IDThanhphan = idThanhPhan,
                IDThanhphan_Cosing = idThanhPhanCosing
            });

            capnhat = true; // Mark as having unsaved changes
            LoadLinkedList();
        }

        private void buttonXoaThanhPhanCosing_Click(object sender, EventArgs e)
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
                        int idThanhPhanCosing = (int)item.GetType().GetProperty("IDThanhphan_Cosing").GetValue(item);

                        var linkToRemove = _listLienKet.FirstOrDefault(l =>
                            l.IDThanhphan == idThanhPhan &&
                            l.IDThanhphan_Cosing == idThanhPhanCosing);

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