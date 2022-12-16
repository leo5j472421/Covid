using Covid.Controllers;

namespace Covid.Services.Interfaces
{
    public interface IMailService
    {
        void sendTestMail();
        void SendMail(SendMailRequest request);
    }
}