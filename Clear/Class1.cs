using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clear
{
    public class Cleaner
    {
        public void WindowsTemp()
        {
            if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%windir%\Temp")))
            {
                string tempPath = Environment.ExpandEnvironmentVariables(@"%windir%\Temp");
                string[] fileNames = System.IO.Directory.GetFiles(tempPath);
                foreach (string fileName in fileNames)
                {
                    try
                    {
                        System.IO.File.Delete(fileName);
                    }
                    catch { }
                }
                foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch { }
                }
            }
        }
        public void AppdataLocalTemp()
        {
            try
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Temp", true);
            }
            catch { }
            string tempPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Temp";
            string[] fileNames2 = System.IO.Directory.GetFiles(tempPath2);
            foreach (string fileName in fileNames2)
            {
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch { }
            }
            foreach (DirectoryInfo dir in new DirectoryInfo(tempPath2).GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }
        }
        public void Downloads()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string[] fileNames = System.IO.Directory.GetFiles(tempPath);
            string[] DirectoryNames = System.IO.Directory.GetDirectories(tempPath);
            foreach (string fileName in fileNames)
            {
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch { }
            }
            foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }
        }
        public void Windows_Prefetch(string contains = "ALL")
        {
            if (contains != "ALL")
            {
                string tempPath = @"C:\Windows\Prefetch";
                string[] fileNames = System.IO.Directory.GetFiles(tempPath);
                foreach (string fileName in fileNames)
                {
                    if (fileName.Contains(contains))
                    try { System.IO.File.Delete(fileName); } catch { }
                }
                foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
                {
                    if (dir.Name.Contains(contains))
                        try { dir.Delete(true); } catch { }
                }
            }
            else
            {
                string tempPath = @"C:\Windows\Prefetch";
                string[] fileNames = System.IO.Directory.GetFiles(tempPath);
                foreach (string fileName in fileNames)
                {
                    try { System.IO.File.Delete(fileName); }
                    catch { }
                }
                foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
                {
                    try { dir.Delete(true); }
                    catch { }
                }
            }
        }
        public void Resycle_Bin()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOSOUND | RecycleFlags.SHERB_NOCONFIRMATION);
        }
        public void Windows_Recent(string contains = "ALL")
        {
            if (contains != "ALL")
            {
                try
                {
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Microsoft\Windows\Recent";
                    string[] fileNames = System.IO.Directory.GetFiles(tempPath);
                    string[] DirectoryNames = System.IO.Directory.GetDirectories(tempPath);
                    foreach (string fileName in fileNames)
                    {
                        if (fileName.Contains(contains))
                        try { System.IO.File.Delete(fileName); } catch { }
                    }
                    foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
                    {
                        if (dir.Name.Contains(contains))
                        try { dir.Delete(true); } catch { }
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Microsoft\Windows\Recent";
                    string[] fileNames = System.IO.Directory.GetFiles(tempPath);
                    string[] DirectoryNames = System.IO.Directory.GetDirectories(tempPath);
                    foreach (string fileName in fileNames)
                    {
                        try
                        {
                            System.IO.File.Delete(fileName);
                        }
                        catch { }
                    }
                    foreach (DirectoryInfo dir in new DirectoryInfo(tempPath).GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }
        
        public void Clear_LastActivity(string contains = "ALL")
        {
            Crash_Logs();
            try
            {
                GrandAccesKey(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bam\State\UserSettings");
                string[] names = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\bam\State\UserSettings", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Console.WriteLine(name);
                    Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\bam\State\UserSettings", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                string[] names = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\UserAssist", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Console.WriteLine(name);
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\UserAssist", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                string[] names = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU", true).DeleteSubKeyTree(name);
                }
            } catch { }
            try
            {
                string[] names = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                string[] names = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunMRU", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunMRU", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                string[] names = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.CurrentUser.OpenSubKey(@"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                GrandAccesKey(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Uninstall");
                string[] names = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
            try
            {
                GrandAccesKey(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Uninstall");
                string[] names = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true).GetSubKeyNames();
                foreach (string name in names)
                {
                    Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true).DeleteSubKeyTree(name);
                }
            }
            catch { }
        }
        private void Start_CMD(string cmd)
        {
            ProcessStartInfo p = new ProcessStartInfo("cmd.exe");
            p.CreateNoWindow = true;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.Arguments = cmd;
            Process.Start(p);
        }
        public void Crash_Logs()
        {
            new Thread(() =>
            {
                Start_CMD("schtasks.exe /Delete /TN * /F");
            }).Start();
            new Thread(() =>
            {
                Start_CMD("for /f %%a in ('WEVTUTIL EL') do WEVTUTIL CL \"%%a\"");
            }).Start();
        }
        private static void GrandAccesKey(string key)
        {
            ProcessStartInfo p = new ProcessStartInfo("cmd.exe");
            p.CreateNoWindow = true;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            if (GetOSBit() == "x64")
                p.Arguments = "/k SetACL64.exe -on \"" + key + "\" -ot reg -actn ace -ace \"n:Администраторы;p:full\"";
            else
                p.Arguments = "/k SetACL32.exe -on \"" + key + "\" -ot reg -actn ace -ace \"n:Администраторы;p:full\"";
            Process.Start(p);
            foreach (Process process in Process.GetProcessesByName("cmd")) { process.Kill(); }
        }
        public static string GetOSBit()
        {
            bool is64bit = Is64Bit();
            if (is64bit)
                return "x64";
            else
                return "x32";
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public static bool Is64Bit()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);
            return retVal;
        }

        enum RecycleFlags : int
        {
            // No confirmation dialog when emptying the recycle bin
            SHERB_NOCONFIRMATION = 0x00000001,
            // No progress tracking window during the emptying of the recycle bin
            SHERB_NOPROGRESSUI = 0x00000001,
            // No sound whent the emptying of the recycle bin is complete
            SHERB_NOSOUND = 0x00000004
        }
        // Shell32.dll is where SHEmptyRecycleBin is located
        [DllImport("Shell32.dll")]
        // The signature of SHEmptyRecycleBin (located in Shell32.dll)
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
    }
}
