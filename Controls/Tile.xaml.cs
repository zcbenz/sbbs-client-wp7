using System;
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
using System.Windows.Media.Imaging;

namespace sbbs_client_wp7
{
    public partial class Tile : UserControl
    {
        public Tile()
        {
            InitializeComponent();
        }

        public String Title
        {
            set
            {
                this.TileTitle.Text = value;
            }
            get
            {
                return this.TileTitle.Text;
            }
        }

        public String Src
        {
            set
            {
                Uri uri = new Uri(value, UriKind.RelativeOrAbsolute);
                this.TileImage.Source = new BitmapImage(uri);
            }
            get
            {
                return this.TileImage.Source.ToString();
            }
        }
    }
}
