using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace MySpot.Tests.EndToEnd.Controllers;

public class HomeControllerTests
{
    [Fact]
    public async Task get_base_endpoint_should_return_200_ok()
    {
        // ARRANGE
        var app = new MySpotTestApp();
        
        // ACT
        var response = await app.Client.GetAsync("");
        
        // ASSERT
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("MySpot API [test]");
    }
}