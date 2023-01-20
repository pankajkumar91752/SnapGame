using System.ComponentModel;


namespace SnapGame.Model
{
    public class GameOptions : INPCbase, INotifyPropertyChanged
    {
        private int decks =1;

        public int Decks
        {
            get { return decks; }
            set { decks = value; OnPropertyChanged(); }
        }
        private bool faceMatch = true;

        public bool MatchFace
        {
            get { return faceMatch; }
            set { faceMatch = value; OnPropertyChanged(); }
        }
        private bool matchSuit;

        public bool MatchSuit
        {
            get { return matchSuit; }
            set { matchSuit = value; OnPropertyChanged(); }
        }

        private bool matchFaceSuit;

        public bool MatchFaceSuit
        {
            get { return matchFaceSuit; }
            set { matchFaceSuit = value; OnPropertyChanged(); }
        }




    }

}
