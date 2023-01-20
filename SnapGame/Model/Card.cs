using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SnapGame.Model
{
    public class Card : INPCbase, INotifyPropertyChanged
    {

        public Card(char face, Suit suit)
        {
            Face = face;
            Suit = suit;
        }
        private bool isOpen;
        public bool IsOpen
        {
            get => isOpen; set { isOpen = value; OnPropertyChanged(); }

        }
        public char Face { get; private set; }
        public Suit Suit { get; private set; }

        public override string ToString() => $" {Suit} {Face}";
    }
    
    public class CardDeck
    {
        public const string Cardfaces = "2,3,4,5,6,7,8,9,10,J,Q,K,A";
        public static int CountInDeck { get; } = CardDeck.Cardfaces.Length/2 * Suit.GetValues<Suit>().Length;

        internal static Card[] BuildDeck()
        {
            var cards = new Card[CountInDeck];
            int i= 0;
            foreach (var f in Cardfaces.Split(','))
            {
                foreach (var s in Enum.GetValues<Suit>())
                {
                    cards[i++] = new Card(f[0], s);
                }

            }
            return cards;
        }
        internal static Card[] Shuffle(Card[] cards) => cards.OrderBy(f => Random.Shared.Next(cards.Length)).ToArray();
        internal static Card[] ShuffledDeck() => Shuffle(BuildDeck());

    }

}
