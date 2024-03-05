using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Telegram.Bot.Hosting;

internal class BotHost : IBotHost
{
    private readonly WebApplication _app;
    private Func<CancellationToken,Task>? _waitForShutDownAsync;

    public BotHost(
        WebApplication app)
    {
        _app = app;
    }

    public Task WaitForShutdownAsync(CancellationToken cancellationToken)
    {
        if (_waitForShutDownAsync is null)
        {
            return Task.CompletedTask;
        }
        
        return _waitForShutDownAsync(cancellationToken);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var task = _app.StartAsync(cancellationToken);
        _waitForShutDownAsync = _app.WaitForShutdownAsync;
        return task;
    }
}