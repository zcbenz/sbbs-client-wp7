using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace sbbs_client_wp7
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.ToptenItems = new ObservableCollection<TopicViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<TopicViewModel> ToptenItems { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 10, Read = 20 });
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 10, Read = 20 });
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 12, Read = 20 });
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 10, Read = 20 });
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 10, Read = 20 });
            this.ToptenItems.Add(new TopicViewModel { Title = "Test", Content = "COntent", Author = "Author", Replies = 10, Read = 20 });

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}