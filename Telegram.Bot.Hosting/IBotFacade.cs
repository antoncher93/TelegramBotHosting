using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting;

public interface IBotFacade
{
    Task OnMessageAsync(Message message);

    Task OnCallbackQueryAsync(CallbackQuery callbackQuery);

    Task OnDefaultUpdateAsync(Update update);
}