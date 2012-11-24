<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Orders" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
    <asp:ObjectDataSource ID="OrdersGridDataSource" runat="server"
        SelectMethod="GetOrders"
		DeleteMethod="DeleteProducts" 
		UpdateMethod="UpdateProducts"
		TypeName="MvcStore.Admin.StoreProxy" >
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
		<UpdateParameters>
			<asp:Parameter Name="id" Type="Int32" />
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
			<asp:Parameter Name="price" Type="Decimal" />
		</UpdateParameters>
	</asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ProductDetailsDataSource" runat="server"
        SelectMethod="GetProductById"
		InsertMethod="InsertProduct"
		DeleteMethod="DeleteProducts" 
		UpdateMethod="UpdateProducts"
		TypeName="MvcStore.Admin.StoreProxy" >
		<DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
		<InsertParameters>
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
			<asp:Parameter Name="price" Type="Decimal" />
		</InsertParameters>
		<UpdateParameters>
			<asp:Parameter Name="id" Type="Int32" />
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
			<asp:Parameter Name="price" Type="Decimal" />
		</UpdateParameters>
		<SelectParameters>
			<asp:ControlParameter ControlID="ProductsGridView" Name="id"
				PropertyName="SelectedValue" Type="Int32" />
		</SelectParameters>
	</asp:ObjectDataSource>
    <br />
	<p>
        <asp:DetailsView ID="ProductDetailsView" runat="server" AutoGenerateRows="False"
			DataKeyNames="Id" DataSourceID="ProductDetailsDataSource" EnableViewState="False"
			AllowPaging="True">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id"
					SortExpression="Id" ReadOnly="True" InsertVisible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name"
					SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description"
					SortExpression="Description" />
				<asp:BoundField DataField="Price" HeaderText="Price"
					SortExpression="Price" />
				<asp:CommandField ShowDeleteButton="True" ShowEditButton="True"
					ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        &nbsp;
	</p>
    <h3>Categories</h3>
    <p>
		Select a category:
        <asp:DropDownList ID="Categories" runat="server" AutoPostBack="True"
			DataSourceID="CategoriesDataSource" DataTextField="Name"
			DataValueField="Id" AppendDataBoundItems="True" EnableViewState="False">
            <asp:ListItem Value="-1">All categories</asp:ListItem>
        </asp:DropDownList>
        <asp:GridView ID="ProductsGridView" runat="server" AutoGenerateColumns="False"
            DataKeyNames="Id" DataSourceID="ProductsGridDataSource" EnableViewState="False">
            <Columns>
				<asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id"
					SortExpression="Id" ReadOnly="True" />
                <asp:BoundField DataField="Name" HeaderText="Name"
					SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description"
					SortExpression="Description" />
				<asp:BoundField DataField="Price" HeaderText="Price"
					SortExpression="Price" />
				<asp:CommandField ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
        &nbsp;
	</p>	
</asp:Content>
