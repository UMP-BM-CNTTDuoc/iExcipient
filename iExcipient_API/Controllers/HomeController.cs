using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibraryIE;

namespace iExcipient_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "home";

            try
            {
                var dataHelper = new KetnoiDB.GetData();
                ViewBag.CountHope = dataHelper.CountThanhPhan();
                ViewBag.CountCosing = dataHelper.CountThanhPhanCosing();
                ViewBag.CountAnnex = dataHelper.CountQuyDinh() + dataHelper.CountQuydinhCosingT();
                ViewBag.CountEwg = dataHelper.GetDSEWGScore().Count;
            }
            catch (Exception)
            {
                ViewBag.CountHope = 0;
                ViewBag.CountCosing = 0;
                ViewBag.CountAnnex = 0;
                ViewBag.CountEwg = 0;
            }

            return View();
        }
    }
}
