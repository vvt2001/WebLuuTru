using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using WebDauThauOnline.Models;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using PagedList;

namespace WebDauThauOnline.Controllers
{
    public class ThongBaoMoiThauController : Controller
    {
        private ThongBaoMoiThauEntities db = new ThongBaoMoiThauEntities();
        private FileEntities db2 = new FileEntities();
        SearchViewModel searchViewModel = new SearchViewModel();

        // GET: ThongBaoMoiThau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db.ThongBaoMoiThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db2.Files.ToList().Where(x => x.Thông_báo_ID == thongBaoMoiThau_ThongTinChiTiet.ID);
            ThongBaoMoiThauViewModel thongBaoMoiThauViewModel = new ThongBaoMoiThauViewModel
            {
                ThongBaoMoiThauModel = thongBaoMoiThau_ThongTinChiTiet,
                Files = files
            };

            if (thongBaoMoiThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }
            return View(thongBaoMoiThauViewModel);
        }

        // GET: ThongBaoMoiThau/Create
        public ActionResult Create()
        {
            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Phân_loại_EnumData = from Phân_loại e in Enum.GetValues(typeof(Phân_loại))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Loại_hợp_đồng_EnumData = from Loại_hợp_đồng e in Enum.GetValues(typeof(Loại_hợp_đồng))
                                         select new
                                         {
                                             ID = (int)e,
                                             Name = e.ToDescriptionString()
                                         };
            var Hình_thức_lựa_chọn_nhà_thầu_EnumData = from Hình_thức_lựa_chọn_nhà_thầu e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_nhà_thầu))
                                                       select new
                                                       {
                                                           ID = (int)e,
                                                           Name = e.ToDescriptionString()
                                                       };
            var Phương_thức_LCNT_EnumData = from Phương_thức_LCNT e in Enum.GetValues(typeof(Phương_thức_LCNT))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_dự_thầu_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                             select new
                                             {
                                                 ID = (int)e,
                                                 Name = e.ToDescriptionString()
                                             };

            var Hình_thức_bảo_đảm_dự_thầu_EnumData = from Hình_thức_bảo_đảm_dự_thầu e in Enum.GetValues(typeof(Hình_thức_bảo_đảm_dự_thầu))
                                                     select new
                                                     {
                                                         ID = (int)e,
                                                         Name = e.ToDescriptionString()
                                                     };
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            ViewBag.Phân_loại_EnumList = new SelectList(Phân_loại_EnumData, "ID", "Name");
            ViewBag.Loại_hợp_đồng_EnumList = new SelectList(Loại_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Hình_thức_lựa_chọn_nhà_thầu_EnumList = new SelectList(Hình_thức_lựa_chọn_nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Phương_thức_LCNT_EnumList = new SelectList(Phương_thức_LCNT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_dự_thầu_EnumList = new SelectList(Hình_thức_dự_thầu_EnumData, "ID", "Name");
            ViewBag.Hình_thức_bảo_đảm_dự_thầu_EnumList = new SelectList(Hình_thức_bảo_đảm_dự_thầu_EnumData, "ID", "Name");
            return View();
        }


        private static string GetPath(string path, int count)
        {
            if (System.IO.File.Exists(path))
            {
                count += 1;
                if (count != 2)
                {
                    int duplicateIndicatorCharCount = 3 + count.ToString().Length;
                    path = path.Remove(path.LastIndexOf('.') - duplicateIndicatorCharCount, duplicateIndicatorCharCount);
                }

                string duplicateIndicator = " (" + count + ")";
                path = path.Insert(path.LastIndexOf('.'), duplicateIndicator);
                return GetPath(path, count);
            }
            else
            {
                return path;
            }

        }
        private static byte[] MD5Hashing(byte[] byteArr)
        {
            byte[] hashBytes;
            // Creates an instance of the default implementation of the MD5 hash algorithm.
            using (var md5Hash = MD5.Create())
            {
                // Generate hash value(byte Array) for input data
                hashBytes = md5Hash.ComputeHash(byteArr);
            }
            return hashBytes;
        }
        private static byte[] ConnectByte(byte[] byteArr1, byte[] byteArr2)
        {
            byte[] result = new byte[byteArr1.Length + byteArr2.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = i < byteArr1.Length ? byteArr1[i] : byteArr2[i - byteArr1.Length];
            }
            return result;
        }

/*        private static string CreateID(int ID)
        {
            long IDCount = (long)DateTime.Now.Year * 10000000 + ID;
            string resultID = (IDCount).ToString();
            return resultID;
        }*/


        // POST: ThongBaoMoiThau/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ThongBaoMoiThauViewModel ThongBaoMoiThauViewModel)
        {
            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Phân_loại_EnumData = from Phân_loại e in Enum.GetValues(typeof(Phân_loại))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Loại_hợp_đồng_EnumData = from Loại_hợp_đồng e in Enum.GetValues(typeof(Loại_hợp_đồng))
                                         select new
                                         {
                                             ID = (int)e,
                                             Name = e.ToDescriptionString()
                                         };
            var Hình_thức_lựa_chọn_nhà_thầu_EnumData = from Hình_thức_lựa_chọn_nhà_thầu e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_nhà_thầu))
                                                       select new
                                                       {
                                                           ID = (int)e,
                                                           Name = e.ToDescriptionString()
                                                       };
            var Phương_thức_LCNT_EnumData = from Phương_thức_LCNT e in Enum.GetValues(typeof(Phương_thức_LCNT))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_dự_thầu_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                             select new
                                             {
                                                 ID = (int)e,
                                                 Name = e.ToDescriptionString()
                                             };
            var Hình_thức_bảo_đảm_dự_thầu_EnumData = from Hình_thức_bảo_đảm_dự_thầu e in Enum.GetValues(typeof(Hình_thức_bảo_đảm_dự_thầu))
                                                     select new
                                                     {
                                                         ID = (int)e,
                                                         Name = e.ToDescriptionString()
                                                     };
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            ViewBag.Phân_loại_EnumList = new SelectList(Phân_loại_EnumData, "ID", "Name");
            ViewBag.Loại_hợp_đồng_EnumList = new SelectList(Loại_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Hình_thức_lựa_chọn_nhà_thầu_EnumList = new SelectList(Hình_thức_lựa_chọn_nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Phương_thức_LCNT_EnumList = new SelectList(Phương_thức_LCNT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_dự_thầu_EnumList = new SelectList(Hình_thức_dự_thầu_EnumData, "ID", "Name");
            ViewBag.Hình_thức_bảo_đảm_dự_thầu_EnumList = new SelectList(Hình_thức_bảo_đảm_dự_thầu_EnumData, "ID", "Name");

            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lĩnh_vực = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lĩnh_vực_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phân_loại = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phân_loại_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Loại_hợp_đồng = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Loại_hợp_đồng_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_lựa_chọn_nhà_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_lựa_chọn_nhà_thầu_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phương_thức_LCNT = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phương_thức_LCNT_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_dự_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_dự_thầu_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Chi_phí_E_HSMT = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Chi_phí_E_HSMT;
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_bảo_đảm_dự_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_bảo_đảm_dự_thầu_EnumValue.ToDescriptionString();
/*            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Thời_điểm_đăng_tải = DateTime.Now;
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_TBMT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID);
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_hiệu_KHLCNT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID);
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lần_chỉnh_sửa = 0;*/
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.AccountID = (int) Session["ID"];
            try
            {
                db.ThongBaoMoiThau_ThongTinChiTiet.Add(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
/*            db.ThongBaoMoiThau_ThongTinChiTiet.Add(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel);
            db.SaveChanges();*/

/*            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_TBMT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID) + " - 00";
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_hiệu_KHLCNT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID);
*/
            db.Entry(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel).State = EntityState.Modified;
            db.SaveChanges();

            if (ThongBaoMoiThauViewModel.EmpFileModel.fileUpload != null)
            {
                foreach (HttpPostedFileBase fileUpload in ThongBaoMoiThauViewModel.EmpFileModel.fileUpload)
                {
                    if (fileUpload == null)
                    {
                        break;
                    }
                    else if(fileUpload.ContentLength > 20000000)
                    {
                        continue;
                    }
                    else
                    {
                        //File upload
                        Models.File file = new Models.File();

                        Stream fileStream = fileUpload.InputStream;
                        BinaryReader binaryReader = new BinaryReader(fileStream);
                        byte[] FileDetail = binaryReader.ReadBytes((Int32)fileStream.Length);


                        file.Name = fileUpload.FileName;
                        file.Content = FileDetail;

                        //Slice file detail into 3 parts
                        int sliceSize = 50;
                        byte[] slicedDetail1 = FileDetail.Take(sliceSize).ToArray();
                        byte[] slicedDetail2 = FileDetail.Skip(sliceSize).Take(sliceSize).ToArray();
                        byte[] slicedDetail3 = FileDetail.Skip(sliceSize * 2).Take(FileDetail.Length - sliceSize * 2).ToArray();

                        //Hash those parts
                        var hashSlice1 = MD5Hashing(slicedDetail1);
                        var hashSlice2 = MD5Hashing(slicedDetail2);
                        var hashSlice3 = MD5Hashing(slicedDetail3);

                        //Reconnect the parts
                        var connectedBytes = ConnectByte(ConnectByte(hashSlice1, hashSlice2), hashSlice3);

                        //Finally hash the connected hashed slices
                        file.Hashed_Content = MD5Hashing(connectedBytes);

                        //file.HashedContent = MD5Hashing(FileDetail);

                        file.FileSize = fileUpload.ContentLength / 1024; //store size as kb

                        //Save file to designated dir
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploaded Files/"));
                        string path = Server.MapPath("~/Uploaded Files/") + fileUpload.FileName;

                        //Make sure duplicated files is different when stored in server folder
                        path = GetPath(path, 1);
                        
                        file.Path = Path.GetFullPath(path);

                        file.Thông_báo_ID = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID;

                        var fileList = db2.Files.Where(x => x.Thông_báo_ID == file.Thông_báo_ID).ToList();
                        if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                        {
                            fileUpload.SaveAs(path);
                            db2.Files.Add(file);
                            db2.SaveChanges();
                        }
                        else
                        {
                            ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                        }
                    }
                }
                db2.SaveChanges();
            }

            ViewBag.UploadStatus = "Đăng tải thành công.";
            return RedirectToAction("Index", "Home");

        }

        // GET: ThongBaoMoiThau/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db.ThongBaoMoiThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db2.Files.ToList().Where(x => x.Thông_báo_ID == thongBaoMoiThau_ThongTinChiTiet.ID);
            ThongBaoMoiThauViewModel thongBaoMoiThauViewModel = new ThongBaoMoiThauViewModel
            {
                ThongBaoMoiThauModel = thongBaoMoiThau_ThongTinChiTiet,
                Files = files
            };
            if (thongBaoMoiThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }

            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lĩnh_vực_EnumValue = EnumExtension.GetValueFromDescription<Lĩnh_vực>(thongBaoMoiThau_ThongTinChiTiet.Lĩnh_vực);
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phân_loại_EnumValue = EnumExtension.GetValueFromDescription<Phân_loại>(thongBaoMoiThau_ThongTinChiTiet.Phân_loại);
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Loại_hợp_đồng_EnumValue = EnumExtension.GetValueFromDescription<Loại_hợp_đồng>(thongBaoMoiThau_ThongTinChiTiet.Loại_hợp_đồng); 
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_lựa_chọn_nhà_thầu_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_lựa_chọn_nhà_thầu>(thongBaoMoiThau_ThongTinChiTiet.Hình_thức_lựa_chọn_nhà_thầu);
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phương_thức_LCNT_EnumValue = EnumExtension.GetValueFromDescription<Phương_thức_LCNT>(thongBaoMoiThau_ThongTinChiTiet.Phương_thức_LCNT); 
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_dự_thầu_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_dự_thầu>(thongBaoMoiThau_ThongTinChiTiet.Hình_thức_dự_thầu);
            thongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_bảo_đảm_dự_thầu_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_bảo_đảm_dự_thầu>(thongBaoMoiThau_ThongTinChiTiet.Hình_thức_bảo_đảm_dự_thầu);

            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Phân_loại_EnumData = from Phân_loại e in Enum.GetValues(typeof(Phân_loại))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Loại_hợp_đồng_EnumData = from Loại_hợp_đồng e in Enum.GetValues(typeof(Loại_hợp_đồng))
                                         select new
                                         {
                                             ID = (int)e,
                                             Name = e.ToDescriptionString()
                                         };
            var Hình_thức_lựa_chọn_nhà_thầu_EnumData = from Hình_thức_lựa_chọn_nhà_thầu e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_nhà_thầu))
                                                       select new
                                                       {
                                                           ID = (int)e,
                                                           Name = e.ToDescriptionString()
                                                       };
            var Phương_thức_LCNT_EnumData = from Phương_thức_LCNT e in Enum.GetValues(typeof(Phương_thức_LCNT))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_dự_thầu_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                             select new
                                             {
                                                 ID = (int)e,
                                                 Name = e.ToDescriptionString()
                                             };

            var Hình_thức_bảo_đảm_dự_thầu_EnumData = from Hình_thức_bảo_đảm_dự_thầu e in Enum.GetValues(typeof(Hình_thức_bảo_đảm_dự_thầu))
                                                     select new
                                                     {
                                                         ID = (int)e,
                                                         Name = e.ToDescriptionString()
                                                     };
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            ViewBag.Phân_loại_EnumList = new SelectList(Phân_loại_EnumData, "ID", "Name");
            ViewBag.Loại_hợp_đồng_EnumList = new SelectList(Loại_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Hình_thức_lựa_chọn_nhà_thầu_EnumList = new SelectList(Hình_thức_lựa_chọn_nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Phương_thức_LCNT_EnumList = new SelectList(Phương_thức_LCNT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_dự_thầu_EnumList = new SelectList(Hình_thức_dự_thầu_EnumData, "ID", "Name");
            ViewBag.Hình_thức_bảo_đảm_dự_thầu_EnumList = new SelectList(Hình_thức_bảo_đảm_dự_thầu_EnumData, "ID", "Name");

            ViewBag.GoiThauID = thongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID;
            int count = 0;
            List<int> FilesID = new List<int>();
            foreach (var item in thongBaoMoiThauViewModel.Files)
            {
                FilesID.Add(item.ID);
                count++;
            }
            ViewBag.FilesID = FilesID;
            ViewBag.IDCount = count;

            return View(thongBaoMoiThauViewModel);
        }

        // POST: ThongBaoMoiThau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(ThongBaoMoiThauViewModel ThongBaoMoiThauViewModel)
        {

            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Phân_loại_EnumData = from Phân_loại e in Enum.GetValues(typeof(Phân_loại))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Loại_hợp_đồng_EnumData = from Loại_hợp_đồng e in Enum.GetValues(typeof(Loại_hợp_đồng))
                                         select new
                                         {
                                             ID = (int)e,
                                             Name = e.ToDescriptionString()
                                         };
            var Hình_thức_lựa_chọn_nhà_thầu_EnumData = from Hình_thức_lựa_chọn_nhà_thầu e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_nhà_thầu))
                                                       select new
                                                       {
                                                           ID = (int)e,
                                                           Name = e.ToDescriptionString()
                                                       };
            var Phương_thức_LCNT_EnumData = from Phương_thức_LCNT e in Enum.GetValues(typeof(Phương_thức_LCNT))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_dự_thầu_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                             select new
                                             {
                                                 ID = (int)e,
                                                 Name = e.ToDescriptionString()
                                             };

            var Hình_thức_bảo_đảm_dự_thầu_EnumData = from Hình_thức_bảo_đảm_dự_thầu e in Enum.GetValues(typeof(Hình_thức_bảo_đảm_dự_thầu))
                                                     select new
                                                     {
                                                         ID = (int)e,
                                                         Name = e.ToDescriptionString()
                                                     };
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            ViewBag.Phân_loại_EnumList = new SelectList(Phân_loại_EnumData, "ID", "Name");
            ViewBag.Loại_hợp_đồng_EnumList = new SelectList(Loại_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Hình_thức_lựa_chọn_nhà_thầu_EnumList = new SelectList(Hình_thức_lựa_chọn_nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Phương_thức_LCNT_EnumList = new SelectList(Phương_thức_LCNT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_dự_thầu_EnumList = new SelectList(Hình_thức_dự_thầu_EnumData, "ID", "Name");
            ViewBag.Hình_thức_bảo_đảm_dự_thầu_EnumList = new SelectList(Hình_thức_bảo_đảm_dự_thầu_EnumData, "ID", "Name");

            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lĩnh_vực = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lĩnh_vực_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phân_loại = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phân_loại_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Loại_hợp_đồng = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Loại_hợp_đồng_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_lựa_chọn_nhà_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_lựa_chọn_nhà_thầu_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phương_thức_LCNT = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Phương_thức_LCNT_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_dự_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_dự_thầu_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Chi_phí_E_HSMT = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Chi_phí_E_HSMT;
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_bảo_đảm_dự_thầu = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Hình_thức_bảo_đảm_dự_thầu_EnumValue.ToDescriptionString();
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Thời_điểm_đăng_tải = DateTime.Now;
            ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lần_chỉnh_sửa += 1;

/*            if(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lần_chỉnh_sửa < 10)
            {
                ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_TBMT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID) + " - 0" + ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lần_chỉnh_sửa.ToString();
            }
            else
            {
                ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Số_TBMT = CreateID(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID) + " - " + ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.Lần_chỉnh_sửa.ToString();
            }*/
            db.Entry(ThongBaoMoiThauViewModel.ThongBaoMoiThauModel).State = EntityState.Modified;
            db.SaveChanges();

            if (ThongBaoMoiThauViewModel.EmpFileModel.fileUpload != null)
            {
                foreach (HttpPostedFileBase fileUpload in ThongBaoMoiThauViewModel.EmpFileModel.fileUpload)
                {
                    if (fileUpload == null)
                    {
                        break;
                    }
                    else if (fileUpload.ContentLength > 20000000)
                    {
                        continue;
                    }
                    else
                    {
                        //File upload
                        Models.File file = new Models.File();

                        Stream fileStream = fileUpload.InputStream;
                        BinaryReader binaryReader = new BinaryReader(fileStream);
                        byte[] FileDetail = binaryReader.ReadBytes((Int32)fileStream.Length);


                        file.Name = fileUpload.FileName;
                        file.Content = FileDetail;

                        //Slice file detail into 3 parts
                        int sliceSize = 50;
                        byte[] slicedDetail1 = FileDetail.Take(sliceSize).ToArray();
                        byte[] slicedDetail2 = FileDetail.Skip(sliceSize).Take(sliceSize).ToArray();
                        byte[] slicedDetail3 = FileDetail.Skip(sliceSize * 2).Take(FileDetail.Length - sliceSize * 2).ToArray();

                        //Hash those parts
                        var hashSlice1 = MD5Hashing(slicedDetail1);
                        var hashSlice2 = MD5Hashing(slicedDetail2);
                        var hashSlice3 = MD5Hashing(slicedDetail3);

                        //Reconnect the parts
                        var connectedBytes = ConnectByte(ConnectByte(hashSlice1, hashSlice2), hashSlice3);

                        //Finally hash the connected hashed slices
                        file.Hashed_Content = MD5Hashing(connectedBytes);

                        //file.HashedContent = MD5Hashing(FileDetail);

                        file.FileSize = fileUpload.ContentLength / 1024; //store size as kb

                        //Save file to designated dir
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploaded Files/"));
                        string path = Server.MapPath("~/Uploaded Files/") + fileUpload.FileName;

                        //Make sure duplicated files is different when stored in server folder
                        path = GetPath(path, 1);
                        file.Path = Path.GetFullPath(path);

                        file.Thông_báo_ID = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID;
                        var fileList = db2.Files.Where(x => x.Thông_báo_ID == ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID).ToList();
                        if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                        {
                            fileUpload.SaveAs(path);
                            db2.Files.Add(file);
                            db2.SaveChanges();
                            ViewBag.EditStatus = "Chỉnh sửa thành công";
                        }
                        else
                        {
                            ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                        }
                    }
                }
            }

            ThongBaoMoiThauViewModel.Files = db2.Files.ToList().Where(x => x.Thông_báo_ID == ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID);
            ViewBag.GoiThauID = ThongBaoMoiThauViewModel.ThongBaoMoiThauModel.ID;
            int count = 0;
            List<int> FilesID = new List<int>();
            foreach (var item in ThongBaoMoiThauViewModel.Files)
            {
                FilesID.Add(item.ID);
                count++;
            }
            ViewBag.FilesID = FilesID;
            ViewBag.IDCount = count;

            ViewBag.EditStatus = "Chỉnh sửa thành công";
            return View("Details",ThongBaoMoiThauViewModel);
        }

        [HttpPost]
        public ActionResult UploadFile(int GoiThauID)
        {

            HttpPostedFileBase[] fileUpload = new HttpPostedFileBase[HttpContext.Request.Files.Count];
            List<int> FilesID = new List<int>();

            for (int i = 0; i < HttpContext.Request.Files.Count; ++i)
            {
                fileUpload[i] = HttpContext.Request.Files[i];
            }
            if (fileUpload != null)
            {
                foreach (HttpPostedFileBase uploadItem in fileUpload)
                {
                    if (uploadItem.ContentLength > 2000000)
                    {
                        continue;
                    }
                    //File upload
                    Models.File file = new Models.File();

                    Stream fileStream = uploadItem.InputStream;
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] FileDetail = binaryReader.ReadBytes((Int32)fileStream.Length);

                    file.Name = uploadItem.FileName;
                    file.Content = FileDetail;

                    //Slice file detail into 3 parts
                    int sliceSize = 50;
                    byte[] slicedDetail1 = FileDetail.Take(sliceSize).ToArray();
                    byte[] slicedDetail2 = FileDetail.Skip(sliceSize).Take(sliceSize).ToArray();
                    byte[] slicedDetail3 = FileDetail.Skip(sliceSize * 2).Take(FileDetail.Length - sliceSize * 2).ToArray();

                    //Hash those parts
                    var hashSlice1 = MD5Hashing(slicedDetail1);
                    var hashSlice2 = MD5Hashing(slicedDetail2);
                    var hashSlice3 = MD5Hashing(slicedDetail3);

                    //Reconnect the parts
                    var connectedBytes = ConnectByte(ConnectByte(hashSlice1, hashSlice2), hashSlice3);

                    //Finally hash the connected hashed slices
                    file.Hashed_Content = MD5Hashing(connectedBytes);

                    //file.HashedContent = MD5Hashing(FileDetail);

                    file.FileSize = uploadItem.ContentLength / 1024; //store size as kb

                    //Save file to designated dir
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploaded Files/"));
                    string path = Server.MapPath("~/Uploaded Files/") + uploadItem.FileName;

                    //Make sure duplicated files is different when stored in server folder
                    path = GetPath(path, 1);
                    file.Path = Path.GetFullPath(path);

                    file.Thông_báo_ID = GoiThauID;

                    var fileList = db2.Files.Where(x => x.Thông_báo_ID == GoiThauID).ToList();
                    if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                    {
                        uploadItem.SaveAs(path);
                        db2.Files.Add(file);
                        db2.SaveChanges();
                        ViewBag.EditStatus = "Chỉnh sửa thành công";
                    }
                    else
                    {
                        ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                    }

                    FilesID.Add(file.ID);
                }
            }

            return Json(FilesID, JsonRequestBehavior.AllowGet);

            /*            string message = "SUCCESS";
                        return Json(new { Message = message, JsonRequestBehavior.AllowGet });*/
        }

        [HttpPost]
        public ActionResult DeleteFileAjax(int FileID)
        {
            Models.File file = db2.Files.Find(FileID);
            if (System.IO.File.Exists(file.Path))
            {
                System.IO.File.Delete(file.Path);
            }
            db2.Files.Remove(file);
            db2.SaveChanges();

            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        // GET: ThongBaoMoiThau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db.ThongBaoMoiThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db2.Files.ToList().Where(x => x.Thông_báo_ID == thongBaoMoiThau_ThongTinChiTiet.ID);
            ThongBaoMoiThauViewModel thongBaoMoiThauViewModel = new ThongBaoMoiThauViewModel
            {
                ThongBaoMoiThauModel = thongBaoMoiThau_ThongTinChiTiet,
                Files = files
            };
            if (thongBaoMoiThau_ThongTinChiTiet == null)

            {
                return HttpNotFound();
            }
            return View(thongBaoMoiThauViewModel);
        }

        // POST: ThongBaoMoiThau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db.ThongBaoMoiThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db2.Files.ToList().Where(x => x.Thông_báo_ID == thongBaoMoiThau_ThongTinChiTiet.ID);
            foreach (var item in files){
                System.IO.File.Delete(item.Path);
                db2.Files.Remove(item);
            }
            db2.SaveChanges();
            db.ThongBaoMoiThau_ThongTinChiTiet.Remove(thongBaoMoiThau_ThongTinChiTiet);
            db.SaveChanges();
            return RedirectToAction("Index", "Home" );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            List<Models.File> ObjFiles = db2.Files.ToList();

            var FileById = (from FC in ObjFiles
                            where FC.ID.Equals(id)
                            select new { FC.Name, FC.Content }).ToList().FirstOrDefault();

            return File(FileById.Content, "application/pdf", FileById.Name);
        }

        [HttpGet]
        public ActionResult DeleteFile(int id, int Thông_báo_ID)
        {

            var Lĩnh_vực_EnumData = from Lĩnh_vực e in Enum.GetValues(typeof(Lĩnh_vực))
                                    select new
                                    {
                                        ID = (int)e,
                                        Name = e.ToDescriptionString()
                                    };
            var Phân_loại_EnumData = from Phân_loại e in Enum.GetValues(typeof(Phân_loại))
                                     select new
                                     {
                                         ID = (int)e,
                                         Name = e.ToDescriptionString()
                                     };
            var Loại_hợp_đồng_EnumData = from Loại_hợp_đồng e in Enum.GetValues(typeof(Loại_hợp_đồng))
                                         select new
                                         {
                                             ID = (int)e,
                                             Name = e.ToDescriptionString()
                                         };
            var Hình_thức_lựa_chọn_nhà_thầu_EnumData = from Hình_thức_lựa_chọn_nhà_thầu e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_nhà_thầu))
                                                       select new
                                                       {
                                                           ID = (int)e,
                                                           Name = e.ToDescriptionString()
                                                       };
            var Phương_thức_LCNT_EnumData = from Phương_thức_LCNT e in Enum.GetValues(typeof(Phương_thức_LCNT))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToDescriptionString()
                                            };
            var Hình_thức_dự_thầu_EnumData = from Hình_thức_dự_thầu e in Enum.GetValues(typeof(Hình_thức_dự_thầu))
                                             select new
                                             {
                                                 ID = (int)e,
                                                 Name = e.ToDescriptionString()
                                             };

            var Hình_thức_bảo_đảm_dự_thầu_EnumData = from Hình_thức_bảo_đảm_dự_thầu e in Enum.GetValues(typeof(Hình_thức_bảo_đảm_dự_thầu))
                                                     select new
                                                     {
                                                         ID = (int)e,
                                                         Name = e.ToDescriptionString()
                                                     };
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            ViewBag.Phân_loại_EnumList = new SelectList(Phân_loại_EnumData, "ID", "Name");
            ViewBag.Loại_hợp_đồng_EnumList = new SelectList(Loại_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Hình_thức_lựa_chọn_nhà_thầu_EnumList = new SelectList(Hình_thức_lựa_chọn_nhà_thầu_EnumData, "ID", "Name");
            ViewBag.Phương_thức_LCNT_EnumList = new SelectList(Phương_thức_LCNT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_dự_thầu_EnumList = new SelectList(Hình_thức_dự_thầu_EnumData, "ID", "Name");
            ViewBag.Hình_thức_bảo_đảm_dự_thầu_EnumList = new SelectList(Hình_thức_bảo_đảm_dự_thầu_EnumData, "ID", "Name");

            Models.File file = db2.Files.Find(id);
            System.IO.File.Delete(file.Path);

            db2.Files.Remove(file);
            db2.SaveChanges();

            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db.ThongBaoMoiThau_ThongTinChiTiet.Find(Thông_báo_ID);
            IEnumerable<Models.File> files = db2.Files.ToList().Where(x => x.Thông_báo_ID == thongBaoMoiThau_ThongTinChiTiet.ID);
            ThongBaoMoiThauViewModel thongBaoMoiThauViewModel = new ThongBaoMoiThauViewModel
            {
                ThongBaoMoiThauModel = thongBaoMoiThau_ThongTinChiTiet,
                Files = files
            };

            return View("Edit", thongBaoMoiThauViewModel);
        }

/*        public ActionResult GetList()
        {
            return View(db.ThongBaoMoiThau_ThongTinChiTiet.ToList());
        }*/

        public ActionResult GetList(int? page)
        {
            var Kiểu_thông_báo_EnumData = from Kiểu_thông_báo e in Enum.GetValues(typeof(Kiểu_thông_báo))
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
            ViewBag.Kiểu_thông_báo_EnumList = new SelectList(Kiểu_thông_báo_EnumData, "ID", "Name");
            ViewBag.Phạm_vi_EnumList = new SelectList(Phạm_vi_EnumData, "ID", "Name");
            ViewBag.Loại_ngày_EnumList = new SelectList(Loại_ngày_EnumData, "ID", "Name");
            ViewBag.Khoảng_thời_gian_EnumList = new SelectList(Khoảng_thời_gian_EnumData, "ID", "Name");
            ViewBag.Hình_thức_EnumList = new SelectList(Hình_thức_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            searchViewModel.thongBaoMoiThauModel = db.ThongBaoMoiThau_ThongTinChiTiet.AsEnumerable().OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize);

            return View(searchViewModel);
        }

        [HttpPost]
        public ActionResult GetList(SearchViewModel searchViewModel, int? page)
        {
            var Kiểu_thông_báo_EnumData = from Kiểu_thông_báo e in Enum.GetValues(typeof(Kiểu_thông_báo))
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
            ViewBag.Kiểu_thông_báo_EnumList = new SelectList(Kiểu_thông_báo_EnumData, "ID", "Name");
            ViewBag.Phạm_vi_EnumList = new SelectList(Phạm_vi_EnumData, "ID", "Name");
            ViewBag.Loại_ngày_EnumList = new SelectList(Loại_ngày_EnumData, "ID", "Name");
            ViewBag.Khoảng_thời_gian_EnumList = new SelectList(Khoảng_thời_gian_EnumData, "ID", "Name");
            ViewBag.Hình_thức_EnumList = new SelectList(Hình_thức_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");
            /*            if (ModelState.IsValid)
                        {*/
            this.searchViewModel = searchViewModel;
            var searchModel = searchViewModel.searchModel;
/*            if (searchModel.Kiểu_thông_tin == Kiểu_thông_tin.Thông_báo_mời_thầu)
            {*/
                var result = db.ThongBaoMoiThau_ThongTinChiTiet.AsEnumerable();

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
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    this.searchViewModel.thongBaoMoiThauModel = result.OrderBy(x => x.ID).ToPagedList(pageNumber, pageSize); ;
                }
                return View("GetList", this.searchViewModel);
        }
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

        public PartialViewResult Notifications()
        {
            var currentDate = DateTime.Now;
            var DayCount = 7;
            var ThongBaoSapDong = db.ThongBaoMoiThau_ThongTinChiTiet.Where(x => DbFunctions.DiffDays(currentDate, x.Thời_điểm_đóng_mở_thầu) <= DayCount && DbFunctions.DiffDays(currentDate, x.Thời_điểm_đóng_mở_thầu) >= 0).AsEnumerable();
            return PartialView(ThongBaoSapDong);
        }
    }
}
