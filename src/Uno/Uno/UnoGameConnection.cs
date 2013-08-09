//
// UnoGameConnection.cs
//
// Author:
//       Alexander Bothe <info@alexanderbothe.com>
//
// Copyright (c) 2013 Alexander Bothe
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uno.Game;
using Uno.Uno;

namespace Uno
{
	public enum UnoProperty
	{
		OwnHand,
		OtherPlayersHandSize,
		OtherGameStates,
		NextPlayer,
	}

	public class UnoGameConnection : GameConnection
	{
		public readonly Dictionary<string, int> OtherPlayersHandSize = new Dictionary<string, int>();
		public readonly List<Card> OwnHand = new List<Card>();
		/// <summary>
		/// Card candidates that can be used to put on the deck.
		/// </summary>
		public readonly List<Card> RecommendedCards = new List<Card>();

		public Card TopMostCard;
		public CardColor ColorSelection;
		public int CardsOnStack;
		public bool ClockwiseGameDirection;
		public string CurrentPlayer;

		public bool IsAtGameTurn
		{
			get{ return CurrentPlayer == PlayerNick; }
		}

		public GameField Field;

		#region Events
		public event Action<UnoProperty> PropertyChanged;

		void NotifyPropChanged(UnoProperty prop)
		{
			if (PropertyChanged != null) {
				Field.BeginInvoke (PropertyChanged, prop);
			}
		}
		#endregion

		#region Init/Exit handlers
		public override void Initialize(string nick)
		{
			base.Initialize(nick);
		}

		protected override void OnGameFinished(bool aborted)
		{
			base.OnGameFinished(aborted);
		}

		protected override void OnGameStarted()
		{
			base.OnGameStarted();
		}

		protected override void OnOtherPlayerLeft(string nick)
		{
			OtherPlayersHandSize.Remove(nick);
			PropertyChanged (UnoProperty.OtherPlayersHandSize);

			base.OnOtherPlayerLeft(nick);
		}
		#endregion

		#region Gameplay actions
		protected override void OnGeneralPlayerInfoReceived(string nick, bool isReady, BinaryReader r)
		{
			base.OnGeneralPlayerInfoReceived(nick, isReady, r);

			OtherPlayersHandSize[nick] = (int)r.ReadByte();
			NotifyPropChanged (UnoProperty.OtherPlayersHandSize);
		}

		protected override void OnPlayerInfoReceived(BinaryReader r)
		{
			OwnHand.Clear();
			RecommendedCards.Clear();

			var num = (int)r.ReadByte();
			while (num-- != 0)
				OwnHand.Add(Card.FromHash(r.ReadUInt16()));

			num = (int)r.ReadByte();
			while (num-- != 0)
				RecommendedCards.Add(Card.FromHash(r.ReadUInt16()));

			base.OnPlayerInfoReceived(r);

			NotifyPropChanged (UnoProperty.OwnHand);
		}

		protected override void OnGameDataReceived(BinaryReader r)
		{
			var message = (UnoMessage)r.ReadByte();

			switch (message) {
				case UnoMessage.ActionNotAllowed:
					MessageBox.Show (r.ReadString(),"Action not allowed!");
					break;
				case UnoMessage.GameStates:
					CardsOnStack = (int)r.ReadByte ();
					TopMostCard = Card.FromHash (r.ReadUInt16 ());
					ColorSelection = (CardColor)r.ReadByte ();
					ClockwiseGameDirection = r.ReadBoolean ();
					CurrentPlayer = r.ReadString ();
					NotifyPropChanged (UnoProperty.OtherGameStates);
					break;
				case UnoMessage.YouAreNext:
					NotifyPropChanged (UnoProperty.NextPlayer);
					break;
				case UnoMessage.GameFinished:
					MessageBox.Show (r.ReadString()+" has won the game!","Game finished");
					break;
			}
		}
		#endregion
	}
}

