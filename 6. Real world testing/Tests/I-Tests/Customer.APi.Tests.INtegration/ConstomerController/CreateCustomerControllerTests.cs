using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Customer.APi.Tests.INtegration.ConstomerController
{
    public class CreateCustomerControllerTests : IClassFixture<CustomerApiFactory>
    {
        private readonly CustomerApiFactory _customerApifactory;

        private readonly HttpClient _httpClient;

        private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
            .RuleFor(x => x.Email, faker => faker.Person.Email)
            .RuleFor(x => x.FullName, faker => faker.Person.FullName)
            .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
            .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ValidGitHubUser);

        public CreateCustomerControllerTests(CustomerApiFactory apiFactory)
        {
            _customerApifactory = apiFactory;
            _httpClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Test()
        {
            await Task.Delay(5000);
        }

        [Fact]
        public async Task Create_WhenCalledWithvalidData_CreatesUser()
        {
            // Arrange

            var customer = _customerGenerator.Generate();
            await Task.Delay(5000);

            // Act

            var response = await _httpClient.PostAsJsonAsync("customers", customer);

            // Assert

            var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
            Assert.NotNull(customerResponse);
            customerResponse.Should().BeEquivalentTo(customer);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location!.ToString().Should()
                .Be("http;//localhost/customers/");
        }
    }
}