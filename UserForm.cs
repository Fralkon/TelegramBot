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
    public partial class UserForm : Form
    {
        MySQL mySQL;
        string idUser = "";
        public UserForm(MySQL mySQL)
        {
            InitializeComponent();
            this.mySQL = mySQL;
        }
        public UserForm(MySQL mySQL, string idUser)
        {
            InitializeComponent();
            this.mySQL = mySQL;
            this.idUser = idUser;
            string SQLData = "SELECT name, id_chat FROM users WHERE id = "+ idUser;
            DataTable datatable = mySQL.GetDataTableSQL(SQLData);
            if(datatable.Rows.Count > 0)
            {
                nameText.Text = datatable.Rows[0][0].ToString();
                idChatText.Text = datatable.Rows[0][1].ToString();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(idChatText.Text.Length == 0)
            {
                MessageBox.Show("Введите ID чата");
                return;
            }
            if(nameText.Text.Length == 0)
            {
                MessageBox.Show("Введите пользателя");
                return;
            }
            if (idUser == "")
            {
                mySQL.SendSQL("INSERT users (id_chat, name) VALUES('" + idChatText.Text + "'," + nameText.Text + ")");
            }
            else
            {
                mySQL.SendSQL("UPDATE users SET id_chat = '" + idChatText.Text + "' , name = '" + nameText.Text + "' WHERE id = " + idUser);
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
