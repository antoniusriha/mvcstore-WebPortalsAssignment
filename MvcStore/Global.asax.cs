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
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using MvcStore.DataAccess;
using MvcStore.Models;

namespace MvcStore
{
	public class MvcApplication : NinjectHttpApplication
	{
		internal static Store Store { get; private set; }

		protected override void OnApplicationStarted ()
		{
			var connectionString = ConfigurationManager.ConnectionStrings ["AspSQLProvider"];
			MvcStoreApplication.InitDb (connectionString.ConnectionString);
			
			AreaRegistration.RegisterAllAreas ();
			RegisterRoutes (RouteTable.Routes);
		}

		protected void Application_BeginRequest (object sender, EventArgs e)
		{
			MvcStoreApplication.OpenSession (HttpContext.Current);
		}
		
		protected void Application_EndRequest (object sender, EventArgs e)
		{
			MvcStoreApplication.CloseSession (HttpContext.Current);
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

		protected override IKernel CreateKernel ()
		{
			var kernel = new StandardKernel ();
			kernel.Bind (typeof (IRepository<>)).To (typeof (NHibernateRepository<>));
			kernel.Load (Assembly.GetExecutingAssembly ());
			return kernel;
		}
	}
}
