//
// NHibernateRepository.cs
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
using System.Collections.Generic;
using NHibernate;
using MvcStore.Models;

namespace MvcStore.DataAccess
{
	public class NHibernateRepository<T> : IRepository<T> where T : ModelBase
	{
		public IList<T> GetItems ()
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				return sd.Session.CreateCriteria (typeof (T)).List<T> ();
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public T GetItemById (int id)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				return sd.Session.Get<T> (id);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void AddItem (T item)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (item);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void DeleteItem (T item)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				// if Delete fails because the object instance somehow
				// got dissociated (Is that a word?), try merge and
				// delete merged entity.
				try {
					sd.Session.Delete (item);
				} catch (NonUniqueObjectException) {
					var mergedEntity = sd.Session.Merge (item);
					sd.Session.Delete (mergedEntity);
				}
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void UpdateItem (T item)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				// Use SaveOrUpdate by default; try merge if that fails
				// because the object instance somehow got dissociated.
				// (Is that a word?)
				try {
					sd.Session.SaveOrUpdate (item);
				} catch (NonUniqueObjectException) {
					sd.Session.Merge (item);
				}
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		SessionHelper sessionHelper = MvcStoreApplication.SessionHelper;
	}
}
