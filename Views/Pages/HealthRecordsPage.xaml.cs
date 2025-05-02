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
    /// Логика взаимодействия для HealthRecordsPage.xaml
    /// </summary>
    public partial class HealthRecordsPage : Page
    {
        private readonly HealthRecordsViewModel _vm;
        public HealthRecordsPage(HealthRecordsViewModel viewModel)
        {
            InitializeComponent();
            _vm = viewModel;
            DataContext = _vm;
        }

        private void OnResetFilters(object sender, RoutedEventArgs e)
        {
            _vm.ResetFilters();
        }
    }
}
