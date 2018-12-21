using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class StringUtils
    {
        public static bool IsEmail(string s)
        {
            if(!s.Contains("@"))
            {
                return false;
            }
            else if(!s.EndsWith(".com"))
            {
                return false;
            }
            return true;
        }
    }
}
