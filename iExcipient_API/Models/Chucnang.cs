using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    [Table("d_Chucnang")]
    public class Chucnang
    {
        [Key]
        public int IDChucnang { get; set; }

        [Required]
        [StringLength(200)]
        public string Tenchucnang { get; set; }

        public string Motachucnang { get; set; }

        // Navigation properties
        public virtual ICollection<ThanhphanChucnang> ThanhphanChucnangs { get; set; }
    }
}