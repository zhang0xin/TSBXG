using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TSBXG.Models;

namespace TSBXG.Helper
{
    public class LogonHelper
    {
        public static bool Logon(string username, string password)
        {
            var db = new TSBXGContainer();
            var currUser = db.User.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
            if (currUser != null)
            {
                HttpContext.Current.Session["CurrentUser"] = currUser;
                return true;
            }
            return false;
        }
    }
}