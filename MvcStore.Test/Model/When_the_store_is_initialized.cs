//
// When_the_store_is_initialized.cs
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
using NUnit.Framework;
using FluentAssertions;
using Moq;
using MvcStore.Backend.Models;

namespace MvcStore.Test
{
	[TestFixture()]
	public class When_the_store_is_initialized
	{
		[SetUp()]
		public void Init ()
		{
			var mockRepo = new Mock<IStoreRepository> ();
			var mockCartRepo = new Mock<ICartRepository> ();
			mockRepo.SetupGet (s => s.Categories).Returns (new List<Category> { new Category ("Misc") });
			store = new Store (mockRepo.Object, mockCartRepo.Object);
		}

		[Test()]
		public void the_store_must_contain_the_misc_category ()
		{
			store.Categories.Should ().Contain (c => c.Name == "Misc");
		}

		Store store;
	}
}
