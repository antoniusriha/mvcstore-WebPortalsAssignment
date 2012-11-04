using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStore.Models;

namespace MvcStore.Controllers
{
    public class StoreController : Controller
    {
        public ActionResult Index ()
        {
			var cats = new List<Category> {
				new Category ("Cat 1"),
				new Category ("Cat 2"),
				new Category ("Cat 3")
			};
            return View (cats);
        }
		
		public ActionResult Browse (string category)
		{
			var cat = new Category ("Category 1");
			return View (cat);
		}
		
		public ActionResult Details (int id)
		{
			var product = new Product ("Handschuh");
			return View (product);
		}
    }
}
