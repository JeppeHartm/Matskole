﻿#pragma checksum "C:\Users\Jeppe\Documents\Visual Studio 2012\Projects\BachelorProjekt\Game2\Component\StaticComponents\BoardOut\BoardOut.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "90801B9495196E12B7C0207A3466C035"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace Game2.Component.StaticComponents.BoardOut {
    
    
    public partial class BoardOut : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl BoardOutView;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Game2;component/Component/StaticComponents/BoardOut/BoardOut.xaml", System.UriKind.Relative));
            this.BoardOutView = ((System.Windows.Controls.UserControl)(this.FindName("BoardOutView")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
        }
    }
}

