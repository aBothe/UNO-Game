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
        
        CardDeck deck = new CardDeck();

        public GameField()
        {
            InitializeComponent();
            deck.Reset();
            for (int i = 0; i < 7; i++)
            {
                KartenBilder.Add(pictureBox1);
                KartenBilder.Add(pictureBox2);
                KartenBilder.Add(pictureBox3);
                KartenBilder.Add(pictureBox4);
                KartenBilder.Add(pictureBox5);
                KartenBilder.Add(pictureBox6);
                KartenBilder.Add(pictureBox7);
            }
        }

        private void GameField_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
           

            List<Card> card = deck.GiveFirstHand();

            for (int i = 0; i < 7; i++)
            {
                Bitmap bild = card[i].getImage();


                KartenBilder[i].Image = bild;
                KartenBilder[i].Height = bild.Height;
                KartenBilder[i].Width = bild.Width;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
