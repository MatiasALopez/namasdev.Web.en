using System.Web.Mvc;

namespace namasdev.Web.Helpers
{
    public static class WebViewPageExtensions
    {
        public static string[] GetMessageSuccess(this WebViewPage page)
        {
            return ViewBagHelper.GetMessageSuccess(page.ViewBag);
        }

        public static string[] GetMessageInfo(this WebViewPage page)
        {
            return ViewBagHelper.GetMessageInfo(page.ViewBag);
        }

        public static string[] GetMessageWarning(this WebViewPage page)
        {
            return ViewBagHelper.GetMessageWarning(page.ViewBag);
        }

        public static string[] GetMessageError(this WebViewPage page)
        {
            return ViewBagHelper.GetMessageError(page.ViewBag);
        }
    }
}
