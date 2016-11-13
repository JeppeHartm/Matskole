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

namespace Game2.Component.StaticComponents.BoardIn
{
    public class BoardInModel : SuperComponentModel
    {
        private int _out = 0;
        public int Out { get { return _out; } private set { _out = value; OnPropertyChanged("Out"); } }
        private double _sizemod = 0;

        public BoardInModel(int value,double sizemod) : base(sizemod, true)
        {
            Locked = true;
            Playable = false;
            Out = value;
            inLimit = 0;
            SizeMod = sizemod;
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/BoardIn/BoardIn.png", UriKind.Relative);
        }

        public override int GetOut()
        {
            return Out;
        }

        public void Attach(GridPoint g)
        {

            X = (int)g.X - (int)((CompWidth / 2) + (0.75*_sizemod));
            Y = (int)g.Y - (CompHeight - 5);
            g.OccupyingComponent = this;
            g.OccupyingComponentLegIndex = 0;
            g.Status = MainPage.GridStatus.gotFreeOutputComponentPin;

        }
         
    }
}
