<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Orders" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<mvc:MvcStoreMasterDetailsView EntityTypeName="Order" runat="server" />
</asp:Content>
