using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;

namespace CapyCareTest.Models
{
    public class CapyNewsArticle
    {
        private string _imageUrl;

        public string ArticleUrl { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public BitmapImage ImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                    return CreatePlaceholderImage();

                try
                {
                    return new BitmapImage(new Uri(ImageUrl));
                }
                catch
                {
                    return CreatePlaceholderImage();
                }
            }
        }

        private static BitmapImage CreatePlaceholderImage()
        {
            return new BitmapImage(new Uri("https://via.placeholder.com/150?text=Капибара+Новости"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
