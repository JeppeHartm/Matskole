using Game2.Component;
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

namespace Game2
{
    public class AnchorPoint : BasePoint
    {
        public AnchorPoint(double x, double y, bool isintoolbox, GridPointArea gpa)
        {
            _x = x;
            _y = y;
            _area = gpa;
            _isInToolbox = isintoolbox;
        }


        private bool _isInToolbox;
        public bool IsInToolbox { get { return _isInToolbox; } }

        private GridPointArea _area;
        public GridPointArea Area { get { return _area; } set { _area = value; } }

        public void SnapAndSetStatus(MathComponentModel m)
        {            
            MainPage parentWindow = (MainPage)Application.Current.RootVisual;
            List<GridPoint> l = Area.GridPointList;
            Area.SetStatus(MainPage.GridStatus.gotComponent);
            foreach (GridPoint g in l)
            {
                g.OccupyingComponent = m;
            }
            if (m is Component1i1oModel)
            {
                l[0].OccupyingComponentLegIndex = 0;
                l[0].Status = MainPage.GridStatus.gotFreeInputComponentPin;
                l[1].OccupyingComponentLegIndex = 0;
                l[1].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[1].X, l[1].Y, m, l[1]));
            }
            else if (m is Component1i2oModel)
            {
                l[0].OccupyingComponentLegIndex = 0;
                l[0].Status = MainPage.GridStatus.gotFreeInputComponentPin;
                l[1].OccupyingComponentLegIndex = 0;
                l[1].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                l[3].OccupyingComponentLegIndex = 1;
                l[3].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[1].X, l[1].Y, ((Component2oModel)m).Primary, l[1]));
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[3].X, l[3].Y, ((Component2oModel)m).Secondary, l[3]));
            }
            else if (m is Component2i1oModel)
            {
                l[0].OccupyingComponentLegIndex = 0;
                l[0].Status = MainPage.GridStatus.gotFreeInputComponentPin;
                l[1].OccupyingComponentLegIndex = 0;
                l[1].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                l[2].OccupyingComponentLegIndex = 1;
                l[2].Status = MainPage.GridStatus.gotFreeInputComponentPin;           
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[1].X, l[1].Y, m, l[1]));
            }
            else if (m is Component2i2oModel)
            {
                l[0].OccupyingComponentLegIndex = 0;
                l[0].Status = MainPage.GridStatus.gotFreeInputComponentPin;
                l[1].OccupyingComponentLegIndex = 0;
                l[1].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                l[2].OccupyingComponentLegIndex = 1;
                l[2].Status = MainPage.GridStatus.gotFreeInputComponentPin;
                l[3].OccupyingComponentLegIndex = 1;
                l[3].Status = MainPage.GridStatus.gotFreeOutputComponentPin;
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[1].X, l[1].Y, ((Component2oModel)m).Primary, l[1]));
                parentWindow.ConnectorList.Add(new Connector.ConnectorModel(l[3].X, l[3].Y, ((Component2oModel)m).Secondary, l[3]));
            }
            m.X = (int)X - (m.CompWidth / 2);
            m.Y = (int)Y - (m.CompHeight / 2);
        }
    }
}
