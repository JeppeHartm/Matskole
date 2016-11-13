using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Game2.Component;

namespace Game2.Component.PlayableComponent.Plus
{
    public class PlusComponentModel : Component2i1oModel
    {
        public PlusComponentModel(AnchorPoint ap, double sizemod) : base(sizemod)
        {
            CurrentAnchorPoint = ap;
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/Plus/PlusRR.png", UriKind.Relative);

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/Plus/PlusRR.png", UriKind.Relative);
            ImgUriAltRedGreen = new Uri("/Game2;Component/Images/Components/Plus/PlusRG.png", UriKind.Relative);
            ImgUriAltGreenRed = new Uri("/Game2;Component/Images/Components/Plus/PlusGR.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/Plus/PlusGG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * 7), ImgHeight, 0, 0);
        }

        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();
            list.Add((l[0] + l[1]));
            return list;
        }
    }
}