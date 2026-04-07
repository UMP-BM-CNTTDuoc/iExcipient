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

        public ActionResult Index(string keyword, int? chucnang, int? dangbaoche)
        {
            SearchVM model = new SearchVM();

            model.Keyword = keyword;
            model.ChucNangID = chucnang;
            model.DangBaoCheID = dangbaoche;

            model.DSChucNang = data.GetDSChucNang();
            model.DSDangBaoChe = data.GetDSDangBaoChe();

            List<ThanhPhan> result = new List<ThanhPhan>();

            if (!string.IsNullOrEmpty(keyword))
                result = data.SearchThanhPhan(keyword);

            else if (chucnang.HasValue)
                result = data.GetThanhPhanByChucNang(chucnang.Value);

            else if (dangbaoche.HasValue)
                result = data.GetThanhPhanByDangBaoChe(dangbaoche.Value);

            else
                result = data.GetDSThanhPhan();

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
