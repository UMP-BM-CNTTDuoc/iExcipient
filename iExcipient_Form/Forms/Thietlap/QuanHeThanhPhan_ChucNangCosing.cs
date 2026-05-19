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
    public partial class QuanHeThanhPhan_ChucNangCosing : Form
    {
        private List<ThanhPhanCosing> _listThanhPhanCosing;
        ThanhPhanCosing workingTP;
        private List<ChucNangCosing> _listChucNangCosing;
        private BindingList<ThanhPhan_ChucNangCosing> _listLienKet;

        BindingSource grid1 = new BindingSource();

        KetnoiDB.GetData getdata = new KetnoiDB.GetData();
        KetnoiDB.InsertData insertdata = new KetnoiDB.InsertData();
        KetnoiDB.DeleteData deletedata = new KetnoiDB.DeleteData();

        private bool capnhat = false;

        public QuanHeThanhPhan_ChucNangCosing()
        {
            InitializeComponent();
        }

        private void QuanHeThanhPhan_ChucNangCosing_Load(object sender, EventArgs e)
        {
            LoadThanhPhanCosing();
            LoadChucNangCosing();
            LoadListLienKet();

            comboBoxThanhPhan.SelectedIndex = -1;
            comboBoxChucNangCosing.SelectedIndex = -1;

            LoadLinkedList();
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            if (capnhat)
            {
                DialogResult result = MessageBox.Show(
                    "Có thay đổi chưa được lưu. Bạn có muốn lưu trước khi thoát?",
                    "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    buttonSua_Click(sender, e);
                    if (capnhat) return;
                }
                else if (result == DialogResult.Cancel)
                    return;
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

                deletedata.DeleteThanhPhan_ChucNangCosing_ByThanhPhanCosing(workingTP.IDThanhphan_Cosing);

                foreach (ThanhPhan_ChucNangCosing i in _listLienKet)
                    insertdata.InsertThanhPhan_ChucNangCosing(i);

                capnhat = false;
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
            using (Forms.Thietlap.Import_ThanhPhan_ChucNangCosing formcon = new Forms.Thietlap.Import_ThanhPhan_ChucNangCosing())
            {
                formcon.ShowDialog();
                LoadThanhPhanCosing();
                LoadChucNangCosing();
                if (workingTP != null)
                    comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan_Cosing;
            }
        }

        private void LoadThanhPhanCosing()
        {
            _listThanhPhanCosing = getdata.GetDSThanhPhanCosing().OrderBy(tp => tp.Ten_INCI).ToList();
            comboBoxThanhPhan.DataSource = _listThanhPhanCosing.ToList();
            comboBoxThanhPhan.DisplayMember = "Ten_INCI";
            comboBoxThanhPhan.ValueMember = "IDThanhphan_Cosing";
        }

        private void LoadChucNangCosing()
        {
            _listChucNangCosing = getdata.GetDSChucNangCosing().OrderBy(cn => cn.Tenchucnangcosing).ToList();
            comboBoxChucNangCosing.DataSource = _listChucNangCosing.ToList();
            comboBoxChucNangCosing.DisplayMember = "Tenchucnangcosing";
            comboBoxChucNangCosing.ValueMember = "IDChucnangcosing";
        }

        private void LoadListLienKet()
        {
            _listLienKet = new BindingList<ThanhPhan_ChucNangCosing>();
        }

        private void comboBoxThanhPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxThanhPhan.SelectedValue == null || !(comboBoxThanhPhan.SelectedValue is int))
                    return;

                if (capnhat)
                {
                    DialogResult result = MessageBox.Show(
                        "Có thay đổi chưa được lưu. Bạn có muốn lưu trước khi chuyển sang Thành Phần khác?",
                        "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        buttonSua_Click(sender, e);
                        if (capnhat) return;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        if (workingTP != null)
                            comboBoxThanhPhan.SelectedValue = workingTP.IDThanhphan_Cosing;
                        return;
                    }
                    else
                        capnhat = false;
                }

                int idThanhPhanCosing = (int)comboBoxThanhPhan.SelectedValue;
                workingTP = getdata.GetThanhPhanCosing(idThanhPhanCosing);

                _listLienKet.Clear();

                if (workingTP != null && workingTP.dsChucNangCosing != null)
                {
                    foreach (ChucNangCosing cn in workingTP.dsChucNangCosing)
                    {
                        _listLienKet.Add(new ThanhPhan_ChucNangCosing
                        {
                            IDThanhphan_Cosing = idThanhPhanCosing,
                            IDChucnangcosing = cn.IDChucnangcosing
                        });
                    }
                }

                LoadLinkedList();
                capnhat = false;
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
                IDThanhphan_Cosing = l.IDThanhphan_Cosing,
                TenThanhPhanCosing = GetTenThanhPhanCosing(l.IDThanhphan_Cosing),
                CAS_No = GetCASNo(l.IDThanhphan_Cosing),
                IDChucnangcosing = l.IDChucnangcosing,
                TenChucNangCosing = GetTenChucNangCosing(l.IDChucnangcosing),
                MoTaChucNangCosing = GetMoTaChucNangCosing(l.IDChucnangcosing)
            }).ToList();

            grid1.DataSource = null;
            grid1.DataSource = displayList;
            dataGridView1.DataSource = grid1;
            dataGridView1.Refresh();
        }

        private string GetTenThanhPhanCosing(int id)
        {
            ThanhPhanCosing tp = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tp != null ? tp.Ten_INCI : "";
        }

        private string GetCASNo(int id)
        {
            ThanhPhanCosing tp = _listThanhPhanCosing.FirstOrDefault(t => t.IDThanhphan_Cosing == id);
            return tp != null ? tp.CAS_No : "";
        }
        private string GetTenChucNangCosing(int id)
        {
            ChucNangCosing cn = _listChucNangCosing.FirstOrDefault(c => c.IDChucnangcosing == id);
            return cn != null ? cn.Tenchucnangcosing : "";
        }

        private string GetMoTaChucNangCosing(int id)
        {
            ChucNangCosing cn = _listChucNangCosing.FirstOrDefault(c => c.IDChucnangcosing == id);
            return cn != null ? cn.Motachucnangcosing : "";
        }

        private void buttonThemChucNangCosing_Click(object sender, EventArgs e)
        {
            if (comboBoxThanhPhan.SelectedValue == null || comboBoxChucNangCosing.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cả Thành Phần và Chức Năng Cosing.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idThanhPhanCosing = (int)comboBoxThanhPhan.SelectedValue;
            int idChucNangCosing = (int)comboBoxChucNangCosing.SelectedValue;

            if (_listLienKet.Any(l => l.IDThanhphan_Cosing == idThanhPhanCosing && l.IDChucnangcosing == idChucNangCosing))
            {
                MessageBox.Show("Liên kết này đã tồn tại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _listLienKet.Add(new ThanhPhan_ChucNangCosing
            {
                IDThanhphan_Cosing = idThanhPhanCosing,
                IDChucnangcosing = idChucNangCosing
            });

            capnhat = true;
            LoadLinkedList();
        }

        private void buttonXoaChucNangCosing_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa liên kết này?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var selectedRows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().ToList();

                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row.DataBoundItem != null)
                    {
                        var item = row.DataBoundItem;
                        int idThanhPhanCosing = (int)item.GetType().GetProperty("IDThanhphan_Cosing").GetValue(item);
                        int idChucNangCosing = (int)item.GetType().GetProperty("IDChucnangcosing").GetValue(item);

                        var linkToRemove = _listLienKet.FirstOrDefault(l =>
                            l.IDThanhphan_Cosing == idThanhPhanCosing &&
                            l.IDChucnangcosing == idChucNangCosing);

                        if (linkToRemove != null)
                            _listLienKet.Remove(linkToRemove);
                    }
                }

                capnhat = true;
                LoadLinkedList();
            }
        }
    }
}