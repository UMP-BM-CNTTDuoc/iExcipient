using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    [Table("d_Quydinh")]
    public class Quydinh
    {
        [Key]
        public int IDQuydinh { get; set; }

        public int IDThanhphan { get; set; }

        public bool? AnnexII { get; set; }

        public bool? AnnexIII { get; set; }

        public bool? AnnexIV { get; set; }

        public bool? AnnexV { get; set; }

        public bool? AnnexVI { get; set; }

        public string DieuKienSuDung { get; set; }

        public string Ghichu { get; set; }

        // Navigation properties
        [ForeignKey("IDThanhphan")]
        public virtual Thanhphan Thanhphan { get; set; }
    }
}