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
    using Sbbs;

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
        }
        
        // Sbbs 接口
        private Sbbs.Service service = new Sbbs.Service();

        // 十大热帖
        // -- 热帖是否载入完毕
        private bool isToptenLoaded;
        public bool IsToptenLoaded {
            get
            {
                return isToptenLoaded;
            }
            set
            {
                isToptenLoaded = value;
                // 检查数据是否全部载入完毕
                ComputeDataLoaded();
            }
        }
        // -- 热帖集合
        private ObservableCollection<TopicViewModel> toptenItems;
        public ObservableCollection<TopicViewModel> ToptenItems
        {
            get
            {
                return toptenItems;
            }
            private set
            {
                if (value != toptenItems)
                {
                    toptenItems = value;
                    NotifyPropertyChanged("ToptenItems");
                }
            }
        }

        // 收藏夹
        private ObservableCollection<BoardViewModel> favoratesItems;
        public ObservableCollection<BoardViewModel> FavoratesItems
        {
            get
            {
                return favoratesItems;
            }
            private set
            {
                if (value != favoratesItems)
                {
                    favoratesItems = value;
                    NotifyPropertyChanged("FavoratesItems");
                }
            }
        }

        // 是否登陆
        public bool IsLogin
        {
            get
            {
                return service.Token != "";
            }
            private set
            {
                NotifyPropertyChanged("IsLogin");
            }
        }

        // 是否正在登录中
        private bool isLogining;
        public bool IsLogining
        {
            get
            {
                return isLogining;
            }
            private set
            {
                if (value != isLogining)
                {
                    isLogining = value;
                    NotifyPropertyChanged("IsLogining");
                }
            }
        }

        // 数据是否全部载入完毕
        private bool isDataLoaded;
        public bool IsDataLoaded
        {
            get
            {
                return isDataLoaded;
            }
            set
            {
                if (value != isDataLoaded)
                {
                    isDataLoaded = value;
                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }
        // 根据各项载入值计算是否全部载入
        private void ComputeDataLoaded()
        {
            IsDataLoaded = IsToptenLoaded;
        }

        public void LoadData()
        {
            // 载入十大
            service.Topten(delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                IsToptenLoaded = true;
                if (error != null)
                    return;

                // 刷新十大
                ToptenItems = topics;
            });
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