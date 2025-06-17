using CapyCareTest.Data;
using CapyCareTest.Views.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace CapyCareTest.ViewModels.Pages
{
    public partial class AddFeedingSchedulesViewModel : ObservableObject
    {
        private readonly CapyCareContext _db;

        public ObservableCollection<Capybara> Capybaras { get; } = new();

        [ObservableProperty] private Capybara? selectedCapybara;
        [ObservableProperty] private DateTime feedingDate = DateTime.Today;
        [ObservableProperty] private TimeSpan feedingTime = TimeSpan.FromHours(24);
        [ObservableProperty] private string foodType = string.Empty;
        [ObservableProperty] private string portionSize = string.Empty;
        private readonly INavigationService _nav;

        public IRelayCommand SaveCommand { get; }

        public AddFeedingSchedulesViewModel(CapyCareContext db, INavigationService nav)
        {
            _db = db;
            _nav = nav;
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            _ = LoadCapybarasAsync();
        }

        private async Task LoadCapybarasAsync()
        {
            var list = await _db.Capybaras.OrderBy(c => c.Name).ToListAsync();
            Capybaras.Clear();
            foreach (var c in list) Capybaras.Add(c);
        }

        private async Task SaveAsync()
        {
            if (SelectedCapybara == null
             || string.IsNullOrWhiteSpace(FoodType)
             || string.IsNullOrWhiteSpace(PortionSize))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dt = FeedingDate.Date + FeedingTime;

            var record = new FeedingSchedule
            {
                FeedingScheduleId = Guid.NewGuid(),
                CapybaraId = SelectedCapybara.CapybaraId,
                FoodType = FoodType,
                PortionSize = PortionSize,
                FeedingTime = dt,
                ResponsibleEmployeeId = null    
            };

            _db.FeedingSchedules.Add(record);


            await _db.SaveChangesAsync();

            MessageBox.Show("Кормление добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очистка формы
            SelectedCapybara = null;
            FeedingDate = DateTime.Today;
            FeedingTime = DateTime.Now.TimeOfDay;
            FoodType = string.Empty;
            PortionSize = string.Empty;

            _nav.Navigate(typeof(FeedingSchedulesPage));
        }
    }
}
