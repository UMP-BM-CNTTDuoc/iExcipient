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
        #region Nhập liệu đơn
        public class InsertData
        {
            /*
            public bool InsertQuyDinh(QuyDinh item)
            {
                // Input: QuyDinh item (DTO)
                // Output: bool
            }
            public bool InsertThanhPhan(ThanhPhan item)
            {
                // Input: ThanhPhan item
                // Output: bool
            }
            public bool InsertDangBaoChe(DangBaoChe item)
            {
                // Input: DangBaoChe item
                // Output: bool
            }
            public bool InsertChucNang(ChucNang item)
            {
                // Input: ChucNang item
                // Output: bool
            }
            public bool InsertThanhPhan_ChucNang(ThanhPhan_ChucNang item)
            {
                // Input: ThanhPhan_ChucNang item
                // Output: bool
            }

            public bool InsertThanhPhan_DangBaoChe(ThanhPhan_DangBaoChe item)
            {
                // Input: ThanhPhan_DangBaoChe item
                // Output: bool
            }

            public bool InsertThanhPhan_ChatLienQuan(ThanhPhan_ChatLienQuan item)
            {
                // Input: ThanhPhan_ChatLienQuan item
                // Output: bool
            }
            */
        }
        #endregion

        #region Nhập hàng loạt
        public class BulkInsertData
        {
            /*
            public bool BulkInsertQuyDinh(List<QuyDinh> list)
            {
                // Input: List<QuyDinh>
                // Output: bool
            }

            public bool BulkInsertThanhPhan(List<ThanhPhan> list)
            {
                // Input: List<ThanhPhan>
                // Output: bool
            }

            public bool BulkInsertDangBaoChe(List<DangBaoChe> list)
            {
                // Input: List<DangBaoChe>
                // Output: bool
            }

            public bool BulkInsertChucNang(List<ChucNang> list)
            {
                // Input: List<ChucNang>
                // Output: bool
            }
            public bool BulkInsertThanhPhan_ChucNang(List<ThanhPhan_ChucNang> listThanhPhan_ChucNang)
            {
                // Input: List<ThanhPhan_ChucNang>
                // Output: bool
            }

            public bool BulkInsertThanhPhan_DangBaoChe(List<ThanhPhan_DangBaoChe> listThanhPhan_DangBaoChe)
            {
                // Input: List<ThanhPhan_DangBaoChe>
                // Output: bool
            }

            public bool BulkInsertThanhPhan_ChatLienQuan(List<ThanhPhan_ChatLienQuan> listThanhPhan_ChatLienQuan)
            {
                // Input: List<ThanhPhan_ChatLienQuan>
                // Output: bool
            }

            */
        }

        #endregion

        #region Lấy dữ liệu
        public class GetData
        {
            /*
            public List<QuyDinh> GetDSQuyDinh()
            {
                // Input: không
                // Output: List<QuyDinh>
            }

            public List<ThanhPhan> GetDSThanhPhan()
            {
                // Input: không
                // Output: List<ThanhPhan>
            }

            public List<DangBaoChe> GetDSDangBaoChe()
            {
                // Input: không
                // Output: List<DangBaoChe>
            }

            public List<ChucNang> GetDSChucNang()
            {
                // Input: không
                // Output: List<ChucNang>
            }
            
            // Lấy toàn bộ quan hệ ThànhPhan - ChucNang
            public List<ThanhPhan_ChucNang> GetDSThanhPhan_ChucNang()
            {
                // Input: không
                // Output: List<ThanhPhan_ChucNang>
            }

            // Lấy toàn bộ quan hệ ThànhPhan - DangBaoChe
            public List<ThanhPhan_DangBaoChe> GetDSThanhPhan_DangBaoChe()
            {
                // Input: không
                // Output: List<ThanhPhan_DangBaoChe>
            }

            // Lấy toàn bộ quan hệ ThànhPhan - ChatLienQuan
            public List<ThanhPhan_ChatLienQuan> GetDSThanhPhan_ChatLienQuan()
            {
                // Input: không
                // Output: List<ThanhPhan_ChatLienQuan>
            }

            */
        }

        #endregion

        #region Update dữ liệu
        public class UpdateData
        {
            /*
            public bool UpdateQuyDinh(
                            int idQuyDinh,
                            int idThanhPhan,
                            bool annexII,
                            bool annexIII,
                            bool annexIV,
                            bool annexV,
                            bool annexVI,
                            string dieuKienSuDung,
                            string ghiChu)
            {
                // Input: từng trường tương ứng control trong DanhMucQuyDinh
                // Output: bool
            }

            public bool UpdateThanhPhan(
                            int idThanhPhan,
                            string tenINN,
                            string tenINCI,
                            string tenIUPAC,
                            string casNo,
                            string congThucHoaHoc,
                            string cauTrucPhanTu,
                            string khoiLuongPhanTu,
                            string tinhChatVatLy,
                            string moTa,
                            string baoQuan,
                            string chatLienQuan,
                            string tltk,
                            DateTime ngayTao,
                            DateTime ngayCapNhat)
            {
                // Input: từng trường tương ứng control DanhMucThanhPhan
                // Output: bool
            }

            public bool UpdateDangBaoChe(int idDangBaoChe, string tenDangBaoChe)
            {
                // Input: int idDangBaoChe, string tenDangBaoChe
                // Output: bool
            }

            public bool UpdateChucNang(int idChucNang, string tenChucNang, string moTa)
            {
                // Input: int idChucNang, string tenChucNang, string moTa
                // Output: bool
            }
            */
        }
        #endregion

        #region Xóa dữ liệu
        public class DeleteData
        {
            /*
            public bool DeleteQuyDinh(int idQuyDinh)
            {
                // Input: int idQuyDinh
                // Output: bool
            }

            public bool DeleteThanhPhan(int idThanhPhan)
            {
                // Input: int idThanhPhan
                // Output: bool
            }

            public bool DeleteDangBaoChe(int idDangBaoChe)
            {
                // Input: int idDangBaoChe
                // Output: bool
            }

            public bool DeleteChucNang(int idChucNang)
            {
                // Input: int idChucNang
                // Output: bool
            }
            */
        }

        #endregion
    }
    #endregion
    #region Data class

    #endregion
}
