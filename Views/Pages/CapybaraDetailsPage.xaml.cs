using CapyCareTest.Data;
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
    /// <summary>
    /// Логика взаимодействия для CapybaraDetailsPage.xaml
    /// </summary>
    public partial class CapybaraDetailsPage : Page
    {
        private readonly CapybaraDetailsViewModel _vm;

        public CapybaraDetailsPage(CapybaraDetailsViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;
        }

        public async Task OnNavigatedToAsync(object parameter)
        {
            if (parameter is Capybara capy)
                await _vm.LoadDataAsync(capy);
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;
    }
}
