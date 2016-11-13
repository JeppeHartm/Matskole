using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2.UI
{
    public partial class MainMenuChooseLevelUi : UserControl
    {
        public MainMenuChooseLevelUi()
        {
            InitializeComponent();

            App.Current.Host.Content.Resized += new EventHandler(Content_Resized);

            _disableLightning = false;
            Lightning3T1Storyboard.Begin();
            Spark1Storyboard.Begin();
        }

        public MainMenu MainMenu;
        public MainPage MainPage;
        private ListBox currentlySelectedListBox;
        private bool IsPerformingSelectionChanged = false;
        private bool _disableLightning = false;
        private Random RNG = new Random();
        private ListBox LastClickedListbox = null;
        private int LastClickedListboxIndex = -1;
        private int ListboxClickCount = 0;
        private ListBox _selectedListBox = null;
        private double _oldCenterY;
        private double _oldCenterX;
        private Visibility _fixedVisibilityOfChosenArea = Visibility.Collapsed;
        private Image _chosenAreaFixed;

        public void EnableFixesBasedOnProgress(int[] CompletedLevels)
        {
            if (CompletedLevels.Contains((LevelCollection.GetListFromSection(LevelSpecification.SectionTypes.Head).Count - 1) + ((int)LevelSpecification.SectionTypes.Head)))
                FixedHead.Visibility = System.Windows.Visibility.Visible;
            else
                FixedHead.Visibility = System.Windows.Visibility.Collapsed;
            
            if (CompletedLevels.Contains((LevelCollection.GetListFromSection(LevelSpecification.SectionTypes.Torso).Count - 1) + ((int)LevelSpecification.SectionTypes.Torso)))
                FixedTorso.Visibility = System.Windows.Visibility.Visible;
            else
                FixedTorso.Visibility = System.Windows.Visibility.Collapsed;

            if (CompletedLevels.Contains((LevelCollection.GetListFromSection(LevelSpecification.SectionTypes.LeftArm).Count - 1) + ((int)LevelSpecification.SectionTypes.LeftArm)))
                FixedLeftArm.Visibility = System.Windows.Visibility.Visible;
            else
                FixedLeftArm.Visibility = System.Windows.Visibility.Collapsed;

            if (CompletedLevels.Contains((LevelCollection.GetListFromSection(LevelSpecification.SectionTypes.RightArm).Count - 1) + ((int)LevelSpecification.SectionTypes.RightArm)))
                FixedRightArm.Visibility = System.Windows.Visibility.Visible;
            else
                FixedRightArm.Visibility = System.Windows.Visibility.Collapsed;

            if (FixedHead.Visibility == System.Windows.Visibility.Visible &&
                FixedTorso.Visibility == System.Windows.Visibility.Visible &&
                FixedLeftArm.Visibility == System.Windows.Visibility.Visible &&
                FixedRightArm.Visibility == System.Windows.Visibility.Visible)
                HappyFace.Visibility = System.Windows.Visibility.Visible;
            else
                HappyFace.Visibility = System.Windows.Visibility.Collapsed;
        }
               
        private void LevelList_KeyDown(object sender, KeyEventArgs e)
        {
            _selectedListBox = sender as ListBox;
            bool EnterPressed = false;
            if (e !=null)
                if (e.Key == Key.Enter)
                    EnterPressed = true;
            if (e == null || EnterPressed)
            {
                if (_selectedListBox.Items.Count > 0 && _selectedListBox.SelectedIndex >= 0 && ((ListBoxItem)_selectedListBox.SelectedItem).IsEnabled)
                {
                    _oldCenterX = this.ActualWidth;
                    _oldCenterY = this.ActualHeight;
                    ZoomDAx.RepeatBehavior = new RepeatBehavior(1);
                    ZoomDAy.RepeatBehavior = new RepeatBehavior(1);
                    ScaleDAx.RepeatBehavior = new RepeatBehavior(1);
                    ScaleDAy.RepeatBehavior = new RepeatBehavior(1);
                    BlackOutDA.BeginTime = new TimeSpan(0, 0, 0, 1, 800);
                    if(_selectedListBox == LevelsListHead)
                    {
                        ZoomDAx.To = 1262 + 90;
                        ZoomDAy.To = 476 + 10;
                        _chosenAreaFixed = FixedHead;
                        _fixedVisibilityOfChosenArea = FixedHead.Visibility;
                        FixedHead.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else if(_selectedListBox == LevelsListTorso)
                    {
                        ZoomDAx.To = 1207 + 70;
                        ZoomDAy.To = 715 + 30;
                        _chosenAreaFixed = FixedTorso;
                        _fixedVisibilityOfChosenArea = FixedTorso.Visibility;
                        FixedTorso.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else if(_selectedListBox == LevelsListRightArm)
                    {
                        ZoomDAx.To = 1388 + 130;
                        ZoomDAy.To = 787 + 50;
                        _chosenAreaFixed = FixedRightArm;
                        _fixedVisibilityOfChosenArea = FixedRightArm.Visibility;
                        FixedRightArm.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else if(_selectedListBox == LevelsListLeftArm)
                    {
                        ZoomDAx.To = 1043 + 60;
                        ZoomDAy.To = 806 + 60;
                        _chosenAreaFixed = FixedLeftArm;
                        _fixedVisibilityOfChosenArea = FixedLeftArm.Visibility;
                        FixedLeftArm.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if(_selectedListBox == LevelsListTest)
                    {
                        ZoomDAx.RepeatBehavior = new RepeatBehavior(0);
                        ZoomDAy.RepeatBehavior = new RepeatBehavior(0);
                        ScaleDAx.RepeatBehavior = new RepeatBehavior(0);
                        ScaleDAy.RepeatBehavior = new RepeatBehavior(0);
                        BlackOutDA.BeginTime = new TimeSpan(0, 0, 0);
                    }

                    BlackOut.Visibility = System.Windows.Visibility.Visible;
                    ZoomStoryboard.Begin();
                }
            }
        }
        private void LevelList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox SelectedListBox = sender as ListBox;
            if ((LastClickedListbox == SelectedListBox && LastClickedListboxIndex == SelectedListBox.SelectedIndex) || LastClickedListbox == null)
                ListboxClickCount++;
            else
                ListboxClickCount = 0;

            LastClickedListbox = SelectedListBox;
            LastClickedListboxIndex = SelectedListBox.SelectedIndex;

            if (ListboxClickCount >= 2)
            {
                ListboxClickCount = 0;
                LastClickedListboxIndex = -1;
                LastClickedListbox = null;
                LevelList_KeyDown(sender, null);
            }
        }


        #region SizeChanged events
        private void Content_Resized(object sender, EventArgs e)
        {
            this.Width = App.Current.Host.Content.ActualWidth;
            this.Height = App.Current.Host.Content.ActualHeight;
        }
        #endregion

        #region lightning animations

        private void Lightning3T1Storyboard_Completed(object sender, EventArgs e)
        {
            Lightning3T1Ani.BeginTime = new TimeSpan(0, 0, RNG.Next(1, 4));
            if (_disableLightning == false)
                Lightning3T1Storyboard.Begin();
        }

        private void Spark1Storyboard_Completed(object sender, EventArgs e)
        {
            Spark1Ani.BeginTime = new TimeSpan(0, 0, RNG.Next(1, 3));
            if (_disableLightning == false)
                Spark1Storyboard.Begin();
        }
        #endregion

        private void Blackout_Completed(object sender, EventArgs e)
        {
            int i;
            if (_selectedListBox == LevelsListHead)
                i = (int)LevelSpecification.SectionTypes.Head;
            else if (_selectedListBox == LevelsListTorso)
                i = (int)LevelSpecification.SectionTypes.Torso;
            else if (_selectedListBox == LevelsListLeftArm)
                i = (int)LevelSpecification.SectionTypes.LeftArm;
            else if (_selectedListBox == LevelsListRightArm)
                i = (int)LevelSpecification.SectionTypes.RightArm;
            else
                i = (int)LevelSpecification.SectionTypes.Custom;

            if (((ListBoxItem)_selectedListBox.SelectedItem) == MainMenu.EndlessLevelEasyListItem)
                MainPage.CreateLevel(LevelSpecification.GenerateRandomLevelSpec(0, MainPage.LvlIdForLastTutorialLevel));
            else if (((ListBoxItem)_selectedListBox.SelectedItem) == MainMenu.EndlessLevelMediumListItem)
                MainPage.CreateLevel(LevelSpecification.GenerateRandomLevelSpec(1, MainPage.LvlIdForLastTutorialLevel));
            else if (((ListBoxItem)_selectedListBox.SelectedItem) == MainMenu.EndlessLevelHardListItem)
                MainPage.CreateLevel(LevelSpecification.GenerateRandomLevelSpec(2, MainPage.LvlIdForLastTutorialLevel));
            else
                MainPage.CreateLevel(_selectedListBox.SelectedIndex + i);
            
            if (_chosenAreaFixed != null && _fixedVisibilityOfChosenArea != null)
                _chosenAreaFixed.Visibility = _fixedVisibilityOfChosenArea;
            this.BackGroundImg.Visibility = System.Windows.Visibility.Visible;
            MainPage.UnloadMainMenu();
            _disableLightning = true;
            MainMenu.MainMenuUiIsOnTop = true;
            MainPage.Focus();
            _selectedListBox = null;
            BlackOut.Opacity = 0;
            BlackOut.Visibility = System.Windows.Visibility.Collapsed;
            ZoomTransform.ScaleX = 1;
            ZoomTransform.ScaleY = 1;
            ZoomTransform.CenterX = _oldCenterX;
            ZoomTransform.CenterY = _oldCenterY;
        }

        

    }
}
