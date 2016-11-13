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
using Game2.Component;

namespace Game2.Connector
{
    public class ConnectorModel : INotifyPropertyChanged
    {
        public ConnectorModel(double xsource, double ysource, SuperComponentModel sourcecomp, GridPoint sourcegridpoint, ConnectorModel sourceconnector = null)
        {
            XSource = xsource;
            YSource = ysource;
            XTarget = xsource;
            YTarget = ysource;
            SourceComponent = sourcecomp;
            SourceConnector = sourceconnector;            
            SourceGridPoint = sourcegridpoint;
            SourceGridPoint.PointVisibility = System.Windows.Visibility.Collapsed;
            if (sourceconnector == null)
                MakeArrowHead(/*TowardsDirections.Down*/);
            else
                MakeArrowHead(/*GetCorrectTowardsDirection(true)*/);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static SolidColorBrush Green = new SolidColorBrush(Color.FromArgb(255, 0, 255, 18));
        public static SolidColorBrush Red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 18));

        private double _xSource = 0;
        private double _ySource = 0;
        private double _xTarget = 0;
        private double _yTarget = 0;
        public double XSource { get { return _xSource; } set { _xSource = value; OnPropertyChanged("XSource"); } }
        public double YSource { get { return _ySource; } set { _ySource = value; OnPropertyChanged("YSource"); } }
        public double XTarget { get { return _xTarget; } set { _xTarget = value; OnPropertyChanged("XTarget"); } }
        public double YTarget { get { return _yTarget; } set { _yTarget = value; OnPropertyChanged("YTarget"); } }
        
        private bool _isReadyToBeUsed = true;
        public bool IsReadyToBeUsed { get { return _isReadyToBeUsed; } set { _isReadyToBeUsed = value; if (_isReadyToBeUsed) { ArrowVisibility = Visibility.Visible; HitBoxVisibility = Visibility.Visible; } else if (!ReachedEnd) { ArrowVisibility = Visibility.Collapsed; HitBoxVisibility = Visibility.Collapsed; } } }
        private bool _reachedEnd = false;
        public bool ReachedEnd { get { return _reachedEnd; } set { _reachedEnd = value; if (_reachedEnd) { ((MainPage)Application.Current.RootVisual).GetOutResult(); } } }

        private ConnectorModel _sourceConnector = null;
        public ConnectorModel SourceConnector { get { return _sourceConnector; } set { _sourceConnector = value; } }
        private ConnectorModel _targetConnector = null;
        public ConnectorModel TargetConnector { get { return _targetConnector; } set { _targetConnector = value; } }
        private SuperComponentModel _sourceComponent = null;
        public SuperComponentModel SourceComponent { get { return _sourceComponent; } set { _sourceComponent = value; } }
        private GridPoint _sourceGridPoint = null;
        public GridPoint SourceGridPoint { get { return _sourceGridPoint; } set { _sourceGridPoint = value; } }


        private double _lightLinethickness = 2;
        public double lightLineThickness { get { return _lightLinethickness; } set { _lightLinethickness = value; OnPropertyChanged("lightLineThickness"); } }
        private double _linethickness = 7;
        public double lineThickness { get { return _linethickness; } set { _linethickness = value; OnPropertyChanged("lineThickness"); } }


        private PointCollection _specialEndingLine = new PointCollection();
        public PointCollection SpecialEndingLine { get { return _specialEndingLine; } set { _specialEndingLine = value; OnPropertyChanged("SpecialEndingLine"); } }
        private PointCollection _specialEndingLightLine = new PointCollection();
        public PointCollection SpecialEndingLightLine { get { return _specialEndingLightLine; } set { _specialEndingLightLine = value; OnPropertyChanged("SpecialEndingLightLine"); } }
        private Visibility _arrowVisibility = Visibility.Visible;
        public Visibility ArrowVisibility { get { return _arrowVisibility; } set { _arrowVisibility = value; OnPropertyChanged("ArrowVisibility"); } }
        private Visibility _hitBoxVisibility = Visibility.Visible;
        public Visibility HitBoxVisibility { get { return _hitBoxVisibility; } set { _hitBoxVisibility = value; OnPropertyChanged("HitBoxVisibility"); } }
        private Thickness _hitBoxPos = new Thickness();
        public Thickness HitBoxPos { get { return _hitBoxPos; } set { _hitBoxPos = value; OnPropertyChanged("HitBoxPos"); } }
        
        private SolidColorBrush _lightLineColor = Red;
        public SolidColorBrush LightLineColor { get { return _lightLineColor; } set { _lightLineColor = value; OnPropertyChanged("LightLineColor"); } }

        private Thickness _arrowHeadPos = new Thickness();
        public Thickness ArrowHeadPos { get { return _arrowHeadPos; } set { _arrowHeadPos = value; OnPropertyChanged("ArrowHeadPos"); } }
        private double _arrowHeadHeight = 22;
        public double ArrowHeadHeight { get { return _arrowHeadHeight; } set { _arrowHeadHeight = value; OnPropertyChanged("ArrowHeadHeight"); } }
        private double _arrowHeadWidth = 28;
        public double ArrowHeadWidth { get { return _arrowHeadWidth; } set { _arrowHeadWidth = value; OnPropertyChanged("ArrowHeadWidth"); } }
        private double _arrowHeadCenterX = 14;
        public double ArrowHeadCenterX { get { return _arrowHeadCenterX; } set { _arrowHeadCenterX = value; OnPropertyChanged("ArrowHeadCenterX"); } }
        private double _arrowHeadCenterY = 11;
        public double ArrowHeadCenterY { get { return _arrowHeadCenterY; } set { _arrowHeadCenterY = value; OnPropertyChanged("ArrowHeadCenterY"); } }
        private double _arrowHeadRotateAngle = 0;
        public double ArrowHeadRotateAngle { get { return _arrowHeadRotateAngle; } set { _arrowHeadRotateAngle = value; OnPropertyChanged("ArrowHeadRotateAngle"); } }
        
        internal void SetColorToGreen(bool SourceCompGotAllInputs)
        {
            if (SourceCompGotAllInputs)
                LightLineColor = Green;
            else
                LightLineColor = Red;
        }
        public void PrepareForDeletion(bool IsPreparingForComponentRemoval, Func<bool> MainPageUpdateLayout, MathComponentModel ComponentBeingMoved = null)
        {
            if (IsPreparingForComponentRemoval && SourceGridPoint.OccupyingComponent != null && ReachedEnd == false && TargetConnector == null && SourceConnector != null && IsReadyToBeUsed == false)
            {
                if (SourceGridPoint.OccupyingComponent is Game2.Component.StaticComponents.BoardOut.BoardOutModel || SourceComponent == ComponentBeingMoved)
                {
                    SourceGridPoint.HoleVisibility = System.Windows.Visibility.Collapsed;
                    SourceGridPoint.PointVisibility = System.Windows.Visibility.Visible;
                    SpecialEndingLine = new PointCollection();
                    SpecialEndingLightLine = new PointCollection();
                    SourceGridPoint.Status = SourceGridPoint.PreviousStatus;
                    MainPageUpdateLayout();
                    return;
                }
            }
            if (IsPreparingForComponentRemoval && SourceGridPoint.OccupyingComponent != null)
            {
                SourceGridPoint.HoleVisibility = System.Windows.Visibility.Collapsed;
                SourceGridPoint.PointVisibility = System.Windows.Visibility.Visible;
                SpecialEndingLine = new PointCollection();
                SpecialEndingLightLine = new PointCollection();
                SourceGridPoint.OccupyingComponent = null;
            }
            else if (ReachedEnd == false && TargetConnector == null && SourceConnector != null && IsReadyToBeUsed == false)
            {
                SourceGridPoint.HoleVisibility = System.Windows.Visibility.Collapsed;
                SourceGridPoint.PointVisibility = System.Windows.Visibility.Visible;
                SpecialEndingLine = new PointCollection();
                SpecialEndingLightLine = new PointCollection();
                SourceGridPoint.Status = SourceGridPoint.PreviousStatus;
            }

            else
            {
                SourceGridPoint.HoleVisibility = System.Windows.Visibility.Collapsed;
                SourceGridPoint.PointVisibility = System.Windows.Visibility.Visible;
                SpecialEndingLine = new PointCollection();
                SpecialEndingLightLine = new PointCollection();
                if (SourceConnector == null)
                    SourceGridPoint.OccupyingComponent = null;
                else
                    SourceGridPoint.Status = SourceGridPoint.PreviousStatus;
            }
            MainPageUpdateLayout();
        }

        public void SetCorrectConnectorEnding(int sizeModifier)
        {
            PointCollection newline = new PointCollection();
            PointCollection newlightline = new PointCollection();
            Point endPoint = new Point();
            double newXTarget = 0, breakPointDist = 5, edgeCorrection = -9;

            newline.Add(new Point(XSource, YSource));
            if (XSource < XTarget) //connector'en kommer fra højre side
            {
                XTarget = XTarget - breakPointDist * sizeModifier;
                newline.Add(new Point(XTarget, YTarget));
                newline.Add(new Point(XTarget, YTarget - (breakPointDist * sizeModifier) - edgeCorrection));
                newline.Add(new Point(XTarget + breakPointDist * sizeModifier, YTarget - (breakPointDist * sizeModifier) - edgeCorrection));
                endPoint = new Point(XTarget + breakPointDist * sizeModifier, YTarget - edgeCorrection);
                newline.Add(endPoint);
            }
            else if (XSource > XTarget) //connector'en kommer fra venstre side
            {
                XTarget = XTarget + breakPointDist * sizeModifier;
                newline.Add(new Point(XTarget, YTarget));
                newline.Add(new Point(XTarget, YTarget - (breakPointDist * sizeModifier) - edgeCorrection));
                newline.Add(new Point(XTarget - breakPointDist * sizeModifier, YTarget - (breakPointDist * sizeModifier) - edgeCorrection));
                endPoint = new Point(XTarget - breakPointDist * sizeModifier, YTarget - edgeCorrection);
                newline.Add(endPoint);
            }
            else
                return;
            newXTarget = XTarget;
            SpecialEndingLine = newline;
            foreach (Point p in newline)
            {
                newlightline.Add(p);
            }
            SpecialEndingLightLine = newlightline;

            XTarget = endPoint.X;
            MakeArrowHead(/*TowardsDirections.Down*/true);
            XTarget = newXTarget;
        }
        #region ArrowHead generation
        
        public void MakeArrowHead(bool IsSpecialEnding = false/*TowardsDirections direction*/)
        {
            MakeHitBox((int)XTarget, (int)YTarget);
            if (IsSpecialEnding)
            {
                ArrowHeadRotateAngle = 180;
                return;
            }

            //*************************************start new arrowhead
            double deltaY, deltaX;
            double angleInDegrees;
            if (XTarget == XSource && YTarget == YSource)
            {
                if (SourceConnector == null)
                    angleInDegrees = 0;
                else
                {
                    angleInDegrees = SourceConnector.ArrowHeadRotateAngle;
                    goto skipfix;
                }
            }
            else
            {
                deltaY = YTarget - YSource;
                deltaX = XTarget - XSource;
                angleInDegrees = Math.Atan2(-deltaX, deltaY) * 180 / Math.PI;
            }

            if (angleInDegrees + 180 > 360)
                angleInDegrees = Math.Abs(angleInDegrees - 180);
            else
                angleInDegrees = angleInDegrees + 180;
            skipfix:
            ArrowHeadRotateAngle = angleInDegrees;
            ArrowHeadPos = new Thickness(XTarget - ((ArrowHeadWidth / 10) * 5), YTarget - ((ArrowHeadHeight / 10) * 5), 0, 0);
            //*************************************end new arrowhead

        }

        private void MakeHitBox(int x, int y)
        {
            HitBoxPos = new Thickness(x-15, y-15, 0, 0);
            return;
        }
        #endregion


        #region methods for grid point connections
        public void RecursiveRouteFinding(GridPoint FinalDestination, bool IsInDeleteMode, LevelSpecification.GridSizeTotal SizeType, System.Collections.ObjectModel.ObservableCollection<ConnectorModel> ConnectorList, double MaxMovementAllowed, ConnectorModel RecursionRoot, Func<Point, GridPoint, bool, GridPoint, GridPoint> ClosestGridPointFromMainPage, Func<bool, bool> EvaluateSystemFromMainPage, double DistMovedSoFar, Func<bool> MainPageUpdateLayout, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> ComponentList, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> fixedList, Func<bool> StopConnectorTutorial1, Func<bool> StartConnectorTutorial2)
        {
            if (MaxMovementAllowed < DistMovedSoFar)
            {
                AbortAllConnections(RecursionRoot, ConnectorList, MainPageUpdateLayout);
                return;
            }
            bool YSourceGreaterThanFinalY = false;
            bool XSourceGreaterThanFinalX = false;
            double deltaX = Math.Abs(SourceGridPoint.X - FinalDestination.X);
            double deltaY = Math.Abs(SourceGridPoint.Y - FinalDestination.Y);
            Point generalDirection = new Point(0,0);
            if (deltaX > deltaY)
            {
                if (SourceGridPoint.X > FinalDestination.X)
                    generalDirection = new Point(SourceGridPoint.X - 15, SourceGridPoint.Y);
                else
                    generalDirection = new Point(SourceGridPoint.X + 15, SourceGridPoint.Y);
            }
            else if (deltaY > deltaX)
            {
                if (SourceGridPoint.Y > FinalDestination.Y)
                    generalDirection = new Point(SourceGridPoint.X, SourceGridPoint.Y - 15);
                else
                    generalDirection = new Point(SourceGridPoint.X, SourceGridPoint.Y + 15);
            }
            else
            {
                YSourceGreaterThanFinalY = (SourceGridPoint.Y > FinalDestination.Y);
                XSourceGreaterThanFinalX = (SourceGridPoint.X > FinalDestination.X);

                if (YSourceGreaterThanFinalY && XSourceGreaterThanFinalX)
                    generalDirection = new Point(SourceGridPoint.X - 15, SourceGridPoint.Y - 10);
                else if (!YSourceGreaterThanFinalY && !XSourceGreaterThanFinalX)
                    generalDirection = new Point(SourceGridPoint.X + 15, SourceGridPoint.Y + 10);
                else if (!YSourceGreaterThanFinalY && XSourceGreaterThanFinalX)
                    generalDirection = new Point(SourceGridPoint.X - 15, SourceGridPoint.Y + 10);
                else
                    generalDirection = new Point(SourceGridPoint.X + 15, SourceGridPoint.Y - 10);

            }
            GridPoint closest = ClosestGridPointFromMainPage(generalDirection, SourceGridPoint, false, FinalDestination);
            if (!ValidatingGridPointForConnection(closest, IsInDeleteMode, RecursionRoot, ConnectorList, MainPageUpdateLayout, ComponentList, fixedList))
                return;
            XTarget = (int)(closest.X);
            YTarget = (int)(closest.Y);
            MakeArrowHead(/*GetCorrectTowardsDirection()*/);
            ConnectorModel cm = CreateNewConnectorAndAssociatedTasks(closest, SizeType, ConnectorList, EvaluateSystemFromMainPage);
            double StepDistanceMoved = Math.Abs(XSource - closest.X) + Math.Abs(YSource - closest.Y);
            if (!(closest == FinalDestination))
                cm.RecursiveRouteFinding(FinalDestination, IsInDeleteMode, SizeType, ConnectorList, MaxMovementAllowed, RecursionRoot, ClosestGridPointFromMainPage, EvaluateSystemFromMainPage, DistMovedSoFar + StepDistanceMoved, MainPageUpdateLayout, ComponentList, fixedList, StopConnectorTutorial1, StartConnectorTutorial2);
            else
            {
                if (MainPage.ConnectorTutorialIsRunning1)
                {
                    StopConnectorTutorial1();
                    StartConnectorTutorial2();
                }
            }
            MainPageUpdateLayout();
        }
        public ConnectorModel CreateNewConnectorAndAssociatedTasks(GridPoint closest, LevelSpecification.GridSizeTotal SizeType, System.Collections.ObjectModel.ObservableCollection<ConnectorModel> ConnectorList, Func<bool, bool> EvaluateSystemFromMainPage)
        {
            ConnectorModel cm = null;
            if (closest.Status == MainPage.GridStatus.isFree)
            {
                cm = new ConnectorModel(closest.X, closest.Y, SourceComponent, closest, this);
                ConnectorList.Add(cm);
                TargetConnector = cm;
            }
            else if (closest.Status == MainPage.GridStatus.gotFreeInputComponentPin)
            {
                closest.OccupyingComponent.In[closest.OccupyingComponentLegIndex] = SourceComponent;
                ReachedEnd = true;
                //only works for non-rotating components, with input always on top.
                SetCorrectConnectorEnding((int)SizeType);

                EvaluateSystemFromMainPage(true);

                //dummy to ensure we get correct information about the final gridpoint of a series of connectors
                cm = new ConnectorModel(closest.X, closest.Y, SourceComponent, closest, this);
                ConnectorList.Add(cm);
                TargetConnector = cm;
                cm.IsReadyToBeUsed = false;
            }

            SetColorToGreen(SourceComponent.HasInputs);


            IsReadyToBeUsed = false;
            SourceGridPoint.HoleVisibility = Visibility.Visible;
            if (SizeType != LevelSpecification.GridSizeTotal.large)
                SourceGridPoint.PointVisibility = System.Windows.Visibility.Collapsed;
            closest.Status = MainPage.GridStatus.gotConnector;
            return cm;
        }
        public bool ValidatingGridPointForConnection(GridPoint gridPoint, bool IsInDeleteMode, ConnectorModel RecursionRoot, System.Collections.ObjectModel.ObservableCollection<ConnectorModel> ConnectorList, Func<bool> MainPageUpdateLayout, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> ComponentList, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> fixedList)
        {
            if (gridPoint == null || IsInDeleteMode)
            {
                AbortAllConnections(RecursionRoot, ConnectorList, MainPageUpdateLayout);
                return false;
            }

            //sikre os imod cycles (kortslutning af kredsløbet)
            if (gridPoint.OccupyingComponent != null)
            {
                if (gridPoint.OccupyingComponent == SourceComponent)
                {
                    ConnectionFailure();
                    return false;
                }
                else if (SourceComponent is Component.Subcomponents.OutputSubComponentModel && gridPoint.OccupyingComponent is Component2oModel)
                {
                    if (((Component2oModel)gridPoint.OccupyingComponent).Primary == SourceComponent || ((Component2oModel)gridPoint.OccupyingComponent).Secondary == SourceComponent)
                    {
                        ConnectionFailure();
                        return false;
                    }
                }

                if( LooperChecker(SourceComponent.In[0], gridPoint, ComponentList, fixedList) || LooperChecker(SourceComponent.In[1], gridPoint, ComponentList, fixedList))
                {
                    ConnectionFailure();
	                return false;
                }                
            }
            return true;
        }
        public void ConnectionFailure()
        {
            XTarget = XSource;
            YTarget = YSource;
            MakeArrowHead(/*GetCorrectTowardsDirection(true)*/);

        }
        private void AbortAllConnections(ConnectorModel RecursionRoot, System.Collections.ObjectModel.ObservableCollection<ConnectorModel> ConnectorList, Func<bool> MainPageUpdateLayout)
        {
            ConnectorModel conmodel = this;
            while (conmodel != RecursionRoot)
            {
                conmodel.PrepareForDeletion(false, MainPageUpdateLayout);
                ConnectorList.Remove(conmodel);
                conmodel = conmodel.SourceConnector;
            }
            RecursionRoot.IsReadyToBeUsed = true;
            RecursionRoot.ReachedEnd = false;
            RecursionRoot.TargetConnector = null;
            RecursionRoot.XTarget = RecursionRoot.XSource;
            RecursionRoot.YTarget = RecursionRoot.YSource;
            RecursionRoot.HitBoxVisibility = Visibility.Visible;

            RecursionRoot.MakeArrowHead();
        }
        private bool LooperChecker(SuperComponentModel CompToCheck, GridPoint gridPoint, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> ComponentList, System.Collections.ObjectModel.ObservableCollection<MathComponentModel> fixedList)
        {
            if (CompToCheck == null || CompToCheck is Game2.Component.StaticComponents.BoardIn.BoardInModel)
	            return false;

            List<MathComponentModel> AllComps = new List<MathComponentModel>();
            AllComps.AddRange(ComponentList);
            AllComps.AddRange(fixedList);

            if (CompToCheck is Game2.Component.Subcomponents.OutputSubComponentModel)
	            CompToCheck = GetOwningComponent((Game2.Component.Subcomponents.OutputSubComponentModel)CompToCheck, AllComps);

            foreach(GridPoint gp in ((MathComponentModel)CompToCheck).CurrentAnchorPoint.Area.GridPointList)
            {
	            if (gp == gridPoint)
		            return true;
            }

            return (LooperChecker(CompToCheck.In[0], gridPoint, ComponentList, fixedList) || LooperChecker(CompToCheck.In[1], gridPoint, ComponentList, fixedList));
        }
        private SuperComponentModel GetOwningComponent(Game2.Component.Subcomponents.OutputSubComponentModel Comp, List<MathComponentModel> ComponentList)
        {
	        foreach(MathComponentModel component in ComponentList)
            {
		        if (component is Component2oModel)
                {
                    if (((Component2oModel)component).Primary == Comp || ((Component2oModel)component).Secondary == Comp)
				        return component;
                }
            }
	        return null;
        }
        #endregion

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
