//
// Global.asax.cs
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MvcStore.Models;

namespace MvcStore
{
	public class MvcApplication : HttpApplication
	{
		const string DbFile = "store.db";

		protected void Application_Start ()
		{
			AreaRegistration.RegisterAllAreas ();
			RegisterRoutes (RouteTable.Routes);

			// Setup database

			// create our NHibernate session factory
			var sessionFactory = CreateSessionFactory();
			
//			using (var session = sessionFactory.OpenSession())
//			{
//				// populate the database
//				using (var transaction = session.BeginTransaction())
//				{
//					// create a couple of Stores each with some Products and Employees
//					var barginBasin = new Store { Name = "Bargin Basin" };
//					var superMart = new Store { Name = "SuperMart" };
//					
//					var potatoes = new Product { Name = "Potatoes", Price = 3.60 };
//					var fish = new Product { Name = "Fish", Price = 4.49 };
//					var milk = new Product { Name = "Milk", Price = 0.79 };
//					var bread = new Product { Name = "Bread", Price = 1.29 };
//					var cheese = new Product { Name = "Cheese", Price = 2.10 };
//					var waffles = new Product { Name = "Waffles", Price = 2.41 };
//					
//					var daisy = new Employee { FirstName = "Daisy", LastName = "Harrison" };
//					var jack = new Employee { FirstName = "Jack", LastName = "Torrance" };
//					var sue = new Employee { FirstName = "Sue", LastName = "Walkters" };
//					var bill = new Employee { FirstName = "Bill", LastName = "Taft" };
//					var joan = new Employee { FirstName = "Joan", LastName = "Pope" };
//					
//					// add products to the stores, there's some crossover in the products in each
//					// store, because the store-product relationship is many-to-many
//					AddProductsToStore(barginBasin, potatoes, fish, milk, bread, cheese);
//					AddProductsToStore(superMart, bread, cheese, waffles);
//					
//					// add employees to the stores, this relationship is a one-to-many, so one
//					// employee can only work at one store at a time
//					AddEmployeesToStore(barginBasin, daisy, jack, sue);
//					AddEmployeesToStore(superMart, bill, joan);
//					
//					// save both stores, this saves everything else via cascading
//					session.SaveOrUpdate(barginBasin);
//					session.SaveOrUpdate(superMart);
//					
//					transaction.Commit();
//				}
//			}
		}

		static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" }
			);
			routes.MapRoute ("Category", "Category", new {
				controller = "Home", action = "Category", id = "" }
			);
		}

#if DEBUG
		public
#endif
		static ISessionFactory CreateSessionFactory()
		{
			return Fluently.Configure()
				.Database(SQLiteConfiguration.Standard
				          .Driver<MonoSqliteDriver>()
				          .UsingFile(DbFile))
					.Mappings(m => m.AutoMappings.Add(CreateAutomappings))
					.ExposeConfiguration(BuildSchema)
					.BuildSessionFactory();
		}

		static AutoPersistenceModel CreateAutomappings()
		{
			// This is the actual automapping - use AutoMap to start automapping,
			// then pick one of the static methods to specify what to map (in this case
			// all the classes in the assembly that contains BaseModel), and then either
			// use the Setup and Where methods to restrict that behaviour, or (preferably)
			// supply a configuration instance of your definition to control the automapper.
			return AutoMap.AssemblyOf<BaseModel>(new StoreAutomappingConfiguration())
				.IgnoreBase<BaseModel> ()
				.Conventions.Add<CascadeConvention>();
		}

		static void BuildSchema(Configuration config)
		{
			// delete the existing db on each run
			if (File.Exists(DbFile))
				File.Delete(DbFile);
			
			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config).Create(false, true);
		}
	}
}
