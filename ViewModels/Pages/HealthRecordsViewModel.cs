using CapyCareTest.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CapyCareTest.ViewModels.Pages
{
   
    public class HealthRecordsViewModel : INotifyPropertyChanged
    {
        readonly CapyCareContext _db;
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnProp([CallerMemberName] string p = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        public List<HealthRecord> AllRecords { get; set; } = new();
        public List<HealthRecord> FilteredRecords { get; set; } = new();

        private string? _searchName;
        public string? SearchName
        {
            get => _searchName;
            set { _searchName = value; OnProp(); FilterRecords(); }
        }

        // Эти два свойства — DateTime? для DatePicker!
        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnProp(); FilterRecords(); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnProp(); FilterRecords(); }
        }
        public IRelayCommand<HealthRecord> DeleteRecordCommand { get; }

        public HealthRecordsViewModel(CapyCareContext db)
        {
            _db = db;
            DeleteRecordCommand = new RelayCommand<HealthRecord>(async rec => await DeleteRecordAsync(rec));
            _ = LoadAsync();
        }

        private async Task DeleteRecordAsync(HealthRecord toDelete)
        {
            if (toDelete == null) return;

            // Подтверждение удаления
            var result = MessageBox.Show(
                $"Удалить запись от {toDelete.CheckDate:dd.MM.yyyy} у {toDelete.Capybara?.Name}?",
                "Подтвердите удаление",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            // Удаляем из БД
            _db.HealthRecords.Remove(toDelete);
            await _db.SaveChangesAsync();

            // Удаляем из локальных коллекций
            AllRecords.Remove(toDelete);
            FilterRecords();
        }

        private async Task LoadAsync()
        {
            AllRecords = await _db.HealthRecords
                .Include(r => r.Capybara)
                .Include(r => r.Vet)
                .OrderByDescending(r => r.CheckDate)
                .ToListAsync();
            FilterRecords();
        }



        public void ResetFilters()
        {
            SearchName = null;
            StartDate = null;
            EndDate = null;
        }

        private void FilterRecords()
        {
            var q = AllRecords.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchName))
                q = q.Where(r => r.Capybara?.Name?.Contains(SearchName, StringComparison.OrdinalIgnoreCase) == true);

            if (StartDate.HasValue)
            {
                var d = DateOnly.FromDateTime(StartDate.Value);
                q = q.Where(r => r.CheckDate.HasValue && r.CheckDate.Value >= d);
            }

            if (EndDate.HasValue)
            {
                var d = DateOnly.FromDateTime(EndDate.Value);
                q = q.Where(r => r.CheckDate.HasValue && r.CheckDate.Value <= d);
            }

            FilteredRecords = q.ToList();
            OnProp(nameof(FilteredRecords));
        }

        public async Task LoadAllAsync()
        {
            AllRecords = await _db.HealthRecords
                .Include(r => r.Capybara)
                .Include(r => r.Vet)
                .OrderByDescending(r => r.CheckDate)
                .ToListAsync();
            FilterRecords();
        }

        public async Task LoadForCapybaraAsync(Guid capybaraId)
        {
            AllRecords = await _db.HealthRecords
                .Where(r => r.CapybaraId == capybaraId)
                .Include(r => r.Capybara)
                .Include(r => r.Vet)
                .OrderByDescending(r => r.CheckDate)
                .ToListAsync();
            FilterRecords();
        }

    }
}
