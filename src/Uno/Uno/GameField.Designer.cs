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
			this.mainPanel = new System.Windows.Forms.Panel();
			this.handPanel = new System.Windows.Forms.Panel();
			this.button_DrawCard = new System.Windows.Forms.Button();
			this.button_Skip = new System.Windows.Forms.Button();
			this.button_Leave = new System.Windows.Forms.Button();
			this.check_Uno = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// mainPanel
			// 
			this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainPanel.Location = new System.Drawing.Point(0, 0);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(859, 277);
			this.mainPanel.TabIndex = 0;
			this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
			// 
			// handPanel
			// 
			this.handPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.handPanel.Location = new System.Drawing.Point(0, 308);
			this.handPanel.Name = "handPanel";
			this.handPanel.Size = new System.Drawing.Size(859, 125);
			this.handPanel.TabIndex = 1;
			this.handPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.handPanel_Paint);
			this.handPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.handPanel_MouseClick);
			this.handPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.handPanel_MouseMove);
			// 
			// button_DrawCard
			// 
			this.button_DrawCard.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button_DrawCard.Location = new System.Drawing.Point(304, 283);
			this.button_DrawCard.Name = "button_DrawCard";
			this.button_DrawCard.Size = new System.Drawing.Size(75, 23);
			this.button_DrawCard.TabIndex = 2;
			this.button_DrawCard.Text = "Draw Card";
			this.button_DrawCard.UseVisualStyleBackColor = true;
			this.button_DrawCard.Click += new System.EventHandler(this.button_DrawCard_Click);
			// 
			// button_Skip
			// 
			this.button_Skip.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button_Skip.Location = new System.Drawing.Point(466, 283);
			this.button_Skip.Name = "button_Skip";
			this.button_Skip.Size = new System.Drawing.Size(75, 23);
			this.button_Skip.TabIndex = 4;
			this.button_Skip.Text = "Skip round";
			this.button_Skip.UseVisualStyleBackColor = true;
			this.button_Skip.Click += new System.EventHandler(this.button_Skip_Click);
			// 
			// button_Leave
			// 
			this.button_Leave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Leave.Location = new System.Drawing.Point(781, 283);
			this.button_Leave.Name = "button_Leave";
			this.button_Leave.Size = new System.Drawing.Size(75, 23);
			this.button_Leave.TabIndex = 5;
			this.button_Leave.Text = "Leave";
			this.button_Leave.UseVisualStyleBackColor = true;
			this.button_Leave.Click += new System.EventHandler(this.button_Leave_Click);
			// 
			// check_Uno
			// 
			this.check_Uno.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.check_Uno.Appearance = System.Windows.Forms.Appearance.Button;
			this.check_Uno.Location = new System.Drawing.Point(385, 283);
			this.check_Uno.Name = "check_Uno";
			this.check_Uno.Size = new System.Drawing.Size(75, 24);
			this.check_Uno.TabIndex = 6;
			this.check_Uno.Text = "UNO";
			this.check_Uno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.check_Uno.UseVisualStyleBackColor = true;
			this.check_Uno.CheckedChanged += new System.EventHandler(this.check_Uno_CheckedChanged);
			// 
			// GameField
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(857, 432);
			this.Controls.Add(this.check_Uno);
			this.Controls.Add(this.button_Leave);
			this.Controls.Add(this.button_Skip);
			this.Controls.Add(this.button_DrawCard);
			this.Controls.Add(this.handPanel);
			this.Controls.Add(this.mainPanel);
			this.DoubleBuffered = true;
			this.Name = "GameField";
			this.Text = "GameField";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.GameField_Load);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.Panel handPanel;
		private System.Windows.Forms.Button button_DrawCard;
		private System.Windows.Forms.Button button_Skip;
		private System.Windows.Forms.Button button_Leave;
		private System.Windows.Forms.CheckBox check_Uno;

	}
}