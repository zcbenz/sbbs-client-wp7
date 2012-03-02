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
                App.ViewModel.LoadFavorates();
            };
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Login_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void Logout_Click(object sender, MouseButtonEventArgs e)
        {
            App.ViewModel.IsLogin = false;
        }
    }
}