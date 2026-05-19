using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibraryIE;
using iExcipient_API.Models;

namespace iExcipient_API.Controllers
{
    public class HopeController : Controller
    {
        public ActionResult Index(string q = "", int? func = null, string ewg = "", string[] annex = null)
        {
            ViewBag.ActiveTab = "hope";

            var model = new HopeSearchViewModel
            {
                Keyword = q,
                FunctionId = func,
                EwgFilter = ewg,
                AnnexFilters = annex ?? new string[0]
            };

            var dataHelper = new KetnoiDB.GetData();
            model.Functions = dataHelper.GetDSChucNang().OrderBy(x => x.Tenchucnang).ToList();

            using (var db = new KetnoiCSDLDataContext())
            {
                var query = db.d_Thanhphans.AsQueryable();

                // 1. Keyword search
                if (!string.IsNullOrEmpty(q))
                {
                    string keywordLower = q.Trim().ToLower();
                    query = query.Where(tp => tp.Ten_INN.ToLower().Contains(keywordLower) ||
                                              tp.Ten_INCI.ToLower().Contains(keywordLower) ||
                                              tp.Ten_IUPAC.ToLower().Contains(keywordLower) ||
                                              tp.TenKhac.ToLower().Contains(keywordLower) ||
                                              tp.CAS_No.ToLower().Contains(keywordLower));
                }

                // 2. Filter by Vietnamese Function
                if (func.HasValue && func.Value > 0)
                {
                    query = query.Where(tp => tp.r_Thanhphan_Chucnangs.Any(rc => rc.IDChucnang == func.Value));
                }

                // 3. Filter by EWG Safety Score
                if (!string.IsNullOrEmpty(ewg))
                {
                    if (ewg == "low")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_to <= 2));
                    }
                    else if (ewg == "med")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_from >= 3 && e.EWG_Score_to <= 6));
                    }
                    else if (ewg == "high")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_from >= 7));
                    }
                }

                // 4. Filter by Annex rules
                if (annex != null && annex.Length > 0)
                {
                    foreach (var flag in annex)
                    {
                        string currentFlag = flag;
                        if (currentFlag == "II") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexII == true));
                        if (currentFlag == "III") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexIII == true));
                        if (currentFlag == "IV") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexIV == true));
                        if (currentFlag == "V") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexV == true));
                        if (currentFlag == "VI") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexVI == true));
                    }
                }

                // Fetch top 100 results for performance
                var dbList = query.OrderBy(tp => tp.Ten_INN).Take(100).ToList();
                foreach (var item in dbList)
                {
                    model.Results.Add(ThanhPhan.fromThanhPhanDBShallow(item));
                }

                // If list has elements, load the first element's detail by default
                if (model.Results.Count > 0)
                {
                    int firstId = model.Results[0].IDThanhphan;
                    return RedirectToAction("Detail", new { id = firstId, q = q, func = func, ewg = ewg, annex = annex });
                }
            }

            ViewBag.ActiveId = 0;
            ViewBag.ActiveDetail = null;
            return View(model);
        }

        public ActionResult Detail(int id, string q = "", int? func = null, string ewg = "", string[] annex = null)
        {
            ViewBag.ActiveTab = "hope";

            var model = new HopeSearchViewModel
            {
                Keyword = q,
                FunctionId = func,
                EwgFilter = ewg,
                AnnexFilters = annex ?? new string[0]
            };

            var dataHelper = new KetnoiDB.GetData();
            model.Functions = dataHelper.GetDSChucNang().OrderBy(x => x.Tenchucnang).ToList();

            using (var db = new KetnoiCSDLDataContext())
            {
                var query = db.d_Thanhphans.AsQueryable();

                // 1. Keyword search (same filtering as Index to match sidebar list)
                if (!string.IsNullOrEmpty(q))
                {
                    string keywordLower = q.Trim().ToLower();
                    query = query.Where(tp => tp.Ten_INN.ToLower().Contains(keywordLower) ||
                                              tp.Ten_INCI.ToLower().Contains(keywordLower) ||
                                              tp.Ten_IUPAC.ToLower().Contains(keywordLower) ||
                                              tp.TenKhac.ToLower().Contains(keywordLower) ||
                                              tp.CAS_No.ToLower().Contains(keywordLower));
                }

                // 2. Filter by Vietnamese Function
                if (func.HasValue && func.Value > 0)
                {
                    query = query.Where(tp => tp.r_Thanhphan_Chucnangs.Any(rc => rc.IDChucnang == func.Value));
                }

                // 3. Filter by EWG Safety Score
                if (!string.IsNullOrEmpty(ewg))
                {
                    if (ewg == "low")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_to <= 2));
                    }
                    else if (ewg == "med")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_from >= 3 && e.EWG_Score_to <= 6));
                    }
                    else if (ewg == "high")
                    {
                        query = query.Where(tp => tp.r_Thanhphan_EWGScores.Any(e => e.EWG_Score_from >= 7));
                    }
                }

                // 4. Filter by Annex rules
                if (annex != null && annex.Length > 0)
                {
                    foreach (var flag in annex)
                    {
                        string currentFlag = flag;
                        if (currentFlag == "II") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexII == true));
                        if (currentFlag == "III") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexIII == true));
                        if (currentFlag == "IV") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexIV == true));
                        if (currentFlag == "V") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexV == true));
                        if (currentFlag == "VI") query = query.Where(tp => tp.d_Quydinhs.Any(qd => qd.AnnexVI == true));
                    }
                }

                // Fetch top 100 results for performance
                var dbList = query.OrderBy(tp => tp.Ten_INN).Take(100).ToList();
                foreach (var item in dbList)
                {
                    model.Results.Add(ThanhPhan.fromThanhPhanDBShallow(item));
                }

                // Fetch details of active ingredient
                var activeItem = db.d_Thanhphans.FirstOrDefault(tp => tp.IDThanhphan == id);
                if (activeItem != null)
                {
                    var detailDto = ThanhPhan.fromThanhPhanDB(activeItem);
                    ViewBag.ActiveDetail = detailDto;
                    ViewBag.ActiveId = id;

                    // Load EWG Score separately
                    ViewBag.EwgScore = dataHelper.GetEWGScoreByThanhPhan(id);

                    // Check for CosIng cross-link
                    var cosingLink = db.r_Link_Cosing_Saches.FirstOrDefault(l => l.IDThanhphan == id);
                    if (cosingLink != null)
                    {
                        var cosingItem = db.d_Thanhphan_Cosings.FirstOrDefault(c => c.IDThanhphan_Cosing == cosingLink.IDThanhphan_Cosing);
                        if (cosingItem != null)
                        {
                            ViewBag.CosingLinkId = cosingItem.IDThanhphan_Cosing;
                            ViewBag.CosingLinkName = cosingItem.Ten_INCI;
                        }
                    }
                }
                else
                {
                    ViewBag.ActiveDetail = null;
                    ViewBag.ActiveId = 0;
                }
            }

            return View("Index", model);
        }
    }
}
