using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_KFC_WEB.Models;

namespace Project_KFC_WEB.Areas.Admin.Controllers
{
    public class CartsController : Controller
    {
        private KFC_Data db = new KFC_Data();

        // GET: Admin/Carts
        public ActionResult Index(int page = 1, bool isReset = false)
        {
            if (isReset) Session["isSearchingCart"] = false;

            List<cart> carts = new List<cart>();
            bool isSearching = Session["isSearchingCart"] != null ? (bool)Session["isSearchingCart"] : false;
            var isSearchingText = Request.QueryString["isSearching"];

            var selectedOptionPrice = Request.QueryString["selectedOptionPrice"];
            var valueSearch = Request.QueryString["valueSearch"];

            carts = db.carts.Include(c => c.account).Include(c => c.food).ToList();
            if(!isSearching)
            {
                if (selectedOptionPrice != null || valueSearch != null)
                {
                    if (string.IsNullOrEmpty(selectedOptionPrice) || string.IsNullOrEmpty(valueSearch))
                    {
                        isSearching = true;
                        Session["isSearchingCart"] = isSearching;
                        carts = SearchCart(carts, selectedOptionPrice, valueSearch);
                        Session["listCart"] = carts;
                    }
                }
            }

            if (isSearching) 
            {
                carts = Session["listCart"] as List<cart>;
                if (carts.ToList().Count() == 0) Session["isSearchingCart"] = false;
            }

            int itemsPerPage = 5;
            int totalItems = carts.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            page = Math.Max(1, Math.Min(page, totalPages));

            var startIndex = (page - 1) * itemsPerPage;
            var endIndex = Math.Min(startIndex + itemsPerPage - 1, totalItems - 1);

            List<cart> cartPage;

            if (startIndex < 0 || startIndex >= totalItems)
            {
                cartPage = null;
            }
            else
            {
                cartPage = carts.GetRange(startIndex, endIndex - startIndex + 1);
            }

            ViewBag.currentPage = page;
            Session["currentPageCart"] = page;
            ViewBag.totalPages = totalPages;

            return View(cartPage);
        }
        public List<cart> SearchCart(List<cart> carts, string selectedOptiontotalPrice, string valueSearch)
        {
            if (selectedOptiontotalPrice != "")
            {
                var number = Convert.ToInt32(string.Join("", selectedOptiontotalPrice.Split('-')[0].Trim().Split('.')));

                if (number == 0)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 0 && item.totalPrice < 10000);
                }
                else if (number == 10000)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 10000 && item.totalPrice < 50000);
                }
                else if (number == 50000)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 50000 && item.totalPrice < 100000);
                }
                else if (number == 100000)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 100000 && item.totalPrice < 500000);

                }
                else if (number == 500000)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 500000 && item.totalPrice < 1000000);

                }
                else if (number == 1000000)
                {
                    carts = carts.FindAll(item => item.totalPrice >= 1000000);

                }
            }

            if (valueSearch != "") carts = carts.FindAll(item => item.account.name.ToLower().Contains(valueSearch.Trim().ToLower()));

            return carts;
        }

        // GET: Admin/Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Admin/Carts/Create
        public ActionResult Create()
        {
            ViewBag.userName = new SelectList(db.accounts, "userName", "name");
            ViewBag.idFood = new SelectList(db.foods, "id", "name");
            return View();
        }

        // POST: Admin/Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idFood,userName,quantity,totalPrice")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userName = new SelectList(db.accounts, "userName", "name", cart.userName);
            ViewBag.idFood = new SelectList(db.foods, "id", "name", cart.idFood);
            return View(cart);
        }

        // GET: Admin/Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.userName = new SelectList(db.accounts, "userName", "name", cart.userName);
            ViewBag.idFood = new SelectList(db.foods, "id", "name", cart.idFood);
            return View(cart);
        }

        // POST: Admin/Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idFood,userName,quantity,totalPrice")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userName = new SelectList(db.accounts, "userName", "name", cart.userName);
            ViewBag.idFood = new SelectList(db.foods, "id", "name", cart.idFood);
            return View(cart);
        }

        // GET: Admin/Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Admin/Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cart cart = db.carts.Find(id);
            db.carts.Remove(cart);
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
