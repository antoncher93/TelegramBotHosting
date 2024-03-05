namespace Telegram.Bot.Hosting;

public interface IBotHost
{
    Task StartAsync(
        CancellationToken cancellationToken);

    Task WaitForShutdownAsync(
        CancellationToken cancellationToken);
}