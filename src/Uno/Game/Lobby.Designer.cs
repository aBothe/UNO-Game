namespace Uno.Game
{
    partial class Lobby
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.button_StartGame = new System.Windows.Forms.Button();
			this.button_Ready = new System.Windows.Forms.CheckBox();
			this.list_Players = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.text_ChatLog = new System.Windows.Forms.TextBox();
			this.text_ChatMessage = new System.Windows.Forms.TextBox();
			this.button_SendChat = new System.Windows.Forms.Button();
			this.button_ReturnToLobby = new System.Windows.Forms.Button();
			this.panel_Admin = new System.Windows.Forms.Panel();
			this.button_KickPlayers = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel_Admin.SuspendLayout();
			this.SuspendLayout();
			// 
			// button_StartGame
			// 
			this.button_StartGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_StartGame.Enabled = false;
			this.button_StartGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button_StartGame.Location = new System.Drawing.Point(71, 29);
			this.button_StartGame.Name = "button_StartGame";
			this.button_StartGame.Size = new System.Drawing.Size(117, 23);
			this.button_StartGame.TabIndex = 1;
			this.button_StartGame.Text = "Start Game";
			// 
			// button_Ready
			// 
			this.button_Ready.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Ready.Location = new System.Drawing.Point(397, 327);
			this.button_Ready.Name = "button_Ready";
			this.button_Ready.Size = new System.Drawing.Size(75, 23);
			this.button_Ready.TabIndex = 0;
			this.button_Ready.Text = "Ready";
			this.button_Ready.Appearance = System.Windows.Forms.Appearance.Button;
			this.button_Ready.Click += button_Ready_Click;
			// 
			// list_Players
			// 
			this.list_Players.Location = new System.Drawing.Point(12, 25);
			this.list_Players.Name = "list_Players";
			this.list_Players.Size = new System.Drawing.Size(191, 168);
			this.list_Players.TabIndex = 2;
			this.list_Players.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_Players.DrawItem += DrawPlayerItem;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(95, 13);
			label1.TabIndex = 3;
			label1.Text = "Waiting Players";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(209, 9);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(33, 13);
			label2.TabIndex = 4;
			label2.Text = "Chat";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.button_SendChat);
			this.panel1.Controls.Add(this.text_ChatMessage);
			this.panel1.Controls.Add(this.text_ChatLog);
			this.panel1.Location = new System.Drawing.Point(212, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(260, 290);
			this.panel1.TabIndex = 5;
			// 
			// text_ChatLog
			// 
			this.text_ChatLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.text_ChatLog.BackColor = System.Drawing.Color.White;
			this.text_ChatLog.Location = new System.Drawing.Point(0, 0);
			this.text_ChatLog.Multiline = true;
			this.text_ChatLog.Name = "text_ChatLog";
			this.text_ChatLog.ReadOnly = true;
			this.text_ChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.text_ChatLog.Size = new System.Drawing.Size(260, 264);
			this.text_ChatLog.TabIndex = 0;
			// 
			// text_ChatMessage
			// 
			this.text_ChatMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.text_ChatMessage.Location = new System.Drawing.Point(0, 270);
			this.text_ChatMessage.Name = "text_ChatMessage";
			this.text_ChatMessage.Size = new System.Drawing.Size(211, 20);
			this.text_ChatMessage.TabIndex = 1;
			this.text_ChatMessage.KeyDown += text_ChatMessage_KeyDown;
			// 
			// button_SendChat
			// 
			this.button_SendChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_SendChat.Location = new System.Drawing.Point(217, 269);
			this.button_SendChat.Name = "button_SendChat";
			this.button_SendChat.Size = new System.Drawing.Size(43, 21);
			this.button_SendChat.TabIndex = 6;
			this.button_SendChat.Text = "Say";
			this.button_SendChat.Click += button_SendChat_Click;
			// 
			// button_ReturnToLobby
			// 
			this.button_ReturnToLobby.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button_ReturnToLobby.Location = new System.Drawing.Point(12, 327);
			this.button_ReturnToLobby.Name = "button_ReturnToLobby";
			this.button_ReturnToLobby.Size = new System.Drawing.Size(104, 23);
			this.button_ReturnToLobby.TabIndex = 6;
			this.button_ReturnToLobby.Text = "Return to Lobby";
			this.button_ReturnToLobby.Click += new System.EventHandler(this.button_ReturnToLobby_Click);
			// 
			// panel_Admin
			// 
			this.panel_Admin.Controls.Add(this.button_KickPlayers);
			this.panel_Admin.Controls.Add(this.button_StartGame);
			this.panel_Admin.Location = new System.Drawing.Point(12, 199);
			this.panel_Admin.Name = "panel_Admin";
			this.panel_Admin.Size = new System.Drawing.Size(191, 116);
			this.panel_Admin.TabIndex = 7;
			// 
			// button_KickPlayers
			// 
			this.button_KickPlayers.Location = new System.Drawing.Point(71, 0);
			this.button_KickPlayers.Name = "button_KickPlayers";
			this.button_KickPlayers.Size = new System.Drawing.Size(120, 23);
			this.button_KickPlayers.TabIndex = 8;
			this.button_KickPlayers.Text = "Kick selected players";
			this.button_KickPlayers.Click += button_KickPlayers_Click;
			// 
			// Lobby
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(484, 362);
			this.Controls.Add(this.panel_Admin);
			this.Controls.Add(this.button_ReturnToLobby);
			this.Controls.Add(this.panel1);
			this.Controls.Add(label2);
			this.Controls.Add(label1);
			this.Controls.Add(this.button_Ready);
			this.Controls.Add(this.list_Players);
			this.MinimumSize = new System.Drawing.Size(500, 400);
			this.Name = "Lobby";
			this.Text = "Game Lobby";
			this.Load += new System.EventHandler(this.Lobby_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel_Admin.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button button_StartGame;
		private System.Windows.Forms.CheckBox button_Ready;
		private System.Windows.Forms.ListBox list_Players;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox text_ChatMessage;
		private System.Windows.Forms.TextBox text_ChatLog;
		private System.Windows.Forms.Button button_SendChat;
		private System.Windows.Forms.Button button_ReturnToLobby;
		private System.Windows.Forms.Panel panel_Admin;
		private System.Windows.Forms.Button button_KickPlayers;
    }
}