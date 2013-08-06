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
		public const int ServerPort = 55001;
		UdpClient udp;
		ManualResetEvent dataReceivedEvt = new ManualResetEvent(true);
		bool disposed;
		Thread listenerThread;

		public IPEndPoint Address{get{return udp.Client.LocalEndPoint as IPEndPoint;}}

		public HostBackend (int port = 0)
		{
			udp = new UdpClient ();
			udp.ExclusiveAddressUse = false;
			udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

			// Nur an bestimmten Port binden, wenn dies verlangt ist. Andernfalls macht UdpClient dies automatisch und sucht einen freien Port aus!
			if (port > 0) {
				udp.Client.Bind (new IPEndPoint (IPAddress.Any, port));

			}

			InitListener ();
		}

		public virtual void Dispose()
		{
			dataReceivedEvt.Reset ();
			disposed = true;
			udp.Close ();
		}

		~HostBackend()
		{
			Dispose ();
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

			if(!disposed)
				udp.Send (l.ToArray(), l.Count, ep);
		}

		protected void Send(byte[] data, IPEndPoint ep)
		{
			if(!disposed)
				udp.Send (data, data.Length, ep);
		}

		void InitListener()
		{
			if (listenerThread != null && listenerThread.IsAlive)
				return;

			listenerThread = new Thread(listenerTh);
			listenerThread.IsBackground = true;
			listenerThread.Start ();
		}

		void listenerTh()
		{
			while(udp.Client != null && dataReceivedEvt.WaitOne(0))
			{
				byte[] data = null;
				IPEndPoint targetAddress = null;
				try{
					data = udp.Receive(ref targetAddress);
				}catch(SocketException ex) {
					if (dataReceivedEvt.WaitOne (0))
						throw ex;
				}

				if (data != null) {
					using (var ms = new MemoryStream(data))
					using (var br = new BinaryReader(ms))
						DataReceived (targetAddress, br);
				}
			}
		}
	}
}

