using System.Data;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    public class EventQuestionArg : EventArgs
    {
        public EventQuestionArg(string text)
        {
            this.Text = text;
        }
        public string Text { get; }
        public override string ToString()
        {
            return "--------------------------" + Environment.NewLine + Text + Environment.NewLine;
        }
    }
    public class EventImageArg: EventArgs
    {
        public Image Image { get; }
        public EventImageArg(Image Image)
        {
            this.Image = Image;
        }
    }
    public class TeleBot
    {
        TCPControl tcpControl;
        MySQL mySQL;
        long IdAdminTG;
        object obj = new object();
        TelegramBotClient botClient;
        CancellationTokenSource cts;
        UpdateHandler handler;
        ReceiverOptions receiverOptions;
        string buffer = "";
        EventWaitHandle eventMessage = new EventWaitHandle(false, EventResetMode.AutoReset);
        public EventHandler<EventQuestionArg> ?EventTeleBotMessage { get; set; }
        public EventHandler<EventImageArg> ?EventTeleBotMessageImage { get; set; }
        public TeleBot()
        {
            mySQL = new MySQL("clicker");
            using (DataTable userAdmin = mySQL.GetDataTableSQL("SELECT id_chat FROM users WHERE id = 1"))
                if(userAdmin.Rows.Count != 0)
                    IdAdminTG = long.Parse(userAdmin.Rows[0]["id_chat"].ToString());
            string ?token = null;
            using (DataTable settingTG = mySQL.GetDataTableSQL("SELECT token FROM telegram_setting WHERE id = 1"))
                if (settingTG.Rows.Count != 0)
                    token = settingTG.Rows[0]["token"].ToString();
            if (token == null)
                throw new Exception("Error token TG");
            botClient = new TelegramBotClient(token);
            cts = new CancellationTokenSource();
            handler = new UpdateHandler(mySQL);
            handler.Question = false;
            handler.eventQuestion += GetMessageQuestion;
            receiverOptions = new ReceiverOptions();

            IPAddress iP = Dns.GetHostAddresses(Dns.GetHostName()).First<IPAddress>(f => f.AddressFamily == AddressFamily.InterNetwork);
            if (iP == null)
                throw new Exception("Error IP server");

            mySQL.SendSQL("UPDATE object SET status = 'online' , ip = '" + iP.ToString() + "' , port = " + TCPControl.Port.ToString() + " WHERE id = 1");

            tcpControl = new TCPControl(iP);
            tcpControl.StartListing();
            tcpControl.MessageReceived += Message;
        }
        public async void Start()
        {
            EventTeleBotMessage(this, new EventQuestionArg("Start Telegram Bot"));
            botClient.StartReceiving(handler, receiverOptions, cancellationToken: cts.Token);
            Console.WriteLine("Bot started");
            await Task.Run(() =>
                Task.Delay(-1, cancellationToken: cts.Token) // Такой вариант советуют MS: https://github.com/dotnet/runtime/issues/28510#issuecomment-458139641
            );
            Console.WriteLine("Bot stopped");
        }
        public void Stop()
        {
            mySQL.SendSQL("UPDATE object SET status = 'offline' WHERE id = 1");
            cts.Cancel();
        }
        public void ResetQuestion()
        {
            handler.Question = false;
            MessageBox.Show("Reset question bot.");
        }
        public void GetMessageQuestion(object? e, EventQuestionArg arg)
        {
            EventTeleBotMessage(this, arg);
            buffer = arg.Text;
            eventMessage.Set();
        }
        public void Message(object? e, EventArgTCPClient arg)
        {
            lock (obj)
            {
                EventTeleBotMessage(this, new EventQuestionArg(arg.ToString()));
                if (arg.Message.Data.Length > 0)
                    EventTeleBotMessageImage(this, new EventImageArg(arg.GetImage()));
                switch (arg.Message.Type)
                {
                    case TypeMessage.CaptchaImage:
                        {
                            using (MemoryStream ms = new MemoryStream(arg.Message.Data))
                            {
                                eventMessage.Reset();
                                botClient.SendPhotoAsync(IdAdminTG, new InputMedia(ms, "Screen.png"));
                                handler.Question = true;
                                eventMessage.WaitOne();
                                tcpControl.SendResultMessage(buffer, arg.Message.IDMashine, arg.Message.Site);
                            };
                            return;
                        }
                    case TypeMessage.Error:
                        {
                            botClient.SendTextMessageAsync(IdAdminTG, arg.Message.Text);
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Error type message");
                            return;
                        }
                }
            }
        }
        class UpdateHandler : IUpdateHandler
        {
            MySQL mySQL;
            public bool Question { get; set; }
            public EventHandler<EventQuestionArg> ?eventQuestion { get; set; }
            public UpdateHandler(MySQL mySQL)
            {
                this.mySQL = mySQL;
            }
            public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                if (update.Type == UpdateType.Message)
                {
                    var message = update.Message;
                    if (Question)
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat, text: "OK", cancellationToken: cancellationToken);
                        eventQuestion(this, new EventQuestionArg(message.Text));
                        Question = false;
                    }
                    else
                        Request(message.Text, botClient, update.Message.Chat, cancellationToken);
                }
            }
            private async void Request(string request, ITelegramBotClient botClient, Chat chat, CancellationToken cancellationToken)
            {
                switch (request)
                {
                    case "status":
                        {
                            string answer = String.Empty;
                            using (DataTable status = mySQL.GetDataTableSQL("SELECT name, status FROM object"))
                            {
                                if (status.Rows.Count > 0)
                                {
                                    foreach(DataRow row in status.Rows)
                                    {
                                        answer += "Object: " + row["name"].ToString() + " status: " + row["status"].ToString() + "\n";
                                    }
                                }
                                else
                                {
                                    answer = "Нет данных.";
                                }
                                await botClient.SendTextMessageAsync(chatId: chat, text: answer, cancellationToken: cancellationToken);
                            }
                            return;
                        }
                    default:
                        {
                            await botClient.SendTextMessageAsync(chatId: chat, text: "Ошибка запроса", cancellationToken: cancellationToken);
                            return;
                        }
                }
            }
            public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                Console.Error.WriteLine(exception);
                return Task.CompletedTask;
            }
            public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}