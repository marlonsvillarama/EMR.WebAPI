using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    static class Utilities
    {
        public static string StripString(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }

            string stripped = new string(s.Where(c => char.IsDigit(c)).ToArray());
            return stripped;
        }

        public static string FormatZip(string s)
        {
            if (String.IsNullOrEmpty(s) == true)
            {
                return String.Empty;
            }

            string stripped = StripString(s);
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(stripped) == true)
            {
                return String.Empty;
            }
            else
            {
                if (stripped.Length == 9)
                {
                    sb.Append(stripped.Substring(0, 5)).Append("-").Append(stripped.Substring(5));
                }
                else
                {
                    sb.Append(stripped);
                }

                return sb.ToString();
            }
        }

        public static string FormatPhone(string s)
        {
            if (String.IsNullOrEmpty(s) == true)
            {
                return String.Empty;
            }

            string stripped = StripString(s);
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(stripped) == true)
            {
                return String.Empty;
            }
            else if (stripped.Length == 10)
            {
                sb.Append("(").Append(stripped.Substring(0, 3)).Append(") ")
                    .Append(stripped.Substring(3, 3)).Append("-")
                    .Append(stripped.Substring(6));
            }
            else if (stripped.Length == 7)
            {
                sb.Append(stripped.Substring(0, 3)).Append("-")
                    .Append(stripped.Substring(3));
            }
            else
            {
                sb.Append(stripped);
            }
            
            return sb.ToString();
        }

        public static string FormatAddress(Address a)
        {
            if (a == null)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(a.Address_1) == false)
            {
                sb.Append(a.Address_1).Append("<br/>");
            }
            if (String.IsNullOrEmpty(a.Address_2) == false)
            {
                sb.Append(a.Address_2).Append("<br/>");
            }
            if (String.IsNullOrEmpty(a.City) == false)
            {
                sb.Append(a.City).Append(", ");
            }
            if (String.IsNullOrEmpty(a.State) == false)
            {
                sb.Append(a.State).Append(" ");
            }
            if (String.IsNullOrEmpty(a.Zip) == false)
            {
                sb.Append(FormatZip(a.Zip));
            }

            return sb.ToString();
        }
    }
}