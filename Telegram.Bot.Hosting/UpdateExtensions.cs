using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Hosting;

public static class UpdateExtensions
{
    public static T Match<T>(
        this Update update,
        Func<Message, T> onMessage,
        Func<CallbackQuery, T> onCallbackQuery,
        Func<T> onDefault)
    {
        return update.Type switch
        {
            UpdateType.Message => onMessage(update.Message!),
            UpdateType.CallbackQuery => onCallbackQuery(update.CallbackQuery!),
            _ => onDefault()
        }; 
    }
}