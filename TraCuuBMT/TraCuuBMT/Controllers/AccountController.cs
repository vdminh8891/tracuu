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

        public ActionResult Login()
        {
            return View();
        }        
        
        public ActionResult Register()
        {
            return View();
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
                    if(temp > 0)
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

        [HttpPost]
        public ActionResult Login(string txtPhone, string txtEmail, string txtPassword)
        {
            using (var db = new TraCuuBMTEntities())
            {
                string md5Password = Util.CreateMD5(txtPassword);
                var user = db.Users.Where(w => w.phone == txtPhone && w.email == txtEmail && w.password == md5Password && w.status > -1).FirstOrDefault();
                if (user != null)
                {
                    Session["userInfo"] = user;
                    return RedirectToAction("Index", "Home");
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


    }
}