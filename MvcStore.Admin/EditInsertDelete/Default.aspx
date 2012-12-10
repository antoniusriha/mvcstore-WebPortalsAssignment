<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="EditInsertDelete_Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        Editing, Inserting, and Deleting Tutorials</h2>
    <p>
        <mvc:SectionLevelTutorialListing ID="SectionLevelTutorialListing1" runat="server" />
        &nbsp;</p>
</asp:Content>

