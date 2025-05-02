using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapyCareTest.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using CapyCareTest.Services;

namespace CapyCareTest.ViewModels.Pages
{
    public partial class AddHealthRecordViewModel : ObservableObject
    {
        private readonly CapyCareContext _db;

        public ObservableCollection<Capybara> Capybaras { get; } = new();

        [ObservableProperty]
        private  Capybara? selectedCapybara;

        [ObservableProperty]
        private DateTime examinationDate = DateTime.Today;

        [ObservableProperty]
        private string? diagnosis;

        [ObservableProperty]
        private string? treatment;

        public Employee CurrentVet { get; set; }

        public IRelayCommand SaveCommand { get; }

        private readonly CurrentUserService _currentUser;

        public AddHealthRecordViewModel(CapyCareContext db, CurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;

            CurrentVet = _currentUser.CurrentEmployee!;


            LoadCapybaras();

            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        private async void LoadCapybaras()
        {
            var list = await _db.Capybaras.OrderBy(c => c.Name).ToListAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Capybaras.Clear();
                foreach (var capy in list)
                    Capybaras.Add(capy);
            });
        }

        private async Task SaveAsync()
        {
            if (SelectedCapybara == null || string.IsNullOrWhiteSpace(Diagnosis) || string.IsNullOrWhiteSpace(Treatment))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var record = new HealthRecord
            {
                CapybaraId = SelectedCapybara.CapybaraId,
                Diagnosis = Diagnosis,
                Treatment = Treatment,
                CheckDate = DateOnly.FromDateTime(ExaminationDate),
                VetId = CurrentVet.EmployeeId
            };

            _db.HealthRecords.Add(record);
            await _db.SaveChangesAsync();

            MessageBox.Show("Медицинская запись успешно добавлена.");
        }
    }
}
