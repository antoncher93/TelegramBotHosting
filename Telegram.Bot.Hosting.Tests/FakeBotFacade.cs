using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting.Tests;

public class FakeBotFacade : IBotFacade
{
    private readonly List<Message> _messages = new();
    private readonly List<CallbackQuery> _callbackQueries = new();
    private readonly List<Update> _defaultUpdates = new();
    
    public Task OnMessageAsync(
        Message message)
    {
        _messages.Add(message);
        return Task.CompletedTask;
    }

    public Task OnCallbackQueryAsync(
        CallbackQuery callbackQuery)
    {
        _callbackQueries.Add(callbackQuery);
        return Task.CompletedTask;
    }

    public Task OnDefaultUpdateAsync(
        Update update)
    {
        _defaultUpdates.Add(update);
        return Task.CompletedTask;
    }

    public List<Message> GetReceivedMessages => _messages.ToList();

    public List<CallbackQuery> GetReceivedCallbackQueries => _callbackQueries.ToList();

    public List<Update> GetDefaultUpdates => _defaultUpdates.ToList();
}