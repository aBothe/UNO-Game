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
        public float [,] xyImage;

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

			var breite = (float)mainPanel.Width;
			var hoehe = (float)mainPanel.Height;

			var mittelPunkt_X = breite / 2f;
			var mittelPunkt_Y = hoehe / 2f;

			// Oberste Karte im Stapel in der Mitte zeichnen
			var img = Connection.TopMostCard.GetImage ();

			var dblWidth = 80f;
			var dblFac = dblWidth / (float)img.Width;
			var dblHeight = dblFac * img.Height;

			g.DrawImage (img, new RectangleF (mittelPunkt_X - dblWidth / 2f, mittelPunkt_Y - dblHeight / 2f, dblWidth, dblHeight));

			// Andere Spieler kreisförmig um die Mitte anordnen, 
			// wobei das Dreieck zwischen unterer linker, rechter Panelecke und Panelmittelpunkt ausgespart wird

			// Den verfügbaren Kreisausschnitt um die Panelmitte berechnen (Erklärung - siehe Doku)
			var gamma = 360.0 - 180.0 * Math.Acos (1.0-(Math.Pow(breite,2) / (2.0 * Math.Pow(mittelPunkt_Y,2.0) + 0.5 * Math.Pow(breite,2.0)))) / Math.PI;

			var winkelDistanz = gamma / (double)Connection.OtherPlayersHandSize.Count;
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
            var g = e.Graphics;
            int move_position = 0;
            xyImage = new float[Connection.OwnHand.Count,2];

            for (int i = 0; i < Connection.OwnHand.Count; i++)
            {
                for (int j = 0; j < 2; j++) {

                    var breite = (float)handPanel.Width;
                    var hoehe = (float)handPanel.Height;
                    var mittelPunkt_X = breite / 4f;
                    var mittelPunkt_Y = hoehe / 2f;

                    if (j == 0)
                    {
                        xyImage[i, j] = mittelPunkt_X+move_position;
                    }
                    if (j == 1)
                    {
                        xyImage[i, j] = mittelPunkt_Y;
                    }
                }
                move_position += 40;
            }

            for (int i = 0; i < Connection.OwnHand.Count; i++) {
                var img = Connection.OwnHand[i].GetImage();
                var dblWidth = 80f;
                var dblFac = dblWidth / (float)img.Width;
                var dblHeight = dblFac * img.Height;
               
                g.DrawImage(img, new RectangleF(xyImage[i, 0], xyImage[i, 1],dblWidth, dblHeight));
            }
         
		}

        private void handPanel_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

		private void handPanel_MouseClick (object sender, MouseEventArgs e)
		{
            int mouseX = System.Windows.Forms.Cursor.Position.X;
            int mouseY = System.Windows.Forms.Cursor.Position.Y - mainPanel.Height;

          int  bild = checkImage(mouseX, mouseY);

          MessageBox.Show("Zeichen : " + Connection.OwnHand[bild].Caption + "  Zahl :" + Connection.OwnHand[bild].Color);
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

        public int checkImage(int wert1, int wert2) {
           // int mouseX = System.Windows.Forms.Cursor.Position.X;
           // int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int x = wert1;
            int y = wert2;
            int bild = -1;
            for (int i = 0; i < Connection.OwnHand.Count; i++)
            {
                var img = Connection.OwnHand[i].GetImage();
                var dblWidth = 80f;
                var dblFac = dblWidth / (float)img.Width;
                var dblHeight = dblFac * img.Height;
              float test1=  xyImage[i, 0];
              float test2 = xyImage[i, 0] + dblWidth;
              float test3 = xyImage[i, 1];
              float test4 = xyImage[i, 1] + dblHeight;

              if (x >= xyImage[i, 0] && x <= xyImage[i, 0] + dblWidth && y >= xyImage[i,1] && y <= xyImage[i,1] + dblHeight)
                {
                    //println("Bild angeklickt:"+i);
                    bild = i;
                   // MessageBox.Show("" + bild);
                }

            }
            return bild;
        
        }

		#endregion

	}
}
