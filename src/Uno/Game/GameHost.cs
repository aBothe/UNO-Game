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
			lock(players)
				foreach (var p in players)
					if (p.Id == id)
						return p;
			return null;
		}

		public Player GetPlayer(string nick)
		{
			lock(players)
				foreach (var p in players)
					if (p.Nick == nick)
						return p;
			return null;
		}

		public int GetPlayerIndex(Player p)
		{
			return players.IndexOf (p);
		}

		public Player GetPlayerByIndex(int i)
		{
			return players [i];
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

				lock(players)
					foreach (var p in players)
						if (!p.ReadyToPlay)
							return false;

				return true;
			}
		}

		public event Action<bool> GameStartabilityChanged;
		#endregion

		#region Init / Constructor
		public GameHost() : base(ServerPort)
		{

		}

		public static GameHost HostGame(GameHostFactory fac)
		{
			if (IsHosting)
				throw new InvalidOperationException ("Shut down other host first!");

			var host = fac.Create ();

			// Initialisiert Spielhost, setzt den Host in den Lobbymodus, und wartet, 
			// bis 2-10 Spieler verbunden & bereit sind + der Spielersteller das Spiel startet

			Instance = host;

			host.State = GameState.Starting;
			host.State = GameState.WaitingForPlayers;

			return host;
		}
		#endregion

		#region Exit / Destructor
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
			State = GameState.ShuttingDown;

			SendToAllPlayers (new[] { (byte)ClientMessage.ServerShutdown });

			Dispose ();
		}
		#endregion




		#region Messaging
		protected override void DataReceived (IPEndPoint ep,BinaryReader r)
		{
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
						//Info: The package sent in OnPlayerAdded will not be received by the recently connected player - because the JoinGranted message will arrive later!
						OnPlayerAdded (player);
					} else {
						w.Write ((byte)ClientMessage.JoinDenied);
						w.Write ("Player limit reached!");
					}
					break;

				case HostMessage.Disconnect:
					player = GetPlayer (playerId);

					if(player!=null)
						DisconnectPlayer (w, player, ClientMessage.Disconnected, "Disconnected by user", ep);
					break;

				case HostMessage.GetReadyState:
					player = GetPlayer (playerId);

					if (player != null) {
						w.Write (playerId);
						w.Write ((byte)ClientMessage.IsReady);
						w.Write (player.ReadyToPlay);
					}
					break;

				case HostMessage.SetReadyState:
					if (State != GameState.WaitingForPlayers)
						break;

					player = GetPlayer (playerId);

					if (player != null) {
						player.ReadyToPlay = r.ReadBoolean ();

						// Send update to clients
						DistributeGeneralPlayerUpdate (player);
						CheckGameStartable ();
					}
					break;

				case HostMessage.ChatMessage:
					player = GetPlayer (playerId);

					if (player == null)
						break;

					var chat = r.ReadString ();
					if (!string.IsNullOrEmpty (chat)) {
						using (var ms2 = new MemoryStream())
						using (var w2 = new BinaryWriter(ms2)) {
							w2.Write ((byte)ClientMessage.ChatMessage);
							w2.Write (player.Nick);
							w2.Write (chat);
							SendToAllPlayers (ms2.ToArray ());
						}
					}
					break;

				case HostMessage.GameData:
					w.Write (playerId);
					var off = answer.Length;
					OnGameDataReceived (GetPlayer (playerId), r, w);

					if (answer.Length == off) {
						answer.SetLength (0);
					}
					break;

				
				case HostMessage.GetPlayerInfo:
					player = GetPlayer (playerId);

					if (player == null)
						break;

					w.Write (player.Id);
					ComposeSpecificPlayerInfoBytes (player, w);
					break;

				case HostMessage.GetPlayersInfo:
					w.Write (playerId);
					ComposeGeneralPlayerInfoBytes (w);
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

		protected virtual void OnPlayerAdded(Player player)	{
			DistributeGeneralPlayerUpdate (player);
			CheckGameStartable ();
		}
		protected virtual void OnPlayerDisconnecting(Player p,ClientMessage reason) {}

		protected virtual void OnPlayerDisconnected(Player p,ClientMessage reason) {
			using (var ms = new MemoryStream())
				using (var w = new BinaryWriter(ms)) {
				w.Write ((byte)ClientMessage.OtherPlayerLeft);
				w.Write (p.Nick);

				SendToAllPlayers (ms.ToArray ());
				CheckGameStartable ();
			}
		}
		protected virtual void OnComposePlayerInfo(Player p, BinaryWriter w) {}
		/// <summary>
		/// Possibility to pass additional general (can seen by all players!) data.
		/// </summary>
		protected virtual void OnComposeGeneralPlayerInfo(Player p, BinaryWriter w) {}

		public void DisconnectPlayer(Player player, ClientMessage reason = ClientMessage.Disconnected, string message = null)
		{
			using (var ms = new MemoryStream())
			using (var w = new BinaryWriter(ms)) {
				DisconnectPlayer (w, player, reason, message);
				Send (ms, player.Address);
			}
		}

		public void DisconnectPlayer(BinaryWriter w,Player player, ClientMessage reason = ClientMessage.Disconnected, string message = null, IPEndPoint ep = null)
		{
			OnPlayerDisconnecting (player, reason);
			lock(players)
				players.Remove (player);
			w.Write (player.Id);
			w.Write ((byte)reason);
			if (message == null)
				w.Write ((byte)0);
			else
				w.Write (message);
			if(ep != null)
				player.Address = ep;
			OnPlayerDisconnected (player, reason);
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

		protected void DistributeGeneralPlayerUpdate(long id)
		{
			var p = GetPlayer (id);
			if (p == null)
				throw new InvalidDataException ("id");

			DistributeGeneralPlayerUpdate (p);
		}

		protected void DistributeGeneralPlayerUpdate(Player p)
		{
			using (var ms = new MemoryStream())
				using (var w = new BinaryWriter(ms)) {
				w.Write ((byte)ClientMessage.GeneralPlayersInfo);
				w.Write ((byte)1);

				w.Write (p.Nick);
				w.Write (p.ReadyToPlay);
				OnComposeGeneralPlayerInfo (p, w);
				SendToAllPlayers (ms.ToArray ());
			}
		}

		protected void DistributeGeneralPlayerUpdate()
		{
			using (var ms = new MemoryStream())
				using (var w = new BinaryWriter(ms)) {
				ComposeGeneralPlayerInfoBytes (w);
				SendToAllPlayers (ms.ToArray ());
			}
		}

		protected void ComposeGeneralPlayerInfoBytes(BinaryWriter w)
		{
			w.Write ((byte)ClientMessage.GeneralPlayersInfo);

			lock (players) {
				w.Write ((byte)PlayerCount);
				foreach (var p in players) {
					w.Write (p.Nick);
					w.Write (p.ReadyToPlay);
					OnComposeGeneralPlayerInfo (p, w);
				}
			}
		}

		void ComposeSpecificPlayerInfoBytes(Player p, BinaryWriter w)
		{
			w.Write ((byte)ClientMessage.PlayerInfo);
			//w.Write (player.ReadyToPlay);

			OnComposePlayerInfo (p, w);
		}

		protected void DistributeSpecificPlayerUpdate(long id)
		{
			var p = GetPlayer (id);
			if (p == null)
				throw new InvalidDataException ("id");

			DistributeSpecificPlayerUpdate (p);
		}

		protected void DistributeSpecificPlayerUpdate(Player p)
		{
			using (var ms = new MemoryStream())
				using (var w = new BinaryWriter(ms)) {
				ComposeSpecificPlayerInfoBytes (p, w);
				SendToPlayer (p,ms.ToArray());
			}
		}

		protected void DistributeSpecificPlayerUpdate()
		{
			using (var ms = new MemoryStream())
			using (var w = new BinaryWriter(ms)) {
				lock (players)
					foreach(var p in players) {
						ComposeSpecificPlayerInfoBytes (p, w);
						SendToPlayer (p,ms.ToArray());
						ms.SetLength (0);
					}
			}
		}
		#endregion

		#region Game generics
		protected virtual void OnGameDataReceived(Player playerOpt, BinaryReader input, BinaryWriter response) {}

		protected void SendGameDataToPlayer(Player p, byte[] data)
		{
			var actData = new byte[data.Length + sizeof(long) + 1];

			BitConverter.GetBytes (p.Id).CopyTo (actData, 0);
			actData [sizeof(long)] = (byte)ClientMessage.GameData;
			data.CopyTo (actData, sizeof(long) + 1);

			Send (actData, p.Address);
		}

		protected void SendToPlayer(Player p, byte[] data)
		{
			var actData = new byte[data.Length + sizeof(long)];

			BitConverter.GetBytes (p.Id).CopyTo (actData, 0);
			data.CopyTo (actData, sizeof(long));

			Send (actData, p.Address);
		}

		protected void SendToAllPlayers(byte[] data)
		{
			var actData = new byte[data.Length + sizeof(long)];

			data.CopyTo (actData, sizeof(long));

			lock (players)
				foreach (var p in players) {
					BitConverter.GetBytes (p.Id).CopyTo (actData, 0);
					Send (actData, p.Address);
				}
		}

		protected void SendGameDataToAllPlayers(byte[] data)
		{
			var actData = new byte[data.Length + sizeof(long) + 1];

			actData [sizeof(long)] = (byte)ClientMessage.GameData;
			data.CopyTo (actData, sizeof(long) + 1);

			lock (players)
				foreach (var p in players) {
					BitConverter.GetBytes (p.Id).CopyTo (actData, 0);
					Send (actData, p.Address);
				}
		}

		public void CheckGameStartable()
		{
			if(GameStartabilityChanged != null)
				GameStartabilityChanged (ReadyToPlay);
		}

		public bool StartGame()
		{
			if (!ReadyToPlay)
				return false;

			State = GameState.StartPlaying;

			if (!StartGameInternal ()) {
				State = GameState.WaitingForPlayers;
				return false;
			}

			State = GameState.Playing;

			SendToAllPlayers (new[] { (byte)ClientMessage.GameStarted });

			return true;
		}

		protected abstract bool StartGameInternal();

		/// <summary>
		/// Notices the host engine that the game has been finished. Must be called by the Game in order to keep the flow consistent!
		/// </summary>
		protected virtual void NoticeGameFinished(bool aborted = false)
		{
			State = GameState.GameFinished;
			SendToAllPlayers (new[] { (byte)ClientMessage.GameFinished, (byte)(aborted?1:0) });
			State = GameState.WaitingForPlayers;
		}
		#endregion
	}
}

