using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public class MailboxViewModel : INotifyPropertyChanged
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

        // 收件箱
        private ObservableCollection<TopicViewModel> inboxItems;
        public ObservableCollection<TopicViewModel> InboxItems
        {
            get
            {
                return inboxItems;
            }
            set
            {
                if (value != inboxItems)
                {
                    inboxItems = value;
                    NotifyPropertyChanged("InboxItems");
                }
            }
        }

        // 发件箱
        private ObservableCollection<TopicViewModel> sentItems;
        public ObservableCollection<TopicViewModel> SentItems
        {
            get
            {
                return sentItems;
            }
            set
            {
                if (value != sentItems)
                {
                    sentItems = value;
                    NotifyPropertyChanged("SentItems");
                }
            }
        }

        // 垃圾箱
        private ObservableCollection<TopicViewModel> deletedItems;
        public ObservableCollection<TopicViewModel> DeletedItems
        {
            get
            {
                return deletedItems;
            }
            set
            {
                if (value != deletedItems)
                {
                    deletedItems = value;
                    NotifyPropertyChanged("DeletedItems");
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
