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
using Wpf.Ui;
using CapyCareTest.Views.Pages;

namespace CapyCareTest.ViewModels.Pages
{
    public class FeedingSchedulesViewModel : INotifyPropertyChanged
    {
        readonly CapyCareContext _db = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnProp([CallerMemberName] string p = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        public ObservableCollection<Capybara> Capybaras { get; } = new();
        public ObservableCollection<FeedingSchedule> FeedingSchedules { get; } = new();
        public ObservableCollection<string> TimeFilters { get; } = new() { "Все", "Сегодня", "Утро", "Вечер" };

        private Capybara? _selectedCapybara;
        public Capybara? SelectedCapybara
        {
            get => _selectedCapybara;
            set { _selectedCapybara = value; OnProp(); ApplyFilters(); }
        }

        private string _selectedTimeFilter = "Все";
        public string SelectedTimeFilter
        {
            get => _selectedTimeFilter;
            set { _selectedTimeFilter = value; OnProp(); ApplyFilters(); }
        }

        private ObservableCollection<FeedingSchedule> _filteredFeedingSchedules = new();
        public ObservableCollection<FeedingSchedule> FilteredFeedingSchedules
        {
            get => _filteredFeedingSchedules;
            set { _filteredFeedingSchedules = value; OnProp(); }
        }

        public IRelayCommand AddScheduleCommand { get; }
        private readonly INavigationService _nav;

        public FeedingSchedulesViewModel(INavigationService nav)
        {
            _nav = nav;
            AddScheduleCommand = new RelayCommand(() => _nav.Navigate(typeof(AddFeedingSchedulesPage)));
            // убрали LoadData() из конструктора
        }

        public async Task LoadAsync()
        {
            Capybaras.Clear();
            foreach (var c in await _db.Capybaras.OrderBy(c => c.Name).ToListAsync())
                Capybaras.Add(c);

            FeedingSchedules.Clear();
            foreach (var f in await _db.FeedingSchedules
                        .Include(f => f.Capybara)
                        .Include(f => f.ResponsibleEmployee)
                        .ToListAsync())
                FeedingSchedules.Add(f);

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var q = FeedingSchedules.AsEnumerable();
            if (SelectedCapybara != null)
                q = q.Where(f => f.CapybaraId == SelectedCapybara.CapybaraId);

            var now = DateTime.Now;
            q = SelectedTimeFilter switch
            {
                "Сегодня" => q.Where(f => f.FeedingTime.Date == now.Date),
                "Утро" => q.Where(f => f.FeedingTime.TimeOfDay < TimeSpan.FromHours(12)),
                "Вечер" => q.Where(f => f.FeedingTime.TimeOfDay >= TimeSpan.FromHours(18)),
                _ => q
            };

            FilteredFeedingSchedules = new ObservableCollection<FeedingSchedule>(q);
        }
    }
}
