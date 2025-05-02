using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CapyCareTest.Data;
using Microsoft.EntityFrameworkCore;

namespace CapyCareTest.ViewModels.Pages
{
    public class FeedingSchedulesViewModel : INotifyPropertyChanged
    {
        private readonly CapyCareContext _db = new();

        public ObservableCollection<Capybara> Capybaras { get; set; } = new();
        public ObservableCollection<FeedingSchedule> FeedingSchedules { get; set; } = new();

        public ObservableCollection<string> TimeFilters { get; set; } = new() { "Все", "Сегодня", "Утро", "Вечер" };

        private Capybara? _selectedCapybara;
        public Capybara? SelectedCapybara
        {
            get => _selectedCapybara;
            set
            {
                _selectedCapybara = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string _selectedTimeFilter = "Все";
        public string SelectedTimeFilter
        {
            get => _selectedTimeFilter;
            set
            {
                _selectedTimeFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private ObservableCollection<FeedingSchedule> _filteredFeedingSchedules = new();
        public ObservableCollection<FeedingSchedule> FilteredFeedingSchedules
        {
            get => _filteredFeedingSchedules;
            set
            {
                _filteredFeedingSchedules = value;
                OnPropertyChanged();
            }
        }

        public FeedingSchedulesViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            var capybaras = await _db.Capybaras.ToListAsync();
            foreach (var c in capybaras)
                Capybaras.Add(c);

            var feeding = await _db.FeedingSchedules
                .Include(f => f.Capybara)
                .Include(f => f.ResponsibleEmployee)
                .ToListAsync();

            foreach (var f in feeding)
                FeedingSchedules.Add(f);

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = FeedingSchedules.AsEnumerable();

            if (SelectedCapybara != null)
                query = query.Where(f => f.CapybaraId == SelectedCapybara.CapybaraId);

            var now = DateTime.Now;

            query = SelectedTimeFilter switch
            {
                "Сегодня" => query.Where(f => f.FeedingTime.Date == now.Date),
                "Утро" => query.Where(f => f.FeedingTime.TimeOfDay < TimeSpan.FromHours(12)),
                "Вечер" => query.Where(f => f.FeedingTime.TimeOfDay >= TimeSpan.FromHours(18)),
                _ => query
            };

            FilteredFeedingSchedules = new ObservableCollection<FeedingSchedule>(query);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
