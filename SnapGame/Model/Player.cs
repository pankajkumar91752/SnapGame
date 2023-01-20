using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;


namespace SnapGame.Model
{
    public class Player : INPCbase, INotifyPropertyChanged
    {
        private bool isActive;
        private string name;

        public string PlayerName
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        

      
        public ObservableCollection<Card> Cards { get; private set; } = new ObservableCollection<Card>();
        public bool IsActive
        {
            get => isActive; set { isActive = value; OnPropertyChanged(); }

        }
        private Card top;

        public Card Top
        {
            get { return top; }
            set { top = value; OnPropertyChanged(); }
        }


        public Card? Pop()
        {
            Top = Cards.LastOrDefault();
          
            if (top == null) return null;
            Cards.RemoveAt(Cards.Count-1);
            if (Cards.LastOrDefault() != null)
            Cards.LastOrDefault().IsOpen=true;
            return top;
        }
        public void Add(IEnumerable<Card> cards) => cards.ToList().ForEach(Add);
        public void Add(Card f)
        {
            if (Cards.LastOrDefault() != null)
                Cards.LastOrDefault().IsOpen = false;
            f.IsOpen = false; Cards.Add(f);
        }

        public override string ToString() => $"{PlayerName}";
        internal void Reset()
        {
            Cards.Clear();
            Top = null;
        }
    }

}
