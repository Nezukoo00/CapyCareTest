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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CapyCareTest.ViewModels.Pages;


namespace CapyCareTest.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnclosuresPage.xaml
    /// </summary>
    public partial class EnclosuresPage : Page
    {
        public EnclosuresPage()
        {
            InitializeComponent();
            DataContext = new EnclosuresViewModel();
        }

        private void ViewInBrowser_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://zoo.mos.ru/kapibara/",
                UseShellExecute = true
            });
        }
    }
}
