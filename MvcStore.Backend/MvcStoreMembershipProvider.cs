//
// MvcStoreMembershipProvider.cs
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
using Mono.Data.Sqlite;

namespace MvcStore.Backend
{
	public static class MvcStoreMembershipProvider
	{
		public static void Setup (string connectionString)
		{
			// create schema users
			
			
			
			connection = new SqliteConnection ("URI=file:" + usersDbFile);
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
