//
// When_a_category_is_added_to_a_new_store.cs
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
using MvcStore.Backend.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace MvcStore.Test.Model
{
	[TestFixture()]
	public class When_a_category_is_added_to_a_new_store
	{
		[SetUp()]
		public void Init ()
		{
			// Arrange
			var mockRepo = new Mock<IStoreRepository> ();
			var mockCartRepo = new Mock<ICartRepository> ();
			category = new Category ("Cat1");
			mockRepo.Setup (s => s.AddCategory (category));
			mockRepo.SetupGet (r => r.Categories).Returns (new List<Category> { category });
			store = new Store (mockRepo.Object, mockCartRepo.Object);

			// Act
			store.AddCategory (category);
		}

		[Test()]
		public void the_store_should_contain_the_category ()
		{
			// Assert
			store.Categories.Should ().Contain (category);
		}

		[Test()]
		public void there_should_be_only_one_category_in_the_store ()
		{
			// Assert
			store.Categories.Count.Should ().Be (1);
		}

		Category category;
		Store store;
	}
}
