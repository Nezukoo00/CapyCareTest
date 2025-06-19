using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CapyCareTest.Resources;
using CapyCareTest.Models;
using CapyCareTest.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Web.WebView2.Wpf;




namespace CapyCareTest.ViewModels.Pages
{
    //AIzaSyAa-w7ZKeYFAV3Ej8CNH_9wZKC_sila6Pg
    //66cfddabb1564e53b2b26d97e09a3731
    public partial class CapybaraNewsViewModel : ObservableObject
    {
        

        private readonly NewsService _newsService;

        [ObservableProperty]
        private bool _isLoading = true;

        [ObservableProperty]
        private NewsModel _news = new NewsModel();


        [ObservableProperty]
        private string _debugInfo = "Загрузка...";

        [ObservableProperty]
        private bool _isDebugVisible = true;


        public ObservableCollection<CapyNewsArticle> NewsArticles { get; } = new();

        public CapybaraNewsViewModel(NewsService newsService)
        {
            _newsService = newsService;
            LoadNewsAsync();
        }

        private async Task LoadNewsAsync()
        {
            try
            {
                _isLoading = true;
                _debugInfo = "Начало загрузки новостей";
                NewsArticles.Clear();

                var articles = await _newsService.GetRecentCapyNewsAsync();
                _news.Articles = new ObservableCollection<CapyNewsArticle>(articles);
                _news.TotalArticles = _news.Articles.Count;

                _debugInfo += $"\nСтатей в коллекции: {News.Articles.Count}";
            }
            catch (Exception ex)
            {
                _debugInfo = $"Ошибка: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }


        [RelayCommand]
        private void OpenArticle(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    // Открываем URL в браузере по умолчанию
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при открытии статьи: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void OpenArticleInApp(string url)
        {
            var webViewWindow = new Window
            {
                Title = "Чтение статьи",
                Width = 1000,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var webView = new WebView2();
            webView.Source = new Uri(url);

            webViewWindow.Content = webView;
            webViewWindow.Show();
        }
    }
}


