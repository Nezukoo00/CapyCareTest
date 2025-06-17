using CapyCareTest.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace CapyCareTest.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "CapyCareTest";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Dashboard",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem() { 
                Content = "Capybaras", 
                Icon = new SymbolIcon {Symbol = SymbolRegular.AnimalDog24 }, 
                TargetPageType = typeof(CapybarasPage) },
            new NavigationViewItem() { 
                Content = "Health Records", 
                Icon = new SymbolIcon {Symbol = SymbolRegular.ClipboardHeart24 }, 
                TargetPageType = typeof(HealthRecordsPage) },
            new NavigationViewItem() { 
                Content = "Add Health Record", 
                Icon = new SymbolIcon {Symbol = SymbolRegular.AddCircle24 }, 
                TargetPageType = typeof(AddHealthRecordPage) },
            new NavigationViewItem() { 
                Content = "Feeding Schedules", 
                Icon = new SymbolIcon {Symbol = SymbolRegular.Food24 }, 
                TargetPageType = typeof(FeedingSchedulesPage) },
            new NavigationViewItem() { 
                Content = "Enclosures", 
                Icon = new SymbolIcon {Symbol = SymbolRegular.HomeMore20 }, 
                TargetPageType = typeof(EnclosuresPage) },
            new NavigationViewItem() {
                Content = "CapybaraDetailsPage",
                Icon = new SymbolIcon {Symbol = SymbolRegular.HomeMore20 },
                TargetPageType = typeof(CapybaraDetailsPage) },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
