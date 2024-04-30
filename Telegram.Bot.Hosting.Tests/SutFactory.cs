using System.Net;
using System.Net.Sockets;

namespace Telegram.Bot.Hosting.Tests;

public static class SutFactory
{
    public static async Task<Sut> CreateAsync(
        bool throwException = false,
        TimeSpan delay = default)
    {
        var port = GetTcpPort();
        var baseAddress = $"http://localhost:{port}";
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseAddress),
        };

        var fakeBotFacade = new FakeBotFacade(
            throwException: throwException,
            delay: delay);

        var cts = new CancellationTokenSource();

        var botHost = BotHostFactory.Create(
            host: "my.test.app",
            port: port,
            telegramBotToken: Values.RandomString(),
            httpMessageHandler: new HttpMessageHandlerStub(),
            botFacadeFactory: _ => fakeBotFacade);

        await Task.Factory.StartNew(() => botHost.RunAsync(), cts.Token);

        return new Sut(
            client: httpClient,
            fakeBotFacade: fakeBotFacade,
            dispose: () =>
            {
                cts.Cancel();
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