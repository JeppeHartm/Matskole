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

namespace Game2.Component.PlayableComponent.ConnectorCross
{
    public class ConnectorCrossComponentModel : Component2i2oModel
    {
        public ConnectorCrossComponentModel(AnchorPoint ap, double sizemod) : base(sizemod)
        {
            init();
            CurrentAnchorPoint = ap;
            Primary = new Subcomponents.OutputSubComponentModel(this,0);
            Secondary = new Subcomponents.OutputSubComponentModel(this,0);
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/ConnectorCross/ConnectorCrossRR.png", UriKind.Relative);

            ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/ConnectorCross/ConnectorCrossRR.png", UriKind.Relative);
            ImgUriAltCrossGreenRed = new Uri("/Game2;Component/Images/Components/ConnectorCross/ConnectorCrossGR.png", UriKind.Relative);
            ImgUriAltCrossRedGreen = new Uri("/Game2;Component/Images/Components/ConnectorCross/ConnectorCrossRG.png", UriKind.Relative);
            ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/ConnectorCross/ConnectorCrossGG.png", UriKind.Relative);

            LeftOutMargin = new Thickness(((CompWidth / 20) * -1), ImgHeight, 0, 0);
            RightOutMargin = new Thickness(((CompWidth / 20) * 16), ((ImgHeight / 20) * 10), 0, 0);
        }

        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();

            list.Add(l[1]);
            list.Add(l[0]);

            return list;
        }
    }
}
