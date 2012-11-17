using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStore.Backend.Models;

namespace MvcStore.Controllers
{
	[Authorize]
    public class CheckoutController : Controller
    {
		const string PromoCode = "FREE";
		
		public ActionResult AddressAndPayment()
		{
			return View();
		}
		
		//
		// POST: /Checkout/AddressAndPayment
		
		[HttpPost]
		public ActionResult AddressAndPayment(FormCollection values)
		{
			var order = new Order (User.Identity.Name);
			TryUpdateModel(order);
			
			try
			{
				if (string.Equals(values["PromoCode"], PromoCode,
					StringComparison.OrdinalIgnoreCase) == false)
				{
					return View(order);
				}
				else
				{
					order.Username = User.Identity.Name;
					
					var cart = store.GetCart (this);
					cart.CreateOrder (order);
					
					return RedirectToAction("Complete", new { id = order.Id });
				}
				
			}
			catch
			{
				//Invalid - redisplay with errors
				return View(order);
			}
		}
		
		//
		// GET: /Checkout/Complete
		
		public ActionResult Complete(int id)
		{
			// Validate customer owns this order
			var cart = store.GetCart (this);
			var order = cart.GetOrder (id);
			bool isValid = order.Username == User.Identity.Name;
			
			if (isValid)
			{
				return View(id);
			}
			else
			{
				return View("Error");
			}
		}
		
		Store store = MvcApplication.Store;
    }
}
