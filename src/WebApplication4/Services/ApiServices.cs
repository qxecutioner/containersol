
using System.Text.Json;
using WebApplication4.Models;
using WebApplication4.Services.Interfaces;

namespace WebApplication4.Services
{
    public class ApiServices(HttpClient httpClient) : IApiServices
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<TransferData> GetTransferData(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();

            return JsonSerializer.Deserialize<TransferData>(content);
        }
    }
}
