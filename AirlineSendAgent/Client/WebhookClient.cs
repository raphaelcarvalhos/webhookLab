using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Client
{
    public class WebhookClient : IWebhookClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public WebhookClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task SendWebhookNotificationAsync(FlightDetailChangePayloadDto model)
        {
            Console.WriteLine("Webhook Client");
            var serializedPayload = JsonSerializer.Serialize(model);
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, model.WebhookURL);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedPayload);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                 using (var response = await httpClient.SendAsync(request))
                 {
                    Console.WriteLine("Mensagem enviada para Travel Agent.");
                    response.EnsureSuccessStatusCode();
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}.");
            }
        }
    }
}