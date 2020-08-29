using System.Threading.Tasks;

namespace CoEvent.Api.Helpers.Mail
{
    public interface IMailHelper
    {
        MailOptions Options { get; }
        Task SendEmailAsync(string to, string subject, string body);
    }
}
