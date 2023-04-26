using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Telegram.Bot.Hosting;

public static class BotHost
{
    private static Func<CancellationToken,Task>? _waitForShutDownAsync;

    public static Task StartAsync(
        string[] args,
        string telegramBotToken,
        Func<ITelegramBotClient, IBotFacade> botFacadeFactory)
    {
        var client = new TelegramBotClient(
            token: telegramBotToken);
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IBotFacade>(botFacadeFactory(client));
        
        var mvcBuilder = builder.Services.AddMvc();
        mvcBuilder.AddApplicationPart(Assembly.GetExecutingAssembly());

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet(
                pattern: "/",
                requestDelegate: _ => Task.FromResult("Hello World"));
            endpoints.MapControllers();
        });
        
        _waitForShutDownAsync = app.WaitForShutdownAsync;
        
        return app.StartAsync();
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