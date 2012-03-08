using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
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

    public partial class MailPage : PhoneApplicationPage
    {
        public MailPage()
        {
            InitializeComponent();

            if (App.ViewModel.Mail == null)
                App.ViewModel.Mail = new TopicViewModel();

            DataContext = App.ViewModel.Mail;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("type"))
            {
                int type = int.Parse(NavigationContext.QueryString["type"]);
                App.Service.Mail(type, App.ViewModel.Mail.Id, delegate(TopicViewModel mail, bool success, string error)
                {
                    LoadProgress.Visibility = Visibility.Collapsed;
                    LoadProgress.IsIndeterminate = false;
                    MailContent.Visibility = Visibility.Visible;
                    if (mail != null)
                        App.ViewModel.Mail.Content = mail.Content;
                });
            }
        }

        private void Reply_Click(object sender, EventArgs e)
        {
        }
    }
}