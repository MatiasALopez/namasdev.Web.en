using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

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
    }
}