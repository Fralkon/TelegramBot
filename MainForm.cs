using System.Data;
using System.Net;

namespace TelegramBot
{
    public partial class MainForm : Form
    {
        TeleBot teleBot;
        MySQL mySQL;
        public MainForm()
        {
            teleBot = new TeleBot();
            mySQL = new MySQL("clicker");
            InitializeComponent();
            usersData.ContextMenuStrip = contextMenuStrip1;
        }
        public void GetMessageQuestion(object e, EventQuestionArg arg)
        {
            textBox1.Invoke(()=> { textBox1.AppendText(arg.ToString()); });
        }
        public void GetMessageQuestionBitmap(object e, EventImageArg arg)
        {
            pictureBox1.Invoke(()=> { pictureBox1.Image = arg.Image; });
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            teleBot.EventTeleBotMessage += GetMessageQuestion;
            teleBot.EventTeleBotMessageImage += GetMessageQuestionBitmap;
            teleBot.Start();
            UpDateTableUser();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            teleBot.Stop();
        }
        private void ñîçäàòüToolStripMenuItem_Click(object sender, EventArgs e)
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
            datatable.Columns["id_chat"].ColumnName = "ID ÷àò";
            datatable.Columns["name"].ColumnName = "Èìÿ";
            datatable.Columns["query_count"].ColumnName = "Êîë çàïðîñîâ";
            usersData.DataSource = datatable;
        }
        private void èçìåíèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usersData.SelectedRows.Count != 0)
            {
                UserForm user = new UserForm(mySQL, usersData.SelectedRows[0].Cells[0].Value.ToString());
                user.ShowDialog();
                UpDateTableUser();
            }
        }
        private void óäàëèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usersData.SelectedRows.Count == 1)
            {
                mySQL.SendSQL("DELETE FROM users WHERE id = " + usersData.SelectedRows[0].Cells[0].Value.ToString());
                UpDateTableUser();
            }
        }
        private void resetQuakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            teleBot.ResetQuestion();
        }
    }
}