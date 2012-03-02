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
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;
            App.ViewModel.Login(username, password, delegate(string error)
            {
                if (error == null)
                {
                    App.ViewModel.IsLogin = true;
                    this.NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("用户名密码错误");
                }
            });
        }
    }
}