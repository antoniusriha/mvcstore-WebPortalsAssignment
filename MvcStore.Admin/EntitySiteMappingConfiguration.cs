//
// EntitySiteMappingConfiguration.cs
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
using System.Web;
using MvcStore.Models;

namespace MvcStore.Admin
{
	public class EntitySiteMappingConfiguration
	{
		public static EntitySiteMappingConfiguration Instance {
			get {
				return instance ?? (instance = new EntitySiteMappingConfiguration ());
			}
		}
		
		public static EntitySiteMappingConfiguration Configure ()
		{
			return Instance;
		}
		
		static EntitySiteMappingConfiguration instance;
		
		EntitySiteMappingConfiguration ()
		{
			dict = new Dictionary<Type, SiteMapNode> ();
		}
		
		public SiteMapNode this [Type type] {
			get { return dict [type].Clone (); }
		}
		
		public MapPart Map (Type entity)
		{
			return new MapPart (this, entity);
		}
		
		public class MapPart
		{
			public MapPart (EntitySiteMappingConfiguration conf, Type entity)
			{
				if (conf == null)
					throw new ArgumentNullException ("conf");
				if (entity == null)
					throw new ArgumentNullException ("entity");
				if (!typeof (ModelBase).IsAssignableFrom (entity))
					throw new ArgumentException ("entity must be derived from ModeBase.");
				this.conf = conf;
				this.entity = entity;
			}
			
			public EntitySiteMappingConfiguration To (SiteMapNode site)
			{
				if (site == null)
					throw new ArgumentNullException ("site");
				conf.dict.Add (entity, site);
				return conf;
			}
			
			EntitySiteMappingConfiguration conf;
			Type entity;
		}
		
		Dictionary<Type, SiteMapNode> dict;
	}
}
