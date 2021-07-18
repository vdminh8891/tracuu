using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuBMT.General;
using TraCuuBMT.Models;

namespace TraCuuBMT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string type = "1", string hsCode = "", string mota = "")
        {
            if (!Util.CheckAuthenAndAuthor(1))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.ToString() });
            }

            ViewBag.type = type;
            //danh sách ban đầu của tất cả các loại
            //bieu thue
            ViewBag.ListThueVAT = Util.GetListThueVAT();
            List<BieuThue> listBieuThue = Util.GetListBieuThueBy("", "");
            ViewBag.ListBieuThue = listBieuThue;
            ViewBag.ListSubBieuThue = Util.GetListSubBieuThue(listBieuThue);

            //kết quả phân tích phân loại
            ViewBag.ListKQPTPL = Util.GetListKTPTPL();
            if (!string.IsNullOrEmpty(hsCode))
            {
                ViewBag.Keyword = hsCode;
            }
            if (!string.IsNullOrEmpty(mota))
            {
                ViewBag.Keyword = mota;
            }

            //search theo loại
            switch (type)
            {
                case "1":
                    ViewBag.ListThueVAT = Util.GetListThueVAT();
                    List<BieuThue> listBieuThueType1 = Util.GetListBieuThueBy(hsCode, mota);
                    ViewBag.ListBieuThue = listBieuThueType1;
                    ViewBag.ListSubBieuThue = Util.GetListSubBieuThue(listBieuThueType1);
                    break;

                case "2":
                    ViewBag.ListKQPTPL = Util.GetListKTPTPLBy(hsCode, mota);
                    break;
            }

            return View();
        }

        public ActionResult RegisterMore()
        {
            if (!Util.CheckAuthenAndAuthor(1))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.ToString() });
            }

            ViewBag.ListPackage = Util.GetListPackage();
            return View();
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

        [HttpPost]
        public ActionResult SendEmail(string itemId)
        {
            bool result = false;
            try
            {
                string fromEMail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                string password = ConfigurationManager.AppSettings["PasswordFromEmail"].ToString();
                string subject = ConfigurationManager.AppSettings["SubjectSendFileEmail"].ToString();
                string body = ConfigurationManager.AppSettings["BodySendFileEmail"].ToString();
                User user = (User)Session["userInfo"];
                string toEmail = user.email;
                KetQuaPhanTichPhanLoai item = Util.GetKQPTPLById(itemId);
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.link_file_vn))
                    {
                        string UploadPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["KQPTPLPath"].ToString());

                        //Its Create complete path to store in server.  
                        string fullpath = UploadPath + item.link_file_vn;
                        if (System.IO.File.Exists(fullpath))
                        {
                            Util.SendEmail(toEmail, fromEMail, subject, body, password, fullpath);
                            result = true;
                        }
                    }
                }
                else
                {
                    //ko tìm thấy bieu thue
                }


            }
            catch (Exception ex)
            {
                result = false;
            }

            return Json(new { result = result, JsonRequestBehavior.AllowGet }); ;
        }
    }
}