using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapyCareTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CapyCareTest.ViewModels.Pages
{
    public class EnclosureViewModel : INotifyPropertyChanged
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string? _size; // строка, как в модели
        public string? Size
        {
            get => _size;
            set { _size = value; OnPropertyChanged(nameof(Size)); }
        }

        private string? _location;
        public string? Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        private int _capacity; // приводим int? к int с дефолтом 0
        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(nameof(Capacity)); }
        }

        private int _capybaraCount;
        public int CapybaraCount
        {
            get => _capybaraCount;
            set { _capybaraCount = value; OnPropertyChanged(nameof(CapybaraCount)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    // Вью-модель страницы списка вольеров
    public class EnclosuresViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EnclosureViewModel> Enclosures { get; } = new();

        public EnclosuresViewModel()
        {
            _ = LoadEnclosuresAsync();
        }

        private async Task LoadEnclosuresAsync()
        {
            using var db = new CapyCareContext();

            var list = await db.Enclosures
                .Include(e => e.Capybaras)
                .ToListAsync();

            Enclosures.Clear();
            foreach (var e in list)
            {
                Enclosures.Add(new EnclosureViewModel
                {
                    Name = e.Name,
                    Size = e.Size,               // string?
                    Location = e.Location,
                    Capacity = e.Capacity ?? 0,      // int? → int
                    CapybaraCount = e.Capybaras.Count
                });
            }

            OnPropertyChanged(nameof(Enclosures));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
