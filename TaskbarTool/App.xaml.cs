using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TaskbarTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly Mutex _mutex = new Mutex(true, "{6BBDC1F2-F66E-4E3E-AC96-E42C4C19FFE4}");
        public App()
        {
            if (!_mutex.WaitOne(TimeSpan.Zero, true))
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other <a href="https://dxtol.com/category/it/windows/">windows</a>
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                this.Shutdown(0);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex.ReleaseMutex();
            base.OnExit(e);
        }
    }

    class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}
