using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace sbbs_client_wp7.Sbbs
{
    [DataContract(Name = "topic")]
    public class TopicViewModel : INotifyPropertyChanged
    {
        private string title;
        private string content;
        private string author;
        private string board;
        private string quote;
        private string quoter;
        private int id;
        private int reid;
        private int time;
        private int size;
        private int replies;
        private int read;
        private bool unread;
        private bool top;
        private bool mark;

        [DataMember(Name = "title")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        [DataMember(Name = "content")]
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (value != content)
                {
                    content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        [DataMember(Name = "author")]
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                if (value != author)
                {
                    author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        [DataMember(Name = "board")]
        public string Board
        {
            get
            {
                return board;
            }
            set
            {
                if (value != board)
                {
                    board = value;
                    NotifyPropertyChanged("Board");
                }
            }
        }

        [DataMember(Name = "quote")]
        public string Quote
        {
            get
            {
                return quote;
            }
            set
            {
                if (value != quote)
                {
                    quote = value;
                    NotifyPropertyChanged("Quote");
                }
            }
        }

        [DataMember(Name = "quoter")]
        public string Quoter
        {
            get
            {
                return quoter;
            }
            set
            {
                if (value != quoter)
                {
                    quoter = value;
                    NotifyPropertyChanged("Quoter");
                }
            }
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        [DataMember(Name = "reid")]
        public int Reid
        {
            get
            {
                return reid;
            }
            set
            {
                if (value != reid)
                {
                    reid = value;
                    NotifyPropertyChanged("Reid");
                }
            }
        }

        [DataMember(Name = "time")]
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        [DataMember(Name = "size")]
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value != size)
                {
                    size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }

        [DataMember(Name = "replies")]
        public int Replies
        {
            get
            {
                return replies;
            }
            set
            {
                if (value != replies)
                {
                    replies = value;
                    NotifyPropertyChanged("Replies");
                    NotifyPropertyChanged("RepliesRead");
                }
            }
        }

        [DataMember(Name = "read")]
        public int Read
        {
            get
            {
                return read;
            }
            set
            {
                if (value != read)
                {
                    read = value;
                    NotifyPropertyChanged("Read");
                    NotifyPropertyChanged("RepliesRead");
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

        [DataMember(Name = "top")]
        public bool Top
        {
            get
            {
                return top;
            }
            set
            {
                if (value != top)
                {
                    top = value;
                    NotifyPropertyChanged("Top");
                }
            }
        }

        [DataMember(Name = "mark")]
        public bool Mark
        {
            get
            {
                return mark;
            }
            set
            {
                if (value != mark)
                {
                    mark = value;
                    NotifyPropertyChanged("Mark");
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
