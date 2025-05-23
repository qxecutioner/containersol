﻿
using System.Text.Json;
using WebApplication4.Models;
using WebApplication4.Services.Interfaces;

namespace WebApplication4.Services
{
    public class ApiServices<T>(HttpClient httpClient) : IApiServices<T>
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<T> GetTransferData(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();

            using (var reader = new StreamReader(content))
            {
                var contentBody = reader.ReadToEnd();

                Console.WriteLine(contentBody);

                return JsonSerializer.Deserialize<T>(contentBody);
            }

        }
    }
}
