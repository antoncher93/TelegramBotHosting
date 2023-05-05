using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting;

public static class BotHost
{
    private static Func<CancellationToken,Task>? _waitForShutDownAsync;

    public static Task StartAsync(
        int port,
        string telegramBotToken,
        Func<ITelegramBotClient, IBotFacade> botFacadeFactory,
        string host = "https://localhost")
    {
        var client = new TelegramBotClient(
            token: telegramBotToken);

        var botFacade = botFacadeFactory(client);
        var builder = WebApplication.CreateBuilder(
            args: new []{"--urls", $"{host}:{port}"});
        var app = builder.Build();

        app.MapGet("/api/healthcheck", _ => Task.CompletedTask);
        app.MapPost("/api/update",  async context =>
        {
            var content = await ReadContentFromRequestAsync(context.Request);
            var update = JsonConvert.DeserializeObject<Update>(content);
            await botFacade.OnUpdateAsync(update!);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        });

        app.UseHttpsRedirection();

        var task = app.StartAsync();
        _waitForShutDownAsync = app.WaitForShutdownAsync;
        return task;
    }

    private static async Task<string> ReadContentFromRequestAsync(
        HttpRequest contextRequest)
    {
        using var reader = new StreamReader(contextRequest.Body);
        return await reader.ReadToEndAsync();
    }

    public static Task WaitForShutdownAsync(
        CancellationToken token)
    {
        if (_waitForShutDownAsync != null)
        {
            return _waitForShutDownAsync(token);
        }

        return Task.CompletedTask;
    }
}