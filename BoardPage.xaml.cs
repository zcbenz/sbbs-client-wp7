using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public partial class BoardPage : PhoneApplicationPage
    {
        // 每页显示多少话题
        const int pageSize = 10;
        // 当前页数
        int currentPage = 0;

        public BoardPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel.CurrentBoard;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 从发帖页面返回，且需要刷新
            if (App.ViewModel.CurrentBoard.NeedRefresh)
            {
                App.ViewModel.CurrentBoard.NeedRefresh = false;
                Refresh_Click(null, null);
            }
            // 浏览版面
            else if (this.NavigationContext.QueryString.ContainsKey("board"))
            {
                string board = this.NavigationContext.QueryString["board"];
                // 跳转到其他版面时清空并重载
                if (board != App.ViewModel.CurrentBoard.EnglishName) {
                    // 重置标题
                    App.ViewModel.CurrentBoard.EnglishName = board;
                    App.ViewModel.CurrentBoard.Description = this.NavigationContext.QueryString["description"];

                    // 清空已有内容
                    if (App.ViewModel.CurrentBoard.Topics != null)
                        App.ViewModel.CurrentBoard.Topics.Clear();

                    currentPage = 0;
                    LoadTopics();
                }
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            LoadTopics();
        }

        private void NewPost_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PostPage.xaml?board=" + App.ViewModel.CurrentBoard.EnglishName, UriKind.Relative));
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BoardSettingsPage.xaml", UriKind.Relative));
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            App.Service.BoardMarkRead(App.ViewModel.CurrentBoard.EnglishName);
            foreach (TopicViewModel topic in App.ViewModel.CurrentBoard.Topics)
            {
                topic.Unread = false;
            }
        }

        private void TopicsList_NextPage(object sendor, NextPageEventArgs e)
        {
            LoadTopics(true);
        }

        private void TopicsList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
                TopicViewModel topic = e.AddedItems[0] as TopicViewModel;

                // 清除未读
                topic.Unread = false;

                this.NavigationService.Navigate(
                    new Uri("/TopicPage.xaml?board=" + topic.Board + "&id=" + topic.Id + "&title=" + HttpUtility.UrlEncode(topic.Title), UriKind.Relative));
            }
        }

        private void LoadTopics(bool append = false)
        {
            if (App.ViewModel.CurrentBoard.IsLoading)
                return;

            App.ViewModel.CurrentBoard.IsLoading = true;

            // 重新加载
            int page = append ? currentPage + 1 : currentPage;
            App.Service.Board(App.ViewModel.CurrentBoard.EnglishName, page * pageSize, pageSize, delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                App.ViewModel.CurrentBoard.IsLoading = false;

                // 判断后面是否还有内容
                TopicsList.IsFullyLoaded = error == null && topics.Count < pageSize;

                if (error == null)
                    // 重置还是添加
                    if (append)
                    {
                        ++currentPage;
                        foreach (TopicViewModel topic in topics)
                            App.ViewModel.CurrentBoard.Topics.Add(topic);

                        // 叠加完毕时往后翻页

                    }
                    else
                    {
                        App.ViewModel.CurrentBoard.Topics = topics;
                    }
                else
                    MessageBox.Show("网络错误");
            });
        }
    }
}