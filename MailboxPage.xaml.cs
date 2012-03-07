using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace sbbs_client_wp7
{
    using Sbbs;

    public partial class MailboxPage : PhoneApplicationPage
    {
        // 每页显示多少邮件
        const int pageSize = 10;
        // 当前页数
        int currentPage = 0;

        // 载入进度
        private bool[] isLoading = { false, false, false };
        private void SetLoading()
        {
            App.ViewModel.Mailbox.IsLoading = isLoading[0] | isLoading[1] | isLoading[2];
        }

        // 载入按钮集合
        Button[] LoadMore = { null, null, null };

        public MailboxPage()
        {
            InitializeComponent();

            if (App.ViewModel.Mailbox == null)
                App.ViewModel.Mailbox = new MailboxViewModel();

            DataContext = App.ViewModel.Mailbox;

            LoadMore[0] = LoadMore1;
            LoadMore[1] = LoadMore2;
            LoadMore[2] = LoadMore3;
        }

        // 进入页面时根据参数切换到指定的页面
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("type"))
            {
                int type = int.Parse(NavigationContext.QueryString["type"]);
                MailboxPivot.SelectedIndex = type;
            }
        }

        // 直到切换到页面时才只刷新必要的页面
        private void Pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            int type = (sender as Pivot).SelectedIndex;
            switch (type)
            {
                case 0:
                    if (App.ViewModel.Mailbox.InboxItems != null)
                        return;
                    break;
                case 1:
                    if (App.ViewModel.Mailbox.SentItems != null)
                        return;
                    break;
                case 2:
                    if (App.ViewModel.Mailbox.DeletedItems != null)
                        return;
                    break;
            }
            LoadMailbox(type);
        }

        // 选中邮件
        private void Mail_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
            }
        }

        // 加载指定的信箱
        private void LoadMailbox(int type, bool append = false)
        {
            isLoading[type] = true;
            SetLoading();
            int loadPage = append ? currentPage + 1 : currentPage;
            App.Service.MailBox(type, loadPage * pageSize, pageSize, delegate(ObservableCollection<TopicViewModel> mails, bool success, string error)
            {
                isLoading[type] = false;
                SetLoading();

                if (mails != null)
                {
                    // 直接覆盖
                    if (!append)
                    {
                        switch (type)
                        {
                            case 0:
                                App.ViewModel.Mailbox.InboxItems = mails;
                                break;
                            case 1:
                                App.ViewModel.Mailbox.SentItems = mails;
                                break;
                            case 2:
                                App.ViewModel.Mailbox.DeletedItems = mails;
                                break;
                        }
                    }
                    // 或者接在后面
                    else
                    {
                        switch (type)
                        {
                            case 0:
                                foreach (TopicViewModel mail in mails)
                                    App.ViewModel.Mailbox.InboxItems.Add(mail);
                                break;
                            case 1:
                                foreach (TopicViewModel mail in mails)
                                    App.ViewModel.Mailbox.SentItems.Add(mail);
                                break;
                            case 2:
                                foreach (TopicViewModel mail in mails)
                                    App.ViewModel.Mailbox.DeletedItems.Add(mail);
                                break;
                        }
                        ++currentPage;
                    }

                    // 判断是否显示下一页
                    if (mails.Count < pageSize)
                    {
                        LoadMore[type].Visibility = Visibility.Collapsed;
                        LoadMore[type].IsEnabled = false;
                    }
                    else
                    {
                        LoadMore[type].Visibility = Visibility.Visible;
                        LoadMore[type].IsEnabled = true;
                    }
                }
            });
        }

        // 刷新按钮
        private void Refresh_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            LoadMailbox(MailboxPivot.SelectedIndex);
        }

        // 载入更多
        private void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            LoadMailbox(MailboxPivot.SelectedIndex, true);
        }
    }
}