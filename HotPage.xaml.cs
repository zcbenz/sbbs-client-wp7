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
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public partial class HotPage : PhoneApplicationPage
    {
        private bool isHotTopicsLoading;
        private bool isHotBoardsLoading;

        public HotPage()
        {
            InitializeComponent();

            if (App.ViewModel.Hot == null)
                App.ViewModel.Hot = new HotViewModel();
            DataContext = App.ViewModel.Hot;
        }

        // 进入页面时根据参数切换到指定的页面
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("type"))
            {
                int type = int.Parse(NavigationContext.QueryString["type"]);
                HotPivot.SelectedIndex = type;
            }
        }

        private void SetLoading()
        {
            App.ViewModel.Hot.IsLoading = isHotTopicsLoading | isHotBoardsLoading;
        }

        private void LoadHotTopics()
        {
            isHotTopicsLoading = true;
            SetLoading();
            App.Service.HotTopics(delegate(ObservableCollection<HotTopicsViewModel> topics, bool success, string error)
            {
                isHotTopicsLoading = false;
                SetLoading();

                if (topics != null)
                {
                    ObservableCollection<TopicsGroupViewModel> newGroup = new ObservableCollection<TopicsGroupViewModel>();
                    foreach (HotTopicsViewModel hot in topics)
                    {
                        TopicsGroupViewModel newItem = new TopicsGroupViewModel(hot.Description);
                        foreach (TopicViewModel topic in hot.Topics)
                            newItem.Add(topic);
                        newGroup.Add(newItem);
                    }

                    App.ViewModel.Hot.TopicsGroupItems = newGroup;
                }
            });
        }

        private void LoadHotBoards()
        {
            isHotBoardsLoading = true;
            SetLoading();
            App.Service.HotBoards(delegate(ObservableCollection<BoardViewModel> boards, bool success, string error)
            {
                isHotBoardsLoading = false;
                SetLoading();

                if (boards != null)
                    App.ViewModel.Hot.HotboardsItems = boards;
            });
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择，否则同样的项目无法点击第二次
                (sender as LongListSelector).SelectedItem = null;
                LongListSelector.LongListSelectorItem item = e.AddedItems[0] as LongListSelector.LongListSelectorItem;
                TopicViewModel topic = item.Item as TopicViewModel;

                // 清除未读
                topic.Unread = false;

                NavigationService.Navigate(
                    new Uri("/TopicPage.xaml?board=" + topic.Board + "&id=" + topic.Id + "&title=" + HttpUtility.UrlEncode(topic.Title), UriKind.Relative));
            }
        }

        private void Board_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
                BoardViewModel board = e.AddedItems[0] as BoardViewModel;

                this.NavigationService.Navigate(new Uri("/BoardPage.xaml?board=" + board.EnglishName + "&description=" + board.Description, UriKind.Relative));
            }
        }

        // 刷新按钮
        private void Refresh_Click(object sender, EventArgs e)
        {
            switch (HotPivot.SelectedIndex)
            {
                case 0:
                    LoadHotTopics();
                    break;
                case 1:
                    LoadHotBoards();
                    break;
            }
        }

        // 直到切换到页面时才只刷新必要的页面
        private void Pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            switch ((sender as Pivot).SelectedIndex)
            {
                case 0:
                    if (App.ViewModel.Hot.TopicsGroupItems == null)
                        LoadHotTopics();
                    break;
                case 1:
                    if (App.ViewModel.Hot.HotboardsItems == null)
                        LoadHotBoards();
                    break;
            }
        }
    }
}