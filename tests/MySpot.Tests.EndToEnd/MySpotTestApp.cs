using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MySpot.Tests.EndToEnd;

internal class MySpotTestApp : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }
    
    public MySpotTestApp()
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("test");
        }).CreateClient();
    }
}