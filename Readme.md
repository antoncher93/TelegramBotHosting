## Telegram Bot Hosting App

### Quick start for your telegram bot application in two steps

1. Use `IBotFacade` implementation to handle updates receiving by Webhook
```
        class BotFacade : IBotFacade
        {
            public Task OnUpdateAsync(Update update)
            {
                // Handle Bot Update here...
            }
        }
```

2. Start BotHost with your parameters
```
        var host = BotHostFactory.Create(
            telegramBotToken: <my_bot_token>,
            port: 5001,
            botFacadeFactory: client =>
            {
                client.SetWebhookAsync("https://<my_hostname>").Wait();
                return new BotFacade();
            });

        await host.RunAsync();
```