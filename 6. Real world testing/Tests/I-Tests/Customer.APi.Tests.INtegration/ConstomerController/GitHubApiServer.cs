using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customer.APi.Tests.Integration.ConstomerController
{
    public class GitHubApiServer : IDisposable
    {
        private WireMockServer _server;

        public string Url { get; internal set; }

        public void Start()
        {
            _server = WireMockServer.Start();
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }

        public void SetupUser(string username)
        {
            _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));
        }
    }
}