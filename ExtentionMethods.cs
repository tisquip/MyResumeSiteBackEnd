using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeSiteBackEnd
{
    public static class ExtentionMethods
    {
        public static string ToStringSportsMonkFormatting(this DateTime caller)
        {
            return $"{caller:yyyy}-{caller:MM}-{caller:dd}";
        }
    }
}
