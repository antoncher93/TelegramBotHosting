// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Hosting;
using TestApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (s, e) => cts.Cancel();

        var provider = new AppSettingsProvider();

        await BotHost.StartAsync(
            port: 10000,
            webhookHost: "https://1aaf-185-12-53-177.ngrok-free.app",
            telegramBotToken: "5692929074:AAEiBfoSy4CndyOU5kx3XZNpNQ3sPlbyAPc",
            botFacadeFactory: client => new BotFacade(provider),
            configureApp: app =>
            {
                app.MapGet("settings", async context =>
                {
                    var chatId = provider.GetChatId() ?? 0L;
                    await context.Response.WriteAsync($"{chatId}");
                });
            });
    
        await BotHost.WaitForShutdownAsync(cts.Token);

        Console.WriteLine("Application stopped");
    }
}