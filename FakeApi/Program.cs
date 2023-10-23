using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FakeApi;

internal class Program
{
    private static void Main(string[] args)
    {
        var wiremockServer = WireMockServer.Start();

        Console.WriteLine($"Wiremock is now running on : {wiremockServer.Url}");

        wiremockServer.Given(Request.Create()
                .WithPath("/example")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody("This is comping from wireMock")
                .WithStatusCode(HttpStatusCode.OK));

        Console.ReadKey();
        wiremockServer.Dispose();
    }
}