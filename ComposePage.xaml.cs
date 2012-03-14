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
    using Sbbs;

    public partial class ComposePage : PhoneApplicationPage
    {
        private int reid = 0;
        private LoadingViewModel viewModel = new LoadingViewModel();

        public ComposePage()
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.NavigationContext.QueryString.ContainsKey("user"))
            {
                string title = this.NavigationContext.QueryString["title"];
                if (title.Length > 3 && title.Substring(0, 3) == "Re:")
                    TitleText.Text = title;
                else
                    TitleText.Text = "Re: " + title;

                UserText.Text = this.NavigationContext.QueryString["user"];
                reid = int.Parse(this.NavigationContext.QueryString["reid"]);

                ContentText.Focus();
            }
            else
            {
                UserText.Focus();
            }
        }

        private void Compose_Click(object sender, EventArgs e)
        {
            if (viewModel.IsLoading) return;
            viewModel.IsLoading = true;

            App.Service.MailSend(UserText.Text, TitleText.Text, ContentText.Text, delegate(TopicViewModel mail, bool success, string error)
            {
                viewModel.IsLoading = false;

                if (!success)
                    MessageBox.Show("网络错误");
                else if (error != null)
                    MessageBox.Show(error);
                else
                    NavigationService.GoBack();
            });
        }
    }
}