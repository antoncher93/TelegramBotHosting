using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TestApp;

public class AppSettingsProvider
{
    public long? GetChatId()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()
            .Get<AppConfig>();

        return config?.ChatId;
    }

    public void SetChatId(long chatId)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()
            .Get<AppConfig>();

        config.ChatId = chatId;
        
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        
        File.WriteAllText("appsettings.json", json);
    }
}