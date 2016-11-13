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
    public abstract class Component1i1oModel : MathComponentModel
    {
        public Component1i1oModel(double sizemod) : base(sizemod) { }
        protected override void init()
        {
            Type = Game2.MainPage.ComponentSizeType.Small;
            inLimit = 1;
            outLimit = 1;
        }
    }
}
