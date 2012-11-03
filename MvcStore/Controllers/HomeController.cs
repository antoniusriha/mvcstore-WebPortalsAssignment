using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcStore.Models;

namespace MvcStore.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			var homeVm = new HomeViewModel () { WelcomeMessage = "Welcome to ASP.NET MVC on Mono!" };
			return View (homeVm);
		}
		
		public ActionResult Category ()
		{
//			var cat = new Category () { Name = "Cat" };
//			cat.Products.Add (new Product () { Name = "P1" });
//			cat.Products.Add (new Product () { Name = "P2" });
			
			return View ();
		}
	}
}
