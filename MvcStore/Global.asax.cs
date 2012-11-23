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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcStore.Backend;
using MvcStore.Backend.Models;
using System.Configuration;

namespace MvcStore
{
	public class MvcApplication : HttpApplication
	{
		internal static Store Store { get; private set; }

		protected void Application_Start ()
		{
			var connectionString = ConfigurationManager.ConnectionStrings ["AspSQLProvider"];
			MvcStoreApplication.InitDb (connectionString.ConnectionString);
			
			// Setup store
			Store = MvcStoreApplication.GetStore ();
			
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
	}
}
