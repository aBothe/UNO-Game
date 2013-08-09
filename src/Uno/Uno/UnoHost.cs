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
		public long NextPlayerId;
		public long PreviousPlayerId;

		public UnoPlayer NextPlayer { 
			get { return GetPlayer (NextPlayerId) as UnoPlayer; }
			set {
				PreviousPlayerId = value;
				NextPlayerId = value != null ? value.Id : 0;
			}
		}

		public readonly Stack<Card> CardStack = new Stack<Card> ();
		public bool ClockwiseDirection;
		public CardColor CurrentColor;

		#region LowLevel

		public override string GameTitle {
			get {
				return "Uno";
			}
		}

		public const byte MinUnoPlayers = 2;
		public const byte MaxUnoPlayers = 10;
		byte maxPlayers = MaxUnoPlayers;

		public override byte MaxPlayers {
			get{ return maxPlayers;}
			set {
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

		void StepToNextPlayer()
		{
			var i = GetPlayerIndex (NextPlayer);
			if (ClockwiseDirection) {
				if (i == -1 || i+1 >= PlayerCount){
					NextPlayer = GetPlayerByIndex (0);
					return;
				}

				NextPlayer = GetPlayerByIndex (i + 1);

			} else {
				if (i == -1 || i-1 < 0){
					NextPlayer = GetPlayerByIndex (PlayerCount-1);
					return;
				}

				NextPlayer = GetPlayerByIndex (i - 1);
			}
		}

		void InformNewPlayer()
		{
			SendGameDataToPlayer (NextPlayer, new[] { (byte)UnoMessage.YouAreNext });
		}

		public bool IsCardCompatibleToStack (Card c)
		{
			if (c.Color == CardColor.Black || CardStack.Count == 0)
				return true;

			if (CurrentColor == c.Color || CardStack.Peek ().Caption == c.Caption) // Gilt das letztere nur für Nummern oder auch für Event-Karten?
				return true;

			return false;
		}

		public bool IsHandCompatibleToStack (UnoPlayer p)
		{
			foreach (var card in p.Cards)
				if (IsCardCompatibleToStack (card))
					return true;
			return false;
		}

		/// <summary>
		/// Used only when starting a game.
		/// </summary>
		bool autoStepToNextPlayer = true;
		public bool TryPutOnStack (UnoPlayer p, Card c, CardColor colorSelection, out string errorMsg)
		{
			errorMsg = null;

			if (p != NextPlayer) {
				errorMsg = "Wait for your round!";
				return false;
			}

			// Ist Karte kompatibel zu zuletzt auf den Stack gelegter Karte?
			// Zusatz: Wenn der Spieler nur noch eine (unpassende) Farbwahlkarte hat, so kann und muss er diese ablegen!
			if (!IsCardCompatibleToStack (c) && (p.CardCount > 1 || )) {
				errorMsg = "Card not allowed to be put on the stack!";
				// Eigentlich müsste nun neue Karte gezogen werden - jedoch explizit durch den Spieler
				return false;
			}

			// Karte aus der Hand des Spielers entfernen und
			p.RemoveCard (c);
			// auf den Stapel gelegter Karten legen.
			CardStack.Push (c);

			// Neue Farbe zuweisen. Ist niemals Black.
			CurrentColor = colorSelection;

			if (p.CardCount == 0) {
				if(p.PressedUnoButton)
				{
					// Spieler hat gewonnen !
					NoticeGameFinished(false);
				}
				else
				{
					// Uno: Vergisst ein Spieler seine vorletzte Karte mit Uno anzukündigen, und der nächste Spieler hat bereits seine Karte ausgespielt, so hat ersterer eine Karte zu ziehen.
					DrawCard(p);
				}
			}

			// Aktionskarten
			switch(c.Caption)
			{
				case CardCaption.Take2:
					StepToNextPlayer();
					var next = NextPlayer;
					next.PutCard(AvailableCards.GiveCard());
					next.PutCard(AvailableCards.GiveCard());
					break;
				case CardCaption.RevertDirectionAndNewColor:
					ClockwiseDirection = !ClockwiseDirection;
					break;
				case CardCaption.SkipNextPlayer:
					StepToNextPlayer();
					break;
				case CardCaption.WishColor:
					// Neue Farbe schon geschrieben
					break;
				case CardCaption.Take4:
					StepToNextPlayer();
					next = NextPlayer;
					for(int i = 4; i > 0; i--)
						next.PutCard(AvailableCards.GiveCard());
					break;
				default:
					// Normale Karten (Nummern) wurden bereits abgearbeitet.
					break;
			}

			// nächsten Spieler bestimmen
			if(autoStepToNextPlayer)
				StepToNextPlayer();
			InformNewPlayer();

			     
			// Spieler über neue Kartenkonstellation informieren

			// Strafwerte/Zustände anpassen, , Gewinn feststellen, Gewinner/Verlierer benachrichtigen
          
		
		}

		/// <summary>
		/// Wenn der Spieler keinen anderen Zug machen kann/will(!!), werden dem Spieler hier Strafkarten angerechnet.
		/// </summary>
		public bool DrawCard (UnoPlayer p)
		{
			return p.PutCard (AvailableCards.GiveCard ());
		}

		public bool PressUnoButton (UnoPlayer p)
		{
			// Prüfen, ob Uno-Button gedrückt werden kann
			if (p.CardCount <= 1) {
				p.PressedUnoButton = true;
				return true;
			}
			return false;
		}

		public bool SkipRound (UnoPlayer p)
		{
			return true;
		}

		#endregion

		protected override Player CreatePlayer (string nick)
		{
			return new UnoPlayer (this, nick); 
		}

		protected override void OnPlayerDisconnected (Player p, ClientMessage reason)
		{
			(p as UnoPlayer).ReleaseHand ();

			// Wenn p als nächster Spieler angedacht war, neuen Nachfolger bestimmen!
			if (NextPlayer == p){
				StepToNextPlayer ();
				InformNewPlayer ();
			}

			base.OnPlayerDisconnected (p, reason);
		}

		protected override bool StartGameInternal ()
		{
			ClockwiseDirection = true;
			NextPlayerId = 0;

			AvailableCards.Reset ();
			foreach (UnoPlayer p in Players) {
				p.ResetCardDeck ();
			}
			CardStack.Clear ();

			// Ersten Spieler bestimmen
			StepToNextPlayer ();

			// Erste Karte aufdecken und ausdecken
			var firstCard = AvailableCards.GiveCard ();
			CurrentColor = firstCard.Color;

			if (CurrentColor == CardColor.Black)
				CurrentColor = CardColor.Yellow;

			// Dem ersten Spieler temporär eine zusätzliche Karte geben und diese auf den Stapel legen
			// Dem nächsten Player signalisieren, dass er/sie dran ist
			NextPlayer.PutCard (firstCard);
			string errMsg;
			autoStepToNextPlayer = false;
			if (!TryPutOnStack (NextPlayer, firstCard, CurrentColor, out errMsg))
				return false;
			autoStepToNextPlayer = true;



			return true;
		}

		protected override void OnComposePlayerInfo (Player p, BinaryWriter w)
		{
			var unop = p as UnoPlayer;

			w.Write ((byte)unop.CardCount);

			foreach (var c in unop.Cards)
				w.Write (c.ToHash ());
			
			base.OnComposePlayerInfo (p, w);
		}

		protected override void OnComposeGeneralPlayerInfo (Player p, BinaryWriter w)
		{
			base.OnComposeGeneralPlayerInfo (p, w);

			var unop = p as UnoPlayer;

			w.Write ((byte)unop.CardCount);
		}

		protected override void OnGameDataReceived (Player playerOpt, BinaryReader r, BinaryWriter w)
		{
			string errorMsg = null;

			if (playerOpt == null)
				errorMsg = "Invalid player!";
			else {
				var message = (UnoMessage)r.ReadByte ();

				switch (message) {
					case UnoMessage.DrawCardFromStack:
						if (!DrawCard (playerOpt as UnoPlayer))
							errorMsg = "Can't draw card!";
						break;
					case UnoMessage.PressUno:
						if (!PressUnoButton (playerOpt as UnoPlayer))
							errorMsg = "Can't press UNO button";
						break;
					case UnoMessage.PutCard:

						var c = Card.FromHash (r.ReadUInt16 ());
						CardColor col;

						if (c.Color != CardColor.Black)
							col = c.Color;
						else
							col = (CardColor)r.ReadByte ();

						TryPutOnStack (playerOpt as UnoPlayer, c, col, out errorMsg);
						break;
					case UnoMessage.SkipRound:
						if (!SkipRound (playerOpt as UnoPlayer))
							errorMsg = "Can't skip round!";
						break;
				}
			}

			if (errorMsg != null) {
				w.Write ((byte)UnoMessage.ActionNotAllowed);
				w.Write (errorMsg);
			}
		}
	}
}

