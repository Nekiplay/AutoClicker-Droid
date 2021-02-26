using Gma.System.MouseKeyHook;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_v3
{
    public partial class Form1 : Form
    {
        LMB lmbhoker = new LMB();
        RMB rmbhoker = new RMB();

        private static IKeyboardMouseEvents m_GlobalHook;
        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lmbhoker.Start();
            rmbhoker.Start();

            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += (sender1, args1) =>
            {
                if (args1.KeyCode == lmbhoker.Key)
                {
                    lmbhoker.Enable = true;
                }
                else if (args1.KeyCode == rmbhoker.Key)
                {
                    rmbhoker.Enable = true;
                }
            };

            m_GlobalHook.KeyUp += (sender2, args2) =>
            {
                if (args2.KeyCode == lmbhoker.Key)
                {
                    lmbhoker.Enable = false;
                }
                else if (args2.KeyCode == rmbhoker.Key)
                {
                    rmbhoker.Enable = false;
                }
            };

            m_GlobalHook.MouseDown += (sender3, args3) =>
            {
                if (args3.Button == lmbhoker.mouseButton)
                {
                    lmbhoker.Enable = true;
                }
                else if (args3.Button == rmbhoker.mouseButton)
                {
                    rmbhoker.Enable = true;
                }
            };

            m_GlobalHook.MouseUp += (sender4, args4) =>
            {
                if (args4.Button == lmbhoker.mouseButton)
                {
                    lmbhoker.Enable = false;
                }
                else if (args4.Button == rmbhoker.mouseButton)
                {
                    rmbhoker.Enable = false;
                }
            };
        }

        public static string GetLocalIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        public static int listenPort = 11011;
        public static UdpSocketReceiver receiver = new UdpSocketReceiver();
        private async void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = ("Connection IP: " + GetLocalIPAddress());

            this.Size = new Size(GetLocalIPAddress().Length * 15, this.Size.Height);

            receiver.MessageReceived += (sender2, args) =>
            {
                // get the remote endpoint details and convert the received data into a string
                var from = String.Format("{0}:{1}", args.RemoteAddress, args.RemotePort);
                var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);

                Console.WriteLine("{0} - {1}", from, data);
                if (data.Contains("579983761217652651027566527281106119116121795555118477772565261"))
                {
                    try
                    {
                        string cps = Regex.Match(data, "579983761217652651027566527281106119116121795555118477772565261: (.*)").Groups[1].Value;
                        lmbhoker.CPS = int.Parse(cps);
                    } catch { }
                }
                else if (data.Contains("736965115901108952976985557874120116989766661061075174109535261"))
                {
                    try
                    {
                        string key = Regex.Match(data, "736965115901108952976985557874120116989766661061075174109535261: (.*)").Groups[1].Value;
                        try
                        {
                            Enum.TryParse(key, out Keys kk);
                            lmbhoker.Key = kk;
                        }
                        catch { }
                        try
                        {
                            Enum.TryParse(key, out MouseButtons mouse);
                            lmbhoker.mouseButton = mouse;
                        } catch { }
                    }
                    catch { }
                }
                else if (data.Contains("1054755671211126810187758965748765748871116718751651091171036961"))
                {
                    try
                    {
                        string cps = Regex.Match(data, "1054755671211126810187758965748765748871116718751651091171036961: (.*)").Groups[1].Value;
                        rmbhoker.CPS = int.Parse(cps);
                    }
                    catch { }
                }
                else if (data.Contains("11970121539754847653877680668454526969494990791021041161058561"))
                {
                    try
                    {
                        string key = Regex.Match(data, "11970121539754847653877680668454526969494990791021041161058561: (.*)").Groups[1].Value;
                        try
                        {
                            Enum.TryParse(key, out Keys kk);
                            rmbhoker.Key = kk;
                        } catch { }
                        try
                        {
                            Enum.TryParse(key, out MouseButtons mouse);
                            rmbhoker.mouseButton = mouse;
                        } catch { }
                    }
                    catch { }
                }
                else if (data == "78781055068437484771181015310489881117611085431144965115801086561")
                {
                    this.Invoke((MethodInvoker)(() => this.Hide()));
                }
                else if (data == "1174711251801081121061121011128912198891031021096777111667379878311161")
                {
                    Process.GetCurrentProcess().Kill();
                }
                else if (data == "99905410610811350835266116728643119114107551151018812199801001204861")
                {
                    Clear.Cleaner cleaner = new Clear.Cleaner();
                    new Thread(() =>
                    {
                        cleaner.Clear_LastActivity();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.AppdataLocalTemp();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.Downloads();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.WindowsTemp();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.Windows_Prefetch();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.Windows_Recent();
                    }).Start();
                    new Thread(() =>
                    {
                        cleaner.Resycle_Bin();
                    }).Start();
                }
            };

            await receiver.StartListeningAsync(listenPort);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
