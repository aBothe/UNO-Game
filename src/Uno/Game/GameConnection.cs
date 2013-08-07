using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace Uno.Game
{
	public abstract class GameConnection : HostBackend
	{
		#region Properties
		public IPEndPoint HostAddress { get; private set; }
		public long HostId { get; private set; }

		readonly long ConnectId = IdGenerator.GenerateId();
		public long PlayerId {get; private set; }
		public string PlayerNick { get; private set;}

		bool connected;
		public bool IsConnected
		{
			get{return connected;}
		}

		bool isPlayerReady;
		public bool IsPlayerReady
		{
			get
			{
				return isPlayerReady;
			}
			set{
				SetPlayerReady (value);
			}
		}
		public bool IsGameStartable{ get; private set;}
		#endregion

		#region Events
		public delegate void NoArgDelegate();
		public event NoArgDelegate Connected;

		public delegate void DisconnectedHandler(ClientMessage msg, string reason);
		public event DisconnectedHandler Disconnected;

		public delegate void ChatHandler (string nick, string message);
		public event ChatHandler ChatArrived;

		public event NoArgDelegate GameStarted;
		public event Action<bool> GameFinished;

		public event Action<bool> ReadyStateChanged;
		public event Action<string> OtherPlayerLeft;

		public delegate void GeneralPlayerInfoHandler (string nick, bool isReady, object furtherInfo);
		public event GeneralPlayerInfoHandler GeneralPlayerInfoReceived;
		#endregion

		#region Init / Constructor
		/// <summary>
		/// Initialize() must be called after overloading first events!
		/// </summary>
		/// <param name="ip">Ip.</param>
		/// <param name="hostId">Host identifier.</param>
		/// <param name="fact">Fact.</param>
		public static GameConnection Create(IPAddress ip, long hostId, 
		                                                    GameHostFactory fact)
		{
			var conn = fact.CreateConnection();

			conn.HostId = hostId;
			conn.HostAddress = new IPEndPoint(ip, ServerPort);

			return conn;
		}

		public virtual void Initialize(string nick)
		{
			// Acquire approval from host
			SendConnectionRequest (nick);
		}

		void SendConnectionRequest(string nick)
		{
			using (var ms = new MemoryStream ())
			using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.Connect);
				w.Write (ConnectId);
				w.Write (nick);

				Send (ms, HostAddress);
			}
		}
		#endregion

		protected virtual void OnConnected()
		{
			connected = true;
			if(Connected!=null)
				Connected();
		}

		protected virtual void OnDisconnected(ClientMessage msg, string reason)
		{
			connected = false;
			if(Disconnected != null)
				Disconnected(msg, reason);
		}

		public void Disconnect()
		{
			using (var ms = new MemoryStream ())
				using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.Disconnect);
				w.Write (PlayerId);

				Send (ms, HostAddress);
			}
		}

		protected override void DataReceived (IPEndPoint ep, BinaryReader r)
		{
			// Id check
			var id = r.ReadInt64 ();
			if (PlayerId != 0) {
				if (id != PlayerId)
					return;
			}
			else if (ConnectId != id)
				return;

			var msg = (ClientMessage)r.ReadByte ();
			string nick;

			switch (msg) {
				case ClientMessage.JoinGranted:

					PlayerId = r.ReadInt64 ();
					PlayerNick = r.ReadString ();

					OnConnected ();
					break;

				case ClientMessage.Kicked:
				case ClientMessage.Disconnected:
				case ClientMessage.Timeout:
				case ClientMessage.JoinDenied:
					OnDisconnected (msg, r.ReadString ());
					break;
				
				case ClientMessage.ServerShutdown:
					OnDisconnected (msg, string.Empty);
					break;

				case ClientMessage.OtherPlayerLeft:
					PlayerNick = r.ReadString();

					OnOtherPlayerLeft(PlayerNick);

					if (OtherPlayerLeft != null)
						OtherPlayerLeft (PlayerNick);
					break;

				case ClientMessage.IsReady:
					isPlayerReady = r.ReadBoolean ();

					if (ReadyStateChanged != null)
						ReadyStateChanged (isPlayerReady);
					break;

				case ClientMessage.PlayerInfo:
					//isPlayerReady = r.ReadBoolean ();
					OnPlayerInfoReceived (r);
					break;

				case ClientMessage.ChatMessage:
					nick = r.ReadString ();
					var chat = r.ReadString ();

					if (ChatArrived != null)
						ChatArrived (nick, chat);
					break;

				case ClientMessage.GameStarted:
					OnGameStarted ();

					if (GameStarted != null)
						GameStarted ();
					break;
				case ClientMessage.GameFinished:
					var aborted = r.ReadBoolean ();

					OnGameFinished (aborted);

					if (GameFinished != null)
						GameFinished (aborted);
					break;

				case ClientMessage.GameData:
					OnGameDataReceived (r);
					break;

				case ClientMessage.GeneralPlayersInfo:
					var count = r.ReadByte ();
					while (count-- > 0) {
						nick = r.ReadString();
						var isReady = r.ReadBoolean ();
						OnGeneralPlayerInfoReceived (nick, isReady, r);
					}
					break;
			}
		}

		#region Player

		protected virtual void OnPlayerInfoReceived(BinaryReader r) { }
		protected virtual void OnOtherPlayerLeft(string nick) { }

		public void AcquirePlayerInfo()
		{
			using (var ms = new MemoryStream ())
				using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.GetPlayerInfo);
				w.Write (PlayerId);

				Send (ms, HostAddress);
			}
		}

		protected void FireGeneralPlayerInfoReceivedEvent(string nick, bool isReady, object furtherInfo = null)
		{
			if (GeneralPlayerInfoReceived != null)
				GeneralPlayerInfoReceived (nick, isReady, furtherInfo);
		}

		/// <summary>
		/// Raises the general player info received event.
		/// Override implementations may not call the first base implementation 
		/// if FireGeneralPlayerInfoReceivedEvent() has already been called!
		/// </summary>
		protected virtual void OnGeneralPlayerInfoReceived(string nick, bool isReady, BinaryReader r) {
			FireGeneralPlayerInfoReceivedEvent (nick, isReady, null);
		}

		public void AcquireGeneralPlayerInfo()
		{
			using (var ms = new MemoryStream ())
				using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.GetPlayersInfo);
				w.Write (PlayerId);

				Send (ms, HostAddress);
			}
		}

		void SetPlayerReady(bool ready)
		{
			using (var ms = new MemoryStream ())
			using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.SetReadyState);
				w.Write (PlayerId);
				w.Write (ready);

				Send (ms, HostAddress);
			}
		}

		public void SendChat(string message)
		{
			if(!string.IsNullOrEmpty(message))
			using (var ms = new MemoryStream ())
			using (var w = new BinaryWriter (ms)) {
				w.Write (HostId);
				w.Write ((byte)HostMessage.ChatMessage);
				w.Write (PlayerId);
				w.Write (message);

				Send (ms, HostAddress);
			}
		}

		#endregion

		protected virtual void OnGameDataReceived(BinaryReader r) {}
		protected virtual void OnGameStarted() {}
		protected virtual void OnGameFinished(bool aborted) {}

		public void SendGameData(MemoryStream ms)
		{
			SendGameData(ms.ToArray());
		}

		public void SendGameData(byte[] data)
		{
			using (var ms = new MemoryStream())
			using (var w = new BinaryWriter(ms))
			{
				w.Write(HostId);
				w.Write((byte)HostMessage.GameData);
				w.Write(PlayerId);
				w.Write(data);

				Send(ms, HostAddress);
			}
		}
	}
}
