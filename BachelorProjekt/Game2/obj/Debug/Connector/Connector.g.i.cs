﻿#pragma checksum "C:\Users\Jeppe\Documents\Visual Studio 2012\Projects\BachelorProjekt\Game2\Connector\Connector.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D8511147EC1197891E6965F96091F262"
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


namespace Game2.Connector {
    
    
    public partial class Connector : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Polyline specialEndingLine;
        
        internal System.Windows.Shapes.Polyline specialEndingLightLine;
        
        internal System.Windows.Shapes.Line line;
        
        internal System.Windows.Shapes.Line lightLine;
        
        internal System.Windows.Controls.Image ArrowHeadImage;
        
        internal System.Windows.Shapes.Ellipse hitBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Game2;component/Connector/Connector.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.specialEndingLine = ((System.Windows.Shapes.Polyline)(this.FindName("specialEndingLine")));
            this.specialEndingLightLine = ((System.Windows.Shapes.Polyline)(this.FindName("specialEndingLightLine")));
            this.line = ((System.Windows.Shapes.Line)(this.FindName("line")));
            this.lightLine = ((System.Windows.Shapes.Line)(this.FindName("lightLine")));
            this.ArrowHeadImage = ((System.Windows.Controls.Image)(this.FindName("ArrowHeadImage")));
            this.hitBox = ((System.Windows.Shapes.Ellipse)(this.FindName("hitBox")));
        }
    }
}

