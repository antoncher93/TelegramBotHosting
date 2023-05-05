using System.Net;
using System.Text;

namespace Telegram.Bot.Hosting.Tests;

public class HttpMessageHandlerStub : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(
            statusCode: HttpStatusCode.OK);
        if (request.RequestUri!.AbsoluteUri.EndsWith("setWebhook"))
        {
            response.Content = new StringContent(
                content: "{\"ok\": true, \"result\": true, \"description\": \"Webhook was set\" }",
                encoding: Encoding.UTF8,
                mediaType: "application/json");
        }

        return Task.FromResult(
            result: response);
    }
}