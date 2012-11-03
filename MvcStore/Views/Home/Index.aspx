<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<MvcStore.Controllers.HomeViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<div>
		<%= Html.Encode (Model.WelcomeMessage) %>
	</div>
</body>

