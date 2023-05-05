using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Hosting.Tests;

public class FakeBotFacade : IBotFacade
{
    private readonly List<Message> _messages = new();
    private readonly List<CallbackQuery> _callbackQueries = new();
    private readonly List<Update> _defaultUpdates = new();
    
    public List<Message> GetReceivedMessages => _messages.ToList();

    public List<CallbackQuery> GetReceivedCallbackQueries => _callbackQueries.ToList();

    public List<Update> GetDefaultUpdates => _defaultUpdates.ToList();
    
    public Task OnUpdateAsync(Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => this.OnMessageAsync(message: update.Message!),
            UpdateType.CallbackQuery => this.OnCallbackQueryAsync(callbackQuery: update.CallbackQuery!),
            _ => this.OnDefaultUpdateAsync(update: update)
        };
    }
    
    private Task OnMessageAsync(
        Message message)
    {
        _messages.Add(message);
        return Task.CompletedTask;
    }

    private Task OnCallbackQueryAsync(
        CallbackQuery callbackQuery)
    {
        _callbackQueries.Add(callbackQuery);
        return Task.CompletedTask;
    }

    private Task OnDefaultUpdateAsync(
        Update update)
    {
        _defaultUpdates.Add(update);
        return Task.CompletedTask;
    }
}