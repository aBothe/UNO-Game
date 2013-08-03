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
using System.Collections.Generic;
using System.IO;

namespace Uno.Game
{
	public class GameStateChangedArgs : EventArgs {
		public readonly GameHost Host;
		public readonly GameState OldState;
		public readonly GameState NewState;

		public GameStateChangedArgs(GameHost h, GameState old, GameState n)
		{
			Host = h;
			OldState = old;
			NewState = n;
		}
	}

	/// <summary>
	/// Singleton. Each program instance may host one game only.
	/// </summary>
	public abstract class GameHost : HostBackend
	{
		#region Properties
		public static GameHost Instance { get; private set; }
		public static bool IsHosting {get{ return Instance != null; }}

		public abstract string GameTitle{ get; }
		public readonly long Id = DateTime.UtcNow.ToBinary();

		public virtual byte MinPlayers {get{return 2;} set {}}
		public virtual byte MaxPlayers {get { return byte.MaxValue; } set {} }

		List<Player> Players =new List<Player>();
		public int PlayerCount
		{
			get{
				return Players.Count;
			}
		}

		GameState state;
		public GameState State { get{return state;}
			private set{
				var args = new GameStateChangedArgs (this, state, value);
				state = value;

				if(GameStateChanged!=null)
					GameStateChanged(this, args);
				if (AnyGameStateChanged != null)
					AnyGameStateChanged (this, args);
			}
		}

		public static EventHandler<GameStateChangedArgs> AnyGameStateChanged;
		public event EventHandler<GameStateChangedArgs> GameStateChanged;

		public virtual bool ReadyToPlay{
			get{
				if (PlayerCount < MinPlayers || PlayerCount > MaxPlayers)
					return false;

				foreach (var p in Players)
					if (!p.ReadyToPlay)
						return false;

				return true;
			}
		}
		#endregion

		#region Init / Constructor
		public GameHost() {

		}

		public static GameHost HostGame(GameHostFactory fac)
		{
			if (IsHosting)
				throw new InvalidOperationException ("Shut down other host first!");

			var host = fac.Create ();

			// Initialisiert Spielhost, setzt den Host in den Lobbymodus, und wartet, 
			// bis 2-10 Spieler verbunden & bereit sind + der Spielersteller das Spiel startet

			/*
			 * TCP-Listener binden, starten und auf Client-Connects warten.
			 * Danach Daten/Statusupdates über alle Verbindungen schicken.
			 * Clientseitige Updates erwarten (Aktionen, etwa Drücken des 'Bereit'-Buttons)
			 */

			Instance = host;

			host.State = GameState.Starting;
			host.State = GameState.WaitingForPlayers;

			return host;
		}
		#endregion

		#region Exit / Destructor
		public override void Dispose ()
		{
			State = GameState.ShuttingDown;
		}

		public static bool ShutDown()
		{
			if (Instance == null)
				return false;

			Instance.Shutdown ();
			Instance = null;
			return true;
		}

		public virtual void Shutdown()
		{
			Dispose ();
		}
		#endregion

		#region Messaging
		protected override void DataReceived (byte[] data)
		{
			var ms = new MemoryStream (data);
			var r = new BinaryReader (ms);

			var message = r.ReadByte ();

			// Ensure that the correct host was reached.
			if (r.ReadInt64 () != Id)
				return;

			switch (message) {

			}
		}
		#endregion

		#region Player

		#endregion


	}
}

