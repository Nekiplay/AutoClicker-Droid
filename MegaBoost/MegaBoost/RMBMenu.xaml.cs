using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MegaBoost
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RMBMenu : ContentPage
    {
        public static string serverip;
        public RMBMenu()
        {
            InitializeComponent();

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ip.txt");
            if (File.Exists(fileName))
            {
                string nickname = File.ReadAllText(fileName);
                if (nickname != string.Empty)
                {
                    serverip = nickname;
                }
            }


            string fileName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RMBcps.txt");
            if (File.Exists(fileName1))
            {
                string nickname = File.ReadAllText(fileName1);
                if (nickname != string.Empty)
                {
                    CPSLMBEditor.Text = nickname;
                }
            }

            string fileName2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RMBkey.txt");
            if (File.Exists(fileName2))
            {
                string nickname = File.ReadAllText(fileName2);
                if (nickname != string.Empty)
                {
                    KeyLMBEditor.Text = nickname;
                }
            }
        }
        static async void SendMessageFromSocket(string message)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var port = 11011;
                var address = serverip;
                UdpSocketClient client = new UdpSocketClient();
                // convert our greeting message into a byte array
                var msgBytes = Encoding.UTF8.GetBytes(message);

                // send to address:port, 
                // no guarantee that anyone is there 
                // or that the message is delivered.
                await client.SendToAsync(msgBytes, address, port);
                await client.DisconnectAsync();
                client.Dispose();
            }
            else
            {

            }
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            SendMessageFromSocket("1054755671211126810187758965748765748871116718751651091171036961: " + CPSLMBEditor.Text);
            SendMessageFromSocket("11970121539754847653877680668454526969494990791021041161058561: " + KeyLMBEditor.Text);

            string fileName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RMBcps.txt");

            if (File.Exists(fileName1))
                File.Delete(fileName1);

            File.WriteAllText(fileName1, CPSLMBEditor.Text);

            string fileName2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RMBkey.txt");

            if (File.Exists(fileName2))
                File.Delete(fileName2);

            File.WriteAllText(fileName2, KeyLMBEditor.Text);
        }
    }
}