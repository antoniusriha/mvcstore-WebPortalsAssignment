//
// StoreProxy.cs
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
using System.Collections;
using MvcStore.Backend.Models;

namespace MvcStore.Admin
{
	public class StoreProxy
	{
		public IEnumerable GetCategories ()
		{
			var cats = Global.Store.Categories;
			return cats;
		}
		
		public Category GetCategoryById (int id)
		{
			return Global.Store.GetCategory (id);
		}
		
		public void InsertCategory (string name, string description)
		{
			var cat = new Category (name);
			cat.Description = description;
			Global.Store.AddCategory (cat);
		}
		
		public void DeleteCategory (int id)
		{
			var cat = Global.Store.GetCategory (id);
			if (cat != null)
				Global.Store.RemoveCategory (cat);
		}
		
		public void UpdateCategory (int id, string name, string description)
		{
			var cat = Global.Store.GetCategory (id);
			if (cat == null)
				return;
			
			cat.SetName (name);
			cat.Description = description;
			Global.Store.UpdateCategory (cat);
		}
	}
}