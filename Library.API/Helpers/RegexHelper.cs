using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public static class RegexHelper
    {
        public static bool IsEmail(string inputData)
        {
            Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样  	
            Match m = RegEmail.Match(inputData);
            return m.Success;
        } 
    }
}
