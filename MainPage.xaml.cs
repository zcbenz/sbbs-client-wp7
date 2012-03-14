using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace sbbs_client_wp7
{
    using Sbbs;
    using System.Collections.ObjectModel;
    using System.Windows.Threading;

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Add Tilt effect for Tile
            TiltEffect.TiltableItems.Add(typeof(Tile));

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // 登录后刷新收藏夹
            App.ViewModel.LoginChanged += delegate(object sender, bool isLogin)
            {
                LoadFavorates();
            };
        }

        // Load data for the ViewModel Items
        private bool isFirstLoading = true;
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // 启动时延迟2秒更新
            if (isFirstLoading)
            {
                isFirstLoading = false;

                // 延迟两秒后开始刷新全部
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += delegate(object s, EventArgs arg)
                {
                    App.ViewModel.IsDataLoaded = false; // 标记开始更新

                    LoadTopten();
                    LoadFavorates();
                    timer.Stop();
                };
                timer.Start();
            }
        }

        // 登录
        private void Login_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        // 注销
        private void Logout_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("真的要注销吗？", "注销", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                App.ViewModel.IsLogin = false;
        }

        // 刷新十大
        private void RefreshTopten_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.IsToptenLoaded = false;
            LoadTopten();
        }

        // 刷新收藏夹
        private void RefreshFavorates_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.IsFavoratesLoaded = false;
            LoadFavorates();
        }

        // 分区热点
        private void HotTopics_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HotPage.xaml?type=0", UriKind.RelativeOrAbsolute));
        }

        // 热门版面
        private void HotBoards_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HotPage.xaml?type=1", UriKind.RelativeOrAbsolute));
        }

        // 版面分区
        private void Sections_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HotPage.xaml?type=2", UriKind.RelativeOrAbsolute));
        }

        // 我的邮箱
        private void Mail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MailboxPage.xaml?type=0", UriKind.RelativeOrAbsolute));
        }

        // 搜索
        private void Search_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        // 浏览历史
        private void History_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        }

        // 关于
        private void About_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }

        // 点击收藏夹
        private void Favorates_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                BoardViewModel board = e.AddedItems[0] as BoardViewModel;
                
                this.NavigationService.Navigate(new Uri("/BoardPage.xaml?board=" + board.EnglishName + "&description=" + board.Description, UriKind.Relative));
            }
        }

        // 点击十大
        private void Topten_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                TopicViewModel topic = e.AddedItems[0] as TopicViewModel;

                this.NavigationService.Navigate(
                    new Uri("/TopicPage.xaml?board=" + topic.Board + "&id=" + topic.Id + "&title=" + HttpUtility.UrlEncode(topic.Title), UriKind.Relative));

                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
            }
        }

        // 载入收藏夹
        private void LoadFavorates()
        {
            MessageBox.Show("IsLogin: " + App.ViewModel.IsLogin);
            // 登录时载入收藏夹，未登陆时清空
            if (App.ViewModel.IsLogin)
            {
                App.Service.Favorates(delegate(ObservableCollection<BoardViewModel> boards, bool success, string error)
                {
                    App.ViewModel.IsFavoratesLoaded = true;
                    if (error != null)
                        return;

                    LocalCache.Set<ObservableCollection<BoardViewModel>>("Favorates", boards);
                    App.ViewModel.FavoratesItems = boards;
                });
            }
            else
            {
                App.ViewModel.IsFavoratesLoaded = true;
                LocalCache.Set<ObservableCollection<BoardViewModel>>("Favorates", null);
                App.ViewModel.FavoratesItems = null;
            }
        }

        // 载入十大
        public void LoadTopten()
        {
            App.Service.Topten(delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                App.ViewModel.IsToptenLoaded = true;
                if (error != null)
                    return;

                // 刷新十大
                LocalCache.Set<ObservableCollection<TopicViewModel>>("Topten", topics);
                App.ViewModel.ToptenItems = topics;
            });
        }
    }
}