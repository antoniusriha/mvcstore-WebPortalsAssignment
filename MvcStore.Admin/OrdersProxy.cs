//
// OrdersProxy.cs
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
using System.Collections;

namespace MvcStore.Admin
{
	public class OrdersProxy
	{
		public IEnumerable GetOrders ()
		{
			return Global.OrderRepository.Orders;
		}
		
		public IEnumerable GetOrderById (int id)
		{
			var order = Global.OrderRepository.GetOrder (id);
			if (order == null)
			return new object [] {};
			else
			return new object [] { order };
		}
		
		public void InsertOrder (string name, string description, decimal price)
		{
			var product = new Product (name) {
				Description = description,
				Price = price
			};
			Global.Store.AddProduct (product);
		}
		
		public void DeleteProduct (int id)
		{
			var product = Global.Store.GetProduct (id);
			if (product != null)
				Global.Store.RemoveProduct (product);
		}
		
		public void UpdateProduct (int id, string name, string description, decimal price)
		{
			var product = Global.Store.GetProduct (id);
			if (product == null)
				return;
			
			product.SetName (name);
			product.Description = description;
			product.Price = price;
			Global.Store.UpdateProduct (product);
		}
	}
}
