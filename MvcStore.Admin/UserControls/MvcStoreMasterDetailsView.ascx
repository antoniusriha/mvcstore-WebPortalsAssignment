<%@ Control Language="C#" Inherits="MvcStore.Admin.UserControls.MvcStoreMasterDetailsView" %>
    <asp:ObjectDataSource ID="MasterDataSource" runat="server"
        SelectMethod="GetItems"
		InsertMethod="AddItem"
		DeleteMethod="DeleteItem"
		UpdateMethod="UpdateItem">
	</asp:ObjectDataSource>
    <asp:ObjectDataSource ID="DetailDataSource" runat="server"
        SelectMethod="GetItemById"
		InsertMethod="AddItem"
		DeleteMethod="DeleteItem"
		UpdateMethod="UpdateItem">
		<SelectParameters>
			<asp:ControlParameter ControlID="MasterView" Name="id"
				PropertyName="SelectedValue" Type="Int32" />
		</SelectParameters>
	</asp:ObjectDataSource>
    <br />
    <h3 id="HeaderText" runat="server" />
    <p>
        <asp:DetailsView ID="DetailView" runat="server" AutoGenerateRows="False"
			DataKeyNames="Id" DataSourceID="DetailDataSource" EnableViewState="False"
			AllowPaging="True">
        </asp:DetailsView>
        &nbsp;
	</p>
    <p>
        <asp:GridView ID="MasterView" runat="server" AutoGenerateColumns="False"
            DataKeyNames="Id" DataSourceID="MasterDataSource" EnableViewState="False">
        </asp:GridView>
        &nbsp;
	</p>
	<asp:Button ID="InsertNewItemButton" OnClick="InsertNewItemButton_Click"
		runat="server" Text="Add a new item ..." />
	<p>Note: This UI is shitty from a HCI perspective.</p>