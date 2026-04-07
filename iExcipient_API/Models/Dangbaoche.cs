using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    [Table("d_Dangbaoche")]
    public class Dangbaoche
    {
        [Key]
        public int IDDangbaoche { get; set; }

        [Required]
        [StringLength(200)]
        public string TenDangbaoche { get; set; }

        // Navigation properties
        public virtual ICollection<ThanhphanDangbaoche> ThanhphanDangbaoches { get; set; }
    }
}