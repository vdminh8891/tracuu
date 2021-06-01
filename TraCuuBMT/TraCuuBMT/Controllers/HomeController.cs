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
        public ActionResult Index()
        {
            if (Session["userInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ListBieuThue = Util.GetListBieuThue();
            ViewBag.ListKQPTPL = Util.GetListKTPTPL();

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
                if(item != null)
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