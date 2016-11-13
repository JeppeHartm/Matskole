﻿#pragma checksum "C:\Users\Jeppe\Documents\Visual Studio 2012\Projects\BachelorProjekt\Game2\UI\MainMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "76E7DD2C2BE3279F3193C77F67A6BF46"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Game2.UI;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Game2.UI {
    
    
    public partial class MainMenu : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl MainMenuView;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContainingGrid;
        
        internal System.Windows.Controls.Grid UnderlyingGrid;
        
        internal System.Windows.Media.Animation.Storyboard Gear1Storyboard;
        
        internal System.Windows.Media.RotateTransform Gear1Transform;
        
        internal System.Windows.Media.Animation.Storyboard Gear2Storyboard;
        
        internal System.Windows.Media.RotateTransform Gear2Transform;
        
        internal System.Windows.Controls.Image RobotType;
        
        internal System.Windows.Shapes.Polygon SpeecBalloonSec;
        
        internal System.Windows.Controls.TextBox UsernameTextBox;
        
        internal System.Windows.Controls.PasswordBox PasswordTextBox;
        
        internal System.Windows.Shapes.Rectangle SpeechBalloonMain;
        
        internal System.Windows.Shapes.Line SpeechBalloonLineRemover;
        
        internal System.Windows.Controls.TextBlock WelcomeMessage;
        
        internal System.Windows.Controls.Button ButtonChooseLevel;
        
        internal System.Windows.Controls.Button AdminButton;
        
        internal System.Windows.Controls.Button LogoutButton;
        
        internal System.Windows.Controls.TextBlock versionText;
        
        internal Game2.UI.TeacherClient TeacherClientUi;
        
        internal Game2.UI.MainMenuChooseLevelUi ChooseLevelUi;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Game2;component/UI/MainMenu.xaml", System.UriKind.Relative));
            this.MainMenuView = ((System.Windows.Controls.UserControl)(this.FindName("MainMenuView")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContainingGrid = ((System.Windows.Controls.Grid)(this.FindName("ContainingGrid")));
            this.UnderlyingGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderlyingGrid")));
            this.Gear1Storyboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Gear1Storyboard")));
            this.Gear1Transform = ((System.Windows.Media.RotateTransform)(this.FindName("Gear1Transform")));
            this.Gear2Storyboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Gear2Storyboard")));
            this.Gear2Transform = ((System.Windows.Media.RotateTransform)(this.FindName("Gear2Transform")));
            this.RobotType = ((System.Windows.Controls.Image)(this.FindName("RobotType")));
            this.SpeecBalloonSec = ((System.Windows.Shapes.Polygon)(this.FindName("SpeecBalloonSec")));
            this.UsernameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("UsernameTextBox")));
            this.PasswordTextBox = ((System.Windows.Controls.PasswordBox)(this.FindName("PasswordTextBox")));
            this.SpeechBalloonMain = ((System.Windows.Shapes.Rectangle)(this.FindName("SpeechBalloonMain")));
            this.SpeechBalloonLineRemover = ((System.Windows.Shapes.Line)(this.FindName("SpeechBalloonLineRemover")));
            this.WelcomeMessage = ((System.Windows.Controls.TextBlock)(this.FindName("WelcomeMessage")));
            this.ButtonChooseLevel = ((System.Windows.Controls.Button)(this.FindName("ButtonChooseLevel")));
            this.AdminButton = ((System.Windows.Controls.Button)(this.FindName("AdminButton")));
            this.LogoutButton = ((System.Windows.Controls.Button)(this.FindName("LogoutButton")));
            this.versionText = ((System.Windows.Controls.TextBlock)(this.FindName("versionText")));
            this.TeacherClientUi = ((Game2.UI.TeacherClient)(this.FindName("TeacherClientUi")));
            this.ChooseLevelUi = ((Game2.UI.MainMenuChooseLevelUi)(this.FindName("ChooseLevelUi")));
        }
    }
}

