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
    public abstract class Component2oModel : MathComponentModel
    {
        public Component2oModel(double sizemod) : base(sizemod) { }
        protected Subcomponents.OutputSubComponentModel primary, secondary;
        public Subcomponents.OutputSubComponentModel Primary { get { return primary;}  set { primary = value ;} }
        public Subcomponents.OutputSubComponentModel Secondary { get { return secondary; } set { secondary = value; } }
        public int GetSecondaryOut()
        {
            return MathFunc(GetIn())[1];
        }

        private Thickness _rightOutMargin = new Thickness();
        public Thickness RightOutMargin { get { return _rightOutMargin; } set { _rightOutMargin = value; OnPropertyChanged("RightOutMargin"); } }
        private string _rightOut = "  ";
        public string RightOut { get { return _rightOut; } set { _rightOut = value; OnPropertyChanged("RightOut"); } }
        
    }
}