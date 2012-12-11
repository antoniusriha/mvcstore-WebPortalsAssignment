<%@ Page Language="C#" Inherits="MvcStore.Admin.Default" MasterPageFile="~/Site.master" Title="Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Welcome to the MvcStore Admin Interface</h1>

    <h3>Sections:</h3>
    <p>
		<mvc:SectionLevelTutorialListing ID="SectionLevelTutorialListing1" runat="server" />
		&nbsp;
	</p>		
</asp:Content>