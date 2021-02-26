using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MegaBoost
{
    public partial class MainPage : ContentPage
    {
        public static string serverip;
        public MainPage()
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

            string fileName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LMBcps.txt");
            if (File.Exists(fileName1))
            {
                string nickname = File.ReadAllText(fileName1);
                if (nickname != string.Empty)
                {
                    CPSLMBEditor.Text = nickname;
                }
            }

            string fileName2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LMBkey.txt");
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

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            SendMessageFromSocket("579983761217652651027566527281106119116121795555118477772565261: " + CPSLMBEditor.Text);
            SendMessageFromSocket("736965115901108952976985557874120116989766661061075174109535261: " + KeyLMBEditor.Text);

            string fileName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LMBcps.txt");

            if (File.Exists(fileName1))
                File.Delete(fileName1);

            File.WriteAllText(fileName1, CPSLMBEditor.Text);

            string fileName2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LMBkey.txt");

            if (File.Exists(fileName2))
                File.Delete(fileName2);

            File.WriteAllText(fileName2, KeyLMBEditor.Text);

        }
    }
}
