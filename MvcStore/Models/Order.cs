//
// Order.cs
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

namespace MvcStore.Models
{
	public class Order : BaseModel
	{
		public Order (string name) : base (name)
		{
			OrderDate = DateTime.Now;
		}

		protected Order () {}

		public virtual string Username { get; set; }

		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual string Address { get; set; }

		public virtual string City { get; set; }

		public virtual string State { get; set; }

		public virtual string PostalCode { get; set; }

		public virtual string Country { get; set; }

		public virtual string Phone { get; set; }

		public virtual string Email { get; set; }

		public virtual decimal Total { get; set; }

		public virtual DateTime OrderDate { get; protected set; }

		/// <summary>
		/// Gets the order details. CAUTION. Don't use this property to add order details to
		/// the order! Use the OderDetail.SetOrder (Order) method instead.
		/// </summary>
		/// <value>
		/// The order details.
		/// </value>
		public virtual IList<OrderDetail> OrderDetails {
			get { return orderDetails ?? (orderDetails = new List<OrderDetail> ()); }
		}

		List<OrderDetail> orderDetails;
	}
}
