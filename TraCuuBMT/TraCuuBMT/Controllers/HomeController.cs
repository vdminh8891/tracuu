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

            ViewBag.ListPackage = Util.GetListPackage();
            ViewBag.ListTransaction = Util.GetListTransactionByUserId(Util.GetCurrentUser().ID);

            ViewBag.type = type;
            //danh sách ban đầu của tất cả các loại
            //bieu thue
            ViewBag.ListThueVAT = Util.GetListThueVAT();
            List<BieuThue> listBieuThue = Util.GetListBieuThueBy("", "");
            ViewBag.ListBieuThue = listBieuThue;
            ViewBag.ListSubBieuThue = Util.GetListSubBieuThue(listBieuThue);

            //kết quả phân tích phân loại
            //ViewBag.ListKQPTPL = Util.GetListKTPTPL();
            if (!string.IsNullOrEmpty(hsCode))
            {
                ViewBag.Keyword = hsCode;
            }

            if (!string.IsNullOrEmpty(mota))
            {
                ViewBag.Keyword = mota;
            }

            if (string.IsNullOrEmpty(mota) && string.IsNullOrEmpty(hsCode))
            {
                hsCode = "aaaaaaaaaaaaaaaaaaaa";//for empty result
                mota = "aaaaaaaaaaaaaaaaaaaa";//for empty result

            }
            ViewBag.ListKQPTPL = Util.GetListKTPTPLBy(hsCode, mota);

            //search theo loại
            switch (type)
            {
                case "1":
                    ViewBag.ListThueVAT = Util.GetListThueVATByNameOrMoTa(mota);
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

        //public ActionResult RegisterMore()
        //{
        //    if (!Util.CheckAuthenAndAuthor(1))
        //    {
        //        return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.ToString() });
        //    }

        //    ViewBag.ListPackage = Util.GetListPackage();
        //    return View();
        //}



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
            string message = "";
            string detailError = "";
            if (!Util.CheckAuthenAndAuthor(1))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.ToString() });
            }
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
                            result = Util.SendEmail(toEmail, fromEMail, subject, body, password, fullpath, ref detailError);
                            if (result)
                            {
                                Util.CreateTransactionByAmount(user.ID, -1);
                                message = "Gửi file thành công";
                            }
                            else
                            {
                                //gửi file thất bại, log của
                                message = "Lỗi hệ thống, gửi file thất bại";
                            }
                        }
                        else
                        {
                            //Không tìm thấy file
                            message = "Gửi file thất bại, không tìm thấy file!";
                        }
                    }
                }
                else
                {
                    message = "Gửi file thất bại, không tìm thấy kết quả phân tích phân loại!";
                    //ko tìm thấy bieu thue
                }


            }
            catch (Exception ex)
            {
                result = false;
                message = "Lỗi hệ thống, gửi file thất bại";
                detailError = ex.ToString();
            }

            return Json(new { result, message, detailError, JsonRequestBehavior.AllowGet }); ;
        }


        [HttpPost]
        public ActionResult RegisterMore(string userId, List<string> listPackageId)
        {
            bool result = false;
            string message = "";
            string detailMessage = "";
            if (!Util.CheckAuthenAndAuthor(1))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.ToString() });
            }
            try
            {
                if (Util.GetCurrentUser().ID == userId)
                {
                    int resultCount = 0;
                    foreach (string packageId in listPackageId)
                    {
                        if (!string.IsNullOrEmpty(packageId))
                        {
                            int insertResult = Util.CreateTransactionByPackageId(userId, packageId);
                            resultCount += insertResult;
                        }
                    }
                    if (resultCount > 0)
                    {
                        if (resultCount == listPackageId.Count)
                        {
                            //insert ok all
                            message = "Đăng ký thành công";
                        }
                        else
                        {
                            //insert not ok all
                            message = "Có lỗi xảy ra! Đăng ký thành công 1 bộ phận!";
                        }
                        result = true;
                    }
                    else
                    {
                        message = "Có lỗi xảy ra! Vui lòng thử lại";
                        detailMessage = "resultCount = 0";
                    }
                }
                else
                {
                    message = "Có lỗi xảy ra! Vui lòng thử lại";
                    detailMessage = "Error session";
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Có lỗi xảy ra! Vui lòng thử lại";
                detailMessage = ex.ToString();
            }

            //return Json(new { result = result, JsonRequestBehavior.AllowGet }); ;
            return Json(new
            {
                result = result,
                message = message,
                detailMessage = detailMessage
            });
        }
    }
}