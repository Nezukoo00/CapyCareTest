using CapyCareTest.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Abstractions.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Wpf.Ui;
using CapyCareTest.Views.Pages;
using System.Drawing.Printing;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.Globalization;
using CapyCareTest.Views.Windows;
using System.Windows.Documents;

namespace CapyCareTest.ViewModels.Pages
{
    public partial class CapybaraDetailsViewModel : ObservableObject
    {
        private readonly CapyCareContext _db;
        private readonly INavigationService _nav;
        public IRelayCommand PrintCommand { get; }
        public IRelayCommand ExportToExcelCommand { get; }

        public ObservableCollection<Capybara> AllCapybaras { get; } = new();

        [ObservableProperty] private Capybara? selectedCapybara;
        [ObservableProperty] private HealthRecord? lastHealthRecord;
        [ObservableProperty] private ObservableCollection<FeedingSchedule> upcomingFeedings = new();
        [ObservableProperty] private ObservableCollection<CapybaraEventParticipation> eventParticipations = new();
        [ObservableProperty] private ObservableCollection<Visitor> recentVisitors = new();

        
        public IRelayCommand ViewAllHealthRecordsCommand { get; }

        
        public CapybaraDetailsViewModel(
            CapyCareContext db,
            INavigationService navigationService
        )
        {
            _db = db;
            _nav = navigationService;


            ViewAllHealthRecordsCommand = new RelayCommand(OpenAllHealthRecords);

            _ = LoadAllCapybarasAsync();

            PrintCommand = new RelayCommand(PrintFlowDocument);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
        }
        private void PrintFlowDocument()
        {
            // 1) Собираем FlowDocument
            var doc = new FlowDocument
            {
                PagePadding = new Thickness(40),
                ColumnGap = 0,
                ColumnWidth = 9999, // один столбец
            };

            var sel = SelectedCapybara!;
            void AddHeading(string text)
            {
                doc.Blocks.Add(new Paragraph(new Run(text))
                {
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 20, 0, 5)
                });
            }

            void AddLine(string label, string? value)
            {
                doc.Blocks.Add(new Paragraph(new Run($"{label}: {value}"))
                {
                    FontSize = 14,
                    Margin = new Thickness(0, 2, 0, 2)
                });
            }

            // Заголовок
            doc.Blocks.Add(new Paragraph(new Run($"Карточка капибары — {sel.Name}"))
            {
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // Общая информация
            AddHeading("Общая информация");
            AddLine("Пол", sel.Gender);
            AddLine("Дата рожд.", sel.BirthDate?.ToString("d"));
            AddLine("Прибыла", sel.ArrivalDate?.ToString("d"));
            AddLine("Вольер", sel.Enclosure?.Name);
            AddLine("Усыновитель", sel.Adopter?.FullName ?? "—");

            // Последний осмотр
            if (LastHealthRecord != null)
            {
                AddHeading("Последний осмотр");
                AddLine("Дата", LastHealthRecord.CheckDate?.ToString("d"));
                AddLine("Диагноз", LastHealthRecord.Diagnosis);
                AddLine("Лечение", LastHealthRecord.Treatment);
                AddLine("Ветеринар", LastHealthRecord.Vet?.Name);
            }

            // Ближайшие кормления
            if (UpcomingFeedings.Any())
            {
                AddHeading("Ближайшие кормления");
                foreach (var f in UpcomingFeedings)
                    AddLine(f.FeedingTime.ToString("g"), $"{f.FoodType} / {f.PortionSize}");
            }

            // События
            if (EventParticipations.Any())
            {
                AddHeading("Участие в событиях");
                foreach (var p in EventParticipations)
                    AddLine(p.Event?.EventName, p.Notes);
            }

            // Последние посетители
            if (RecentVisitors.Any())
            {
                AddHeading("Последние посетители");
                foreach (var v in RecentVisitors)
                    AddLine(v.Name, v.VisitDate?.ToString("d"));
            }

            // 2) Печать через PrintDialog
            var pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                // получить пейджинатор
                IDocumentPaginatorSource paginator = doc;
                pd.PrintDocument(paginator.DocumentPaginator, $"Capybara_{sel.Name}");
            }
        }

        private void ExportToExcel()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = $"Capybara_{SelectedCapybara?.Name}.xlsx"
            };

            if (dlg.ShowDialog() != true)
                return;

            // Устанавливаем лицензию
            ExcelPackage.License.SetNonCommercialPersonal("CapyCareTest");

            // Создаём и заполняем книгу
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("Details");

            // Заголовки
            ws.Cells[1, 1].Value = "Property";
            ws.Cells[1, 2].Value = "Value";

            int row = 2;
            void Write(string prop, object? val)
            {
                ws.Cells[row, 1].Value = prop;
                ws.Cells[row, 2].Value = val?.ToString();
                row++;
            }

            // Общая информация
            Write("Name", SelectedCapybara?.Name);
            Write("Gender", SelectedCapybara?.Gender);
            Write("BirthDate", SelectedCapybara?.BirthDate?.ToString("d"));
            Write("ArrivalDate", SelectedCapybara?.ArrivalDate?.ToString("d"));
            Write("Enclosure", SelectedCapybara?.Enclosure?.Name);
            Write("Adopter", SelectedCapybara?.Adopter?.FullName ?? "–");

            // Последний осмотр
            if (LastHealthRecord != null)
            {
                Write("", "");
                Write("Last Check", LastHealthRecord.CheckDate?.ToString("d"));
                Write("Diagnosis", LastHealthRecord.Diagnosis);
                Write("Treatment", LastHealthRecord.Treatment);
                Write("Vet", LastHealthRecord.Vet?.Name);
            }

            // Upcoming feedings
            if (UpcomingFeedings.Any())
            {
                Write("", "");
                Write("Upcoming feedings:", "");
                foreach (var f in UpcomingFeedings)
                    Write(f.FeedingTime.ToString("g"), $"{f.FoodType} / {f.PortionSize}");
            }

            // Events
            if (EventParticipations.Any())
            {
                Write("", "");
                Write("Events:", "");
                foreach (var p in EventParticipations)
                    Write(p.Event?.EventName, p.Notes);
            }

            // Visitors
            if (RecentVisitors.Any())
            {
                Write("", "");
                Write("Visitors:", "");
                foreach (var v in RecentVisitors)
                {
                    string dateStr = v.VisitDate.HasValue
                        ? v.VisitDate.Value.ToString("d", CultureInfo.CurrentCulture)
                        : string.Empty;

                    Write(v.Name, dateStr);
                }
            }

            var file = new FileInfo(dlg.FileName);
            pkg.SaveAs(file);

            MessageBox.Show("Экспорт завершён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        
        }
    
        private async Task LoadAllCapybarasAsync()
        {
            var list = await _db.Capybaras
         .Include(c => c.Enclosure)    
         .OrderBy(c => c.Name)
         .ToListAsync();

            AllCapybaras.Clear();
            foreach (var c in list)
                AllCapybaras.Add(c);
        }

        partial void OnSelectedCapybaraChanged(Capybara? value)
        {
            // Как только пользователь выбирает капибару, подгружаем детали
            if (value != null)
                _ = LoadDataAsync(value);
        }

        public async Task LoadDataAsync(Capybara capy)
        {
            SelectedCapybara = capy;

            LastHealthRecord = await _db.HealthRecords
                .Where(h => h.CapybaraId == capy.CapybaraId)
                .OrderByDescending(h => h.CheckDate)
                .Include(h => h.Vet)
                .FirstOrDefaultAsync();

            var now = DateTime.Now;
            var feedings = await _db.FeedingSchedules
                .Where(f => f.CapybaraId == capy.CapybaraId && f.FeedingTime >= now)
                .Include(f => f.ResponsibleEmployee)
                .OrderBy(f => f.FeedingTime)
                .Take(5)
                .ToListAsync();
            UpcomingFeedings = new ObservableCollection<FeedingSchedule>(feedings);

            var parts = await _db.CapybaraEventParticipations
                .Where(p => p.CapybaraId == capy.CapybaraId)
                .Include(p => p.Event)
                .ToListAsync();
            EventParticipations = new ObservableCollection<CapybaraEventParticipation>(parts);

            var visitors = await _db.Visitors
                .Where(v => v.CapybaraId == capy.CapybaraId)
                .OrderByDescending(v => v.VisitDate)
                .Take(5)
                .ToListAsync();
            RecentVisitors = new ObservableCollection<Visitor>(visitors);
        }

        private void OpenAllHealthRecords()
        {
            if (SelectedCapybara == null)
                return;

            // Навигация на HealthRecordsPage, передаём сам объект Capybara
            _ = _nav.Navigate(typeof(HealthRecordsPage), SelectedCapybara);
        }
    }
}
