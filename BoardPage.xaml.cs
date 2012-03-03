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

                // 跳转到其他版面时清空并重载
                if (board != App.ViewModel.CurrentBoard.EnglishName) {
                    // 标题
                    App.ViewModel.CurrentBoard.EnglishName = board;
                    App.ViewModel.CurrentBoard.Description = this.NavigationContext.QueryString["description"];

                    App.ViewModel.CurrentBoard.Topics.Clear();

                    App.Service.Board(board, currentPage * pageSize, pageSize, delegate(ObservableCollection<TopicViewModel> topics, bool success, string error)
                    {
                        App.ViewModel.CurrentBoard.IsLoaded = true;
                        if (error == null)
                            App.ViewModel.CurrentBoard.Topics = topics;
                    });
                }
            }
        }
    }
}