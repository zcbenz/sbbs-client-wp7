using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace sbbs_client_wp7.Sbbs
{
    [DataContract(Name = "board")]
    public class BoardViewModel : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private bool unread;
        private uint users;
        private uint count;
        private bool leaf = true;

        [DataMember(Name = "name")]
        public string EnglishName
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

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

        [DataMember(Name = "unread")]
        public bool Unread
        {
            get
            {
                return unread;
            }
            set
            {
                if (value != unread)
                {
                    unread = value;
                    NotifyPropertyChanged("Unread");
                }
            }
        }

        [DataMember(Name = "users")]
        public uint Users
        {
            get
            {
                return users;
            }
            set
            {
                if (value != users)
                {
                    users = value;
                    NotifyPropertyChanged("Users");
                }
            }
        }

        [DataMember(Name = "count")]
        public uint Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        [DataMember(Name = "leaf")]
        public bool Leaf
        {
            get
            {
                return leaf;
            }
            set
            {
                if (value != leaf)
                {
                    leaf = value;
                    NotifyPropertyChanged("Leaf");
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
