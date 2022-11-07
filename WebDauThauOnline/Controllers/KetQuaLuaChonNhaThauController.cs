using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebDauThauOnline.Models;

namespace WebDauThauOnline.Controllers
{
    public class KetQuaLuaChonNhaThauController : Controller
    {
        private ThongBaoMoiThauEntities db1 = new ThongBaoMoiThauEntities();
        private KetQuaLuaChonNhaThauEntities db2 = new KetQuaLuaChonNhaThauEntities();
        private FileEntities db3 = new FileEntities();

        // GET: KetQuaLuaChonNhaThau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaLuaChonNhaThau_ThongTinChiTiet ketQuaLuaChonNhaThau_ThongTinChiTiet = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db3.Files.ToList().Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThau_ThongTinChiTiet.ID);
            KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel = new KetQuaLuaChonNhaThauViewModel
            {
                KetQuaLuaChonNhaThau_ThongTinChiTiet = ketQuaLuaChonNhaThau_ThongTinChiTiet,
                Files = files
            }; 
            if (ketQuaLuaChonNhaThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }
            return View(ketQuaLuaChonNhaThauViewModel);
        }

        // GET: KetQuaLuaChonNhaThau/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongBaoMoiThau_ThongTinChiTiet thongBaoMoiThau_ThongTinChiTiet = db1.ThongBaoMoiThau_ThongTinChiTiet.Find(id);
            KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel = new KetQuaLuaChonNhaThauViewModel
            {
                ThongBaoMoiThau_ThongTinChiTiet = thongBaoMoiThau_ThongTinChiTiet
            };

            if (ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }

            ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Lĩnh_vực_EnumValue = EnumExtension.GetValueFromDescription<Lĩnh_vực>(ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Lĩnh_vực);


            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            return View(ketQuaLuaChonNhaThauViewModel);
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
 
        // POST: KetQuaLuaChonNhaThau/Create
        [HttpPost]
        
        public ActionResult Create(KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel)
        {

            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Tên_gói_thầu = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Tên_gói_thầu;
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Tên_dự_án_Dự_toán_mua_sắm = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Tên_dự_toán_mua_sắm;
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Bên_mời_thầu = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Bên_mời_thầu;
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT_EnumValue.ToDescriptionString();
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Giá_dự_toán = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Dự_toán_gói_thầu;
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng_EnumValue.ToDescriptionString();
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Thời_gian_thực_hiện_HĐ = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Thời_gian_thực_hiện_hợp_đồng;
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực = ketQuaLuaChonNhaThauViewModel.ThongBaoMoiThau_ThongTinChiTiet.Lĩnh_vực_EnumValue.ToDescriptionString();
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.AccountID = (int)Session["ID"];

            db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Add(ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet);
            db2.SaveChanges();

            if (ketQuaLuaChonNhaThauViewModel.EmpFileModel.fileUpload != null)
            {
                foreach (HttpPostedFileBase fileUpload in ketQuaLuaChonNhaThauViewModel.EmpFileModel.fileUpload)
                {
                    if (fileUpload == null)
                    {
                        break;
                    }
                    else if (fileUpload.ContentLength > 2000000)
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

                        file.Kết_quả_ID = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID;

                        var fileList = db3.Files.Where(x => x.Kết_quả_ID == file.Kết_quả_ID).ToList();
                        if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                        {
                            fileUpload.SaveAs(path);
                            db3.Files.Add(file);
                            db3.SaveChanges();
                            ViewBag.EditStatus = "Chỉnh sửa thành công";
                        }
                        else
                        {
                            ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                        }
                    }
                }
                db3.SaveChanges();
            }

            ViewBag.FileStatus = "Đăng tải thành công";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateNew()
        {
            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");


            return View();
        }

        [HttpPost]
        public ActionResult CreateNew(KetQuaLuaChonNhaThauViewModel KetQuaLuaChonNhaThauViewModel)
        {
            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT = KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT_EnumValue.ToDescriptionString();
            KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng = KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng_EnumValue.ToDescriptionString();
            KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực = KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực_EnumValue.ToDescriptionString();
            KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.AccountID = (int)Session["ID"];

            db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Add(KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet);
            db2.SaveChanges();

            if (KetQuaLuaChonNhaThauViewModel.EmpFileModel.fileUpload != null)
            {
                foreach (HttpPostedFileBase fileUpload in KetQuaLuaChonNhaThauViewModel.EmpFileModel.fileUpload)
                {
                    if (fileUpload == null)
                    {
                        break;
                    }
                    else if (fileUpload.ContentLength > 2000000)
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

                        file.Kết_quả_ID = KetQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID;

                        var fileList = db3.Files.Where(x => x.Kết_quả_ID == file.Kết_quả_ID).ToList();
                        if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                        {
                            fileUpload.SaveAs(path);
                            db3.Files.Add(file);
                            db3.SaveChanges();
                            ViewBag.EditStatus = "Chỉnh sửa thành công";
                        }
                        else
                        {
                            ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                        }
                    }
                }
                db3.SaveChanges();
            }

            ViewBag.FileStatus = "Đăng tải thành công";

            return RedirectToAction("Index", "Home");
        }



        // GET: KetQuaLuaChonNhaThau/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaLuaChonNhaThau_ThongTinChiTiet ketQuaLuaChonNhaThau_ThongTinChiTiet = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db3.Files.ToList().Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThau_ThongTinChiTiet.ID);
            KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel = new KetQuaLuaChonNhaThauViewModel
            {
                KetQuaLuaChonNhaThau_ThongTinChiTiet = ketQuaLuaChonNhaThau_ThongTinChiTiet,
                Files = files
            }; 
            if (ketQuaLuaChonNhaThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }

            ketQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực_EnumValue = EnumExtension.GetValueFromDescription<Lĩnh_vực>(ketQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực);
            ketQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_lựa_chọn_NT>(ketQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT);
            ketQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_hợp_đồng>(ketQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng);

            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            ViewBag.GoiThauID = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID;
            int count = 0;
            List<int> FilesID = new List<int>();
            foreach (var item in ketQuaLuaChonNhaThauViewModel.Files)
            {
                FilesID.Add(item.ID);
                count++;
            }
            ViewBag.FilesID = FilesID;
            ViewBag.IDCount = count;

            return View(ketQuaLuaChonNhaThauViewModel);
        }

        // POST: KetQuaLuaChonNhaThau/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel)
        {
            ViewBag.FileStatus = "";
            ViewBag.EditStatus = "";

            var Hình_thức_lựa_chọn_NT_EnumData = from Hình_thức_lựa_chọn_NT e in Enum.GetValues(typeof(Hình_thức_lựa_chọn_NT))
                                                 select new
                                                 {
                                                     ID = (int)e,
                                                     Name = e.ToDescriptionString()
                                                 };
            var Hình_thức_hợp_đồng_EnumData = from Hình_thức_hợp_đồng e in Enum.GetValues(typeof(Hình_thức_hợp_đồng))
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
            ViewBag.Hình_thức_lựa_chọn_NT_EnumList = new SelectList(Hình_thức_lựa_chọn_NT_EnumData, "ID", "Name");
            ViewBag.Hình_thức_hợp_đồng_EnumList = new SelectList(Hình_thức_hợp_đồng_EnumData, "ID", "Name");
            ViewBag.Lĩnh_vực_EnumList = new SelectList(Lĩnh_vực_EnumData, "ID", "Name");

            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT_EnumValue.ToDescriptionString();
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng_EnumValue.ToDescriptionString();

            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Ngày_đăng_tải = DateTime.Now;

            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực_EnumValue = EnumExtension.GetValueFromDescription<Lĩnh_vực>(ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Lĩnh_vực);
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_lựa_chọn_NT>(ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_lựa_chọn_NT);
            ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng_EnumValue = EnumExtension.GetValueFromDescription<Hình_thức_hợp_đồng>(ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.Hình_thức_hợp_đồng);

            foreach (HttpPostedFileBase fileUpload in ketQuaLuaChonNhaThauViewModel.EmpFileModel.fileUpload)
            {
                if (fileUpload == null)
                {
                    break;
                }
                else if (fileUpload.ContentLength > 2000000)
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

                    file.Kết_quả_ID = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID;

                    var fileList = db3.Files.Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID).ToList();
                    if (fileList.FindIndex(x => x.Name == file.Name) < 0) {
                        fileUpload.SaveAs(path);
                        db3.Files.Add(file);
                        db3.SaveChanges();
                    }
                    else {
                        ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                    }
                }
            }
            
            ketQuaLuaChonNhaThauViewModel.Files = db3.Files.ToList().Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID);

            db2.Entry(ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet).State = EntityState.Modified;
            db2.SaveChanges();

            ViewBag.GoiThauID = ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet.ID;
            int count = 0;
            List<int> FilesID = new List<int>();
            foreach (var item in ketQuaLuaChonNhaThauViewModel.Files)
            {
                FilesID.Add(item.ID);
                count++;
            }
            ViewBag.FilesID = FilesID;
            ViewBag.IDCount = count;

            ViewBag.EditStatus = "Chỉnh sửa thành công";

            return View("Details", ketQuaLuaChonNhaThauViewModel);
        }

        [HttpPost]
        public ActionResult UploadFile(int GoiThauID)
        {

            HttpPostedFileBase[] fileUpload = new HttpPostedFileBase[HttpContext.Request.Files.Count];
            List<int> FilesID = new List<int>();

            for(int i=0; i < HttpContext.Request.Files.Count; ++i)
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

                    file.Kết_quả_ID = GoiThauID;

                    var fileList = db3.Files.Where(x => x.Kết_quả_ID == GoiThauID).ToList();
                    if (fileList.FindIndex(x => x.Name == file.Name) < 0)
                    {
                        uploadItem.SaveAs(path);
                        db3.Files.Add(file);
                        db3.SaveChanges();
                        FilesID.Add(file.ID);
                    }
                    else
                    {
                        ViewBag.FileStatus = "Không được đăng tải file bị trùng.";
                    }
                }
            }

            return Json(FilesID, JsonRequestBehavior.AllowGet);

/*            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });*/
        }

        // GET: KetQuaLuaChonNhaThau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaLuaChonNhaThau_ThongTinChiTiet ketQuaLuaChonNhaThau_ThongTinChiTiet = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db3.Files.ToList().Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThau_ThongTinChiTiet.ID);
            KetQuaLuaChonNhaThauViewModel ketQuaLuaChonNhaThauViewModel = new KetQuaLuaChonNhaThauViewModel
            {
                KetQuaLuaChonNhaThau_ThongTinChiTiet = ketQuaLuaChonNhaThau_ThongTinChiTiet,
                Files = files
            };
            if (ketQuaLuaChonNhaThauViewModel.KetQuaLuaChonNhaThau_ThongTinChiTiet == null)
            {
                return HttpNotFound();
            }
            return View(ketQuaLuaChonNhaThauViewModel);
        }

        // POST: KetQuaLuaChonNhaThau/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            KetQuaLuaChonNhaThau_ThongTinChiTiet ketQuaLuaChonNhaThau_ThongTinChiTiet = db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Find(id);
            IEnumerable<Models.File> files = db3.Files.ToList().Where(x => x.Kết_quả_ID == ketQuaLuaChonNhaThau_ThongTinChiTiet.ID);
            foreach (var item in files)
            {
                System.IO.File.Delete(item.Path);
                db3.Files.Remove(item);
            }
            db3.SaveChanges();
            db2.KetQuaLuaChonNhaThau_ThongTinChiTiet.Remove(ketQuaLuaChonNhaThau_ThongTinChiTiet);
            db2.SaveChanges();


            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db1.Dispose(); 
                db2.Dispose();
                db3.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult DeleteFileAjax(int FileID)
        {
            Models.File file = db3.Files.Find(FileID);
            if (System.IO.File.Exists(file.Path))
            {
                System.IO.File.Delete(file.Path);
            }
            db3.Files.Remove(file);
            db3.SaveChanges();

            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            List<Models.File> ObjFiles = db3.Files.ToList();

            var FileById = (from FC in ObjFiles
                            where FC.ID.Equals(id)
                            select new { FC.Name, FC.Content }).ToList().FirstOrDefault();

            return File(FileById.Content, "application/pdf", FileById.Name);
        }
    }
}
