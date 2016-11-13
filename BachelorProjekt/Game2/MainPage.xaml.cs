using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Game2.Component;
using Game2.Component.StaticComponents;
using Game2.Component.StaticComponents.BoardIn;
using Game2.Component.StaticComponents.BoardOut;
using Game2.Component.PlayableComponent.Plus;
using Game2.Component.PlayableComponent.Minus;
using Game2.Component.PlayableComponent.Multiply;
using Game2.Component.PlayableComponent.Divide;
using Game2.Component.PlayableComponent.ConnectorCross;
using Game2.Component.PlayableComponent.Filter;
using Game2.Connector;
using Game2.UI;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using System.ComponentModel;
using Game2.Component.PlayableComponent.Splitter;
using Game2.Component.PlayableComponent.PlusMinusX;

namespace Game2
{
    public partial class MainPage : UserControl
    {
        public static readonly string Version = "1.0.0.132";

        static int boardsizeX = 1280;
        static int boardsizeY = 800;
        static int boardmarginX = 100;
        static int boardmarginY = 150;
        static int toolboxoffset = 100;

        public ObservableCollection<MathComponentModel> ComponentList { get; set; }
        public ObservableCollection<MathComponentModel> MovingComponentList { get; set; }
        public ObservableCollection<MathComponentModel> FixedComponentList { get; set; }
        public ObservableCollection<MathComponentModel> ToolboxList { get; set; }
        public ObservableCollection<MathComponentModel> ToolboxDummyList { get; set; }
        public ObservableCollection<ConnectorModel> ConnectorList { get; set; }
        public ObservableCollection<BoardInModel> BoardInList { get; set; }
        public ObservableCollection<BoardOutModel> BoardOutList { get; set; }

        public ObservableCollection<ScoreScreenModel> ScoreScreenContainer { get; set; }
        public ObservableCollection<MainMenuModel> MainMenuContainer { get; set; }
        public MainMenuModel UnloadedMainMenuContainer;

        public List<GridPoint> GridPoints;
        public List<GridPointArea> GridPointAreasLarge;
        public List<GridPointArea> GridPointAreasSmall;
        public List<AnchorPoint> AnchorPoints;
        public List<AnchorPoint> SmallAnchorPoints;
        public List<AnchorPoint> ToolboxAnchorPoints;
        public List<LevelSpecification> AvailableLevels;
        public enum GridStatus { isFree = 0, gotConnector = 1, gotComponent = 2, gotFreeInputComponentPin = 3, gotFreeOutputComponentPin = 4 }
        public enum ComponentSizeType { Large, Small }
        private LevelSpecification currentLevel;

        private Dictionary<LevelSpecification.ComponentType, int> placeInToolbox;
        private int _boardSizeX = 0;
        private int _boardSizeY = 0;
        public double CompSizeModifier;
        public LevelSpecification.GridSizeTotal SizeType = LevelSpecification.GridSizeTotal.large;
        public bool IsInDeleteMode = false;
        public string UserName = "";
        private bool LevelAlreadyLoaded = false;
        public BaseUserControlComponent CurrentlySelectedComponent = null;
        public bool SmartConnectorsOn = true;
        private Random RNG = new Random();
        private bool _gameIsPaused = false;
        private bool _toolBoxTutorialIsRunning1 = false;
        public static bool ConnectorTutorialIsRunning1 = false;
        public int LvlIdForLastTutorialLevel;
        private string _humanSolutionToCurrentLevel = "";
        private bool IsEndlessLevel = false;

        //fields for storing database info
        public int currentLevelIndex;
        private int currentLevelId;

        public ConnectorModel CurrentlyMovingConnector = null;

        private readonly bool _skipToolboxAnimations = false;
        public readonly bool SkipMainMenu = false;


        public MainPage()
        {
            ComponentList = new ObservableCollection<MathComponentModel> { };
            MovingComponentList = new ObservableCollection<MathComponentModel> { };
            FixedComponentList = new ObservableCollection<MathComponentModel> { };
            ToolboxList = new ObservableCollection<MathComponentModel> { };
            ToolboxDummyList = new ObservableCollection<MathComponentModel> { };
            ConnectorList = new ObservableCollection<ConnectorModel> { };
            BoardInList = new ObservableCollection<BoardInModel> { };
            BoardOutList = new ObservableCollection<BoardOutModel> { };
            GridPoints = new List<GridPoint>();
            GridPointAreasLarge = new List<GridPointArea>();
            GridPointAreasSmall = new List<GridPointArea>();
            AnchorPoints = new List<AnchorPoint>();
            SmallAnchorPoints = new List<AnchorPoint>();
            ToolboxAnchorPoints = new List<AnchorPoint>();
            AvailableLevels = new List<LevelSpecification>();
            placeInToolbox = new Dictionary<LevelSpecification.ComponentType,int>();

            ScoreScreenContainer = new ObservableCollection<ScoreScreenModel> { };
            MainMenuContainer = new ObservableCollection<MainMenuModel> { };
            InitializeComponent();
            InitializeNewMainMenu();

            Gear1Storyboard.Begin();
            Gear2Storyboard.Begin();
            Gear3Storyboard.Begin();

            App.Current.Host.Content.Resized += new EventHandler(Content_Resized);
        }

    
        #region public functions
        #region find points functions
        public AnchorPoint ClosestAnchorPoint(Point guess, ComponentSizeType requestType)
        {
            AnchorPoint result = null;
            List<AnchorPoint> toolbox = ToolboxAnchorPoints.ToList<AnchorPoint>();
            List<AnchorPoint> chosenAnchorPoints = (toolbox.Concat<AnchorPoint>(AnchorPoints)).ToList<AnchorPoint>();
            if (requestType == ComponentSizeType.Small)
                chosenAnchorPoints = (toolbox.Concat<AnchorPoint>(SmallAnchorPoints)).ToList<AnchorPoint>();
            double bestDist = 1000000000, newBestDist = 0;
            bool free;
            foreach (AnchorPoint p in chosenAnchorPoints)
            {
                free = true;
                foreach (GridPoint gp in p.Area.GridPointList)
                {
                    free = free && (gp.Status == GridStatus.isFree);
                }
                if (free || p.IsInToolbox)
                {
                    newBestDist = TestAndSet(guess, bestDist, p.X, p.Y);
                    if (bestDist != newBestDist)
                    {
                        result = p;
                        bestDist = newBestDist;
                    }
                }
            }

            if (result != null)
                TestLabel.Content = "x: " + (int)result.X + " y: " + (int)result.Y + " " + result.IsInToolbox;
            return result;
        }
        public GridPoint ClosestGridPoint(Point guess, GridPoint sourceGP, bool ReturnAbsoluteClosestGridPoint = false, GridPoint FinalDestination = null)
        {
            GridPoint result = null, gp = null;
            double bestDist = 1000000000, currentDist = 0;
            if (ReturnAbsoluteClosestGridPoint)
            {
                foreach (GridPoint gridpoint in GridPoints)
                {
                    currentDist = Math.Sqrt(Math.Pow(Math.Abs(gridpoint.X - guess.X), 2) + Math.Pow(Math.Abs(gridpoint.Y - guess.Y), 2));
                    if (currentDist < bestDist)
                    {
                        if (gridpoint.Status == GridStatus.isFree || gridpoint.Status == GridStatus.gotFreeInputComponentPin)
                        {
                            result = gridpoint;
                            bestDist = currentDist;
                        }
                    }
                }
                return result;
            }

            bool top = false, topRight = false, right = false, bottomRight = false, bottom = false, bottomLeft = false, left = false, topLeft = false;
            if (GridPoints.IndexOf(sourceGP) < _boardSizeX)
                top = true;
            if (GridPoints.IndexOf(sourceGP) == _boardSizeX - 1)
                topRight = true;
            if (((GridPoints.IndexOf(sourceGP) % _boardSizeX)) == _boardSizeX - 1)
                right = true;
            if (GridPoints.IndexOf(sourceGP) == ((_boardSizeX * _boardSizeY) - 1))
                bottomRight = true;
            if (GridPoints.IndexOf(sourceGP) >= ((_boardSizeX * (_boardSizeY - 1))))
                bottom = true;
            if (GridPoints.IndexOf(sourceGP) == ((_boardSizeX * (_boardSizeY - 1))))
                bottomLeft = true;
            if (GridPoints.IndexOf(sourceGP) % _boardSizeX == 0)
                left = true;
            if (GridPoints.IndexOf(sourceGP) == 0)
                topLeft = true;
            List<int> row = new List<int>() { -1, 0, 1 };
            List<int> col = new List<int>() { -1, 0, 1 };
            List<Tuple<int, int>> possibleGPs = new List<Tuple<int, int>>() 
            {
                Tuple.Create<int, int>(-1,0),  //Above source GridPoint
                Tuple.Create<int, int>(0,-1),   //Left of source GridPoint
                Tuple.Create<int, int>(0,1),    //Right of source GridPoint
                Tuple.Create<int, int>(1,0),   //Below source GridPoint
            };

            bool possibleGridPointNotFound = true;
            Tuple<int, int> attemptTuple;
            Tuple<int, int> tupleContainingResultPos = null;
            while (possibleGridPointNotFound)
            {
                foreach (int rowi in row)
                {
                    foreach (int coli in col)
                    {
                        attemptTuple = Tuple.Create<int, int>(rowi, coli);
                        if (possibleGPs.Contains(attemptTuple))
                        {
                            if (!(
                                (rowi == -1 && top) || (rowi == -1 && topLeft) || (rowi == -1 && topRight) || (coli == 1 && topRight) || (coli == -1 && topLeft)
                                || (coli == -1 && left) || (coli == 1 && right)
                                || (rowi == 1 && bottom) || (rowi == 1 && bottomLeft) || (rowi == 1 && bottomLeft) || (coli == 1 && bottomRight) || (coli == -1 && bottomLeft)
                                ))
                            {
                                gp = GridPoints[GridPoints.IndexOf(sourceGP) + rowi * _boardSizeX + coli];
                                currentDist = Math.Sqrt(Math.Pow(Math.Abs(gp.X - guess.X), 2) + Math.Pow(Math.Abs(gp.Y - guess.Y), 2));
                                if (currentDist < bestDist)
                                {
                                    tupleContainingResultPos = attemptTuple;
                                    result = gp;
                                    //result.parentWindow = this; collection code
                                    bestDist = currentDist;
                                }
                            }
                        }
                    }
                }
                if (FinalDestination == null)
                    possibleGridPointNotFound = false;
                else
                {
                    if (result == FinalDestination)
                        possibleGridPointNotFound = false;
                    else if (result.Status == GridStatus.isFree)
                    {
                        possibleGridPointNotFound = false;
                    }
                    else
                    {
                        if (tupleContainingResultPos == null)
                            return null;
                        possibleGPs.Remove(tupleContainingResultPos);
                        bestDist = 100000000;
                        attemptTuple = null;
                        tupleContainingResultPos = null;
                    }
                }
                if (possibleGPs.Count == 0)
                    return null;
            }
            if (!(result.Status == GridStatus.isFree || result.Status == GridStatus.gotFreeInputComponentPin))
                return null;

            return result;

        }
        #endregion
        public void UnloadMainMenu()
        {
            if (MainMenuContainer.Count < 1)
                return;
            UnloadedMainMenuContainer = MainMenuContainer[0];
            MainMenuContainer.RemoveAt(0);
            Clock.Content = "00:00";
            InitiateConveyorBeltAnimations();
        }

        public void CreateLevel(int lvlid)
        {
            foreach (LevelSpecification ls in AvailableLevels)
            {
                if (ls.LevelNumber == lvlid)
                {
                    CreateLevel(ls);
                    break;
                }
            }
        }
        public void CreateLevel(LevelSpecification ls)
        {
            if (LevelAlreadyLoaded)
                FlushLevel();
            CreateLevelFromSpecs(currentLevel = ls);
            _humanSolutionToCurrentLevel = ls.HumanSolution;
            IsEndlessLevel = ls.IsEndlessLevel;
        }

        public bool EvaluateSystem(bool CheckConnectors = true)
        {
            if (_toolBoxTutorialIsRunning1 && ComponentList.Count > 0)
            {
                StopToolboxTutorial1();
                StartToolboxTutorial2();
            }


            bool result = true;

            Component.MathComponentModel[] AllComponents = new Component.MathComponentModel[ComponentList.Count + FixedComponentList.Count];
            ComponentList.CopyTo(AllComponents, 0);
            FixedComponentList.CopyTo(AllComponents, ComponentList.Count);
            foreach (Component.MathComponentModel mcm in AllComponents)
            {
                #region Setting the output value of components if possible
                mcm.OutFontSize = (int)(10 * CompSizeModifier);
                if (mcm.HasInputs)
                {                
                    mcm.LeftOut = "" + mcm.GetOut();
                    if (mcm is Component2oModel)
                    {
                        ((Component2oModel)mcm).RightOut = "" + ((Component2oModel)mcm).GetSecondaryOut();
                    }
                }
                else
                {
                    mcm.LeftOut = "  ";
                    if (mcm is Component2oModel)
                    {
                        ((Component2oModel)mcm).RightOut = "  ";
                    }
                }
                #endregion
                if (CheckConnectors)
                {
                    foreach (ConnectorModel conmodel in ConnectorList)
                    {
                        if (mcm is Component2oModel)
                        {
                            if (conmodel.SourceComponent == ((Component2oModel)mcm).Primary || conmodel.SourceComponent == ((Component2oModel)mcm).Secondary)
                            {
                                conmodel.SetColorToGreen(mcm.HasInputs);
                            }
                        }
                        else
                        {
                            if (conmodel.SourceComponent == mcm)
                            {
                                conmodel.SetColorToGreen(mcm.HasInputs);
                            }
                        }
                    }
                }
                if (mcm.inLimit == 2)
                {
                    if (mcm is ConnectorCrossComponentModel)
                    {
                        #region ConnectorCorss special case
                        if (mcm.In[0] == null && mcm.In[1] == null)
                            mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        else if (mcm.In[0] != null && mcm.In[1] == null)
                        {
                            if (mcm.In[0].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltCrossGreenRed;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                        else if (mcm.In[0] == null && mcm.In[1] != null)
                        {
                            if (mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltCrossRedGreen;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                        else
                        {
                            if (mcm.In[0].HasInputs && mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltAllGreen;
                            else if(!mcm.In[0].HasInputs && mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltCrossRedGreen;
                            else if(mcm.In[0].HasInputs && !mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltCrossGreenRed;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                        #endregion
                    }
                    else
                    {
                        if (mcm.In[0] == null && mcm.In[1] == null)
                            mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        else if (mcm.In[0] != null && mcm.In[1] == null)
                        {
                            if (mcm.In[0].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltGreenRed;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                        else if (mcm.In[0] == null && mcm.In[1] != null)
                        {
                            if (mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltRedGreen;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                        else
                        {
                            if (mcm.In[0].HasInputs && mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltAllGreen;
                            else if (!mcm.In[0].HasInputs && mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltRedGreen;
                            else if (mcm.In[0].HasInputs && !mcm.In[1].HasInputs)
                                mcm.ImgSourcePath = mcm.ImgUriAltGreenRed;
                            else
                                mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                        }
                    }
                }
                else
                {
                    if (mcm.In[0] == null)
                        mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                    else
                    {
                        if (mcm.In[0].HasInputs)
                            mcm.ImgSourcePath = mcm.ImgUriAltAllGreen;
                        else
                            mcm.ImgSourcePath = mcm.ImgUriAltAllRed;
                    }
                }
                if (mcm.HasInputs == false)
                    result = false;
            }

            foreach (BoardOutModel bom in BoardOutList)
            {
                if (bom.IsSatisfied())
                    bom.OutColor = ConnectorModel.Green;
                else
                    bom.OutColor = ConnectorModel.Red;
            }

            UpdateLayout();

            return result;
        }

        public int GetPlaceInToolbox(LevelSpecification.ComponentType type)
        {
            return placeInToolbox[type];
        }

        public void GetOutResult()
        {
            int satisfied = 0;
            int points = 0;
            foreach (BoardOutModel bo in BoardOutList)
            {
                if (bo.HasInputs)
                    TestLabel.Content = "Expected output: " + bo.ExpectedOutput + ", got: " + bo.GetOut() + (bo.IsSatisfied() ? ". Game has been won. Hooooray!" : "");
                if (bo.IsSatisfied()) satisfied++;
            }
            if (satisfied == BoardOutList.Count)
            {
                points += 100 * BoardInList.Count * BoardInList.Count;
                points -= 25 * ComponentList.Count * ComponentList.Count;
                points -= ClockCounter;
                TestLabel.Content += " points: " + points;
                int placedCounter = 0;
                List<DatabaseFunctions.CompletedLevelData.UsedCompData> l = new List<DatabaseFunctions.CompletedLevelData.UsedCompData>();
                foreach(MathComponentModel mcm in ComponentList)
                {
                    if(!mcm.IsInToolbox){
                        placedCounter++;
                        int index = -1;
                        int mcmid = (int)GetTypeFromModel(mcm);
                        foreach(DatabaseFunctions.CompletedLevelData.UsedCompData c in l){
                            if(c.compid == mcmid){
                                index = l.IndexOf(c);
                                break;
                            }
                        }
                        if(index == -1){
                            l.Add(new DatabaseFunctions.CompletedLevelData.UsedCompData(mcmid,1));
                        }else{
                            l[index].amount++;
                        }                    
                    }
                }
                try
                {
                    
                    DatabaseFunctions.registerCompletedLevel(UserName, currentLevel.LevelNumber, currentLevelIndex++, ClockCounter, l);
                }
                catch (System.Security.SecurityException se)
                {
                    MessageBox.Show("Ingen uploads når programmet kører lokalt");
                }
                ScoreScreenContainer.Add(new ScoreScreenModel(points, 0, ClockCounter, placedCounter, this, currentLevel.LevelNumber, UnloadedMainMenuContainer.CompletedLevels));
                UnloadedMainMenuContainer.CompletedLevels.Add(currentLevel.LevelNumber);
                PauseButton.IsEnabled = false;

            }
        }

        public void SetComponentsZIndexOnTop()
        {
            Canvas.SetZIndex(ComponentItemControl, 3);
        }
        public void SetComponentsZIndexOnBottom()
        {
            Canvas.SetZIndex(ComponentItemControl, 1);
        }

        public bool MainPageUpdateLayout()
        {
            UpdateLayout();
            return true;
        }

        public void StartBackgroundMusic()
        {
            BackgroundMusicLoop.Stop();
            BackgroundMusicLoop.Play();
        }
        public void StopBackgroundMusic()
        {
            BackgroundMusicLoop.Stop();
        }
        #endregion

        private void CreateLevelFromSpecs(LevelSpecification ls)
        {
            //Create gridpoints
            int dimX = (int)ls.GridSize.Item1, dimY = (int)ls.GridSize.Item2;
            _boardSizeX = dimX;
            _boardSizeY = dimY;
            
            switch (ls.GridSize.Item1)
            {
                case LevelSpecification.GridSizeX.small:
                    SizeType = LevelSpecification.GridSizeTotal.small;
                    CompSizeModifier = 3;
                    break;
                case LevelSpecification.GridSizeX.medium:
                    SizeType = LevelSpecification.GridSizeTotal.medium;
                    CompSizeModifier = 1.55;
                    break;
                case LevelSpecification.GridSizeX.large:
                default:
                    SizeType = LevelSpecification.GridSizeTotal.large;
                    CompSizeModifier = 1;
                    break;
            }            

            CreateGridPoints(dimX, dimY);
            //Create gridpointareas and anchorpoints
            CreateGPAsAndAPs(dimX, dimY);

            CreateToolbox(dimX,dimY,ls.Toolbox.Count);
            //Populate the toolboxlists

            CreateToolboxComponents(ls);
            //Fix fixed components
            CreateFixedComponents(ls);

            CreateBoardIns(ls);

            CreateBoardOuts(ls);

            LevelAlreadyLoaded = true;
            currentLevelId = ls.LevelNumber;
            

            if (ls.LevelNumber == 0)
                StartConnectorTutorial1();
            if (ls.LevelNumber == 1)
                StartToolboxTutorial1();

            
        }
        #region helper functions for lvlalgorithm
        private void CreateBoardOuts(LevelSpecification ls)
        {
            //this logic is assuming X sizes of small=6, medium=12, large=18
            List<int> placements = GetBoardInOutPlacements(ls.BoardOut.Count);
            for (int i = 0; i < placements.Count; i++)
            {
                BoardOutModel b = new BoardOutModel(ls.BoardOut[i], CompSizeModifier);
                b.OutColor = ConnectorModel.Red;
                GridPoint g = GridPoints[placements[i]+((_boardSizeY-1)*(int)ls.GridSize.Item1)];
                b.Attach(g);
                b.AttachedGridPoint = g;
                BoardOutList.Add(b);
            }
            UpdateLayout();
        }
        private List<int> GetBoardInOutPlacements(int WhichBoardCount)
        {
            List<int> placements = new List<int>();
            #region logic for deciding placements, depending on boardsize and board ins.
            //this logic is assuming X sizes of small=6, medium=12, large=18
            switch (SizeType)
            {
                case LevelSpecification.GridSizeTotal.small:
                    switch (WhichBoardCount)
                    {
                        case 1:
                            placements.AddRange(new List<int>() { 3 });
                            break;
                        case 2:
                        default:
                            placements.AddRange(new List<int>() { 2, 4 });
                            break;
                    }
                    break;
                case LevelSpecification.GridSizeTotal.medium:
                    switch (WhichBoardCount)
                    {
                        case 1:
                            placements.AddRange(new List<int>() { 5 });
                            break;
                        case 2:
                            placements.AddRange(new List<int>() { 4, 6 });
                            break;
                        case 3:
                            placements.AddRange(new List<int>() { 2, 5, 8 });
                            break;
                        case 4:
                            placements.AddRange(new List<int>() { 2, 4, 6, 8 });
                            break;
                        case 5:
                        default:
                            placements.AddRange(new List<int>() { 1, 3, 5, 7, 9 });
                            break;
                    }
                    break;
                case LevelSpecification.GridSizeTotal.large:
                default:
                    switch (WhichBoardCount)
                    {
                        case 1:
                            placements.AddRange(new List<int>() { 8 });
                            break;
                        case 2:
                            placements.AddRange(new List<int>() { 4, 12 });
                            break;
                        case 3:
                            placements.AddRange(new List<int>() { 4, 8, 12 });
                            break;
                        case 4:
                            placements.AddRange(new List<int>() { 3, 7, 11, 15 });
                            break;
                        case 5:
                            placements.AddRange(new List<int>() { 3, 6, 9, 12, 15 });
                            break;
                        case 6:
                            placements.AddRange(new List<int>() { 1, 4, 7, 10, 13, 16 });
                            break;
                        case 7:
                            placements.AddRange(new List<int>() { 3, 5, 7, 9, 11, 13, 15 });
                            break;
                        case 8:
                        default:
                            placements.AddRange(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 15 });
                            break;
                    }
                    break;
            }
            #endregion
            return placements;
        }
        private void CreateBoardIns(LevelSpecification ls)
        {
            //this logic is assuming X sizes of small=6, medium=12, large=18
            List<int> placements = GetBoardInOutPlacements(ls.BoardIn.Count);            

            for (int i = 0; i < placements.Count; i++)
            {
                BoardInModel b = new BoardInModel(ls.BoardIn[i], CompSizeModifier);
                GridPoint g = GridPoints[placements[i]];
                b.Attach(g);
                ConnectorList.Add(new Connector.ConnectorModel(g.X, g.Y, b, g));
                BoardInList.Add(b);
            }

            UpdateLayout();
        }

        private void CreateToolbox(int dimX, int dimY, int toolboxSize)
        {
            List<GridPoint> tbgp;
            double x1, x2, y1, y2, AdditionalYDistBetweenComps = 20;
            Point pointofap; ;
            for (int i = 0; i < toolboxSize; i++)
            {
                x1 = boardsizeX + boardmarginX + toolboxoffset;
                x2 = x1 + boardsizeX / dimX;
                y1 = (boardmarginY + i * boardsizeY / dimY)  + AdditionalYDistBetweenComps * i;
                y2 = (y1 + boardsizeY / dimY)  + AdditionalYDistBetweenComps * i;
                tbgp = new List<GridPoint>();
                tbgp.Add(new GridPoint(x1, y1));
                tbgp.Add(new GridPoint(x1, y2));
                tbgp.Add(new GridPoint(x2, y1));
                tbgp.Add(new GridPoint(x2, y2));
                pointofap = new Point(x1 + (x2 - x1) / 2, (y1 + (y2 - y1) / 2));

                ToolboxAnchorPoints.Add( new AnchorPoint(pointofap.X, pointofap.Y, true, new GridPointArea(tbgp)));
                
            }
        }
        private void CreateFixedComponents(LevelSpecification ls)
        {
            foreach (LevelSpecification.FixedComponent c in ls.FixedComponents)
            {
                MathComponentModel m;
                Point placement = new Point();
                placement.X = boardmarginX + c.x * boardsizeX / (int)ls.GridSize.Item1;
                placement.Y = boardmarginY + c.y * boardsizeY / (int)ls.GridSize.Item2;
                switch (c.type)
                {
                    case LevelSpecification.ComponentType.plus:
                        m = new PlusComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier) { };
                        break;
                    case LevelSpecification.ComponentType.minus:
                        m = new MinusComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.multiply:
                        m = new MultiplyComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.divide:
                        m = new DivideComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter1:
                        m = new FilterComponentModel(1, ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter10:
                        m = new FilterComponentModel(10, ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter100:
                        m = new FilterComponentModel(100, ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.connectorcross:
                        m = new ConnectorCrossComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.splitter:
                        m = new SplitterComponentModel(ClosestAnchorPoint(placement, ComponentSizeType.Large), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.plus1:
                        m = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.PlusFunc,ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);                        
                        break;
                    case LevelSpecification.ComponentType.plus10:
                        m = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.PlusFunc, ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.plus100:
                        m = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.PlusFunc, ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus1:
                        m = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.MinusFunc, ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus10:
                        m = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.MinusFunc, ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus100:
                        m = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.MinusFunc, ClosestAnchorPoint(placement, ComponentSizeType.Small), CompSizeModifier);
                        break;
                    default:
                        m = null;
                        break;
                }
                m.CurrentAnchorPoint.SnapAndSetStatus(m);
                m.Locked = true;
                m.LockVisibility = System.Windows.Visibility.Visible;
                FixedComponentList.Add(m);
            }
            UpdateLayout();
        }

        private void CreateToolboxComponents(LevelSpecification ls)
        {
            int anchorPointer = 0;
            foreach (LevelSpecification.ComponentType c in ls.Toolbox)
            {
                MathComponentModel m, d;

                switch (c)
                {
                    case LevelSpecification.ComponentType.plus:
                        m = new PlusComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier) { };
                        d = new PlusComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier) { };
                        break;
                    case LevelSpecification.ComponentType.minus:
                        m = new MinusComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new MinusComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.multiply:
                        m = new MultiplyComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new MultiplyComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.divide:
                        m = new DivideComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new DivideComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter1:
                        m = new FilterComponentModel(1, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new FilterComponentModel(1, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter10:
                        m = new FilterComponentModel(10, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new FilterComponentModel(10, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.filter100:
                        m = new FilterComponentModel(100, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new FilterComponentModel(100, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.connectorcross:
                        m = new ConnectorCrossComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new ConnectorCrossComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.splitter:
                        m = new SplitterComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new SplitterComponentModel(ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.plus1:
                        m = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.plus10:
                        m = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.plus100:
                        m = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.PlusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus1:
                        m = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(1, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus10:
                        m = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(10, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    case LevelSpecification.ComponentType.minus100:
                        m = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        d = new PlusMinusXComponentModel(100, PlusMinusXComponentModel.MinusFunc, ToolboxAnchorPoints[anchorPointer], CompSizeModifier);
                        break;
                    default:
                        m = null;
                        d = null;
                        break;
                }
                m.X = (int)m.CurrentAnchorPoint.X - (m.CompWidth / 2);
                m.Y = (int)m.CurrentAnchorPoint.Y - (m.CompHeight / 2);
                m.IsInToolbox = true;
                d.X = (int)d.CurrentAnchorPoint.X - (d.CompWidth / 2);
                d.Y = (int)d.CurrentAnchorPoint.Y - (d.CompHeight / 2);
                d.Playable = false;
                ToolboxList.Add(m);
                ToolboxDummyList.Add(d);
                placeInToolbox.Add(c, anchorPointer);
                anchorPointer++;
            }
            UpdateLayout();
        }

        private void CreateGPAsAndAPs(int dimX, int dimY)
        {
            List<GridPoint> gps = null, gpslarge = null;
            GridPointArea small = null, large = null;
            GridPoint gp1 = null, gp2 = null;
            for (int i = 0; i < GridPoints.Count; i++)
            {
                if (i < (dimY - 1) * dimX)
                {
                    bool lastCol = (i + 1) % dimX == 0;
                    gps = new List<GridPoint>();
                    gp1 = GridPoints[i];
                    gp2 = GridPoints[i + dimX];
                    gps.Add(gp1);
                    gps.Add(gp2);
                    small = new GridPointArea(gps);
                    GridPointAreasSmall.Add(small);
                    SmallAnchorPoints.Add(new AnchorPoint(GridPoints[i].X, GridPoints[i].Y + (GridPoints[i + dimX].Y - GridPoints[i].Y) / 2, lastCol, small));
                    if (!lastCol)
                    {
                        gpslarge = new List<GridPoint>();
                        gpslarge.Add(gp1);
                        gpslarge.Add(gp2);
                        gpslarge.Add(GridPoints[i + 1]);
                        gpslarge.Add(GridPoints[i + dimX + 1]);
                        large = new GridPointArea(gpslarge);
                        GridPointAreasLarge.Add(large);
                        AnchorPoints.Add(new AnchorPoint(GridPoints[i].X + (GridPoints[i + 1].X - GridPoints[i].X) / 2, GridPoints[i].Y + (GridPoints[i + dimX].Y - GridPoints[i].Y) / 2, lastCol, large));
                    }
                    
                    
                }
            }
        }

        private void CreateGridPoints(int dimX, int dimY)
        {

            Color gpcolor = Color.FromArgb(255, 192, 192, 192);

            double x = 0, y = 0;
            double wmod = boardsizeX / dimX;
            double hmod = boardsizeY / dimY;
            #region graphics details - board
            Border backBorder = new Border();
            backBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            backBorder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            double borderThickness = 8;
            backBorder.BorderThickness = new Thickness(borderThickness);
            backBorder.BorderBrush = new System.Windows.Media.SolidColorBrush(Colors.Black);
            Rectangle back = new Rectangle();
            backBorder.Child = back;
            UnderlyingCanvas.Children.Add(backBorder);

            Uri imgscrew = new Uri("/Game2;Component/Images/Screw.png", UriKind.Relative);
            int basesize = 20;

            Image hole1 = new Image(), hole2 = new Image(), hole3 = new Image(), hole4 = new Image();
            hole1.Source = new BitmapImage(imgscrew);
            hole1.Height = basesize * CompSizeModifier;
            hole1.Width = basesize * CompSizeModifier;
            hole1.Margin = new Thickness(boardmarginX - ((wmod + hole1.Width) / 2), boardmarginY - ((hmod + hole1.Height) / 2), 0, 0);
            UnderlyingCanvas.Children.Add(hole1);

            hole2.Source = new BitmapImage(imgscrew);
            hole2.Height = basesize * CompSizeModifier;
            hole2.Width = basesize * CompSizeModifier;
            hole2.Margin = new Thickness(boardsizeX + boardmarginX - ((wmod + hole2.Width) / 2) + (borderThickness / 2), boardmarginY - ((hmod + hole2.Height) / 2), 0, 0);
            UnderlyingCanvas.Children.Add(hole2);

            hole3.Source = new BitmapImage(imgscrew);
            hole3.Height = basesize * CompSizeModifier;
            hole3.Width = basesize * CompSizeModifier;
            hole3.Margin = new Thickness(boardmarginX - ((wmod + hole3.Width) / 2), boardsizeY + boardmarginY - ((hmod + hole3.Height) / 2) + (borderThickness / 2), 0, 0);
            UnderlyingCanvas.Children.Add(hole3);

            hole4.Source = new BitmapImage(imgscrew);
            hole4.Height = basesize * CompSizeModifier;
            hole4.Width = basesize * CompSizeModifier;
            hole4.Margin = new Thickness(boardsizeX + boardmarginX - ((wmod + hole4.Width) / 2) + (borderThickness / 2), boardsizeY + boardmarginY - ((hmod + hole4.Height) / 2) + (borderThickness / 2), 0, 0);
            UnderlyingCanvas.Children.Add(hole4);

            //back.Fill = new SolidColorBrush(/*Color.FromArgb(0xff,0x4f,0xa0,0x65)*/Color.FromArgb(255, 110, 170, 110));
            back.Fill = new SolidColorBrush(/*Color.FromArgb(0xff,0x4f,0xa0,0x65)*/Color.FromArgb(255, 34, 139, 34));
            back.Height = boardsizeY + (hole1.Height / 2);
            back.Width = boardsizeX + (hole1.Width / 2);
            back.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            back.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            
            //backBorder.Margin = new Thickness(boardmarginX - hole1.Width * 2, boardmarginY - hole1.Height * 2, 0, 0);
            int correction = 2;
            backBorder.Margin = new Thickness((boardmarginX - hole1.Width * 2) - (borderThickness / 2) - correction, boardmarginY - hole1.Height * 2 - (borderThickness / 2), 0, 0); 

            #endregion
            for (int h = 0; h < dimY; h++)
            {
                y = h * hmod;
                for (int w = 0; w < dimX; w++)
                {
                    x = w * wmod;
                    
                    if ((w + 1) < dimX && (h + 1) < dimY)
                    {
                        Image tile = new Image();
                        tile.Source = new BitmapImage(new Uri("/Game2;Component/Images/Tile3.png", UriKind.Relative));
                        tile.Height = hmod;
                        tile.Width = wmod;
                        tile.Margin = new Thickness(x + boardmarginX, y + boardmarginY, 0, 0);
                        UnderlyingCanvas.Children.Add(tile);
                    }
                    

                    Ellipse visualGP = new Ellipse() { Fill = new SolidColorBrush(gpcolor), Width = 7 * CompSizeModifier, Height = 7 * CompSizeModifier };
                    visualGP.Margin = new Thickness(x + boardmarginX - (visualGP.Width / 2), y + boardmarginY - (visualGP.Height / 2), 0, 0);
                    Ellipse visualGPHole = new Ellipse() { Fill = new SolidColorBrush(Colors.Black), Width = 7, Height = 7, Visibility = System.Windows.Visibility.Collapsed };
                    visualGPHole.Margin = new Thickness(x + boardmarginX - (visualGPHole.Width / 2), y + boardmarginY - (visualGPHole.Height / 2), 0, 0);
                    UnderlyingCanvas.Children.Add(visualGP);

                    UnderlyingCanvas.Children.Add(visualGPHole);
                    

                    

                    GridPoint gp = new GridPoint(x + boardmarginX, y + boardmarginY) { VisualGP = Tuple.Create<Ellipse, Ellipse>(visualGP, visualGPHole) };
                    GridPoints.Add(gp);

                }

            }
            UpdateLayout();
        }

        #endregion

        #region private helper functions, events & animations
        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {           
            IsInDeleteMode = !IsInDeleteMode;
            if (IsInDeleteMode)
            {
                GridOfViewBox.Cursor = Cursors.None;
                GridOfViewBox.SetValue(Game2.CustomCursor.CustomCursor.CursorTemplateProperty, Resources["DeleteCursor"] as DataTemplate);
                TestLabel.Content = "delete mode on";
            }
            else
            {
                GridOfViewBox.SetValue(Game2.CustomCursor.CustomCursor.CursorTemplateProperty, null);
                GridOfViewBox.Cursor = null;
                TestLabel.Content = "delete mode off";
            }
        }
        public void ScoreScreen_Continue()
        {
            if (currentLevel.LevelNumber == 0)
            {
                StopConnectorTutorial2();
            }
            else if (currentLevel.LevelNumber == 1)
            {
                StopToolboxTutorial2();
            }
            LoadMainMenu();
            PauseButton.IsEnabled = true;
        }
        private void Button_Click_GoToMainMenu(object sender, RoutedEventArgs e)
        {
            MessageBoxResult gotomainmenu = MessageBox.Show("Er du sikker på at du vil gå til hovedmenuen? Spillet vil ikke gemme hvor langt du er nået i denne bane.", "Er du sikker", MessageBoxButton.OKCancel);
            MessageBoxResult solution;
            if (gotomainmenu == MessageBoxResult.OK)
            {
                if (IsEndlessLevel)
                {
                    solution = MessageBox.Show("Vil du se løsningen?", "Løsningen", MessageBoxButton.OKCancel);
                    if (solution == MessageBoxResult.OK)
                    {
                        MessageBox.Show(_humanSolutionToCurrentLevel);
                    }
                }
                StopConnectorTutorial1();
                StopToolboxTutorial1();
                StopConnectorTutorial2();
                StopToolboxTutorial2();
                LoadMainMenu();
            }
        }
        private List<int> _indicisOfCollapsedGPs = new List<int>();
        private List<int> _indicisOfCollapsedGPsAndHoles = new List<int>();
        private void Button_Click_Pause(object sender, RoutedEventArgs e)
        {
            TogglePause(true);
        }

        private void TogglePause(bool isTruePause = false)
        {
            if (_gameIsPaused)
            {
                GameClockTimer.Start();
                GrayOut.Visibility = System.Windows.Visibility.Collapsed;
                

                ComponentItemControl.Visibility = System.Windows.Visibility.Visible;
                ConnectorItemControl.Visibility = System.Windows.Visibility.Visible;
                ToolboxItemControl.Visibility = System.Windows.Visibility.Visible;
                ToolboxDummyItemControl.Visibility = System.Windows.Visibility.Visible;
                Gear1Storyboard.Resume();
                Gear2Storyboard.Resume();
                Gear3Storyboard.Resume();
                Lightning1T2Storyboard.Resume();

                foreach (int i in _indicisOfCollapsedGPs)
                {
                    GridPoints[i].PointVisibility = System.Windows.Visibility.Collapsed;
                    GridPoints[i].HoleVisibility = System.Windows.Visibility.Visible;
                }
                _indicisOfCollapsedGPs.Clear();
                _indicisOfCollapsedGPs.Capacity = 75;

                foreach (int i in _indicisOfCollapsedGPsAndHoles)
                    GridPoints[i].PointVisibility = System.Windows.Visibility.Collapsed;
                _indicisOfCollapsedGPsAndHoles.Clear();
                _indicisOfCollapsedGPsAndHoles.Capacity = 25;
            }
            else
            {
                if (GameClockTimer.IsEnabled)
                    GameClockTimer.Stop();
                if (isTruePause)
                    GrayOut.Visibility = System.Windows.Visibility.Visible;
                ComponentItemControl.Visibility = System.Windows.Visibility.Collapsed;
                ConnectorItemControl.Visibility = System.Windows.Visibility.Collapsed;
                ToolboxItemControl.Visibility = System.Windows.Visibility.Collapsed;
                ToolboxDummyItemControl.Visibility = System.Windows.Visibility.Collapsed;
                Gear1Storyboard.Pause();
                Gear2Storyboard.Pause();
                Gear3Storyboard.Pause();
                Lightning1T2Storyboard.Pause();

                for (int i = 0; i < GridPoints.Count; i++)
                {
                    if (GridPoints[i].PointVisibility == System.Windows.Visibility.Collapsed && GridPoints[i].HoleVisibility == System.Windows.Visibility.Visible)
                    {
                        _indicisOfCollapsedGPs.Add(i);
                        GridPoints[i].PointVisibility = System.Windows.Visibility.Visible;
                        GridPoints[i].HoleVisibility = System.Windows.Visibility.Collapsed;
                    }
                    else if (GridPoints[i].PointVisibility == System.Windows.Visibility.Collapsed && GridPoints[i].HoleVisibility == System.Windows.Visibility.Collapsed)
                    {
                        _indicisOfCollapsedGPsAndHoles.Add(i);
                        GridPoints[i].PointVisibility = System.Windows.Visibility.Visible;
                        GridPoints[i].HoleVisibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }


            _gameIsPaused = !_gameIsPaused;
            
            DeleteButton.IsEnabled = !_gameIsPaused;
            GoToMainMenuButton.IsEnabled = !_gameIsPaused;
            SoundCheckBox.IsEnabled = !_gameIsPaused;

            UpdateLayout();
        }


        private void Button_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Spillet, robotværkstedet, er designet og implementeret af Jeppe Hartmund og Andreas Slott Jensenius. \nAlt undtagen følgende er deres egen kreation:\nBilleder af robotten, laboratoriet og baggrunden er lavet af Christian Kaiser.\nThrottledMouseEvent af Colin Eberhardt http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/ \nCustomCursor af Morten Nielsen http://www.sharpgis.net/post/2011/05/09/Custom-Cursors-in-Silverlight.aspx \nMusikken i spillet er fra Blizzard Entertainments World of Warcraft. \nDen gule gnist er fra http://www.crestock.com/image/1735253-three-star.aspx \nDet mørkeblå lyn er fra http://fanna1119.deviantart.com/art/transparent-Lightning-207664962 \nDet lyseblå lyn er fra http://www.cartographersguild.com/mapping-elements/7343-lightning-spark-request.html \nBilledet af låsen er fra http://openclipart.org/detail/22179/lock-by-nicubunu \nLyden fra røgeksplosionen er fra http://soundbible.com/107-bomb-explosion-1.html\nLyden fra transportbåndet er fra http://www.youtube.com/watch?v=__hp_fIwaPo \n\nAlle rettigheder tilfalder skaberne af dem. Dette spil er ikke kommercielt, vil ikke blive brugt til at tjene penge og for at bruge/se/høre de førnævnte genstande(med undtagelse af dem lavet af Christian Kaiser hvis tilladelse vi har) kræver det et brugernavn og password som ikke er offentligt tilgængeligt.");
        }

        private int ClockCounter = 0;
        private System.Windows.Threading.DispatcherTimer GameClockTimer = null;
        private void StartGameClock()
        {
            ClockCounter = 0;
            if (GameClockTimer == null)
            {
                GameClockTimer = new System.Windows.Threading.DispatcherTimer();
                GameClockTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                GameClockTimer.Tick += new EventHandler(Each_Tick);
            }
            GameClockTimer.Start();
        }
        private void PauseGameClock()
        {
            if (GameClockTimer.IsEnabled)
                GameClockTimer.Stop();
        }
        private void Each_Tick(object o, EventArgs sender)
        {
            ClockCounter++;
            int minCounter = (int)(Math.Floor((ClockCounter) / 60)), 
                secCounter = ClockCounter - (minCounter * 60);
            string minString = "", secString = "";

            if (secCounter < 10)
                secString = "0" + secCounter.ToString();
            else
                secString = secCounter.ToString();

            if (ClockCounter < 60)
                Clock.Content = "00:"+secString;
            else
            {
                if (minCounter < 10)
                    minString = "0"+minCounter.ToString();
                else
                    minString = minCounter.ToString();

                Clock.Content = minString + ":" + secString;            
              
            }
        }

        private void Content_Resized(object sender, EventArgs e)
        {
            this.Height = App.Current.Host.Content.ActualHeight;
            this.Width = App.Current.Host.Content.ActualWidth;
        }

        private double TestAndSet(Point guess, double bestDist, double x, double y)
        {
            double currentDist = Math.Abs((x - guess.X)) + Math.Abs((y - guess.Y));
            if (currentDist < bestDist)
            {
                bestDist = currentDist;
            }
            return bestDist;
        }
        private double CheckIfMaxAndFix(double number, double max)
        {
            if (number > max)
                return max;
            else
                return number;
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            if (SkipMainMenu)
            {
                UpdateLayout();
                CreateLevelFromSpecs(LevelSpecification.GetTestLevelSpec()); //test code
                UpdateLayout();
                InitiateConveyorBeltAnimations();
                UpdateLayout();
            }
            else
            {
                List<LevelSpecification> TutorialLvls = LevelSpecification.GetLevelSpecForSection(LevelSpecification.SectionTypes.Custom);
                foreach (LevelSpecification ls in TutorialLvls)
                {
                    AvailableLevels.Add(ls);
                }

                LvlIdForLastTutorialLevel = TutorialLvls.Count - 1;
                AvailableLevels.Add(new LevelSpecification(LevelSpecification.GridSizeTotal.large, null, null, null, null, LevelSpecification.SectionTypes.Custom, new List<int>() { LvlIdForLastTutorialLevel }, -1, true, 0));
                AvailableLevels.Add(new LevelSpecification(LevelSpecification.GridSizeTotal.large, null, null, null, null, LevelSpecification.SectionTypes.Custom, new List<int>() { LvlIdForLastTutorialLevel }, -2, true, 1));
                AvailableLevels.Add(new LevelSpecification(LevelSpecification.GridSizeTotal.large, null, null, null, null, LevelSpecification.SectionTypes.Custom, new List<int>() { LvlIdForLastTutorialLevel }, -3, true, 2));

                List<LevelSpecification> HeadLvls = LevelSpecification.GetLevelSpecForSection(LevelSpecification.SectionTypes.Head);
                foreach (LevelSpecification ls in HeadLvls)
                {
                    AvailableLevels.Add(ls);
                }
                List<LevelSpecification> TorsoLvls = LevelSpecification.GetLevelSpecForSection(LevelSpecification.SectionTypes.Torso);
                foreach (LevelSpecification ls in TorsoLvls)
                {
                    AvailableLevels.Add(ls);
                }
                List<LevelSpecification> LeftArmLvls = LevelSpecification.GetLevelSpecForSection(LevelSpecification.SectionTypes.LeftArm);
                foreach (LevelSpecification ls in LeftArmLvls)
                {
                    AvailableLevels.Add(ls);
                }
                List<LevelSpecification> RightArmLvls = LevelSpecification.GetLevelSpecForSection(LevelSpecification.SectionTypes.RightArm);
                foreach (LevelSpecification ls in RightArmLvls)
                {
                    AvailableLevels.Add(ls);
                }

            }


            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = System.Windows.Media.Color.FromArgb(0xFF, 0x22, 0x22, 0x22);
            for (int i = 0; i < ConveyorBeltLength; i++)
            {
                ConveyorBelt.Children.Add(new Rectangle() { Width = 250, Height = 7, Stretch = Stretch.Fill, Fill = new SolidColorBrush(brush.Color) });
            }
            ConveyorBelt.Children.Add(new Rectangle() { Width = 250, Height = 7, Stretch = Stretch.Fill, Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent) });
  
            UpdateLayout();
            EndOfConveyorBeltBlocker.Margin = new Thickness(1375, ConveyorBelt.ActualHeight + 50 + 5, 0, 0);
            UpdateLayout();
        }
        private void FlushLevel()
        {
            foreach (GridPoint gp in GridPoints)
            {
                UnderlyingCanvas.Children.Remove(gp.VisualGP.Item1);
                UnderlyingCanvas.Children.Remove(gp.VisualGP.Item2);
                gp.VisualGP = null;
            }
            GridPoints.Clear();
            ConnectorList.Clear();
            ComponentList.Clear();
            FixedComponentList.Clear();
            BoardInList.Clear();
            BoardOutList.Clear();
            ToolboxList.Clear();
            ToolboxDummyList.Clear();
            GridPointAreasLarge.Clear();
            GridPointAreasSmall.Clear();
            AnchorPoints.Clear();
            SmallAnchorPoints.Clear();
            placeInToolbox.Clear();
            ToolboxAnchorPoints.Clear();
            ScoreScreenContainer.Clear();
            LevelAlreadyLoaded = false;
        }
        private void LoadMainMenu()
        {
            if (MainMenuContainer.Count > 1)
                return;
            if (UnloadedMainMenuContainer == null)
                return;
            MainMenuContainer.Add(UnloadedMainMenuContainer);
            UnloadedMainMenuContainer = null;
            if (GameClockTimer != null)
                PauseGameClock();
 
        }

        private void InitializeNewMainMenu()
        {
            if (SkipMainMenu)
            {
                UpdateLayout();
                return;
            }
            MainMenuModel mmm = new MainMenuModel();
            UnloadedMainMenuContainer = mmm;
            LoadMainMenu();
        }

        #region conveyor belt animations
        private int ConveyorBeltDuration = 0;
        private int ConveyorBeltEndDuration = 10;
        private int ConveyorBeltLength = 11;
        private void InitiateConveyorBeltAnimations()
        {
            Lightning1T2Storyboard.Begin();
            bool NoCompsInToolox = false;
            if (ToolboxList.Count == 0)
                NoCompsInToolox = true;
            if (!_skipToolboxAnimations && NoCompsInToolox == false)
            {
                AnimationCleanup();
                PauseButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                GoToMainMenuButton.IsEnabled = false;
                ConveyorBeltDuration = 0;
                ConveyorBeltEndDuration = 10;
                ConveyorBeltLength = 11;
                ToolboxItemControl.IsEnabled = false;
                ToolboxItemControl.Opacity = 0;
                ToolboxDummyItemControl.Opacity = 0;
                ConveyorBeltBoxesCounter = 0;
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).Completed -= ConveyorBeltBoxesAnimation_Completed;

                ConveyorBeltEndDuration = (int)((ToolboxAnchorPoints[ToolboxAnchorPoints.Count - 1].Y) / 70);

                ConveyorBeltBoxes.Children.Add(new Image() { Margin = new Thickness(0, 0, 0, 0), Width = ToolboxList[0].CompWidth * 1.0, Height = ToolboxList[0].CompHeight * 1.0, Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Game2;Component/Images/box2.png", UriKind.Relative)) });
                for (int i = 0; i < ToolboxAnchorPoints.Count - 1; i++)
                {
                    ConveyorBeltBoxes.Children.Add(new Image() { Width = ToolboxList[0].CompWidth * 1.0, Height = ToolboxList[0].CompHeight * 1.0, Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Game2;Component/Images/box2.png", UriKind.Relative)) });
                }
                UpdateLayout();
         
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).From = -(ConveyorBeltBoxes.ActualHeight - boardmarginY);
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).By = 70;
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).AutoReverse = false;
                ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).Completed += ConveyorBeltBoxesAnimation_Completed;
                ConveyorBeltBoxesStoryboard.Begin();

                ConveyorBeltStoryboard.Begin();

                ConveyorBeltSound.Stop();
                ConveyorBeltSound.Play();
            }
            else
            {
                ToolboxItemControl.Opacity = 1;
                ToolboxDummyItemControl.Opacity = 1;
                ToolboxItemControl.IsEnabled = true;

                StartGameClock();
                PauseButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                GoToMainMenuButton.IsEnabled = true;
            }
        }
        private int ConveyorBeltBoxesCounter = 0;
        private void ConveyorBeltBoxesAnimation_Completed(object sender, EventArgs e)
        {
            ConveyorBeltBoxesCounter++;
            ((DoubleAnimation)ConveyorBeltBoxesStoryboard.Children[0]).From = (-(ConveyorBeltBoxes.ActualHeight - boardmarginY)) + (ConveyorBeltBoxesCounter - 1) * 70;
            if (ConveyorBeltBoxesCounter < ConveyorBeltEndDuration)
                ConveyorBeltBoxesStoryboard.Begin();
        }

        private void ConveyorBeltStoryboard_Completed(object sender, EventArgs e)
        {
            ConveyorBeltDuration++;
            if (ConveyorBeltDuration < ConveyorBeltEndDuration)
            {
                ConveyorBeltStoryboard.Begin();           
            }
            else
            {
                ConveyorBeltBoxesStoryboard.Pause();
                ConveyorBeltSound.Stop();


                Random rng = new Random();
                Storyboard sb;

                ObservableCollection<MathComponentModel> collection;
                ItemsControl ic;
                for (int i = 0; i < ConveyorBeltBoxes.Children.Count; i++)
                {
                    collection = new ObservableCollection<MathComponentModel>();
                    collection.Add(ToolboxDummyList[i]);
                    _dummyCollections.Add(collection);
                    ic = new ItemsControl();
                    ic.ItemsSource = _dummyCollections[_dummyCollections.Count - 1];
                    _fadingItemsControlList.Add(ic);
                }

                List<int> delay = new List<int>();
                int j;
                for (int i = 0; i < ConveyorBeltBoxes.Children.Count; i++)
                {
                    j = rng.Next(0, ConveyorBeltBoxes.Children.Count) * 400;
                    while (delay.Contains(j))
                        j = rng.Next(0, ConveyorBeltBoxes.Children.Count) * 400;
                    delay.Add(j);

                }
                for (int i = 0; i < ConveyorBeltBoxes.Children.Count; i++)
                {
                    sb = new Storyboard();
                    sb.Duration = new Duration(TimeSpan.FromSeconds(0));
                    sb.RepeatBehavior = new RepeatBehavior(1);
                    sb.BeginTime = new TimeSpan(0, 0, 0, 0, delay[i]);
                    _smokeMapping.Add(sb, (Image)ConveyorBeltBoxes.Children[i]);
                    sb.Completed += MakeToSmoke;
                    LayoutRoot.Resources.Add("sb" + sb.GetHashCode(), sb);
                    _conveyorBeltBoxesStoryboards.Add(sb);
                    sb.Begin();
                }
            }
        }
        private Dictionary<Storyboard, Image> _smokeMapping = new Dictionary<Storyboard, Image>();
        private List<ItemsControl> _fadingItemsControlList = new List<ItemsControl>();
        private List<ObservableCollection<MathComponentModel>> _dummyCollections = new List<ObservableCollection<MathComponentModel>>();
        private List<Storyboard> _conveyorBeltBoxesStoryboards = new List<Storyboard>();
        private void MakeToSmoke(object sender, EventArgs e)
        {
            Image boxImg, smokeImg = new Image();
            bool b = _smokeMapping.TryGetValue((Storyboard)sender, out boxImg);
            boxImg.Opacity = 0;
            smokeImg.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Game2;Component/Images/smoke2.png", UriKind.Relative));
            smokeImg.Height = boxImg.Height * 1.4;
            smokeImg.Width = boxImg.Width * 1.4;
            smokeImg.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            smokeImg.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            smokeImg.Stretch = Stretch.Fill;
            SuperComponentModel whichComp = ToolboxDummyList[ConveyorBeltBoxes.Children.IndexOf(boxImg)];
            smokeImg.Margin = new Thickness(whichComp.X - (whichComp.CompWidth / 4), whichComp.Y + (whichComp.CompHeight / 2), 0, 0);
            GridOfViewBox.Children.Add(smokeImg);
            UpdateLayout();
            SmokeExplosion.Stop();
            SmokeExplosion.Play();

            double animationLength = 1.8;
            ExponentialEase easeFunc = new ExponentialEase();
            easeFunc.EasingMode = EasingMode.EaseIn;

            Storyboard sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.FromSeconds(animationLength));
            sb.RepeatBehavior = new RepeatBehavior(1);
            LayoutRoot.Resources.Add("sb" + sb.GetHashCode(), sb);
            _conveyorBeltBoxesStoryboards.Add(sb);

            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = 1;
            da1.To = 0;
            da1.EasingFunction = easeFunc;
            da1.AutoReverse = false;
            Storyboard.SetTarget(da1, smokeImg);
            Storyboard.SetTargetProperty(da1, new PropertyPath(UIElement.OpacityProperty));
            da1.Duration = new Duration(TimeSpan.FromSeconds(animationLength));

            DoubleAnimation da2 = new DoubleAnimation();
            da2.By = smokeImg.Width * 0.5;
            Storyboard.SetTarget(da2, smokeImg);
            Storyboard.SetTargetProperty(da2, new PropertyPath(Image.WidthProperty));
            da2.Duration = new Duration(TimeSpan.FromSeconds(animationLength));
            da2.AutoReverse = true;

            DoubleAnimation da3 = new DoubleAnimation();
            da3.By = smokeImg.Height * 0.5;
            Storyboard.SetTarget(da3, smokeImg);
            Storyboard.SetTargetProperty(da3, new PropertyPath(Image.HeightProperty));
            da3.Duration = new Duration(TimeSpan.FromSeconds(animationLength));
            da3.AutoReverse = true;


            DoubleAnimation da4;
            ItemsControl ic = _fadingItemsControlList[ConveyorBeltBoxes.Children.IndexOf(boxImg)];
            MainCanvas.Children.Add(ic);
            da4 = new DoubleAnimation();
            da4.To = 1;
            da4.AutoReverse = false;
            da4.EasingFunction = easeFunc;
            Storyboard.SetTarget(da4, ic);
            Storyboard.SetTargetProperty(da4, new PropertyPath(UIElement.OpacityProperty));
            da4.Duration = new Duration(TimeSpan.FromSeconds(animationLength));            


            sb.Children.Add(da1);
            sb.Children.Add(da2);
            sb.Children.Add(da3);
            sb.Children.Add(da4);

            sb.Begin();
            _smokeMapping.Add(sb, smokeImg);
            sb.Completed += ShowComps;
        }
        private int _animationFinishedCounter = 0;
        private List<Image> _smokeImgCollection = new List<Image>();
        private void ShowComps(object sender, EventArgs e)
        {
            Image smokeImg;
            bool b = _smokeMapping.TryGetValue((Storyboard)sender, out smokeImg);
            smokeImg.Visibility = Visibility.Collapsed;
            _animationFinishedCounter++;
            _smokeImgCollection.Add(smokeImg);

            if (_animationFinishedCounter >= ConveyorBeltBoxes.Children.Count)
            {
                ToolboxItemControl.Opacity = 1;
                ToolboxDummyItemControl.Opacity = 1;
                ToolboxItemControl.IsEnabled = true;
                AnimationCleanup();
                StartGameClock();
                PauseButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                GoToMainMenuButton.IsEnabled = true;
            }
        }
        private void AnimationCleanup()
        {
            foreach (Image img in _smokeImgCollection)
            {
                GridOfViewBox.Children.Remove(img);
            }
            _smokeImgCollection.Clear();
            foreach (ItemsControl ic in _fadingItemsControlList)
            {
                MainCanvas.Children.Remove(ic);
            }
            _fadingItemsControlList.Clear();
            ConveyorBeltBoxes.Children.Clear();
            foreach (ObservableCollection<MathComponentModel> oc in _dummyCollections)
            {
                oc.Clear();
            }
            _dummyCollections.Clear();
            foreach (Storyboard sb in _conveyorBeltBoxesStoryboards)
            {
                LayoutRoot.Resources.Remove("sb" + sb.GetHashCode());
            }
            _conveyorBeltBoxesStoryboards.Clear();

            _smokeMapping.Clear();
            _animationFinishedCounter = 0;
        } 
        #endregion

        private void Lightning1T2Storyboard_Completed(object sender, EventArgs e)
        {
            Lightning1T2Ani.BeginTime = new TimeSpan(0, 0, RNG.Next(15, 35));
            int i = RNG.Next(1, 20);
            if (i > 14)
                RotTransLightning1T2.Angle = -10;
            else if (i < 6)
                RotTransLightning1T2.Angle = -20;
            else
                RotTransLightning1T2.Angle = 0;
            Lightning1T2Storyboard.Begin();
        }

        private void BackgroundMusicLoop_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusicLoop.Stop();
            BackgroundMusicLoop.Play();
        }

        private Dictionary<string, double> _oldSoundValues = new Dictionary<string, double>();
        private void SoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            double smokesoundvol = 0, conveyorsoundvol = 0, backgroundsoundvol = 0;
            bool SoundValuesSuccess = true;

            if (SmokeExplosion == null)
                return;

            SoundValuesSuccess = SoundValuesSuccess && _oldSoundValues.TryGetValue(SmokeExplosion.Name, out smokesoundvol);
            SoundValuesSuccess = SoundValuesSuccess && _oldSoundValues.TryGetValue(ConveyorBeltSound.Name, out conveyorsoundvol);
            SoundValuesSuccess = SoundValuesSuccess && _oldSoundValues.TryGetValue(BackgroundMusicLoop.Name, out backgroundsoundvol);

            if (SoundValuesSuccess)
            {
                SmokeExplosion.Volume = smokesoundvol;
                ConveyorBeltSound.Volume = conveyorsoundvol;
                BackgroundMusicLoop.Volume = backgroundsoundvol;
            }
        }
        private void SoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _oldSoundValues.Clear();
            _oldSoundValues.Add(SmokeExplosion.Name, SmokeExplosion.Volume);
            _oldSoundValues.Add(ConveyorBeltSound.Name, ConveyorBeltSound.Volume);
            _oldSoundValues.Add(BackgroundMusicLoop.Name, BackgroundMusicLoop.Volume);

            SmokeExplosion.Volume = 0;
            ConveyorBeltSound.Volume = 0;
            BackgroundMusicLoop.Volume = 0;
        }

        private void StartToolboxTutorial1()
        {
            _toolBoxTutorialIsRunning1 = true;
            ToolTuTLabel.Visibility = System.Windows.Visibility.Visible;
            ToolTutContainer.Visibility = System.Windows.Visibility.Visible;
            ToolTutStoryboard.Begin();       
        }
        private void StartConnectorTutorial1()
        {
            ConnectorTutorialIsRunning1 = true;
            ConTuTLabel.Visibility = System.Windows.Visibility.Visible;
            ConTutContainer.Visibility = System.Windows.Visibility.Visible;
            ConTutStoryboard.Begin();
        }

        private void StopToolboxTutorial1()
        {
            _toolBoxTutorialIsRunning1 = false;
            ToolTuTLabel.Visibility = System.Windows.Visibility.Collapsed;
            ToolTutContainer.Visibility = System.Windows.Visibility.Collapsed;
            ToolTutStoryboard.Stop();            
        }
        public bool StopConnectorTutorial1()
        {
            ConnectorTutorialIsRunning1 = false;
            ConTuTLabel.Visibility = System.Windows.Visibility.Collapsed;
            ConTutContainer.Visibility = System.Windows.Visibility.Collapsed;
            ConTutStoryboard.Stop();
            return true;
        }

        private void StartToolboxTutorial2()
        {
            ToolTuT2Label.Visibility = System.Windows.Visibility.Visible;
        }
        public bool StartConnectorTutorial2()
        {
            ConTuT2Label.Visibility = System.Windows.Visibility.Visible;
            return true;
        }

        private void StopToolboxTutorial2()
        {
            ToolTuT2Label.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void StopConnectorTutorial2()

        {
            ConTuT2Label.Visibility = System.Windows.Visibility.Collapsed;
        }
        private LevelSpecification.ComponentType GetTypeFromModel(MathComponentModel model)
        {
            if (model is PlusComponentModel)
                return LevelSpecification.ComponentType.plus;
            else if (model is MinusComponentModel)
                return LevelSpecification.ComponentType.minus;
            else if (model is DivideComponentModel)
                return LevelSpecification.ComponentType.divide;
            else if (model is MultiplyComponentModel)
                return LevelSpecification.ComponentType.multiply;
            else if (model is ConnectorCrossComponentModel)
                return LevelSpecification.ComponentType.connectorcross;
            else if (model is PlusMinusXComponentModel)
            {
                if (((PlusMinusXComponentModel)model).X == 1)
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus1;
                    else
                        return LevelSpecification.ComponentType.minus1;
                }
                if (((PlusMinusXComponentModel)model).X == 10)
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus10;
                    else
                        return LevelSpecification.ComponentType.minus10;
                }
                if (((PlusMinusXComponentModel)model).X == 100)
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus100;
                    else
                        return LevelSpecification.ComponentType.minus100;
                }
            }
            else if (model is SplitterComponentModel)
                return LevelSpecification.ComponentType.splitter;
            else if (model is FilterComponentModel)
            {
                if (((FilterComponentModel)model).Filters == 1)
                    return LevelSpecification.ComponentType.filter1;
                else if (((FilterComponentModel)model).Filters == 10)
                    return LevelSpecification.ComponentType.filter10;
                else
                    return LevelSpecification.ComponentType.filter100;
            }

            return LevelSpecification.ComponentType.divide;
        }
        #endregion

    }
}
