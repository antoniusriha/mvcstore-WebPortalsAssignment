//
// When_the_database_is_created_and_dummy_data_is_loaded_into_the_db.cs
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
using NUnit.Framework;
using FluentAssertions;
using NHibernate;
using NHibernate.Persister.Entity;
using MvcStore.Backend;
using MvcStore.Backend.Models;

namespace MvcStore.Test
{
	[TestFixture()]
	public class When_the_database_is_created_and_dummy_data_is_loaded_into_the_db
	{
		[SetUp()]
		public void Init ()
		{
			MvcStoreApplication.InitDb ("store.db", "users.db");
			sessionFactory = MvcStoreApplication.CreateSessionFactory ();
			MvcStoreApplication.LoadDummyData (sessionFactory);
		}

		[Test()]
		public void the_db_should_contain_a_product_table ()
		{
			var metaData = sessionFactory.GetClassMetadata (typeof (Product)) as AbstractEntityPersister;
			metaData.Should ().NotBeNull ();
			metaData.Name.Should ().Be ("MvcStore.Models.Product");
		}

		[Test()]
		public void the_product_table_should_contain_items ()
		{
			using (var session = sessionFactory.OpenSession())
			{
				using (session.BeginTransaction())
				{
					var products = session.CreateCriteria (typeof (Product)).List<Product> ();
					products.Should ().NotBeNull ();
					products.Count.Should ().BeGreaterThan (0);
				}
			}
		}

		[Test()]
		public void the_category_table_should_contain_items ()
		{
			using (var session = sessionFactory.OpenSession())
			{
				using (session.BeginTransaction())
				{
					var cats = session.CreateCriteria (typeof (Category)).List<Category> ();
					cats.Should ().NotBeNull ();
					cats.Count.Should ().BeGreaterThan (0);
				}
			}
		}

		ISessionFactory sessionFactory;
	}
}
