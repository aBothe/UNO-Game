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

        public Bitmap getImage()
        {
           
            String imageLocation = "/Uno Karten/";

            switch (Color)
            {
                case CardColor.Red:
                    imageLocation += "RED/";
                    break;
                case CardColor.Green:
                    imageLocation += "GREEN/";
                    break;
                case CardColor.Yellow:
                    imageLocation += "YELLOW/";
                    break;
                case CardColor.Blue:
                    imageLocation += "BLUE/";
                    break;
                case CardColor.Black:
                    imageLocation += "BLACK/";
                    break;
            }

            switch (Caption)
            {
                case CardCaption.Zero:
                    imageLocation += "ZERO.png";
                    break;
                case CardCaption.One:
                    imageLocation += "ONE.png";
                    break;
                case CardCaption.Two:
                    imageLocation += "TWO.png";
                    break;
                case CardCaption.Three:
                    imageLocation += "THREE.png";
                    break;
                case CardCaption.Four:
                    imageLocation += "FOUR.png";
                    break;
                case CardCaption.Five:
                    imageLocation += "FIVE.png";
                    break;
                case CardCaption.Six:
                    imageLocation += "SIX.png";
                    break;
                case CardCaption.Seven:
                    imageLocation += "SEVEN.png";
                    break;
                case CardCaption.Eight:
                    imageLocation += "EIGHT.png";
                    break;
                case CardCaption.Nine:
                    imageLocation += "NINE.png";
                    break;

                case CardCaption.WishColor:
                    imageLocation += "WILD.png";
                    break;

                case CardCaption.Take4:
                    imageLocation += "FOUR_WILD.png";
                    break;

                case CardCaption.Take2:
                    imageLocation += "DRAW_TWO.png";
                    break;

                case CardCaption.RevertDirectionAndNewColor:
                    imageLocation += "REVERSE.png";
                    break;


                case CardCaption.SkipNextPlayer:
                    imageLocation += "SKIP.png";
                    break;
            }

           
            Bitmap bitmap = new Bitmap("C:/Users/Tido/Documents/Visual Studio 2012/Projects/UNO-Game/src/Uno/bin/Debug/"+imageLocation);
           bitmap =  ResizeMe(bitmap,80);
            return bitmap;

        }

        private static Bitmap ResizeMe(Image srcImg, double dblWidth)
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
