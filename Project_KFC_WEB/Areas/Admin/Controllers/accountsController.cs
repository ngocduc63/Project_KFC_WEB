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
        public ActionResult Index()
        {
            var accounts = db.accounts.ToList();
            var selectedOption = Request.QueryString["selectedOption"];
            if (selectedOption != null)
            {
                var valueSearch = Request.QueryString["valueSearch"];
                if (valueSearch != null)
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
            }

            return View(accounts);
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
