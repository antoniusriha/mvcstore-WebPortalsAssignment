//
// SessionHelper.cs
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
using NHibernate;

namespace MvcStore.DataAccess
{
	public class SessionHelper
	{
		public SessionHelper (ISessionFactory sessionFactory)
		{
			if (sessionFactory == null)
				throw new ArgumentNullException ("sessionFactory");
			this.sessionFactory = sessionFactory;
		}

		public void CommitIfNecessary (SessionData sessionData)
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
					if (session.IsOpen) {
						session.Flush ();
						session.Dispose ();
					}
				}
			}
		}
		
		public SessionData GetSessionData ()
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
