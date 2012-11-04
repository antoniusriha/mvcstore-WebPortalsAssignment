//
// StoreFixture.cs
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
using System.Linq;
using NUnit.Framework;
using Moq;
using MvcStore.Models;
using FluentAssertions;

namespace MvcStore.Test.Model
{
	[TestFixture()]
	public class When_a_category_is_added_to_the_store
	{
		[SetUp()]
		public void Init ()
		{
			// Arrange
			var mockStore = new Mock<IStore> ();
			store = mockStore.Object;
			category = new Category ("Cat1");
			mockStore.Setup (s => s.AddCategory (category));
			mockStore.Setup (s => s.Categories.Contains (category)).Returns (true);

			// Act
			mockStore.Object.AddCategory (category);
		}

		[Test()]
		public void the_store_should_return_the_category ()
		{
			// Assert
			store.Categories.First ().Should ().Be (category);
		}

		IStore store;
		Category category;
	}
}
