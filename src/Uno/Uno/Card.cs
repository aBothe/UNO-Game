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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Uno.Properties;

namespace Uno
{
	public struct Card
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

		public override int GetHashCode()
		{
			return (int)ToHash();
		}
		
		public override bool Equals (object obj)
		{
			return obj is Card && ((Card)obj).ToHash () == ToHash ();
		}

        public Bitmap GetImage()
        {
			var sb = new StringBuilder();

			switch (Caption)
			{
				case CardCaption.Zero:
					sb.Append("ZERO");
					break;
				case CardCaption.One:
					sb.Append("ONE");
					break;
				case CardCaption.Two:
					sb.Append("TWO");
					break;
				case CardCaption.Three:
					sb.Append("THREE");
					break;
				case CardCaption.Four:
					sb.Append("FOUR");
					break;
				case CardCaption.Five:
					sb.Append("FIVE");
					break;
				case CardCaption.Six:
					sb.Append("SIX");
					break;
				case CardCaption.Seven:
					sb.Append("SEVEN");
					break;
				case CardCaption.Eight:
					sb.Append("EIGHT");
					break;
				case CardCaption.Nine:
					sb.Append("NINE");
					break;

				case CardCaption.WishColor:
					sb.Append("WILD");
					break;

				case CardCaption.Take4:
					sb.Append("FOUR_WILD");
					break;

				case CardCaption.Take2:
					sb.Append("DRAW_TWO");
					break;

				case CardCaption.RevertDirectionAndNewColor:
					sb.Append("REVERSE");
					break;

				case CardCaption.SkipNextPlayer:
					sb.Append("SKIP");
					break;

				case CardCaption.None:
					sb.Append("BACKCARD");
					break;
			}

            switch (Color)
            {
                case CardColor.Red:
                    sb.Append("_Red");
                    break;
                case CardColor.Green:
                    sb.Append("_Green");
                    break;
                case CardColor.Yellow:
                    sb.Append("_Yellow");
                    break;
                case CardColor.Blue:
                    sb.Append("_Blue");
                    break;
            }

			return Resources.ResourceManager.GetObject(sb.ToString()) as Bitmap;
        }

        public static Bitmap ResizeMe(Image srcImg, double dblWidth)
        {
            // Faktor berechnen
            double dblFac = dblWidth / srcImg.Width;
            double dblHeight = dblFac * srcImg.Height;

            // Bild bearbeiten
            Bitmap resizedImg = new Bitmap((int)dblWidth, (int)dblHeight);
            using (Graphics gNew = Graphics.FromImage(resizedImg))
            {
                gNew.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gNew.DrawImage(srcImg, new Rectangle(0, 0, (int)dblWidth, (int)dblHeight));
            }
            return resizedImg;
        }
	
	}

	public enum CardColor : byte
	{
		Red,
		Green,
		Blue,
		Yellow,
		Black,
        None
	}

	public enum CardCaption : byte
	{
		None,
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
		/// Colored. Zieh Zwei (+2) – Der nächste Spieler muss zwei Karten aufnehmen und wird übersprungen.
		/// </summary>
		Take2,
		/// <summary>
		/// Retour – Bei mehr als zwei Spielern wird die Spielrichtung gewechselt, 
		/// bei zwei Spielern hat die Karte die gleiche Funktion wie die Aussetzen-Karte.
		/// </summary>
		RevertDirectionAndNewColor,
		/// <summary>
		/// Aussetzen – Der nächste Spieler wird übersprungen.
		/// </summary>
		SkipNextPlayer,
		/// <summary>
		/// Zieh Vier Farbwahl (+4) – Der nächste Spieler muss vier Karten aufnehmen und wird übersprungen, außerdem kann der Spieler, der die +4 gelegt hat, die zu spielende Farbe bestimmen.
		/// </summary>
		Take4,
		/// <summary>
		/// Farbwahl – Der Spieler, der diese Karte spielt, schreibt dem nächsten Spieler vor, 
		/// welche Farbe dieser legen muss. 
		/// Man darf diese Karte jedoch nicht auf eine andere Farbwahlkarte legen.
		/// </summary>
		WishColor
	}
}

