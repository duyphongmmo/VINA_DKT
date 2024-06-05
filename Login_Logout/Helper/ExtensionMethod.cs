using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login_Logout.Helper
{
    public static class ExtensionMethod
    {
        public static string ToStringD(this DateTime dt)
        {
            return $"{dt.Day}/{dt.Month}/{dt.Year}";
        }
    }
}