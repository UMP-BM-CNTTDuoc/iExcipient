using System;
using System.Collections.Generic;
using ClassLibraryIE;

namespace iExcipient_API.Models
{
    public class HopeSearchViewModel
    {
        public string Keyword { get; set; }
        public int? FunctionId { get; set; }
        public string EwgFilter { get; set; }
        public string[] AnnexFilters { get; set; }
        public List<ThanhPhan> Results { get; set; }
        public List<ChucNang> Functions { get; set; }

        public HopeSearchViewModel()
        {
            Results = new List<ThanhPhan>();
            Functions = new List<ChucNang>();
            AnnexFilters = new string[0];
        }
    }

    public class CosingSearchViewModel
    {
        public string Keyword { get; set; }
        public int? FunctionId { get; set; }
        public string[] AnnexFilters { get; set; }
        public List<ThanhPhanCosing> Results { get; set; }
        public List<ChucNangCosing> Functions { get; set; }

        public CosingSearchViewModel()
        {
            Results = new List<ThanhPhanCosing>();
            Functions = new List<ChucNangCosing>();
            AnnexFilters = new string[0];
        }
    }
}
