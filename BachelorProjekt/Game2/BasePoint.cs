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

namespace Game2
{
    public class BasePoint
    {

        protected double _x;
        public double X { get { return _x; } protected set { _x = value; } }
        protected double _y;
        public double Y { get { return _y; } protected set { _y = value; } }

    }
}
