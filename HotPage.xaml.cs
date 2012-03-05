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
        private HotViewModel viewModel = new HotViewModel();
        private bool isHotTopicsLoading = true;

        public HotPage()
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("type"))
            {
                int type = int.Parse(NavigationContext.QueryString["type"]);
                switch (type)
                {
                    case 0:
                        if (viewModel.TopicsGroupItems == null)
                            LoadHotTopics();
                        break;
                }
            }
        }

        private void SetLoading()
        {
            viewModel.IsLoading = isHotTopicsLoading;
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

                    viewModel.TopicsGroupItems = newGroup;
                }
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
    }
}