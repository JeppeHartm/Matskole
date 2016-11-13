using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2.Connector
{
    public partial class Connector : UserControl
    {
        public Connector()
        {
            InitializeComponent();
        }

        private MainPage _parentWindow;
        private ConnectorModel _owningModel;
        private bool _leftDown = false;
        private Point _endPoint = new Point();

        private void ArrowHeadLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_owningModel == null)
                _owningModel = (ConnectorModel)lightLine.GetBindingExpression(Polyline.StrokeThicknessProperty).DataItem;
            if (!_owningModel.IsReadyToBeUsed)
                return;
            _parentWindow = (MainPage)Application.Current.RootVisual;
            if (_parentWindow.IsInDeleteMode)
                LineMouseDown(sender, e);
            hitBox.CaptureMouse();
            _leftDown = true;

        }
        protected void ArrowHeadMouseMove(object sender, MouseEventArgs e)
        {
            if (_owningModel == null)
                _owningModel = (ConnectorModel)lightLine.GetBindingExpression(Polyline.StrokeThicknessProperty).DataItem;

            if (!_leftDown) return;
            _parentWindow = (MainPage)Application.Current.RootVisual;
            if (_parentWindow.IsInDeleteMode)
                return;
            var mousePosition = e.GetPosition(_parentWindow.MainCanvas);
            _owningModel.XTarget = (int)(mousePosition.X);
            _owningModel.YTarget = (int)(mousePosition.Y);
            if (_endPoint == null) _endPoint = new Point();
            _endPoint.X = (mousePosition.X);
            _endPoint.Y = (mousePosition.Y);
            _parentWindow.CurrentlyMovingConnector = _owningModel;

                _owningModel.MakeArrowHead();
        }
        private void ArrowHeadMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_owningModel == null)
                _owningModel = (ConnectorModel)lightLine.GetBindingExpression(Polyline.StrokeThicknessProperty).DataItem;
            if (!_owningModel.IsReadyToBeUsed)
            {
                hitBox.ReleaseMouseCapture();
                return;
            }

            hitBox.ReleaseMouseCapture();
            _parentWindow = (MainPage)Application.Current.RootVisual;
            _leftDown = false;
            if (_parentWindow.SmartConnectorsOn)
            {
                ConnectDirectlyToTargetedGridPoint();
                return;
            }
            GridPoint closest = _parentWindow.ClosestGridPoint(_endPoint, _owningModel.SourceGridPoint);
            if (!(_owningModel.ValidatingGridPointForConnection(closest, _parentWindow.IsInDeleteMode, _owningModel, _parentWindow.ConnectorList, _parentWindow.MainPageUpdateLayout, _parentWindow.ComponentList, _parentWindow.FixedComponentList)))
                return;
            _owningModel.XTarget = (int)(closest.X);
            _owningModel.YTarget = (int)(closest.Y);
            _owningModel.MakeArrowHead();

            _owningModel.CreateNewConnectorAndAssociatedTasks(closest, _parentWindow.SizeType, _parentWindow.ConnectorList, _parentWindow.EvaluateSystem);
          
        }
        private void ConnectDirectlyToTargetedGridPoint()
        {
            GridPoint endGridPoint = _parentWindow.ClosestGridPoint(_endPoint, _owningModel.SourceGridPoint, true);
            if (!(_owningModel.ValidatingGridPointForConnection(endGridPoint, _parentWindow.IsInDeleteMode, _owningModel, _parentWindow.ConnectorList, _parentWindow.MainPageUpdateLayout, _parentWindow.ComponentList, _parentWindow.FixedComponentList)))
                return;

            double ShortestDistance = Math.Abs(_owningModel.SourceGridPoint.X - endGridPoint.X) + Math.Abs(_owningModel.SourceGridPoint.Y - endGridPoint.Y);

            _owningModel.RecursiveRouteFinding(endGridPoint, _parentWindow.IsInDeleteMode, _parentWindow.SizeType, _parentWindow.ConnectorList, ShortestDistance, _owningModel, _parentWindow.ClosestGridPoint, _parentWindow.EvaluateSystem, 0, _parentWindow.MainPageUpdateLayout, _parentWindow.ComponentList, _parentWindow.FixedComponentList, _parentWindow.StopConnectorTutorial1, _parentWindow.StartConnectorTutorial2);
        }

        private void LineMouseDown(object sender, MouseButtonEventArgs e)
        {
            _parentWindow = (MainPage)Application.Current.RootVisual;
            if (!_parentWindow.IsInDeleteMode)
                return;
            if (_owningModel == null)
                _owningModel = (ConnectorModel)lightLine.GetBindingExpression(Polyline.StrokeThicknessProperty).DataItem;

            ConnectorModel cm = _owningModel;
            cm.IsReadyToBeUsed = true;
            if (cm.ReachedEnd)
            {
                cm.ReachedEnd = false;
                cm.TargetConnector.SourceGridPoint.OccupyingComponent.In[cm.TargetConnector.SourceGridPoint.OccupyingComponentLegIndex] = null;
                cm.SpecialEndingLine = new PointCollection();
                cm.SpecialEndingLightLine = new PointCollection();
            }
            cm.XTarget = cm.XSource;
            cm.YTarget = cm.YSource;
            if (cm.SourceConnector != null)
                cm.MakeArrowHead();
            else
                cm.MakeArrowHead();
            while (cm.TargetConnector != null) 
            {
                if (cm.ReachedEnd)
                {
                    cm.TargetConnector.SourceGridPoint.OccupyingComponent.In[cm.TargetConnector.SourceGridPoint.OccupyingComponentLegIndex] = null;
                }
                cm.TargetConnector.PrepareForDeletion(false, _parentWindow.MainPageUpdateLayout);
                _parentWindow.ConnectorList.Remove(cm.TargetConnector);
                cm = cm.TargetConnector;
            }
            _owningModel.TargetConnector = null;

            _parentWindow.EvaluateSystem(true);
            _parentWindow.MainPageUpdateLayout();
        }
    }
}
