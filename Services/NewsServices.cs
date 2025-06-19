using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using CapyCareTest.Resources;
using CapyCareTest.Models;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CapyCareTest.Services
{
    public class NewsService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            DefaultRequestHeaders =
        {
            {"User-Agent", "CapyCareTest/1.0"}
        }
        };


        private const string ApiKey = "66cfddabb1564e53b2b26d97e09a3731";
        private readonly ILogger _logger;



        public async Task<IEnumerable<CapyNewsArticle>> GetRecentCapyNewsAsync()
        {
            try
            {

                // Формируем URL для запроса
                var url = $"https://newsapi.org/v2/everything?q=капибара&language=ru&pageSize=20&apiKey={ApiKey}";

                // Добавляем заголовки
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd("CapyNewsApp/1.0");
                request.Headers.Add("X-Api-Key", ApiKey);

                // Отправляем запрос
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Читаем ответ
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<NewsApiResponse>(json);

                return apiResponse.Articles.Select(a => new CapyNewsArticle
                {
                    Title = a.Title,
                    Description = a.Description,
                    ImageUrl = a.UrlToImage,
                    PublishedDate = a.PublishedAt,
                    Source = a.Source.Name,
                    ArticleUrl = a.Url
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex}");
                return new[]
                {
            new CapyNewsArticle
            {
                Title = "Пример новости 1",
                Description = "Это тестовая новость о капибарах",
                ImageUrl = "https://placekitten.com/300/200",
                Source = "Test News",
                PublishedDate = DateTime.Now
            },
            new CapyNewsArticle
            {
                Title = "Пример новости 2",
                Description = "Еще одна тестовая новость",
                ImageUrl = "https://placekitten.com/300/201",
                Source = "Test News",
                PublishedDate = DateTime.Now.AddDays(-1)
            }
        };
            }
        }
    }
}
