using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Uno.Game;

namespace Uno.Uno
{
    public partial class GameField : Form
	{
		#region Properties
		public readonly UnoGameConnection Connection;
        List<PictureBox> KartenBilder = new List<PictureBox>();
        List<Bitmap> bilder = new List<Bitmap>();
        List<Card> spielerHand = new List<Card>();
        Card backCard = new Card(CardColor.None, CardCaption.None);
        int aktuellePoistionHand = 0;
        
        CardDeck deck = new CardDeck();
		#endregion

		#region Init / Constructor
		public GameField(UnoGameConnection con)
        {
			this.Connection = con;
            InitializeComponent();
            deck.Reset();
           
                //KartenBilder.Add(pictureBox1);
         
            
        }

        private void GameField_Load(object sender, EventArgs e)
        {
           spielerHand = deck.GiveFirstHand();

            for (int i = 0; i < 7; i++)
            {
                Bitmap bild = spielerHand[i].getImage();
                bilder.Add(bild);
            }
        }
		#endregion

		#region Main panel
		private void mainPanel_Paint(object sender, PaintEventArgs e)
		{

		}
		#endregion

		#region Hand panel
		private void handPanel_Paint(object sender, PaintEventArgs e)
		{
			
		}

		private void handPanel_MouseMove(object sender, MouseEventArgs e)
		{

		}

		private void handPanel_MouseClick(object sender, MouseEventArgs e)
		{

		}
		#endregion

		#region Buttons
		private void button_DrawCard_Click(object sender, EventArgs e)
		{
			Connection.DrawCard();
		}

		private void check_Uno_CheckedChanged(object sender, EventArgs e)
		{
			Connection.PressUnoButton();
		}

		private void button_Skip_Click(object sender, EventArgs e)
		{
			Connection.SkipRound();
		}

		private void button_Leave_Click(object sender, EventArgs e)
		{
			Connection.Disconnect();
		}
		#endregion
	}
}
