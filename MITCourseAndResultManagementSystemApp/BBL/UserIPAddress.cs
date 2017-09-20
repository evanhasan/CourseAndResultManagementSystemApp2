using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class UserIPAddress
    {
        public string GetUserIp()
        {
            //HttpRequest currentRequest = HttpContext.Current.Request;
            //string ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (ipAddress == null || ipAddress.ToLower() == "unknown")
            //    ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
            //return ipAddress;
            string UserIP=null;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    UserIP = Convert.ToString(IP);
                }
            }
            return UserIP;
        }
    }
}