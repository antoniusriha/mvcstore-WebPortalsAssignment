<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Categories" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<mvc:MvcStoreMasterDetailsView EntityTypeName="Category" runat="server" />
</asp:Content>
