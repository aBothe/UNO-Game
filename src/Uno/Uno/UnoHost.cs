//
// UnoHost.cs
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
using Uno.Game;

namespace Uno
{
	public class UnoHost : GameHost
	{
		#region Properties
		public readonly CardDeck AvailableCards = new CardDeck ();
		public long CurrentPlayer;
		public readonly Stack<Card> CardStack = new Stack<Card>();
		public bool ClockwiseDirection;
		public CardColor CurrentColor;
		public int CardsToDraw;
		public int PlayerRoundsToSkip;

		#region LowLevel
		public override string GameTitle {
			get {
				return "Uno";
			}
		}

		public const byte MinUnoPlayers = 2;
		public const byte MaxUnoPlayers = 10;

		byte maxPlayers = MaxUnoPlayers;
		public override byte MaxPlayers
		{
			get{return maxPlayers;}
			set{
				if (State == GameState.Playing)
					return;
				if (value < MinPlayers)
					throw new InvalidOperationException ("MayPlayer value cannot be smaller than MinPlayer count");
				maxPlayers = Math.Max (MaxUnoPlayers, value);
			}
		}

		byte minPlayers = MinUnoPlayers;
		public override byte MinPlayers {
			get {
				return minPlayers;
			}
			set {
				if (value > MaxPlayers)
					throw new InvalidOperationException ("MinPlayer value cannot be larger than MaxPlayer count");
				minPlayers = Math.Max (value, MinUnoPlayers);
			}
		}
		#endregion
		#endregion

		#region Card logic

		public bool TryPutOnStack(Card c)
		{
			// Ist Karte kompatibel zu zuletzt auf den Stack gelegter Karte?

			// Karte darauflegen

			// Strafwerte/Zustände anpassen, nächsten Spieler bestimmen, Gewinn feststellen, Gewinner/Verlierer benachrichtigen

			return true;
		}

		#endregion


		protected override Player CreatePlayer (string nick)
		{
			return new UnoPlayer (this, nick); 
		}

		protected override void OnPlayerDisconnected(Player p, ClientMessage reason)
		{
			(p as UnoPlayer).ReleaseHand();

			base.OnPlayerDisconnected(p, reason);
		}

		protected override bool StartGameInternal ()
		{
			AvailableCards.Reset();
			foreach (UnoPlayer p in Players)
			{
				p.ResetCardDeck();
			}

			return true;
		}

		protected override void OnComposePlayerInfo(Player p, BinaryWriter w)
		{
			var unop = p as UnoPlayer;

			w.Write((byte)unop.CardCount);

			foreach (var c in unop.Cards)
				w.Write(c.ToHash());
			
			base.OnComposePlayerInfo(p, w);
		}

		protected override void OnComposeGeneralPlayerInfo(Player p, BinaryWriter w)
		{
			base.OnComposeGeneralPlayerInfo(p, w);

			var unop = p as UnoPlayer;

			w.Write((byte)unop.CardCount);
		}
	}
}

