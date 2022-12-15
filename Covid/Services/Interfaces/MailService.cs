using System;
using System.Net;
using System.Net.Mail;

namespace Covid.Services.Interfaces
{
    public class MailService : IMailService
    {
        public void sendTestMail()
        {
            const string SendMailFrom = "leo.chu@techbodia.com";
            const string sendMailTo = "leo.chu@remotes.com.tw";
            const string SendMailSubject = "Email Subject";
            var SendMailBody = "Email Body";

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com",587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();
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
    }
}