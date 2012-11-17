//
// ShoppingCart.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcStore.Backend.Models
{
    public class ShoppingCart
    {
		public const string CartSessionKey = "CartId";
		
		public ShoppingCart (HttpContextBase context, IShoppingCartRepository repo)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			if (repo == null)
				throw new ArgumentNullException ("repo");
			cartRepo = repo;

			// get cart; if non-existent, create cart
			var cartId = GetCartId (context);
			cart = cartRepo.Carts.SingleOrDefault (c => c.Name == cartId);
			if (cart == null) {
				cart = new Cart (cartId);
				cartRepo.AddCart (cart);
			}
		}

        public void AddToCart (Product product)
		{
			// Get the matching cart item and album instances
			var cartItem = cart.Items.SingleOrDefault (i => i.Product == product);

			if (cartItem == null) {
				// Create a new cart item if no cart item exists
				cartItem = new CartItem (product) { Count = 1 };
				cartItem.SetCart (cart);
				cartRepo.AddCartItem (cartItem);
			} else {
				cartItem.Count++;
				cartRepo.UpdateCartItem (cartItem);
			}
        }

        public int RemoveFromCart (int id)
        {
            // Get the cart item
			var cartItem = cartRepo.GetCartItem (id);

            int itemCount = 0;
            if (cartItem != null) {
                if (cartItem.Count > 1) {
                    itemCount = --cartItem.Count;
					cartRepo.UpdateCartItem (cartItem);
                } else
					cartRepo.RemoveCartItem (cartItem);
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            foreach (var cartItem in cart.Items)
				cartRepo.RemoveCartItem (cartItem);
        }

        public IList<CartItem> GetCartItems ()
        {
			return cart.Items;
        }

        public int GetCount()
        {
			return cart.Items.Count;
        }

        public decimal GetTotal()
        {
			return cart.Items.Select (c => c.Count * c.Product.Price).Sum ();
        }

        public int CreateOrder (Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems) {
                var orderDetail = new OrderDetail (item.Product) { Quantity = item.Count };
				orderDetail.SetOrder (order);

                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Product.Price);
            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
			cartRepo.AddOrder (order);

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return order.Id;
        }
		
		public Order GetOrder (int id)
		{
			return cartRepo.GetOrder (id);
		}

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId (HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null) {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name)) {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                } else {
                    // Generate a new random GUID using System.Guid class
                    var tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart (string userName)
		{
			// check if user already has a cart
			var userCart = cartRepo.Carts.SingleOrDefault (c => c.Name == userName);
			// if user already has a cart
			if (userCart != null) {
				// migrate items
				var oldCart = cart;
				cart = userCart;
				foreach (var item in oldCart.Items) {
					for (int i = 0; i < item.Count; i++)
						AddToCart (item.Product);
				}
				
				// delete anonymous cart
				cartRepo.RemoveCart (oldCart);
			} else {
				// just rename the cart
				cart.SetName (userName);
				cartRepo.UpdateCart (cart);
			}
        }

		Cart cart;
		IShoppingCartRepository cartRepo;
    }
}