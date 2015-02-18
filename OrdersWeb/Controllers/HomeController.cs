using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersWeb.Models;

namespace OrdersWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Order newOrder = new Order()
            {
                Items = new List<Item>(){
                    new Item(){Id=1,ItemDescription = "Test"},
                    new Item(){Id=2,ItemDescription = "Test2"}
                }
            };
            return View(newOrder);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}