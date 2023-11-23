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
            List<cart> carts = new List<cart>(); 
            if(Session["cartUser"] as List<cart> == null)
            {
                Session["cartUser"] = new List<cart>();
            }
            else
            {
                carts = Session["cartUser"] as List<cart>;
            } 

            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();
            ViewBag.quantityCart = carts.Count;

            return View(db.foods.ToList());
        }

        public ActionResult Menu(int index = 0)
        {
            List<cart> carts = new List<cart>();
            if (Session["cartUser"] as List<cart> == null)
            {
                Session["cartUser"] = new List<cart>();
            }
            else
            {
                carts = Session["cartUser"] as List<cart>;
            }

            ViewBag.foodCategories = db.foodCategories.ToList();
            ViewBag.Length = db.foodCategories.ToList().Count();
            ViewBag.currentIndex = index;
            ViewBag.quantityCart = carts.Count;

            return View(db.foods.ToList());
        }

        public ActionResult FoodDetail(int id = 1)
        {
            List<cart> carts = new List<cart>();
            if (Session["cartUser"] as List<cart> == null)
            {
                Session["cartUser"] = new List<cart>();
            }
            else
            {
                carts = Session["cartUser"] as List<cart>;
            }

            var idCategory = db.foods.ToList().FirstOrDefault(item => item.id == id).idCategory;
            ViewBag.category = db.foodCategories.ToList().FirstOrDefault(item => item.id == idCategory);
            ViewBag.index = db.foodCategories.ToList().FindIndex(item => item.id == idCategory);
            ViewBag.quantityCart = carts.Count;

            return View(db.foods.ToList().FirstOrDefault( item => item.id == id));
        }

        public ActionResult Cart()
        {
            List<cart> carts = Session["cartUser"] as List<cart>;
            ViewBag.listFood = db.foods.ToList();
            ViewBag.quantityCart = carts.Count;

            return View(carts);
        }

        public ActionResult AddCart(int? id, string view = "Index" ,int quantity = 1)
        {
            int index = 1;
            if(id != null)
            {
                List<cart> carts = new List<cart>();
                if (Session["cartUser"] as List<cart> == null)
                {
                    Session["cartUser"] = new List<cart>();
                }
                else
                {
                    carts = Session["cartUser"] as List<cart>;
                }

                var userName = Session["userName"] == null ? "admin" : Session["userName"] as string;

                cart cart = new cart();
                cart.idFood = id;
                cart.userName = userName;
                cart.quantity = quantity;

                carts.Add(cart);

                Session["cartUser"] = carts;

                var idCategory = db.foods.ToList().FirstOrDefault(item => item.id == id).idCategory;
                index = db.foodCategories.ToList().FindIndex(item => item.id == idCategory);
            }

            return RedirectToAction(view, new { index = index});
        }
    }
}