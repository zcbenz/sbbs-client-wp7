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
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                LoadTopten();
                LoadFavorates();
            }
        }

        // 登录
        private void Login_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        // 注销
        private void Logout_Click(object sender, MouseButtonEventArgs e)
        {
            App.ViewModel.IsLogin = false;
        }

        // 点击收藏夹
        private void Favorates_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;

                BoardViewModel board = e.AddedItems[0] as BoardViewModel;
                // 收藏夹目录暂时不管
                if (board.Leaf != true)
                    return;

                this.NavigationService.Navigate(new Uri("/BoardPage.xaml?board=" + board.EnglishName + "&description=" + board.Description, UriKind.Relative));
            }
        }

        // 载入收藏夹
        private void LoadFavorates()
        {
            // 登录时载入收藏夹，未登陆时清空
            if (App.ViewModel.IsLogin)
            {
                App.Service.Favorates(delegate(ObservableCollection<BoardViewModel> boards, bool success, string error)
                {
                    App.ViewModel.IsFavoratesLoaded = true;
                    if (error != null)
                        return;

                    App.ViewModel.FavoratesItems = boards;
                });
            }
            else
            {
                App.ViewModel.IsFavoratesLoaded = true;
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
                App.ViewModel.ToptenItems = topics;
            });
        }
    }
}