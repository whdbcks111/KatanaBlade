using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace .Application
{
    public class MySession
    {
        private static MySession session;

        // Gets the current session.
        public static MySession Current
        {
            get
            {
                if (HttpContext.Current.Session["__MySession__"] == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                else
                {
                    session = (MySession)HttpContext.Current.Session["__MySession__"];
                }
                return session;
            }
        }

        public static bool IsNull()
        {
            return (HttpContext.Current.Session["__MySession__"] == null) ? true : false;
        }
    }
}