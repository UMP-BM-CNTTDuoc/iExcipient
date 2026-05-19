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
            public bool InsertThanhPhanEWGScore(Thanhphan_EWGScore item)
            {
                try
                {
                    // kiểm tra tồn tại
                    bool exists = db.r_Thanhphan_EWGScores
                                    .Any(x => x.IDThanhphan == item.IDThanhphan);

                    if (exists)
                        return false; // đã tồn tại → không insert

                    r_Thanhphan_EWGScore link = item.toThanhphan_EWGScore();

                    db.r_Thanhphan_EWGScores.InsertOnSubmit(link);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool InsertChucNangCosing(ChucNangCosing item)
            {
                try
                {
                    bool exists = db.d_Chucnangcosings
                                    .Any(x => x.Tenchucnangcosing.ToLower() == item.Tenchucnangcosing.ToLower());
                    if (exists)
                        return false;
                    d_Chucnangcosing cn = item.toChucNangCosingDB();
                    db.d_Chucnangcosings.InsertOnSubmit(cn);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool InsertThanhPhan_ChucNangCosing(ThanhPhan_ChucNangCosing item)
            {
                try
                {
                    bool exists = db.r_Thanhphan_Chucnangcosings
                                    .Any(x => x.IDThanhphan_Cosing == item.IDThanhphan_Cosing &&
                                              x.IDChucnangcosing == item.IDChucnangcosing);
                    if (exists)
                        return false;
                    r_Thanhphan_Chucnangcosing link = item.toThanhPhan_ChucNangCosingDB();
                    db.r_Thanhphan_Chucnangcosings.InsertOnSubmit(link);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool InsertThanhPhanCosing(ThanhPhanCosing item)
            {
                try
                {
                    bool exists = db.d_Thanhphan_Cosings
                        .Any(x => x.CAS_No != null && x.CAS_No.Trim() == item.CAS_No.Trim());
                    if (exists) return false;
                    db.d_Thanhphan_Cosings.InsertOnSubmit(item.toThanhPhanCosingDB());
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool InsertQuydinhCosing(QuydinhCosing item)
            {
                try
                {
                    db.d_Quydinh_Cosings.InsertOnSubmit(item.toQuydinhCosingDB());
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool InsertLinkCosingVaSach(LinkCosingVaSach item)
            {
                try
                {
                    bool exists = db.r_Link_Cosing_Saches
                        .Any(x => x.IDThanhphan_Cosing == item.IDThanhphan_Cosing &&
                                  x.IDThanhphan == item.IDThanhphan);
                    if (exists) return false;
                    db.r_Link_Cosing_Saches.InsertOnSubmit(item.toLinkCosingVaSachDB());
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
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
                    // 4. Remove những phần tử bị trùng (nếu không có CAS_No thì cho qua)
                    dsimport = dsimport
                        .Where(x => string.IsNullOrEmpty(x.CAS_No) ||
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
            public bool BulkInsertThanhphan_EWGScore(List<Thanhphan_EWGScore> list)
            {
                try
                {
                    // 1. Chuyển đổi sang DB entities
                    List<r_Thanhphan_EWGScore> dsimport = new List<r_Thanhphan_EWGScore>();
                    foreach (Thanhphan_EWGScore i in list)
                    {
                        r_Thanhphan_EWGScore a = i.toThanhphan_EWGScore();
                        dsimport.Add(a);
                    }

                    // 2. Lấy dữ liệu hiện có
                    List<r_Thanhphan_EWGScore> dshienco = db.r_Thanhphan_EWGScores.ToList();

                    // 3. Tạo HashSet composite key
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.IDThanhphan.ToString())
                    );

                    // 4. Lọc bỏ trùng lặp
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan.ToString()))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false;

                    // 5. Insert
                    db.r_Thanhphan_EWGScores.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool BulkInsertChucNangCosing(List<ChucNangCosing> list)
            {
                try
                {
                    List<d_Chucnangcosing> dsimport = list
                        .Select(i => i.toChucNangCosingDB()).ToList();
                    List<d_Chucnangcosing> dshienco = db.d_Chucnangcosings.ToList();
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.Tenchucnangcosing.Trim().ToLower())
                    );
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.Tenchucnangcosing.Trim().ToLower()))
                        .ToList();
                    if (dsimport.Count == 0) return false;
                    db.d_Chucnangcosings.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool BulkInsertThanhPhan_ChucNangCosing(List<ThanhPhan_ChucNangCosing> list)
            {
                try
                {
                    List<r_Thanhphan_Chucnangcosing> dsimport = new List<r_Thanhphan_Chucnangcosing>();
                    foreach (ThanhPhan_ChucNangCosing i in list)
                        dsimport.Add(i.toThanhPhan_ChucNangCosingDB());

                    List<r_Thanhphan_Chucnangcosing> dshienco = db.r_Thanhphan_Chucnangcosings.ToList();
                    HashSet<string> tapHienCo = new HashSet<string>(dshienco.Select(x => x.IDThanhphan_Cosing + "_" + x.IDChucnangcosing));
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan_Cosing + "_" + x.IDChucnangcosing))
                        .ToList();

                    if (dsimport.Count == 0)
                        return false;

                    db.r_Thanhphan_Chucnangcosings.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool BulkInsertThanhPhanCosing(List<ThanhPhanCosing> list)
            {
                try
                {
                    List<d_Thanhphan_Cosing> dsimport = list.Select(i => i.toThanhPhanCosingDB()).ToList();
                    List<d_Thanhphan_Cosing> dshienco = db.d_Thanhphan_Cosings.ToList();
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Where(x => !string.IsNullOrEmpty(x.CAS_No))
                                .Select(x => x.CAS_No.Trim().ToLower())
                    );
                    dsimport = dsimport
                        .Where(x => string.IsNullOrEmpty(x.CAS_No) ||
                                    !tapHienCo.Contains(x.CAS_No.Trim().ToLower()))
                        .ToList();
                    if (dsimport.Count == 0) return false;
                    db.d_Thanhphan_Cosings.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool BulkInsertQuydinhCosing(List<QuydinhCosing> list)
            {
                try
                {
                    List<d_Quydinh_Cosing> dsimport = list.Select(i => i.toQuydinhCosingDB()).ToList();
                    db.d_Quydinh_Cosings.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool BulkInsertLinkCosingVaSach(List<LinkCosingVaSach> list)
            {
                try
                {
                    List<r_Link_Cosing_Sach> dsimport = list.Select(i => i.toLinkCosingVaSachDB()).ToList();
                    List<r_Link_Cosing_Sach> dshienco = db.r_Link_Cosing_Saches.ToList();
                    HashSet<string> tapHienCo = new HashSet<string>(
                        dshienco.Select(x => x.IDThanhphan_Cosing + "_" + x.IDThanhphan)
                    );
                    dsimport = dsimport
                        .Where(x => !tapHienCo.Contains(x.IDThanhphan_Cosing + "_" + x.IDThanhphan))
                        .ToList();
                    if (dsimport.Count == 0) return false;
                    db.r_Link_Cosing_Saches.InsertAllOnSubmit(dsimport);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
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

            // Lấy toàn bộ Chức Năng
            public List<ChucNang> GetDSChucNang()
            {
                List<ChucNang> kq = new List<ChucNang>();

                List<d_Chucnang> ds = db.d_Chucnangs.ToList();

                foreach (d_Chucnang i in ds)
                    kq.Add(ChucNang.fromChucNangDB(i));

                return kq;
            }


            // Lấy toàn bộ EWG Score
            public List<Thanhphan_EWGScore> GetDSEWGScore()
            {
                List<Thanhphan_EWGScore> kq = new List<Thanhphan_EWGScore>();

                List<r_Thanhphan_EWGScore> ds = db.r_Thanhphan_EWGScores.ToList();

                foreach (r_Thanhphan_EWGScore i in ds)
                    kq.Add(Thanhphan_EWGScore.fromThanhphan_EWGScore(i));

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

            // Lấy EWG Score theo IDThanhPhan
            public Thanhphan_EWGScore GetEWGScoreByThanhPhan(int idThanhPhan)
            {
                try
                {
                    r_Thanhphan_EWGScore item = (from data in db.r_Thanhphan_EWGScores
                                                 where data.IDThanhphan == idThanhPhan
                                                 select data).FirstOrDefault();

                    return Thanhphan_EWGScore.fromThanhphan_EWGScore(item);
                }
                catch
                {
                    return null;
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

            // Lấy toàn bộ Chức Năng Cosing
            public List<ChucNangCosing> GetDSChucNangCosing()
            {
                List<ChucNangCosing> kq = new List<ChucNangCosing>();

                List<d_Chucnangcosing> ds = db.d_Chucnangcosings.ToList();

                foreach (d_Chucnangcosing i in ds)
                    kq.Add(ChucNangCosing.fromChucNangCosingDB(i));

                return kq;
            }

            // Lấy danh sách Chức Năng Cosing theo IDThanhPhan
            public List<ChucNangCosing> GetChucNangCosingByThanhPhan(int idThanhPhan)
            {
                List<ChucNangCosing> kq = new List<ChucNangCosing>();
                try
                {
                    List<d_Chucnangcosing> ds = (from data in db.d_Chucnangcosings
                                                 join rela in db.r_Thanhphan_Chucnangcosings
                                                     on data.IDChucnangcosing equals rela.IDChucnangcosing
                                                 where rela.IDThanhphan_Cosing == idThanhPhan
                                                 select data).ToList();

                    foreach (d_Chucnangcosing i in ds)
                        kq.Add(ChucNangCosing.fromChucNangCosingDB(i));

                    return kq;
                }
                catch
                {
                    return kq;
                }
            }

            // Lấy toàn bộ quan hệ Thành Phần - Chức Năng Cosing
            public List<ThanhPhan_ChucNangCosing> GetDSThanhPhan_ChucNangCosing()
            {
                List<ThanhPhan_ChucNangCosing> kq = new List<ThanhPhan_ChucNangCosing>();

                List<r_Thanhphan_Chucnangcosing> ds = db.r_Thanhphan_Chucnangcosings.ToList();

                foreach (r_Thanhphan_Chucnangcosing i in ds)
                    kq.Add(ThanhPhan_ChucNangCosing.fromThanhPhan_ChucNangCosingDB(i));

                return kq;
            }
            // Lấy toàn bộ ThanhPhanCosing
            public List<ThanhPhanCosing> GetDSThanhPhanCosing()
            {
                List<ThanhPhanCosing> kq = new List<ThanhPhanCosing>();
                List<d_Thanhphan_Cosing> ds = db.d_Thanhphan_Cosings.ToList();
                foreach (d_Thanhphan_Cosing i in ds)
                    kq.Add(ThanhPhanCosing.fromThanhPhanCosingDB(i));
                return kq;
            }

            // Lấy 1 ThanhPhanCosing theo ID
            public ThanhPhanCosing GetThanhPhanCosing(int id)
            {
                d_Thanhphan_Cosing item = db.d_Thanhphan_Cosings
                    .SingleOrDefault(x => x.IDThanhphan_Cosing == id);
                return ThanhPhanCosing.fromThanhPhanCosingDB(item);
            }

            // Lấy ChucNangCosing theo IDThanhphan_Cosing
            public List<ChucNangCosing> GetChucNangCosingByThanhPhanCosing(int idThanhphanCosing)
            {
                List<ChucNangCosing> kq = new List<ChucNangCosing>();
                try
                {
                    List<d_Chucnangcosing> ds = (from data in db.d_Chucnangcosings
                                                 join rela in db.r_Thanhphan_Chucnangcosings
                                                     on data.IDChucnangcosing equals rela.IDChucnangcosing
                                                 where rela.IDThanhphan_Cosing == idThanhphanCosing
                                                 select data).ToList();
                    foreach (d_Chucnangcosing i in ds)
                        kq.Add(ChucNangCosing.fromChucNangCosingDB(i));
                    return kq;
                }
                catch { return kq; }
            }

            // Lấy QuydinhCosing theo IDThanhphan_Cosing
            public List<QuydinhCosing> GetQuydinhCosingByThanhPhanCosing(int idThanhphanCosing)
            {
                List<QuydinhCosing> kq = new List<QuydinhCosing>();
                try
                {
                    List<d_Quydinh_Cosing> ds = db.d_Quydinh_Cosings
                        .Where(x => x.IDThanhphan_Cosing == idThanhphanCosing).ToList();
                    foreach (d_Quydinh_Cosing i in ds)
                        kq.Add(QuydinhCosing.fromQuydinhCosingDB(i));
                    return kq;
                }
                catch { return kq; }
            }

            // Lấy toàn bộ QuydinhCosing
            public List<QuydinhCosing> GetDSQuydinhCosing()
            {
                List<QuydinhCosing> kq = new List<QuydinhCosing>();
                List<d_Quydinh_Cosing> ds = db.d_Quydinh_Cosings.ToList();
                foreach (d_Quydinh_Cosing i in ds)
                    kq.Add(QuydinhCosing.fromQuydinhCosingDB(i));
                return kq;
            }

            // Lấy toàn bộ LinkCosingVaSach
            public List<LinkCosingVaSach> GetDSLinkCosingVaSach()
            {
                List<LinkCosingVaSach> kq = new List<LinkCosingVaSach>();
                List<r_Link_Cosing_Sach> ds = db.r_Link_Cosing_Saches.ToList();
                foreach (r_Link_Cosing_Sach i in ds)
                    kq.Add(LinkCosingVaSach.fromLinkCosingVaSachDB(i));
                return kq;
            }
        }

        #endregion

        #region Update dữ liệu
        public class UpdateData
        {
            public bool UpdateQuyDinh(int idQuydinh, int idThanhphan, bool? annexII, bool? annexIII,
                                      bool? annexIV, bool? annexV, bool? annexVI)
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

            public bool UpdateThanhPhan(int idThanhphan, string ten_INN, string ten_INCI, string ten_IUPAC, string tenKhac,
                                        string cas_No, string congThucHoaHoc, string khoiLuongPhanTu,
                                        string cauTrucPhanTu, string tinhChatVatLy, string moTa,
                                        string baoQuan, string tltk, string ungDung, string tuongKy)
            {
                try
                {
                    d_Thanhphan tp = db.d_Thanhphans.SingleOrDefault(x => x.IDThanhphan == idThanhphan);
                    if (tp != null)
                    {
                        tp.Ten_INN = ten_INN;
                        tp.Ten_INCI = ten_INCI;
                        tp.Ten_IUPAC = ten_IUPAC;
                        tp.TenKhac = tenKhac;
                        tp.CAS_No = cas_No;
                        tp.CongThucHoaHoc = congThucHoaHoc;
                        tp.KhoiLuongPhanTu = khoiLuongPhanTu;
                        tp.CauTrucPhanTu = cauTrucPhanTu;
                        tp.TinhChatVatLy = tinhChatVatLy;
                        tp.MoTa = moTa;
                        tp.BaoQuan = baoQuan;
                        tp.TLTK = tltk;
                        tp.NgayCapNhat = DateTime.Now;
                        tp.Ungdung = ungDung;
                        tp.Tuongky = tuongKy;
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
            public bool UpdateThanhphan_EWGScore(int idThanhphan, int? EWGScore_from, int? EWGScore_to, string EWGScore, string EWGScore_DataAvailability)
            {
                try
                {
                    r_Thanhphan_EWGScore link = db.r_Thanhphan_EWGScores.SingleOrDefault(x =>
                        x.IDThanhphan == idThanhphan);
                    if (link != null)
                    {
                        link.EWG_Score_from = EWGScore_from ?? 0;
                        link.EWG_Score_to = EWGScore_to ?? 0;
                        link.EWG_Score = EWGScore;
                        link.EWG_DataAvailability = EWGScore_DataAvailability;
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
            public bool UpdateChucNangCosing(int idChucnangcosing, string tenChucnangcosing, string motaChucnangcosing)
            {
                try
                {
                    d_Chucnangcosing cn = db.d_Chucnangcosings.SingleOrDefault(x => x.IDChucnangcosing == idChucnangcosing);
                    if (cn != null)
                    {
                        cn.Tenchucnangcosing = tenChucnangcosing;
                        cn.Motachucnangcosing = motaChucnangcosing;
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

            public bool UpdateThanhPhan_ChucNangCosing(int idThanhphan, int idChucnangcosingOld, int idChucnangcosingNew)
            {
                try
                {
                    r_Thanhphan_Chucnangcosing link = db.r_Thanhphan_Chucnangcosings.SingleOrDefault(x =>
                        x.IDThanhphan_Cosing == idThanhphan && x.IDChucnangcosing == idChucnangcosingOld);
                    if (link != null)
                    {
                        link.IDChucnangcosing = idChucnangcosingNew;
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
            public bool UpdateThanhPhanCosing(int id, string tenInci, string casNo, string ecNo)
            {
                try
                {
                    d_Thanhphan_Cosing item = db.d_Thanhphan_Cosings
                        .SingleOrDefault(x => x.IDThanhphan_Cosing == id);
                    if (item == null) return false;
                    item.Ten_INCI = tenInci;
                    item.CAS_No = casNo;
                    item.EC_No = ecNo;
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool UpdateQuydinhCosing(int idQuydinhCosing, int idThanhphanCosing,
                bool? annexII, bool? annexIII, bool? annexIV, bool? annexV, bool? annexVI)
            {
                try
                {
                    d_Quydinh_Cosing item = db.d_Quydinh_Cosings
                        .SingleOrDefault(x => x.IDQuydinh_Cosing == idQuydinhCosing);
                    if (item == null) return false;
                    item.IDThanhphan_Cosing = idThanhphanCosing;
                    item.AnnexII = annexII;
                    item.AnnexIII = annexIII;
                    item.AnnexIV = annexIV;
                    item.AnnexV = annexV;
                    item.AnnexVI = annexVI;
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
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
                    // Delete related Thanhphan_EWGScore records
                    IQueryable<r_Thanhphan_EWGScore> relatedThanhphan_EWGScore = db.r_Thanhphan_EWGScores.Where(x => x.IDThanhphan == idThanhPhan);
                    if (relatedQuyDinh.Any())
                    {
                        db.r_Thanhphan_EWGScores.DeleteAllOnSubmit(relatedThanhphan_EWGScore);
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
            public bool DeleteChucNangCosing(int idChucNangCosing)
            {
                try
                {
                    IQueryable<r_Thanhphan_Chucnangcosing> relatedLinks = db.r_Thanhphan_Chucnangcosings
                        .Where(x => x.IDChucnangcosing == idChucNangCosing);
                    if (relatedLinks.Any())
                        db.r_Thanhphan_Chucnangcosings.DeleteAllOnSubmit(relatedLinks);

                    d_Chucnangcosing cn = db.d_Chucnangcosings.SingleOrDefault(x => x.IDChucnangcosing == idChucNangCosing);
                    if (cn != null)
                    {
                        db.d_Chucnangcosings.DeleteOnSubmit(cn);
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

            public int GetRelatedCountChucNangCosing(int idChucNangCosing)
            {
                try
                {
                    return db.r_Thanhphan_Chucnangcosings.Count(x => x.IDChucnangcosing == idChucNangCosing);
                }
                catch
                {
                    return 0;
                }
            }
            public bool DeleteThanhPhanCosing(int id)
            {
                try
                {
                    // Xóa các quan hệ trước
                    var links = db.r_Link_Cosing_Saches.Where(x => x.IDThanhphan_Cosing == id);
                    db.r_Link_Cosing_Saches.DeleteAllOnSubmit(links);

                    var chucnangs = db.r_Thanhphan_Chucnangcosings.Where(x => x.IDThanhphan_Cosing == id);
                    db.r_Thanhphan_Chucnangcosings.DeleteAllOnSubmit(chucnangs);

                    var quydinhs = db.d_Quydinh_Cosings.Where(x => x.IDThanhphan_Cosing == id);
                    db.d_Quydinh_Cosings.DeleteAllOnSubmit(quydinhs);

                    d_Thanhphan_Cosing item = db.d_Thanhphan_Cosings
                        .SingleOrDefault(x => x.IDThanhphan_Cosing == id);
                    if (item == null) return false;
                    db.d_Thanhphan_Cosings.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool DeleteQuydinhCosing(int idQuydinhCosing)
            {
                try
                {
                    d_Quydinh_Cosing item = db.d_Quydinh_Cosings
                        .SingleOrDefault(x => x.IDQuydinh_Cosing == idQuydinhCosing);
                    if (item == null) return false;
                    db.d_Quydinh_Cosings.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool DeleteLinkCosingVaSach(int idLink)
            {
                try
                {
                    r_Link_Cosing_Sach item = db.r_Link_Cosing_Saches
                        .SingleOrDefault(x => x.IDLink == idLink);
                    if (item == null) return false;
                    db.r_Link_Cosing_Saches.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch { return false; }
            }

            public bool DeleteThanhPhan_ChucNangCosing_ByThanhPhanCosing(int idThanhphanCosing)
            {
                try
                {
                    var links = db.r_Thanhphan_Chucnangcosings
                        .Where(x => x.IDThanhphan_Cosing == idThanhphanCosing).ToList();
                    if (links.Any())
                    {
                        db.r_Thanhphan_Chucnangcosings.DeleteAllOnSubmit(links);
                        db.SubmitChanges();
                    }
                    return true;
                }
                catch { return false; }
            }

            public int GetRelatedCountThanhPhanCosing(int idThanhphanCosing)
            {
                try
                {
                    return db.r_Link_Cosing_Saches.Count(x => x.IDThanhphan_Cosing == idThanhphanCosing);
                }
                catch { return 0; }
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
        public string KhoiLuongPhanTu { get; set; }
        public string CauTrucPhanTu { get; set; }
        public string TinhChatVatLy { get; set; }
        public string MoTa { get; set; }
        public string BaoQuan { get; set; }
        public string TLTK { get; set; }
        public string UngDung { get; set; }
        public string TuongKy { get; set; }
        public string TenKhac { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public List<QuyDinh> dsQuyDinh { get; set; }
        public List<ChucNang> dsChucNang { get; set; }
        public List<ThanhPhan> dsThanhPhanLienQuan { get; set; }
        public List<ChucNangCosing> dsChucNangCosing { get; set; }

        public ThanhPhan()
        {
            Ten_INN = "";
            Ten_INCI = "";
            Ten_IUPAC = "";
            TenKhac = "";
            CAS_No = "";
            CongThucHoaHoc = "";
            CauTrucPhanTu = "";
            TinhChatVatLy = "";
            MoTa = "";
            BaoQuan = "";
            TLTK = "";
            UngDung = "";
            TuongKy = "";
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
                TenKhac = item.TenKhac,
                CAS_No = item.CAS_No,
                CongThucHoaHoc = item.CongThucHoaHoc,
                KhoiLuongPhanTu = item.KhoiLuongPhanTu,
                CauTrucPhanTu = item.CauTrucPhanTu,
                TinhChatVatLy = item.TinhChatVatLy,
                MoTa = item.MoTa,
                BaoQuan = item.BaoQuan,
                TLTK = item.TLTK,
                NgayTao = item.NgayTao,
                NgayCapNhat = item.NgayCapNhat,
                UngDung = item.Ungdung,
                TuongKy = item.Tuongky
            };
            KetnoiDB.GetData db = new KetnoiDB.GetData();
            kq.dsQuyDinh = db.GetQuyDinhByThanhPhan(item.IDThanhphan);
            kq.dsChucNang = db.GetChucNangByThanhPhan(item.IDThanhphan);
            kq.dsThanhPhanLienQuan = db.GetThanhPhanLienQuan(item.IDThanhphan);
            kq.dsChucNangCosing = db.GetChucNangCosingByThanhPhan(item.IDThanhphan);
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
                TenKhac = this.TenKhac,
                CAS_No = this.CAS_No,
                CongThucHoaHoc = this.CongThucHoaHoc,
                KhoiLuongPhanTu = this.KhoiLuongPhanTu,
                CauTrucPhanTu = this.CauTrucPhanTu,
                TinhChatVatLy = this.TinhChatVatLy,
                MoTa = this.MoTa,
                BaoQuan = this.BaoQuan,
                TLTK = this.TLTK,
                NgayTao = this.NgayTao,
                NgayCapNhat = this.NgayCapNhat,
                Ungdung = this.UngDung,
                Tuongky = this.TuongKy
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

        public QuyDinh()
        {
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

    public class ChucNangCosing
    {
        public int IDChucnangcosing { get; set; }
        public string Tenchucnangcosing { get; set; }
        public string Motachucnangcosing { get; set; }

        public ChucNangCosing()
        {
            Motachucnangcosing = "";
        }

        public static ChucNangCosing fromChucNangCosingDB(d_Chucnangcosing item)
        {
            if (item == null)
                return null;
            ChucNangCosing kq = new ChucNangCosing
            {
                IDChucnangcosing = item.IDChucnangcosing,
                Tenchucnangcosing = item.Tenchucnangcosing,
                Motachucnangcosing = item.Motachucnangcosing
            };
            return kq;
        }

        public d_Chucnangcosing toChucNangCosingDB()
        {
            d_Chucnangcosing kq = new d_Chucnangcosing
            {
                IDChucnangcosing = this.IDChucnangcosing,
                Tenchucnangcosing = this.Tenchucnangcosing,
                Motachucnangcosing = this.Motachucnangcosing
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

    public class ThanhPhan_ChucNangCosing
    {
        public int IDThanhphan_Chucnangcosing { get; set; }
        public int IDThanhphan_Cosing { get; set; }
        public int IDChucnangcosing { get; set; }

        public ThanhPhan_ChucNangCosing()
        {

        }

        public static ThanhPhan_ChucNangCosing fromThanhPhan_ChucNangCosingDB(r_Thanhphan_Chucnangcosing item)
        {
            if (item == null)
                return null;
            ThanhPhan_ChucNangCosing kq = new ThanhPhan_ChucNangCosing
            {
                IDThanhphan_Cosing = item.IDThanhphan_Cosing,
                IDChucnangcosing = item.IDChucnangcosing
            };
            return kq;
        }

        public r_Thanhphan_Chucnangcosing toThanhPhan_ChucNangCosingDB()
        {
            r_Thanhphan_Chucnangcosing kq = new r_Thanhphan_Chucnangcosing
            {
                IDThanhphan_Cosing = this.IDThanhphan_Cosing,
                IDChucnangcosing = this.IDChucnangcosing
            };
            return kq;
        }
    }
    public class ThanhPhanCosing
    {
        public int IDThanhphan_Cosing { get; set; }
        public string Ten_INCI { get; set; }
        public string CAS_No { get; set; }
        public string EC_No { get; set; }
        public List<ChucNangCosing> dsChucNangCosing { get; set; }
        public List<QuydinhCosing> dsQuydinhCosing { get; set; }

        public ThanhPhanCosing()
        {
            Ten_INCI = "";
            CAS_No = "";
            EC_No = "";
        }

        public static ThanhPhanCosing fromThanhPhanCosingDB(d_Thanhphan_Cosing item)
        {
            if (item == null) return null;
            ThanhPhanCosing kq = new ThanhPhanCosing
            {
                IDThanhphan_Cosing = item.IDThanhphan_Cosing,
                Ten_INCI = item.Ten_INCI,
                CAS_No = item.CAS_No,
                EC_No = item.EC_No
            };
            KetnoiDB.GetData db = new KetnoiDB.GetData();
            kq.dsChucNangCosing = db.GetChucNangCosingByThanhPhanCosing(item.IDThanhphan_Cosing);
            kq.dsQuydinhCosing = db.GetQuydinhCosingByThanhPhanCosing(item.IDThanhphan_Cosing);
            return kq;
        }

        public d_Thanhphan_Cosing toThanhPhanCosingDB()
        {
            return new d_Thanhphan_Cosing
            {
                IDThanhphan_Cosing = this.IDThanhphan_Cosing,
                Ten_INCI = this.Ten_INCI,
                CAS_No = this.CAS_No,
                EC_No = this.EC_No
            };
        }
    }
    public class QuydinhCosing
    {
        public int IDQuydinh_Cosing { get; set; }
        public int IDThanhphan_Cosing { get; set; }
        public bool? AnnexII { get; set; }
        public bool? AnnexIII { get; set; }
        public bool? AnnexIV { get; set; }
        public bool? AnnexV { get; set; }
        public bool? AnnexVI { get; set; }

        public QuydinhCosing() { }

        public static QuydinhCosing fromQuydinhCosingDB(d_Quydinh_Cosing item)
        {
            if (item == null) return null;
            return new QuydinhCosing
            {
                IDQuydinh_Cosing = item.IDQuydinh_Cosing,
                IDThanhphan_Cosing = item.IDThanhphan_Cosing,
                AnnexII = item.AnnexII,
                AnnexIII = item.AnnexIII,
                AnnexIV = item.AnnexIV,
                AnnexV = item.AnnexV,
                AnnexVI = item.AnnexVI
            };
        }

        public d_Quydinh_Cosing toQuydinhCosingDB()
        {
            return new d_Quydinh_Cosing
            {
                IDQuydinh_Cosing = this.IDQuydinh_Cosing,
                IDThanhphan_Cosing = this.IDThanhphan_Cosing,
                AnnexII = this.AnnexII,
                AnnexIII = this.AnnexIII,
                AnnexIV = this.AnnexIV,
                AnnexV = this.AnnexV,
                AnnexVI = this.AnnexVI
            };
        }
    }
    public class LinkCosingVaSach
    {
        public int IDLink { get; set; }
        public int IDThanhphan_Cosing { get; set; }
        public int IDThanhphan { get; set; }

        public LinkCosingVaSach() { }

        public static LinkCosingVaSach fromLinkCosingVaSachDB(r_Link_Cosing_Sach item)
        {
            if (item == null) return null;
            return new LinkCosingVaSach
            {
                IDLink = item.IDLink,
                IDThanhphan_Cosing = item.IDThanhphan_Cosing,
                IDThanhphan = item.IDThanhphan
            };
        }

        public r_Link_Cosing_Sach toLinkCosingVaSachDB()
        {
            return new r_Link_Cosing_Sach
            {
                IDLink = this.IDLink,
                IDThanhphan_Cosing = this.IDThanhphan_Cosing,
                IDThanhphan = this.IDThanhphan
            };
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

    public class Thanhphan_EWGScore
    {
        public int IDThanhphan { get; set; }
        public int? EWG_Score_from { get; set; }
        public int? EWG_Score_to { get; set; }
        public string EWG_Score { get; set; }
        public string EWG_DataAvailability { get; set; }

        public Thanhphan_EWGScore()
        {

        }
        public static Thanhphan_EWGScore fromThanhphan_EWGScore(r_Thanhphan_EWGScore item)
        {
            if (item == null)
                return null;
            Thanhphan_EWGScore kq = new Thanhphan_EWGScore
            {
                IDThanhphan = item.IDThanhphan,
                EWG_Score_from = item.EWG_Score_from,
                EWG_Score_to = item.EWG_Score_to,
                EWG_Score = PhanLoaiDoAnToan(item.EWG_Score_to),
                EWG_DataAvailability = item.EWG_DataAvailability
            };
            return kq;
        }

        public r_Thanhphan_EWGScore toThanhphan_EWGScore()
        {
            r_Thanhphan_EWGScore kq = new r_Thanhphan_EWGScore
            {
                IDThanhphan = this.IDThanhphan,
                EWG_Score_from = this.EWG_Score_from,
                EWG_Score_to = this.EWG_Score_to,
                EWG_Score = this.EWG_Score,
                EWG_DataAvailability = this.EWG_DataAvailability
            };
            return kq;
        }

        public static string PhanLoaiDoAnToan(int? scoreTo)
        {
            if (scoreTo == null)
                return string.Empty;

            int val = scoreTo.Value;

            if (val >= 1 && val <= 2)
                return "Nguy cơ thấp";
            else if (val >= 3 && val <= 6)
                return "Nguy cơ trung bình";
            else if (val >= 7 && val <= 10)
                return "Nguy cơ cao";
            else
                return string.Empty;
        }
    }
    #endregion
}
