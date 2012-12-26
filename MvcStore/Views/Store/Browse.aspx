<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage<MvcStore.Models.Category>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Browse</title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Browsing Category: <%: Model.Name %></h2>
	<ul id="album-list">
    	<% foreach (var product in Model.Products) { %>
        	<li>
				<a href='<%= Url.Action ("Details", new { id = product.Id }) %>' >
					<img alt="<%= product.Name %>" width="100" height="100"
						 src='<%= Url.Action ("ShowProductImage", "Store", new { productId = product.Id }) %>' />
					<span><%= product.Name %></span>
				</a>
        	</li>
    	<% } %>
	</ul>
</asp:Content>
