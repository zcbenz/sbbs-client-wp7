using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace sbbs_client_wp7
{
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
