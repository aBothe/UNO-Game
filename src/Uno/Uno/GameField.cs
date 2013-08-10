using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Uno.Game;
using System.Drawing.Drawing2D;

namespace Uno.Uno
{
	public partial class GameField : Form
	{

		#region Properties

		public readonly UnoGameConnection Connection;

		#endregion

		#region Init / Constructor

		public GameField (UnoGameConnection con)
		{
			this.Connection = con;
			InitializeComponent ();
		}

		private void GameField_Load (object sender, EventArgs e)
		{

		}

		#endregion

		#region Main panel

		private void mainPanel_Paint (object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			
			// Oberste Karte im Stapel in der Mitte zeichnen
			var img = Connection.TopMostCard.GetImage ();

			var dblWidth = 80f;
			var dblFac = dblWidth / (float)img.Width;
			var dblHeight = dblFac * img.Height;

			g.DrawImage (img, new RectangleF ((float)mainPanel.Width / 2f - dblWidth / 2f, 
			                                  (float)mainPanel.Height / 2f - dblHeight / 2f, dblWidth, dblHeight));

			// Andere Spieler kreisförmig um die Mitte anordnen, 
			// wobei das Dreieck zwischen unterer linker, rechter Panelecke und Panelmittelpunkt ausgespart wird

		}

		public static Bitmap ResizeMe (Image srcImg, double dblWidth)
		{
			// Faktor berechnen
			double dblFac = dblWidth / srcImg.Width;
			double dblHeight = dblFac * srcImg.Height;

			// Bild bearbeiten
			Bitmap resizedImg = new Bitmap ((int)dblWidth, (int)dblHeight);
			using (Graphics gNew = Graphics.FromImage(resizedImg)) {
				gNew.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gNew.DrawImage (srcImg, new Rectangle (0, 0, (int)dblWidth, (int)dblHeight));
			}
			return resizedImg;
		}

		#endregion

		#region Hand panel

		private void handPanel_Paint (object sender, PaintEventArgs e)
		{
			
		}

		private void handPanel_MouseMove (object sender, MouseEventArgs e)
		{

		}

		private void handPanel_MouseClick (object sender, MouseEventArgs e)
		{

		}

		#endregion

		#region Buttons

		private void button_DrawCard_Click (object sender, EventArgs e)
		{
			Connection.DrawCard ();
		}

		private void check_Uno_CheckedChanged (object sender, EventArgs e)
		{
			Connection.PressUnoButton ();
		}

		private void button_Skip_Click (object sender, EventArgs e)
		{
			Connection.SkipRound ();
		}

		private void button_Leave_Click (object sender, EventArgs e)
		{
			Connection.Disconnect ();
		}

		#endregion

	}
}
