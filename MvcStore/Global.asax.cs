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
using Mono.Data.Sqlite;
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
		static readonly string DbFile = Path.Combine ("App_Data", "store.db");

		internal static Store Store { get; private set; }

		static ISessionFactory sessionFactory;

		protected void Application_Start ()
		{
			// Setup database: Create NHibernate session factory
			sessionFactory = CreateSessionFactory();

			// Init db with dummy data
			LoadDummyData (sessionFactory);

			// Setup ASP.NET membership schema
			SetupAspNetMembershipProviderDb ();

			// Setup store
			Store = new Store (new SqliteStoreRepository (sessionFactory),
			                   new SqliteShoppingCartRepository (sessionFactory));

			AreaRegistration.RegisterAllAreas ();
			RegisterRoutes (RouteTable.Routes);
		}

		protected void Application_BeginRequest (object sender, EventArgs e)
		{
			// serious hack
			if (Context.Request.Url.LocalPath.StartsWith ("/Account"))
				return;

			var session = sessionFactory.OpenSession ();
			session.BeginTransaction ();
			ManagedWebSessionContext.Bind (HttpContext.Current, session);
		}
		
		protected void Application_EndRequest (object sender, EventArgs e)
		{
			// serious hack
			if (Context.Request.Url.LocalPath.StartsWith ("/Account"))
				return;

			var session = ManagedWebSessionContext.Unbind (HttpContext.Current, sessionFactory);
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

		static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Store", action = "Index", id = "" }
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
				.Conventions.Add<CascadeConvention>()
				.Override<CartItem> (c => c.HasOne (x => x.Product).Cascade.None ())
				;
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

		static void SetupAspNetMembershipProviderDb ()
		{
			connection = new SqliteConnection ("URI=file:" + Path.Combine (System.Environment.CurrentDirectory, DbFile));
			connection.Open ();
			CreateTables ();
			connection.Close ();
		}
		
		static void CreateTables ()
		{
			ExecuteScalar (@"CREATE TABLE [aspnet_Applications] (
				[ApplicationId] TEXT PRIMARY KEY UNIQUE NOT NULL,
				[ApplicationName] TEXT UNIQUE NOT NULL,
				[Description] TEXT NULL
			)");
			
			ExecuteScalar (@"CREATE TABLE [aspnet_Roles] (
				[RoleId] TEXT PRIMARY KEY UNIQUE NOT NULL,
				[RoleName] TEXT NOT NULL,
				[LoweredRoleName] TEXT NOT NULL,
				[ApplicationId] TEXT NOT NULL
			)");
			
			ExecuteScalar (@"CREATE TABLE [aspnet_UsersInRoles] (
				[UserId] TEXT NOT NULL,
				[RoleId] TEXT NOT NULL
			)");
			
			ExecuteScalar (@"CREATE TABLE [aspnet_Profile] (
				[UserId] TEXT UNIQUE NOT NULL,
				[LastUpdatedDate] TIMESTAMP NOT NULL,
				[PropertyNames] TEXT NOT NULL,
				[PropertyValuesString] TEXT NOT NULL,
				[PropertyValuesBinary] BLOB NOT NULL
			)");
			
			ExecuteScalar (@"CREATE TABLE [aspnet_Users] (
				[UserId] TEXT PRIMARY KEY UNIQUE NOT NULL,
				[Username] TEXT NOT NULL,
				[LoweredUsername] TEXT NOT NULL,
				[ApplicationId] TEXT NOT NULL,
				[Email] TEXT NULL,
				[LoweredEmail] TEXT NULL,
				[Comment] TEXT NULL,
				[Password] TEXT NOT NULL,
				[PasswordFormat] TEXT NOT NULL,
				[PasswordSalt] TEXT NOT NULL,
				[PasswordQuestion] TEXT NULL,
				[PasswordAnswer] TEXT NULL,
				[IsApproved] BOOL NOT NULL,
				[IsAnonymous] BOOL  NOT NULL,
				[LastActivityDate] TIMESTAMP  NOT NULL,
				[LastLoginDate] TIMESTAMP NOT NULL,
				[LastPasswordChangedDate] TIMESTAMP NOT NULL,
				[CreateDate] TIMESTAMP  NOT NULL,
				[IsLockedOut] BOOL NOT NULL,
				[LastLockoutDate] TIMESTAMP NOT NULL,
				[FailedPasswordAttemptCount] INTEGER NOT NULL,
				[FailedPasswordAttemptWindowStart] TIMESTAMP NOT NULL,
				[FailedPasswordAnswerAttemptCount] INTEGER NOT NULL,
				[FailedPasswordAnswerAttemptWindowStart] TIMESTAMP NOT NULL
			)");
			
			ExecuteScalar (@"CREATE UNIQUE INDEX idxUsers ON [aspnet_Users] ( 'LoweredUsername' , 'ApplicationId' )");
			ExecuteScalar (@"CREATE INDEX idxUsersAppId ON [aspnet_Users] ( 'ApplicationId' )");
			ExecuteScalar (@"CREATE UNIQUE INDEX idxRoles ON [aspnet_Roles] ( 'LoweredRoleName' , 'ApplicationId' )");
			ExecuteScalar (@"CREATE UNIQUE INDEX idxUsersInRoles ON [aspnet_UsersInRoles] ( 'UserId', 'RoleId')");
			
			// Add initial Membership provider info and Administrator role
			ExecuteScalar (@"INSERT INTO [aspnet_Applications] ([ApplicationId], [ApplicationName], [Description])
				values ('9fe59163-682b-45f8-b5f6-d95100cebedc', 'SQLite ASP.NET Provider', ''
			)");
			
			ExecuteScalar (@"INSERT INTO [aspnet_Roles] ([RoleId], [RoleName], [LoweredRoleName], [ApplicationId])
				values ('f514a999-e7e8-49d3-a260-c9c08504064e', 'Administrator',
				'administrator', '9fe59163-682b-45f8-b5f6-d95100cebedc'
			)");
			
			ExecuteScalar (@"insert into aspnet_users ('UserId', 'Username', 'LoweredUsername', 'ApplicationId',
				'Email', 'LoweredEmail', 'Comment', 'Password', 'PasswordFormat', 'PasswordSalt', 'PasswordQuestion',
				'PasswordAnswer', 'IsApproved', 'IsAnonymous', 'LastActivityDate', 'LastLoginDate',
				'LastPasswordChangedDate', 'CreateDate', 'IsLockedOut', 'LastLockoutDate', 'FailedPasswordAttemptCount',
				'FailedPasswordAttemptWindowStart', 'FailedPasswordAnswerAttemptCount', 'FailedPasswordAnswerAttemptWindowStart')
				values ('937c1a49-aa14-43b9-b3c9-3f1172e8f6a8', 'Administrator', 'administrator', '9fe59163-682b-45f8-b5f6-d95100cebedc',
				'antoniusriha@gmail.com', 'antoniusriha@gmail.com', NULL, 'password123!', 'Clear', 'QxMZMspPsC1n7dumr0ILAA==', NULL, NULL,
				'1', '0', '2012-11-08 10:09:50.1625854', '2012-11-08 10:09:50.1625854', '2012-11-08 10:09:50.0738366', '2012-11-08 10:09:50.0738366',
				'0', '1753-01-01 00:00:00', '0', '1753-01-01 00:00:00', '0', '1753-01-01 00:00:00'
			)");
			
			ExecuteScalar (@"insert into aspnet_usersinroles ('UserId', 'RoleId') values ('937c1a49-aa14-43b9-b3c9-3f1172e8f6a8', 'f514a999-e7e8-49d3-a260-c9c08504064e')");
		}
		
		static object ExecuteScalar (string command)
		{
			object resultset;
			SqliteCommand cmd = connection.CreateCommand ();
			cmd.CommandText = command;
			resultset = cmd.ExecuteScalar ();
			return resultset;
		}
		
		static SqliteConnection connection;
	}
}
