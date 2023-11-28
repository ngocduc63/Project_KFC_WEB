using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_KFC_WEB.Models;

namespace Project_KFC_WEB.Controllers
{
    public class LoginController : Controller
    {
        KFC_Data db = new KFC_Data();

        // GET: Login
        public ActionResult Index(int check = 0)
        {
            if(check == 2)
            {
                ViewBag.error = "Sai tài khoản mật khẩu";
            }

            bool isLogin = Session["isLoginUser"] == null ? false : (bool) Session["isLoginUser"];

            if (isLogin) return RedirectToAction("Profile", "Login");

            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();

            return View();
        }

        public ActionResult Profile()
        {

            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();

            return View();
        }

        public ActionResult Register(bool check = true)
        {
            if(!check)
            {
                ViewBag.error = "Đăng kí thất bại";
            }

            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();

            return View();
        }

        public ActionResult CheckLogin()
        {
            var userName = Request.QueryString["emailLogin"];
            var passWord = Request.QueryString["passLogin"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return RedirectToAction("Index", "Login", new { check = 2 });
            }
            else
            {
                if (db.accounts.ToList().Find(item => item.userName == userName && item.passWord == passWord) == null)
                {
                    return RedirectToAction("Index", "Login", new { check = 2 });
                }
                else
                {
                    Session["userName"] = userName;
                    Session["isLoginUser"] = true;

                    return RedirectToAction("Index", "Home");
                }

            }
        }

        public ActionResult CheckRegister()
        {
            var userName = Request.QueryString["emailLogin"];
            var passWord = Request.QueryString["passLogin"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return RedirectToAction("Index", "Login", new { check = 2 });
            }
            else
            {
                try
                {
                    account acc = new account();

                    acc.userName = userName;
                    acc.passWord = passWord;
                    acc.carts = null;

                    db.accounts.Add(acc);
                    db.SaveChanges();

                    Session["userName"] = userName;
                    Session["isLoginUser"] = true;

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    return RedirectToAction("Register", "Login", new { check  = false});
                }

                
            }

        }
    }
}