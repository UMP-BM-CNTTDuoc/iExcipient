using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    [Table("d_Thanhphan")]
    public class Thanhphan
    {
        [Key]
        public int IDThanhphan { get; set; }

        [StringLength(500)]
        public string Ten_INN { get; set; }

        [StringLength(500)]
        public string Ten_ICI { get; set; }

        [StringLength(1000)]
        public string Ten_IUPAC { get; set; }

        [StringLength(100)]
        public string CAS_No { get; set; }

        [StringLength(500)]
        public string CongThucHoaHoc { get; set; }

        [StringLength(200)]
        public string KhoiLuongPhanTu { get; set; }

        public string CauTrucPhanTu { get; set; }

        public string TinhChatVatLy { get; set; }

        public string MoTa { get; set; }

        public string BaoQuan { get; set; }

        public string TLTK { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        // Navigation properties
        public virtual ICollection<ThanhphanDangbaoche> ThanhphanDangbaoches { get; set; }
        public virtual ICollection<ThanhphanChucnang> ThanhphanChucnangs { get; set; }
        public virtual ICollection<Quydinh> Quydinhs { get; set; }
        public virtual ICollection<ChatlienquanSource> ChatlienquanSources { get; set; }
        public virtual ICollection<ChatlienquanRelated> ChatlienquanRelateds { get; set; }
    }
}
