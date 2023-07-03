namespace TelegramBot
{
    partial class TelegramBotManager
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
            components = new System.ComponentModel.Container();
            usersData = new DataGridView();
            menuStrip1 = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripUser = new ContextMenuStrip(components);
            создатьToolStripMenuItem = new ToolStripMenuItem();
            изменитьToolStripMenuItem = new ToolStripMenuItem();
            удалитьToolStripMenuItem = new ToolStripMenuItem();
            resetQuakToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)usersData).BeginInit();
            menuStrip1.SuspendLayout();
            contextMenuStripUser.SuspendLayout();
            SuspendLayout();
            // 
            // usersData
            // 
            usersData.AllowUserToAddRows = false;
            usersData.AllowUserToDeleteRows = false;
            usersData.AllowUserToResizeColumns = false;
            usersData.AllowUserToResizeRows = false;
            usersData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            usersData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            usersData.BackgroundColor = SystemColors.ButtonFace;
            usersData.Location = new Point(12, 27);
            usersData.MultiSelect = false;
            usersData.Name = "usersData";
            usersData.ReadOnly = true;
            usersData.RowHeadersVisible = false;
            usersData.RowTemplate.Height = 25;
            usersData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersData.Size = new Size(776, 411);
            usersData.TabIndex = 1;
            usersData.DoubleClick += изменитьToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, resetQuakToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(48, 20);
            файлToolStripMenuItem.Text = "Файл";
            // 
            // contextMenuStripUser
            // 
            contextMenuStripUser.Items.AddRange(new ToolStripItem[] { создатьToolStripMenuItem, изменитьToolStripMenuItem, удалитьToolStripMenuItem });
            contextMenuStripUser.Name = "contextMenuStrip1";
            contextMenuStripUser.Size = new Size(129, 70);
            // 
            // создатьToolStripMenuItem
            // 
            создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            создатьToolStripMenuItem.Size = new Size(128, 22);
            создатьToolStripMenuItem.Text = "Создать";
            // 
            // изменитьToolStripMenuItem
            // 
            изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            изменитьToolStripMenuItem.Size = new Size(128, 22);
            изменитьToolStripMenuItem.Text = "Изменить";
            // 
            // удалитьToolStripMenuItem
            // 
            удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            удалитьToolStripMenuItem.Size = new Size(128, 22);
            удалитьToolStripMenuItem.Text = "Удалить";
            // 
            // resetQuakToolStripMenuItem
            // 
            resetQuakToolStripMenuItem.Name = "resetQuakToolStripMenuItem";
            resetQuakToolStripMenuItem.Size = new Size(47, 20);
            resetQuakToolStripMenuItem.Text = "Reset";
            resetQuakToolStripMenuItem.Click += resetQuakToolStripMenuItem_Click;
            // 
            // TelegramBotManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(usersData);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TelegramBotManager";
            Text = "TelegramBotManager";
            ((System.ComponentModel.ISupportInitialize)usersData).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStripUser.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView usersData;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ContextMenuStrip contextMenuStripUser;
        private ToolStripMenuItem создатьToolStripMenuItem;
        private ToolStripMenuItem изменитьToolStripMenuItem;
        private ToolStripMenuItem удалитьToolStripMenuItem;
        private ToolStripMenuItem resetQuakToolStripMenuItem;
    }
}