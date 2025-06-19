using CapyCareTest.Services;
using CapyCareTest.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CapyCareTest.Views.Pages
{
    
    public partial class CapybaraNewsPage : Page
    {
        public CapybaraNewsPage(CapybaraNewsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private async void TestApi_Click(object sender, RoutedEventArgs e)
        {
            var service = new NewsService();
            var articles = await service.GetRecentCapyNewsAsync();

            MessageBox.Show($"Получено статей: {articles.Count()}\n" +
                            $"Первая статья: {articles.FirstOrDefault()?.Title}");
        }
    }


}
