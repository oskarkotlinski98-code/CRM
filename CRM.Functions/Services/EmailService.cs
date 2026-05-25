using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CRM.Functions.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(
        string recipient,
        string customerInfo)
    {
        var apiKey =
            _configuration["SendGridApiKey"];

        var sender =
            _configuration["SenderEmail"];

        Console.WriteLine($"Recipient: {recipient}");
        Console.WriteLine($"Sender: {sender}");
        Console.WriteLine($"API exists: {!string.IsNullOrEmpty(apiKey)}");

        var client = new SendGridClient(apiKey);

        var from = new EmailAddress(
            sender,
            "CRM System");

        var to = new EmailAddress(recipient);

        var message =
            MailHelper.CreateSingleEmail(
                from,
                to,
                "New Customer Assignment",
                customerInfo,
                customerInfo);

        var response =
            await client.SendEmailAsync(message);

        Console.WriteLine(
            $"Status: {response.StatusCode}");
    }
}