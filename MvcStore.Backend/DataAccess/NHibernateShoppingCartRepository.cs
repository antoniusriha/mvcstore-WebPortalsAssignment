//
// SqliteShoppingCartRepository.cs
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
using System.Collections.Generic;
using NHibernate;
using MvcStore.Backend.Models;

namespace MvcStore.Backend.DataAccess
{
	public class NHibernateShoppingCartRepository : IShoppingCartRepository
	{
		public NHibernateShoppingCartRepository ()
		{
			sessionHelper = new SessionHelper (MvcStoreApplication.SessionFactory);
		}

		public IList<Cart> Carts {
			get {
				var sd = sessionHelper.GetSessionData ();
				try {
					return sd.Session.CreateCriteria (typeof (Cart)).List<Cart> ();
				} finally {
					sessionHelper.CommitIfNecessary (sd);
				}
			}
		}

		public void AddCart (Cart cart)
		{
			if (cart == null)
				throw new ArgumentNullException ("cart");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (cart);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void RemoveCart (Cart cart)
		{
			if (cart == null)
				throw new ArgumentNullException ("cart");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.Delete (cart);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void UpdateCart (Cart cart)
		{
			if (cart == null)
				throw new ArgumentNullException ("cart");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (cart);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public CartItem GetCartItem (int id)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				return sd.Session.Get<CartItem> (id);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void AddCartItem (CartItem item)
		{
			if (item == null)
				throw new ArgumentNullException ("item");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (item);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void RemoveCartItem (CartItem item)
		{
			if (item == null)
				throw new ArgumentNullException ("item");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.Delete (item);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void UpdateCartItem (CartItem item)
		{
			if (item == null)
				throw new ArgumentNullException ("item");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (item);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public Order GetOrder (int id)
		{
			var sd = sessionHelper.GetSessionData ();
			try {
				return sd.Session.Get<Order> (id);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void AddOrder (Order order)
		{
			if (order == null)
				throw new ArgumentNullException ("order");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.SaveOrUpdate (order);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		public void RemoveOrder (Order order)
		{
			if (order == null)
				throw new ArgumentNullException ("order");
			var sd = sessionHelper.GetSessionData ();
			try {
				sd.Session.Delete (order);
			} finally {
				sessionHelper.CommitIfNecessary (sd);
			}
		}

		SessionHelper sessionHelper;
	}
}
