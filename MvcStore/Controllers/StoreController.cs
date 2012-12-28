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
		readonly IRepository<Category> categoryRepo;
		readonly IRepository<Product> productRepo;
		
		public StoreController (IRepository<Category> categoryRepo,
		                        IRepository<Product> productRepo)
		{
			if (productRepo == null)
				throw new ArgumentNullException ("productRepo");
			if (categoryRepo == null)
				throw new ArgumentNullException ("categoryRepo");
			this.productRepo = productRepo;
			this.categoryRepo = categoryRepo;
		}

        public ActionResult Index ()
        {
            return View (categoryRepo.GetItems ());
        }
		
		public ActionResult Browse (string category)
		{
			return View (categoryRepo.GetItems ().Single (c => c.Name == category));
		}
		
		public ActionResult Details (int id)
		{
			return View (productRepo.GetItemById (id));
		}

		public ActionResult ShowProductImage (int productId)
		{
			var p = productRepo.GetItemById (productId);
			return File (p.Image, "image/png");
		}

		public ActionResult Search ()
		{
			return View ();
		}

		[HttpPost]
		public ActionResult GetMatchingProducts (string searchTerm)
		{
			if (string.IsNullOrWhiteSpace (searchTerm))
				return Json (new Dictionary<string, string> ());

			searchTerm = searchTerm.ToLower ().Trim ();
			var products = productRepo.GetItems ()
				.Where (p => p.Name.ToLower ().Contains (searchTerm))
				.Take (20)
				.Select (p => new { Id = p.Id, ProductName = p.Name });
			return Json (products);
		}
		
		//
		// GET: /Store/GenreMenu
		[ChildActionOnly]
		public ActionResult CategoryMenu ()
		{
			return PartialView (categoryRepo.GetItems ());
		}
    }
}
