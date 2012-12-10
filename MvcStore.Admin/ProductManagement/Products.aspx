<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Products" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<mvc:MvcStoreMasterDetailsView EntityTypeName="Product" runat="server" />	
</asp:Content>
