using DungeonMastersArchive.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DungeonMastersArchive.Components.Account
{
    internal sealed class IdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly EmailSenderSettings _settings;
        public IdentityEmailSender(IOptions<EmailSenderSettings> emailSenderSettings)
        {
            _settings = emailSenderSettings.Value;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            await SendEmailAsync(email, $"Bekräfta ditt konto hos SL-armour genom klicka på länken: <a href='{confirmationLink}'>{confirmationLink}</a>", "Bekräfta e-postadress");
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            await SendEmailAsync(email, $"Återställ ditt lösenord genom att använda följande kod: {resetCode}", "Återställ ditt lösenord");
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            await SendEmailAsync(email, $"Återställ ditt lösenord genom att klicka på följande länk: <a href='{resetLink}'>{resetLink}</a>", "Återställ ditt lösenord");
        }

        private async Task SendEmailAsync(string email, string htmlBody, string subject)
        {
            var mailMessage = new MailMessage(_settings.Username, email);
            mailMessage.From = new MailAddress(_settings.DefaultFromEmail);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlBody;
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient(_settings.Host);
            smtpClient.Host = _settings.Host;
            smtpClient.Port = _settings.Port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            { 
            }
            
        }
    }
}
