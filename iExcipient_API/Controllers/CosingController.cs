using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibraryIE;
using iExcipient_API.Models;

namespace iExcipient_API.Controllers
{
    public class CosingController : Controller
    {
        public ActionResult Index(string q = "", string inci = "", string cas = "", string ec = "", int? func = null, string[] annex = null)
        {
            ViewBag.ActiveTab = "cosing";

            var model = new CosingSearchViewModel
            {
                Keyword = q,
                FunctionId = func,
                AnnexFilters = annex ?? new string[0]
            };

            var dataHelper = new KetnoiDB.GetData();
            model.Functions = dataHelper.GetDSChucNangCosing().OrderBy(x => x.Tenchucnangcosing).ToList();

            using (var db = new KetnoiCSDLDataContext())
            {
                var query = db.d_Thanhphan_Cosings.AsQueryable();

                // 1. Basic keyword search
                if (!string.IsNullOrEmpty(q))
                {
                    string keywordLower = q.Trim().ToLower();
                    query = query.Where(tp => tp.Ten_INCI.ToLower().Contains(keywordLower) ||
                                              tp.CAS_No.ToLower().Contains(keywordLower) ||
                                              tp.EC_No.ToLower().Contains(keywordLower));
                }
                else
                {
                    // Advanced search parameters
                    if (!string.IsNullOrEmpty(inci))
                    {
                        string inciLower = inci.Trim().ToLower();
                        query = query.Where(tp => tp.Ten_INCI.ToLower().Contains(inciLower));
                    }
                    if (!string.IsNullOrEmpty(cas))
                    {
                        string casLower = cas.Trim().ToLower();
                        query = query.Where(tp => tp.CAS_No.ToLower().Contains(casLower));
                    }
                    if (!string.IsNullOrEmpty(ec))
                    {
                        string ecLower = ec.Trim().ToLower();
                        query = query.Where(tp => tp.EC_No.ToLower().Contains(ecLower));
                    }
                }

                // 2. Filter by CosIng Function
                if (func.HasValue && func.Value > 0)
                {
                    query = query.Where(tp => tp.r_Thanhphan_Chucnangcosings.Any(rc => rc.IDChucnangcosing == func.Value));
                }

                // 3. Filter by Annex rules
                if (annex != null && annex.Length > 0)
                {
                    foreach (var flag in annex)
                    {
                        string currentFlag = flag;
                        if (currentFlag == "II") query = query.Where(tp => tp.d_Quydinh_Cosings.Any(qd => qd.AnnexII == true));
                        if (currentFlag == "III") query = query.Where(tp => tp.d_Quydinh_Cosings.Any(qd => qd.AnnexIII == true));
                        if (currentFlag == "IV") query = query.Where(tp => tp.d_Quydinh_Cosings.Any(qd => qd.AnnexIV == true));
                        if (currentFlag == "V") query = query.Where(tp => tp.d_Quydinh_Cosings.Any(qd => qd.AnnexV == true));
                        if (currentFlag == "VI") query = query.Where(tp => tp.d_Quydinh_Cosings.Any(qd => qd.AnnexVI == true));
                    }
                }

                // Fetch top 100 for high performance
                var dbList = query.OrderBy(tp => tp.Ten_INCI).Take(100).ToList();
                foreach (var item in dbList)
                {
                    model.Results.Add(ThanhPhanCosing.fromThanhPhanCosingDB(item));
                }

                // Load cross-links to HOPE
                var cosingIds = model.Results.Select(r => r.IDThanhphan_Cosing).ToList();
                var links = db.r_Link_Cosing_Saches
                    .Where(l => cosingIds.Contains(l.IDThanhphan_Cosing))
                    .ToList();

                ViewBag.HopeLinks = links
                    .GroupBy(l => l.IDThanhphan_Cosing)
                    .ToDictionary(g => g.Key, g => g.First().IDThanhphan);
            }

            ViewBag.Inci = inci;
            ViewBag.Cas = cas;
            ViewBag.Ec = ec;

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.ActiveTab = "cosing";

            using (var db = new KetnoiCSDLDataContext())
            {
                var activeItem = db.d_Thanhphan_Cosings.FirstOrDefault(tp => tp.IDThanhphan_Cosing == id);
                if (activeItem == null)
                {
                    return HttpNotFound("Ingredient not found in CosIng database.");
                }

                var detailDto = ThanhPhanCosing.fromThanhPhanCosingDB(activeItem);
                ViewBag.ActiveDetail = detailDto;

                // Load cross-link to HOPE Excipient
                var hopeLink = db.r_Link_Cosing_Saches.FirstOrDefault(l => l.IDThanhphan_Cosing == id);
                if (hopeLink != null)
                {
                    var hopeItem = db.d_Thanhphans.FirstOrDefault(h => h.IDThanhphan == hopeLink.IDThanhphan);
                    if (hopeItem != null)
                    {
                        ViewBag.HopeLinkId = hopeItem.IDThanhphan;
                        ViewBag.HopeLinkName = hopeItem.Ten_INN;
                    }
                }
            }

            return View();
        }
    }
}
