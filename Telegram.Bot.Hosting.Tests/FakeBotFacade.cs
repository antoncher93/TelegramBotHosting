using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Hosting.Tests;

public class FakeBotFacade : IBotFacade
{
    private readonly List<Message> _messages = new();
    private readonly List<CallbackQuery> _callbackQueries = new();
    private readonly List<Update> _defaultUpdates = new();
    private readonly bool _throwException;
    private readonly TimeSpan _delay;

    public FakeBotFacade(
        bool throwException,
        TimeSpan delay)
    {
        _throwException = throwException;
        _delay = delay;
    }

    public List<Message> GetReceivedMessages => _messages.ToList();

    public List<CallbackQuery> GetReceivedCallbackQueries => _callbackQueries.ToList();

    public List<Update> GetDefaultUpdates => _defaultUpdates.ToList();
    
    public async Task OnUpdateAsync(
        Update update)
    {
        if (_throwException)
        {
            throw new Exception("Some test exception was occured.");
        }

        await Task.Delay(_delay);
        
        var task = update.Type switch
        {
            UpdateType.Message => this.OnMessageAsync(message: update.Message!),
            UpdateType.CallbackQuery => this.OnCallbackQueryAsync(callbackQuery: update.CallbackQuery!),
            _ => this.OnDefaultUpdateAsync(update: update)
        };

        await task;
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