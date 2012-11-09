<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage<int>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Checkout Complete</title>	
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
<h2>Checkout Complete</h2>
<p>Thanks for your order! Your order number is: <%: Model %></p>
<p>How about shopping for some more products in our 
    <%: Html.ActionLink("store", "Index", "Store") %>
</p>
</asp:Content>	