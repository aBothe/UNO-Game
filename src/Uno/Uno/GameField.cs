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
        List<PictureBox> KartenBilder = new List<PictureBox>();
        List<Bitmap> bilder = new List<Bitmap>();
        List<Card> spielerHand = new List<Card>();
        Card backCard = new Card(CardColor.None, CardCaption.None);
        int aktuellePoistionHand = 0;
        
        CardDeck deck = new CardDeck();

        public GameField()
        {
            InitializeComponent();
            deck.Reset();
           
                KartenBilder.Add(pictureBox1);
         
            
        }

        private void GameField_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
            Pb_aktelleKarte.Image = deck.GiveCard().getImage();
            Pb_Player2.Image = backCard.getImage();
            Pb_Player3.Image = backCard.getImage();
            Pb_Player4.Image = backCard.getImage();

           spielerHand = deck.GiveFirstHand();

            for (int i = 0; i < 7; i++)
            {
                Bitmap bild = spielerHand[i].getImage();
                bilder.Add(bild);

            
            }
            pictureBox1.Image = bilder[aktuellePoistionHand];
          //  pictureBox1.Height = bilder[aktuellePoistionHand].Height;
          //  pictureBox1.Width = bilder[aktuellePoistionHand].Width;
            ln_aktuelleKarte.Text = aktuellePoistionHand.ToString();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Bn_Forward_Click(object sender, EventArgs e)
        {
            aktuellePoistionHand = (aktuellePoistionHand + 1) % spielerHand.Count;
            pictureBox1.Image = bilder[aktuellePoistionHand];
           // pictureBox1.Height = bilder[aktuellePoistionHand].Height;
           // pictureBox1.Width = bilder[aktuellePoistionHand].Width;
            ln_aktuelleKarte.Text = aktuellePoistionHand.ToString();
        }

        private void Bn_Return_Click(object sender, EventArgs e)
        {
            if (aktuellePoistionHand > 0)
            {
                aktuellePoistionHand = (aktuellePoistionHand - 1) % spielerHand.Count;
            }
            pictureBox1.Image = bilder[aktuellePoistionHand];
          //  pictureBox1.Height = bilder[aktuellePoistionHand].Height;
          //  pictureBox1.Width = bilder[aktuellePoistionHand].Width;
            ln_aktuelleKarte.Text = aktuellePoistionHand.ToString();
        }
    }
}
