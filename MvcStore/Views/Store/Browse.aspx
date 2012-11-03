<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_Layout.master" Inherits="System.Web.Mvc.ViewPage<MvcStore.Models.Category>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Browse</title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Browsing Category: <%: Model.Name %></h2>
</asp:Content>
