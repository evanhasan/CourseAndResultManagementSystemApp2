using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class RandomNumber
    {
        public string Random(int length)
        {
              var random = new Random();
                    string RandomNumber1 = string.Empty;
                    for (int i = 0; i < length; i++)
                        RandomNumber1 = String.Concat(RandomNumber1, random.Next(length).ToString());
                    return RandomNumber1;
        }
    }
}