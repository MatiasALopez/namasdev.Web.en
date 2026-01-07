using System;
using System.Web;

namespace namasdev.Web.Helpers
{
    public class UrlBuilder
    {
        public const string ORDER_SUFIX_DESC = " desc";
        public const string ORDER_NAME = "order";
        public const string PAGE_NAME = "page";

        public static string BuildUrlWithPage(Uri url, int page)
        {
            return BuildUrlWithParameter(url, PAGE_NAME, page.ToString());
        }

        public static string BuildUrlWithOrder(Uri url, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
        {
            var qs = HttpUtility.ParseQueryString(url.Query);

            order = BuildOrderExpression(order, qs[orderName],
                 applyOrderDescToFirstElementOnly);

            return BuildUrlWithParameter(url, orderName, order);
        }

        public static string BuildOrderExpression(string order, string currentOrder,
            bool applyOrderDescToFirstElementOnly = false)
        {
            if (string.Equals(order, currentOrder))
            {
                int separatorIndex = order.IndexOf(',');
                if (separatorIndex >= 0)
                {
                    if (applyOrderDescToFirstElementOnly)
                    {
                        order = order.Insert(separatorIndex, ORDER_SUFIX_DESC);
                    }
                    else
                    {
                        order = order.Replace(",", ORDER_SUFIX_DESC + ",") + ORDER_SUFIX_DESC;
                    }
                }
                else
                {
                    order = order + ORDER_SUFIX_DESC;
                }
            }

            return order;
        }

        public static string BuildUrlWithParameter(Uri url, string orderName, string orderValue)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var qs = HttpUtility.ParseQueryString(url.Query);
            if (!String.IsNullOrWhiteSpace(orderValue))
            {
                qs[orderName] = orderValue;
            }
            else
            {
                qs.Remove(orderName);
            }

            return url.GetLeftPart(UriPartial.Path) + "?" + qs.ToString();
        }

        public static string BuildAbsoluteUrl(string relativeUrl)
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + relativeUrl;
        }
    }
}