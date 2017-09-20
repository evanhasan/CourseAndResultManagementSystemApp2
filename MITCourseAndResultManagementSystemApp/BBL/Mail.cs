using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class Mail
    {
        public void SendMail(string ReceiverMail, string Sub, string Message)
        {
            var SenderEmail = new MailAddress("abulhasanevan@gmail.com", "IIT DU");
            var Receiver = new MailAddress(ReceiverMail, "Faculty");

            const string Password = "44905591";
            string Subject = Sub;
            string Body = Message;

            var Smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(SenderEmail.Address, Password)
            };

            using (var MESSAGE = new MailMessage(SenderEmail, Receiver)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true,
                
                

            })
            {
                Smtp.Send(MESSAGE);                
            }                       
        }
    }
}