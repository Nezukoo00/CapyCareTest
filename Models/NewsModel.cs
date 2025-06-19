using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapyCareTest.Models
{
    public class NewsModel : INotifyPropertyChanged
    {
        private int _totalArticles;
        private ObservableCollection<CapyNewsArticle> _articles;

        public ObservableCollection<CapyNewsArticle> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged(nameof(Articles));
            }
        }

        public int TotalArticles
        {
            get => _totalArticles;
            set
            {
                _totalArticles = value;
                OnPropertyChanged(nameof(TotalArticles));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
