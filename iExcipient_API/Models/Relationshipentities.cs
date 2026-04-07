using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    [Table("r_Thanhphan_Dangbaoche")]
    public class ThanhphanDangbaoche
    {
        [Key, Column(Order = 0)]
        public int IDThanhphan { get; set; }

        [Key, Column(Order = 1)]
        public int IDDangbaoche { get; set; }

        // Navigation properties
        [ForeignKey("IDThanhphan")]
        public virtual Thanhphan Thanhphan { get; set; }

        [ForeignKey("IDDangbaoche")]
        public virtual Dangbaoche Dangbaoche { get; set; }
    }

    [Table("r_Thanhphan_Chucnang")]
    public class ThanhphanChucnang
    {
        [Key, Column(Order = 0)]
        public int IDThanhphan { get; set; }

        [Key, Column(Order = 1)]
        public int IDChucnang { get; set; }

        // Navigation properties
        [ForeignKey("IDThanhphan")]
        public virtual Thanhphan Thanhphan { get; set; }

        [ForeignKey("IDChucnang")]
        public virtual Chucnang Chucnang { get; set; }
    }

    [Table("r_Chatlienquan")]
    public class ChatlienquanSource
    {
        [Key, Column(Order = 0)]
        public int IDThanhphan { get; set; }

        [Key, Column(Order = 1)]
        public int IDThanhphanLienquan { get; set; }

        // Navigation properties
        [ForeignKey("IDThanhphan")]
        public virtual Thanhphan SourceThanhphan { get; set; }

        [ForeignKey("IDThanhphanLienquan")]
        public virtual Thanhphan RelatedThanhphan { get; set; }
    }

    [Table("r_Chatlienquan")]
    public class ChatlienquanRelated
    {
        [Key, Column(Order = 0)]
        public int IDThanhphan { get; set; }

        [Key, Column(Order = 1)]
        public int IDThanhphanLienquan { get; set; }

        // Navigation properties
        [ForeignKey("IDThanhphanLienquan")]
        public virtual Thanhphan SourceThanhphan { get; set; }

        [ForeignKey("IDThanhphan")]
        public virtual Thanhphan RelatedThanhphan { get; set; }
    }
}