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

namespace Game2.Component.PlayableComponent.Splitter
{
    public class SplitterComponentModel : Component1i2oModel
    {
        public SplitterComponentModel(AnchorPoint ap, double sizemod)
            : base(sizemod)
        {
            CurrentAnchorPoint = ap;
            Primary = new Subcomponents.OutputSubComponentModel(this, 0);
            Secondary = new Subcomponents.OutputSubComponentModel(this, 0);
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/Splitter/SplitterRR.png", UriKind.Relative);

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/Splitter/SplitterRR.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/Splitter/SplitterGG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * -1), ImgHeight, 0, 0);
            RightOutMargin = new Thickness(((CompWidth / 20) * 17), ((ImgHeight / 20) * 10), 0, 0);
        }

        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();

            int val = l[0];
            int output = (int)Math.Ceiling(val / 2.0);

            list.Add(output);
            list.Add((val - output));

            return list;
        }

    }
}
