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
using System.Collections.Generic;

namespace Game2.Component.PlayableComponent.Filter
{
    public class FilterComponentModel : Component1i2oModel
    {
        public FilterComponentModel(int filters, AnchorPoint ap, double sizemod) : base(sizemod)
        {
            CurrentAnchorPoint = ap;
            Filters = filters;
            Primary = new Subcomponents.OutputSubComponentModel(this, 0);
            Secondary = new Subcomponents.OutputSubComponentModel(this, 0);
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/Filter/Filter"+filters+"RR.png", UriKind.Relative);//test, should be FilterRR so it defaults to all red

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/Filter/Filter" + filters + "RR.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/Filter/Filter" + filters + "GG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * -1), ImgHeight, 0, 0);
            RightOutMargin = new Thickness(((CompWidth / 20) * 17), ((ImgHeight / 20) * 10), 0, 0);
        }

        private int _filters;
        public int Filters{ get { return _filters; } set { _filters = value; OnPropertyChanged("Filters"); } }

        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();

            int val = l[0];
            int output = (int)(((int)(val / Filters) % 10) * Filters);

            list.Add(output);
            list.Add((val - output));

            return list;
        }

    }
}
