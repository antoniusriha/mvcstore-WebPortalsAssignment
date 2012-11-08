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
            return View (store.Categories);
        }
		
		public ActionResult Browse (string category)
		{
			return View (store.Categories.Single (c => c.Name == category));
		}
		
		public ActionResult Details (int id)
		{
			return View (store.GetProduct (id));
		}

		Store store = MvcApplication.Store;
    }
}
