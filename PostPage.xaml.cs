using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;

namespace sbbs_client_wp7
{
    using Sbbs;

    public partial class PostPage : PhoneApplicationPage
    {
        private string board;
        private int reid = 0;
        private LoadingViewModel viewModel = new LoadingViewModel();

        public PostPage()
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.NavigationContext.QueryString.ContainsKey("reid"))
            {
                string title = this.NavigationContext.QueryString["title"];
                if (title.Length > 3 && title.Substring(0, 3) == "Re:")
                    TitleText.Text = title;
                else
                    TitleText.Text = "Re: " + title;

                TypeTitle.Text = "回复";
                Board.Text = board = this.NavigationContext.QueryString["board"];
                reid = int.Parse(this.NavigationContext.QueryString["reid"]);
            }
            else
            {
                TypeTitle.Text = "发帖";
                Board.Text = board = this.NavigationContext.QueryString["board"];
            }
        }

        private void Post_Click(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;

            App.Service.TopicPost(board, reid, TitleText.Text, ContentText.Text, delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                viewModel.IsLoading = false;
                if (!success)
                    MessageBox.Show("网络错误");
                else if (error != null)
                    MessageBox.Show(error);
                else
                {
                    // 跳转到版面时标记刷新
                    if (reid == 0)
                        App.ViewModel.CurrentBoard.NeedRefresh = true;
                    // 跳转到话题时直接在最后添加
                    else
                        App.ViewModel.CurrentTopic.Topics.Add(topics[0]);

                    NavigationService.GoBack();
                }
            });
        }
    }
}