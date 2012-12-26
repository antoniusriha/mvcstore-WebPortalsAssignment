MvcStore
========

Version 0.0.2
-------------
Verwendete Technologien/Tools:
	* ASP.NET MVC 2 (aspx engine)
	* mono 2.10.8.1 (== .NET 4.0)
	* Ubuntu Precise 12.04 LTS (Entwicklungs- und Deployment-OS)
	* Apache web server 2.2.22
	* PostgreSQL 9.1.7
	* PostgreSQL ASP.NET Membership Provider von Nauck.IT
	http://dev.nauck-it.de/projects/show/aspsqlprovider
	* Fluent NHibernate 1.3 (mit Automapping)
	* MonoDevelop 3.1.0 (latest)
	* NUnit 2.6
	* Moq 4.0
	* FluentAssertions 2.0
	* Linode zum Hosten


+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


Version 0.0.1
-------------

Verwendete Technologien/Tools:
	* ASP.NET MVC 2 (aspx engine)
	* mono 2.10.8.1 (== .NET 4.0)
	* Ubuntu Precise 12.04 LTS (Entwicklungs- und Deployment-OS)
	* Apache web server 2.2.22
	* SQLite 3
	* SQLite ASP.NET Membership Provider von
	http://www.codeproject.com/Articles/29199/SQLite-Membership-Role-and-Profile-Providers
	(Wurde auf die Verwendung mit mono angepasst)
	* Fluent NHibernate 1.3 (mit Automapping)
	* MonoDevelop 3.1.0 (latest)
	* NUnit 2.6
	* Moq 4.0
	* FluentAssertions 2.0
	* Linode zum Hosten

Recherche - Quellen:
	* .NET Referenz (msdn)
	* Vergleich aspx engine - Razor: http://bit.ly/RNdhHB
	* Diverse Anleitungen auf http://www.mono-project.com (v.a. mod_mono)
	* Gute blog posts für ASP.NET deployment/development mit mono:
	http://www.integratedwebsystems.com/
	* MVC Music Store Tutorial auf http://www.asp.net/mvc
	* Fluent NHibernate Tutorial:
	https://github.com/jagregory/fluent-nhibernate/wiki/Auto-mapping
	* Moq Quickstart: http://code.google.com/p/moq/wiki/QuickStart
	* Intro in TDD: http://stephenwalther.com/archive/2008/06/12/tdd-introduction-to-moq.aspx
	* Google + Stackoverflow
	* und noch einige weiter Webseiten

Arbeitszeit: ~30h

Besondere Schwierigkeiten:
	* Fluent NHibernate (keine bisherigen Erfahrungen damit, weder NHibernate noch
	Fluent NHibernate)
	* ASP.NET MVC (keine bisherigen Erfahrungen damit)
	* Architektur im Model/Data Access Layer, IOC-Container (keine bisherigen Erfahrungen
	damit)
	* Verständnis für Notwendigkeit des Mockens nicht so leicht zu erlangen. Folgender
	Erfahrungsbericht zeigt die Schwierigkeiten (für mich) nachvollziehbar:
	http://odetocode.com/blogs/scott/archive/2005/02/16/the-5-stages-of-mocking.aspx
