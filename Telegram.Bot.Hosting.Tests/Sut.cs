using System.Text;
using Telegram.Bot.Types;

namespace Telegram.Bot.Hosting.Tests;

public class Sut : IDisposable
{
    private readonly HttpClient _client;
    private readonly Action _dispose;
    private readonly FakeBotFacade _fakeBotFacade;

    public Sut(
        HttpClient client,
        Action dispose,
        FakeBotFacade fakeBotFacade)
    {
        _client = client;
        _dispose = dispose;
        _fakeBotFacade = fakeBotFacade;
    }

    public Task SendMessageAsync(
        Message message)
    {
        var update = new Update()
        {
            Id = 1,
            Message = message,
        };
        
        return this.SendUpdateAsync(update);
    }
    
    public Task SendCallbackQueryAsync(
        CallbackQuery callbackQuery)
    {
        var update = new Update()
        {
            Id = 1,
            CallbackQuery = callbackQuery,
        };
        
        return this.SendUpdateAsync(update);
    }

    public Task<HttpResponseMessage> SendUpdateAsync(
        Update update)
    {
        var request = new HttpRequestMessage(
            method: HttpMethod.Post,
            requestUri: "/update");

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(update);
        
        request.Content = new StringContent(
            content: json,
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        return _client.SendAsync(request);
    }

    public List<Message> GetMessages() => _fakeBotFacade.GetReceivedMessages;

    public List<CallbackQuery> GetCallbackQueries() => _fakeBotFacade.GetReceivedCallbackQueries;

    public List<Update> GetDefaultUpdates() => _fakeBotFacade.GetDefaultUpdates;

    public void Dispose()
    {
        _client.Dispose();
        _dispose();
    }
}