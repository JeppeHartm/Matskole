using System;
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
    public abstract class Component1i2oModel : Component2oModel
    {
        public Component1i2oModel(double sizemod) : base(sizemod) { }
        protected override void init()
        {
            inLimit = 1;
            outLimit = 2;
        }
    }
}
