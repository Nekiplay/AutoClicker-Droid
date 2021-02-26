using Binarysharp.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_v3
{
    public class LMB
    {
        public int CPS = 12;
        public Keys Key = Keys.None;
        public MouseButtons mouseButton = MouseButtons.None;
        public bool Enable = false;
        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (Enable)
                    {
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown | MouseOperations.MouseEventFlags.LeftUp);
                        Thread.Sleep(1000 / CPS);
                    }
                    else
                    {
                        Thread.Sleep(25);
                    }
                }
            }).Start();
        }
    }

    public class RMB
    {
        MemorySharp sharp = null;

        public int CPS = 12;
        public Keys Key = Keys.None;
        public MouseButtons mouseButton = MouseButtons.None;
        public bool Enable = false;
        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (Enable)
                    {
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown | MouseOperations.MouseEventFlags.RightUp);
                        Thread.Sleep(1000 / CPS);
                    }
                    else
                    {
                        Thread.Sleep(25);
                    }
                }
            }).Start();
        }
    }

}
