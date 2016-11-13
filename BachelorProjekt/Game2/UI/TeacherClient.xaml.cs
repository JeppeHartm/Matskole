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

namespace Game2.UI
{
    public partial class TeacherClient : UserControl
    {
        public MainMenu mainMenu;
        
        private List<String> classNames = new List<string>();
        public TeacherClient()
        {
            InitializeComponent();
        }

        private async void ClassList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StudentList.Items.Clear();
            ListBoxItem lbi;
            
            classNames.Clear();
            try
            {
                classNames = await DatabaseFunctions.getUsernamesByClassID(((String)((ListBoxItem)ClassList.SelectedItem).Tag));
            }
            catch (System.Security.SecurityException se)
            {
                MessageBox.Show("databasen virker ikke fra din egen computer");
                return;
            }

            foreach (String user in classNames)
            {
                lbi = new ListBoxItem();
                lbi.Tag = user;
                lbi.Content = user;
                lbi.FontSize = 16;
                lbi.FontWeight = FontWeights.Bold;
                StudentList.Items.Add(lbi);
            }


            if (StudentList.Items.Count > 0)
                StudentList.SelectedIndex = 0;
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            mainMenu.MainMenuUiIsOnTop = true;
            mainMenu.Focus();
        }

        private void Button_Click_Open(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void TeacherClient_Loaded(object sender, RoutedEventArgs e)
        {
            return;
        }

        public async void LoadClassDataIntoTeacherClient(bool FirstTime)
        {
            if (FirstTime)
                mainMenu.TeacherClientHaventBeenOpenedBefore = false;
            else
                return;



            string[] classids;

            try
            {
                classids = await DatabaseFunctions.getDistinctClassIDs();
            }
            catch (System.Security.SecurityException se)
            {
                MessageBox.Show("databasen virker ikke fra din egen computer");
                return;
            }

            if (classids == null)
            {
                MessageBox.Show("databasen virker ikke fra din egen computer");
                return;
            }

            ListBoxItem lbi;

            foreach (string s in classids)
            {
                lbi = new ListBoxItem();
                lbi.Tag = s;
                lbi.Content = s;
                lbi.FontSize = 16;
                lbi.FontWeight = FontWeights.Bold;
                ClassList.Items.Add(lbi);
            }
        }

        private async void LevelSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentList.SelectedItem == null)
                return;
            if (StudentList.Items.Count == 0)
                return;
            if (StudentList.SelectedIndex < 0)
                return;

            int i = 0, highestTimeSpent = -1, highestIndex = 0;
            List<Point> datapoints = new List<Point>();
            
            foreach (String s in classNames)
            {
                if (s == ((String)((ListBoxItem)StudentList.SelectedItem).Tag))
                {
                    DatabaseFunctions.UserData user = await DatabaseFunctions.getUserData(s, true, true);
                    foreach (DatabaseFunctions.CompletedLevelData lvl in user.completedLevels)
                    {
                        if (LevelSelection.SelectedIndex == 0 && lvl.levelid >= 200 && lvl.levelid < 300)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 1 && lvl.levelid >= 300 && lvl.levelid < 400)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 2 && lvl.levelid >= 200 && lvl.levelid < 400)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 3 && lvl.levelid >= 400 && lvl.levelid < 500)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 4 && lvl.levelid >= 100 && lvl.levelid < 200)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 5 && lvl.levelid == -1)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 6 && lvl.levelid == -2)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 7 && lvl.levelid == -3)
                        {
                            datapoints.Add(new Point(i, lvl.timespent));
                            i++;
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                        }
                        else if (LevelSelection.SelectedIndex == 8)
                        {
                            datapoints.Add(new Point(lvl.index, lvl.timespent));
                            if (lvl.timespent > highestTimeSpent)
                                highestTimeSpent = lvl.timespent;
                            if (lvl.index > highestIndex)
                                highestIndex = lvl.index;
                        }

                    }
                }
            }
            
            if (i > 0)
                highestIndex = i;

            
            if (highestTimeSpent == -1)
                return;

            

            List<Point> normalizedDataPoints = new List<Point>();
            foreach (Point p in datapoints)
                normalizedDataPoints.Add(CreateNormalizedDataPoint(p.X, p.Y, highestIndex, highestTimeSpent));
            
            CreateChart(normalizedDataPoints, highestIndex, highestTimeSpent);
            
        }

        private Point CreateNormalizedDataPoint(double X, double Y, int Xmax, int Ymax)
        {
            Point result;
            double Xfactor = 1300, Yfactor = 730;

                result = new Point(X * ( Xfactor / Xmax), Yfactor - ( Y * ((Yfactor / Ymax))));


            return result;
        }

        private void CreateChart(List<Point> points, int Xmax, int Ymax)
        {
            double buff1, buff2;
            PointCollection plot = new PointCollection();
            
            for (int i = 0; i < points.Count; i++ )
            {
                plot.Add(points[i]);
            }
            Plot.Points.Clear();
            Plot.Points = plot;

            X100.Content = "" + (Xmax);
            X80.Content = "" + (Xmax * 0.8);
            X60.Content = "" + (Xmax * 0.6);
            X40.Content = "" + (Xmax * 0.4);
            X20.Content = "" + (Xmax * 0.2);

            Y100.Content = "" + (Ymax);
            Y80.Content = "" + (Ymax * 0.8);
            Y60.Content = "" + (Ymax * 0.6);
            Y40.Content = "" + (Ymax * 0.4);
            Y20.Content = "" + (Ymax * 0.2);            

        }
    }
}
