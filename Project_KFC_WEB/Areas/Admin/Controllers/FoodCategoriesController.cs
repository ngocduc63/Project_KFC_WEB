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
    public class FoodCategoriesController : Controller
    {
        private KFC_Data db = new KFC_Data();

        // GET: Admin/FoodCategories
        public ActionResult Index()
        {
            var foodCategories = db.foodCategories.ToList();
            var valueSearch = Request.QueryString["valueSearch"];

            if(valueSearch != null)
            {
                foodCategories = foodCategories.FindAll(item => item.name.ToLower().Contains(valueSearch.ToLower()));
            }

            return View(foodCategories);
        }

        // GET: Admin/FoodCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foodCategory foodCategory = db.foodCategories.FirstOrDefault((item) => item.id == id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // GET: Admin/FoodCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/FoodCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,image")] foodCategory foodCategory, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    imageFile.SaveAs(path);
                    foodCategory.image = fileName;
                }

                var lastFoodCategory = db.foodCategories.OrderByDescending(fc => fc.id).FirstOrDefault();

                if (lastFoodCategory != null)
                {
                    foodCategory.id = lastFoodCategory.id + 1;
                }
                else
                {
                    foodCategory.id = 1;
                }

                db.foodCategories.Add(foodCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foodCategory);
        }

        // GET: Admin/FoodCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foodCategory foodCategory = db.foodCategories.FirstOrDefault((item) => item.id == id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // POST: Admin/FoodCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,image")] foodCategory foodCategory, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    imageFile.SaveAs(path);
                    foodCategory.image = fileName;
                    db.Entry(foodCategory).State = EntityState.Modified;
                }
                else
                {
                    var existingFoodCategory = db.foodCategories.FirstOrDefault(item => item.id == foodCategory.id);

                    if (existingFoodCategory != null)
                    {
                        existingFoodCategory.name = foodCategory.name;
                        db.Entry(existingFoodCategory).State = EntityState.Modified;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foodCategory);
        }


        // GET: Admin/FoodCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foodCategory foodCategory = db.foodCategories.FirstOrDefault((item) => item.id == id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // POST: Admin/FoodCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            foodCategory foodCategory = db.foodCategories.FirstOrDefault((item) => item.id == id);
            db.foodCategories.Remove(foodCategory);
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
