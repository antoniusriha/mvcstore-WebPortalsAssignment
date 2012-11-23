<%@ Page Language="C#" Inherits="MvcStore.Admin.BasicReporting.SimpleDisplay" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="Northwind+ProductsRow"
        DeleteMethod="DeleteProduct" InsertMethod="AddProduct" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetProducts" TypeName="ProductsBLL" UpdateMethod="UpdateProduct">
        <DeleteParameters>
            <asp:Parameter Name="productID" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
</asp:Content>
