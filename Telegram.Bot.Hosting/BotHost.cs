using Microsoft.AspNetCore.Builder;

namespace Telegram.Bot.Hosting;

internal class BotHost : IBotHost
{
    private readonly WebApplication _app;

    public BotHost(
        WebApplication app)
    {
        _app = app;
    }

    public Task RunAsync()
    {
        return _app.RunAsync();
    }
}