//
// BaseModel.cs
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
using System.ComponentModel.DataAnnotations;

namespace MvcStore.Models
{
	public abstract class ModelBase : IEquatable<ModelBase>
	{
		[ScaffoldColumn(false)]
		public virtual int Id { get; set; }
		
		[ScaffoldColumn(false)]
		public virtual string Description { get; set; }

		public virtual bool Equals (ModelBase other)
		{
			if (other == null)
				return false;
			
			if (GetType () != other.GetType ())
				return false;
			
			return Id == other.Id;
		}
		
		public override bool Equals (object obj)
		{
			var m = obj as ModelBase;
			if (!Equals (m, null))
				return Equals (m);
			return false;
		}
		
		public override int GetHashCode ()
		{
			return GetType ().GetHashCode () ^ Id;
		}
		
		public static bool operator == (ModelBase x, ModelBase y)
		{
			if (!Equals (x, null))
				return x.Equals (y);
			return Equals (y, null);
		}
		
		public static bool operator != (ModelBase x, ModelBase y)
		{
			return !(x == y);
		}
		
		public override string ToString ()
		{
			var type = GetType ();
			var name = type.Name + ": " + Id;
			var nameProperty = type.GetProperty ("Name");
			if (nameProperty != null && nameProperty.PropertyType == typeof (string))
				name = (string)nameProperty.GetValue (this, new object [] {});
			return name;
		}
	}
}
