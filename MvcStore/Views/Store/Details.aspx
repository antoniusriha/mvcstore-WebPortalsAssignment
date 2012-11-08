<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_Layout.master" Inherits="System.Web.Mvc.ViewPage<MvcStore.Models.Product>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Details</title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Product: <%: Model.Name %></h2>
	<p>Description: <%: Model.Description %></p>
	<p>Price: Euro <%: Model.Price %></p>
</asp:Content>
