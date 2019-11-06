using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class MenuExtensions
{
    public static MvcHtmlString MenuItem(
        this HtmlHelper htmlHelper,
        string text,
        string action,
        string controller,
        string actionActive,
        string controllerActive
    )
    {
        var li = new TagBuilder("li");
        if (string.Equals(actionActive, action, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(controllerActive, controller, StringComparison.OrdinalIgnoreCase))
        {
            li.AddCssClass("active");
        }
        li.InnerHtml = htmlHelper.ActionLink(text, action, controller).ToHtmlString();
        return MvcHtmlString.Create(li.ToString());
    }
}