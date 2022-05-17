using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySpot.Application.Commands;
using Shouldly;
using Xunit;

namespace MySpot.Tests.EndToEnd.Controllers;

[Collection("users-tests")]
public class UsersControllerTests
{
    [Fact]
    public async Task post_users_should_return_201_created()
    {
        var app = new MySpotTestApp();
        var command = new SignUp(Guid.NewGuid(), "user1@myspot.io", 
            "user1", "secret", "John Doe", "user");
        
        var response = await app.Client.PostAsJsonAsync("users", command);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    private readonly TestDatabase _testDatabase;
    
    public UsersControllerTests()
    {
        _testDatabase = new TestDatabase();
    }
}