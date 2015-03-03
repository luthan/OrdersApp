using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrdersWeb.Models;

namespace OrdersWeb.Controllers
{
    public class BillingCategoryController : Controller
    {
        private OrderAppDatabaseContext db = new OrderAppDatabaseContext();

        // GET: BillingCategory
        public ActionResult Index()
        {
            return View(db.BillingCategories.ToList());
        }

        // GET: BillingCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingCategory billingCategory = db.BillingCategories.Find(id);
            if (billingCategory == null)
            {
                return HttpNotFound();
            }
            return View(billingCategory);
        }

        // GET: BillingCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BillingCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillingCategoryName")] BillingCategory billingCategory)
        {
            if (ModelState.IsValid)
            {
                db.BillingCategories.Add(billingCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(billingCategory);
        }

        // GET: BillingCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingCategory billingCategory = db.BillingCategories.Find(id);
            if (billingCategory == null)
            {
                return HttpNotFound();
            }
            return View(billingCategory);
        }

        // POST: BillingCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillingCategoryName")] BillingCategory billingCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billingCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(billingCategory);
        }

        // GET: BillingCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingCategory billingCategory = db.BillingCategories.Find(id);
            if (billingCategory == null)
            {
                return HttpNotFound();
            }
            return View(billingCategory);
        }

        // POST: BillingCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BillingCategory billingCategory = db.BillingCategories.Find(id);
            db.BillingCategories.Remove(billingCategory);
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
