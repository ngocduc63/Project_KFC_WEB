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
            ViewBag.Length = db.foodCategories.ToList().Count();

            return View(db.foods.ToList());
        }

        public ActionResult Menu(int index = 0)
        {
            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();
            ViewBag.currentIndex = index;

            return View(db.foods.ToList());
        }

        public ActionResult FoodDetail(int id = 1)
        {
            var idCategory = db.foods.ToList().FirstOrDefault(item => item.id == id).idCategory;
            ViewBag.category = db.foodCategories.ToList().FirstOrDefault(item => item.id == idCategory);
            ViewBag.index = db.foodCategories.ToList().FindIndex(item => item.id == idCategory);

            return View(db.foods.ToList().FirstOrDefault( item => item.id == id));
        }
    }
}