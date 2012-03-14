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
using System.Windows.Navigation;

namespace sbbs_client_wp7
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("reg"))
            {
                LoginPivot.SelectedIndex = 1;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.IsLogining = true;
            App.Service.Login(Username.Text, Password.Password, delegate(string token, bool success, string error)
            {
                App.ViewModel.IsLogining = false;
                if (error == null)
                {
                    // 保存获得的Token
                    App.Service.Token = token;
                    LocalCache.Set<string>("Token", token);

                    App.ViewModel.IsLogin = true;
                    this.NavigationService.GoBack();
                }
                else if (!success)
                    MessageBox.Show("网络错误");
                else
                    MessageBox.Show("用户名密码错误");
            });
        }
    }
}