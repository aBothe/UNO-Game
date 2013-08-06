//
// GameHostBackend.cs
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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Uno.Game
{
	public abstract class HostBackend : IDisposable
	{
		public const int ClientToServerCommunicationPort = 55001;
		public const int ServerToClientCommunicationPort = 55002;
		UdpClient udp;

		public readonly IPEndPoint Address;

		public HostBackend (int port)
		{
			udp = new UdpClient ();
			udp.ExclusiveAddressUse = false;

			udp.Client.Bind (Address = new IPEndPoint(IPAddress.Any,port));

			var listenerThread = new Thread(listenerTh);
			listenerThread.IsBackground = true;
			listenerThread.Start ();
		}

		public virtual void Dispose()
		{
			udp.Close ();
		}

		protected abstract void DataReceived(IPEndPoint ep,BinaryReader data);

		protected void Send(MemoryStream ms, IPEndPoint ep)
		{
			Send (ms.ToArray (), ep);
		}

		protected void Send(IPEndPoint ep, params IEnumerable<byte>[] data)
		{
			var l = new List<byte> ();
			foreach (var d in data)
				l.AddRange (d);

			udp.Send (l.ToArray(), l.Count, ep);
		}

		protected void Send(byte[] data, IPEndPoint ep)
		{
			udp.Send (data, data.Length, ep);
		}

		void listenerTh()
		{
			while(udp.Client.IsBound)
			{
				IPEndPoint targetAddress = null;
				var data = udp.Receive(ref targetAddress);
				using(var ms = new MemoryStream(data))
				using(var br = new BinaryReader(ms))
				DataReceived (targetAddress,br);
			}
		}
	}
}

