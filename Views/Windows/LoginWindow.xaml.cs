using CapyCareTest.Data;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace CapyCareTest.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly CapyCareContext _db;

        public Employee? LoggedInEmployee { get; private set; }

        public LoginWindow(CapyCareContext db)
        {
            InitializeComponent();
            _db = new CapyCareContext();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string passwordInput = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwordInput))
            {
                MessageBox.Show("Пожалуйста, введите имя и дату найма.");
                return;
            }

            if (!DateOnly.TryParse(passwordInput, out var hireDate))
            {
                MessageBox.Show("Неверный формат даты. Используйте формат ГГГГ-ММ-ДД.");
                return;
            }

            LoggedInEmployee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Name == username && e.HireDate == hireDate);

            if (LoggedInEmployee != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Неверные имя или дата найма.");
            }
           


        }

        
    }
}
