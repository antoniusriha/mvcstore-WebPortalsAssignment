//
// SqliteProductRepository.cs
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
using MvcStore.Backend.Models;

namespace MvcStore.Backend.DataAccess
{
	public class NHibernateStoreRepository : IStoreRepository
	{
		public NHibernateStoreRepository ()
		{
			sessionHelper = new SessionHelper (MvcStoreApplication.SessionFactory);
		}

		public IList<Category> Categories {
			get {
				var sd = sessionHelper.GetSessionData ();
				try {
					return sd.Session.CreateCriteria (typeof (Category)).List<Category> ();
				} finally {
					sessionHelper.CommitIfNecessary (sd);
				}
			}
		}
		
		public IList<Product> Products {
			get {
				var sd = sessionHelper.GetSessionData ();
				try {
					return sd.Session.CreateCriteria (typeof (Product)).List<Product> ();
				} finally {
					sessionHelper.CommitIfNecessary (sd);
				}
			}
		}

		public void AddCategory (Category category)
		{
			if (category == null)
				throw new ArgumentNullException ("category");

			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (category);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public bool RemoveCategory (Category category)
		{
			if (category == null)
				return false;

			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.Delete (category);
			} catch {
				return false;
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
			return true;
		}

		public void AddProduct (Product product)
		{
			if (product == null)
				throw new ArgumentNullException ("product");

			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (product);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public bool RemoveProduct (Product product)
		{
			if (product == null)
				return false;

			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.Delete (product);
			} catch {
				return false;
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
			return true;
		}

		public Product GetProduct (int id)
		{
			Product product = null;
			var sd = sessionHelper.GetSessionData ();
			try {
				product = sd.Session.Get<Product> (id);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
			return product;
		}

		SessionHelper sessionHelper;
	}
}
