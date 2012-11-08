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
using NHibernate;
using MvcStore.Models;

namespace MvcStore.DataAccess
{
	public class SqliteStoreRepository : IStoreRepository
	{
		public SqliteStoreRepository (ISessionFactory sessionFactory)
		{
			if (sessionFactory == null)
				throw new ArgumentNullException ("sessionFactory");
			this.sessionFactory = sessionFactory;
		}

		public IList<Category> Categories {
			get {
				var sd = GetSessionData ();
				try {
					return sd.Session.CreateCriteria (typeof (Category)).List<Category> ();
				} finally {
					CommitIfNecessary (sd);
				}
			}
		}
		
		public IList<Product> Products {
			get {
				var sd = GetSessionData ();
				try {
					return sd.Session.CreateCriteria (typeof (Product)).List<Product> ();
				} finally {
					CommitIfNecessary (sd);
				}
			}
		}

		public void AddCategory (Category category)
		{
			if (category == null)
				throw new ArgumentNullException ("category");

			var sd = GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (category);
			} finally {
				CommitIfNecessary (sd);
			}
		}

		public bool RemoveCategory (Category category)
		{
			if (category == null)
				return false;

			var sd = GetSessionData ();
			try {
				sd.Session.Delete (category);
			} catch {
				return false;
			} finally {
				CommitIfNecessary (sd);
			}
			return true;
		}

		public void AddProduct (Product product)
		{
			if (product == null)
				throw new ArgumentNullException ("product");

			var sd = GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (product);
			} finally {
				CommitIfNecessary (sd);
			}
		}

		public bool RemoveProduct (Product product)
		{
			if (product == null)
				return false;

			var sd = GetSessionData ();
			try {
				sd.Session.Delete (product);
			} catch {
				return false;
			} finally {
				CommitIfNecessary (sd);
			}
			return true;
		}

		public Product GetProduct (int id)
		{
			Product product = null;
			var sd = GetSessionData ();
			try {
				product = sd.Session.Get<Product> (id);
			} finally {
				CommitIfNecessary (sd);
			}
			return product;
		}
		
		void CommitIfNecessary (SessionData sessionData)
		{
			if (sessionData.IsManaged)
				return;

			var session = sessionData.Session;
			if (session != null) {
				try {
					session.Transaction.Commit ();
				} catch {
					session.Transaction.Rollback ();
				} finally {
					session.Flush ();
					if (session.IsOpen)
						session.Dispose ();
				}
			}
		}
		
		SessionData GetSessionData ()
		{
			ISession session;
			bool managed;
			try {
				session = sessionFactory.GetCurrentSession ();
				managed = true;
			} catch {
				session = sessionFactory.OpenSession ();
				session.BeginTransaction ();
				managed = false;
			}
			return new SessionData (session, managed);
		}

		ISessionFactory sessionFactory;
	}
}
