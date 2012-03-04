using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace sbbs_client_wp7
{
    public partial class BoardSettingsPage : PhoneApplicationPage
    {
        public BoardSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 恢复选择
            ignoreSelection = true;
            Mode.SelectedIndex = App.Service.BoardMode % 3;
        }

        private bool ignoreSelection = true; // 永远忽略第一次选择（由系统触发）
        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 设置标记忽略事件
            if (ignoreSelection)
            {
                ignoreSelection = false;
                return;
            }

            ListPicker list = sender as ListPicker;

            // 设置并保存模式
            int mode = list.SelectedIndex;

            if (mode != App.Service.BoardMode)
            {
                App.Service.BoardMode = mode;
                LocalCache.Set<int>("BoardMode", App.Service.BoardMode);

                // 刷新版面以生效
                App.ViewModel.CurrentBoard.NeedRefresh = true;
            }
        }
    }
}