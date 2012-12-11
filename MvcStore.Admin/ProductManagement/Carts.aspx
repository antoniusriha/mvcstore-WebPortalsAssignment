<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Carts" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<mvc:MvcStoreMasterDetailsView EntityTypeName="Cart" runat="server" />	
</asp:Content>
