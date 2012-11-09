<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Error</h2>	
	<p>
		We're sorry, we've hit an unexpected error.
    	<a href="javascript:history.go(-1)">Click here</a> 
    	if you'd like to go back and try that again.
	</p>
</asp:Content>
