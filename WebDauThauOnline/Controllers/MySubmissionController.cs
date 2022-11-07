using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebDauThauOnline.Models;

namespace WebDauThauOnline.Controllers
{
    public class MySubmissionController : Controller
    {
        private readonly KetQuaLuaChonNhaThauEntities db2 = new KetQuaLuaChonNhaThauEntities();
        private readonly ThongBaoMoiThauEntities db1 = new ThongBaoMoiThauEntities();
        SearchViewModel searchViewModel = new SearchViewModel();
        [HttpPost]
        public ActionResult ChangeTimeRange(Khoảng_thời_gian khoangThoiGian)
        {
            int timeInterval;
            switch (khoangThoiGian)
            {
                case Khoảng_thời_gian.mặc_định:
                    timeInterval = 0;
                    break;
                case Khoảng_thời_gian.một_tuần_gần_nhất:
                    timeInterval = 7;
                    break;
                case Khoảng_thời_gian.sáu_tuần_gần_nhất:
                    timeInterval = 42;
                    break;
                case Khoảng_thời_gian.một_tháng_gần_nhất:
                    timeInterval = 30;
                    break;
                case Khoảng_thời_gian.một_năm_gần_nhất:
                    timeInterval = 365;
                    break;
                default:
                    timeInterval = 0;
                    break;
            }
            return Json(timeInterval, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string filter, int? page)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Accounts");
            }
            var Kiểu_thông_tin_EnumData = from Kiểu_thông_tin e in Enum.GetValues(typeof(Kiểu_thông_tin))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phạm_vi_EnumData = from Phạm_vi e in Enum.GetValues(typeof(Phạm_vi))
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToDescriptionString()
                                   };
            var Loại_ngày_EnumData = from Loại_ngày e in Enum.GetValues(typeof(Loại_ngày))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Khoảng_thời_gian_EnumData = from Khoảng_thời_gian e in Enum.GetValues(typeof(Khoảng_thời_gian))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            ViewBag.Kiểu_thông_tin_EnumList = new SelectList(Kiểu_thông_tin_EnumData, "ID", "Name");
            ViewBag.Phạm_vi_EnumList = new SelectList(Phạm_vi_EnumData, "ID", "Name");
            ViewBag.Loại_ngày_EnumList = new SelectList(Loại_ngày_EnumData, "ID", "Name");
            ViewBag.Khoảng_thời_gian_EnumList = new SelectList(Khoảng_thời_gian_EnumData, "ID", "Name");
            ViewBag.Hình_thức_EnumList = new SelectList(Hình_thức_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            string currentFilter = (filter ?? "ThongBaoMoiThau");

            ViewBag.filter = currentFilter;
            if (currentFilter == "KetQuaLuaChonNhaThau")
            {
                searchViewModel.ketQuaLuaChonNhaThauModel = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]).OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
                ViewBag.IsEmpty = false;
                if(searchViewModel.ketQuaLuaChonNhaThauModel.Count() == 0)
                {
                    ViewBag.IsEmpty = true;
                }
            }
            else if (currentFilter == "ThongBaoMoiThau")
            {
                searchViewModel.thongBaoMoiThauModel = db1.ThongBaoMoiThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]).OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
                ViewBag.IsEmpty = false;
                if (searchViewModel.thongBaoMoiThauModel.Count() == 0)
                {
                    ViewBag.IsEmpty = true;
                }
            }
            return View(searchViewModel);
        }

        public ActionResult SearchResult(int? page)
        {
            var Kiểu_thông_tin_EnumData = from Kiểu_thông_tin e in Enum.GetValues(typeof(Kiểu_thông_tin))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phạm_vi_EnumData = from Phạm_vi e in Enum.GetValues(typeof(Phạm_vi))
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToDescriptionString()
                                   };
            var Loại_ngày_EnumData = from Loại_ngày e in Enum.GetValues(typeof(Loại_ngày))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Khoảng_thời_gian_EnumData = from Khoảng_thời_gian e in Enum.GetValues(typeof(Khoảng_thời_gian))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            ViewBag.Kiểu_thông_tin_EnumList = new SelectList(Kiểu_thông_tin_EnumData, "ID", "Name");
            ViewBag.Phạm_vi_EnumList = new SelectList(Phạm_vi_EnumData, "ID", "Name");
            ViewBag.Loại_ngày_EnumList = new SelectList(Loại_ngày_EnumData, "ID", "Name");
            ViewBag.Khoảng_thời_gian_EnumList = new SelectList(Khoảng_thời_gian_EnumData, "ID", "Name");
            ViewBag.Hình_thức_EnumList = new SelectList(Hình_thức_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            searchViewModel.thongBaoMoiThauModel = db1.ThongBaoMoiThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]).OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
            searchViewModel.ketQuaLuaChonNhaThauModel = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]).OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

            return View(searchViewModel);
        }

        [HttpPost]
        public ActionResult SearchResult(SearchViewModel searchViewModel, int? page)
        {
            var Kiểu_thông_tin_EnumData = from Kiểu_thông_tin e in Enum.GetValues(typeof(Kiểu_thông_tin))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phạm_vi_EnumData = from Phạm_vi e in Enum.GetValues(typeof(Phạm_vi))
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToDescriptionString()
                                   };
            var Loại_ngày_EnumData = from Loại_ngày e in Enum.GetValues(typeof(Loại_ngày))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Khoảng_thời_gian_EnumData = from Khoảng_thời_gian e in Enum.GetValues(typeof(Khoảng_thời_gian))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            ViewBag.Kiểu_thông_tin_EnumList = new SelectList(Kiểu_thông_tin_EnumData, "ID", "Name");
            ViewBag.Phạm_vi_EnumList = new SelectList(Phạm_vi_EnumData, "ID", "Name");
            ViewBag.Loại_ngày_EnumList = new SelectList(Loại_ngày_EnumData, "ID", "Name");
            ViewBag.Khoảng_thời_gian_EnumList = new SelectList(Khoảng_thời_gian_EnumData, "ID", "Name");
            ViewBag.Hình_thức_EnumList = new SelectList(Hình_thức_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            /*            if (ModelState.IsValid)
                        {*/
            this.searchViewModel = searchViewModel;
            var searchModel = searchViewModel.searchModel;
            if (searchModel.Kiểu_thông_tin == Kiểu_thông_tin.Thông_báo_mời_thầu)
            {
                var result = db1.ThongBaoMoiThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]);

                if (searchModel.Số_TBMT_Tên_gói_thầu != null)
                    result = result.Where(x => x.Số_TBMT.Contains(searchModel.Số_TBMT_Tên_gói_thầu) || x.Tên_gói_thầu.Contains(searchModel.Số_TBMT_Tên_gói_thầu));
                if (searchModel.Bên_mời_thầu != null)
                    result = result.Where(x => x.Bên_mời_thầu.Contains(searchModel.Bên_mời_thầu));
                /*if (searchModel.Phạm_vi != null)*/

                var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
                var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

                switch (searchModel.Loại_ngày)
                {
                    case Loại_ngày.Ngày_đăng_tải:
                        result = result.Where(x => x.Thời_điểm_đăng_tải >= Từ_ngày);
                        if (searchModel.Đến_ngày != null)
                        {
                            result = result.Where(x => x.Thời_điểm_đăng_tải <= Đến_ngày);
                        }
                        break;
                    case Loại_ngày.Ngày_đóng_thầu:
                        result = result.Where(x => x.Thời_điểm_đóng_mở_thầu >= Từ_ngày);
                        if (searchModel.Đến_ngày != null)
                        {
                            result = result.Where(x => x.Thời_điểm_đóng_mở_thầu <= Đến_ngày);
                        }
                        break;
                    case Loại_ngày.Ngày_phát_hành_HSMT:
                        result = result.Where(x => x.Thời_gian_nhận_E_HSDT_từ_ngày >= Từ_ngày);
                        if (searchModel.Đến_ngày != null)
                        {
                            result = result.Where(x => x.Thời_gian_nhận_E_HSDT_từ_ngày <= Đến_ngày);
                        }
                        break;
                    default:
                        break;
                }
                if (searchModel.Hình_thức != null)
                    result = result.Where(x => x.Hình_thức_dự_thầu.Contains(searchModel.Hình_thức.ToDescriptionString()));
                if (searchModel.Lĩnh_vực != null)
                    result = result.Where(x => x.Lĩnh_vực.Contains(searchModel.Lĩnh_vực.ToDescriptionString()));

                if (result.Any())
                {
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    this.searchViewModel.thongBaoMoiThauModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize); ;
                }
                return View("SearchResult", this.searchViewModel);
            }

            if (searchModel.Kiểu_thông_tin == Kiểu_thông_tin.Kết_quả_lựa_chọn_nhà_thầu)
            {
                var result = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.AsEnumerable().Where(x => x.AccountID == (int)Session["ID"]);

                if (searchModel.Số_TBMT_Tên_gói_thầu != null)
                    result = result.Where(x => x.Số_TBMT.Contains(searchModel.Số_TBMT_Tên_gói_thầu) || x.Tên_gói_thầu.Contains(searchModel.Số_TBMT_Tên_gói_thầu));
                if (searchModel.Bên_mời_thầu != null)
                    result = result.Where(x => x.Bên_mời_thầu.Contains(searchModel.Bên_mời_thầu));
                /*if (searchModel.Phạm_vi != null)*/

                var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
                var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

                switch (searchModel.Loại_ngày)
                {
                    case Loại_ngày.Ngày_đăng_tải:
                        result = result.Where(x => x.Ngày_đăng_tải >= Từ_ngày);
                        if (searchModel.Đến_ngày != null)
                        {
                            result = result.Where(x => x.Ngày_đăng_tải <= Đến_ngày);
                        }
                        break;
                    case Loại_ngày.Ngày_đóng_thầu:
                        result = result.Where(x => x.Ngày_phê_duyệt >= Từ_ngày);
                        if (searchModel.Đến_ngày != null)
                        {
                            result = result.Where(x => x.Ngày_phê_duyệt <= Đến_ngày);
                        }
                        break;
                    case Loại_ngày.Ngày_phát_hành_HSMT:
                        break;
                    default:
                        break;
                }
                if (result.Any())
                {
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    this.searchViewModel.ketQuaLuaChonNhaThauModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize); ;
                }
                return View("SearchResult", this.searchViewModel);
            }
            /*}*/
            return View("SearchResult", this.searchViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ChangeTimeRange()
        {
            return View(searchViewModel);
        }
    }
}