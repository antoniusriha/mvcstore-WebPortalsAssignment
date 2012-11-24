//
// Store.cs
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
using System.Web.Mvc;

namespace MvcStore.Backend.Models
{
	public class Store
	{
		public Store (IStoreRepository repo, IShoppingCartRepository cartRepo)
		{
			if (cartRepo == null)
				throw new ArgumentNullException ("cartRepo");
			if (repo == null)
				throw new ArgumentNullException ("repo");
			
			this.repo = repo;
			this.cartRepo = cartRepo;

			// Setup category Misc, if not already there
			var misc = Categories.SingleOrDefault (c => c.Name == "Misc");
			if (misc == null) {
				misc = new Category ("Misc");
				AddCategory (misc);
			}
		}

		public IList<Category> Categories {
			get { return repo.Categories; }
		}

		public void AddProduct (Product product)
		{
			if (product == null)
				throw new ArgumentNullException ("product");

			var misc = repo.Categories.Single (c => c.Name == "Misc");
			if (product.Category == null)
				product.SetCategory (misc);

			repo.AddProduct (product);
		}

		public bool RemoveProduct (Product product)
		{
			if (product == null)
				return false;
			return repo.RemoveProduct (product);
		}
		
		public void UpdateProduct (Product product)
		{
			if (product == null)
				throw new ArgumentNullException ("category");
			repo.UpdateProduct (product);
		}

		public Product GetProduct (int id)
		{
			return repo.GetProduct (id);
		}

		public void AddCategory (Category category)
		{
			if (category == null)
				throw new ArgumentNullException ("category");
			repo.AddCategory (category);
		}

		public bool RemoveCategory (Category category)
		{
			var misc = repo.Categories.Single (c => c.Name == "Misc");
			if (category == null || category.Id == misc.Id)
				return false;

			// transfer all products of the category being deleted to misc
			foreach (var item in category.Products)
				item.SetCategory (misc);
			repo.UpdateCategory (misc);
			
			return repo.RemoveCategory (category);
		}
		
		public void UpdateCategory (Category category)
		{
			if (category == null)
				throw new ArgumentNullException ("category");
			
			var misc = repo.Categories.Single (c => c.Name == "Misc");
			if (category == misc)
				return;
			repo.UpdateCategory (category);
		}
		
		public Category GetCategory (int id)
		{
			return repo.Getcategory (id);
		}

		public ShoppingCart GetCart (HttpContextBase context)
		{
			return new ShoppingCart (context, cartRepo);
		}

		public ShoppingCart GetCart (Controller controller)
		{
			return new ShoppingCart (controller.HttpContext, cartRepo);
		}

		IStoreRepository repo;
		IShoppingCartRepository cartRepo;
	}
}
