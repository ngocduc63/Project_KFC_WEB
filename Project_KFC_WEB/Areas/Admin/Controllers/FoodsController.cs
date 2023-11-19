using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_KFC_WEB.Models;

namespace Project_KFC_WEB.Areas.Admin.Controllers
{
    public class FoodsController : Controller
    {
        private KFC_Data db = new KFC_Data();

        // GET: Admin/Foods
        public ActionResult Index()
        {
            var foods = db.foods.Include(f => f.foodCategory).ToList();

            ViewBag.foodCategories = db.foodCategories.ToList();

            var selectedCategoryOption = Request.QueryString["selectedCategoryOption"];
            var selectedOptionPrice = Request.QueryString["selectedOptionPrice"];
            var checkSale = Request.QueryString["checkSale"];
            var valueSearch = Request.QueryString["valueSearch"];

            if (selectedCategoryOption != null
              || selectedOptionPrice != null
              || valueSearch != null
              || checkSale != null)
            {
                foods = SrearchFood(foods, selectedCategoryOption, selectedOptionPrice, valueSearch, checkSale);
            }

            return View(foods);
        }

        public List<food> SrearchFood(List<food> foods, string selectedCategoryOption, string selectedOptionPrice, string valueSearch, string checkSale)
        {
            if (selectedCategoryOption != "") foods = foods.FindAll(item => item.foodCategory.name.ToLower().Equals(selectedCategoryOption.ToLower()));

            if (selectedOptionPrice != "")
            {
                var number = Convert.ToInt32(string.Join("", selectedOptionPrice.Split('-')[0].Trim().Split('.')));

                if (number == 0)
                {
                    foods = foods.FindAll(item => item.price >= 0 && item.price < 10000);
                }
                else if (number == 10000)
                {
                    foods = foods.FindAll(item => item.price >= 10000 && item.price < 50000);
                }
                else if (number == 50000)
                {
                    foods = foods.FindAll(item => item.price >= 50000 && item.price < 100000);
                }
                else if (number == 100000)
                {
                    foods = foods.FindAll(item => item.price >= 100000 && item.price < 500000);

                }
                else if (number == 500000)
                {
                    foods = foods.FindAll(item => item.price >= 500000 && item.price < 1000000);

                }
                else if (number == 1000000)
                {
                    foods = foods.FindAll(item => item.price >= 1000000);

                }
            }

            if (valueSearch != "") foods = foods.FindAll(item => item.name.ToLower().Contains(valueSearch.ToLower()));

            if (checkSale != null) foods = checkSale.Contains("on") ? foods.FindAll(item => item.discount > 0) : foods;


            return foods;
        }

        // GET: Admin/Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            food food = db.foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Admin/Foods/Create
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.foodCategories, "id", "name");
            return View();
        }

        // POST: Admin/Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idCategory,name,image,price,discount,description,timeSellStart,timeSellEnd")] food food, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    imageFile.SaveAs(path);
                    food.image = fileName;
                }

                var lastFood = db.foods.OrderByDescending(f => f.id).FirstOrDefault();

                if (lastFood != null)
                {
                    food.id = lastFood.id + 1;
                }
                else
                {
                    food.id = 1;
                }

                db.foods.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(food);
        }

        // GET: Admin/Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            food food = db.foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategory = new SelectList(db.foodCategories, "id", "name", food.idCategory);
            return View(food);
        }

        // POST: Admin/Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idCategory,name,image,price,discount,description,timeSellStart,timeSellEnd")] food food, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    imageFile.SaveAs(path);
                    food.image = fileName;

                    db.Entry(food).State = EntityState.Modified;
                }
                else
                {
                    var existingFood = db.foods.FirstOrDefault(item => item.id == food.id);

                    if (existingFood != null)
                    {
                        existingFood.idCategory = food.idCategory;
                        existingFood.name = food.name;
                        existingFood.price = food.price;
                        existingFood.discount = food.discount;
                        existingFood.description = food.description;
                        existingFood.timeSellStart = food.timeSellStart;
                        existingFood.timeSellEnd = food.timeSellEnd;

                        db.Entry(existingFood).State = EntityState.Modified;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.foodCategories, "id", "name", food.idCategory);
            return View(food);
        }

        // GET: Admin/Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            food food = db.foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Admin/Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            food food = db.foods.Find(id);
            db.foods.Remove(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
