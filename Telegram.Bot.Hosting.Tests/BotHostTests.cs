using FluentAssertions;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Hosting.Tests;

public class BotHostTests
{
    [Fact]
    public async Task ReceivesMessage()
    {
        using var sut = await SutFactory.CreateAsync();

        var message = RandomMessage();

        await sut.SendMessageAsync(message);

        sut.GetMessages()
            .Single()
            .Should()
            .BeEquivalentTo(message);
    }
    
    [Fact]
    public async Task ReceivesCallbackQuery()
    {
        using var sut = await SutFactory.CreateAsync();

        var callbackQuery = RandomCallbackQuery();

        await sut.SendCallbackQueryAsync(callbackQuery);

        sut.GetCallbackQueries()
            .Single()
            .Should()
            .BeEquivalentTo(callbackQuery);
    }
    
    [Fact]
    public async Task ReceivesDefaultUpdate()
    {
        using var sut = await SutFactory.CreateAsync();

        var update = RandomUpdate();

        await sut.SendUpdateAsync(update);

        sut.GetDefaultUpdates()
            .Single()
            .Should()
            .BeEquivalentTo(update);
    }

    private Update RandomUpdate()
    {
        return new Update()
        {
            Id = 1,
            ChatJoinRequest = new ChatJoinRequest()
            {
                Chat = RandomChat(),
                From = RandomUser(),
                Date = RandomDateTime(),
            }
        };
    }

    private static Message RandomMessage()
    {
        return new Message()
        {
            MessageId = 1,
            Text = "Random text",
            Date = RandomDateTime(),
            From = RandomUser(),
            Chat = RandomChat(),
        };
    }

    private static CallbackQuery RandomCallbackQuery()
    {
        return new CallbackQuery()
        {
            Id = Guid.NewGuid().ToString(),
            Message = RandomMessage(),
            Data = "TestData",
            From = RandomUser(),
            ChatInstance = "TestChatInstance",
        };
    }

    private static User RandomUser()
    {
        return new User()
        {
            Id = 15,
            FirstName = "TestName",
            Username = "testUserName",
        };
    }

    private static Chat RandomChat()
    {
        return new Chat()
        {
            Id = 52,
        };
    }

    private static DateTime RandomDateTime()
    {
        var utcNow = DateTime.UtcNow;
        return new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, DateTimeKind.Utc);
    }
}