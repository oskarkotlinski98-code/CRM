using CRM.Functions.Entities;
using CRM.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;



public class CustomerNotification
{
    private readonly EmailService _emailService;
    private readonly ILogger _logger;

    public CustomerNotification(
        EmailService emailService,
        ILoggerFactory loggerFactory)
    {
        _emailService = emailService;

        _logger =
            loggerFactory
            .CreateLogger<CustomerNotification>();
    }

    [Function("CustomerNotification")]

    public async Task Run(

        [CosmosDBTrigger(
            databaseName: "CRMDatabase",
            containerName: "Customers",
            Connection = "CosmosConnection",
            LeaseContainerName = "leases")]

        IReadOnlyList<Customer> customers)
    {
        if (customers == null || customers.Count == 0)
            return;

        foreach (var customer in customers)
        {
            _logger.LogInformation(
                $"Customer changed: {customer.Name}");

            var emailBody = $@"
            You are now responsible for a customer.

            Name: {customer.Name}
            Title: {customer.Title}
            Phone: {customer.Phone}
            Email: {customer.Email}
            Address: {customer.Address}

            Responsible Seller:
            {customer.ResponsibleSeller.Name}
            ";

            await _emailService.SendEmailAsync(
                customer.ResponsibleSeller.Email,
                emailBody);
        }
    }
}