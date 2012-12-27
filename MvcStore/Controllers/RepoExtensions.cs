//
// CartHelper.cs
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
using System.Web;
using MvcStore.Models;

namespace MvcStore.Controllers
{
	static class RepoExtensions
	{
		internal static readonly string CartSessionKey = "CartId";
		
		internal static void AddToCart (this IRepository<Cart> cartRepo,
		                                Cart cart, Product product,
		                                IRepository<CartItem> cartItemRepo)
		{
			var cartItem = cart.Items.SingleOrDefault (i => i.Product == product);
			if (cartItem == null) {
				// Create a new cart item if no cart item exists
				cartItem = new CartItem { Product = product, Count = 1 };
				cartItem.SetCart (cart);
				cartRepo.UpdateItem (cart); // one call should suffice
			} else {
				cartItem.Count++;
				cartItemRepo.UpdateItem (cartItem);
			}
		}
		
		internal static int CreateOrder (this IRepository<Order> orderRepo, Cart cart,
		                                 Order order, IRepository<CartItem> cartItemRepo)
		{
			decimal orderTotal = 0;
			var cartItems = cart.Items;
			// Iterate over the items in the cart, adding the order details for each
			foreach (var item in cartItems) {
				var orderDetail = new OrderDetail {
					Product = item.Product,
					Quantity = item.Count
				};
				orderDetail.SetOrder (order);
				
				// Set the order total of the shopping cart
				orderTotal += (item.Count * item.Product.Price);
			}
			
			// Set the order's total to the orderTotal count
			order.Total = orderTotal;
			
			// Save the order
			orderRepo.AddItem (order);
			
			// Empty the shopping cart
			foreach (var cartItem in cartItems)
				cartItemRepo.DeleteItem (cartItem);
			
			// Return the OrderId as the confirmation number
			return order.Id;
		}
		
		internal static Cart GetCart (this IRepository<Cart> cartRepo,
		                              HttpContextBase context)
		{
			var cartId = GetCartId (context);
			var cart = cartRepo.GetItems ().SingleOrDefault (c => c.Owner == cartId);
			if (cart == null) {
				cart = new Cart { Owner = cartId };
				cartRepo.AddItem (cart);
			}
			return cart;
		}
		
		// We're using HttpContextBase to allow access to cookies.
		static string GetCartId (HttpContextBase context)
		{
			if (context.Session [CartSessionKey] == null) {
				if (!string.IsNullOrWhiteSpace (context.User.Identity.Name)) {
					context.Session [CartSessionKey] = context.User.Identity.Name;
				} else {
					// Generate a new random GUID using System.Guid class
					var tempCartId = Guid.NewGuid ();
					
					// Send tempCartId back to client as a cookie
					context.Session [CartSessionKey] = tempCartId.ToString ();
				}
			}
			
			return context.Session [CartSessionKey].ToString ();
		}
	}
}
