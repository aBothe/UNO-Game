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
		public float[,] xyImage;

		#region Properties

		public readonly UnoGameConnection Connection;

		#endregion

		#region Init / Constructor

		public GameField (UnoGameConnection con)
		{
			this.Connection = con;
			InitializeComponent ();

			con.PropertyChanged += PropChanged;
		}

		~GameField()
		{
			Connection.PropertyChanged -= PropChanged;
		}

		bool loaded;
		private void GameField_Load (object sender, EventArgs e)
		{
			loaded = true;
		}

		#endregion

		void PropChanged(UnoProperty prop)
		{
			if (!loaded)
				return;

			BeginInvoke (new MethodInvoker (() => {
				switch (prop) {
					case UnoProperty.OwnHand:
						handPanel.Invalidate ();
						break;
					case UnoProperty.OtherGameStates:
					case UnoProperty.OtherPlayersHandSize:
						mainPanel.Invalidate ();
						break;
					case UnoProperty.NextPlayer:

						break;
				}
			}));
		}

		#region Main panel

		public const double TAU = 2 * Math.PI;
		Font playerFont = SystemFonts.CaptionFont;
		Font activePlayerFont;
		Bitmap blankCardImage = Card.GetImage (CardCaption.None, CardColor.Black);

		private void mainPanel_Paint (object sender, PaintEventArgs e)
		{
			if (activePlayerFont == null)
				activePlayerFont = new Font (playerFont, FontStyle.Bold);

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

			g.DrawImage (img, new RectangleF (mittelPunkt_X - dblWidth / 2f, (mittelPunkt_Y + dblHeight) < hoehe ? mittelPunkt_Y : (hoehe - dblHeight), dblWidth, dblHeight));

			// Andere Spieler kreisförmig um die Mitte anordnen, 
			// wobei das Dreieck zwischen unterer linker, rechter Panelecke und Panelmittelpunkt ausgespart wird

			// Den verfügbaren Kreisausschnitt um die Panelmitte berechnen (Erklärung - siehe Doku)
			var gamma = Math.Acos (1.0 - (Math.Pow (breite, 2) / (2.0 * Math.Pow (mittelPunkt_Y, 2.0) + 0.5 * Math.Pow (breite, 2.0))));

			var numOtherPlayers = Connection.OtherPlayersHandSize.Count;
			var winkelDistanz = (TAU - gamma) / (double)numOtherPlayers;

			var radius = Math.Min (mittelPunkt_X, mittelPunkt_Y);
			var phase = -Math.PI / 2 + gamma / 2;

			dblWidth = 60f;
			dblFac = dblWidth / (float)img.Width;
			dblHeight = dblFac * img.Height;

			var playerEnum = Connection.OtherPlayersHandSize.GetEnumerator ();
			for (int i = 1; i < numOtherPlayers; i++) {
				playerEnum.MoveNext ();
				if (playerEnum.Current.Key == Connection.PlayerNick)
					playerEnum.MoveNext ();
				var kv = playerEnum.Current;

				var p_X = mittelPunkt_X + radius * (float)Math.Cos (winkelDistanz * i + phase);
				var p_Y = mittelPunkt_Y - radius * (float)Math.Sin (winkelDistanz * i + phase);

				var f = kv.Key == Connection.CurrentPlayer ? activePlayerFont : playerFont;
				var size = g.MeasureString (kv.Key, f);
				g.DrawString (kv.Key, f, Brushes.Black, p_X - size.Width / 2, p_Y);

				g.DrawImage (blankCardImage, p_X - dblWidth / 2, p_Y += size.Height, dblWidth, dblHeight);

				var numString = kv.Value.ToString ();

				size = g.MeasureString (numString, f);

				g.DrawString (numString, f, Brushes.Black, p_X - size.Width / 2, p_Y + dblHeight / 2 - size.Height / 2);
			}
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

		const float HandCardWidth = 80f;

		private void handPanel_Paint (object sender, PaintEventArgs e)
		{
			var widthPerCard = HandCardWidth / 2;
			var n = Connection.OwnHand.Count;

			var g = e.Graphics;
			var move_position = 0f;
			xyImage = new float[n, 2];

			var breite = (float)handPanel.Width;
			var hoehe = (float)handPanel.Height;
			var mittelPunkt_X = breite / 4f;

			// Wenn man derart viele Karten auf der Hand hat, dass es die Breite des Panels ausfüllt,
			if (n * widthPerCard > (breite - mittelPunkt_X)) {
				//.. den Mittelpunkt nach links anpassen
				mittelPunkt_X = breite - (n * widthPerCard);

				// Wenn die Hand bis nach links aus dem Panel reicht, die Darstellungsbreite pro Karte anpassen
				if (mittelPunkt_X < 0) {
					mittelPunkt_X = 0;
					widthPerCard = breite / n;
				}
			}

			for (int i = 0; i < n; i++) {
				var c = Connection.OwnHand [i];
				var img = c.GetImage ();

				xyImage [i, 0] = mittelPunkt_X + move_position;
				xyImage [i, 1] = (Connection.RecommendedCards.Contains(c) ? 0f : 15f);
				move_position += widthPerCard;

				var dblFac = HandCardWidth / (float)img.Width;
				var dblHeight = dblFac * img.Height;

				g.DrawImage (img, new RectangleF (xyImage [i, 0], xyImage [i, 1], HandCardWidth, dblHeight));
			}
		}

		private void handPanel_MouseMove (object sender, MouseEventArgs e)
		{
          
		}

		private void handPanel_MouseClick (object sender, MouseEventArgs e)
		{
			int bild = HitTestCard (e.Location);
			if (bild != -1) {
				var c = Connection.OwnHand [bild];
				var col = c.Color;

				if (c.Color == CardColor.Black) {
					var chooser = new ColorChooser ();
					chooser.ShowDialog (this);
					col = chooser.SelectedColor;
				}

				Connection.PutCardOnStackTop (c, col);
				//MessageBox.Show ("Zeichen : " + Connection.OwnHand [bild].Caption + "  Zahl :" + Connection.OwnHand [bild].Color);
			}
		}

		int HitTestCard (Point loc)
		{
			var x = loc.X;
			var y = loc.Y;

			int bild = -1;
			for (int i = 0; i < Connection.OwnHand.Count; i++) {
				var img = Connection.OwnHand [i].GetImage ();

				var dblFac = HandCardWidth / (float)img.Width;
				var dblHeight = dblFac * img.Height;

				//float test1=  xyImage[i, 0];
				float test2 = xyImage [i, 0] + HandCardWidth;
				//float test3 = xyImage[i, 1];
				//float test4 = xyImage[i, 1] + dblHeight;

				if (x >= xyImage [i, 0] && x <= xyImage [i, 0] + HandCardWidth && y >= xyImage [i, 1] && y <= xyImage [i, 1] + dblHeight) {
					//println("Bild angeklickt:"+i);
					bild = i;
					// MessageBox.Show("" + bild);
				}

			}
			return bild;

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
