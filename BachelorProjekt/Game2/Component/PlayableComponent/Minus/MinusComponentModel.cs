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

namespace Game2.Component.PlayableComponent.Minus
{
    public class MinusComponentModel : Component2i1oModel
    {
        public MinusComponentModel(AnchorPoint ap, double sizemod) : base(sizemod)
        {
            CurrentAnchorPoint = ap;
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/Minus/MinusRR.png", UriKind.Relative);

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/Minus/MinusRR.png", UriKind.Relative);
            ImgUriAltRedGreen = new Uri("/Game2;Component/Images/Components/Minus/MinusRG.png", UriKind.Relative);
            ImgUriAltGreenRed = new Uri("/Game2;Component/Images/Components/Minus/MinusGR.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/Minus/MinusGG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * 9), ((ImgHeight / 20) * 8), 0, 0);
        }
        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();
            list.Add((l[0] - l[1]));
            return list;
        }
    }
}
