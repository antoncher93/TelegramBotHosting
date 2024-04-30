using System.Net;
using Microsoft.AspNetCore.Builder;

namespace Telegram.Bot.Hosting.Extensions;

public static class WebApplicationExtensions
{
    public static void UseTimeoutFallback(
        this WebApplication app)
    {
        app.Use(
            middleware: async (context, next) =>
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                var task = next(context);

                var resultTask = await Task.WhenAny(
                    task,
                    Task.Delay(Timeout.Infinite, cts.Token));

                if (resultTask == task)
                {
                    await task;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
            });
    }
    
    public static void UseExceptionHandling(
        this WebApplication app)
    {
        app.Use(
            middleware: async (context, @delegate) =>
        {
            try
            {
                await @delegate.Invoke(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
        });
    }
}