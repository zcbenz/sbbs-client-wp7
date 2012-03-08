using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Sbbs
{
    [DataContract]
    public class HotTopicsViewModel : INotifyPropertyChanged
    {
        private string description;
        private ObservableCollection<TopicViewModel> topics;
        
        [DataMember(Name = "description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        [DataMember(Name = "topics")]
        public ObservableCollection<TopicViewModel> Topics
        {
            get
            {
                return topics;
            }
            set
            {
                if (value != topics)
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

    public class TopicsGroupViewModel : ObservableCollection<TopicViewModel>
    {
        public TopicsGroupViewModel(string name)
        {
            this.Title = name;
        }

        public string Title { get; set; }
                        
        public bool HasItems
        {
            get
            {
                return Count != 0;
            }

            private set
            {
            }
        }
    }
}
