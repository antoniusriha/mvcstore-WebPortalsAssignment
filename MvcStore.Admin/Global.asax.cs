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
using System.Web;
using MvcStore.Models;

namespace MvcStore.Admin
{
	public class Global : HttpApplication
	{
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
			var connectionString = ConfigurationManager.ConnectionStrings ["AspSQLProvider"];
			MvcStoreApplication.InitDb (connectionString.ConnectionString);
			
			var root = SiteMap.RootNode;
			EntitySiteMappingConfiguration.Configure ()
				.Map<Category> ().To (root.Find ("Category"))
				.Map<Product> ().To (root.Find ("Product"))
				.Map<Order> ().To (root.Find ("Order"))
				.Map<OrderDetail> ().To (root.Find ("Category"))
				.Map<Cart> ().To (root.Find ("Product"))
				.Map<CartItem> ().To (root.Find ("Order"));
		}
		
		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
			MvcStoreApplication.OpenSession (HttpContext.Current);
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
			MvcStoreApplication.CloseSession (HttpContext.Current);
		}
		
		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}
