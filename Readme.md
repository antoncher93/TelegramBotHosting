## Telegram Bot Hosting App

### Quick start for your telegram bot application
1. Use `IBotFacade` implementation to handle updates receiving by Webhook
```
public class MyBotFacade : IBotFacade
{
    public Task OnUpdateAsync(Update update)
    {
        // Handle some update from telegram bot here
    }
}
```

2. Start BotHost with your parameters
```
        await BotHost.StartAsync(
            port: myPort, // some free TCP/IP port 
            telegramBotToken: "myTelegramBotToken", // your telegram bot token
            webhookHost: "https://my-webhook", // hostname where the application will be available
            botFacadeFactory: client => new MyFacade(client));
```

3. Wait till application will be stopped
```
        await BotHost.WaitForShutdownAsync(cts.Token);
```