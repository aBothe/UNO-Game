//
// Host.cs
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
using Uno.GameList;
using System.Collections.Generic;
using Uno.Game;

namespace Uno.GameHost
{
	public class GameStateChangedArgs : EventArgs {
		public readonly Host Host;
		public readonly GameState OldState;
		public readonly GameState NewState;

		public GameStateChangedArgs(Host h, GameState old, GameState n)
		{
			Host = h;
			OldState = old;
			NewState = n;
		}
	}

	public enum GameState : byte
	{
		WaitingForPlayers,
		Playing,
		GameFinished,
	}

	/// <summary>
	/// Singleton. Each program instance may host one game only.
	/// </summary>
	public class Host : IDisposable
	{
		#region Properties
		public static Host Instance { get; private set; }
		public static bool IsHosting {get{ return Instance != null; }}
		public const byte MinUnoPlayers = 2;
		public const byte MaxUnoPlayers = 10;

		byte maxPlayers = MaxUnoPlayers;
		public byte MaxPlayers
		{
			get{return maxPlayers;}
			set{
				if (State == GameState.Playing)
					return;
				maxPlayers = Math.Max (MaxUnoPlayers, value);
			}
		}

		List<Player> Players =new List<Player>();
		public int PlayerCount
		{
			get{ 
				return Players.Count;
			}
		}


		GameState state = GameState.WaitingForPlayers;
		public GameState State { get{return state;}
			private set{
				if(GameStateChanged!=null)
					GameStateChanged(this, new GameStateChangedArgs(this, state, value));
				state = value;
			}
		}

		public event EventHandler<GameStateChangedArgs> GameStateChanged;

		#endregion

		#region Init / Constructor
		private Host() {

		}

		public Host CreateHost()
		{
			var host = new Host{

			};
			// Initialisiert Spielhost, setzt den Host in den Lobbymodus, und wartet, 
			// bis 2-10 Spieler verbunden & bereit sind + der Spielersteller das Spiel startet

			return Instance = host;
		}

		public void Dispose ()
		{

		}
		#endregion

		public void Shutdown()
		{

		}
	}
}

