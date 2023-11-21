using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_KFC_WEB.Models;

namespace Project_KFC_WEB.Controllers
{
    public class HomeController : Controller
    {
        KFC_Data db = new KFC_Data();

        public ActionResult Index() 
        {
            ViewBag.foodCategories = db.foodCategories.ToList();

            return View(db.foods.ToList());
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
    }
}