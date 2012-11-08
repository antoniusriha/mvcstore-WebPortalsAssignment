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
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MvcStore.Models;
using MvcStore.DataAccess;

namespace MvcStore
{
	public class MvcApplication : HttpApplication
	{
		const string DbFile = "store.db";

		internal static Store Store { get; private set; }

		static ISessionFactory sessionFactory;

		protected void Application_Start ()
		{
			// Setup database: Create NHibernate session factory
			sessionFactory = CreateSessionFactory();

			// Init db with dummy data
			LoadDummyData (sessionFactory);

			// Setup store
			Store = new Store (new SqliteStoreRepository (sessionFactory));

			AreaRegistration.RegisterAllAreas ();
			RegisterRoutes (RouteTable.Routes);
		}

		protected void Application_BeginRequest (object sender, EventArgs e)
		{
			var session = sessionFactory.OpenSession ();
			session.BeginTransaction ();
			ManagedWebSessionContext.Bind (HttpContext.Current, session);
		}
		
		protected void Application_EndRequest (object sender, EventArgs e)
		{
			var session = ManagedWebSessionContext.Unbind (HttpContext.Current, sessionFactory);
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

		static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" }
			);
		}

#if DEBUG
		public
#endif
		static void LoadDummyData (ISessionFactory sessionFactory)
		{
			using (var session = sessionFactory.OpenSession()) {
				// populate the database
				using (var transaction = session.BeginTransaction()) {
					var food = new Category ("Food");

					var prod = new Product ("Potatoes") { Price = 3.60m };
					prod.SetCategory (food);
					prod = new Product ("Fish") { Price = 4.49m };
					prod.SetCategory (food);
					prod = new Product ("Milk") { Price = 0.79m };
					prod.SetCategory (food);
					prod = new Product ("Bread") { Price = 1.29m };
					prod.SetCategory (food);
					prod = new Product ("Cheese") { Price = 2.10m };
					prod.SetCategory (food);
					prod = new Product ("Waffles") { Price = 2.41m };
					prod.SetCategory (food);
					
					// save category, this saves everything else via cascading
					session.SaveOrUpdate (food);
					
					transaction.Commit();
				}
			}
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

			// This is for session management via HttpContext (ASP.NET sessions)
			config.SetProperty ("current_session_context_class", "managed_web");
			
			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config).Create(false, true);
		}
	}
}
