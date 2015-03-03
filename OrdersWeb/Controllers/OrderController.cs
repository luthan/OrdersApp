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
        public ActionResult Add()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
            ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
            return View(new Order() { OrderDate = DateTime.Today, Items = new List<Item>() { new Item() } });
        }

        public ActionResult Edit(int? id)
        {
            Order order = db.Orders.Include(i => i.Items).Where(i => i.Id == id).Single();

            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
            ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Order order, string btnSubmit, string userName)
        {
            if (btnSubmit == "addRow") 
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                
                order.Items.Add(new Item());

                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                return View(order);
            } 
            else if (btnSubmit == "generatePO")
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                order.PoNumber = getNewPO();

                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                return View(order);
            } 
            else
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
                    ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                    return View(order);
                }
            }   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order, string btnSubmit, string userName)
        {
            if (btnSubmit == "addRow")
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                order.Items.Add(new Item() { OrderId = order.Id });

                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                return View(order);
            }
            else if (btnSubmit == "generatePO")
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                order.PoNumber = getNewPO();

                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                return View(order);
            }
            else
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
                    order.UserName = userName;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.CategoryName), "Id", "CategoryName");
                    ViewBag.BillingCategoryId = new SelectList(db.BillingCategories.OrderBy(x => x.BillingCategoryName), "Id", "BillingCategoryName");
                    return View(order);
                }
            }
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

        private string getNewPO()
        {
            string po = db.Orders.OrderByDescending(x => x.PoNumber).Where(x => x.PoNumber != null).Select(x => x.PoNumber).ToList().First();
            int lastPO = Convert.ToInt32(po.Substring(po.Length - 3, 3));
            if (DateTime.Now.Year.ToString() == po.Substring(po.Length - 7, 4))
            {
                po = "po" + po.Substring(po.Length - 7, 4) + (lastPO + 1).ToString("000");
            }
            else
            {
                po = "po" + DateTime.Now.Year.ToString() + "001";
            }
            return po;
        }

        public ActionResult DashBoard()
        {

            double total = 0;
            var year = DateTime.Now.Year;
            var items = db.Items.Where(m => m.Order.OrderDate.Year == year && m.BillingCategory.BillingCategoryName.Contains("Hardware")).ToList();
            //var items = db.Items.Where(m => m.BillingCategory.BillingCategoryName.Contains("/Hardware/") && m.Order.OrderDate.Year == year).ToList();
            foreach (var item in items)
            {
                total += item.Quantity * item.Price;
            }
            return Content(total.ToString());
        }
    }
}
