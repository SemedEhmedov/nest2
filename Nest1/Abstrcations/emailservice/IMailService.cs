using Nest1.Helpers.email;

namespace Nest1.Abstrcations.emailservice
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);   
    }
}
