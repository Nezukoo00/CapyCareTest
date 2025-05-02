using CapyCareTest.ViewModels.Pages;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace CapyCareTest.Views.Pages
{
    public partial class DashboardPage : Page
    {
        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
