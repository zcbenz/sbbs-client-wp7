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

    public class CurrentBoardViewModel : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private ObservableCollection<BoardViewModel> topics;

        public string EnglishName
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("EnglishName");
                }
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<BoardViewModel> Topics
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
