using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class Password
    {
       
        public int CheckStrength(string x)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            bool has=false;
            int flag = 0;
            foreach (var item in specialChar)
            {
                if (x.Contains(item))
                    has =true;
            }
            if (has == true)
            {
                flag= 1;
            }
            else
            {
                flag= 0;
            }
            if (x.Where(y => char.IsDigit(y)).Any() == true)
            {
                flag++;
            }
            if (x.Where(y => char.IsUpper(y)).Any() == true)
            {
                flag++;
            }

            if (x.Where(y => char.IsLower(y)).Any() == true)
            {
                flag++;
            }
            return flag;
        }
    }
}