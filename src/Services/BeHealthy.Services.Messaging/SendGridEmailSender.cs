namespace BeHealthy.Services.Messaging
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly EmailAddress fromAddress;

        public SendGridEmailSender(string apiKey, string senderEmailAddress, string senderName)
        {
            this.client = new SendGridClient(apiKey);
            this.fromAddress = new EmailAddress(senderEmailAddress, senderName);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlMessage))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var toAddress = new EmailAddress(email);

            var message = MailHelper.CreateSingleEmail(this.fromAddress, toAddress, subject, null, htmlMessage);

            try
            {
                var response = await this.client.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
