using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Customers.Api.Tests.Integration.CustomerController;

//[Collection("CustomerApi Collection")]
public class CreateCustomerControllerTests : IAsyncLifetime, IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly List<Guid> _createdIds = new();

    private readonly Faker<CustomerRequest> _customerGenerator =
        new Faker<CustomerRequest>()
            .RuleFor(x => x.FullName, faker => faker.Person.FullName)
            .RuleFor(x => x.Email, faker => faker.Person.Email)
            .RuleFor(x => x.GitHubUsername, "nickchapsas")
            .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    private readonly HttpClient _httpClient;

    public CreateCustomerControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        foreach (var createdId in _createdIds) await _httpClient.DeleteAsync($"customers/{createdId}");
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenCustomerIsCreated()
    {
        // Arrange
        var customer = _customerGenerator.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("customers", customer);

        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customer);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        _createdIds.Add(customerResponse!.Id);
    }
}