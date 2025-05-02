using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapyCareTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;
using CapyCareTest.Views.Pages;
using System.Diagnostics;


namespace CapyCareTest.ViewModels.Pages
{
    public partial class CapybarasViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<Capybara> Capybaras { get; } = new();
        public ObservableCollection<Enclosure> Enclosures { get; } = new();

        public ObservableCollection<string> Genders { get; } = new() { "Male", "Female" };

        public ObservableCollection<string> Statuses { get; } = new()
            {
                "Healthy",
                "Needs Checkup",
                "Under Observation"
            };

        public IRelayCommand LoadCapybarasCommand { get; }

        public CapybarasViewModel(INavigationService nav)
        {
            _navigationService = nav;
            LoadCapybarasCommand = new RelayCommand(async () => await LoadCapybarasAsync());

            _ = LoadFiltersAsync();
            _ = LoadCapybarasAsync();
        }

        private async Task LoadFiltersAsync()
        {
            using var context = new CapyCareContext();
            var enclosures = await context.Enclosures.OrderBy(e => e.Name).ToListAsync();

            App.Current.Dispatcher.Invoke(() =>
            {
                Enclosures.Clear();
                foreach (var enc in enclosures)
                    Enclosures.Add(enc);
            });
        }

        private string? _selectedGender;
        public string? SelectedGender
        {
            get => _selectedGender;
            set
            {
                if (SetProperty(ref _selectedGender, value))
                    _ = LoadCapybarasAsync();
            }
        }

        private string? _selectedStatus;
        public string? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                    _ = LoadCapybarasAsync();
            }
        }

        private Guid? _selectedEnclosureId;
        public Guid? SelectedEnclosureId
        {
            get => _selectedEnclosureId;
            set
            {
                if (SetProperty(ref _selectedEnclosureId, value))
                    _ = LoadCapybarasAsync();
            }
        }

        private bool _isLoading = false;

        private async Task LoadCapybarasAsync()
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                using var context = new CapyCareContext();

                var query = context.Capybaras
                    .Include(c => c.Enclosure)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(SelectedGender))
                    query = query.Where(c => c.Gender == SelectedGender);
                if (!string.IsNullOrWhiteSpace(SelectedStatus))
                    query = query.Where(c => c.Status == SelectedStatus);
                if (SelectedEnclosureId.HasValue)
                    query = query.Where(c => c.EnclosureId == SelectedEnclosureId.Value);

                Debug.WriteLine($"Filters: Gender={SelectedGender}, Status={SelectedStatus}, EnclosureId={SelectedEnclosureId}");

                var result = await query.ToListAsync();

                Debug.WriteLine($"Loaded {result.Count} capybaras");

                App.Current.Dispatcher.Invoke(() =>
                {
                    Capybaras.Clear();
                    foreach (var capy in result)
                        Capybaras.Add(capy);
                });
            }
            finally
            {
                _isLoading = false;
            }
        }

        
    }
}

