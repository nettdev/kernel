using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Nett.SharedKernel.UnitTest;

public class MediatrExtensionsTest
{
    [Fact]
    public async Task TestName()
    {
        //Arrange
        var mediatorMock = new Moq.Mock<IMediator>();
        var context = new FakeConext();
        var userFake = new UserFakeContext{ Name = "Jeziel"};

        context.Users.Add(userFake);

        //Act
        await mediatorMock.Object.DispatchDomainEventsAsync(context);
    
        //Assert
        mediatorMock.Verify(v => v.Publish(It.IsAny<INotification>(), default(CancellationToken)));
    }
}

class FakeConext : DbContext
{
    public DbSet<UserFakeContext> Users => Set<UserFakeContext>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseInMemoryDatabase("FakeDataBase");
}

class UserFakeContext : Entity
{
    [Key]
    public string? Name { get; set; }

    public UserFakeContext() =>
        AddEvent(new EventTest());
}

public record EventTest : INotification;