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
using Wpf.Ui.Abstractions.Controls;

namespace CapyCareTest.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для FeedingSchedulesPage.xaml
    /// </summary>
    public partial class FeedingSchedulesPage : Page, INavigationAware
    {
        private readonly FeedingSchedulesViewModel _vm;

        public FeedingSchedulesPage(FeedingSchedulesViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;
        }

        // Будет вызвано каждый раз при навигации *на* эту страницу
        public async Task OnNavigatedToAsync()
        {
            await _vm.LoadAsync();
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;
    }
}
