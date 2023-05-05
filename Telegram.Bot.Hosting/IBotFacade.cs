using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting;

public interface IBotFacade
{
    Task OnUpdateAsync(Update update);
}