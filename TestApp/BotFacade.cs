using Telegram.Bot.Hosting;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TestApp;

public class BotFacade : IBotFacade
{
    private readonly AppSettingsProvider _provider;

    public BotFacade(AppSettingsProvider provider)
    {
        _provider = provider;
    }

    public Task OnUpdateAsync(Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            return this.OnMessageAsync(update.Message!);
        }

        return Task.CompletedTask;
    }

    private Task OnMessageAsync(Message message)
    {
        if (message.Text == "/start")
        {
            _provider.SetChatId(message.Chat.Id);
        }
        
        return Task.CompletedTask;
    }
}