using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibraryIE;
using iExcipient_API.Models;

namespace iExcipient_API.Controllers
{
    public class TraCuuController : Controller
    {
        KetnoiDB.GetData data = new KetnoiDB.GetData();

        public ActionResult Index(string keyword, int? chucnang, int? dangbaoche,
            bool? annexII, bool? annexIII, bool? annexIV, bool? annexV, bool? annexVI)
        {
            SearchVM model = new SearchVM
            {
                Keyword = keyword,
                ChucNangID = chucnang,
                DangBaoCheID = dangbaoche,
                AnnexII = annexII,
                AnnexIII = annexIII,
                AnnexIV = annexIV,
                AnnexV = annexV,
                AnnexVI = annexVI,
                DSChucNang = data.GetDSChucNang(),
                DSDangBaoChe = data.GetDSDangBaoChe()
            };

            List<ThanhPhan> result = new List<ThanhPhan>();

            // Tìm theo keyword
            if (!string.IsNullOrEmpty(keyword))
                result = data.SearchThanhPhan(keyword);
            else
                result = data.GetDSThanhPhan();

            // Lọc theo chức năng
            if (chucnang.HasValue)
                result = result.Where(tp => tp.dsChucNang.Any(cn => cn.IDChucnang == chucnang.Value)).ToList();

            // Lọc theo dạng bào chế
            if (dangbaoche.HasValue)
                result = result.Where(tp => tp.dsDangBaoChe.Any(db => db.IDDangbaoche == dangbaoche.Value)).ToList();

            // Lọc theo Annex
            if (annexII == true)
                result = result.Where(tp => tp.dsQuyDinh.Any(qd => qd.AnnexII == true)).ToList();
            if (annexIII == true)
                result = result.Where(tp => tp.dsQuyDinh.Any(qd => qd.AnnexIII == true)).ToList();
            if (annexIV == true)
                result = result.Where(tp => tp.dsQuyDinh.Any(qd => qd.AnnexIV == true)).ToList();
            if (annexV == true)
                result = result.Where(tp => tp.dsQuyDinh.Any(qd => qd.AnnexV == true)).ToList();
            if (annexVI == true)
                result = result.Where(tp => tp.dsQuyDinh.Any(qd => qd.AnnexVI == true)).ToList();

            model.KetQua = result;
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var tp = data.GetThanhPhan(id);
            return View(tp);
        }
    }
}
