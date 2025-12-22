using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Owin.Security;

using namasdev.Web.Helpers;

namespace namasdev.Web.Controllers
{
    public class ControllerBase : Controller
    {
        private ControllerHelper _controllerHelper;
        protected ControllerHelper ControllerHelper
        {
            get { return _controllerHelper ?? (_controllerHelper = new ControllerHelper(this)); }
        }

        private UserHelper _userHelper;
        protected UserHelper UserHelper
        {
            get { return _userHelper ?? (_userHelper = new UserHelper(this.HttpContext)); }
        }

        private IAuthenticationManager _authenticationManager;
        protected IAuthenticationManager AuthenticationManager
        {
            get { return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication); }
        }

        private SessionHelper _sessionHelper;
        protected SessionHelper SessionHelper
        {
            get { return _sessionHelper ?? (_sessionHelper = new SessionHelper(this.HttpContext.Session)); }
        }

        private string _userId;
        protected string UserId
        {
            get { return _userId ?? (_userId = UserHelper.UserId); }
        }

        private string _userName;
        protected string UserName
        {
            get { return _userName ?? (_userName = UserHelper.UserName); }
        }

        public JsonResult CreateJsonResultOk(
            string message = null,
            JsonRequestBehavior jsonRequestBehaviour = JsonRequestBehavior.DenyGet)
        {
            return CreateJsonResult(ok: true, message: message, jsonRequestBehaviour: jsonRequestBehaviour);
        }

        public JsonResult CreateJsonResultError(string message,
            JsonRequestBehavior jsonRequestBehaviour = JsonRequestBehavior.DenyGet)
        {
            return CreateJsonResult(ok: false, message: message, jsonRequestBehaviour: jsonRequestBehaviour);
        }

        private JsonResult CreateJsonResult(bool ok, string message,
            JsonRequestBehavior jsonRequestBehaviour = JsonRequestBehavior.DenyGet)
        {
            return Json(new { ok, message }, jsonRequestBehaviour);
        }

        public void SignOutAndClearSession()
        {
            var types = AuthenticationManager.GetAuthenticationTypes();
            AuthenticationManager.SignOut(types.Select(t => t.AuthenticationType).ToArray());

            Session.RemoveAll();
        }
    }
}