using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelegramBot
{
    public partial class TelegramBotManager : Form
    {
        MySQL mySQL;
        TeleBot bot;
        public TelegramBotManager(MySQL mySQL, TeleBot bot)
        {
            this.bot = bot;
            this.mySQL = mySQL;
            InitializeComponent();
            usersData.ContextMenuStrip = contextMenuStripUser;
            UpDateTableUser();
        }
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserForm user = new UserForm(mySQL);
            user.ShowDialog();
            UpDateTableUser();
        }
        private void UpDateTableUser()
        {
            string SQLData = "SELECT * FROM users";
            DataTable datatable = mySQL.GetDataTableSQL(SQLData);
           
            datatable.Columns["id"].ColumnName = "ID";
            datatable.Columns["id_chat"].ColumnName = "ID чат";
            datatable.Columns["name"].ColumnName = "Имя";
            datatable.Columns["query_count"].ColumnName = "Кол запросов";
            usersData.DataSource = datatable;
        }
        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usersData.SelectedRows.Count != 0)
            {
                UserForm user = new UserForm(mySQL, usersData.SelectedRows[0].Cells[0].Value.ToString());
                user.ShowDialog();
                UpDateTableUser();
            }
        }
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usersData.SelectedRows.Count == 1)
            {
                mySQL.SendSQL("DELETE FROM users WHERE id = " + usersData.SelectedRows[0].Cells[0].Value.ToString());
                UpDateTableUser();
            }
        }
        private void resetQuakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot.ResetQuestion();
        }
    }
}
