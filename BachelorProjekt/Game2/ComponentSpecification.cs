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

namespace Game2.ComponentSpecification
{
    public class GenericComponentModel
    {
        public int InLimit { get; private set; }
        private int _outLimit = 1;
        public int OutLimit { get { return _outLimit; } private set { _outLimit = value;} }
        public MainPage.ComponentSizeType SizeType { get { return OutLimit == 1 && InLimit == 1 ? MainPage.ComponentSizeType.Small : MainPage.ComponentSizeType.Large; } }
        public int LeftMargin { get; private set; }
        public int RightMargin { get; private set; }

        public List<Func<int, int, int>> ChosenMathFun { get; private set; }
        public List<Uri> ImageSources { get; private set; }


        public GenericComponentModel(List<Func<int, int, int>> mathfunsies, List<Uri> imageSources, int inLimit, int leftMargin)
        {
            ChosenMathFun = mathfunsies;
            ImageSources = imageSources;
            InLimit = inLimit;
            LeftMargin = leftMargin;
        }
        public GenericComponentModel(List<Func<int, int, int>> mathfunsies, List<Uri> imageSources, int inLimit, int leftMargin, int outLimit, int rightMargin)
            : this(mathfunsies, imageSources, inLimit, leftMargin)
        {        
            OutLimit = outLimit;
            RightMargin = rightMargin;
        }


        

    }

    public static class MathFunctions
    {
        #region math functions
        public static int Plus(int input1, int input2)
        {
            return input1 + input2;
        }
        public static int Minus(int input1, int input2)
        {
            return input1 - input2;
        }

        #region private helper functions
        private static int Filtero1(int input1, int filter)
        {
            int output = ((int)(input1 / filter) % 10) * filter;
            return output;
        }
        private static int Filtero2(int input1, int filter)
        {
            return input1 - Filtero1(input1, filter);
        }
        #endregion
        public static int Filter1o1(int input1, int input2 = 0)
        {
            return Filtero1(input1, 1);
        }
        public static int Filter1o2(int input1, int input2 = 0)
        {
            return Filtero2(input1, 1);
        }
        public static int Filter10o1(int input1, int input2 = 0)
        {
            return Filtero1(input1, 10);
        }
        public static int Filter10o2(int input1, int input2 = 0)
        {
            return Filtero2(input1, 10);
        }
        public static int Filter100o1(int input1, int input2 = 0)
        {
            return Filtero1(input1, 100);
        }
        public static int Filter100o2(int input1, int input2 = 0)
        {
            return Filtero2(input1, 100);
        }
        #endregion
    }
    
}
