using Customer.APi.Tests.Integration.ConstomerController;
using Customers.Api;
using Customers.Api.Database;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Testcontainers.PostgreSql;
using TestContainers.Container.Abstractions.Hosting;

namespace Customer.APi.Tests.INtegration.ConstomerController
{
    public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public const string ValidGitHubUser = "validuser";
        public const string ThrottledUser = "throttle";

        private readonly PostgreSqlContainer _dbcontainer =
            new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithEnvironment("POSTGRES_USER", "postgres")
                .WithEnvironment("POSTGRES_PASSWORD", "postgres")
                .WithEnvironment("POSTGRES_DB", "mydb")
                .WithPortBinding(5555, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();

        private readonly GitHubApiServer _gitHubApiServer = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.AddSingleton<IDbConnectionFactory>(_ =>
                  new NpgsqlConnectionFactory(_dbcontainer.GetConnectionString()));

                services.AddHttpClient("GitHub", httpclient =>
                    {
                        httpclient.BaseAddress = new Uri(_gitHubApiServer.Url);
                        httpclient.DefaultRequestHeaders.Add(
                            HeaderNames.Accept, "application/vnd.github.v3+json");
                        httpclient.DefaultRequestHeaders.Add(
                            HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
                    }

                );
            });
        }

        public async Task InitializeAsync()
        {
            _gitHubApiServer.Start();
            _gitHubApiServer.SetupUser("validuser");
            await _dbcontainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            _gitHubApiServer.Dispose();
            await _dbcontainer.DisposeAsync();
        }
    }
}