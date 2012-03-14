using System;
using System.IO;
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
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using System.Windows.Resources;

namespace sbbs_client_wp7
{
    public partial class AboutPage : PhoneApplicationPage
    {
        private StackPanel _licenses;

        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Uri manifest = new Uri("WMAppManifest.xml", UriKind.Relative);
            var si = Application.GetResourceStream(manifest);
            if (si != null)
            {
                using (StreamReader sr = new StreamReader(si.Stream))
                {
                    bool haveApp = false;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (!haveApp)
                        {
                            int i = line.IndexOf("AppPlatformVersion=\"", StringComparison.InvariantCulture);
                            if (i >= 0)
                            {
                                haveApp = true;
                                line = line.Substring(i + 20);

                                int z = line.IndexOf("\"");
                                if (z >= 0)
                                {
                                    // if you're interested in the app plat version at all
                                    // AppPlatformVersion = line.Substring(0, z);
                                }
                            }
                        }

                        int y = line.IndexOf("Version=\"", StringComparison.InvariantCulture);
                        if (y >= 0)
                        {
                            int z = line.IndexOf("\"", y + 9, StringComparison.InvariantCulture);
                            if (z >= 0)
                            {
                                // We have the version, no need to read on.
                                VersionText.Text = line.Substring(y + 9, z - y - 9);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                VersionText.Text = "Unknown";
            }
        }
        
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string s = ((ButtonBase)sender).Tag as string;
 
            switch (s)
            {
                case "Review":
                    var task = new MarketplaceReviewTask();
                    task.Show();
                    break;
                case "Email":
                    EmailComposeTask emailTask = new EmailComposeTask();
                    emailTask.To = ((ButtonBase)sender).Content.ToString();
                    emailTask.Subject = "定制Windows Phone应用";
                    emailTask.Show();
                    break;
                case "Phone":
                    SmsComposeTask smsComposeTask = new SmsComposeTask();
                    smsComposeTask.To = ((ButtonBase)sender).Content.ToString();
                    smsComposeTask.Body = "定制Windows Phone应用";
                    smsComposeTask.Show();
                    break;
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot piv = (Pivot)sender;
            if (piv.SelectedIndex > 0 && _licenses == null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    _licenses = new StackPanel();

                    StreamResourceInfo sri = Application.GetResourceStream(
                        new Uri("LICENSE.txt", UriKind.Relative));
                    if (sri != null)
                    {
                        using (StreamReader sr = new StreamReader(sri.Stream))
                        {
                            string line;
                            bool lastWasEmpty = true;
                            do
                            {
                                line = sr.ReadLine();

                                if (line == string.Empty)
                                {
                                    Rectangle r = new Rectangle
                                    {
                                        Height = 20,
                                    };
                                    _licenses.Children.Add(r);
                                    lastWasEmpty = true;
                                }
                                else
                                {
                                    TextBlock tb = new TextBlock
                                    {
                                        TextWrapping = TextWrapping.Wrap,
                                        Text = line,
                                        Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"],
                                    };
                                    if (!lastWasEmpty)
                                    {
                                        tb.Opacity = 0.7;
                                    }
                                    lastWasEmpty = false;
                                    _licenses.Children.Add(tb);
                                }
                            } while (line != null);
                        }
                    }

                    sv1.Content = _licenses;
                });
            }
        }
    }
}