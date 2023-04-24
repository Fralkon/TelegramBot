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
        }    
        public void GetMessageQuestion(object e, EventQuestionArg arg)
        {
            textBox1.Invoke((MethodInvoker)delegate { textBox1.Text +=  arg.ToString(); });
        }
        public void GetMessageQuestionBitmap(object e, EventImageArg arg)
        {
            pictureBox1.Invoke((MethodInvoker)delegate { pictureBox1.Image = arg.Image; });
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            teleBot.EventTeleBotMessage += GetMessageQuestion;
            teleBot.EventTeleBotMessageImage += GetMessageQuestionBitmap;
            teleBot.Start(); 
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            teleBot.Stop();
        }
        private void objectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectsManager authForm = new ObjectsManager(mySQL);
            authForm.ShowDialog();
        }

        private void telegramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TelegramBotManager telegramBotManager = new TelegramBotManager(mySQL);
            telegramBotManager.ShowDialog();
        }
    }
}