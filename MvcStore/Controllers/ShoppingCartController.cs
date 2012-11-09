using System;
using System.Web.Mvc;
using MvcStore.Models;
using MvcStore.ViewModels;

namespace MvcStore.Controllers
{
	public class ShoppingCartController : Controller
	{
		//
		// GET: /ShoppingCart/
		public ActionResult Index()
		{
			var cart = store.GetCart(HttpContext);
			
			// Set up our ViewModel
			var viewModel = new ShoppingCartViewModel
			{
				CartItems = cart.GetCartItems(),
				CartTotal = cart.GetTotal()
			};
			// Return the view
			return View(viewModel);
		}

		//
		// GET: /Store/AddToCart/5
		public ActionResult AddToCart(int id)
		{
			// Retrieve the product from the database
			var addedProduct = store.GetProduct (id);
			
			// Add it to the shopping cart
			var cart = store.GetCart(HttpContext);
			cart.AddToCart(addedProduct);
			
			// Go back to the main store page for more shopping
			return RedirectToAction("Index");
		}

		//
		// AJAX: /ShoppingCart/RemoveFromCart/5
		[HttpPost]
		public ActionResult RemoveFromCart(int id)
		{
			// Remove the item from the cart
			var cart = store.GetCart(HttpContext);
			
			// Get the name of the album to display confirmation
			var productName = store.GetProduct (id).Name;
			
			// Remove from cart
			int itemCount = cart.RemoveFromCart(id);
			
			// Display the confirmation message
			var results = new ShoppingCartRemoveViewModel
			{
				Message = Server.HtmlEncode(productName) +
				" has been removed from your shopping cart.",
				CartTotal = cart.GetTotal(),
				CartCount = cart.GetCount(),
				ItemCount = itemCount,
				DeleteId = id
			};
			return Json (results);
		}

		//
		// GET: /ShoppingCart/CartSummary
		[ChildActionOnly]
		public ActionResult CartSummary()
		{
			var cart = store.GetCart(HttpContext);
			
			ViewData["CartCount"] = cart.GetCount();
			return PartialView("CartSummary");
		}
		
		Store store = MvcApplication.Store;
	}
}
