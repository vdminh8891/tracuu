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
        private AccountController Acc = new AccountController();

        public ActionResult Index()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }


        public ActionResult ListKQPTPL()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            List<KetQuaPhanTichPhanLoai> listKQPTPL = new List<KetQuaPhanTichPhanLoai>();
            using (var db = new TraCuuBMTEntities())
            {
                listKQPTPL = db.KetQuaPhanTichPhanLoais.Where(w => w.status > 0).OrderByDescending(w=>w.createDate).ToList();
            }
            ViewBag.ListKQPTPL = listKQPTPL;
            ViewBag.KQPTPLPath = Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["KQPTPLPath"].ToString();
            return View();
        }

        public ActionResult EditKQPTPL(string id)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }

            using (var db = new TraCuuBMTEntities())
            {
                KetQuaPhanTichPhanLoai item = db.KetQuaPhanTichPhanLoais.Where(w => w.status > 0 && w.ID == id).FirstOrDefault();
                if (item != null)
                {
                    ViewBag.KetQuaPhanTichPhanLoai = item;
                    ViewBag.KQPTPLPath = Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["KQPTPLPath"].ToString();
                    return View();
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy Kết quả phân tích phân loại này";
                    return RedirectToAction("ListKQPTPL", "Admin");
                }
            }
        }

        [HttpPost]
        public ActionResult EditKQPTPL(FormCollection form, HttpPostedFileBase file_vn)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            //validate
            string kqptplId = form["kqptplId"] ?? "";
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
            DateTime? Ngay_Vanban_HH = null;
            if (!string.IsNullOrEmpty(Ngay_Vanban_HHString))
            {
                Ngay_Vanban_HH = DateTime.ParseExact(Ngay_Vanban_HHString, "MM/dd/yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
            }
             
            string statusString = form["status"] ?? "";

            string FileName = "";
            if (file_vn != null && file_vn?.ContentLength != 0)
            {
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
            }

            using (var db = new TraCuuBMTEntities())
            {
                if (!string.IsNullOrEmpty(kqptplId))
                {
                    KetQuaPhanTichPhanLoai kqptpl = db.KetQuaPhanTichPhanLoais.Where(w => w.status > 0 && w.ID == kqptplId).FirstOrDefault();
                    if (kqptpl != null)
                    {
                        int temp = Util.ParseStringToInt(statusString);
                        if (temp > -99)
                        {
                            kqptpl.status = temp;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Lỗi dữ liệu!";
                            return RedirectToAction("ListKQPTPL", "Admin");
                        }
                        kqptpl.hsCode = hsCode;
                        kqptpl.So_Vanban = so_Vanban;
                        kqptpl.CoQuan_PhatHanh_PTPL = coQuan_PhatHanh_PTPL;
                        kqptpl.Mota_Dnkhaibao = mota_Dnkhaibao;
                        kqptpl.Mota_KQPTPL = mota_KQPTPL;
                        kqptpl.DonVi_XNK = donVi_XNK;
                        kqptpl.Co_Quan_YC_PTPL = co_Quan_YC_PTPL;
                        kqptpl.ToKhai_HQ = toKhai_HQ;
                        kqptpl.Loai_Hinh = loai_Hinh;
                        if (!string.IsNullOrEmpty(FileName))
                        {
                            kqptpl.link_file_vn = FileName;
                        }
                        kqptpl.Ngay_Vanban = Ngay_Vanban;
                        kqptpl.Ngay_Vanban_HH = Ngay_Vanban_HH;
                        kqptpl.unit = unit;
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Tạo mới thành công!";
                        return RedirectToAction("ListKQPTPL", "Admin");
                    }
                    else
                    {
                        TempData["WarningMessage"] = "Không tìm thấy Kết quả phân tích phân loại này";
                        return RedirectToAction("ListKQPTPL", "Admin");
                    }
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy Kết quả phân tích phân loại này";
                    return RedirectToAction("ListKQPTPL", "Admin");
                }
            }

        }

        public ActionResult CreateKQPTPL()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateKQPTPL(FormCollection form, HttpPostedFileBase file_vn)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            DateTime? Ngay_Vanban_HH = null;
            if (!string.IsNullOrEmpty(Ngay_Vanban_HHString))
            {
                Ngay_Vanban_HH = DateTime.ParseExact(Ngay_Vanban_HHString, "MM/dd/yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
            }
             
            string statusString = form["status"] ?? "";

            string FileName = "";
            if (file_vn != null && file_vn?.ContentLength != 0)
            {
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
            }



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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                double THUE_TVCBPG = Util.ParseStringToFloat(form["THUE_TVCBPG"] ?? "");
                double THUE_TTDB = Util.ParseStringToFloat(form["THUE_TTDB"] ?? "");
                double THUE_PBDX = Util.ParseStringToFloat(form["THUE_PBDX"] ?? "");
                double THUE_EXPORT = Util.ParseStringToFloat(form["THUE_EXPORT"] ?? "");
                double THUE_BVMT = Util.ParseStringToFloat(form["THUE_BVMT"] ?? "");
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
                        bieuThue.THUE_BVMT = THUE_BVMT;
                        bieuThue.THUE_EXPORT = THUE_EXPORT;
                        bieuThue.THUE_PBDX = THUE_PBDX;
                        bieuThue.THUE_TTDB = THUE_TTDB;
                        bieuThue.THUE_TVCBPG = THUE_TVCBPG;

                        if (!string.IsNullOrEmpty(FileName))
                        {
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateBieuThue(FormCollection form, HttpPostedFileBase file_vn)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            double THUE_TVCBPG = Util.ParseStringToFloat(form["THUE_TVCBPG"] ?? "");
            double THUE_TTDB = Util.ParseStringToFloat(form["THUE_TTDB"] ?? "");
            double THUE_PBDX = Util.ParseStringToFloat(form["THUE_PBDX"] ?? "");
            double THUE_EXPORT = Util.ParseStringToFloat(form["THUE_EXPORT"] ?? "");
            double THUE_BVMT = Util.ParseStringToFloat(form["THUE_BVMT"] ?? "");

            string statusString = form["status"] ?? "";

            string FileName = "";
            if (file_vn != null && !string.IsNullOrEmpty(file_vn.FileName))
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
                item.description = description??"";
                item.tenBieuThue = tenBieuThue??"";
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
                item.THUE_BVMT = THUE_BVMT;
                item.THUE_EXPORT = THUE_EXPORT;
                item.THUE_PBDX = THUE_PBDX;
                item.THUE_TTDB = THUE_TTDB;
                item.THUE_TVCBPG = THUE_TVCBPG;
                db.BieuThues.Add(item);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListBieuThue", "Admin");
        }

        public ActionResult TransactionUser()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            using (var db = new TraCuuBMTEntities())
            {
                ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
                ViewBag.ListTransaction = db.Transactions.Where(w => w.status >= 0).OrderByDescending(w => w.createDate).ToList();

            }
            return View();
        }

        public ActionResult TransactionStatistic()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            using (var db = new TraCuuBMTEntities())
            {
                ViewBag.ListTransaction = db.Transactions.Where(w => w.status >= 0).OrderByDescending(w => w.createDate).ToList();
                ViewBag.ListUser = db.Users.Where(w => w.status >= 0).ToList();
            }
            return View();
        }

        public ActionResult CreateTransaction()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            using (var db = new TraCuuBMTEntities())
            {
                string transactionId = form["transactionId"] ?? "";
                string note = form["note"];
                string statusString = form["status"] ?? "";
                if (!string.IsNullOrEmpty(transactionId))
                {
                    Transaction item = db.Transactions.Where(w => w.status >= 0 && w.ID == transactionId).FirstOrDefault();
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
            TempData["WarningMessage"] = "Không tìm thấy giao dịch này";
            return RedirectToAction("ListTransaction", "Admin");
        }

        public ActionResult ListTransaction()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreatePackage(FormCollection form)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            //validate
            string packageName = form["packageName"];
            string description = form["description"];
            string amount = form["amount"];
            string price = form["price"];
            string statusString = form["status"] ?? "";

            using (var db = new TraCuuBMTEntities())
            {
                Package item = new Package();
                item.ID = Util.GenerateID("package");
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
                item.amount = Util.ParseStringToInt(amount ?? "");
                item.description = description;
                User currentUser = Util.GetCurrentUser();
                item.creator = currentUser != null ? currentUser.email : "";
                item.lastEditDate = DateTime.Now;
                item.lastEditor = currentUser != null ? currentUser.email : "";
                item.packageName = packageName;
                item.price = Util.ParseStringToFloat(price ?? "");
                item.type = 1;

                db.Packages.Add(item);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListPackage", "Admin");
        }

        public ActionResult EditPackage(string id)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            using (var db = new TraCuuBMTEntities())
            {
                Package item = db.Packages.Where(w => w.status > 0 && w.ID == id).FirstOrDefault();
                if (item != null)
                {
                    ViewBag.Package = item;
                    return View();
                }
                else
                {
                    TempData["WarningMessage"] = "Không tìm thấy package này";
                    return RedirectToAction("ListPackage", "Admin");
                }
            }
        }

        [HttpPost]
        public ActionResult EditPackage(FormCollection form)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            using (var db = new TraCuuBMTEntities())
            {
                string packageName = form["packageName"];
                string description = form["description"];
                string amount = form["amount"];
                string price = form["price"];
                string statusString = form["status"] ?? "";
                string packageId = form["packageId"] ?? "";

                if (!string.IsNullOrEmpty(packageId))
                {
                    Package item = db.Packages.Where(w => w.status > 0 && w.ID == packageId).FirstOrDefault();
                    if (item != null)
                    {
                        int temp = Util.ParseStringToInt(statusString);
                        if (temp > -99)
                        {
                            item.status = temp;
                            item.amount = Util.ParseStringToInt(amount);
                            item.price = Util.ParseStringToInt(price);
                            item.packageName = packageName;
                            item.lastEditDate = DateTime.Now;
                            item.description = description;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Lỗi dữ liệu!";
                            return RedirectToAction("ListPackage", "Admin");
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                        return RedirectToAction("ListPackage", "Admin");
                    }

                }
            }
            TempData["WarningMessage"] = "Không tìm thấy package này";
            return RedirectToAction("ListPackage", "Admin");
        }

        public ActionResult ListPackage()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateNormalUser(FormCollection form)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                user.password = Util.CreateMD5(password);
                user.role = 1;
                user.type = 1;
                user.username = email;
                db.Users.Add(user);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListNormalUser", "Admin");
        }


        public ActionResult ListAdminUser()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            List<User> listUser = new List<User>();
            using (var db = new TraCuuBMTEntities())
            {
                listUser = db.Users.Where(w => w.status > 0 && w.role == 2).ToList();
            }
            ViewBag.ListUser = listUser;
            return View();
        }
        public ActionResult CreateAdminUser()
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdminUser(FormCollection form)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                user.password = Util.CreateMD5(password);
                user.role = 2; //admin
                user.type = 1; //admin type
                user.username = email;
                db.Users.Add(user);
                db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Tạo mới thành công!";
            return RedirectToAction("ListAdminUser", "Admin");
        }

        public ActionResult EditAdminUser(string id)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                    return RedirectToAction("ListAdminUser", "Admin");
                }
            }
        }


        [HttpPost]
        public ActionResult EditAdminUser(FormCollection form)
        {
            if (!Util.CheckAuthenAndAuthor(2))
            {
                return RedirectToAction("LoginAdmin", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                            return RedirectToAction("ListAdminUser", "Admin");
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                        return RedirectToAction("ListAdminUser", "Admin");
                    }

                }
            }
            TempData["WarningMessage"] = "Không tìm thấy user này";
            return RedirectToAction("ListNormalUser", "Admin");
        }


    }
}