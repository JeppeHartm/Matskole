﻿using System;
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
using Game2;
using Game2.Component.PlayableComponent;

namespace Game2.Component.PlayableComponent.Plus
{
    public partial class PlusComponent : BaseUserControlComponent
    {
        public PlusComponent()
        {
            InitializeComponent();
            var throttledEvent = new ThrottledMouseMoveEvent(LayoutRoot);
            throttledEvent.ThrottledMouseMove += new MouseEventHandler(LayoutRoot_MouseMove_1);
        }

        public void SetOwningModelofSenderViewModel()
        {
            _owningModel = (MathComponentModel)PlusComponentView.GetBindingExpression(TagProperty).DataItem;
        }
        private void PlusComponentView_Loaded_1(object sender, RoutedEventArgs e)
        {
            _parentWindow = (MainPage)Application.Current.RootVisual;
        }

        private void LayoutRoot_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            LeftMouseButtonDown(sender, e, LayoutRoot,this);
        }

        public void LayoutRoot_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            LeftMouseButtonUp(sender, e, LayoutRoot, this);
        }

        private void LayoutRoot_MouseMove_1(object sender, MouseEventArgs e)
        {
            MouseMoveEvent(sender, e, this, LayoutRoot);
        }
    }
}