using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting.Controllers;

[ApiController]
public class UpdateController : ControllerBase
{
    private readonly IBotFacade _botFacade;

    public UpdateController(
        IBotFacade botFacade)
    {
        _botFacade = botFacade;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody]Update update)
    {
        Console.WriteLine($"Update {update.Id} received");
        await update.Match(
            onMessage: _botFacade.OnMessageAsync,
            onCallbackQuery: _botFacade.OnCallbackQueryAsync,
            onDefault: () => _botFacade.OnDefaultUpdateAsync(update));

        return this.Ok();
    }
}