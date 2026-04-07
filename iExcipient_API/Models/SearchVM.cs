using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    public class SearchVM
    {
        public string Keyword { get; set; }

        public int? ChucNangID { get; set; }

        public int? DangBaoCheID { get; set; }

        public List<ThanhPhan> KetQua { get; set; }

        public List<ChucNang> DSChucNang { get; set; }

        public List<DangBaoChe> DSDangBaoChe { get; set; }
    }
}