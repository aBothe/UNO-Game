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
using System.IO;
using System.Collections.Generic;
using Uno.Game;

namespace Uno.Games
{
	public class ServerListBackend
	{
		#region Properties
		public const int MetaCommunicationPort = 55000;
		internal static readonly IPAddress multicastaddress = IPAddress.Parse("239.0.0.222"); 
		internal static readonly IPEndPoint multicastEndpoint;
		UdpClient udp;

		public event Action<GameHostEntry> EntryReceived;
		#endregion

		static ServerListBackend()
		{
			multicastEndpoint = new IPEndPoint(multicastaddress, MetaCommunicationPort);
		}

		public ServerListBackend ()
		{
			GameHost.AnyGameStateChanged += ThisHostStateChanged;
			udp = new UdpClient ();
			udp.ExclusiveAddressUse = false;
			udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			udp.JoinMulticastGroup (multicastaddress);

			udp.Client.Bind (new IPEndPoint(IPAddress.Any,MetaCommunicationPort));

			var listenerThread =new Thread(listenerTh);
			listenerThread.IsBackground = true;
			listenerThread.Start ();
		}

		~ServerListBackend()
		{
			GameHost.AnyGameStateChanged -= ThisHostStateChanged;
			udp.Close ();
		}

		void ThisHostStateChanged(object sender, GameStateChangedArgs ea)
		{
			SendHostUpdate (ea.Host);
		}

		public void SendExistenceRequest()
		{
			udp.Send (new []{(byte)InteractionMessage.PingRequest }, 1, multicastEndpoint);
		}

		public void SendHostUpdate()
		{
			if(GameHost.IsHosting)
				SendHostUpdate (GameHost.Instance);
		}

		public void SendHostUpdate(GameHost host)
		{
			var d = GameHostEntry.SerializeHost(host);
			if (d != null && udp.Client != null)
				udp.Send(d, d.Length, multicastEndpoint);
		}

		void listenerTh()
		{
			while(udp.Client != null)
			{
				IPEndPoint targetAddress = null;
				var data = udp.Receive(ref targetAddress);

				switch ((InteractionMessage)data [0]) {
					case InteractionMessage.PingRequest:
						SendHostUpdate ();
						break;
					case InteractionMessage.PingAnswer:
						if (EntryReceived != null) {
							var gh = GameHostEntry.FromBytes (targetAddress, data, 1);
							EntryReceived (gh);
						}
						break;
				}
			}
		}
	}
}

