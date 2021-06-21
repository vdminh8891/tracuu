using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using TraCuuBMT.General;
using TraCuuBMT.Models;

namespace TraCuuBMT.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ListKQPTPL()
        {
            List<KetQuaPhanTichPhanLoai> listKQPTPL = new List<KetQuaPhanTichPhanLoai>();
            using (var db = new TraCuuBMTEntities())
            {
                listKQPTPL = db.KetQuaPhanTichPhanLoais.Where(w => w.status > 0).ToList();
            }
            ViewBag.ListKQPTPL = listKQPTPL;
            ViewBag.KQPTPLPath = Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["KQPTPLPath"].ToString();
            return View();
        }

        public ActionResult CreateKQPTPL()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateKQPTPL(FormCollection form, HttpPostedFileBase file_vn)
        {
            //validate
            string hsCode = form["hsCode"];
            string unit = form["unit"];
            string so_Vanban = form["so_Vanban"];
            string coQuan_PhatHanh_PTPL = form["coQuan_PhatHanh_PTPL"];
            string mota_Dnkhaibao = form["mota_Dnkhaibao"];
            string mota_KQPTPL = form["mota_KQPTPL"];
            string donVi_XNK = form["donVi_XNK"];
            string co_Quan_YC_PTPL = form["co_Quan_YC_PTPL"];
            string toKhai_HQ = form["toKhai_HQ"];
            string loai_Hinh = form["loai_Hinh"];
            string Ngay_VanbanString = form["Ngay_Vanban"];
            DateTime Ngay_Vanban = DateTime.ParseExact(Ngay_VanbanString, "MM/dd/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            string Ngay_Vanban_HHString = form["Ngay_Vanban_HH"];
            DateTime Ngay_Vanban_HH = DateTime.ParseExact(Ngay_Vanban_HHString, "MM/dd/yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
            string statusString = form["status"] ?? "";

            string FileName = Path.GetFileNameWithoutExtension(file_vn.FileName);

            //To Get File Extension  
            string FileExtension = Path.GetExtension(file_vn.FileName);

            //Add Current Date To Attached File Name  
            FileName = "KQPTPL_" + DateTimeOffset.Now.ToUnixTimeSeconds() + FileExtension;

            //Get Upload path from Web.Config file AppSettings.  
            string UploadPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["KQPTPLPath"].ToString());
            //string UploadPath = Server.MapPath("./") + ConfigurationManager.AppSettings["BieuThuePath"].ToString();
            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
            //Its Create complete path to store in server.  
            string fullpath = UploadPath + FileName;

            //To copy and save file into server.  
            file_vn.SaveAs(fullpath);


            using (var db = new TraCuuBMTEntities())
            {
                KetQuaPhanTichPhanLoai item = new KetQuaPhanTichPhanLoai();
                item.ID = Util.GenerateID("KQPTPL");
                int temp = Util.ParseStringToInt(statusString);
                if (temp > -99)
                {
                    item.status = temp;
                }
                else
                {
                    item.status = 1;
                }

                item.createDate = DateTime.Now;
                item.hsCode = hsCode;

                item.So_Vanban = so_Vanban;
                item.CoQuan_PhatHanh_PTPL = coQuan_PhatHanh_PTPL;
                item.Mota_Dnkhaibao = mota_Dnkhaibao;
                item.Mota_KQPTPL = mota_KQPTPL;
                item.DonVi_XNK = donVi_XNK;
                item.Co_Quan_YC_PTPL = co_Quan_YC_PTPL;
                item.ToKhai_HQ = toKhai_HQ;
                item.Loai_Hinh = loai_Hinh;
                item.link_file_vn = FileName;
                item.Ngay_Vanban = Ngay_Vanban;
                item.Ngay_Vanban_HH = Ngay_Vanban_HH;
                item.creator = "";
                item.unit = unit;
                db.KetQuaPhanTichPhanLoais.Add(item);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListKQPTPL", "Admin");
        }

        public ActionResult ListBieuThue()
        {
            List<BieuThue> listBieuThue = new List<BieuThue>();
            using (var db = new TraCuuBMTEntities())
            {
                listBieuThue = db.BieuThues.Where(w => w.status > 0).ToList();
            }
            ViewBag.ListBieuThue = listBieuThue;
            //ViewBag.BieuThuePath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BieuThuePath"].ToString());
            //ViewBag.BieuThuePath = Server.MapPath(ConfigurationManager.AppSettings["BieuThuePath"].ToString());
            ViewBag.BieuThuePath = Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["BieuThuePath"].ToString();
            return View();
        }

        public ActionResult EditBieuThue(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                BieuThue item = db.BieuThues.Where(w => w.status > 0 && w.ID == id).FirstOrDefault();
                if (item != null)
                {
                    ViewBag.BieuThue = item;
                    return View();
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy biểu thuế này";
                    return RedirectToAction("ListBieuThue", "Admin");
                }
            }
        }

        [HttpPost]
        public ActionResult EditBieuThue(FormCollection form, HttpPostedFileBase file_vn)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string bieuThueId = form["bieuThueId"] ?? "";
                string statusString = form["status"] ?? "";
                string tenBieuThue = form["tenBieuThue"];
                string description = form["description"];
                string Ten_Hanghoa_VN = form["Ten_Hanghoa_VN"];
                string Ten_Hanghoa_EN = form["Ten_Hanghoa_EN"];
                string HS_CODE = form["HS_CODE"];
                string note = form["note"];
                string DVT_SL2 = form["DVT_SL2"];
                double B01 = Util.ParseStringToFloat(form["B01"] ?? "");
                double B02 = Util.ParseStringToFloat(form["B02"] ?? "");
                double B03 = Util.ParseStringToFloat(form["B03"] ?? "");
                double B04 = Util.ParseStringToFloat(form["B04"] ?? "");
                double B05 = Util.ParseStringToFloat(form["B05"] ?? "");
                double B06 = Util.ParseStringToFloat(form["B06"] ?? "");
                double B07 = Util.ParseStringToFloat(form["B07"] ?? "");
                double B08 = Util.ParseStringToFloat(form["B08"] ?? "");
                double B09 = Util.ParseStringToFloat(form["B09"] ?? "");
                double B10 = Util.ParseStringToFloat(form["B10"] ?? "");
                double B11 = Util.ParseStringToFloat(form["B11"] ?? "");
                double B12 = Util.ParseStringToFloat(form["B12"] ?? "");
                double B13 = Util.ParseStringToFloat(form["B13"] ?? "");
                double B14 = Util.ParseStringToFloat(form["B14"] ?? "");
                double B15 = Util.ParseStringToFloat(form["B15"] ?? "");
                double B16 = Util.ParseStringToFloat(form["B16"] ?? "");
                double B17 = Util.ParseStringToFloat(form["B17"] ?? "");
                double B18 = Util.ParseStringToFloat(form["B18"] ?? "");
                double B19 = Util.ParseStringToFloat(form["B19"] ?? "");
                double B20 = Util.ParseStringToFloat(form["B20"] ?? "");
                double B21 = Util.ParseStringToFloat(form["B21"] ?? "");
                double B22 = Util.ParseStringToFloat(form["B22"] ?? "");
                double B23 = Util.ParseStringToFloat(form["B23"] ?? "");
                double B24 = Util.ParseStringToFloat(form["B24"] ?? "");
                double B25 = Util.ParseStringToFloat(form["B25"] ?? "");
                double B30 = Util.ParseStringToFloat(form["B30"] ?? "");
                double B61 = Util.ParseStringToFloat(form["B61"] ?? "");
                double B99 = Util.ParseStringToFloat(form["B99"] ?? "");
                string FileName = "";
                if (file_vn != null && file_vn?.ContentLength != 0)
                {
                    try
                    {
                        FileName = Path.GetFileNameWithoutExtension(file_vn.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(file_vn.FileName);

                        //Add Current Date To Attached File Name  
                        FileName = "BieuThue_" + DateTimeOffset.Now.ToUnixTimeSeconds() + FileExtension;

                        //Get Upload path from Web.Config file AppSettings.  
                        string UploadPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["BieuThuePath"].ToString());
                        //string UploadPath = Server.MapPath("./") + ConfigurationManager.AppSettings["BieuThuePath"].ToString();
                        if (!Directory.Exists(UploadPath))
                        {
                            Directory.CreateDirectory(UploadPath);
                        }
                        //Its Create complete path to store in server.  
                        string fullpath = UploadPath + FileName;

                        //To copy and save file into server.  
                        file_vn.SaveAs(fullpath);
                    }
                    catch (Exception ex)
                    {
                        FileName = "";
                        //ghi log
                    }
                }


                if (!string.IsNullOrEmpty(bieuThueId))
                {
                    BieuThue bieuThue = db.BieuThues.Where(w => w.status > 0 && w.ID == bieuThueId).FirstOrDefault();
                    if (bieuThue != null)
                    {
                        int temp = Util.ParseStringToInt(statusString);
                        if (temp > -99)
                        {
                            bieuThue.status = temp;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Lỗi dữ liệu!";
                            return RedirectToAction("ListBieuThue", "Admin");
                        }
                        bieuThue.tenBieuThue = tenBieuThue;
                        bieuThue.description = description;
                        bieuThue.HS_CODE = HS_CODE;
                        bieuThue.note = note;
                        bieuThue.DVT_SL2 = DVT_SL2;
                        bieuThue.Ten_Hanghoa_VN = Ten_Hanghoa_VN;
                        bieuThue.B01 = B01;
                        bieuThue.B02 = B02;
                        bieuThue.B03 = B03;
                        bieuThue.B04 = B04;
                        bieuThue.B05 = B05;
                        bieuThue.B06 = B06;
                        bieuThue.B07 = B07;
                        bieuThue.B08 = B08;
                        bieuThue.B09 = B09;
                        bieuThue.B10 = B10;
                        bieuThue.B11 = B11;
                        bieuThue.B12 = B12;
                        bieuThue.B13 = B13;
                        bieuThue.B14 = B14;
                        bieuThue.B15 = B15;
                        bieuThue.B16 = B16;
                        bieuThue.B17 = B17;
                        bieuThue.B18 = B18;
                        bieuThue.B19 = B19;
                        bieuThue.B20 = B20;
                        bieuThue.B21 = B21;
                        bieuThue.B22 = B22;
                        bieuThue.B23 = B23;
                        bieuThue.B24 = B24;
                        bieuThue.B25 = B25;
                        bieuThue.B30 = B30;
                        bieuThue.B61 = B61;
                        bieuThue.B99 = B99;

                        if(!string.IsNullOrEmpty(FileName)){
                            bieuThue.link_file_vn = FileName;
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                        return RedirectToAction("ListBieuThue", "Admin");
                    }

                }
            }
            TempData["WarningMessage"] = "Không tìm thấy user này";
            return RedirectToAction("ListNormalUser", "Admin");
        }

        public ActionResult CreateBieuThue()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBieuThue(FormCollection form, HttpPostedFileBase file_vn)
        {
            //validate
            string tenBieuThue = form["tenBieuThue"];
            string description = form["description"];
            string HS_CODE = form["HS_CODE"];
            string note = form["note"];
            string DVT_SL2 = form["DVT_SL2"];
            string Ten_Hanghoa_VN = form["Ten_Hanghoa_VN"];
            string Ten_Hanghoa_EN = form["Ten_Hanghoa_EN"];
            double B01 = Util.ParseStringToFloat(form["B01"] ?? "");
            double B02 = Util.ParseStringToFloat(form["B02"] ?? "");
            double B03 = Util.ParseStringToFloat(form["B03"] ?? "");
            double B04 = Util.ParseStringToFloat(form["B04"] ?? "");
            double B05 = Util.ParseStringToFloat(form["B05"] ?? "");
            double B06 = Util.ParseStringToFloat(form["B06"] ?? "");
            double B07 = Util.ParseStringToFloat(form["B07"] ?? "");
            double B08 = Util.ParseStringToFloat(form["B08"] ?? "");
            double B09 = Util.ParseStringToFloat(form["B09"] ?? "");
            double B10 = Util.ParseStringToFloat(form["B10"] ?? "");
            double B11 = Util.ParseStringToFloat(form["B11"] ?? "");
            double B12 = Util.ParseStringToFloat(form["B12"] ?? "");
            double B13 = Util.ParseStringToFloat(form["B13"] ?? "");
            double B14 = Util.ParseStringToFloat(form["B14"] ?? "");
            double B15 = Util.ParseStringToFloat(form["B15"] ?? "");
            double B16 = Util.ParseStringToFloat(form["B16"] ?? "");
            double B17 = Util.ParseStringToFloat(form["B17"] ?? "");
            double B18 = Util.ParseStringToFloat(form["B18"] ?? "");
            double B19 = Util.ParseStringToFloat(form["B19"] ?? "");
            double B20 = Util.ParseStringToFloat(form["B20"] ?? "");
            double B21 = Util.ParseStringToFloat(form["B21"] ?? "");
            double B22 = Util.ParseStringToFloat(form["B22"] ?? "");
            double B23 = Util.ParseStringToFloat(form["B23"] ?? "");
            double B24 = Util.ParseStringToFloat(form["B24"] ?? "");
            double B25 = Util.ParseStringToFloat(form["B25"] ?? "");
            double B30 = Util.ParseStringToFloat(form["B30"] ?? "");
            double B61 = Util.ParseStringToFloat(form["B61"] ?? "");
            double B99 = Util.ParseStringToFloat(form["B99"] ?? "");
            string statusString = form["status"] ?? "";

            string FileName = "";
            if(file_vn != null && !string.IsNullOrEmpty(file_vn.FileName))
            {
                try
                {
                    FileName = Path.GetFileNameWithoutExtension(file_vn.FileName);

                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(file_vn.FileName);

                    //Add Current Date To Attached File Name  
                    FileName = "BieuThue_" + DateTimeOffset.Now.ToUnixTimeSeconds() + FileExtension;

                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["BieuThuePath"].ToString());
                    //string UploadPath = Server.MapPath("./") + ConfigurationManager.AppSettings["BieuThuePath"].ToString();
                    if (!Directory.Exists(UploadPath))
                    {
                        Directory.CreateDirectory(UploadPath);
                    }
                    //Its Create complete path to store in server.  
                    string fullpath = UploadPath + FileName;

                    //To copy and save file into server.  
                    file_vn.SaveAs(fullpath);
                }
                catch(Exception ex)
                {
                    //ghi log
                }
            }
            


            using (var db = new TraCuuBMTEntities())
            {
                BieuThue item = new BieuThue();
                item.ID = Util.GenerateID("bieuThue");
                int temp = Util.ParseStringToInt(statusString);
                if (temp > -99)
                {
                    item.status = temp;
                }
                else
                {
                    item.status = 1;
                }

                item.createDate = DateTime.Now;
                item.description = description;
                item.tenBieuThue = tenBieuThue;
                item.HS_CODE = HS_CODE;
                item.note = note;
                item.link_file_vn = FileName;
                item.Ten_Hanghoa_EN = Ten_Hanghoa_EN;
                item.Ten_Hanghoa_VN = Ten_Hanghoa_VN;
                item.creator = "";
                item.B01 = B01;
                item.B02 = B02;
                item.B03 = B03;
                item.B04 = B04;
                item.B05 = B05;
                item.B06 = B06;
                item.B07 = B07;
                item.B08 = B08;
                item.B09 = B09;
                item.B10 = B10;
                item.B11 = B11;
                item.B12 = B12;
                item.B13 = B13;
                item.B14 = B14;
                item.B15 = B15;
                item.B16 = B16;
                item.B17 = B17;
                item.B18 = B18;
                item.B19 = B19;
                item.B20 = B20;
                item.B21 = B21;
                item.B22 = B22;
                item.B23 = B23;
                item.B24 = B24;
                item.B25 = B25;
                item.B30 = B30;
                item.B61 = B61;
                item.B99 = B99;
                item.DVT_SL2 = DVT_SL2;
                db.BieuThues.Add(item);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListBieuThue", "Admin");
        }

        public ActionResult TransactionUser()
        {
            using (var db = new TraCuuBMTEntities())
            {
                ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
                ViewBag.ListTransaction = db.Transactions.Where(w => w.status >= 0).OrderByDescending(w => w.createDate).ToList();

            }
            return View();
        }

        public ActionResult TransactionStatistic()
        {
            using (var db = new TraCuuBMTEntities())
            {
                ViewBag.ListTransaction = db.Transactions.Where(w => w.status >= 0).OrderByDescending(w => w.createDate).ToList();
                ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
            }
            return View();
        }

        public ActionResult CreateTransaction()
        {
            using (var db = new TraCuuBMTEntities())
            {
                ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
                ViewBag.ListPackage = db.Packages.Where(w => w.status >= 0).ToList();
            }
            return View();
        }

        //[HttpPost]
        //public ActionResult CreateTransaction(FormCollection form)
        //{
        //    //validate
        //    string email = form["email"];
        //    string phone = form["phone"];
        //    string statusString = form["status"] ?? "";
        //    string password = form["password"];

        //    using (var db = new TraCuuBMTEntities())
        //    {
        //        Transaction item = new Transaction();
        //        item.ID = Util.GenerateID("transaction");
        //        item.amount = amount;
        //        item.createDate = DateTime.Now;
        //        item.creator = ((User)Session["userInfo"]).ID;
        //        item.currentPrice = currentPrice;
        //        item.lastEditDate = DateTime.Now;
        //        item.lastEditor = ((User)Session["userInfo"]).ID;
        //        item.note = note;
        //        item.packageId = packageId;
        //        item.type
        //        int temp = Util.ParseStringToInt(statusString);
        //        if (temp > -99)
        //        {
        //            item.status = temp;
        //        }
        //        else
        //        {
        //            item.status = 1;
        //        }

        //        item.createDate = DateTime.Now;
        //        item.type = 1;
        //        db.Transactions.Add(item);
        //        db.SaveChanges();
        //    }
        //    TempData["SuccessMessage"] = "Tạo mới thành công!";
        //    return RedirectToAction("ListNormalUser", "Admin");
        //}

        public ActionResult EditTransaction(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                Transaction item = db.Transactions.Where(w => w.status >= 0 && w.ID == id).FirstOrDefault();
                if (item != null)
                {
                    ViewBag.Transaction = item;
                    ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
                    return View();
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy Transaction này";
                    return RedirectToAction("ListTransaction", "Admin");
                }
            }
        }

        [HttpPost]
        public ActionResult EditTransaction(FormCollection form)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string transactionId = form["transactionId"] ?? "";
                string note = form["note"];
                string statusString = form["status"] ?? "";
                if (!string.IsNullOrEmpty(transactionId))
                {
                    Transaction item = db.Transactions.Where(w => w.status > 0 && w.ID == transactionId).FirstOrDefault();
                    if (item != null)
                    {
                        int temp = Util.ParseStringToInt(statusString);
                        if (temp > -99)
                        {
                            item.status = temp;
                            item.note = note;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Lỗi dữ liệu!";
                            return RedirectToAction("ListNormalUser", "Admin");
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                        return RedirectToAction("ListTransaction", "Admin");
                    }

                }
            }
            TempData["WarningMessage"] = "Không tìm thấy user này";
            return RedirectToAction("ListTransaction", "Admin");
        }

        public ActionResult ListTransaction()
        {
            List<Transaction> listItem = new List<Transaction>();
            using (var db = new TraCuuBMTEntities())
            {
                listItem = db.Transactions.Where(w => w.status >= 0).OrderByDescending(w => w.createDate).ToList();
                ViewBag.ListTransaction = listItem;
                ViewBag.ListUser = db.Users.Where(w => w.status > 0).ToList();
            }

            return View();
        }

        public ActionResult CreatePackage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePackage(FormCollection form)
        {
            //validate
            string email = form["email"];
            string phone = form["phone"];
            string statusString = form["status"] ?? "";
            string password = form["password"];

            using (var db = new TraCuuBMTEntities())
            {
                User user = new User();
                user.ID = Util.GenerateID("user");
                int temp = Util.ParseStringToInt(statusString);
                if (temp > -99)
                {
                    user.status = temp;
                }
                else
                {
                    user.status = 1;
                }

                user.createDate = DateTime.Now;
                user.email = email;
                user.phone = phone;
                user.password = password;
                user.role = 1;
                user.type = 1;
                user.username = email;
                db.Users.Add(user);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListNormalUser", "Admin");
        }

        public ActionResult ListPackage()
        {
            List<Package> listPackage = new List<Package>();
            using (var db = new TraCuuBMTEntities())
            {
                listPackage = db.Packages.Where(w => w.status > 0).ToList();
            }
            ViewBag.ListPackage = listPackage;
            return View();
        }

        public ActionResult ListNormalUser()
        {
            List<User> listUser = new List<User>();
            using (var db = new TraCuuBMTEntities())
            {
                listUser = db.Users.Where(w => w.status > 0 && w.type == 1).ToList();
            }
            ViewBag.ListUser = listUser;
            return View();
        }

        public ActionResult EditUser(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                User user = db.Users.Where(w => w.status > 0 && w.ID == id).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.User = user;
                    return View();
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy user này";
                    return RedirectToAction("ListNormalUser", "Admin");
                }
            }
        }


        [HttpPost]
        public ActionResult EditUser(FormCollection form)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string userId = form["userId"] ?? "";
                string email = form["email"];
                string phone = form["phone"];
                string statusString = form["status"] ?? "";
                if (!string.IsNullOrEmpty(userId))
                {
                    User user = db.Users.Where(w => w.status > 0 && w.ID == userId).FirstOrDefault();
                    if (user != null)
                    {
                        int temp = Util.ParseStringToInt(statusString);
                        if (temp > -99)
                        {
                            user.status = temp;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Lỗi dữ liệu!";
                            return RedirectToAction("ListNormalUser", "Admin");
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                        return RedirectToAction("ListNormalUser", "Admin");
                    }

                }
            }
            TempData["WarningMessage"] = "Không tìm thấy user này";
            return RedirectToAction("ListNormalUser", "Admin");
        }


        public ActionResult CreateNormalUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNormalUser(FormCollection form)
        {
            //validate
            string email = form["email"];
            string phone = form["phone"];
            string statusString = form["status"] ?? "";
            string password = form["password"];

            using (var db = new TraCuuBMTEntities())
            {
                User user = new User();
                user.ID = Util.GenerateID("user");
                int temp = Util.ParseStringToInt(statusString);
                if (temp > -99)
                {
                    user.status = temp;
                }
                else
                {
                    user.status = 1;
                }

                user.createDate = DateTime.Now;
                user.email = email;
                user.phone = phone;
                user.password = password;
                user.role = 1;
                user.type = 1;
                user.username = email;
                db.Users.Add(user);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListNormalUser", "Admin");
        }
    }
}