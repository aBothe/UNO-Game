namespace Uno.Uno
{
    partial class GameField
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ln_aktuelleKarte = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Bn_Return = new System.Windows.Forms.Button();
            this.Bn_Forward = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Ln_Player4 = new System.Windows.Forms.Label();
            this.Pb_Player4 = new System.Windows.Forms.PictureBox();
            this.Ln_player3 = new System.Windows.Forms.Label();
            this.Pb_Player3 = new System.Windows.Forms.PictureBox();
            this.Ln_Player2 = new System.Windows.Forms.Label();
            this.Pb_Player2 = new System.Windows.Forms.PictureBox();
            this.Lb_Card = new System.Windows.Forms.Label();
            this.Pb_aktelleKarte = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_aktelleKarte)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.26102F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.73898F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(980, 567);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ln_aktuelleKarte);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.Bn_Return);
            this.panel1.Controls.Add(this.Bn_Forward);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 321);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 243);
            this.panel1.TabIndex = 0;
            // 
            // ln_aktuelleKarte
            // 
            this.ln_aktuelleKarte.AutoSize = true;
            this.ln_aktuelleKarte.Location = new System.Drawing.Point(345, 219);
            this.ln_aktuelleKarte.Name = "ln_aktuelleKarte";
            this.ln_aktuelleKarte.Size = new System.Drawing.Size(161, 13);
            this.ln_aktuelleKarte.TabIndex = 3;
            this.ln_aktuelleKarte.Text = "Zeigt die Aktuellle Zahl der Karte";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(374, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 126);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Bn_Return
            // 
            this.Bn_Return.Location = new System.Drawing.Point(293, 136);
            this.Bn_Return.Name = "Bn_Return";
            this.Bn_Return.Size = new System.Drawing.Size(75, 23);
            this.Bn_Return.TabIndex = 2;
            this.Bn_Return.Text = "Return";
            this.Bn_Return.UseVisualStyleBackColor = true;
            this.Bn_Return.Click += new System.EventHandler(this.Bn_Return_Click);
            // 
            // Bn_Forward
            // 
            this.Bn_Forward.Location = new System.Drawing.Point(491, 136);
            this.Bn_Forward.Name = "Bn_Forward";
            this.Bn_Forward.Size = new System.Drawing.Size(75, 23);
            this.Bn_Forward.TabIndex = 1;
            this.Bn_Forward.Text = "Forward";
            this.Bn_Forward.UseVisualStyleBackColor = true;
            this.Bn_Forward.Click += new System.EventHandler(this.Bn_Forward_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Ln_Player4);
            this.panel2.Controls.Add(this.Pb_Player4);
            this.panel2.Controls.Add(this.Ln_player3);
            this.panel2.Controls.Add(this.Pb_Player3);
            this.panel2.Controls.Add(this.Ln_Player2);
            this.panel2.Controls.Add(this.Pb_Player2);
            this.panel2.Controls.Add(this.Lb_Card);
            this.panel2.Controls.Add(this.Pb_aktelleKarte);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(974, 312);
            this.panel2.TabIndex = 1;
            // 
            // Ln_Player4
            // 
            this.Ln_Player4.AutoSize = true;
            this.Ln_Player4.Location = new System.Drawing.Point(870, 288);
            this.Ln_Player4.Name = "Ln_Player4";
            this.Ln_Player4.Size = new System.Drawing.Size(42, 13);
            this.Ln_Player4.TabIndex = 7;
            this.Ln_Player4.Text = "Player4";
            // 
            // Pb_Player4
            // 
            this.Pb_Player4.Location = new System.Drawing.Point(853, 152);
            this.Pb_Player4.Name = "Pb_Player4";
            this.Pb_Player4.Size = new System.Drawing.Size(85, 133);
            this.Pb_Player4.TabIndex = 6;
            this.Pb_Player4.TabStop = false;
            // 
            // Ln_player3
            // 
            this.Ln_player3.AutoSize = true;
            this.Ln_player3.Location = new System.Drawing.Point(431, 128);
            this.Ln_player3.Name = "Ln_player3";
            this.Ln_player3.Size = new System.Drawing.Size(41, 13);
            this.Ln_player3.TabIndex = 5;
            this.Ln_player3.Text = "player3";
            // 
            // Pb_Player3
            // 
            this.Pb_Player3.Location = new System.Drawing.Point(406, 3);
            this.Pb_Player3.Name = "Pb_Player3";
            this.Pb_Player3.Size = new System.Drawing.Size(100, 122);
            this.Pb_Player3.TabIndex = 4;
            this.Pb_Player3.TabStop = false;
            // 
            // Ln_Player2
            // 
            this.Ln_Player2.AutoSize = true;
            this.Ln_Player2.Location = new System.Drawing.Point(39, 288);
            this.Ln_Player2.Name = "Ln_Player2";
            this.Ln_Player2.Size = new System.Drawing.Size(42, 13);
            this.Ln_Player2.TabIndex = 3;
            this.Ln_Player2.Text = "Player2";
            // 
            // Pb_Player2
            // 
            this.Pb_Player2.Location = new System.Drawing.Point(18, 162);
            this.Pb_Player2.Name = "Pb_Player2";
            this.Pb_Player2.Size = new System.Drawing.Size(86, 123);
            this.Pb_Player2.TabIndex = 2;
            this.Pb_Player2.TabStop = false;
            // 
            // Lb_Card
            // 
            this.Lb_Card.AutoSize = true;
            this.Lb_Card.Location = new System.Drawing.Point(414, 291);
            this.Lb_Card.Name = "Lb_Card";
            this.Lb_Card.Size = new System.Drawing.Size(73, 13);
            this.Lb_Card.TabIndex = 1;
            this.Lb_Card.Text = "Aktuelle Karte";
            // 
            // Pb_aktelleKarte
            // 
            this.Pb_aktelleKarte.Location = new System.Drawing.Point(406, 162);
            this.Pb_aktelleKarte.Name = "Pb_aktelleKarte";
            this.Pb_aktelleKarte.Size = new System.Drawing.Size(100, 126);
            this.Pb_aktelleKarte.TabIndex = 0;
            this.Pb_aktelleKarte.TabStop = false;
            // 
            // GameField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 605);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GameField";
            this.Text = "GameField";
            this.Load += new System.EventHandler(this.GameField_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Player2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_aktelleKarte)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Bn_Return;
        private System.Windows.Forms.Button Bn_Forward;
        private System.Windows.Forms.Label ln_aktuelleKarte;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Lb_Card;
        private System.Windows.Forms.PictureBox Pb_aktelleKarte;
        private System.Windows.Forms.Label Ln_Player4;
        private System.Windows.Forms.PictureBox Pb_Player4;
        private System.Windows.Forms.Label Ln_player3;
        private System.Windows.Forms.PictureBox Pb_Player3;
        private System.Windows.Forms.Label Ln_Player2;
        private System.Windows.Forms.PictureBox Pb_Player2;
    }
}