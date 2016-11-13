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

namespace Game2.Component.StaticComponents.BoardOut
{
    public class BoardOutModel : SuperComponentModel
    {

        private int _expectedOutput;
        public int ExpectedOutput { get { return _expectedOutput; } private set { _expectedOutput = value; OnPropertyChanged("ExpectedOutput"); } }

        private SolidColorBrush _outColor = Game2.Connector.ConnectorModel.Green;
        public SolidColorBrush OutColor { get { return _outColor; } set { _outColor = value; OnPropertyChanged("OutColor"); } }

        public GridPoint AttachedGridPoint;

        public BoardOutModel(int value, double sizemod): base(sizemod, true)
        {
            Locked = true;
            Playable = false;
            ExpectedOutput = value;
            inLimit = 1;
            ImgSourcePath = new Uri("/Game2;Component/Images/Components/BoardOut/BoardOut.png", UriKind.Relative);
        }

        public override int GetOut()
        {
            if (In[0] != null)
                return In[0].GetOut();
            else
                return 0;
        }
        public void Attach(GridPoint g)
        {
            X = (int)g.X - (CompWidth / 2);
            Y = (int)g.Y - 3;
            g.OccupyingComponent = this;
            g.OccupyingComponentLegIndex = 0;
            g.Status = MainPage.GridStatus.gotFreeInputComponentPin;
        }
        public bool IsSatisfied()
        {
            if (In[0] == null)
                return false;
            if (In[0].HasInputs)
                return GetOut() == ExpectedOutput;
            else
                return false;
        }
    }
}
