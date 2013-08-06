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
		#endregion

		#region Events
		public delegate void ConnectedHandler();
		public event ConnectedHandler Connected;

		public delegate void DisconnectedHandler(ClientMessage msg, string reason);
		public event DisconnectedHandler Disconnected;

		public delegate void ChatHandler(string nick, string message);
		public event ChatHandler ChatArrived;

		public event Action<bool> ReadyStateChanged;

		public event Action<string> OtherPlayerLeft;
		#endregion

		#region Init / Constructor
		public static GameConnection TryEstablishConnection(IPEndPoint ip, long hostId, string nick, 
		                                                    GameHostFactory fact, 
		                                                    ConnectedHandler onconnected=null,
		                                                    DisconnectedHandler ondisconnected = null)
		{
			var conn = fact.CreateConnection();

			if(onconnected != null)
				conn.Connected += onconnected;
			if (ondisconnected != null)
				conn.Disconnected += ondisconnected;

			conn.HostId = hostId;
			conn.Init (ip, nick);

			return conn;
		}

		protected GameConnection() : base(ServerToClientCommunicationPort) {}

		protected virtual void Init(IPEndPoint hostAddress, string nick)
		{
			HostAddress = hostAddress;

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

				case ClientMessage.OtherPlayerLeft:
					if (OtherPlayerLeft != null) {
						OtherPlayerLeft (r.ReadString ());
					}
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

		protected virtual void OnPlayerInfoReceived(BinaryReader r) {}
		protected virtual void OnComposePlayerInfo(BinaryWriter w) {}

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

		protected virtual void OnGeneralPlayerInfoReceived(string nick, bool isReady, BinaryReader r) {}

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
				w.Write (message);

				Send (ms, HostAddress);
			}
		}

		#endregion

		protected virtual void OnGameDataReceived(BinaryReader r) {}
	}
}
