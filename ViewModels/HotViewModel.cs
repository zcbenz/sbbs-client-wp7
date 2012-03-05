using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public class HotViewModel : INotifyPropertyChanged
    {
        private bool isLoading;

        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    NotifyPropertyChanged("IsLoading");
                }
            }
        }

        // 热门版面集合
        private ObservableCollection<BoardViewModel> hotboardsItems;
        public ObservableCollection<BoardViewModel> HotboardsItems
        {
            get
            {
                return hotboardsItems;
            }
            set
            {
                if (value != hotboardsItems)
                {
                    hotboardsItems = value;
                    NotifyPropertyChanged("HotboardsItems");
                }
            }
        }

        // 分区热点
        private ObservableCollection<TopicsGroupViewModel> topicsGroupItems;
        public ObservableCollection<TopicsGroupViewModel> TopicsGroupItems
        {
            get
            {
                return topicsGroupItems;
            }
            set
            {
                if (value != topicsGroupItems)
                {
                    topicsGroupItems = value;
                    NotifyPropertyChanged("TopicsGroupItems");
                }
            }
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
