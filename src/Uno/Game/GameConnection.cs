using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace Uno.Game
{
	public abstract class GameConnection : IDisposable
	{
		#region Properties
		public const int ServerToClientCommunicationPort = 55002;
		UdpClient udp;
		public IPEndPoint HostAddress { get; private set; }
		public long HostId { get; private set; }
		readonly long ConnectId = IdGenerator.GenerateId();
		public long PlayerId {get; private set; }
		public string PlayerNick { get; private set;}

		public IPEndPoint Address
		{
			get{
				return udp.Client.RemoteEndPoint as IPEndPoint;
			}
		}

		bool connected;
		public bool IsConnected
		{
			get{return connected;}
		}

		public delegate void ConnectedHandler();
		public event ConnectedHandler Connected;

		public delegate void DisconnectedHandler(ClientMessage msg, string reason);
		public event DisconnectedHandler Disconnected;

		#endregion

		#region Init / Constructor
		public static GameConnection TryEstablishConnection(IPEndPoint ip, long hostId, string nick, GameHostFactory fact, ConnectedHandler onconnected=null)
		{
			var conn = fact.CreateConnection();

			if(onconnected != null)
				conn.Connected += onconnected;

			conn.HostId = hostId;
			conn.Init (ip, nick);

			return conn;
		}

		protected virtual void Init(IPEndPoint hostAddress, string nick)
		{
			HostAddress = hostAddress;
			udp = new UdpClient();
			udp.ExclusiveAddressUse = false;

			udp.Client.Bind (new IPEndPoint(IPAddress.Any,ServerToClientCommunicationPort));

			// Acquire approval from host
			SendConnectionRequest (nick);

			new Thread (listenerTh) {
				IsBackground = true,
				Name = "ClientListener"
			}.Start ();
		}

		void SendConnectionRequest(string nick)
		{
			var ms = new MemoryStream ();
			var w = new BinaryWriter (ms);
			w.Write (HostId);
			w.Write ((byte)HostMessage.Connect);
			w.Write (ConnectId);
			w.Write (nick);

			udp.Send (ms.ToArray (), (int)ms.Length, HostAddress);

			w.Close ();
			ms.Dispose ();
		}
		#endregion

		public virtual void Dispose()
		{
			udp.Close ();
		}

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

		void listenerTh()
		{
			while(udp.Client.IsBound)
			{
				IPEndPoint targetAddress = null;
				var data = udp.Receive(ref targetAddress);

				var inputStream = new MemoryStream (data);
				var r = new BinaryReader (inputStream);

				// Id check
				var id = r.ReadInt64 ();
				if (PlayerId != 0) {
					if (id != PlayerId)
						return;
				}
				else if (ConnectId != id)
					return;

				var msg = (ClientMessage)r.ReadByte ();
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
				}

			}
		}
	}
}
