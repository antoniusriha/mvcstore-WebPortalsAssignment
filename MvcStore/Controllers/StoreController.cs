using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcStore.Controllers
{
    public class StoreController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }
		
		public ActionResult Browse()
		{
			return View ();
		}
		
		public ActionResult Details()
		{
			return View ();
		}
    }
}
