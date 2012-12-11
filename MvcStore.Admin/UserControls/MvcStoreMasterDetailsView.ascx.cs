//
// MvcStoreMasterDetailsView.ascx.cs
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
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MvcStore.Models;

namespace MvcStore.Admin.UserControls
{
	public partial class MvcStoreMasterDetailsView : UserControl
	{
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
		}
		
		public string EntityTypeName {
			get { return entityTypeName; }
			set {
				if (value == entityTypeName)
					return;
				
				entityTypeName = value;
				HeaderText.InnerText = value + " - Management";
				var dotn = "MvcStore.Models." + value;
				var tn = "MvcStore.Admin.RepositoryProxy`1[[MvcStore.Models."
					+ value + ",libmvcstore]]";
				MasterDataSource.DataObjectTypeName = dotn;
				MasterDataSource.TypeName = tn;
				DetailDataSource.DataObjectTypeName = dotn;
				DetailDataSource.TypeName = tn;
				
				GenerateViewFields (Type.GetType (dotn + ",libmvcstore"));
			}
		}
		
		void GenerateViewFields (Type entity)
		{
			if (!typeof(ModelBase).IsAssignableFrom (entity))
				throw new ArgumentException ("entity must be a ModelBase.");
			
			// first, a cmd field
			var cfMaster = new CommandField {
				ShowDeleteButton = true,
				ShowEditButton = true
			};
			MasterView.Columns.Add (cfMaster);
			
			var f1 = CreateField ("Id", true);
			DetailView.Fields.Add (f1);
			
			var col1 = CreateField ("Id", true);
			MasterView.Columns.Add (col1);
			
			// do reflection stuff
			var properties = entity.GetProperties ();
			
			// if there is a "Name" property, make it the next col/field.
			if (properties.Any (e => e.Name == "Name")) {
				var f2 = CreateField ("Name");
				DetailView.Fields.Add (f2);
				var col2 = CreateField ("Name");
				MasterView.Columns.Add (col2);
			}
			
			// all other cols/fields
			foreach (var item in properties) {
				// skip name and id
				if (item.Name == "Name" || item.Name == "Id")
					continue;
				
				// no set types besides string
				if (item.PropertyType != typeof (string) &&
					typeof (IEnumerable).IsAssignableFrom (item.PropertyType))
					continue;
				
				if (typeof (ModelBase).IsAssignableFrom (item.PropertyType)) {
					var site = EntitySiteMappingConfiguration.Instance [item.PropertyType];
					var hlc = new HyperLinkField {
						DataTextField = item.Name,
						NavigateUrl = site.Url,
						HeaderText = item.Name
					};
					MasterView.Columns.Add (hlc);
					
					var hlf = new HyperLinkField {
						DataTextField = item.Name,
						NavigateUrl = site.Url,
						HeaderText = item.Name
					};
					DetailView.Fields.Add (hlf);
					
					continue;
				}
				
				var f = CreateField (item.Name, !item.CanWrite);
				DetailView.Fields.Add (f);
				var c = CreateField (item.Name, !item.CanWrite);
				MasterView.Columns.Add (c);
			}
			
			var cfDetail = new CommandField {
				ShowDeleteButton = true,
				ShowEditButton = true,
				ShowInsertButton = true
			};
			DetailView.Fields.Add (cfDetail);
			
			var cfSelect = new CommandField { ShowSelectButton = true };
			MasterView.Columns.Add (cfSelect);
		}
		
		BoundField CreateField (string dataField, bool readOnly = false)
		{
			return new BoundField {
				DataField = dataField,
				HeaderText = dataField,
				SortExpression = dataField,
				ReadOnly = readOnly,
				InsertVisible = !readOnly
			};
		}
		
		protected void InsertNewItemButton_Click (object sender, EventArgs e)
		{
			DetailView.ChangeMode (DetailsViewMode.Insert);
		}
		
		string entityTypeName;
	}
}
