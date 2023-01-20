using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


namespace SnapGame.Model
{
    public class INPCbase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string member = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
    }
    public enum GameState
    {
        Stop = 1,
        Running = 2,
        End = 4,
    }
    public class GameContrSnapoller : INPCbase
    {
        private Player? winner;
        int p = 0;
        public Player[] Players { get; set; }
        public Player? Winner { get => winner; set { winner = value; OnPropertyChanged(); } }

        public ObservableCollection<Card> CardPile { get; set; } = new ObservableCollection<Card>();

        public Card? TopCard => CardPile.LastOrDefault();

        private string result;

        public string Result
        {
            get { return result; }
            set { result = value; OnPropertyChanged(); }
        }

        public GameContrSnapoller(GameOptions? options)
        {
            Options = options;
            Players = new[] {new Player { PlayerName ="P1"},
                new Player { PlayerName ="P2"},
            };

        }

        public GameState State { get; internal set; } = GameState.Stop;
        public GameOptions? Options { get; }

        internal void Start()
        {
            this.State = GameState.Running;
            UpdateStatus();
        }

        private void UpdateStatus() => Result = $"state {State} Active player:{p} {(winner != null ? "winner :" + winner?.PlayerName : null)} ";

        internal void Stop()
        {
            this.State = GameState.Stop;
            UpdateStatus();

        }

        internal void Deal()
        {
            Winner = default;
            this.State = GameState.Stop;
            CardPile.Clear();
            foreach (var item in Players)
            {
                item.Reset();
            }
            p = Random.Shared.Next(0, 1);

            for (int i = 0; i < Options.Decks; i++)
            {
                foreach (var c in CardDeck.ShuffledDeck())
                {
                    Players[p].Add(c);
                    NextPlayer();
                }

            }
            UpdateStatus();

        }

        private int NextPlayer()
        {
            Players[p].IsActive = false;
            p = p == 0 ? 1 : 0;
            Players[p].IsActive = true;
            return p;
        }

        internal void Move()
        {
            if (State != GameState.Running) return;

            var cp = Players[p];

            var c = cp.Pop();
            if (c == null)
            {
                End(Players[NextPlayer()]);
                UpdateStatus();

                return;
            };
            var l = CardPile.FirstOrDefault();
            c.IsOpen = true;
            CardPile.Insert(0, c);
            UpdateStatus();

            ExecSnap(c, l);




            NextPlayer();

        }

        private void ExecSnap(Card? c, Card? c1)
        {
            if (!CheckSnap(c, c1)) return;
            var p = Players[Random.Shared.Next(0, 1)];
            Result += $"Snapped {c} and {c1} to player{p} ";
            for (int i = CardPile.Count; i > 0; i--)
            {
                c = CardPile.Last();
                CardPile.RemoveAt(CardPile.Count - 1);
                p.Add(c);

            }

        }

        private bool CheckSnap(Card? c, Card? c1)
        {
            if (Options.MatchFaceSuit)
                return c?.Face == c1?.Face || c?.Suit == c1?.Suit;
            if (Options.MatchFace)
                return c?.Face == c1?.Face;
            if (Options.MatchSuit)
                return c?.Suit == c1?.Suit;
            return false;
        }

        internal void End(Player winner = null)
        {
            State = GameState.End;
            Winner = winner ?? Players.FirstOrDefault(f => f.Cards.Count > 0);
        }
    }
    public enum Suit

    {
        Club,
        Heart,
        Spade,
        Diamond

    }

}
