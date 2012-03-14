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
            // 载入保存的设置
            App.Service.Token = LocalCache.Get<string>("Token");
            App.Service.BoardMode = LocalCache.Get<int>("BoardMode", 2);

            // 初始化
            CurrentBoard = new CurrentBoardViewModel();
            CurrentTopic = new CurrentTopicViewModel();
        }
        
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
            set
            {
                if (value != toptenItems)
                {
                    toptenItems = value;
                    NotifyPropertyChanged("ToptenItems");
                }
            }
        }

        // 收藏夹
        // -- 收藏夹是否载入完毕
        private bool isFavoratesLoaded;
        public bool IsFavoratesLoaded
        {
            get
            {
                return isFavoratesLoaded;
            }
            set
            {
                isFavoratesLoaded = value;
                // 检查数据是否全部载入完毕
                ComputeDataLoaded();
            }
        }
        // -- 收藏夹集合
        private ObservableCollection<BoardViewModel> favoratesItems;
        public ObservableCollection<BoardViewModel> FavoratesItems
        {
            get
            {
                return favoratesItems;
            }
            set
            {
                if (value != favoratesItems)
                {
                    favoratesItems = value;
                    NotifyPropertyChanged("FavoratesItems");
                }
            }
        }

        // 当前版面
        public CurrentBoardViewModel CurrentBoard { get; set; }
        // 当前话题
        public CurrentTopicViewModel CurrentTopic { get; set; }
        // 热门页面
        public HotViewModel Hot { get; set; }
        // 邮箱页面
        public MailboxViewModel Mailbox { get; set; }
        // 邮件
        public TopicViewModel Mail { get; set; }

        // 是否已经登陆
        public bool IsLogin
        {
            get
            {
                return App.Service.Token != null && App.Service.Token != "";
            }
            set
            {
                // 启动所有登录钩子
                LoginChanged(this, value);

                // 注销时清除Token
                if (value == false)
                {
                    LocalCache.Set<string>("Token", null);
                    App.Service.Token = null;
                }

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
            set
            {
                if (value != isLogining)
                {
                    isLogining = value;
                    NotifyPropertyChanged("IsLogining");
                }
            }
        }
        // 登录钩子
        public delegate void LoginChangedHandler(object sender, bool isLogin);
        public event LoginChangedHandler LoginChanged;

        // 数据是否全部载入完毕
        private bool isDataLoaded = true; // 初始时假装更新完毕，进入程序后再开始更新
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
            IsDataLoaded = IsToptenLoaded && IsFavoratesLoaded;
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