//
// RepositoryProxy.cs
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
using System.Linq;
using MvcStore.Models;
using MvcStore.DataAccess;

namespace MvcStore.Admin
{
	public class RepositoryProxy<T> where T : ModelBase
	{
		readonly IRepository<T> repo;
		
		public RepositoryProxy ()
		{
			// This is bad, but I'll do it anyway
			repo = new NHibernateRepository<T> ();
		}
		
		public IEnumerable GetItems ()
		{
			return repo.GetItems ().OrderBy (i => i.Id);
		}
		
		public IEnumerable GetItemById (int id)
		{
			var item = repo.GetItemById (id);
			if (item != null)
				return new object [] { item };
			return new object [] {};
		}
		
		public void AddItem (T item)
		{
			repo.AddItem (item);
		}
		
		public void UpdateItem (T item)
		{
			repo.UpdateItem (item);
		}
		
		public void DeleteItem (T item)
		{
			repo.DeleteItem (item);
		}
	}
}
