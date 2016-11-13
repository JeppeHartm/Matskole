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

namespace Game2.UI
{
    public class ScoreScreenModel : INotifyPropertyChanged
    {
        const int IMGWIDTH = 600;
        const int IMGHEIGHT = 663;

        private Uri _imgsourcepath;
        public Uri ImgSourcePath { get { return _imgsourcepath; } set { _imgsourcepath = value; OnPropertyChanged("ImgSourcePath"); } }
        Uri ImgUriBronze = new Uri("/Game2;Component/Images/ScoreScreen/ScoreScreen_b.png", UriKind.Relative);
        Uri ImgUriSilver = new Uri("/Game2;Component/Images/ScoreScreen/ScoreScreen_s.png", UriKind.Relative);
        Uri ImgUriGold = new Uri("/Game2;Component/Images/ScoreScreen/ScoreScreen_g.png", UriKind.Relative);
        
        private int _points;
        private int _time;
        private int _amount;
        private int _x;
        private int _y;
        private string _message;
        public int Points { get { return _points; } set { _points = value; OnPropertyChanged("Points"); } }
        public int Time { get { return _time; } set { _time = value; OnPropertyChanged("Time"); } }
        public int Amount { get { return _amount; } set { _amount = value; OnPropertyChanged("Amount"); } }
        public int X { get { return _x; } set { _x = value; OnPropertyChanged("X"); } }
        public int Y { get { return _y; } set { _y = value; OnPropertyChanged("Y"); } }
        public string Message { get { return _message; } set { _message = value; OnPropertyChanged("Message"); } }

        public MainPage parent;
        private List<int> completedLevels;
        public ScoreScreenModel(int points, int goldpoints, int time, int amount, MainPage parent, int levelnumber, List<int> completedLevels)
        {
            Points = points;
            Time = time;
            Amount = amount;
            ImgSourcePath = GetBGImgSource(points, goldpoints);
            this.parent = parent;
            this.completedLevels = completedLevels;
            X -= IMGWIDTH/2;
            Y -= IMGHEIGHT/2;
            Message = GetMessage(levelnumber);
            
        }
        private string GetMessage(int levelnumber)
        {
            string output = "Det klarede du flot!";

            if (levelnumber == LevelCollection.GetListFromSection((int)Game2.LevelSpecification.SectionTypes.Custom).Count - 1 && !hasCompleted(LevelCollection.GetListFromSection((int)Game2.LevelSpecification.SectionTypes.Custom).Count - 1))
            {
                output = "Tillykke! Du har nu klaret basic træning og er klar til at gå igang med højre og venstre arm! Du kan også træne videre ved at vælge Ny bane nem/medium/svær afhængig af hvor god du er.";
            }
            else if (levelnumber == (int)Game2.LevelSpecification.SectionTypes.LeftArm + 3 && 
                    hasCompleted((int)Game2.LevelSpecification.SectionTypes.RightArm) && 
                    !hasCompleted((int)Game2.LevelSpecification.SectionTypes.LeftArm) || 
                    levelnumber == (int)Game2.LevelSpecification.SectionTypes.RightArm + 3 && 
                    !hasCompleted((int)Game2.LevelSpecification.SectionTypes.RightArm) && 
                    hasCompleted((int)Game2.LevelSpecification.SectionTypes.LeftArm))
            {
                output = "Tillykke! Ved at komme godt igang med reperation af begge arme er du nu klar til at arbejde på kroppen, men pas på! Det bliver endnu sværere!";
            }
            else if (levelnumber == ((int)Game2.LevelSpecification.SectionTypes.Torso) + 2 && !hasCompleted(((int)Game2.LevelSpecification.SectionTypes.Torso) + 2))
            {
                output = "Tillykke! Du kan nu prøve at arbejde med hovedet, men det er meget svært!";
            }
            else if (allComplete())
            {
                output = "Tillykke! Du har klaret alle banerne.";
            }
            return output;
        }

        private bool hasCompleted(int p)
        {
            bool output = false;
            foreach (int i in completedLevels)
            {
                if (output = i == p) break;
            }
            return output;
        }
        private bool allComplete()
        {
            foreach (LevelSpecification l in parent.AvailableLevels)
            {
                if (!hasCompleted(l.LevelNumber)) return false;
            }
            return true;
        }
        private Uri GetBGImgSource(int points, int goldpoints)
        {
            Uri img = null;
            if (points >= goldpoints)
            {
                img = ImgUriGold;
            }
            else if (points >= goldpoints / 2)
            {
                img = ImgUriSilver;
            }
            else
            {
                img = ImgUriBronze;
            }
            return img;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
