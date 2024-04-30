using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting;

public static class BotHostFactory
{
    public static IBotHost Create(
        string host,
        string telegramBotToken,
        Func<ITelegramBotClient, IBotFacade> botFacadeFactory,
        int port = 8080,
        HttpMessageHandler? httpMessageHandler = default)
    {
        var client = new TelegramBotClient(
            token: telegramBotToken,
            httpClient: new HttpClient(
                handler: httpMessageHandler ?? new HttpClientHandler()));

        client
            .SetWebhookAsync($"{host}/update")
            .GetAwaiter()
            .GetResult();

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

        app.MapGet("/photos/{fileId}", async (string fileId) => await DownloadFileAsync(client, fileId));

        return new BotHost(app);
    }

    private static async Task<object> DownloadFileAsync(
        TelegramBotClient client,
        string fileId)
    {
        var file = await client.GetFileAsync(fileId);

        try
        {
            using var stream = new MemoryStream();
        
            await client.DownloadFileAsync(
                filePath: file.FilePath!,
                destination: stream);

            var image = stream.ToArray();

            return Results.File(image, "image/jpeg");
        }
        catch (Exception e)
        {
            return Results.NotFound();
        }
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