<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<p>
		Type to find your product:
		<input id="SearchBox" data-bind="value: searchTerm, valueUpdate:'afterkeydown'" />
	</p>
	<ul data-bind="foreach: searchResults">
		<li>
			<a data-bind="attr: { href: hrefUrl }" ><span data-bind="text: productName"></span></a>
		</li>
	</ul>
	<script type="text/javascript">
		var SearchResult = function (id, productName) {
			this.hrefUrl = '/Store/Details/' + id;
			this.productName = productName;
		}
		
		function AppViewModel () {
			this.searchTerm = ko.observable ();
			this.searchResults = ko.observableArray ([]);
		}
		
		var vm = new AppViewModel ();

		// Activates knockout.js
		ko.applyBindings (vm);
		
		var flushRequest = true;
		$("#SearchBox").keypress (function () {
			flushRequest = true;
		});
		
		window.setInterval(function () {
			if (flushRequest) {
				flushRequest = false;
				var text = $("#SearchBox").val ();
				if (text != '') {
					$.post ("/Store/GetMatchingProducts", { "searchTerm": text }, function (data) {
						vm.searchResults.removeAll ();
						for (var i = 0; i < data.length; i++) {
							vm.searchResults.push (new SearchResult (data [i].Id, data [i].ProductName));
						}
					});
				}
			}
		}, 1000);
</script>
</asp:Content>
