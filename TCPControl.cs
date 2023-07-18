using System;
using System.Collections.Generic;
using System.Data;
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
    public enum EnumTypeSite
    {
        None,
        Router,
        SeoFast,
        Aviso,
        Profitcentr,
        WmrFast,
        WebofSar,
        Losena,
        SeoClub,
        VipClick
    }
    public enum TypeMessage
    {
        CaptchaImage,
        Captcha,
        Error,
        Info,
        Result
    }
    public class TCPMessage
    {
        public string? Text { get; set; }
        public byte[]? Data { get; set; }
        [JsonConstructor]
        public TCPMessage()
        {

        }
        public TCPMessage(string text, int IDMashine, TypeMessage type, EnumTypeSite typeSite)
        {
            this.IDMashine = IDMashine;
            Type = type;
            Text = text;
            Site = typeSite;
        }
        public TypeMessage Type { get; set; }
        public EnumTypeSite Site { get; set; }
        public int IDMashine { get; set; }
    }
    public static class IPManager
    {
        public static IPEndPoint GetEndPoint(MySQL mySQL, int IDObject)
        {
            using (DataTable dt = mySQL.GetDataTableSQL("SELECT ip, port FROM object WHERE id = " + IDObject.ToString()))
            {
                if (dt.Rows.Count > 0)
                {
                    return new IPEndPoint(IPAddress.Parse(dt.Rows[0]["ip"].ToString()),
                        int.Parse(dt.Rows[0]["port"].ToString()));
                }
                else return null;
            }
        }
    }
    public class EventArgTCPClient : EventArgs
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
                "IDMashine: " + Message.IDMashine.ToString() + Environment.NewLine +
                "Name site: " + Message.Site.ToString() + Environment.NewLine +
                "Text: " + Message.Text + Environment.NewLine +
                "File: " + (Message.Data.Length > 0 ? "Yes" : "No");
        }
    }
    class TCPControl
    {
        public event EventHandler<EventArgTCPClient> MessageReceived;
        TcpListener Listener;
        MySQL MySQL;
        public const int Port = 50001;
        public TCPControl(IPAddress iPAddress)
        {
            MySQL = new MySQL("clicker");
            Listener = new TcpListener(iPAddress,Port);
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
                try
                {
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                client.Close();
            });
        }
        ~TCPControl()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }
        private void SendTCPMesage(TCPMessage message)
        {
            TcpClient ControlServer = new TcpClient();
            try
            {
                ControlServer.Connect(IPManager.GetEndPoint(MySQL, message.IDMashine));
                ControlServer.GetStream().Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize<TCPMessage>(message) + "\0"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ControlServer.Close();
        }
        public void SendResultMessage(string text, int IDMashine, EnumTypeSite site)
        {
            TCPMessage message = new TCPMessage(text, IDMashine, TypeMessage.Result, site);
            SendTCPMesage(message);
        }
        public void SendMessage(string text, int IDMashine)
        {
            TCPMessage message = new TCPMessage(text, IDMashine, TypeMessage.Result, EnumTypeSite.None);
            SendTCPMesage(message);
        }
    }
}