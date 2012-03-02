using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Phone.Controls;

namespace sbbs_client_wp7
{
    using Sbbs;

    public partial class BoardPage : PhoneApplicationPage
    {
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
                App.ViewModel.CurrentBoard.EnglishName = this.NavigationContext.QueryString["board"];
                App.ViewModel.CurrentBoard.Description = this.NavigationContext.QueryString["description"];
            }
        }
    }
}