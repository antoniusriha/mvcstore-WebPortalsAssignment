<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage<MvcStore.Models.Product>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Produkt - <%: Model.Name %></title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
	<p>
    	<img alt="<%: Model.Name %>" src="@Model.AlbumArtUrl" />
	</p>
	<div id="album-details">
		<p>
			<em>Description:</em>
			<%: Model.Description %>
		</p>
		<p>
			<em>Price:</em>
			<%: String.Format ("{0:F}", Model.Price) %>
		</p>
		<p class="button">
			<%: Html.ActionLink ("Add to cart", "AddToCart", "ShoppingCart", new { id = Model.Id }, "") %>
		</p>
	</div>
</asp:Content>
