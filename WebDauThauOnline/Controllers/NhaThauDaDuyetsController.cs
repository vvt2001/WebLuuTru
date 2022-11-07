using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDauThauOnline.Models;
using PagedList;

namespace WebDauThauOnline.Controllers
{
    public class NhaThauDaDuyetsController : Controller
    {
        private NhaThauDaDuyetEntities db = new NhaThauDaDuyetEntities();
        NhaThauSearchViewModel NhaThauSearchViewModel = new NhaThauSearchViewModel();

        public ActionResult Create()
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Loại_hình_doanh_nghiệp_EnumData = from Loại_hình_doanh_nghiệp e in Enum.GetValues(typeof(Loại_hình_doanh_nghiệp))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            var Quốc_gia_EnumData = from Quốc_gia e in Enum.GetValues(typeof(Quốc_gia))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Trạng_thái_đóng_phí_EnumData = from Trạng_thái_đóng_phí e in Enum.GetValues(typeof(Trạng_thái_đóng_phí))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Loại_hình_doanh_nghiệp_EnumList = new SelectList(Loại_hình_doanh_nghiệp_EnumData, "ID", "Name");
            ViewBag.Quốc_gia_EnumList = new SelectList(Quốc_gia_EnumData, "ID", "Name");
            ViewBag.Trạng_thái_đóng_phí_EnumList = new SelectList(Trạng_thái_đóng_phí_EnumData, "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(NhaThauDaDuyet nhaThauDaDuyet)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Loại_hình_doanh_nghiệp_EnumData = from Loại_hình_doanh_nghiệp e in Enum.GetValues(typeof(Loại_hình_doanh_nghiệp))
                                                  select new
                                                  {
                                                      ID = (int)e,
                                                      Name = e.ToDescriptionString()
                                                  };

            var Quốc_gia_EnumData = from Quốc_gia e in Enum.GetValues(typeof(Quốc_gia))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Trạng_thái_đóng_phí_EnumData = from Trạng_thái_đóng_phí e in Enum.GetValues(typeof(Trạng_thái_đóng_phí))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Loại_hình_doanh_nghiệp_EnumList = new SelectList(Loại_hình_doanh_nghiệp_EnumData, "ID", "Name");
            ViewBag.Quốc_gia_EnumList = new SelectList(Quốc_gia_EnumData, "ID", "Name");
            ViewBag.Trạng_thái_đóng_phí_EnumList = new SelectList(Trạng_thái_đóng_phí_EnumData, "ID", "Name");

            nhaThauDaDuyet.Loại_hình_doanh_nghiệp = nhaThauDaDuyet.Loại_hình_doanh_nghiệp_EnumValue.ToDescriptionString();
            if (nhaThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                nhaThauDaDuyet.Tỉnh_Thành_phố = nhaThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();
            nhaThauDaDuyet.Quốc_gia = nhaThauDaDuyet.Quốc_gia_EnumValue.ToDescriptionString();
            nhaThauDaDuyet.Trạng_thái_đóng_phí = nhaThauDaDuyet.Trạng_thái_đóng_phí_EnumValue.ToDescriptionString();

            nhaThauDaDuyet.AccountID = (int)Session["ID"];
            if (nhaThauDaDuyet.Quốc_gia == "Việt Nam")
                nhaThauDaDuyet.Vị_trí_nhà_thầu = "Trong nước";
            else
            {
                nhaThauDaDuyet.Vị_trí_nhà_thầu = "Nước ngoài";
            }
            db.NhaThauDaDuyets.Add(nhaThauDaDuyet);
            db.SaveChanges();

            return RedirectToAction("GetList");
        }

 /*       public ActionResult ApproveRequest(int? page)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            NhaThauSearchViewModel.NhaThauDaDuyetModel = db.NhaThauDaDuyets.AsEnumerable().Where(x=>x.Trạng_thái == "Chưa được phê duyệt").OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

            return View(NhaThauSearchViewModel);
        }

        [HttpPost]
        public ActionResult ApproveRequest(NhaThauSearchViewModel NhaThauSearchViewModel, int? page)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");

            this.NhaThauSearchViewModel = NhaThauSearchViewModel;
            var searchModel = NhaThauSearchViewModel.NhaThauSearchModel;
            var result = db.NhaThauDaDuyets.Where(x => x.Trạng_thái == "Chưa được phê duyệt").AsEnumerable();


            if (searchModel.Nhà_Thầu != null)
                result = result.Where(x => x.Vị_trí_nhà_thầu.Contains(searchModel.Nhà_Thầu.ToDescriptionString()));

            var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
            var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

            result = result.Where(x => x.Ngày_phê_duyệt >= Từ_ngày);
            if (searchModel.Đến_ngày != null)
                result = result.Where(x => x.Ngày_phê_duyệt <= Đến_ngày);

            if (searchModel.Tỉnh_Thành_Phố != null)
                result = result.Where(x => x.Tỉnh_Thành_phố.Contains(searchModel.Tỉnh_Thành_Phố.ToDescriptionString()));
            if (searchModel.Tên_nhà_thầu != null)
                result = result.Where(x => x.Tên_nhà_thầu.Contains(searchModel.Tên_nhà_thầu));

            if (result.Any())
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                this.NhaThauSearchViewModel.NhaThauDaDuyetModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
            }

            return View("ApproveRequest", this.NhaThauSearchViewModel);
        }
        public ActionResult ApproveAll()
        {
            foreach (NhaThauDaDuyet item in db.NhaThauDaDuyets.Where(x=>x.Trạng_thái == "Chưa được phê duyệt").ToArray())
            {
                item.Trạng_thái = "Đã được phê duyệt";
                item.Ngày_phê_duyệt = DateTime.Today;
                db.Entry(item).State = EntityState.Modified;
            }

            db.SaveChanges();
            return RedirectToAction("ApproveRequest");
        }

        public ActionResult ConfirmApprove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NhaThauDaDuyet nhaThauDaDuyet = db.NhaThauDaDuyets.Find(id);

            if (nhaThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            return View(nhaThauDaDuyet);
        }
        [HttpPost]
        public ActionResult ConfirmApprove(int id)
        {
            var approved = db.NhaThauDaDuyets.Find(id);
            approved.Trạng_thái = "Đã được phê duyệt";
            approved.Ngày_phê_duyệt = DateTime.Today;
            db.Entry(approved).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("GetList");
        }*/

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NhaThauDaDuyet nhaThauDaDuyet = db.NhaThauDaDuyets.Find(id);

            if (nhaThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            return View(nhaThauDaDuyet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetList(int? page)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                   select new
                                   {
                                       ID = (int)e,
                                       Name = e.ToDescriptionString()
                                   };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            NhaThauSearchViewModel.NhaThauDaDuyetModel = db.NhaThauDaDuyets.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

            return View(NhaThauSearchViewModel);
        }

        [HttpPost]
        public ActionResult GetList(NhaThauSearchViewModel NhaThauSearchViewModel, int? page)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");

            this.NhaThauSearchViewModel = NhaThauSearchViewModel;
            var searchModel = NhaThauSearchViewModel.NhaThauSearchModel;
            var result = db.NhaThauDaDuyets.AsEnumerable();


            if (searchModel.Nhà_Thầu != null)
                result = result.Where(x => x.Vị_trí_nhà_thầu.Contains(searchModel.Nhà_Thầu.ToDescriptionString()));

            var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
            var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

            result = result.Where(x => x.Ngày_phê_duyệt >= Từ_ngày);
            if (searchModel.Đến_ngày != null)
                result = result.Where(x => x.Ngày_phê_duyệt <= Đến_ngày);

            if (searchModel.Tỉnh_Thành_Phố != null)
                result = result.Where(x => x.Tỉnh_Thành_phố.Contains(searchModel.Tỉnh_Thành_Phố.ToDescriptionString()));
            if (searchModel.Tên_nhà_thầu != null)
                result = result.Where(x => x.Tên_nhà_thầu.Contains(searchModel.Tên_nhà_thầu));

            if (result.Any())
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                this.NhaThauSearchViewModel.NhaThauDaDuyetModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
            }

            return View("GetList", this.NhaThauSearchViewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaThauDaDuyet NhaThauDaDuyet = db.NhaThauDaDuyets.Find(id);
            if (NhaThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Loại_hình_doanh_nghiệp_EnumData = from Loại_hình_doanh_nghiệp e in Enum.GetValues(typeof(Loại_hình_doanh_nghiệp))
                                                  select new
                                                  {
                                                      ID = (int)e,
                                                      Name = e.ToDescriptionString()
                                                  };

            var Quốc_gia_EnumData = from Quốc_gia e in Enum.GetValues(typeof(Quốc_gia))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Trạng_thái_đóng_phí_EnumData = from Trạng_thái_đóng_phí e in Enum.GetValues(typeof(Trạng_thái_đóng_phí))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Loại_hình_doanh_nghiệp_EnumList = new SelectList(Loại_hình_doanh_nghiệp_EnumData, "ID", "Name");
            ViewBag.Quốc_gia_EnumList = new SelectList(Quốc_gia_EnumData, "ID", "Name");
            ViewBag.Trạng_thái_đóng_phí_EnumList = new SelectList(Trạng_thái_đóng_phí_EnumData, "ID", "Name");

            return View(NhaThauDaDuyet);
        }
        [HttpPost]
        public ActionResult Edit(NhaThauDaDuyet nhaThauDaDuyet)
        {
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Nhà_thầu_EnumData = from Nhà_thầu e in Enum.GetValues(typeof(Nhà_thầu))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Loại_hình_doanh_nghiệp_EnumData = from Loại_hình_doanh_nghiệp e in Enum.GetValues(typeof(Loại_hình_doanh_nghiệp))
                                                  select new
                                                  {
                                                      ID = (int)e,
                                                      Name = e.ToDescriptionString()
                                                  };

            var Quốc_gia_EnumData = from Quốc_gia e in Enum.GetValues(typeof(Quốc_gia))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Trạng_thái_đóng_phí_EnumData = from Trạng_thái_đóng_phí e in Enum.GetValues(typeof(Trạng_thái_đóng_phí))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Nhà_thầu_EnumList = new SelectList(Nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Loại_hình_doanh_nghiệp_EnumList = new SelectList(Loại_hình_doanh_nghiệp_EnumData, "ID", "Name");
            ViewBag.Quốc_gia_EnumList = new SelectList(Quốc_gia_EnumData, "ID", "Name");
            ViewBag.Trạng_thái_đóng_phí_EnumList = new SelectList(Trạng_thái_đóng_phí_EnumData, "ID", "Name");

            nhaThauDaDuyet.Loại_hình_doanh_nghiệp = nhaThauDaDuyet.Loại_hình_doanh_nghiệp_EnumValue.ToDescriptionString();
            if (nhaThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                nhaThauDaDuyet.Tỉnh_Thành_phố = nhaThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();
            nhaThauDaDuyet.Quốc_gia = nhaThauDaDuyet.Quốc_gia_EnumValue.ToDescriptionString();
            nhaThauDaDuyet.Trạng_thái_đóng_phí = nhaThauDaDuyet.Trạng_thái_đóng_phí_EnumValue.ToDescriptionString();

            if (nhaThauDaDuyet.Quốc_gia == "Việt Nam")
                nhaThauDaDuyet.Vị_trí_nhà_thầu = "Trong nước";
            else
            {
                nhaThauDaDuyet.Vị_trí_nhà_thầu = "Nước ngoài";
            }
            db.Entry(nhaThauDaDuyet).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("GetList");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NhaThauDaDuyet NhaThauDaDuyet = db.NhaThauDaDuyets.Find(id);

            if (NhaThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            return View(NhaThauDaDuyet);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaThauDaDuyet NhaThauDaDuyet = db.NhaThauDaDuyets.Find(id);
            db.NhaThauDaDuyets.Remove(NhaThauDaDuyet);
            db.SaveChanges();
            return RedirectToAction("GetList");
        }
    }
}
