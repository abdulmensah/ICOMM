using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace FullCalendar_MVC.Models
{
    public class CustomProviderProvider : SimpleMembershipProvider
    {
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return base.GetUser(username, userIsOnline);
        }

        public override bool ValidateUser(string username, string password)
        {
            return true; // base.ValidateUser(username, password);
        }
    }
}