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

namespace Game2.Component.PlayableComponent.Multiply
{
    public class MultiplyComponentModel : Component1i1oModel
    {
        public MultiplyComponentModel(AnchorPoint ap, double sizemod) : base(sizemod)
        {
            CurrentAnchorPoint = ap;
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/Multiply/MultiplyRR.png", UriKind.Relative);

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/Multiply/MultiplyRR.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/Multiply/MultiplyGG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * 13), ((ImgHeight / 20) * 10), 0, 0);
        }
        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();

            list.Add((l[0] * 10));

            return list;
        }
    }
}
