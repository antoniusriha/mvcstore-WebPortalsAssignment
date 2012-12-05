//
// CheckoutController.cs
//
// Author:
//       Antonius Riha <antoniusriha@gmail.com>
//
// Copyright (c) 2012 Antonius Riha
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Web.Mvc;
using MvcStore.Models;

namespace MvcStore.Controllers
{
	[Authorize]
    public class CheckoutController : Controller
    {
		const string PromoCode = "FREE";

		readonly IRepository<Cart> cartRepo;
		readonly IRepository<Order> orderRepo;
		readonly IRepository<CartItem> cartItemRepo;
		
		public CheckoutController (IRepository<Cart> cartRepo,
		                           IRepository<Order> orderRepo,
		                           IRepository<CartItem> cartItemRepo)
		{
			if (cartItemRepo == null)
				throw new ArgumentNullException ("cartItemRepo");
			this.cartItemRepo = cartItemRepo;
			if (orderRepo == null)
				throw new ArgumentNullException ("orderRepo");
			this.orderRepo = orderRepo;
			if (cartRepo == null)
				throw new ArgumentNullException ("cartRepo");
			this.cartRepo = cartRepo;
		}
		
		public ActionResult AddressAndPayment()
		{
			return View();
		}
		
		//
		// POST: /Checkout/AddressAndPayment
		
		[HttpPost]
		public ActionResult AddressAndPayment (FormCollection values)
		{
			var order = new Order { Username = User.Identity.Name };
			TryUpdateModel (order);
			
			try {
				if (string.Equals (values ["PromoCode"], PromoCode,
					StringComparison.OrdinalIgnoreCase) == false) {
					return View (order);
				} else {
					order.Username = User.Identity.Name;
					
					var cart = cartRepo.GetCart (HttpContext);
					orderRepo.CreateOrder (cart, order, cartItemRepo);
					
					return RedirectToAction ("Complete", new { id = order.Id });
				}
				
			} catch {
				//Invalid - redisplay with errors
				return View (order);
			}
		}
		
		//
		// GET: /Checkout/Complete
		
		public ActionResult Complete (int id)
		{
			// Validate customer owns this order
			var order = orderRepo.GetItemById (id);
			bool isValid = order.Username == User.Identity.Name;
			
			if (isValid) {
				return View (id);
			} else {
				return View ("Error");
			}
		}
    }
}
