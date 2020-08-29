namespace CoEvent.Api.Helpers.Mail
{
    /// <summary>
    /// MailOptions class, provides a way to configure the MailClient.
    /// </summary>
    public class MailOptions
    {
        #region Properties
        /// <summary>
        /// get/set - The from address used when an email is sent.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// get/set - The email account address.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// get/set - The email account password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// get/set - The email account SMTP DNS.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// get/set - The email account SMTP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// get/set - Whether to enable SSL.
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// get/set - Whether to use default credentials when sending email.
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// get/set - How long to wait in milliseconds before timing out a request to send email.
        /// </summary>
        public int Timeout { get; set; } = 15000;
        #endregion
    }
}
