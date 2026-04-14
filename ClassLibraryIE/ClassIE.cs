using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryIE
{
    #region Kết nối CSDL
    public class KetnoiDB
    {
        protected static KetnoiCSDLDataContext db = new KetnoiCSDLDataContext();
        #region Nhập liệu đơn
        public class InsertData
        {
            public bool InsertQuyDinh(QuyDinh item)
            {
                try
                {
                    d_Quydinh qd = item.toQuyDinhDB();

                    db.d_Quydinhs.InsertOnSubmit(qd);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertThanhPhan(ThanhPhan item)
            {
                try
                {
                    // kiểm tra tồn tại theo CAS_No
                    bool exists = db.d_Thanhphans
                                    .Any(x => x.CAS_No != null &&
                                              x.CAS_No.Trim() == item.CAS_No.Trim());

                    if (exists)
                        return false; // đã tồn tại → không insert

                    d_Thanhphan tp = item.toThanhPhanDB();

                    db.d_Thanhphans.InsertOnSubmit(tp);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertDangBaoChe(DangBaoChe item)
            {
                try
                {
                    // kiểm tra tồn tại
                    bool exists = db.d_Dangbaoches
                                    .Any(x => x.TenDangbaoche.ToLower() == item.TenDangbaoche.ToLower());

                    if (exists)
                        return false; // đã tồn tại → không insert

                    d_Dangbaoche dbc = item.toDangBaoCheDB();

                    db.d_Dangbaoches.InsertOnSubmit(dbc);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertChucNang(ChucNang item)
            {
                try
                {
                    // kiểm tra tồn tại
                    bool exists = db.d_Chucnangs
                                    .Any(x => x.Tenchucnang.ToLower() == item.Tenchucnang.ToLower());
                    if (exists)
                        return false; // đã tồn tại → không insert
                    d_Chucnang cn = item.toChucNangDB();
                    db.d_Chucnangs.InsertOnSubmit(cn);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertThanhPhan_ChucNang(ThanhPhan_ChucNang item)
            {
                try
                {
                    // kiểm tra tồn tại
                    bool exists = db.r_Thanhphan_Chucnangs
                                    .Any(x => x.IDThanhphan == item.IDThanhphan &&
                                              x.IDChucnang == item.IDChucnang);

                    if (exists)
                        return false; // đã tồn tại → không insert

                    r_Thanhphan_Chucnang link = item.toThanhPhan_ChucNangDB();

                    db.r_Thanhphan_Chucnangs.InsertOnSubmit(link);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertThanhPhan_DangBaoChe(ThanhPhan_DangBaoChe item)
            {
                try
                {
                    // kiểm tra tồn tại
                    bool exists = db.r_Thanhphan_Dangbaoches
                                    .Any(x => x.IDThanhphan == item.IDThanhphan &&
                                              x.IDDangbaoche == item.IDDangbaoche);

                    if (exists)
                        return false; // đã tồn tại → không insert

                    r_Thanhphan_Dangbaoche link = item.toThanhPhan_DangBaoCheDB();

                    db.r_Thanhphan_Dangbaoches.InsertOnSubmit(link);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertChatLienQuan(ChatLienQuan item)
            {
                try
                {
                    // Kiểm tra tồn tại cả 2 chiều A-B và B-A
                    bool exists = db.r_Chatlienquans
                                    .Any(x => (x.IDThanhphan == item.IDThanhphan &&
                                               x.IDThanhphanLienquan == item.IDThanhphanLienquan)
                                           || (x.IDThanhphan == item.IDThanhphanLienquan &&
                                               x.IDThanhphanLienquan == item.IDThanhphan));
                    if (exists)
                        return false;

                    r_Chatlienquan link = item.toChatLienQuanDB();
                    db.r_Chatlienquans.InsertOnSubmit(link);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region Nhập hàng loạt
        public class BulkInsertData
        {
            public bool BulkInsertQuyDinh(List<QuyDinh> list)
            {
                try
                {
                    List<d_Quydinh> dsimport = new List<d_Quydinh>();
                    foreach (QuyDinh i in list)
                    {
                        d_Quydinh a = i.toQuyDinhDB();
                        dsimport.Add(a);
                    }
                    db.d_Quydinhs.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool BulkInsertThanhPhan(List<ThanhPhan> list)
            {
                try
                {
                    List<d_Thanhphan> dsimport = new List<d_Thanhphan>();
                    foreach (ThanhPhan i in list)
                    {
                        d_Thanhphan a = i.toThanhPhanDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có trong DB
                    List<d_Thanhphan> dshienco = db.d_Thanhphans.ToList();

                    // 3. Tạo HashSet để check trùng nhanh (O(1))
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco
                            .Where(x => !string.IsNullOrEmpty(x.CAS_No))
                            .Select(x => x.CAS_No.Trim().ToLower())
                    );

                    // 4. Remove những phần tử bị trùng
                    dsimport = dsimport
                        .Where(x => !string.IsNullOrEmpty(x.CAS_No) &&
                                    !tapHienCo.Contains(x.CAS_No.Trim().ToLower()))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false; // không có gì để insert

                    db.d_Thanhphans.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool BulkInsertDangBaoChe(List<DangBaoChe> list)
            {
                try
                {
                    List<d_Dangbaoche> dsimport = new List<d_Dangbaoche>();
                    foreach (DangBaoChe i in list)
                    {
                        d_Dangbaoche a = i.toDangBaoCheDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có trong DB
                    List<d_Dangbaoche> dshienco = db.d_Dangbaoches.ToList();

                    // 3. Tạo HashSet để check trùng nhanh (O(1))
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.TenDangbaoche.Trim().ToLower())
                    );

                    // 4. Remove những phần tử bị trùng
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.TenDangbaoche.Trim().ToLower()))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false; // không có gì để insert

                    db.d_Dangbaoches.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool BulkInsertChucNang(List<ChucNang> list)
            {
                try
                {
                    List<d_Chucnang> dsimport = new List<d_Chucnang>();
                    foreach (ChucNang i in list)
                    {
                        d_Chucnang a = i.toChucNangDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có trong DB
                    List<d_Chucnang> dshienco = db.d_Chucnangs.ToList();

                    // 3. Tạo HashSet để check trùng nhanh (O(1))
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.Tenchucnang.Trim().ToLower())
                    );

                    // 4. Remove những phần tử bị trùng
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.Tenchucnang.Trim().ToLower()))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false; // không có gì để insert

                    db.d_Chucnangs.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool BulkInsertThanhPhan_ChucNang(List<ThanhPhan_ChucNang> list)
            {
                try
                {
                    // 1. Chuyển đổi sang DB entities
                    List<r_Thanhphan_Chucnang> dsimport = new List<r_Thanhphan_Chucnang>();
                    foreach (ThanhPhan_ChucNang i in list)
                    {
                        r_Thanhphan_Chucnang a = i.toThanhPhan_ChucNangDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có trong DB
                    List<r_Thanhphan_Chucnang> dshienco = db.r_Thanhphan_Chucnangs.ToList();

                    // 3. Tạo HashSet để check trùng nhanh (O(1))
                    // Key format: "IDThanhphan_IDChucnang"
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.IDThanhphan.ToString() + "_" + x.IDChucnang.ToString())
                    );

                    // 4. Lọc bỏ những phần tử đã tồn tại
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan.ToString() + "_" + x.IDChucnang.ToString()))
                        .ToList();

                    // 5. Kiểm tra có dữ liệu để insert không
                    if (dsimport.Count == 0)
                        return false; // không có gì để insert

                    // 6. Insert và commit
                    db.r_Thanhphan_Chucnangs.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    // Log exception nếu cần: ex.Message
                    return false;
                }
            }

            public bool BulkInsertThanhPhan_DangBaoChe(List<ThanhPhan_DangBaoChe> list)
            {
                try
                {
                    // 1. Chuyển đổi sang DB entities
                    List<r_Thanhphan_Dangbaoche> dsimport = new List<r_Thanhphan_Dangbaoche>();
                    foreach (ThanhPhan_DangBaoChe i in list)
                    {
                        r_Thanhphan_Dangbaoche a = i.toThanhPhan_DangBaoCheDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có
                    List<r_Thanhphan_Dangbaoche> dshienco = db.r_Thanhphan_Dangbaoches.ToList();

                    // 3. Tạo HashSet composite key
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.IDThanhphan.ToString() + "_" + x.IDDangbaoche.ToString())
                    );

                    // 4. Lọc bỏ trùng lặp
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan.ToString() + "_" + x.IDDangbaoche.ToString()))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false;

                    // 5. Insert
                    db.r_Thanhphan_Dangbaoches.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool BulkInsertChatLienQuan(List<ChatLienQuan> list)
            {
                try
                {
                    // 1. Chuyển đổi sang DB entities
                    List<r_Chatlienquan> dsimport = new List<r_Chatlienquan>();
                    foreach (ChatLienQuan i in list)
                    {
                        r_Chatlienquan a = i.toChatLienQuanDB();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có
                    List<r_Chatlienquan> dshienco = db.r_Chatlienquans.ToList();

                    // 3. Tạo HashSet cả 2 chiều
                    HashSet<string> tapHienCo = new HashSet<string>();
                    foreach (var x in dshienco)
                    {
                        tapHienCo.Add(x.IDThanhphan + "_" + x.IDThanhphanLienquan);
                        tapHienCo.Add(x.IDThanhphanLienquan + "_" + x.IDThanhphan); // ← thêm chiều ngược
                    }

                    // 4. Lọc bỏ trùng lặp (kiểm tra cả 2 chiều)
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan + "_" + x.IDThanhphanLienquan))
                        .ToList();

                    // 5. Insert
                    db.r_Chatlienquans.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region Lấy dữ liệu
        public class GetData
        {
            // Lấy toàn bộ Quy Định
            public List<QuyDinh> GetDSQuyDinh()
            {
                List<QuyDinh> kq = new List<QuyDinh>();

                List<d_Quydinh> ds = (from data in db.d_Quydinhs
                                      select data).ToList();

                foreach (d_Quydinh i in ds)
                    kq.Add(QuyDinh.fromQuyDinhDB(i));

                return kq;
            }

            // Lấy toàn bộ Thành Phần
            public List<ThanhPhan> GetDSThanhPhan()
            {
                List<ThanhPhan> kq = new List<ThanhPhan>();

                List<d_Thanhphan> ds = (from data in db.d_Thanhphans
                                        select data).ToList();

                foreach (d_Thanhphan i in ds)
                    kq.Add(ThanhPhan.fromThanhPhanDB(i));

                return kq;
            }

            // Lấy toàn bộ Dạng Bào Chế
            public List<DangBaoChe> GetDSDangBaoChe()
            {
                List<DangBaoChe> kq = new List<DangBaoChe>();

                List<d_Dangbaoche> ds = db.d_Dangbaoches.ToList();

                foreach (d_Dangbaoche i in ds)
                    kq.Add(DangBaoChe.fromDangBaoCheDB(i));

                return kq;
            }

            // Lấy toàn bộ Chức Năng
            public List<ChucNang> GetDSChucNang()
            {
                List<ChucNang> kq = new List<ChucNang>();

                List<d_Chucnang> ds = db.d_Chucnangs.ToList();

                foreach (d_Chucnang i in ds)
                    kq.Add(ChucNang.fromChucNangDB(i));

                return kq;
            }

            // Lấy toàn bộ quan hệ Thành Phần - Chức Năng
            public List<ThanhPhan_ChucNang> GetDSThanhPhan_ChucNang()
            {
                List<ThanhPhan_ChucNang> kq = new List<ThanhPhan_ChucNang>();

                List<r_Thanhphan_Chucnang> ds = db.r_Thanhphan_Chucnangs.ToList();

                foreach (r_Thanhphan_Chucnang i in ds)
                    kq.Add(ThanhPhan_ChucNang.fromThanhPhan_ChucNangDB(i));

                return kq;
            }

            // Lấy toàn bộ quan hệ Thành Phần - Dạng Bào Chế
            public List<ThanhPhan_DangBaoChe> GetDSThanhPhan_DangBaoChe()
            {
                List<ThanhPhan_DangBaoChe> kq = new List<ThanhPhan_DangBaoChe>();

                List<r_Thanhphan_Dangbaoche> ds = db.r_Thanhphan_Dangbaoches.ToList();

                foreach (r_Thanhphan_Dangbaoche i in ds)
                    kq.Add(ThanhPhan_DangBaoChe.fromThanhPhan_DangBaoCheDB(i));

                return kq;
            }

            // Lấy toàn bộ quan hệ Chất Liên Quan
            public List<ChatLienQuan> GetDSChatLienQuan()
            {
                List<ChatLienQuan> kq = new List<ChatLienQuan>();

                List<r_Chatlienquan> ds = db.r_Chatlienquans.ToList();

                foreach (r_Chatlienquan i in ds)
                    kq.Add(ChatLienQuan.fromChatLienQuanDB(i));

                return kq;
            }

            // Lấy Thành Phần theo ID
            public ThanhPhan GetThanhPhan(int idThanhPhan)
            {
                ThanhPhan kq = new ThanhPhan();
                try
                {
                    d_Thanhphan thanhphan = (from data in db.d_Thanhphans
                                             where data.IDThanhphan == idThanhPhan
                                             select data).FirstOrDefault();

                    kq = ThanhPhan.fromThanhPhanDB(thanhphan);

                    // Lấy danh sách quy định của thành phần
                    kq.dsQuyDinh = GetQuyDinhByThanhPhan(idThanhPhan);

                    // Lấy danh sách dạng bào chế của thành phần
                    kq.dsDangBaoChe = GetDangBaoCheByThanhPhan(idThanhPhan);

                    // Lấy danh sách chức năng của thành phần
                    kq.dsChucNang = GetChucNangByThanhPhan(idThanhPhan);

                    // Lấy danh sách thành phần liên quan
                    kq.dsThanhPhanLienQuan = GetThanhPhanLienQuan(idThanhPhan);

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Lấy danh sách Quy Định theo IDThanhPhan
            public List<QuyDinh> GetQuyDinhByThanhPhan(int idThanhPhan)
            {
                List<QuyDinh> kq = new List<QuyDinh>();
                try
                {
                    List<d_Quydinh> ds = (from data in db.d_Quydinhs
                                          where data.IDThanhphan == idThanhPhan
                                          select data).ToList();

                    foreach (d_Quydinh i in ds)
                        kq.Add(QuyDinh.fromQuyDinhDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Lấy danh sách Dạng Bào Chế theo IDThanhPhan
            public List<DangBaoChe> GetDangBaoCheByThanhPhan(int idThanhPhan)
            {
                List<DangBaoChe> kq = new List<DangBaoChe>();
                try
                {
                    List<d_Dangbaoche> ds = (from data in db.d_Dangbaoches
                                             join rela in db.r_Thanhphan_Dangbaoches
                                                 on data.IDDangbaoche equals rela.IDDangbaoche
                                             where rela.IDThanhphan == idThanhPhan
                                             select data).ToList();

                    foreach (d_Dangbaoche i in ds)
                        kq.Add(DangBaoChe.fromDangBaoCheDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Lấy danh sách Chức Năng theo IDThanhPhan
            public List<ChucNang> GetChucNangByThanhPhan(int idThanhPhan)
            {
                List<ChucNang> kq = new List<ChucNang>();
                try
                {
                    List<d_Chucnang> ds = (from data in db.d_Chucnangs
                                           join rela in db.r_Thanhphan_Chucnangs
                                               on data.IDChucnang equals rela.IDChucnang
                                           where rela.IDThanhphan == idThanhPhan
                                           select data).ToList();

                    foreach (d_Chucnang i in ds)
                        kq.Add(ChucNang.fromChucNangDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Lấy danh sách Thành Phần Liên Quan theo IDThanhPhan
            public List<ThanhPhan> GetThanhPhanLienQuan(int idThanhPhan)
            {
                List<ThanhPhan> kq = new List<ThanhPhan>();
                try
                {
                    // Chiều đi: idThanhPhan là chính, lấy các chất liên quan
                    List<d_Thanhphan> ds1 = (from data in db.d_Thanhphans
                                             join rela in db.r_Chatlienquans
                                                 on data.IDThanhphan equals rela.IDThanhphanLienquan
                                             where rela.IDThanhphan == idThanhPhan
                                             select data).ToList();

                    // Chiều về: idThanhPhan là liên quan, lấy các chất chính
                    List<d_Thanhphan> ds2 = (from data in db.d_Thanhphans
                                             join rela in db.r_Chatlienquans
                                                 on data.IDThanhphan equals rela.IDThanhphan
                                             where rela.IDThanhphanLienquan == idThanhPhan
                                             select data).ToList();

                    // Gộp 2 chiều, loại trùng theo IDThanhphan
                    List<d_Thanhphan> ds = ds1.Union(ds2)
                                               .GroupBy(x => x.IDThanhphan)
                                               .Select(g => g.First())
                                               .ToList();

                    foreach (d_Thanhphan i in ds)
                        kq.Add(ThanhPhan.fromThanhPhanDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Tìm kiếm Thành Phần theo từ khóa
            public List<ThanhPhan> SearchThanhPhan(string keyword)
            {
                List<ThanhPhan> kq = new List<ThanhPhan>();
                try
                {
                    List<d_Thanhphan> ds = db.d_Thanhphans
                        .Where(tp => tp.Ten_INN.Contains(keyword) ||
                                     tp.Ten_INCI.Contains(keyword) ||
                                     tp.Ten_IUPAC.Contains(keyword) ||
                                     tp.CAS_No.Contains(keyword))
                        .ToList();

                    foreach (d_Thanhphan i in ds)
                        kq.Add(ThanhPhan.fromThanhPhanDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Tìm kiếm Thành Phần theo Chức Năng
            public List<ThanhPhan> GetThanhPhanByChucNang(int idChucNang)
            {
                List<ThanhPhan> kq = new List<ThanhPhan>();
                try
                {
                    List<d_Thanhphan> ds = (from data in db.d_Thanhphans
                                            join rela in db.r_Thanhphan_Chucnangs
                                                on data.IDThanhphan equals rela.IDThanhphan
                                            where rela.IDChucnang == idChucNang
                                            select data).ToList();

                    foreach (d_Thanhphan i in ds)
                        kq.Add(ThanhPhan.fromThanhPhanDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Tìm kiếm Thành Phần theo Dạng Bào Chế
            public List<ThanhPhan> GetThanhPhanByDangBaoChe(int idDangBaoChe)
            {
                List<ThanhPhan> kq = new List<ThanhPhan>();
                try
                {
                    List<d_Thanhphan> ds = (from data in db.d_Thanhphans
                                            join rela in db.r_Thanhphan_Dangbaoches
                                                on data.IDThanhphan equals rela.IDThanhphan
                                            where rela.IDDangbaoche == idDangBaoChe
                                            select data).ToList();

                    foreach (d_Thanhphan i in ds)
                        kq.Add(ThanhPhan.fromThanhPhanDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            public ThanhPhan GetThanhPhanByID(int id)
            {
                try
                {
                    d_Thanhphan tp = db.d_Thanhphans
                        .FirstOrDefault(x => x.IDThanhphan == id);

                    if (tp == null)
                        return null;

                    return ThanhPhan.fromThanhPhanDB(tp);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        #region Update dữ liệu
        public class UpdateData
        {
            public bool UpdateQuyDinh(int idQuydinh, int idThanhphan, bool? annexII, bool? annexIII,
                                      bool? annexIV, bool? annexV, bool? annexVI,
                                      string dieuKienSuDung, string ghichu)
            {
                try
                {
                    d_Quydinh qd = db.d_Quydinhs.SingleOrDefault(x => x.IDQuydinh == idQuydinh);
                    if (qd != null)
                    {
                        qd.IDThanhphan = idThanhphan;
                        qd.AnnexII = annexII;
                        qd.AnnexIII = annexIII;
                        qd.AnnexIV = annexIV;
                        qd.AnnexV = annexV;
                        qd.AnnexVI = annexVI;
                        qd.DieuKienSuDung = dieuKienSuDung;
                        qd.Ghichu = ghichu;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateThanhPhan(int idThanhphan, string ten_INN, string ten_INCI, string ten_IUPAC,
                                        string cas_No, string congThucHoaHoc, double khoiLuongPhanTu,
                                        string cauTrucPhanTu, string tinhChatVatLy, string moTa,
                                        string baoQuan, string tltk)
            {
                try
                {
                    d_Thanhphan tp = db.d_Thanhphans.SingleOrDefault(x => x.IDThanhphan == idThanhphan);
                    if (tp != null)
                    {
                        tp.Ten_INN = ten_INN;
                        tp.Ten_INCI = ten_INCI;
                        tp.Ten_IUPAC = ten_IUPAC;
                        tp.CAS_No = cas_No;
                        tp.CongThucHoaHoc = congThucHoaHoc;
                        tp.KhoiLuongPhanTu = khoiLuongPhanTu;
                        tp.CauTrucPhanTu = cauTrucPhanTu;
                        tp.TinhChatVatLy = tinhChatVatLy;
                        tp.MoTa = moTa;
                        tp.BaoQuan = baoQuan;
                        tp.TLTK = tltk;
                        tp.NgayCapNhat = DateTime.Now;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateDangBaoChe(int idDangbaoche, string tenDangbaoche)
            {
                try
                {
                    d_Dangbaoche dbc = db.d_Dangbaoches.SingleOrDefault(x => x.IDDangbaoche == idDangbaoche);
                    if (dbc != null)
                    {
                        dbc.TenDangbaoche = tenDangbaoche;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateChucNang(int idChucnang, string tenChucnang, string motaChucnang)
            {
                try
                {
                    d_Chucnang cn = db.d_Chucnangs.SingleOrDefault(x => x.IDChucnang == idChucnang);
                    if (cn != null)
                    {
                        cn.Tenchucnang = tenChucnang;
                        cn.Motachucnang = motaChucnang;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateThanhPhan_ChucNang(int idThanhphan, int idChucnangOld, int idChucnangNew)
            {
                try
                {
                    r_Thanhphan_Chucnang link = db.r_Thanhphan_Chucnangs.SingleOrDefault(x =>
                        x.IDThanhphan == idThanhphan && x.IDChucnang == idChucnangOld);
                    if (link != null)
                    {
                        link.IDChucnang = idChucnangNew;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateThanhPhan_DangBaoChe(int idThanhphan, int idDangbaocheOld, int idDangbaocheNew)
            {
                try
                {
                    r_Thanhphan_Dangbaoche link = db.r_Thanhphan_Dangbaoches.SingleOrDefault(x =>
                        x.IDThanhphan == idThanhphan && x.IDDangbaoche == idDangbaocheOld);
                    if (link != null)
                    {
                        link.IDDangbaoche = idDangbaocheNew;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool UpdateChatLienQuan(int idThanhphan, int idThanhphanLienquanOld, int idThanhphanLienquanNew)
            {
                try
                {
                    r_Chatlienquan link = db.r_Chatlienquans.SingleOrDefault(x =>
                        x.IDThanhphan == idThanhphan && x.IDThanhphanLienquan == idThanhphanLienquanOld);
                    if (link != null)
                    {
                        link.IDThanhphanLienquan = idThanhphanLienquanNew;
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
        #endregion

        #region Xóa dữ liệu
        public class DeleteData
        {
            public bool DeleteQuyDinh(int idQuyDinh)
            {
                try
                {
                    d_Quydinh qd = db.d_Quydinhs.SingleOrDefault(x => x.IDQuydinh == idQuyDinh);
                    if (qd != null)
                    {
                        db.d_Quydinhs.DeleteOnSubmit(qd);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteThanhPhan(int idThanhPhan)
            {
                try
                {
                    // Delete related records in r_Thanhphan_Chucnang
                    DeleteThanhPhan_ChucNang_ByThanhPhan(idThanhPhan);

                    // Delete related records in r_Thanhphan_Dangbaoche
                    DeleteThanhPhan_DangBaoChe_ByThanhPhan(idThanhPhan);

                    // Delete related records in r_Chatlienquan (as main component)
                    DeleteThanhPhan_ChatLienQuan_ByThanhPhan(idThanhPhan);

                    // Delete related records in r_Chatlienquan (as related component)
                    IQueryable<r_Chatlienquan> relatedAsLienQuan = db.r_Chatlienquans.Where(x => x.IDThanhphanLienquan == idThanhPhan);
                    if (relatedAsLienQuan.Any())
                    {
                        db.r_Chatlienquans.DeleteAllOnSubmit(relatedAsLienQuan);
                    }

                    // Delete related QuyDinh records
                    IQueryable<d_Quydinh> relatedQuyDinh = db.d_Quydinhs.Where(x => x.IDThanhphan == idThanhPhan);
                    if (relatedQuyDinh.Any())
                    {
                        db.d_Quydinhs.DeleteAllOnSubmit(relatedQuyDinh);
                    }

                    // Delete the main record
                    d_Thanhphan tp = db.d_Thanhphans.SingleOrDefault(x => x.IDThanhphan == idThanhPhan);
                    if (tp != null)
                    {
                        db.d_Thanhphans.DeleteOnSubmit(tp);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteDangBaoChe(int idDangBaoChe)
            {
                try
                {
                    // Delete related records in r_Thanhphan_Dangbaoche first
                    IQueryable<r_Thanhphan_Dangbaoche> relatedRecords = db.r_Thanhphan_Dangbaoches.Where(x => x.IDDangbaoche == idDangBaoChe);
                    if (relatedRecords.Any())
                    {
                        db.r_Thanhphan_Dangbaoches.DeleteAllOnSubmit(relatedRecords);
                    }

                    // Delete the main record
                    d_Dangbaoche dbc = db.d_Dangbaoches.SingleOrDefault(x => x.IDDangbaoche == idDangBaoChe);
                    if (dbc != null)
                    {
                        db.d_Dangbaoches.DeleteOnSubmit(dbc);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteChucNang(int idChucNang)
            {
                try
                {
                    // Delete related records in r_Thanhphan_Chucnang first
                    IQueryable<r_Thanhphan_Chucnang> relatedLinks = db.r_Thanhphan_Chucnangs.Where(x => x.IDChucnang == idChucNang);
                    if (relatedLinks.Any())
                    {
                        db.r_Thanhphan_Chucnangs.DeleteAllOnSubmit(relatedLinks);
                    }

                    // Delete the main record
                    d_Chucnang cn = db.d_Chucnangs.SingleOrDefault(x => x.IDChucnang == idChucNang);
                    if (cn != null)
                    {
                        db.d_Chucnangs.DeleteOnSubmit(cn);
                        db.SubmitChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteThanhPhan_ChucNang_ByThanhPhan(int idThanhPhan)
            {
                try
                {
                    List<r_Thanhphan_Chucnang> links = (from data in db.r_Thanhphan_Chucnangs
                                                        where data.IDThanhphan == idThanhPhan
                                                        select data).ToList();
                    if (links != null && links.Any())
                    {
                        db.r_Thanhphan_Chucnangs.DeleteAllOnSubmit(links);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteThanhPhan_DangBaoChe_ByThanhPhan(int idThanhPhan)
            {
                try
                {
                    List<r_Thanhphan_Dangbaoche> links = (from data in db.r_Thanhphan_Dangbaoches
                                                          where data.IDThanhphan == idThanhPhan
                                                          select data).ToList();
                    if (links != null && links.Any())
                    {
                        db.r_Thanhphan_Dangbaoches.DeleteAllOnSubmit(links);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool DeleteThanhPhan_ChatLienQuan_ByThanhPhan(int idThanhPhan)
            {
                try
                {
                    // Xóa cả 2 chiều: A là chính hoặc A là liên quan
                    List<r_Chatlienquan> links = (from data in db.r_Chatlienquans
                                                  where data.IDThanhphan == idThanhPhan
                                                     || data.IDThanhphanLienquan == idThanhPhan
                                                  select data).ToList();

                    if (links != null && links.Any())
                    {
                        db.r_Chatlienquans.DeleteAllOnSubmit(links);
                        db.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            // Method để kiểm tra số lượng quan hệ cho ChucNang
            public int GetRelatedCountChucNang(int idChucNang)
            {
                try
                {
                    return db.r_Thanhphan_Chucnangs.Count(x => x.IDChucnang == idChucNang);
                }
                catch
                {
                    return 0;
                }
            }
            // Method đếm số lượng quan hệ cho DangBaoChe
            public int GetRelatedCountDangBaoChe(int idDangBaoChe)
            {
                try
                {
                    return db.r_Thanhphan_Dangbaoches.Count(x => x.IDDangbaoche == idDangBaoChe);
                }
                catch
                {
                    return 0;
                }
            }

            // Method đếm số lượng quan hệ cho ThanhPhan
            public class ThanhPhanRelationCount
            {
                public int ChucNangCount { get; set; }
                public int DangBaoCheCount { get; set; }
                public int ChatLienQuanCount { get; set; }
                public int ChatLienQuanAsRelatedCount { get; set; }
                public int QuyDinhCount { get; set; }
                public int TotalCount { get; set; }
            }

            public ThanhPhanRelationCount GetRelatedCountThanhPhan(int idThanhPhan)
            {
                try
                {
                    var result = new ThanhPhanRelationCount
                    {
                        ChucNangCount = db.r_Thanhphan_Chucnangs.Count(x => x.IDThanhphan == idThanhPhan),
                        DangBaoCheCount = db.r_Thanhphan_Dangbaoches.Count(x => x.IDThanhphan == idThanhPhan),
                        ChatLienQuanCount = db.r_Chatlienquans.Count(x => x.IDThanhphan == idThanhPhan),
                        ChatLienQuanAsRelatedCount = db.r_Chatlienquans.Count(x => x.IDThanhphanLienquan == idThanhPhan),
                        QuyDinhCount = db.d_Quydinhs.Count(x => x.IDThanhphan == idThanhPhan)
                    };

                    result.TotalCount = result.ChucNangCount + result.DangBaoCheCount +
                                      result.ChatLienQuanCount + result.ChatLienQuanAsRelatedCount +
                                      result.QuyDinhCount;

                    return result;
                }
                catch
                {
                    return new ThanhPhanRelationCount();
                }
            }

            // Method đếm số lượng QuyDinh cho một ThanhPhan
            public int GetRelatedCountQuyDinh(int idThanhPhan)
            {
                try
                {
                    return db.d_Quydinhs.Count(x => x.IDThanhphan == idThanhPhan);
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion
    }
    #endregion
    #region Data class
    public class ThanhPhan
    {
        public int IDThanhphan { get; set; }
        public string Ten_INN { get; set; }
        public string Ten_INCI { get; set; }
        public string Ten_IUPAC { get; set; }
        public string CAS_No { get; set; }
        public string CongThucHoaHoc { get; set; }
        public double KhoiLuongPhanTu { get; set; }
        public string CauTrucPhanTu { get; set; }
        public string TinhChatVatLy { get; set; }
        public string MoTa { get; set; }
        public string BaoQuan { get; set; }
        public string TLTK { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public List<QuyDinh> dsQuyDinh { get; set; }
        public List<DangBaoChe> dsDangBaoChe { get; set; }
        public List<ChucNang> dsChucNang { get; set; }
        public List<ThanhPhan> dsThanhPhanLienQuan { get; set; }

        public ThanhPhan()
        {
            Ten_INN = "";
            Ten_INCI = "";
            Ten_IUPAC = "";
            CAS_No = "";
            CongThucHoaHoc = "";
            CauTrucPhanTu = "";
            TinhChatVatLy = "";
            MoTa = "";
            BaoQuan = "";
            TLTK = "";
        }

        public static ThanhPhan fromThanhPhanDB(d_Thanhphan item)
        {
            if (item == null)
                return null;
            ThanhPhan kq = new ThanhPhan
            {
                IDThanhphan = item.IDThanhphan,
                Ten_INN = item.Ten_INN,
                Ten_INCI = item.Ten_INCI,
                Ten_IUPAC = item.Ten_IUPAC,
                CAS_No = item.CAS_No,
                CongThucHoaHoc = item.CongThucHoaHoc,
                KhoiLuongPhanTu = item.KhoiLuongPhanTu ?? 0,
                CauTrucPhanTu = item.CauTrucPhanTu,
                TinhChatVatLy = item.TinhChatVatLy,
                MoTa = item.MoTa,
                BaoQuan = item.BaoQuan,
                TLTK = item.TLTK,
                NgayTao = item.NgayTao,
                NgayCapNhat = item.NgayCapNhat
            };
            return kq;
        }

        public d_Thanhphan toThanhPhanDB()
        {
            d_Thanhphan kq = new d_Thanhphan
            {
                IDThanhphan = this.IDThanhphan,
                Ten_INN = this.Ten_INN,
                Ten_INCI = this.Ten_INCI,
                Ten_IUPAC = this.Ten_IUPAC,
                CAS_No = this.CAS_No,
                CongThucHoaHoc = this.CongThucHoaHoc,
                KhoiLuongPhanTu = this.KhoiLuongPhanTu,
                CauTrucPhanTu = this.CauTrucPhanTu,
                TinhChatVatLy = this.TinhChatVatLy,
                MoTa = this.MoTa,
                BaoQuan = this.BaoQuan,
                TLTK = this.TLTK,
                NgayTao = this.NgayTao,
                NgayCapNhat = this.NgayCapNhat
            };
            return kq;
        }
    }

    public class QuyDinh
    {
        public int IDQuydinh { get; set; }
        public int IDThanhphan { get; set; }
        public bool? AnnexII { get; set; }
        public bool? AnnexIII { get; set; }
        public bool? AnnexIV { get; set; }
        public bool? AnnexV { get; set; }
        public bool? AnnexVI { get; set; }
        public string DieuKienSuDung { get; set; }
        public string Ghichu { get; set; }

        public QuyDinh()
        {
            DieuKienSuDung = "";
            Ghichu = "";
        }

        public static QuyDinh fromQuyDinhDB(d_Quydinh item)
        {
            if (item == null)
                return null;
            QuyDinh kq = new QuyDinh
            {
                IDQuydinh = item.IDQuydinh,
                IDThanhphan = item.IDThanhphan,
                AnnexII = item.AnnexII,
                AnnexIII = item.AnnexIII,
                AnnexIV = item.AnnexIV,
                AnnexV = item.AnnexV,
                AnnexVI = item.AnnexVI,
                DieuKienSuDung = item.DieuKienSuDung,
                Ghichu = item.Ghichu
            };
            return kq;
        }

        public d_Quydinh toQuyDinhDB()
        {
            d_Quydinh kq = new d_Quydinh
            {
                IDQuydinh = this.IDQuydinh,
                IDThanhphan = this.IDThanhphan,
                AnnexII = this.AnnexII,
                AnnexIII = this.AnnexIII,
                AnnexIV = this.AnnexIV,
                AnnexV = this.AnnexV,
                AnnexVI = this.AnnexVI,
                DieuKienSuDung = this.DieuKienSuDung,
                Ghichu = this.Ghichu
            };
            return kq;
        }
    }

    public class DangBaoChe
    {
        public int IDDangbaoche { get; set; }
        public string TenDangbaoche { get; set; }

        public DangBaoChe()
        {

        }

        public static DangBaoChe fromDangBaoCheDB(d_Dangbaoche item)
        {
            if (item == null)
                return null;
            DangBaoChe kq = new DangBaoChe
            {
                IDDangbaoche = item.IDDangbaoche,
                TenDangbaoche = item.TenDangbaoche
            };
            return kq;
        }

        public d_Dangbaoche toDangBaoCheDB()
        {
            d_Dangbaoche kq = new d_Dangbaoche
            {
                IDDangbaoche = this.IDDangbaoche,
                TenDangbaoche = this.TenDangbaoche
            };
            return kq;
        }
    }

    public class ChucNang
    {
        public int IDChucnang { get; set; }
        public string Tenchucnang { get; set; }
        public string Motachucnang { get; set; }

        public ChucNang()
        {
            Motachucnang = "";
        }

        public static ChucNang fromChucNangDB(d_Chucnang item)
        {
            if (item == null)
                return null;
            ChucNang kq = new ChucNang
            {
                IDChucnang = item.IDChucnang,
                Tenchucnang = item.Tenchucnang,
                Motachucnang = item.Motachucnang
            };
            return kq;
        }

        public d_Chucnang toChucNangDB()
        {
            d_Chucnang kq = new d_Chucnang
            {
                IDChucnang = this.IDChucnang,
                Tenchucnang = this.Tenchucnang,
                Motachucnang = this.Motachucnang
            };
            return kq;
        }
    }

    public class ThanhPhan_DangBaoChe
    {
        public int IDThanhphan { get; set; }
        public int IDDangbaoche { get; set; }

        public ThanhPhan_DangBaoChe()
        {

        }

        public static ThanhPhan_DangBaoChe fromThanhPhan_DangBaoCheDB(r_Thanhphan_Dangbaoche item)
        {
            if (item == null)
                return null;
            ThanhPhan_DangBaoChe kq = new ThanhPhan_DangBaoChe
            {
                IDThanhphan = item.IDThanhphan,
                IDDangbaoche = item.IDDangbaoche
            };
            return kq;
        }

        public r_Thanhphan_Dangbaoche toThanhPhan_DangBaoCheDB()
        {
            r_Thanhphan_Dangbaoche kq = new r_Thanhphan_Dangbaoche
            {
                IDThanhphan = this.IDThanhphan,
                IDDangbaoche = this.IDDangbaoche
            };
            return kq;
        }
    }

    public class ThanhPhan_ChucNang
    {
        public int IDThanhphan { get; set; }
        public int IDChucnang { get; set; }

        public ThanhPhan_ChucNang()
        {

        }

        public static ThanhPhan_ChucNang fromThanhPhan_ChucNangDB(r_Thanhphan_Chucnang item)
        {
            if (item == null)
                return null;
            ThanhPhan_ChucNang kq = new ThanhPhan_ChucNang
            {
                IDThanhphan = item.IDThanhphan,
                IDChucnang = item.IDChucnang
            };
            return kq;
        }

        public r_Thanhphan_Chucnang toThanhPhan_ChucNangDB()
        {
            r_Thanhphan_Chucnang kq = new r_Thanhphan_Chucnang
            {
                IDThanhphan = this.IDThanhphan,
                IDChucnang = this.IDChucnang
            };
            return kq;
        }
    }

    public class ChatLienQuan
    {
        public int IDThanhphan { get; set; }
        public int IDThanhphanLienquan { get; set; }

        public ChatLienQuan()
        {

        }

        public static ChatLienQuan fromChatLienQuanDB(r_Chatlienquan item)
        {
            if (item == null)
                return null;
            ChatLienQuan kq = new ChatLienQuan
            {
                IDThanhphan = item.IDThanhphan,
                IDThanhphanLienquan = item.IDThanhphanLienquan
            };
            return kq;
        }

        public r_Chatlienquan toChatLienQuanDB()
        {
            r_Chatlienquan kq = new r_Chatlienquan
            {
                IDThanhphan = this.IDThanhphan,
                IDThanhphanLienquan = this.IDThanhphanLienquan
            };
            return kq;
        }
    }
    #endregion
}
