using System;
using System.Web;

namespace namasdev.Web.Helpers
{
    public class SessionHelper
    {
        private readonly HttpSessionStateBase _session;

        public SessionHelper(HttpSessionStateBase session)
        {
            _session = session;
        }

        private bool GetBool(string key)
        {
            return bool.Equals(_session[key], true);
        }

        private void SetBool(string key, bool value)
        {
            if (value)
            {
                _session[key] = true;
            }
            else
            {
                _session.Remove(key);
            }
        }

        private string GetString(string key)
        {
            return (string)_session[key];
        }

        private void SetString(string key, string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                _session[key] = value;
            }
            else
            {
                _session.Remove(key);
            }
        }
    }
}