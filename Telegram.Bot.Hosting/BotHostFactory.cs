using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

        app.MapGet("/healthcheck",  async context => await BuildInfoAsync(context));
        
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

    private static async Task BuildInfoAsync(HttpContext httpContext)
    {
        var info = new
        {
            Status = "ok",
            Version = typeof(BotHost).Assembly.GetName().Version?.ToString(),
        };

        await httpContext.Response.WriteAsJsonAsync(info);
    }
}