using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuBMT.General;
using TraCuuBMT.Models;

namespace TraCuuBMT.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(string userId, string newPassword, string confirmNewPassword)
        {
            string result = "0";
            string message = "";
            string detailMessage = "";
            if (Session["userInfo"] != null)
            {
                try
                {
                    User user = (User)Session["userInfo"];

                    using (var db = new TraCuuBMTEntities())
                    {
                        var userTemp = db.Users.Where(w => w.ID == userId && w.status > -1).FirstOrDefault();
                        if (userTemp != null && userTemp.ID == user.ID)
                        {
                            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmNewPassword))
                            {
                                if (newPassword == confirmNewPassword)
                                {
                                    userTemp.password = Util.CreateMD5(newPassword);
                                    db.SaveChanges();
                                    result = "1";
                                    message = "Đổi mật khẩu thành công";
                                }
                                else
                                {
                                    result = "-1";
                                    message = "Mật khẩu không khớp";
                                }
                            }
                            else
                            {
                                result = "-1";
                                message = "Mật khẩu không thể rỗng";
                            }
                        }
                        else
                        {
                            result = "-1";
                            message = "Phiên đăng nhập không hợp lệ";
                        }
                    }

                }
                catch (Exception ex)
                {
                    result = "-3";
                    message = "Lỗi hệ thống";
                    detailMessage = ex.ToString();
                }
            }
            else
            {
                result = "-1";
                message = "Phiên đăng nhập không hợp lệ";
            }

            return Json(new
            {
                result = result,
                message = message,
                detailMessage = detailMessage
            });
        }

        [HttpPost]
        public ActionResult Register(string txtPhone, string txtEmail, string txtPassword, string txtRepassword)
        {
            //validate
            using (var db = new TraCuuBMTEntities())
            {
                var user = db.Users.Where(w => w.phone == txtPhone && w.email == txtEmail && w.status > -1).FirstOrDefault();
                if (user == null)
                {
                    user = new User();
                    user.username = txtEmail;
                    user.email = txtEmail;
                    user.phone = txtPhone;
                    user.role = 1;
                    user.status = 1;
                    user.type = 1; //normal user
                    user.createDate = DateTime.Now;
                    user.ID = Util.GenerateID("user");
                    user.password = Util.CreateMD5(txtPassword);

                    db.Users.Add(user);
                    int temp = db.SaveChanges();
                    if (temp > 0)
                    {
                        //tạo giao dịch + lượt tải
                        Transaction newItem = new Transaction();
                        newItem.ID = Util.GenerateID("transaction");
                        newItem.createDate = DateTime.Now;
                        newItem.creator = "System";
                        newItem.currentPrice = 0;
                        newItem.status = 1;//ok, apply
                        newItem.type = 1;//created by system
                        newItem.userId = user.ID;
                        newItem.packageId = "firstRegisterPackage";
                        db.Transactions.Add(newItem);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string txtPhone, string txtEmail, string txtPassword, string returnUrl)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string md5Password = Util.CreateMD5(txtPassword);
                var user = db.Users.Where(w => w.phone == txtPhone && w.email == txtEmail && w.password == md5Password && w.role == 1 && w.status > -1).FirstOrDefault();
                if (user != null)
                {
                    Session["userInfo"] = user;
                    TempData.Remove("WarningMessage");

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["WarningMessage"] = "Thông tin đăng nhập không đúng";
                }
            }
            return View();
        }

        public ActionResult LoginAdmin(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(string txtPhone, string txtEmail, string txtPassword, string returnUrl)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string md5Password = Util.CreateMD5(txtPassword);
                var user = db.Users.Where(w => w.phone == txtPhone && w.email == txtEmail && w.password == md5Password && w.status > -1 && w.role == 2).FirstOrDefault();
                if (user != null)
                {
                    Session["userInfo"] = user;

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult LogoutAdmin()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("LoginAdmin", "Account");
        }

        


    }
}