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
using System.Net;

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
		public readonly long Id = IdGenerator.GenerateId();

		public virtual byte MinPlayers {get{return 2;} set {}}
		public virtual byte MaxPlayers {get { return byte.MaxValue; } set {} }

		List<Player> players =new List<Player>();
		public int PlayerCount
		{
			get{
				return players.Count;
			}
		}

		public IEnumerable<Player> Players
		{
			get{return players;}
		}

		public Player GetPlayer(long id)
		{
			foreach (var p in players)
				if (p.Id == id)
					return p;
			return null;
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

				foreach (var p in players)
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
		protected override void DataReceived (IPEndPoint ep,byte[] data)
		{
			var ms = new MemoryStream (data);
			var r = new BinaryReader (ms);

			// Ensure that the correct host was reached.
			if (r.ReadInt64 () != Id)
				return;

			var message = (HostMessage)r.ReadByte ();

			var playerId = r.ReadInt64 ();

			MemoryStream answer;
			BinaryWriter w;
			Player player;

			answer = new MemoryStream ();
			w = new BinaryWriter (answer);

			switch (message) {
				case HostMessage.Connect:
					var requestedNick = r.ReadString ();

					w.Write (playerId); // Is ConnectId atm

					if (State != GameState.WaitingForPlayers) {
						w.Write ((byte)ClientMessage.JoinDenied);
						w.Write ("Server is not awaiting new players!");
					} else if (players.Count < MaxPlayers) {
						// Create new player
						player = CreatePlayer (GetValidPlayerNick (requestedNick));

						player.Address = ep;
						w.Write ((byte)ClientMessage.JoinGranted);
						w.Write (player.Id);
						w.Write (player.Nick);

						players.Add (player);
						OnPlayerAdded (player);
					} else {
						w.Write ((byte)ClientMessage.JoinDenied);
						w.Write ("Player limit reached!");
					}
					break;

				case HostMessage.Disconnect:
					DisconnectPlayer (w,playerId, ClientMessage.Disconnected, "Disconnected by user", ep);
					break;

				case HostMessage.SetReadyState:
					if (State != GameState.WaitingForPlayers)
						break;

					player = GetPlayer (playerId);

					if (player != null)
						player.ReadyToPlay = r.ReadBoolean ();
					break;

				case HostMessage.GameData:
					w.Write (playerId);
					var off = ms.Length;
					OnGameDataReceived (GetPlayer (playerId), r, w);

					if (ms.Length == off) {
						ms.SetLength (0);
					}
					break;

				
				case HostMessage.GetPlayerInfo:
					player = GetPlayer (playerId);

					if (player == null)
						break;

					w.Write (player.Id);
					w.Write (player.ReadyToPlay);

					OnGetPlayerInfo (player, w);
					break;
				case HostMessage.GetPlayersInfo:
					w.Write (playerId);

					lock (players) {
						w.Write ((byte)PlayerCount);
						foreach (var p in players) {
							w.Write (p.Nick);
							w.Write (p.ReadyToPlay);
							OnGetPlayerGeneralInfo (p, w);
						}
					}
					break;
			}

			if(answer.Length > 0)
				Send (answer, ep);
			w.Close ();
			answer.Dispose ();
		}
		#endregion

		#region Player
		protected abstract Player CreatePlayer(string nick);

		protected virtual void OnPlayerAdded(Player player)	{}
		protected virtual void OnPlayerDisconnecting(Player p,ClientMessage reason) {}
		protected virtual void OnPlayerDisconnected(Player p,ClientMessage reason) {}
		protected virtual void OnGetPlayerInfo(Player p, BinaryWriter w) {}
		/// <summary>
		/// Possibility to pass additional general (can seen by all players!) data.
		/// </summary>
		protected virtual void OnGetPlayerGeneralInfo(Player p, BinaryWriter w) {}

		public bool DisconnectPlayer(BinaryWriter w,long id, ClientMessage reason = ClientMessage.Disconnected, string message = null, IPEndPoint ep = null)
		{
			var player = GetPlayer(id);

			if(player != null)
			{
				OnPlayerDisconnecting (player, reason);
				players.Remove (player);
				w.Write (id);
				w.Write ((byte)reason);
				if (message == null)
					w.Write ((byte)0);
				else
					w.Write (message);
				player.Address = ep;
				OnPlayerDisconnected (player, reason);
				return true;
			}
			return false;
		}

		string GetValidPlayerNick(string originalNick)
		{
			lock (players) {
				bool rep;
				string currentNick = originalNick;
				int i = 2;
				do {
					rep = false;
					foreach (var p in players)
						if (p.Nick == currentNick) {
							currentNick = originalNick + (i++).ToString();
							rep = true;
							break;
						}
				} while(rep);

				return currentNick;
			}
		}
		#endregion

		#region Game generics
		protected virtual void OnGameDataReceived(Player playerOpt, BinaryReader input, BinaryWriter response) {}

		protected void SendGameDataToPlayer(Player p, byte[] data)
		{
			using (var ms = new MemoryStream ())
				using (var w = new BinaryWriter(ms)) {
				w.Write (p.Id);
				w.Write (data);
				Send (ms, p.Address);
			}
		}

		protected void SendGameDataToAllPlayers(byte[] data)
		{
			using (var ms = new MemoryStream ())
			using (var w = new BinaryWriter(ms)) {
				lock (players)
					foreach (var p in players) {
						w.Write (p.Id);
						w.Write (data);
						Send (ms, p.Address);
						ms.SetLength (0);
					}
			}
		}
		#endregion
	}
}

