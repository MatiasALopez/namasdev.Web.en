using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using namasdev.Web.Models;

namespace namasdev.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DisplayNameWithoutEncodingFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            return new MvcHtmlString(metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last());
        }

        public static MvcHtmlString MessageSuccess(this HtmlHelper html,
            bool isDismissable = false)
        {
            return AlertSuccess(html,
                messages: ViewBagHelper.GetMessageSuccess(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString MessageInfo(this HtmlHelper html,
            bool isDismissable = false)
        {
            return AlertInfo(html, 
                messages: ViewBagHelper.GetMessageInfo(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString MessageWarning(this HtmlHelper html,
            bool isDismissable = false)
        {
            return AlertWarning(html, 
                messages: ViewBagHelper.GetMessageWarning(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString MessageError(this HtmlHelper html,
            bool isDismissable = false)
        {
            return AlertDanger(html, 
                messages: ViewBagHelper.GetMessageError(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertSuccess(this HtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertSuccess(html, 
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertSuccess(this HtmlHelper html, IEnumerable<string> messages,
            bool isDismissable = false)
        {
            return Alert(html, 
                AlertType.Success, 
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertInfo(this HtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertInfo(html, 
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertInfo(this HtmlHelper html, IEnumerable<string> messages,
            bool isDismissable = false)
        {
            return Alert(html, 
                AlertType.Info, 
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertWarning(this HtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertWarning(html, 
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertWarning(this HtmlHelper html, IEnumerable<string> messages,
            bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Warning, 
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertDanger(this HtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertDanger(html, 
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static MvcHtmlString AlertDanger(this HtmlHelper html, IEnumerable<string> messages,
           bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Danger, 
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static MvcHtmlString Alert(this HtmlHelper html, AlertType type, string[] messages,
            bool isDismissable = false)
        {
            if (messages == null || !messages.Any())
            {
                return new MvcHtmlString("");
            }

            string alertCssClass = $"alert alert-{type.ToString().ToLower()}", 
                dismissButton = null;
            if (isDismissable)
            {
                alertCssClass += "alert-dismissible fade show";
                dismissButton = "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>";
            }
            return new MvcHtmlString($"<div class=\"{alertCssClass}\" role=\"alert\">{html.Raw(Core.Types.Formatter.List(messages, "<br/>"))}{dismissButton}</div>");
        }
    }
}