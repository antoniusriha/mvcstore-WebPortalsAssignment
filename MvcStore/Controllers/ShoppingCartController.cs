//
// ShoppingCartController.cs
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
using System.Linq;
using System.Web.Mvc;
using MvcStore.Models;
using MvcStore.ViewModels;

namespace MvcStore.Controllers
{
	public class ShoppingCartController : Controller
	{
		readonly IRepository<Cart> cartRepo;
		readonly IRepository<Product> productRepo;
		readonly IRepository<CartItem> cartItemRepo;

		public ShoppingCartController (IRepository<Cart> cartRepo,
		                               IRepository<Product> productRepo,
		                               IRepository<CartItem> cartItemRepo)
		{
			if (cartItemRepo == null)
				throw new ArgumentNullException ("cartItemRepo");
			this.cartItemRepo = cartItemRepo;
			if (productRepo == null)
				throw new ArgumentNullException ("productRepo");
			this.productRepo = productRepo;
			if (cartRepo == null)
				throw new ArgumentNullException ("cartRepo");
			this.cartRepo = cartRepo;
		}
		
		//
		// GET: /ShoppingCart/
		public ActionResult Index ()
		{
			var cart = cartRepo.GetCart (HttpContext);
			
			// Set up our ViewModel
			var viewModel = new ShoppingCartViewModel
			{
				CartItems = cart.Items,
				CartTotal = cart.GetTotal ()
			};
			// Return the view
			return View (viewModel);
		}

		//
		// GET: /Store/AddToCart/5
		public ActionResult AddToCart (int id)
		{
			// Retrieve the product from the database
			var product = productRepo.GetItemById (id);
			
			// Add it to the shopping cart
			var cart = cartRepo.GetCart (HttpContext);
			cartRepo.AddToCart (cart, product, cartItemRepo);
			
			// Go back to the main store page for more shopping
			return RedirectToAction ("Index");
		}

		//
		// AJAX: /ShoppingCart/RemoveFromCart/5
		[HttpPost]
		public ActionResult RemoveFromCart(int id)
		{
			var cart = cartRepo.GetCart (HttpContext);
			var cartItem = cart.Items.Single (i => i.Id == id);
			var productName = cartItem.Product.Name;
			
			// Remove from cart
			int itemCount = 0;
			if (cartItem.Count > 1) {
				itemCount = --cartItem.Count;
				cartItemRepo.UpdateItem (cartItem);
			} else
				cartItemRepo.DeleteItem (cartItem);
			
			// Display the confirmation message
			var results = new ShoppingCartRemoveViewModel
			{
				Message = Server.HtmlEncode(productName) +
				" has been removed from your shopping cart.",
				CartTotal = cart.GetTotal(),
				CartCount = cart.Items.Count,
				ItemCount = itemCount,
				DeleteId = id
			};
			return Json (results);
		}

		//
		// GET: /ShoppingCart/CartSummary
		[ChildActionOnly]
		public ActionResult CartSummary ()
		{
			var cart = cartRepo.GetCart (HttpContext);
			ViewData ["CartCount"] = cart.Items.Count;
			return PartialView ("CartSummary");
		}
	}
}
