﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/_LayoutDetail.master" Inherits="System.Web.Mvc.ViewPage<MvcStore.ViewModels.ShoppingCartViewModel>" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
	<title>Shopping Cart</title>
</asp:Content>
<asp:Content ID="MainContentContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%: Url.Content("~/Scripts/jquery-1.8.2.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        // Document.ready -> link up remove event handler
        $(".RemoveLink").click(function () {
            // Get the id from the link
            var recordToDelete = $(this).attr("data-id");
            if (recordToDelete != '') {
                // Perform the ajax post
                $.post("/ShoppingCart/RemoveFromCart", {"id": recordToDelete },
                    function (data) {
                        // Successful requests get here
                        // Update the page elements
                        if (data.ItemCount == 0) {
                            $('#row-' + data.DeleteId).fadeOut('slow');
                        } else {
                            $('#item-count-' + data.DeleteId).text(data.ItemCount);
                        }
                        $('#cart-total').text(data.CartTotal);
                        $('#update-message').text(data.Message);
                        $('#cart-status').text('Cart (' + data.CartCount + ')');
                    });
            }
        });
    });
</script>
<h3>
    <em>Review</em> your cart:
</h3>
<p class="button">
    <%: Html.ActionLink("Checkout >>", "AddressAndPayment", "Checkout") %>
</p>
<div id="update-message">
</div>
<table>
    <tr>
        <th>
            Album Name
        </th>
        <th>
            Price (each)
        </th>
        <th>
            Quantity
        </th>
        <th></th>
    </tr>
    <% foreach (var item in Model.CartItems) { %>
        <tr id="row-<%: item.Id %>">
            <td>
                <%: Html.ActionLink(item.Product.Name, "Details", "Store", new { id = item.Product.Id }, null) %>
            </td>
            <td>
                <%: item.Product.Price %>
            </td>
            <td id="item-count-<%: item.Id %>">
                <%: item.Count %>
            </td>
            <td>
                <a href="#" class="RemoveLink" data-id="<%: item.Id %>">Remove from cart</a>
            </td>
        </tr>
    <% } %>
    <tr>
        <td>
            Total
        </td>
        <td>
        </td>
        <td>
        </td>
        <td id="cart-total">
            <%: Model.CartTotal %>
        </td>
    </tr>
</table>
</asp:Content>
