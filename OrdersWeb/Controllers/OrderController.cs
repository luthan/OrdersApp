using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrdersWeb.Models;
//using System.DirectoryServices.AccountManagement;


namespace OrdersWeb.Controllers
{
    public class OrderController : Controller
    {
        private OrderAppDatabaseContext db = new OrderAppDatabaseContext();

        public ActionResult Index()
        {
            return View(db.Orders.OrderByDescending(m => m.OrderDate).ThenByDescending(m=>m.Id).ToList());
        }

        

        // GET: Order/Create
        public ActionResult Order(int? id, string addOrUpdate)
        {
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");
            if (addOrUpdate == "Add")
            {
                ViewBag.addOrUpdate = addOrUpdate;
                ViewBag.Title = "Add Order";
                return View(new Order()
                {
                    OrderDate = DateTime.Today,
                    Items = new List<Item>()
                    {
                        new Item(){}
                    }
                });

            } else if(addOrUpdate == "Edit")
            {
                ViewBag.addOrUpdate = addOrUpdate;
                ViewBag.Title = "Edit Order";
                Order order = db.Orders.Include(i => i.Items).Where(i => i.Id == id).Single();
                return View(order);
            }
            else
            {
                ViewBag.Title = "Add Order";
                return View(new Order() { OrderDate = DateTime.Today, Items = new List<Item>() { new Item() } });
            }
            
            
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(Order order, string btnSubmit, string userName)
        {
            if (btnSubmit == "addRowAdd") 
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                order.Items.Add(new Item());
                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");
                ViewBag.addOrUpdate = "Add";
                ViewBag.Title = "Add Order";
                return View(order);
            }
            else if (btnSubmit == "addRowEdit")
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                order.Items.Add(new Item() { OrderId = order.Id });
                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                ViewBag.addOrUpdate = "Edit";
                ViewBag.Title = "Edit Order";
                return View(order);
            }
            else
            {
                if (btnSubmit == "Add")
                {
                    if (ModelState.IsValid)
                    {
                        order.UserName = userName;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                        ViewBag.Title = "Add Order";
                        return View(order);
                    }
                    
                }
                else if (btnSubmit == "Edit")
                {
                    if (ModelState.IsValid)
                    {
                        db.Items.RemoveRange(db.Items.Where(m => m.OrderId == order.Id));
                        db.SaveChanges();
                        foreach (var tmp in order.Items)
                        {
                            db.Items.Add(tmp);
                            db.SaveChanges();
                        }
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                        ViewBag.Title = "Add Order";
                        return View(order);
                    }
                }
                else
                {
                    return View();
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

            //Order order = db.Orders.Include(i => i.Items).Where(i => i.Id == id).Single();
           
            //ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x=>x.CategoryName), "Id", "CategoryName");

            return RedirectToAction("Order", new { id = id, addOrUpdate = "Edit" });
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Order order)
        //{
        //    db.Entry(order).State = EntityState.Modified;
        //    db.SaveChanges();
        //    foreach (var tmp in order.Items)
        //    {
        //        db.Entry(tmp).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Create");
        //}

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
