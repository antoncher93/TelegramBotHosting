using System.Net;
using System.Net.Sockets;

namespace Telegram.Bot.Hosting.Tests;

public static class SutFactory
{
    public static async Task<Sut> CreateAsync()
    {
        var port = GetTcpPort();
        var baseAddress = $"http://localhost:{port}";
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseAddress),
        };

        var fakeBotFacade = new FakeBotFacade();

        var cts = new CancellationTokenSource();

        await BotHost.StartAsync(
            webhookHost: "https://webhook.my",
            port: port,
            telegramBotToken: Values.RandomString(),
            httpMessageHandler: new HttpMessageHandlerStub(),
            botFacadeFactory: _ => fakeBotFacade);

        var completionTask = BotHost.WaitForShutdownAsync(cts.Token);

        return new Sut(
            client: httpClient,
            fakeBotFacade: fakeBotFacade,
            dispose: () =>
            {
                cts.Cancel();
                completionTask.Wait();
            });
    }

    private static int GetTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}