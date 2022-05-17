using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;
using Shouldly;
using Xunit;

namespace MySpot.Tests.EndToEnd.Controllers;

[Collection("users-tests")]
public class UsersControllerTests
{
    [Fact]
    public async Task post_users_should_return_201_created()
    {
        var userRepository = new TestUserRepository();
        var app = new MySpotTestApp(services =>
        {
            services.AddSingleton<IUserRepository>(userRepository);
        });
        
        var command = new SignUp(Guid.NewGuid(), "user1@myspot.io", 
            "user1", "secret", "John Doe", "user");

        var response = await app.Client.PostAsJsonAsync("users", command);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task post_sign_in_should_return_ok_200_status_code_and_jwt()
    {
        // Arrange
        var app = new MySpotTestApp();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        var clock = new Clock();
        const string password = "secret";
    
        var user = new User(Guid.NewGuid(), "test-user1@myspot.io",
            "test-user1", passwordManager.Secure(password), "Test Doe", Role.User(), clock.Current());
        await _testDatabase.Context.Users.AddAsync(user);
        await _testDatabase.Context.SaveChangesAsync();

        // Act
        var command = new SignIn(user.Email, password);
        var response = await app.Client.PostAsJsonAsync("users/sign-in", command);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();

        // Assert
        jwt.ShouldNotBeNull();
        jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    private readonly TestDatabase _testDatabase;
    
    public UsersControllerTests()
    {
        _testDatabase = new TestDatabase();
    }
}