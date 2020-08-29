using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace CoEvent.Api.Helpers.Mail
{
    public class MailHelper : IMailHelper
    {
        #region Varaible
        public MailOptions Options { get; }
        #endregion

        #region Constructors
        public MailHelper(IOptions<MailOptions> options)
        {
            this.Options = options.Value;
        }
        #endregion

        #region Methods
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(this.Options.FromEmail),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(to));
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(this.Options.Host, this.Options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(this.Options.Username, this.Options.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        #endregion
    }
}
