using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;

namespace namasdev.Web.Helpers
{
	public class UserHelper
	{
		private readonly HttpContextBase _context;

        public UserHelper(HttpContextBase context)
		{
			_context = context;
		}

		public bool IsLoggedIn
		{
			get { return _context.User.Identity.IsAuthenticated; }
		}

        private string _userId;
        public string UserId
        {
            get
            {
                return
                    _userId
                    ?? (_userId = IsLoggedIn
                            ? _context.User.Identity.GetUserId()
                            : null);
            }
        }

		private string _userName;
        public string UserName
        {
            get
            {
                return
                    _userName
                    ?? (_userName = IsLoggedIn
                            ? _context.User.Identity.Name
                            : null);
            }
        }

        public bool IsInRole(string role)
		{
			return _context.User.IsInRole(role);
		}

		public bool IsInAnyRole(params string[] roles)
		{
			if (roles == null || !roles.Any())
			{
				return false;
			}

			return roles.Any(r => _context.User.IsInRole(r));
		}
    }
}