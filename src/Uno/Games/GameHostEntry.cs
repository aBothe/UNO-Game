using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Uno.Game;

namespace Uno.Games
{
	public class GameHostEntry
	{
		public string GameTitle;
		public int PlayerCount;
		public int MaxPlayers;
		public GameState State;
		public IPEndPoint Address;

		public override bool Equals(object obj)
		{
			var o = obj as GameHostEntry;
			return o != null && o.Address.Equals(Address);
		}

		public override int GetHashCode ()
		{
			return Address.GetHashCode ();
		}

		public override string ToString()
		{
			string msg;
			switch (State)
			{
				case GameState.WaitingForPlayers:
				case GameState.GameFinished:
					msg = "Open";
					break;
				default:
					msg = "Closed";
					break;
			}
			return string.Format("{4},IP {0} ({1}/{2} Players, {3})", Address.Address, PlayerCount, MaxPlayers, msg, GameTitle);
		}
	}
}
