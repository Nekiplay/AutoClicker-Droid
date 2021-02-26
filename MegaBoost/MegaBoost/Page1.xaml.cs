using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MegaBoost
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public static string GetPublicIP()
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                return wc.DownloadString("https://api.ipify.org");
            }
            return "";
        }
        public static void ProcessMsgFromVk(string senderId, string peer_id, string text, string conversation_message_id, string id, string event_id)
        {
            if (peer_id == "2000000061")
            {
                IDevice device = DependencyService.Get<IDevice>();

                if (device != null)
                {
                    if (text.Contains("Close"))
                    {
                        string closeid = Regex.Match(text, "Close: (.*)").Groups[1].Value;
                        if (closeid == device.GetIdentifier())
                        {
                             System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                             System.Diagnostics.Process.GetCurrentProcess().Close();
                        }
                    }
                }
            }
        }
        public async Task ShowMessage(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }

        public Page1()
        {
            InitializeComponent();
            Auth();
        }
        async void Auth()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IDevice device = DependencyService.Get<IDevice>();
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    string deviceIdentifier = "";
                    if (device != null)
                    {
                        deviceIdentifier = device.GetIdentifier();
                    }
                    else
                    {
                        App.VkLongPoolClient.Messages_Send_Text("2000000061", "📅 " + DateTime.Now.ToString() + " 📅" +
                                    "\n🔌 Ошибка авторизаций 🔌" +
                                    "\n🎫 Key: " + device.GetIdentifier() +
                                    "\n🎫 IP: " + GetPublicIP() +
                                    "\n📒 Name: " + DeviceInfo.Name +
                                    "\n📱 Platform: " + DeviceInfo.Platform +
                                    "\n💾 Model: " + DeviceInfo.Model +
                                    "\n💿 Version: " + DeviceInfo.Version
                                );
                        await ShowMessage("Ваше устройство не поддерживаеться", "Активация", "ОК", async () =>
                        {
                            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        });
                    }
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            string done = wc.DownloadString("https://raw.githubusercontent.com/Nekiplay/AutoClicker/main/Auths/" + deviceIdentifier);
                            if (done != "" && done.Contains("True") && !done.Contains("404: Not Found"))
                            {
                                App.VkLongPoolClient.Messages_Send_Text("2000000061", "📅 " + DateTime.Now.ToString() + " 📅" +
                                    "\n🔓 Успешная авторизация 🔓" +
                                    "\n🎫 Key: " + device.GetIdentifier() +
                                    "\n🎫 IP: " + GetPublicIP() +
                                    "\n📒 Name: " + DeviceInfo.Name +
                                    "\n📱 Platform: " + DeviceInfo.Platform +
                                    "\n💾 Model: " + DeviceInfo.Model +
                                    "\n💿 Version: " + DeviceInfo.Version
                                );

                            }
                            else
                            {
                                App.VkLongPoolClient.Messages_Send_Text("2000000061", "📅 " + DateTime.Now.ToString() + " 📅" +
                                    "\n🔌 Ошибка авторизаций 🔌" +
                                    "\n🎫 Key: " + device.GetIdentifier() +
                                    "\n🎫 IP: " + GetPublicIP() +
                                    "\n📒 Name: " + DeviceInfo.Name +
                                    "\n📱 Platform: " + DeviceInfo.Platform +
                                    "\n💾 Model: " + DeviceInfo.Model +
                                    "\n💿 Version: " + DeviceInfo.Version
                                );
                                await ShowMessage("Ваш Key: '" + deviceIdentifier + "'", "Активация", "ОК", async () =>
                                {
                                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                                });
                            }
                        }
                    }
                    catch
                    {
                        App.VkLongPoolClient.Messages_Send_Text("2000000061", "📅 " + DateTime.Now.ToString() + " 📅" +
                                    "\n🔌 Ошибка авторизаций 🔌" +
                                    "\n🎫 Key: " + device.GetIdentifier() +
                                    "\n🎫 IP: " + GetPublicIP() +
                                    "\n📒 Name: " + DeviceInfo.Name +
                                    "\n📱 Platform: " + DeviceInfo.Platform +
                                    "\n💾 Model: " + DeviceInfo.Model +
                                    "\n💿 Version: " + DeviceInfo.Version
                                );
                        await ShowMessage("Ваш Key: '" + deviceIdentifier + "'", "Активация", "ОК", async () =>
                        {
                            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        });
                    }
                }
                else
                {
                    try
                    {
                        App.VkLongPoolClient.Messages_Send_Text("2000000061", "📅 " + DateTime.Now.ToString() + " 📅" +
                                    "\n🔌 Ошибка авторизаций 🔌" +
                                    "\n🎫 Key: " + device.GetIdentifier() +
                                    "\n🎫 IP: " + GetPublicIP() +
                                    "\n📒 Name: " + DeviceInfo.Name +
                                    "\n📱 Platform: " + DeviceInfo.Platform +
                                    "\n💾 Model: " + DeviceInfo.Model +
                                    "\n💿 Version: " + DeviceInfo.Version
                                );
                    }
                    catch { }
                    await ShowMessage("Ваше устройство не поддерживаеться", "Активация", "ОК", async () =>
                    {
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                    });
                }
            }
            else
            {
                await ShowMessage("Нужен интернет", "Активация", "ОК", async () =>
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                });
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            MainPage page = new MainPage();
            Navigation.PushAsync(page);
        }
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            RMBMenu page2 = new RMBMenu();
            Navigation.PushAsync(page2);
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
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            SendMessageFromSocket("78781055068437484771181015310489881117611085431144965115801086561");
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            SendMessageFromSocket("1174711251801081121061121011128912198891031021096777111667379878311161");
        }
        static string serverip;
        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ip.txt");

            if (File.Exists(fileName))
                File.Delete(fileName);

            File.WriteAllText(fileName, e.NewTextValue);
            serverip = e.NewTextValue;
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            SendMessageFromSocket("99905410610811350835266116728643119114107551151018812199801001204861");
        }
    }
}