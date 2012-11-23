<%@ Page Language="C#" Inherits="MvcStore.Admin.UserManagement.Default" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<%@ Register Src="../UserControls/SectionLevelTutorialListing.ascx" TagName="SectionLevelTutorialListing"
    TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<h2>User Management</h2>
    <p>
		<uc1:SectionLevelTutorialListing ID="SectionLevelTutorialListing1" runat="server" />
		&nbsp;
	</p>		
</asp:Content>
