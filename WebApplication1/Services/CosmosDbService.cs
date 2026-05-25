using CRM.Api.Data.Entities;
using Microsoft.Azure.Cosmos;

namespace CRM.Api.Services;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient cosmosClient,
        IConfiguration configuration)
    {
        var databaseName =
            configuration["CosmosDb:DatabaseName"];

        var containerName =
            configuration["CosmosDb:ContainerName"];

        _container = cosmosClient.GetContainer(
            databaseName,
            containerName);
    }

    // CREATE
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        var response = await _container.CreateItemAsync(
            customer,
            new PartitionKey(customer.Id));

        return response.Resource;
    }

    // GET ALL
    public async Task<List<Customer>> GetCustomersAsync()
    {
        var query = _container.GetItemQueryIterator<Customer>(
            "SELECT * FROM c");

        List<Customer> customers = new();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            customers.AddRange(response);
        }

        return customers;
    }

    // GET BY ID
    public async Task<Customer?> GetCustomerAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Customer>(
                id,
                new PartitionKey(id));

            return response.Resource;
        }
        catch (CosmosException ex)
        {
            if (ex.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            throw;
        }
    }

    // UPDATE
    public async Task<Customer> UpdateCustomerAsync(
        string id,
        Customer customer)
    {
        customer.Id = id;

        var response =
            await _container.UpsertItemAsync(
                customer,
                new PartitionKey(id));

        return response.Resource;
    }

    // DELETE
    public async Task DeleteCustomerAsync(string id)
    {
        await _container.DeleteItemAsync<Customer>(
            id,
            new PartitionKey(id));
    }

    // SEARCH BY CUSTOMER NAME
    public async Task<List<Customer>> SearchCustomerByNameAsync(
        string name)
    {
        var query = new QueryDefinition(
            "SELECT * FROM c WHERE CONTAINS(c.Name,@name)")
            .WithParameter("@name", name);

        var results =
            _container.GetItemQueryIterator<Customer>(query);

        List<Customer> customers = new();

        while (results.HasMoreResults)
        {
            var response = await results.ReadNextAsync();

            customers.AddRange(response);
        }

        return customers;
    }

    // SEARCH BY SELLER NAME
    public async Task<List<Customer>> SearchBySellerAsync(
        string sellerName)
    {
        var query = new QueryDefinition(
            @"SELECT * FROM c
            WHERE CONTAINS(
            c.ResponsibleSeller.Name,
            @sellerName)")
            .WithParameter(
                "@sellerName",
                sellerName);

        var results =
            _container.GetItemQueryIterator<Customer>(
                query);

        List<Customer> customers = new();

        while (results.HasMoreResults)
        {
            var response = await results.ReadNextAsync();

            customers.AddRange(response);
        }

        return customers;
    }
}