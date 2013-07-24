//
// HostListener.cs
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Uno.GameHost;
using System.IO;
using System.Collections.Generic;

namespace Uno.GameList
{
	public class GameHostEntry
	{
		public int PlayerCount;
		public int MaxPlayers;
		public GameState State;
		public IPEndPoint Address;
	}

	public class ServerListBackend
	{
		#region Properties
		internal static readonly IPAddress multicastaddress = IPAddress.Parse("239.0.0.222"); 
		internal static readonly IPEndPoint multicastEndpoint;
		UdpClient udp;

		List<GameHostEntry> lastHosts = new List<GameHostEntry>();
		public event Action<GameHostEntry> EntryReceived;

		public IEnumerable<GameHostEntry> LastHosts
		{
			get{ return lastHosts; }
		}
		#endregion

		static ServerListBackend()
		{
			multicastEndpoint = new IPEndPoint(multicastaddress, 55000);
		}

		public ServerListBackend ()
		{
			udp = new UdpClient ();
			udp.ExclusiveAddressUse = false;
			udp.JoinMulticastGroup (multicastaddress);

			udp.Client.Bind (new IPEndPoint(IPAddress.Any,55000));

			var listenerThread =new Thread(listenerTh);
			listenerThread.IsBackground = true;
			listenerThread.Start ();
		}

		public void SendExistenceRequest()
		{
			lastHosts.Clear ();
			udp.Send (new []{(byte)InteractionMessage.PingRequest }, 1, multicastEndpoint);
		}

		void listenerTh()
		{
			while(true)
			{
				IPEndPoint targetAddress = null;
				var data = udp.Receive(ref targetAddress);

				switch ((InteractionMessage)data [0]) {
					case InteractionMessage.PingRequest:
						if (Host.IsHosting) {
							var host = Host.Instance;
							using (var ms = new MemoryStream ())
							using (var w = new BinaryWriter (ms)) {
								w.Write ((byte)InteractionMessage.PingAnswer);
								w.Write (host.PlayerCount);
								w.Write (host.MaxPlayers);
								w.Write ((byte)host.State);
								udp.Send (ms.GetBuffer (), (int)ms.Length, targetAddress);
							}
						}
						break;
					case InteractionMessage.PingAnswer:
						using (var ms = new MemoryStream (data))
						using (var r = new BinaryReader (ms)) {
							var gh = new GameHostEntry ();
							gh.Address = targetAddress;
							r.ReadByte ();
							gh.PlayerCount = r.ReadInt32 ();
							gh.MaxPlayers = r.ReadByte ();
							gh.State = (GameState)r.ReadByte ();
						}
						break;
				}
			}
		}
	}
}

