using System;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Source.Options;

namespace Source.Service;

public class EmailSender(IOptions<SmtpOption> options) : IEmailSender
{
    private readonly SmtpOption _options = options.Value;
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var body = new TextPart(TextFormat.Html);

        body.Text = htmlMessage;
    
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress(null, _options.FromEmail));
        message.To.Add(new MailboxAddress(null, email));
        message.Body = body;
        message.Subject = subject;

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Host, _options.Port);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}