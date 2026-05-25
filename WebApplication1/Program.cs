using CRM.Api.Data.Entities;
using CRM.Api.Services;
using Microsoft.Azure.Cosmos;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
           

            // Cosmos client registration
            builder.Services.AddSingleton(s =>
            {
                var configuration = s.GetRequiredService<IConfiguration>();

                return new CosmosClient(
                    configuration["CosmosDb:ConnectionString"]);
            });

            // Register custom service
            builder.Services.AddSingleton<CosmosDbService>();

            var app = builder.Build();



            // ==========================
            // CUSTOMER ENDPOINTS
            // ==========================


            // GET all customers
            app.MapGet("/customers",
            async (CosmosDbService service) =>
            {
                var customers =
                    await service.GetCustomersAsync();

                return Results.Ok(customers);
            });


            // GET customer by ID
            app.MapGet("/customers/{id}",
            async (string id,
            CosmosDbService service) =>
            {
                var customer =
                    await service.GetCustomerAsync(id);

                return customer is null
                    ? Results.NotFound()
                    : Results.Ok(customer);
            });


            // CREATE customer
            app.MapPost("/customers",
            async (
            Customer customer,
            CosmosDbService service) =>
            {
                var result =
                    await service.AddCustomerAsync(customer);

                return Results.Ok(result);
            });


            // UPDATE customer
            app.MapPut("/customers/{id}",
            async (
            string id,
            Customer customer,
            CosmosDbService service) =>
            {
                var updated =
                    await service.UpdateCustomerAsync(
                        id,
                        customer);

                return Results.Ok(updated);
            });


            // DELETE customer
            app.MapDelete("/customers/{id}",
            async (
            string id,
            CosmosDbService service) =>
            {
                await service.DeleteCustomerAsync(id);

                return Results.Ok(
                    $"Customer {id} deleted");
            });


            // SEARCH BY CUSTOMER NAME
            app.MapGet("/customers/search/{name}",
            async (
            string name,
            CosmosDbService service) =>
            {
                var customers =
                    await service
                        .SearchCustomerByNameAsync(name);

                return Results.Ok(customers);
            });


            // SEARCH BY RESPONSIBLE SELLER
            app.MapGet("/customers/seller/{sellerName}",
            async (
            string sellerName,
            CosmosDbService service) =>
            {
                var customers =
                    await service
                        .SearchBySellerAsync(
                            sellerName);

                return Results.Ok(customers);
            });

            app.Run();
        }
    }
}
