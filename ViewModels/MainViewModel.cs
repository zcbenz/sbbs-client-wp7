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
            private set
            {
                if (value != favoratesItems)
                {
                    favoratesItems = value;
                    NotifyPropertyChanged("FavoratesItems");
                }
            }
        }

        // 是否已经登陆
        private bool isLogin;
        public bool IsLogin
        {
            get
            {
                return isLogin;
            }
            set
            {
                if (isLogin != value)
                {
                    isLogin = value;

                    // 启动所有登录钩子
                    LoginChanged(this, isLogin);

                    // 注销时清除Token
                    if (isLogin == false)
                    {
                        service.Token = null;
                    }

                    NotifyPropertyChanged("IsLogin");
                }
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
        // 登录
        public void Login(string username, string password, Action<string> callback)
        {
            IsLogining = true;
            service.Login(username, password, delegate(string token, bool success, string error)
            {
                IsLogining = false;

                if (error == null)
                    service.Token = token;

                callback(error);
            });
        }
        // 登录钩子
        public delegate void LoginChangedHandler(object sender, bool isLogin);
        public event LoginChangedHandler LoginChanged;

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
            IsDataLoaded = IsToptenLoaded && IsFavoratesLoaded;
        }

        public void LoadData()
        {
            LoadTopten();
            LoadFavorates();
        }

        // 载入十大
        public void LoadTopten()
        {
            service.Topten(delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                IsToptenLoaded = true;
                if (error != null)
                    return;

                // 刷新十大
                ToptenItems = topics;
            });
        }

        // 载入收藏夹
        public void LoadFavorates()
        {
            if (IsLogin)
            {
                service.Favorates(delegate(ObservableCollection<BoardViewModel> boards, bool success, string error)
                {
                    IsFavoratesLoaded = true;
                    if (error != null)
                        return;

                    FavoratesItems = boards;
                });
            }
            else
            {
                IsFavoratesLoaded = true;
                FavoratesItems = new ObservableCollection<BoardViewModel>();
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