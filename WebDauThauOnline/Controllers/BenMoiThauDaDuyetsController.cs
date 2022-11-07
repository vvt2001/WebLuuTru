using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDauThauOnline.Models;

namespace WebDauThauOnline.Controllers
{
    public class BenMoiThauDaDuyetsController : Controller
    {
        private BenMoiThauDaDuyetEntities db = new BenMoiThauDaDuyetEntities();

        BenMoiThauSearchViewModel BenMoiThauSearchViewModel = new BenMoiThauSearchViewModel();

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BenMoiThauDaDuyet BenMoiThauDaDuyet = db.BenMoiThauDaDuyets.Find(id);

            if (BenMoiThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            return View(BenMoiThauDaDuyet);
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

            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };


            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            BenMoiThauSearchViewModel.BenMoiThauDaDuyetModel = db.BenMoiThauDaDuyets.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

            return View(BenMoiThauSearchViewModel);
        }

        [HttpPost]
        public ActionResult GetList(BenMoiThauSearchViewModel BenMoiThauSearchViewModel, int? page)
        {

            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };
            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };


            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            this.BenMoiThauSearchViewModel = BenMoiThauSearchViewModel;
            var searchModel = BenMoiThauSearchViewModel.BenMoiThauSearchModel;
            var result = db.BenMoiThauDaDuyets.AsEnumerable();

            if (searchModel.Bộ_ban_ngành != null)
                result = result.Where(x => x.Tập_đoàn_TCT_Bộ_ban_ngành.Contains(searchModel.Bộ_ban_ngành.ToDescriptionString()));
            if (searchModel.Tập_đoàn_TCT != null)
                result = result.Where(x => x.Tập_đoàn_TCT_Bộ_ban_ngành.Contains(searchModel.Tập_đoàn_TCT.ToDescriptionString()));
            if (searchModel.Tỉnh_Thành_phố != null)
                result = result.Where(x => x.Tỉnh_Thành_Phố.Contains(searchModel.Tỉnh_Thành_phố.ToDescriptionString()));

            var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
            var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

            result = result.Where(x => x.Ngày_phê_duyệt >= Từ_ngày);
            if (searchModel.Đến_ngày != null)
                result = result.Where(x => x.Ngày_phê_duyệt <= Đến_ngày);

            if (searchModel.Mã_cơ_quan != null)
                result = result.Where(x => x.Mã_cơ_quan.Contains(searchModel.Mã_cơ_quan));
            if (searchModel.Tên_bên_mời_thầu != null)
                result = result.Where(x => x.Bên_mời_thầu.Contains(searchModel.Tên_bên_mời_thầu));

            if (result.Any())
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                this.BenMoiThauSearchViewModel.BenMoiThauDaDuyetModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
            }

            return View("GetList", this.BenMoiThauSearchViewModel);
        }

        public ActionResult Create()
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phân_loại_trực_thuộc_EnumData = from Phân_loại_trực_thuộc e in Enum.GetValues(typeof(Phân_loại_trực_thuộc))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                                  select new
                                                  {
                                                      ID = (int)e,
                                                      Name = e.ToDescriptionString()
                                                  };

            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Phân_loại_trực_thuộc_EnumList = new SelectList(Phân_loại_trực_thuộc_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(BenMoiThauDaDuyet benMoiThauDaDuyet)
        {

            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phân_loại_trực_thuộc_EnumData = from Phân_loại_trực_thuộc e in Enum.GetValues(typeof(Phân_loại_trực_thuộc))
                                                select new
                                                {
                                                    ID = (int)e,
                                                    Name = e.ToDescriptionString()
                                                };
            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Phân_loại_trực_thuộc_EnumList = new SelectList(Phân_loại_trực_thuộc_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            benMoiThauDaDuyet.Phân_loại_trực_thuộc = benMoiThauDaDuyet.Phân_loại_trực_thuộc_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                benMoiThauDaDuyet.Tỉnh_Thành_Phố = benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();
            if(benMoiThauDaDuyet.Phân_loại_trực_thuộc == "Bộ ban ngành" && benMoiThauDaDuyet.Bộ_ban_ngành_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Bộ_ban_ngành_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Phân_loại_trực_thuộc == "Tập đoàn/ TCT" && benMoiThauDaDuyet.Tập_đoàn_TCT_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Tập_đoàn_TCT_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Phân_loại_trực_thuộc == "UBND Tỉnh/ Thành phố" && benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();

            benMoiThauDaDuyet.AccountID = (int)Session["ID"];

            db.BenMoiThauDaDuyets.Add(benMoiThauDaDuyet);
            db.SaveChanges();

            return RedirectToAction("GetList");
        }

        /*       public ActionResult ApproveRequest(int? page)
               {

                   var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };
                   var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };
                   var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };


                   ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
                   ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
                   ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

                   int pageSize = 10;
                   int pageNumber = (page ?? 1);
                   BenMoiThauSearchViewModel.BenMoiThauDaDuyetModel = db.BenMoiThauDaDuyets.Where(x => x.Trạng_thái == "Chưa được phê duyệt").OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

                   return View(BenMoiThauSearchViewModel);
               }

               [HttpPost]
               public ActionResult ApproveRequest(BenMoiThauSearchViewModel BenMoiThauSearchViewModel, int? page)
               {

                   var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };
                   var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                               select new
                                               {
                                                   ID = (int)e,
                                                   Name = e.ToDescriptionString()
                                               };
                   var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };


                   ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
                   ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
                   ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

                   this.BenMoiThauSearchViewModel = BenMoiThauSearchViewModel;
                   var searchModel = BenMoiThauSearchViewModel.BenMoiThauSearchModel;
                   var result = db.BenMoiThauDaDuyets.Where(x => x.Trạng_thái == "Chưa được phê duyệt").AsEnumerable();

                   if (searchModel.Bộ_ban_ngành != null)
                       result = result.Where(x => x.Tập_đoàn_TCT_Bộ_ban_ngành.Contains(searchModel.Bộ_ban_ngành.ToDescriptionString()));
                   if (searchModel.Tập_đoàn_TCT != null)
                       result = result.Where(x => x.Tập_đoàn_TCT_Bộ_ban_ngành.Contains(searchModel.Tập_đoàn_TCT.ToDescriptionString()));
                   if (searchModel.Tỉnh_Thành_phố != null)
                       result = result.Where(x => x.Tỉnh_Thành_Phố.Contains(searchModel.Tỉnh_Thành_phố.ToDescriptionString()));

                   var Từ_ngày = Convert.ToDateTime(searchModel.Từ_ngày);
                   var Đến_ngày = Convert.ToDateTime(searchModel.Đến_ngày);

                   result = result.Where(x => x.Ngày_phê_duyệt >= Từ_ngày);
                   if (searchModel.Đến_ngày != null)
                       result = result.Where(x => x.Ngày_phê_duyệt <= Đến_ngày);

                   if (searchModel.Mã_cơ_quan != null)
                       result = result.Where(x => x.Mã_cơ_quan.Contains(searchModel.Mã_cơ_quan));
                   if (searchModel.Tên_bên_mời_thầu != null)
                       result = result.Where(x => x.Bên_mời_thầu.Contains(searchModel.Tên_bên_mời_thầu));

                   if (result.Any())
                   {
                       int pageSize = 10;
                       int pageNumber = (page ?? 1);
                       this.BenMoiThauSearchViewModel.BenMoiThauDaDuyetModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);
                   }

                   return View("ApproveRequest", this.BenMoiThauSearchViewModel);
               }

               public ActionResult ApproveAll()
               {

                   foreach (BenMoiThauDaDuyet item in db.BenMoiThauDaDuyets.Where(x => x.Trạng_thái == "Chưa được phê duyệt").ToArray())
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

                   BenMoiThauDaDuyet BenMoiThauDaDuyet = db.BenMoiThauDaDuyets.Find(id);

                   if (BenMoiThauDaDuyet == null)
                   {
                       return HttpNotFound();
                   }
                   return View(BenMoiThauDaDuyet);
               }
               [HttpPost]
               public ActionResult ConfirmApprove(int id)
               {
                   var approved = db.BenMoiThauDaDuyets.Find(id);
                   approved.Trạng_thái = "Đã được phê duyệt";
                   approved.Ngày_phê_duyệt = DateTime.Today;
                   db.Entry(approved).State = EntityState.Modified;
                   db.SaveChanges();
                   return RedirectToAction("ApproveRequest");
               }
       */
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenMoiThauDaDuyet BenMoiThauDaDuyet = db.BenMoiThauDaDuyets.Find(id);
            if (BenMoiThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phân_loại_trực_thuộc_EnumData = from Phân_loại_trực_thuộc e in Enum.GetValues(typeof(Phân_loại_trực_thuộc))
                                                select new
                                                {
                                                    ID = (int)e,
                                                    Name = e.ToDescriptionString()
                                                };
            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Phân_loại_trực_thuộc_EnumList = new SelectList(Phân_loại_trực_thuộc_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            return View(BenMoiThauDaDuyet);
        }
        [HttpPost]
        public ActionResult Edit(BenMoiThauDaDuyet benMoiThauDaDuyet)
        {
            var Tỉnh_Thành_phố_EnumData = from Tỉnh_Thành_phố e in Enum.GetValues(typeof(Tỉnh_Thành_phố))
                                          select new
                                          {
                                              ID = (int)e,
                                              Name = e.ToDescriptionString()
                                          };
            var Phân_loại_trực_thuộc_EnumData = from Phân_loại_trực_thuộc e in Enum.GetValues(typeof(Phân_loại_trực_thuộc))
                                                select new
                                                {
                                                    ID = (int)e,
                                                    Name = e.ToDescriptionString()
                                                };
            var Bộ_ban_ngành_EnumData = from Bộ_ban_ngành e in Enum.GetValues(typeof(Bộ_ban_ngành))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            var Tập_đoàn_TCT_EnumData = from Tập_đoàn_TCT e in Enum.GetValues(typeof(Tập_đoàn_TCT))
                                        select new
                                        {
                                            ID = (int)e,
                                            Name = e.ToDescriptionString()
                                        };

            ViewBag.Tỉnh_Thành_phố_EnumList = new SelectList(Tỉnh_Thành_phố_EnumData, "ID", "Name");
            ViewBag.Phân_loại_trực_thuộc_EnumList = new SelectList(Phân_loại_trực_thuộc_EnumData, "ID", "Name");
            ViewBag.Bộ_ban_ngành_EnumList = new SelectList(Bộ_ban_ngành_EnumData, "ID", "Name");
            ViewBag.Tập_đoàn_TCT_EnumList = new SelectList(Tập_đoàn_TCT_EnumData, "ID", "Name");

            benMoiThauDaDuyet.Phân_loại_trực_thuộc = benMoiThauDaDuyet.Phân_loại_trực_thuộc_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                benMoiThauDaDuyet.Tỉnh_Thành_Phố = benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Phân_loại_trực_thuộc == "Bộ ban ngành" && benMoiThauDaDuyet.Bộ_ban_ngành_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Bộ_ban_ngành_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Phân_loại_trực_thuộc == "Tập đoàn/ TCT" && benMoiThauDaDuyet.Tập_đoàn_TCT_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Tập_đoàn_TCT_EnumValue.ToDescriptionString();
            if (benMoiThauDaDuyet.Phân_loại_trực_thuộc == "UBND Tỉnh/ Thành phố" && benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue != null)
                benMoiThauDaDuyet.Tập_đoàn_TCT_Bộ_ban_ngành = benMoiThauDaDuyet.Tỉnh_Thành_phố_EnumValue.ToDescriptionString();


            db.Entry(benMoiThauDaDuyet).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("GetList");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BenMoiThauDaDuyet BenMoiThauDaDuyet = db.BenMoiThauDaDuyets.Find(id);

            if (BenMoiThauDaDuyet == null)
            {
                return HttpNotFound();
            }
            return View(BenMoiThauDaDuyet);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BenMoiThauDaDuyet BenMoiThauDaDuyet = db.BenMoiThauDaDuyets.Find(id);
            db.BenMoiThauDaDuyets.Remove(BenMoiThauDaDuyet);
            db.SaveChanges();
            return RedirectToAction("GetList");
        }
    }
}
