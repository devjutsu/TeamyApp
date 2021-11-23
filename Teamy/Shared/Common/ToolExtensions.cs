using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Teamy.Shared.Common
{
    public static class ToolExtensions
    {
        public static string CalSlugify(this string value)
        {
            ////Remove all accents
            //var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            //value = Encoding.ASCII.GetString(bytes);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "-", RegexOptions.Compiled);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "+", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }


    }
}
