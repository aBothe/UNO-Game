//
// Card.cs
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

namespace Uno
{
	public class Card
	{
		public readonly CardColor Color;
		public readonly CardCaption Caption;

		public Card (CardColor Color, CardCaption Caption)
		{
			this.Color = Color;
			this.Caption = Caption;
		}

		public ushort ToHash ()
		{
			return (ushort)(((byte)Color << 8) + (byte)Caption);
		}

		public static Card FromHash (ushort hash)
		{
			return new Card((CardColor)(hash >> 8), (CardCaption)hash);
		}

		public override bool Equals (object obj)
		{
			return obj is Card && (obj as Card).ToHash () == ToHash ();
		}
	}

	public enum CardColor : byte
	{
		Red,
		Green,
		Blue,
		Yellow,
		Black
	}

	public enum CardCaption : byte
	{
		Zero,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		/// <summary>
		/// Colored.
		/// </summary>
		Take2,
		RevertDirectionAndNewColor,
		SkipNextPlayer,
		/// <summary>
		/// Black
		/// </summary>
		Take4,
		WishColor,
	}
}

