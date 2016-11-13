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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game2.UI
{
    public partial class ScoreScreen : UserControl
    {

        
        public ScoreScreen()
        {
            InitializeComponent();
            
        }

        private void Continue_ButtonEvent(object sender, RoutedEventArgs e)
        {
            ((MainPage)Application.Current.RootVisual).ScoreScreen_Continue();
        }
    }
}
