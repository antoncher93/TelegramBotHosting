using Telegram.Bot.Hosting;
using Telegram.Bot.Types;

namespace StudyApp;

public class MyBotFacade : IBotFacade
{
    public Task OnUpdateAsync(Update update)
    {
        return Task.CompletedTask;
    }
}