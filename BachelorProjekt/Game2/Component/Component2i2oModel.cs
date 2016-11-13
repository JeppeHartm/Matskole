﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2.Component
{
    public abstract class Component2i2oModel : Component2oModel
    {
        public Component2i2oModel(double sizemod) : base(sizemod) { }
        protected override void init()
        {
            inLimit = 2;
            outLimit = 2;
        }

    }
}