using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.Commands.Handlers;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Integration.Handlers;

public class SignUpHandlerTests : IDisposable
{
    private Task Act(SignUp command) => _handler.HandleAsync(command);

    [Fact]
    public async Task given_valid_command_user_should_be_created()
    {
        // ARRANGE
        await InitDatabaseAsync();
        var command = new SignUp(Guid.NewGuid(), "user1@myspot.io",
            "user1", "secret", "John Doe", "user");
        
        // ACT
        await Act(command);
        
        // ASSERT
        var user = await _testDatabase.Context
            .Users.SingleOrDefaultAsync(x => x.Id.Equals(command.UserId));

        user.ShouldNotBeNull();
    }

    private async Task InitDatabaseAsync()
    {
        await _testDatabase.Context.Database.MigrateAsync();
    }
    
    #region Arrange

    private TestDatabase _testDatabase;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IClock _clock;
    private ICommandHandler<SignUp> _handler;

    public SignUpHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _clock = new Clock();
        _passwordManager = new PasswordManager(new PasswordHasher<User>());
        _userRepository = new PostgresUserRepository(_testDatabase.Context);
        _handler = new SignUpHandler(_userRepository, _passwordManager, _clock);
    }

    #endregion

    public void Dispose()
    {
        _testDatabase?.Dispose();
    }
}