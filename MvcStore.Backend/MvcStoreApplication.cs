//
// MvcStoreApplication.cs
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
using System.Web;
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
	public class MvcStoreApplication
	{
		internal static SessionHelper SessionHelper { get; private set; }

		public static void InitDb (string connectionString, bool exportSchema = false)
		{
			MvcStoreApplication.connectionString = connectionString;
			MvcStoreApplication.exportSchema = exportSchema;
			
			// Setup database: Create NHibernate session factory
			sessionFactory = CreateSessionFactory ();
			SessionHelper = new SessionHelper (sessionFactory);
			
			if (!exportSchema)
				return;
			
			// Init db with dummy data
			LoadDummyData (sessionFactory);
		}
		
		public static void OpenSession (HttpContext context)
		{
			var session = sessionFactory.OpenSession ();
			session.BeginTransaction ();
			ManagedWebSessionContext.Bind (context, session);
		}
		
		public static void CloseSession (HttpContext context)
		{
			var session = ManagedWebSessionContext.Unbind (context, sessionFactory);
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
		
#if DEBUG
		public
#endif
		static void LoadDummyData (ISessionFactory sessionFactory)
		{
			using (var session = sessionFactory.OpenSession ()) {
				// populate the database
				using (var transaction = session.BeginTransaction ()) {
					var food = new Category { Name = "Food" };
					
					var prod = new Product () { Name = "Potatoes", Price = 3.60m };
					prod.SetCategory (food);
					prod = new Product () { Name = "Fish", Price = 4.49m };
					prod.SetCategory (food);
					prod = new Product () { Name = "Milk", Price = 0.79m };
					prod.SetCategory (food);
					prod = new Product () { Name = "Bread", Price = 1.29m };
					prod.SetCategory (food);
					prod = new Product () { Name = "Cheese", Price = 2.10m };
					prod.SetCategory (food);
					prod = new Product () { Name = "Waffles", Price = 2.41m };
					prod.SetCategory (food);
					
					// save category, this saves everything else via cascading
					session.SaveOrUpdate (food);
					
					transaction.Commit ();
				}
			}
		}
		
#if DEBUG
		public
#endif
		static ISessionFactory CreateSessionFactory ()
		{
			return Fluently.Configure ()
				.Database (PostgreSQLConfiguration.Standard
				           .ConnectionString (connectionString))
					.Mappings (m => m.AutoMappings.Add (CreateAutomappings))
					.ExposeConfiguration (BuildSchema)
					.BuildSessionFactory ();
		}
		
		static AutoPersistenceModel CreateAutomappings ()
		{
			// This is the actual automapping - use AutoMap to start automapping,
			// then pick one of the static methods to specify what to map (in this case
			// all the classes in the assembly that contains BaseModel), and then either
			// use the Setup and Where methods to restrict that behaviour, or (preferably)
			// supply a configuration instance of your definition to control the automapper.
			return AutoMap.AssemblyOf<ModelBase> (new StoreAutomappingConfiguration ())
				.IgnoreBase<ModelBase> ()
				.Conventions.Add<CascadeConvention> ()
				.Override<CartItem> (map => map.References (x => x.Product).Cascade.None ())
				.Override<Category> (map => map.HasMany (x => x.Products).Cascade.SaveUpdate ());
		}
		
		static void BuildSchema (Configuration config)
		{
			// This is for session management via HttpContext (ASP.NET sessions)
			config.SetProperty ("current_session_context_class", "managed_web");
			
			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			if (exportSchema)
				new SchemaExport (config).Create (false, true);
		}

		static ISessionFactory sessionFactory;
		static string connectionString;
		static bool exportSchema;
	}
}

