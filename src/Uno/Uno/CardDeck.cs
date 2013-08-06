using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Game
{
	public class CardDeck
	{
		readonly List<Card> CardList = new List<Card> ();

		public CardDeck ()
		{
			Reset ();
		}

		public void Reset()
		{
			CardList.Clear ();

			CardList.Add (new Card (CardColor.Red, CardCaption.Zero));
			CardList.Add (new Card (CardColor.Blue, CardCaption.Zero));
			CardList.Add (new Card (CardColor.Green, CardCaption.Zero));
			CardList.Add (new Card (CardColor.Yellow, CardCaption.Zero));

			for (int i = 0; i < 4; i++) {
				CardList.Add (new Card (CardColor.Black, CardCaption.Take4));
				CardList.Add (new Card (CardColor.Black, CardCaption.WishColor));
			}

			for (int i = 0; i < 2; i++) {

				CardList.Add (new Card (CardColor.Red, CardCaption.One));
				CardList.Add (new Card (CardColor.Blue, CardCaption.One));
				CardList.Add (new Card (CardColor.Green, CardCaption.One));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.One));

				CardList.Add (new Card (CardColor.Red, CardCaption.Two));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Two));
				CardList.Add (new Card (CardColor.Green, CardCaption.Two));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Two));

				CardList.Add (new Card (CardColor.Red, CardCaption.Three));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Three));
				CardList.Add (new Card (CardColor.Green, CardCaption.Three));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Three));

				CardList.Add (new Card (CardColor.Red, CardCaption.Four));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Four));
				CardList.Add (new Card (CardColor.Green, CardCaption.Four));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Four));

				CardList.Add (new Card (CardColor.Red, CardCaption.Five));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Five));
				CardList.Add (new Card (CardColor.Green, CardCaption.Five));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Five));


				CardList.Add (new Card (CardColor.Red, CardCaption.Six));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Six));
				CardList.Add (new Card (CardColor.Green, CardCaption.Six));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Six));

				CardList.Add (new Card (CardColor.Red, CardCaption.Seven));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Seven));
				CardList.Add (new Card (CardColor.Green, CardCaption.Seven));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Seven));

				CardList.Add (new Card (CardColor.Red, CardCaption.Eight));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Eight));
				CardList.Add (new Card (CardColor.Green, CardCaption.Eight));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Eight));

				CardList.Add (new Card (CardColor.Red, CardCaption.Nine));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Nine));
				CardList.Add (new Card (CardColor.Green, CardCaption.Nine));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Nine));

				CardList.Add (new Card (CardColor.Red, CardCaption.Take2));
				CardList.Add (new Card (CardColor.Blue, CardCaption.Take2));
				CardList.Add (new Card (CardColor.Green, CardCaption.Take2));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.Take2));

				CardList.Add (new Card (CardColor.Red, CardCaption.RevertDirectionAndNewColor));
				CardList.Add (new Card (CardColor.Blue, CardCaption.RevertDirectionAndNewColor));
				CardList.Add (new Card (CardColor.Green, CardCaption.RevertDirectionAndNewColor));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.RevertDirectionAndNewColor));

				CardList.Add (new Card (CardColor.Red, CardCaption.SkipNextPlayer));
				CardList.Add (new Card (CardColor.Blue, CardCaption.SkipNextPlayer));
				CardList.Add (new Card (CardColor.Green, CardCaption.SkipNextPlayer));
				CardList.Add (new Card (CardColor.Yellow, CardCaption.SkipNextPlayer));
			}
		}

		public static void Shuffle<Card> (IList<Card> list)
		{
			Random rng = new Random ();
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = rng.Next (n + 1);
				Card value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}

		public const int InitialCardsPerPlayer = 7;

		public List<Card> GiveFirstHand ()
		{
			var hand = new List<Card> ();
			var rng = new Random ();
			for (int i = 0; i < InitialCardsPerPlayer; i++) {
				int n = rng.Next (CardList.Count);
				hand.Add (CardList [n]);
				CardList.RemoveAt (n);
			}

			return hand;
		}

        public Card GiveCard() {
            int n = (CardList.Count)-1;
            Card card = CardList[n];
            CardList.RemoveAt(n);
            return card;
        }

      

		public bool Put(Card c)
		{
			if (CardList.Contains (c))
				return false;

			CardList.Add (c);
			return true;
		}

		public void Put(IEnumerable<Card> subDeck)
		{
			foreach (var c in subDeck)
				Put (c);
		}
	}
}
