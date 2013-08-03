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

namespace Uno
{
	public abstract class HostBackend : IDisposable
	{
		public const int ClientToServerCommunicationPort = 55001;
		UdpClient udp;

		public IPEndPoint Address
		{
			get{
				return udp.Client.RemoteEndPoint as IPEndPoint;
			}
		}

		public HostBackend ()
		{
			udp = new UdpClient ();
			udp.ExclusiveAddressUse = false;

			udp.Client.Bind (new IPEndPoint(IPAddress.Any,ClientToServerCommunicationPort));

			var listenerThread = new Thread(listenerTh);
			listenerThread.IsBackground = true;
			listenerThread.Start ();
		}

		public virtual void Dispose()
		{
			udp.Close ();
		}

		protected abstract void DataReceived(byte[] data);

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
				DataReceived (data);
			}
		}
	}
}

