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
            var annexFilters = annex ?? new string[0];

            var model = new HopeSearchViewModel
            {
                Keyword = q,
                FunctionId = func,
                EwgFilter = ewg,
                AnnexFilters = annexFilters
            };

            var dataHelper = new KetnoiDB.GetData();
            model.Functions = dataHelper.GetDSChucNang().OrderBy(x => x.Tenchucnang).ToList();

            // HỨNG TRỰC TIẾP: Nhận List<ThanhPhan> đã được lọc AND sạch sẽ từ tầng dữ liệu
            model.Results = dataHelper.GetHopeIngredients(q, func, ewg, annexFilters);

            if (model.Results.Count > 0)
            {
                int firstId = model.Results[0].IDThanhphan;
                string annexJoined = annexFilters.Length > 0 ? string.Join(",", annexFilters) : "";

                return RedirectToAction("Detail", new
                {
                    id = firstId,
                    q = q,
                    func = func,
                    ewg = ewg,
                    annex = annexJoined
                });
            }

            ViewBag.ActiveId = 0;
            ViewBag.ActiveDetail = null;

            return View(model);
        }

        public ActionResult Detail(int id, string q = "", int? func = null, string ewg = "", string annex = "")
        {
            ViewBag.ActiveTab = "hope";

            string[] annexArray = !string.IsNullOrEmpty(annex)
                ? annex.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                : new string[0];

            var model = new HopeSearchViewModel
            {
                Keyword = q,
                FunctionId = func,
                EwgFilter = ewg,
                AnnexFilters = annexArray
            };

            var dataHelper = new KetnoiDB.GetData();
            model.Functions = dataHelper.GetDSChucNang().OrderBy(x => x.Tenchucnang).ToList();

            // HỨNG TRỰC TIẾP: Điền lại danh sách kết quả Sidebar thỏa mãn điều kiện lọc AND
            model.Results = dataHelper.GetHopeIngredients(q, func, ewg, annexArray);

            // Lấy chi tiết tá dược hiển thị ở content bên phải (Giữ nguyên logic nạp đầy đủ)
            using (var db = new KetnoiCSDLDataContext())
            {
                var activeItem = db.d_Thanhphans.FirstOrDefault(tp => tp.IDThanhphan == id);
                if (activeItem != null)
                {
                    ViewBag.ActiveDetail = ThanhPhan.fromThanhPhanDB(activeItem);
                    ViewBag.ActiveId = id;
                    ViewBag.EwgScore = dataHelper.GetEWGScoreByThanhPhan(id);

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
