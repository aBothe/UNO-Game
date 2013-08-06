//
// Player.cs
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
using System.Net;

namespace Uno.Game
{
	public class Player
	{
		public readonly GameHost Host;
		public readonly long Id = IdGenerator.GenerateId();
		public readonly string Nick;
		public IPEndPoint Address;

		bool ready = false;
		public bool ReadyToPlay{
			get{
				return ready;
			}
			set{
				if (Host.State != GameState.WaitingForPlayers)
					throw new InvalidOperationException ("Can't modify player state when not in game lobby!");
				ready = value;
			}
		}

		public Player (GameHost host, string nick)
		{
			Nick = nick;
			Host = host;

			host.GameStateChanged += OnGameStateChanged;
		}

		protected virtual void OnGameStateChanged(object sender, GameStateChangedArgs e)
		{
			if(e.NewState== GameState.GameFinished)
				ready = false;
		}
	}
}

