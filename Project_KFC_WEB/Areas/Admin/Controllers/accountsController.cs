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
    public class accountsController : Controller
    {
        private KFC_Data db = new KFC_Data();

        // GET: Admin/accounts
        public ActionResult Index(int page = 1, bool isReset = false)
        {
            if (isReset) Session["isSearchingCart"] = false;

            List<account> accounts = new List<account>();
            bool isSearching = Session["isSearchingCart"] != null ? (bool)Session["isSearchingCart"] : false;

            var selectedOption = Request.QueryString["selectedOption"];
            var valueSearch = Request.QueryString["valueSearch"];

            accounts = db.accounts.ToList();
            if (!isSearching)
            {
                if (!string.IsNullOrEmpty(selectedOption) && !string.IsNullOrEmpty(valueSearch))
                {
                    isSearching = true;
                    Session["isSearchingAccount"] = isSearching;
                    accounts = SeearchAccount(accounts, selectedOption, valueSearch);
                }
            }

            if (isSearching)
            {
                accounts = Session["listAccount"] as List<account>;
                if (accounts.ToList().Count() == 0) Session["isSearchingCart"] = false;
                accounts = SeearchAccount(accounts, selectedOption, valueSearch);
            }

            int itemsPerPage = 5;
            int totalItems = accounts.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            page = Math.Max(1, Math.Min(page, totalPages));

            var startIndex = (page - 1) * itemsPerPage;
            var endIndex = Math.Min(startIndex + itemsPerPage - 1, totalItems - 1);

            List<account> accountPage;

            if (startIndex < 0 || startIndex >= totalItems)
            {
                accountPage = null;
            }
            else
            {
                accountPage = accounts.GetRange(startIndex, endIndex - startIndex + 1);
            }

            ViewBag.currentPage = page;
            Session["currentPageAccount"] = page;
            ViewBag.totalPages = totalPages;

            return View(accountPage);
        }

        public List<account> SeearchAccount(List<account> accounts, string selectedOption, string valueSearch)
        {
            if (!string.IsNullOrEmpty(selectedOption) && !string.IsNullOrEmpty(valueSearch))
            {
                if (selectedOption.Contains("đăng"))
                {
                    accounts = accounts.FindAll(item => item.userName.ToLower().Contains(valueSearch.Trim().ToLower()));
                }
                else if (selectedOption.Contains("người"))
                {
                    accounts = accounts.FindAll(item => item.name.ToLower().Contains(valueSearch.Trim().ToLower()));
                }
                else if (selectedOption.Contains("số"))
                {
                    accounts = accounts.FindAll(item => item.phone.Contains(valueSearch.Trim()));
                }
                else if (selectedOption.Contains("chỉ"))
                {
                    accounts = accounts.FindAll(item => item.address.ToLower().Contains(valueSearch.Trim().ToLower()));
                }
            }

            Session["listAccount"] = accounts;

            return accounts;
        }

        // GET: Admin/accounts/Details/5
        public ActionResult Details(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(userName);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Admin/accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userName,passWord,name,address,phone")] account account)
        {
            if (ModelState.IsValid)
            {
                db.accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Admin/accounts/Edit/5
        public ActionResult Edit(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.FirstOrDefault(item => item.userName == userName);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userName,passWord,name,address,phone")] account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Admin/accounts/Delete/5
        public ActionResult Delete(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.FirstOrDefault(item => item.userName == userName);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string userName)
        {
            account account = db.accounts.FirstOrDefault(item => item.userName == userName);
            db.accounts.Remove(account);
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
