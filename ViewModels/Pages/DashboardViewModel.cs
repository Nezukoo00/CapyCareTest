using CapyCareTest.Data;
using CapyCareTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;



namespace CapyCareTest.ViewModels.Pages
{

    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;

        private int _capybaraCount;
        public int CapybaraCount
        {
            get => _capybaraCount;
            set { _capybaraCount = value; OnPropertyChanged(nameof(CapybaraCount)); }
        }

        private int _activeHealthIssuesCount;
        public int ActiveHealthIssuesCount
        {
            get => _activeHealthIssuesCount;
            set { _activeHealthIssuesCount = value; OnPropertyChanged(nameof(ActiveHealthIssuesCount)); }
        }

        private int _feedingTodayCount;
        public int FeedingTodayCount
        {
            get => _feedingTodayCount;
            set { _feedingTodayCount = value; OnPropertyChanged(nameof(FeedingTodayCount)); }
        }

        private int _noCheckupCapybarasCount;
        public int NoCheckupCapybarasCount
        {
            get => _noCheckupCapybarasCount;
            set { _noCheckupCapybarasCount = value; OnPropertyChanged(nameof(NoCheckupCapybarasCount)); }
        }

        private HealthRecord? _latestHealthRecord;
        public HealthRecord? LatestHealthRecord
        {
            get => _latestHealthRecord;
            set { _latestHealthRecord = value; OnPropertyChanged(nameof(LatestHealthRecord)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public DashboardViewModel()
        {
            // Сразу загрузим данные и запустим таймер
            _ = RefreshAsync();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _timer.Tick += async (s, e) => await RefreshAsync();
            _timer.Start();
        }

        private async Task RefreshAsync()
        {
            await LoadCountsAsync();
            await LoadLatestRecordAsync();
        }

        private async Task LoadCountsAsync()
        {
            using var db = new CapyCareContext();

            CapybaraCount = await db.Capybaras.CountAsync();

            // последние медицинские записи каждой капибары
            var latestPerCapy = await db.HealthRecords
                .GroupBy(h => h.CapybaraId)
                .Select(g => g.OrderByDescending(h => h.CheckDate).FirstOrDefault())
                .ToListAsync();

            ActiveHealthIssuesCount = latestPerCapy.Count(r => r != null && !string.IsNullOrWhiteSpace(r.Diagnosis));

            // Предположим, в FeedingSchedules есть поле CheckDate (или DateOnly) для даты кормления.
            // Если нет, то этот счётчик пропускаем или реализуем после добавления даты в модель.
            //FeedingTodayCount = await db.FeedingSchedules
            //    .CountAsync(f => f.FeedingTime.HasValue && DateOnly.FromDateTime(DateTime.Today) == DateOnly.FromTimeOnly(f.FeedingTime.Value));

            var threshold = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
            NoCheckupCapybarasCount = await db.Capybaras
                .Where(c =>
                    !db.HealthRecords
                        .Where(h => h.CapybaraId == c.CapybaraId)
                        .OrderByDescending(h => h.CheckDate)
                        .Select(h => h.CheckDate)
                        .Take(1)
                        .Any(d => d > threshold))
                .CountAsync();
        }

        private async Task LoadLatestRecordAsync()
        {
            using var db = new CapyCareContext();

            LatestHealthRecord = await db.HealthRecords
                .Include(h => h.Capybara)
                .Include(h => h.Vet)
                .OrderByDescending(h => h.CheckDate)
                .FirstOrDefaultAsync();
        }
    }


}

