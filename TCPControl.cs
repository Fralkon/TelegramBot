using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelegramBot
{
    public enum EnumIPManager
    {
        BotTelegram,
        ClickMashine
    }
    public static class IPManager {
        public static IPEndPoint GetEndPoint(EnumIPManager enumIP)
        {
            switch (enumIP)
            {
                case EnumIPManager.BotTelegram: return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1000);
                case EnumIPManager.ClickMashine: return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1001);
                default: throw new Exception("Error GetEndPoint");
            }
        }
    }
    enum EnumTypeSite
    {
        None,
        Router,
        SeoFast,
        Aviso,
        Profitcentr,
        WmrFast,
        WebofSar,
        Losena
    }
    enum TypeMessage
    {
        CaptchaImage,
        Captcha,
        Error,
        Info,
        Result
    }
    class TCPMessage
    {
        public string Text { get; set; }
        public byte[] Data { get; set; }
        [JsonConstructor]
        public TCPMessage()
        {

        }
        public TCPMessage(string text, TypeMessage type, EnumTypeSite typeSite)
        {
            Type = type;
            Text = text;
            Site = typeSite;
        }
        public TypeMessage Type { get; set; }
        public EnumTypeSite Site { get; set; }
    }
    class EventArgTCPClient : EventArgs
    {
        public TCPMessage Message { get; set; }
        public EventArgTCPClient(TCPMessage message)
        {
            Message = message;
        }
        public Image GetImage()
        {
            using (MemoryStream ms = new MemoryStream(Message.Data))
            {
                return Image.FromStream(ms);
            }
        }
        public override string ToString()
        {
            return
                "Type message: " + Message.Type.ToString() + Environment.NewLine +
                "Name site: " + Message.Site.ToString() + Environment.NewLine +
                "Text: " + Message.Text + Environment.NewLine +
                "File: " + (Message.Data.Length > 0 ? "Yes" : "No");
        }
    }
    class TCPControl : IDisposable
    {
        public event EventHandler<EventArgTCPClient> MessageReceived;
        TcpListener Listener;
        public TCPControl(IPEndPoint ipEndPoint)
        {
            Listener = new TcpListener(ipEndPoint);
            Listener.Start();
        }
        public async void StartListing()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    ClientThread(Listener.AcceptTcpClient());
                }
            });
        }
        private async void ClientThread(TcpClient client)
        {
            await Task.Run(() =>
            {
                var stream = client.GetStream();
                string message = string.Empty;
                do
                {
                    byte buffer = (byte)stream.ReadByte();
                    if (buffer == 0)
                        break;
                    message += (char)buffer;
                }
                while (true);
                Console.WriteLine(message);
                TCPMessage? m = JsonSerializer.Deserialize<TCPMessage>(message);
                if (m == null)
                    Console.WriteLine("Error serelize");
                else
                    MessageReceived?.Invoke(this, new EventArgTCPClient(m));
                client.Close();
            });
        }

        public void Dispose()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }
        private void SendTCPMesage(TCPMessage message, EnumIPManager enumIP)
        {
            TcpClient ControlServer = new TcpClient();
            ControlServer.Connect(IPManager.GetEndPoint(enumIP));
            ControlServer.GetStream().Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize<TCPMessage>(message) + "\0"));
            ControlServer.Close();
        }
        public void SendResultMessage(string text, EnumTypeSite site)
        {
            TCPMessage message = new TCPMessage(text, TypeMessage.Result, site);
            SendTCPMesage(message, EnumIPManager.ClickMashine);
        }
        public void SendMessage(string text, EnumIPManager enumIP)
        {
            TCPMessage message = new TCPMessage(text, TypeMessage.Result, EnumTypeSite.None);
            SendTCPMesage(message, enumIP);
        }
    }
}