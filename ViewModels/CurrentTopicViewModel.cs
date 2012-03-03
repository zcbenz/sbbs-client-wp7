using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public class CurrentTopicViewModel : INotifyPropertyChanged
    {
        private string board;
        private string title;
        private bool isLoaded;
        private ObservableCollection<TopicViewModel> topics;

        public string Board
        {
            get
            {
                return board;
            }
            set
            {
                if (board != value)
                {
                    board = value;
                    NotifyPropertyChanged("Board");
                }
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public ObservableCollection<TopicViewModel> Topics
        {
            get
            {
                return topics;
            }
            set
            {
                if (topics != value)
                {
                    topics = value;
                    NotifyPropertyChanged("Topics");
                }
            }
        }

        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            set
            {
                if (isLoaded != value)
                {
                    isLoaded = value;
                    NotifyPropertyChanged("IsLoaded");
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
