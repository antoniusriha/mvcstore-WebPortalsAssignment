<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<MvcStore.Models.Category>>" %>

<ul id="categories">
    <% foreach (var cat in Model) { %>
        <li>
			<%: Html.ActionLink(cat.Name, "Browse", "Store", new { Category = cat.Name }, null) %>
        </li>
    <% } %>
</ul>
