//
// Main.cs
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
using MvcStore;

class Program
{
	const string DbName = "mvcstore";
	
	static void Main (string[] args)
	{
		Console.WriteLine ("Getting configuration ...");
		var connectionString
			= ConfigurationManager.ConnectionStrings ["AspSQLProvider"].ToString ();
		Console.WriteLine ("ConnectionString: " + connectionString);
		
		// Delete and recreate db
		Console.WriteLine ("Tying to delete database ...");
		var pDropDb = Process.Start ("/usr/bin/dropdb", "mvcstore");
		pDropDb.WaitForExit ();
		
		Console.WriteLine ();
		Console.WriteLine ("Trying to create database ...");
		var pCreateDb = Process.Start ("/usr/bin/createdb", "mvcstore");
		pCreateDb.WaitForExit ();
		Console.WriteLine ();
		
		// setup store schema
		Console.WriteLine ("Setting up store schema ...");
		MvcStoreApplication.InitDb (connectionString, true);
		Console.WriteLine ();
		
		// loading dummy data into store schema
		Console.WriteLine ("Loading dummy data into store schema ...");
		var pLoadData = Process.Start ("/usr/bin/psql", DbName + " -f populatedb.sql");
		pLoadData.WaitForExit ();
		Console.WriteLine ();
		
		// setup membership schema
		Console.WriteLine ("Setting up membership schema ...");
		var pExeMemSchema = Process.Start ("/usr/bin/psql",
			                               DbName + " -f DatabaseSchema.sql");
		pExeMemSchema.WaitForExit ();
		Console.WriteLine ();
		
		// setup membership admin role and user
		Console.WriteLine ("Setting up membership admin ...");
		var pExeMemAdmin = Process.Start ("/usr/bin/psql",
		                                  DbName + " -f MembershipRolesSetup.sql");
		pExeMemAdmin.WaitForExit ();
		Console.WriteLine ();
		
		Console.WriteLine ("Complete.");
	}
}
