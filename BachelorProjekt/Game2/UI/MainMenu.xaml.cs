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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game2.UI
{
    public partial class MainMenu : UserControl
    {
        public DatabaseFunctions.UserData currentUserData;
        public bool IsUserBoy = true;
        public bool MainMenuUiIsOnTop = true;
        public int[] completedLevels;
        public bool UnlockAll = false;
        public ListBoxItem EndlessLevelEasyListItem;
        public ListBoxItem EndlessLevelMediumListItem;
        public ListBoxItem EndlessLevelHardListItem;
        public bool TeacherClientHaventBeenOpenedBefore = true;
        public MainMenu()
        {
            InitializeComponent();

            App.Current.Host.Content.Resized += new EventHandler(Content_Resized);
            versionText.Text = "Version "+MainPage.Version;
            Gear1Storyboard.Begin();
            Gear2Storyboard.Begin();


        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
            double width = ContainingGrid.ActualWidth, height = ContainingGrid.ActualHeight;
            #region tiles
            int dim = 100;
            Image img;
            for (int i = 0; i < (width / dim); i++)
            {
                for (int j = 0; j < (height / dim); j++)
                {
                    img = new Image();
                    img.Source = new BitmapImage(new Uri("/Game2;Component/Images/Tile3.png", UriKind.Relative));
                    img.Height = dim;
                    img.Width = dim;
                    img.Margin = new Thickness(dim * i, dim * j, 0, 0);
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    UnderlyingGrid.Children.Add(img);
                }
            } 
            #endregion

            UpdateLayout();
            #region screws
            int screwSize = 20, screwOffset = 5;
            for (int i = 0; i < 4; i++)
            {
                img = new Image();
                img.Source = new BitmapImage(new Uri("/Game2;Component/Images/Screw.png", UriKind.Relative));
                img.Height = screwSize;
                img.Width = screwSize;
                switch (i)
                {
                    case 0:
                        img.Margin = new Thickness(screwOffset + (screwSize / 2), screwOffset + (screwSize / 2), 0, 0);
                        break;
                    case 1:
                        img.Margin = new Thickness(width - (screwOffset), screwOffset + (screwSize / 2), 0, 0);
                        break;
                    case 2:
                        img.Margin = new Thickness(screwOffset + (screwSize / 2), height - (screwOffset), 0, 0);
                        break;
                    case 3:
                        img.Margin = new Thickness(width - (screwOffset), height - (screwOffset), 0, 0);
                        break;
                }                
                img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                UnderlyingGrid.Children.Add(img);
            }
            #endregion

            UpdateLayout();
            #region grid points
            Color gpcolor = Color.FromArgb(255, 192, 192, 192);

            double x = 0, y = 0, gpOffsetX = 55, gpOffsetY = 47.5, amountOfGPsX = 10, amountOfGPsY = 6;
            double wmod = (width) / amountOfGPsX;
            double hmod = (height) / amountOfGPsY;

            for (int h = 0; h < amountOfGPsY; h++)
            {
                y = h * hmod;
                for (int w = 0; w < amountOfGPsX; w++)
                {
                    x = w * wmod;


                    Ellipse visualGP = new Ellipse() { Fill = new SolidColorBrush(gpcolor), Width = 7, Height = 7, 
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left, VerticalAlignment = System.Windows.VerticalAlignment.Top };
                    visualGP.Margin = new Thickness(x + gpOffsetX - (visualGP.Width / 2), y + gpOffsetY - (visualGP.Height / 2), 0, 0);
                    UnderlyingGrid.Children.Add(visualGP);
                }

            }
            UpdateLayout();
            #endregion

        }


        private void Button_Click_NewContinue(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;
            //når dette skal laves så se på choose level func
            MessageBox.Show("Systemet til at gemme fremskidt igennem spillet er ikke færdigt. Klik på \"Vælg en bestemt bane\"-knappen for at starte spillet.");
            Gear1Storyboard.Pause();
            Gear2Storyboard.Pause();
        }

        private void Button_Click_Highscore(object sender, RoutedEventArgs e)
        {
            return;
            if (MainMenuUiIsOnTop == false)
                return;
            MessageBox.Show("Highscore systemet er ikke færdigt.");

        }

        private void Button_Click_Logout(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;
            MainPage parentWindow = (MainPage)Application.Current.RootVisual;
            parentWindow.MainMenuContainer[0].IsLoggedIn = false;
            parentWindow.MainMenuContainer[0].StartButtonContent = "Start et nyt spil!";
            parentWindow.MainMenuContainer[0].AdminUserVisibility = System.Windows.Visibility.Collapsed;
        }

        private async void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;
            
            MainPage parentWindow = (MainPage)Application.Current.RootVisual;
            MainMenuModel MainMenuModel = parentWindow.MainMenuContainer[0];

            MainMenuModel.LoginFieldUsername = UsernameTextBox.Text;
            MainMenuModel.LoginFieldPassword = PasswordTextBox.Password;
            bool valid = false;
            try
            {
                completedLevels = await DatabaseFunctions.login(MainMenuModel.LoginFieldUsername, MainMenuModel.LoginFieldPassword);
                valid = DatabaseFunctions.httpResponse.Equals("1");
                currentUserData = await DatabaseFunctions.getUserData(MainMenuModel.LoginFieldUsername, true, false);
                parentWindow.currentLevelIndex = await DatabaseFunctions.getMaxIndex(MainMenuModel.LoginFieldUsername);

            }
            catch (System.Security.SecurityException se)
            {
                MessageBox.Show("loginsystemet virker ikke fra din egen computer");
                DatabaseFunctions.httpResponse = "1";
                valid = true;
                completedLevels = new int[0];
                currentUserData = new DatabaseFunctions.UserData();
                currentUserData.classid = "0";
                currentUserData.isMale = true;
                currentUserData.isTeacher = true;
                currentUserData.password = "Dev123";
                currentUserData.username = "Dev";

                parentWindow.MainMenuContainer[0].AllLevelsCompleted = true;
            }
            if (!valid)
            {
                MessageBox.Show("Brugernavn eller password er forkert.");
                return;
            }
            MainMenuModel.IsLoggedIn = true;
            parentWindow.BackgroundMusicLoop.Stop();
            parentWindow.BackgroundMusicLoop.Play();

            parentWindow.UserName = MainMenuModel.LoginFieldUsername;
            if (false/*ask database if user has started the game, if true do:*/)
                parentWindow.MainMenuContainer[0].StartButtonContent = "Fortsæt spillet fra hvor du er kommet til!";
            if (currentUserData.isTeacher)
            {
                parentWindow.MainMenuContainer[0].UserIsAdmin = true;
                parentWindow.MainMenuContainer[0].AdminUserVisibility = System.Windows.Visibility.Visible;
            }
            else
                parentWindow.MainMenuContainer[0].AdminUserVisibility = System.Windows.Visibility.Collapsed;


            IsUserBoy = currentUserData.isMale;

            if (IsUserBoy)
                RobotType.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Game2;Component/Images/Robot.png", UriKind.Relative));
            else
                RobotType.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Game2;Component/Images/Robot.png", UriKind.Relative));

            Random r = new Random();
            switch (r.Next(5))
            {
                case 1:
                    MainMenuModel.UsernameMessage = "Hej " + parentWindow.UserName + "! Er du klar til at hjælpe mig?";
                    break;
                case 2:
                    MainMenuModel.UsernameMessage = "... Uh oh, jeg havde nær ikke set dig " + parentWindow.UserName + "! Skal vi komme igang?";
                    break;
                case 3:
                    MainMenuModel.UsernameMessage = "Hvad så " + parentWindow.UserName + "? Håber du har haft det godt. Skal vi begynde?";
                    break;
                case 4:
                    MainMenuModel.UsernameMessage = "Endelig er du tilbage " + parentWindow.UserName + "! Lad os se at komme videre!";
                    break;
                default:
                    MainMenuModel.UsernameMessage = "Halløjsa " + parentWindow.UserName + "! Er du frisk på at fixe mig?";
                    break;
            }
        }

        private void Button_Click_CreateUser(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;
            if(false/*ask database if username is available, if false do:*/)
            {
                MessageBox.Show("Brugernavnet er allerede taget.");
                return;
            }
            //put new user and associated password into database
            MessageBox.Show("Du kan ikke oprette en bruger lige nu.");

        }

        private void Button_Click_ChooseLevel(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;
            MainPage MainPage = (MainPage)Application.Current.RootVisual;
            ChooseLevelUi.MainMenu = this;
            ChooseLevelUi.MainPage = MainPage;

            string lines;

            foreach (LevelSpecification ls in MainPage.AvailableLevels)
            {
                if ((ls.LevelNumber - (int)ls.SectionType) > 7)
                    lines = "--";
                else
                    lines = "---";
                switch (ls.SectionType)
                {
                    case LevelSpecification.SectionTypes.Head:
                        ChooseLevelUi.LevelsListHead.Items.Add(new ListBoxItem() { Content = lines + " Level " + (ls.LevelNumber - ((int)LevelSpecification.SectionTypes.Head) + 1) + " " + lines, IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 });
                        break;
                    case LevelSpecification.SectionTypes.RightArm:
                        ChooseLevelUi.LevelsListRightArm.Items.Add(new ListBoxItem() { Content = lines + " Level " + (ls.LevelNumber - ((int)LevelSpecification.SectionTypes.RightArm) + 1) + " " + lines, IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 });
                        break;
                    case LevelSpecification.SectionTypes.LeftArm:
                        ChooseLevelUi.LevelsListLeftArm.Items.Add(new ListBoxItem() { Content = lines + " Level " + (ls.LevelNumber - ((int)LevelSpecification.SectionTypes.LeftArm) + 1) + " " + lines, IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 });
                        break;
                    case LevelSpecification.SectionTypes.Torso:
                        ChooseLevelUi.LevelsListTorso.Items.Add(new ListBoxItem() { Content = lines + " Level " + (ls.LevelNumber - ((int)LevelSpecification.SectionTypes.Torso) + 1) + " " + lines, IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 });
                        break;
                    case LevelSpecification.SectionTypes.Custom:
                        if (ls.IsEndlessLevel && ls.EndlessLevelDifficulty == 0)
                        {
                            EndlessLevelEasyListItem = new ListBoxItem() { Content = "ny nem bane", IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 };
                            ChooseLevelUi.LevelsListTest.Items.Add(EndlessLevelEasyListItem);
                        }
                        else if (ls.IsEndlessLevel && ls.EndlessLevelDifficulty == 1)
                        {
                            EndlessLevelMediumListItem = new ListBoxItem() { Content = "ny midt bane", IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 };
                            ChooseLevelUi.LevelsListTest.Items.Add(EndlessLevelMediumListItem);
                        }
                        else if (ls.IsEndlessLevel && ls.EndlessLevelDifficulty == 2)
                        {
                            EndlessLevelHardListItem = new ListBoxItem() { Content = "ny svær bane", IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 };
                            ChooseLevelUi.LevelsListTest.Items.Add(EndlessLevelHardListItem);
                        }
                        else
                            ChooseLevelUi.LevelsListTest.Items.Add(new ListBoxItem() { Content = lines + " Level " + (ls.LevelNumber - ((int)LevelSpecification.SectionTypes.Custom) + 1) + " " + lines, IsEnabled = false, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 19 });
                        break;
                }
            }

            //unlocks levels already completed
            int j = 0;
            if (completedLevels == null && MainPage.MainMenuContainer[0].AllLevelsCompleted == false)
            {
                completedLevels = new int[MainPage.MainMenuContainer[0].CompletedLevels.Count];
                foreach (int i in MainPage.MainMenuContainer[0].CompletedLevels)
                {
                    completedLevels[j] = i;
                    j++;
                }
            }
            if (MainPage.MainMenuContainer[0].AllLevelsCompleted || MainPage.MainMenuContainer[0].UserIsAdmin)
            {
                MainPage.MainMenuContainer[0].AllLevelsCompleted = true;
                foreach (LevelSpecification ls in MainPage.AvailableLevels)
                    EnabledLevelInList(ls, ls.LevelNumber);
            }
            else
            {
                foreach (int i in completedLevels)
                {
                    MainPage.MainMenuContainer[0].CompletedLevels.Add(i);
                    EnabledLevelInList(GetLevelSpecFromLevelId(i, MainPage), i);
                }
            }

            //unlocks levels that haven't been completed but whose required levels have been completed
            int lvlId = -1;
            bool allLevelRequirementsMet = true;
            Dictionary<LevelSpecification.SectionTypes, int> farthestLevelDiscoveredInSection = new Dictionary<LevelSpecification.SectionTypes, int>();
            farthestLevelDiscoveredInSection.Add(LevelSpecification.SectionTypes.Custom, 0);
            farthestLevelDiscoveredInSection.Add(LevelSpecification.SectionTypes.Head, -1);
            farthestLevelDiscoveredInSection.Add(LevelSpecification.SectionTypes.Torso, -1);
            farthestLevelDiscoveredInSection.Add(LevelSpecification.SectionTypes.LeftArm, -1);
            farthestLevelDiscoveredInSection.Add(LevelSpecification.SectionTypes.RightArm, -1);
            if (completedLevels == null)
            {
                completedLevels = new int[1];
                completedLevels[0] = -1;
            }
            foreach (LevelSpecification ls in MainPage.AvailableLevels)
            {
                lvlId = ls.LevelNumber - (int)ls.SectionType;
                if (completedLevels.Contains((lvlId + (int)ls.SectionType)) == false)
                {
                    allLevelRequirementsMet = true;
                    foreach (int i in ls.RequiredLevelsToUnlock)
                    {
                        if ((completedLevels.Contains(i) == false))
                            allLevelRequirementsMet = false;
                    }
                    if (allLevelRequirementsMet)
                    {
                        EnabledLevelInList(ls, lvlId + (int)ls.SectionType);
                        if (lvlId > farthestLevelDiscoveredInSection[ls.SectionType])
                            farthestLevelDiscoveredInSection[ls.SectionType] = lvlId;
                    }
                }
                else
                {
                    if (lvlId > farthestLevelDiscoveredInSection[ls.SectionType])
                        farthestLevelDiscoveredInSection[ls.SectionType] = lvlId;
                }
            }

            if (completedLevels.Contains(MainPage.LvlIdForLastTutorialLevel))
                ChooseLevelUi.LevelsListTest.SelectedItem = EndlessLevelMediumListItem;
            else
                ChooseLevelUi.LevelsListTest.SelectedIndex = farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.Custom];
            
            if (farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.LeftArm] >= 0)
                ChooseLevelUi.LevelsListLeftArm.SelectedIndex = farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.LeftArm];

            if (farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.RightArm] >= 0)
                ChooseLevelUi.LevelsListRightArm.SelectedIndex = farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.RightArm];
            if (farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.Torso] >= 0)
                ChooseLevelUi.LevelsListTorso.SelectedIndex = farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.Torso];
            if (farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.Head] >= 0)
                ChooseLevelUi.LevelsListHead.SelectedIndex = farthestLevelDiscoveredInSection[LevelSpecification.SectionTypes.Head];
            
            ChooseLevelUi.LevelsListTest.Focus();

            ChooseLevelUi.Visibility = System.Windows.Visibility.Visible;

            Gear1Storyboard.Pause();
            Gear2Storyboard.Pause();

            ChooseLevelUi.EnableFixesBasedOnProgress(completedLevels);

            MainMenuUiIsOnTop = false;
        }

        private void Button_Click_OpenAdminClient(object sender, RoutedEventArgs e)
        {
            if (MainMenuUiIsOnTop == false)
                return;

            MainMenuUiIsOnTop = false;

            TeacherClientUi.mainMenu = this;
            TeacherClientUi.LoadClassDataIntoTeacherClient(TeacherClientHaventBeenOpenedBefore);
            TeacherClientUi.Visibility = System.Windows.Visibility.Visible;            
            TeacherClientUi.Focus();
        }

        private LevelSpecification GetLevelSpecFromLevelId(int id, MainPage mainPage)
        {
            foreach(LevelSpecification ls in mainPage.AvailableLevels)
            {
                if (ls.LevelNumber == id)
                    return ls;
            }
            return null;
        }

        private void EnabledLevelInList(LevelSpecification ls, int lvlId)
        {
            switch (ls.SectionType)
            {
                case LevelSpecification.SectionTypes.Head:
                    ((ListBoxItem)ChooseLevelUi.LevelsListHead.Items[lvlId - ((int)LevelSpecification.SectionTypes.Head)]).IsEnabled = true;
                    break;
                case LevelSpecification.SectionTypes.Torso:
                    ((ListBoxItem)ChooseLevelUi.LevelsListTorso.Items[lvlId - ((int)LevelSpecification.SectionTypes.Torso)]).IsEnabled = true;
                    break;
                case LevelSpecification.SectionTypes.LeftArm:
                    ((ListBoxItem)ChooseLevelUi.LevelsListLeftArm.Items[lvlId - ((int)LevelSpecification.SectionTypes.LeftArm)]).IsEnabled = true;
                    break;
                case LevelSpecification.SectionTypes.RightArm:
                    ((ListBoxItem)ChooseLevelUi.LevelsListRightArm.Items[lvlId - ((int)LevelSpecification.SectionTypes.RightArm)]).IsEnabled = true;
                    break;
                case LevelSpecification.SectionTypes.Custom:
                    if (ls.IsEndlessLevel)
                    {
                        switch (ls.EndlessLevelDifficulty)
                        {
                            case 0:
                                EndlessLevelEasyListItem.IsEnabled = true;
                                break;
                            case 1:
                                EndlessLevelMediumListItem.IsEnabled = true;
                                break;
                            case 2:
                            default:
                                EndlessLevelHardListItem.IsEnabled = true;
                                break;
                        }
                    }
                    else
                        ((ListBoxItem)ChooseLevelUi.LevelsListTest.Items[lvlId - ((int)LevelSpecification.SectionTypes.Custom)]).IsEnabled = true;
                    break;
            }
        }

        private void Content_Resized(object sender, EventArgs e)
        {
            MainPage MainPage = (MainPage)Application.Current.RootVisual;
            if (MainPage.SkipMainMenu)
                return;
            MainMenuModel owningModel;
            if (MainPage.UnloadedMainMenuContainer != null)
                owningModel = MainPage.UnloadedMainMenuContainer;
            else
                owningModel = MainPage.MainMenuContainer[0];

            owningModel.MenuHeight = App.Current.Host.Content.ActualHeight;
            owningModel.MenuWidth = App.Current.Host.Content.ActualWidth;
        }


    }
}
