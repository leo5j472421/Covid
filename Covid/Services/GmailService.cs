using System;
using System.Net;
using System.Net.Mail;
using Covid.Controllers;
using Covid.Repositories.Interfaces;
using Covid.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Covid.Services
{
    public class GmailService : IMailService
    {
        public GmailService(IConfiguration configuration, IMailRepository mailRepository, ILoggerService logger)
        {
            _configuration = configuration;
            _mailRepository = mailRepository;
            _logger = logger;
        }

        private readonly IConfiguration _configuration;
        private readonly IMailRepository _mailRepository;
        private ILoggerService _logger;

        public void sendTestMail()
        {
            const string SendMailFrom = "leo.chu@techbodia.com";
            const string sendMailTo = "leo.chu@remotes.com.tw";
            const string SendMailSubject = "Email Subject";
            var SendMailBody = "Email Body";

            try
            {
                var SmtpServer =
                    new SmtpClient(_configuration.GetSection("MailSettings").GetSection("SMTPServer").Value,
                        int.Parse(_configuration.GetSection("MailSettings").GetSection("SMTPServerPort").Value));
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                var email = new MailMessage();
                // START
                email.From = new MailAddress(SendMailFrom);
                email.To.Add(sendMailTo);
                // email.CC.Add(SendMailFrom);
                email.Subject = SendMailSubject;
                const string htmlString = @"<html>
                      <body>
                      <p>Dear Ms. Susan,</p>
                      <p>Thank you for your letter of yesterday inviting me to come for an interview on Friday afternoon, 5th July, at 2:30.
                              I shall be happy to be there as requested and will bring my diploma and other papers with me.</p>
                      <p>Sincerely,<br>-Jack</br></p>
                      </body>
                      </html>
                     ";
                email.Body = htmlString;

                email.IsBodyHtml = true;
                //END
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "bbqomkgjwbuialgj");
                SmtpServer.Send(email);
                Console.WriteLine("Email Successfully Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SendMail(SendMailRequest request)
        {
            var sentMailList = _mailRepository.GetSentMailList(request);
            var mailConfig = GetMailConfig();
            foreach (var mailInfo in sentMailList)
            {
                try
                {
                    var SmtpServer =
                        new SmtpClient(mailConfig.SMTPServer, mailConfig.Port);
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    var email = new MailMessage();
                    // START
                    email.From = new MailAddress(mailConfig.MailFrom);
                    email.To.Add(mailInfo.Mail);
                    // email.CC.Add(SendMailFrom);
                    email.Subject = mailInfo.Subject.Replace("{Name}",mailInfo.Name);;
                    email.Body = mailInfo.Body.Replace("{Company}",mailInfo.Company);
                    email.IsBodyHtml = true;
                    //END
                    SmtpServer.Timeout = 5000;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new NetworkCredential(mailConfig.MailFrom, mailConfig.MailPassword);
                    SmtpServer.Send(email);
                    _logger.Info("Email Successfully Sent");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString());
                }
            }
        }

        private MailConfig GetMailConfig()
        {
            var mailSettings = _configuration.GetSection("MailSetting");
            return new MailConfig()
            {
                SMTPServer = mailSettings.GetSection("SMTPServer").Value,
                Port = int.Parse(mailSettings.GetSection("SMTPServerPort").Value),
                MailFrom = mailSettings.GetSection("MailFrom").Value,
                MailPassword = mailSettings.GetSection("MailPassword").Value
            };
        }
    }

    internal class MailConfig
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string MailFrom { get; set; }
        public string MailPassword { get; set; }
    }
}