using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public abstract class SuperComponentModel : INotifyPropertyChanged
    {

        public SuperComponentModel(double sizemod, bool IsStaticComp = false)
        {
            double extraHeightMod = 0.20;
            SizeMod = sizemod;

            if (IsStaticComp)
            {
                ImgHeight = (int)(69 * sizemod);
                ImgWidth = (int)(76 * sizemod);
                CompHeight = (int)(69 * sizemod);
                CompWidth = (int)(76 * sizemod);
            }
            else
            {
                ImgHeight = (int)(ImgHeight * (sizemod + (sizemod * extraHeightMod)));
                ImgWidth = (int)(ImgWidth * sizemod); 
                CompHeight = (int)(CompHeight * (sizemod + (sizemod * extraHeightMod)));
                CompWidth = (int)((CompWidth * sizemod) * 1.2);//to fit with very large numbers use *1.2
            }
        }

        private Uri _imgUriAllRed = new Uri("RR.png", UriKind.Relative);
        public Uri ImgUriAltAllRed { get { return _imgUriAllRed; } protected set { _imgUriAllRed = value; } }

        private Uri _imgUriAltGreenRed = new Uri("GR.png", UriKind.Relative);
        public Uri ImgUriAltGreenRed { get { return _imgUriAltGreenRed; } protected set { _imgUriAltGreenRed = value; } }

        private Uri _imgUriAltRedGreen = new Uri("RG.png", UriKind.Relative);
        public Uri ImgUriAltRedGreen { get { return _imgUriAltRedGreen; } protected set { _imgUriAltRedGreen = value; } }

        private Uri _imgUriAltCrossRedGreenGreenRed = new Uri("RGGR.png", UriKind.Relative);
        public Uri ImgUriAltCrossRedGreen { get { return _imgUriAltCrossRedGreenGreenRed; } protected set { _imgUriAltCrossRedGreenGreenRed = value; } }
        private Uri _imgUriAltCrossGreenRedRedGreen = new Uri("GRRG.png", UriKind.Relative);
        public Uri ImgUriAltCrossGreenRed { get { return _imgUriAltCrossGreenRedRedGreen; } protected set { _imgUriAltCrossGreenRedRedGreen = value; } }

        private Uri _imgUriAltAllGreen = new Uri("GG.png", UriKind.Relative);
        public Uri ImgUriAltAllGreen { get { return _imgUriAltAllGreen; } protected set { _imgUriAltAllGreen = value; } }

        public double SizeMod = 1;



        private Uri _imgSourcePath = new Uri("", UriKind.Relative);
        public Uri ImgSourcePath { get { return _imgSourcePath; } set { _imgSourcePath = value; OnPropertyChanged("ImgSourcePath"); } }

        private SuperComponentModel[] _in = new SuperComponentModel[2];
        
        public SuperComponentModel[] In { get { return _in; } set { _in = value; } }
        

        private int _xpos;
        public virtual int X { get { return _xpos; } set { _xpos = value; } }
        private int _ypos;
        public virtual int Y { get { return _ypos; } set { _ypos = value; } }

        private bool _playable = true;
        public bool Playable { get { return _playable; } set { _playable = value; } }
        private bool _locked = false;
        public bool Locked { get { return _locked; } set { _locked = value; } }

        private int _outlimit;
        public int outLimit { get { return _outlimit; } set { _outlimit = value; } }
        private int _inlimit;
        public int inLimit { get { return _inlimit; } set { _inlimit = value; } }

        public bool HasInputs 
        { 
            get {
                bool b = true;                
                for (int i = 0; i < inLimit; i++)
                {
                    if (In[i] == null)
                        b = false;
                    else
                        b = b && (In[i] != null) && In[i].HasInputs;;
                }
                return b;
            }
        } 

        public event PropertyChangedEventHandler PropertyChanged;

        private int _imgHeight = 52;
        public int ImgHeight { get { return _imgHeight; } set { _imgHeight = value; OnPropertyChanged("ImgHeight"); } }
        private int _imgWidth = 85;
        public int ImgWidth { get { return _imgWidth; } set { _imgWidth = value; OnPropertyChanged("ImgWidth"); } }

        private int _height = 52;
        public int CompHeight { get { return _height + 10; } set { _height = value; OnPropertyChanged("CompHeight"); } }
        private int _width = 95;
        public int CompWidth { get { return _width + 20; } set { _width = value; OnPropertyChanged("CompWidth"); } }
        

        public abstract int GetOut();

        protected void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
