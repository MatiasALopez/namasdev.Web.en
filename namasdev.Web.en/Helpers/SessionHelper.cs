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

        public bool GetBool(string key)
        {
            return bool.Equals(_session[key], true);
        }

        public void SetBool(string key, bool value)
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

        public string GetString(string key)
        {
            return (string)_session[key];
        }

        public void SetString(string key, string value)
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