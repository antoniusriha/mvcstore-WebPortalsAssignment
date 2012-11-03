<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_Layout.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcStore.Models.Category>>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Store</title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<h3>Browse Categories</h3>
	<p>Select from <%: Model.Count () %> categories:</p>
	<ul>
		<% foreach (var cat in Model) { %>
			<li><%: Html.ActionLink (cat.Name, "Browse", new { category = cat.Name }) %></li>
		<% } %>
	</ul>
</asp:Content>
