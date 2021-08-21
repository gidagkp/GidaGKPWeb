using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using GidaGkpWeb.Infrastructure.Utility;
using System.Web.Mail;

namespace GidaGkpWeb.Infrastructure
{
    public class EmailService : IMessageSystem
    {
        public void Send(Message msg)
        {
            //notify user via email
            bool isProductionServer = Convert.ToBoolean(ConfigurationManager.AppSettings["isProductionServer"]);
            string GIDAEmail = Convert.ToString(ConfigurationManager.AppSettings["GIDAEmail"]);
            string GIDARelayServer = Convert.ToString(ConfigurationManager.AppSettings["GIDARelayServer"]);
            string HostEmailName = Convert.ToString(ConfigurationManager.AppSettings["HostEmailName"]);
            string HostEmailPassword = Convert.ToString(ConfigurationManager.AppSettings["HostEmailPassword"]);
            string HostAddress = Convert.ToString(ConfigurationManager.AppSettings["HostAddress"]);
            string hostEmail = Convert.ToString(ConfigurationManager.AppSettings["HostEmail"]);
            int HostPort = Convert.ToInt32(ConfigurationManager.AppSettings["HostPort"]);
            var fromAddress = new MailAddress(hostEmail, HostEmailName);
            var toAddress = new MailAddress(msg.MessageTo, string.IsNullOrEmpty(msg.MessageNameTo) ? "User" : msg.MessageNameTo);
            string subject = msg.Subject;
            string body = msg.Body;

            if (isProductionServer)
            {
                var objMail = new System.Web.Mail.MailMessage();
                objMail.From = GIDAEmail;
                objMail.To = msg.MessageTo;
                objMail.Subject = subject;
                objMail.BodyFormat = MailFormat.Html;
                objMail.Body = body;
                SmtpMail.SmtpServer = GIDARelayServer;
                SmtpMail.Send(objMail);
            }
            else
            {
                var smtp = new SmtpClient
                {
                    Host = HostAddress,
                    Port = HostPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, HostEmailPassword)
                };

                using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.SendCompleted += (s, e) =>
                    {
                        smtp.Dispose();
                        message.Dispose();
                    };
                    smtp.Send(message);
                }
            }
        }
    }
}