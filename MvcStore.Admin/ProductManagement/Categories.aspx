<%@ Page Language="C#" Inherits="MvcStore.Admin.ProductManagement.Categories" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
    <asp:ObjectDataSource ID="CategoriesGridDataSource" runat="server"
        SelectMethod="GetCategories"
		DeleteMethod="DeleteCategory" 
		UpdateMethod="UpdateCategory"
		TypeName="MvcStore.Admin.StoreProxy" >
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
		<UpdateParameters>
			<asp:Parameter Name="id" Type="Int32" />
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
		</UpdateParameters>
	</asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CategoryDetailsDataSource" runat="server"
        SelectMethod="GetCategoryById"
		InsertMethod="InsertCategory"
		DeleteMethod="DeleteCategory" 
		UpdateMethod="UpdateCategory"
		TypeName="MvcStore.Admin.StoreProxy" >
		<DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
		<InsertParameters>
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
		</InsertParameters>
		<UpdateParameters>
			<asp:Parameter Name="id" Type="Int32" />
			<asp:Parameter Name="name" Type="String" />
			<asp:Parameter Name="description" Type="String" />
		</UpdateParameters>
		<SelectParameters>
			<asp:ControlParameter ControlID="CategoriesGridView" Name="id"
					PropertyName="SelectedValue" Type="Int32" />
		</SelectParameters>
	</asp:ObjectDataSource>
    <br />
    <h3>Category Details</h3>
    <p>
        <asp:DetailsView ID="CategoryDetailsView" runat="server" AutoGenerateRows="False"
			DataKeyNames="Id" DataSourceID="CategoryDetailsDataSource" EnableViewState="False"
			AllowPaging="True">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id"
					SortExpression="Id" ReadOnly="True" InsertVisible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name"
					SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description"
					SortExpression="Description" />
				<asp:CommandField ShowDeleteButton="True" ShowEditButton="True"
					ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        &nbsp;
	</p>
    <h3>Categories</h3>
    <p>
        <asp:GridView ID="CategoriesGridView" runat="server" AutoGenerateColumns="False"
            DataKeyNames="Id" DataSourceID="CategoriesGridDataSource" EnableViewState="False">
            <Columns>
				<asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id"
					SortExpression="Id" ReadOnly="True" />
                <asp:BoundField DataField="Name" HeaderText="Name"
					SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description"
					SortExpression="Description" />
				<asp:CommandField ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
        &nbsp;
	</p>
</asp:Content>
