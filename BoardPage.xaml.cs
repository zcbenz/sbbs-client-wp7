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
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.NavigationContext.QueryString.ContainsKey("board"))
            {
                string board = this.NavigationContext.QueryString["board"];
                LoadMore.IsEnabled = false;

                // 跳转到其他版面时清空并重载
                if (board != App.ViewModel.CurrentBoard.EnglishName) {
                    // 重置标题
                    App.ViewModel.CurrentBoard.EnglishName = board;
                    App.ViewModel.CurrentBoard.Description = this.NavigationContext.QueryString["description"];

                    // 还原加载按钮
                    LoadMore.IsEnabled = true;

                    // 清空已有内容
                    if (App.ViewModel.CurrentBoard.Topics != null)
                        App.ViewModel.CurrentBoard.Topics.Clear();

                    // 重新加载
                    App.Service.Board(board, currentPage * pageSize, pageSize, delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
                    {
                        App.ViewModel.CurrentBoard.IsLoaded = true;
                        if (error == null)
                            App.ViewModel.CurrentBoard.Topics = topics;
                    });
                }
            }
        }

        private void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.CurrentBoard.IsLoaded = false;
            App.Service.Board(App.ViewModel.CurrentBoard.EnglishName, (currentPage + 1)* pageSize, pageSize, delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
            {
                App.ViewModel.CurrentBoard.IsLoaded = true;
                if (error == null)
                {
                    currentPage++;

                    // 判断后面时候还有内容
                    if (topics.Count < pageSize)
                        LoadMore.IsEnabled = false;

                    foreach (TopicViewModel topic in topics)
                    {
                        App.ViewModel.CurrentBoard.Topics.Add(topic);
                    }
                }
                else
                {
                    MessageBox.Show("网络错误");
                }
            });
        }
    }
}