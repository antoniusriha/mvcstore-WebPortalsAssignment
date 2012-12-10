<%@ Page Language="C#" Inherits="MvcStore.Admin.BasicReporting.Default" MasterPageFile="~/Site.master" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="MainContent" ID="MainContentContent" runat="server">
	<h2>Basic Reporting Tutorials</h2>
    <p>
		<mvc:SectionLevelTutorialListing ID="SectionLevelTutorialListing1" runat="server" />
		&nbsp;
	</p>	
</asp:Content>
