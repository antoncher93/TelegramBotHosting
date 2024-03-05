using System.Net;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting;

public static class BotHostFactory
{
    public static IBotHost Create(
        string telegramBotToken,
        int port,
        Func<ITelegramBotClient, IBotFacade> botFacadeFactory,
        HttpMessageHandler? httpMessageHandler = default)
    {
        var client = new TelegramBotClient(
            token: telegramBotToken,
            httpClient: new HttpClient(
                handler: httpMessageHandler ?? new HttpClientHandler()));

        var botFacade = botFacadeFactory(client);
        var builder = WebApplication.CreateBuilder(
            args: new []{"--urls", $"http://*:{port}"});
        
        var app = builder.Build();

        app.MapGet("/healthcheck", _ => Task.CompletedTask);
        app.MapPost("/update",  async context =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var content = await reader.ReadToEndAsync();
            var update = JsonConvert.DeserializeObject<Update>(content);
            if (update != null)
            {
                await botFacade.OnUpdateAsync(update);
            }
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        });

        return new BotHost(app);
    }
}