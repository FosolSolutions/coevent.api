using CoEvent.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CoEvent.Api.Helpers.Mail
{
    /// <summary>
    /// MailClient class, provides a way to send emails.
    /// </summary>
    public class MailClient
    {
        #region Variables
        private MailOptions _options;
        private SmtpClient _client;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a MailClient object, and initializes it with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public MailClient(MailOptions options)
        {
            _options = options;

            _client = new SmtpClient(_options.Host)
            {
                Port = _options.Port,
                UseDefaultCredentials = _options.UseDefaultCredentials,
                Credentials = new System.Net.NetworkCredential(_options.Username, _options.Password),
                EnableSsl = _options.EnableSsl,
                Timeout = _options.Timeout
            };
        }

        /// <summary>
        /// Creates a new instance of a MailClient object, and initializes it with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public MailClient(IOptions<MailOptions> options) : this(options.Value)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a mail message to invite the participant to the calendar.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public MailMessage CreateInvitation(Participant participant)
        {
            // TODO: Move this to some kind of templating service.
            // TODO: Need to include the current domain name instead of hardcoding it.
            var message = new MailMessage
            {
                From = new MailAddress(_options.FromEmail),
                Subject = "Victoria Ecclesial Volunteer Schedule",
                IsBodyHtml = true,
                Body = $@"
				Hello {participant.FirstName},
				<p>
                    Apology for the earlier invite not working.  Due to technical issues I sent it before the schedule was ready.
                    This new link should allow you to login and volunteer.
                    There are some oddities with mobile devices that sometimes requires you to refresh the page.
                    Please reach out to me if you run into issues.
                </p>
                <p>
					Please use the following link to access the Victoria Ecclesial Volunteer Schedule - <a href=""https://coeventapp.azurewebsites.net/login?key={participant.Key}"">VOLUNTEER LINK</a><br/>
					This link is specifically generated for you, please do not forward it to someone else.
				</p>
                <p>
				    Love in Christ, Jeremy<br/>
                    m: 778 977-4786<br/>
                    e: jeremymfoster@hotmail.com
                </p>"
            };

            message.To.Add(new MailAddress(participant.Email));
            return message;
        }

        /// <summary>
        /// Send the specified email message.
        /// </summary>
        /// <param name="message"></param>
        public void Send(MailMessage message)
        {
            _client.Send(message);
        }

        /// <summary>
        /// Send the specified email message asynchronously.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsync(MailMessage message)
        {
            await _client.SendMailAsync(message);
        }
        #endregion
    }
}
