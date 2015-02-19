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
    public class OrderController : Controller
    {
        private OrderAppDatabaseContext db = new OrderAppDatabaseContext();

        // GET: Order
        public ActionResult Index()
        {
            List<Order> allOrders = db.Orders.ToList();
            foreach (var order in allOrders)
            {
                List<Item> items = db.Items.Where(m => m.OrderId == order.Id).ToList();
                order.Items = items;
            }
            return View(db.Orders.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include(i => i.Items).Where(i => i.Id == id).Single();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {

            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");
            return View(new Order()
            {
                OrderDate = DateTime.Today,
                Items = new List<Item>()
            {
                new Item(){}
            }

            });
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, string addRow)
        {
            if (addRow != null) 
            {
                order.Items.Add(new Item());
                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");
                return View(order);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    return View(order);
                }
               
            }

            
        }


        
        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Include(i => i.Items).Where(i => i.Id == id).Single();
           
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");

            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            foreach (var tmp in order.Items)
            {
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Create");
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            List<Item> items = db.Items.Where(s => s.OrderId == id).ToList();
            foreach (var tmp in items)
            {
                db.Items.Remove(tmp);
                db.SaveChanges();
            }
            db.Orders.Remove(order);
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
