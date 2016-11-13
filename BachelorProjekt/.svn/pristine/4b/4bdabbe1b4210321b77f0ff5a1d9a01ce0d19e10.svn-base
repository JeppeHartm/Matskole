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

namespace Game2.Component.PlayableComponent.PlusMinusX
{
    public class PlusMinusXComponentModel : Component1i1oModel
    {
        public PlusMinusXComponentModel(int x, Func<int, int, int> plusOrMinus, AnchorPoint ap, double sizemod)
            : base(sizemod)
        {
            XX = x;
            PlusOrMinus = plusOrMinus;

            CurrentAnchorPoint = ap;


            if (PlusOrMinus == PlusFunc)
            {
                ImgSourcePath = new Uri("/Game2;Component/Images/Components/PlusMinusX/Plus" + XX + "RR.png", UriKind.Relative);

                ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/PlusMinusX/Plus" + XX + "RR.png", UriKind.Relative);
                ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/PlusMinusX/Plus" + XX + "GG.png", UriKind.Relative);
            }
            else
            {
                ImgSourcePath = new Uri("/Game2;Component/Images/Components/PlusMinusX/Minus" + XX + "RR.png", UriKind.Relative);

                ImgUriAltAllRed = new Uri("/Game2;Component/Images/Components/PlusMinusX/Minus" + XX + "RR.png", UriKind.Relative);
                ImgUriAltAllGreen = new Uri("/Game2;Component/Images/Components/PlusMinusX/Minus" + XX + "GG.png", UriKind.Relative);
            }

            LeftOutMargin = new Thickness(((CompWidth / 20) * 13), ((ImgHeight / 20) * 10), 0, 0);
        }

        public int XX = 0;
        public Func<int,int,int> PlusOrMinus;

        public override List<int> MathFunc(int[] l)
        {
            List<int> list = new List<int>();

            list.Add((PlusOrMinus(l[0],XX)));

            return list;
        }

        public static int PlusFunc(int a, int b)
        {
            return (int)(a + b);
        }
        public static int MinusFunc(int a, int b)
        {
            return (int)(a - b);
        }
    }
}
