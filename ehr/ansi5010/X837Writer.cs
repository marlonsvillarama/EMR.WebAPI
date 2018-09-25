using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EMR.WebAPI.ehr.ansi5010
{
    public class X837Writer
    {
        StringBuilder sb;

        public X837Writer(string segment)
        {
            sb = new StringBuilder().Append(segment).Append("*");
        }

        public X837Writer AddElement(object obj)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }

            sb.Append(obj.ToString().ToUpper()).Append("*")
                .ToString();

            return this;
        }

        public string Output
        {
            get
            {
                string s = sb.ToString();

                s = s.Substring(0, s.Length - 1);

                sb = new StringBuilder().Append(s).Append("~");

                if (X837Config.SeparateNewLine == true)
                {
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }

        public static string WriteDate(DateTime dt)
        {
            return String.Format("{0}{1}{2}",
                dt.Year.ToString(),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0')
            );
        }

        public static string WriteTime(DateTime dt)
        {
            return String.Format("{0}{1}",
                dt.Hour.ToString().PadLeft(2, '0'),
                dt.Minute.ToString().PadLeft(2, '0'));
        }

        public static string WriteSeparator(bool separator = false)
        {
            return (separator == true ? Environment.NewLine : String.Empty);
        }
    }
}