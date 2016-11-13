using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2.UI
{
    public class MainMenuModel : INotifyPropertyChanged
    {
        private bool _isLoggedIn = false;
        public bool IsLoggedIn { get { return _isLoggedIn; } set { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); if (_isLoggedIn) { LoginView = Visibility.Collapsed; LoggedInView = Visibility.Visible; } else { LoginView = Visibility.Visible; LoggedInView = Visibility.Collapsed; } } }
        private string _usernameMessage = "";
        public string UsernameMessage { get { return _usernameMessage; } set { _usernameMessage = value; OnPropertyChanged("UsernameMessage"); } }
        private string _startButtonContent = "Start et nyt spil!";
        public string StartButtonContent { get { return _startButtonContent; } set { _startButtonContent = value; OnPropertyChanged("StartButtonContent"); } }
        private string _loginFieldUsername = "";
        public string LoginFieldUsername { get { return _loginFieldUsername; } set { _loginFieldUsername = value; OnPropertyChanged("LoginFieldUsername"); } }
        private string _loginFieldPassword = "";
        public string LoginFieldPassword { get { return _loginFieldPassword; } set { _loginFieldPassword = value; OnPropertyChanged("LoginFieldPassword"); } }
        private List<int> _completedLevels = new List<int>();
        public List<int> CompletedLevels { get { return _completedLevels; } set { _completedLevels = value; } }
        private bool _allLevelsCompleted = false;
        public bool AllLevelsCompleted { get { return _allLevelsCompleted; } set { _allLevelsCompleted = value; } }
        private Visibility _loginView = Visibility.Visible;
        public Visibility LoginView { get { return _loginView; } private set { _loginView = value; OnPropertyChanged("LoginView"); } }
        private Visibility _loggedInView = Visibility.Collapsed;
        public Visibility LoggedInView { get { return _loggedInView; } private set { _loggedInView = value; OnPropertyChanged("LoggedInView"); } }
        private Visibility _adminUserVisibility = Visibility.Collapsed;
        public Visibility AdminUserVisibility { get { return _adminUserVisibility; } set { _adminUserVisibility = value; OnPropertyChanged("AdminUserVisibility"); if (_adminUserVisibility == Visibility.Collapsed) { SetExtraAdminSpacing(0); } else { SetExtraAdminSpacing(5); } } }
        private GridLength _extraAdminSpacing = new GridLength(0, GridUnitType.Auto);
        public GridLength ExtraAdminSpacing { get { return _extraAdminSpacing; } }
        private void SetExtraAdminSpacing(double input)
        {
            _extraAdminSpacing = new GridLength(input, GridUnitType.Pixel);
            OnPropertyChanged("ExtraAdminSpacing");
        }

        public bool UserIsAdmin = false;

        private double _menuWidth = 100;
        public double MenuWidth { get { return _menuWidth; } set { _menuWidth = value; OnPropertyChanged("MenuWidth"); } }
        private double _menuHeight = 100;
        public double MenuHeight { get { return _menuHeight; } set { _menuHeight = value; OnPropertyChanged("MenuHeight"); } }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
