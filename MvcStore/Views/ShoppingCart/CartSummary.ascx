﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%: Html.ActionLink("Cart (" + ViewData["CartCount"] + ")",
    "Index",
    "ShoppingCart",
    new { id = "cart-status" }) %>