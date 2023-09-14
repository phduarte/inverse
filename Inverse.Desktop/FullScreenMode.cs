using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    internal class FullScreenMode
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        const int SW_HIDE = 0;
        const int SW_SHOW = 1;

        public static void Toggle(Form form, bool fullscreenActivated)
        {
            if (fullscreenActivated)
            {
                //form.TopMost = true;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Height = form.Height + HideTrayBar();
                form.WindowState = FormWindowState.Maximized;
            }
            else
            {
                //form.TopMost = false;
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.WindowState = FormWindowState.Maximized;
                form.Height = form.Height - ShowTraybar();
            }
        }

        private static int HideTrayBar()
        {
            try
            {
                IntPtr tWnd = IntPtr.Zero;
                IntPtr bWnd = IntPtr.Zero;
                tWnd = FindWindow("Shell_TrayWnd", null);
                bWnd = FindWindowEx(tWnd, IntPtr.Zero, "BUTTON", null);
                ShowWindow(tWnd, SW_HIDE);
                ShowWindow(bWnd, SW_HIDE);

                if (GetWindowRect(tWnd, out RECT rect))
                {
                    return rect.Bottom - rect.Top;
                }

                return 0;
            }
            catch (Exception ex)
            {
                // Tratar erro ao ocultar a barra de tarefas aqui
                return 0;
            }
        }

        private static int ShowTraybar()
        {
            try
            {
                IntPtr tWnd = IntPtr.Zero;
                IntPtr bWnd = IntPtr.Zero;
                tWnd = FindWindow("Shell_TrayWnd", null);
                bWnd = FindWindowEx(tWnd, IntPtr.Zero, "BUTTON", null);
                ShowWindow(bWnd, SW_SHOW);
                ShowWindow(tWnd, SW_SHOW);

                if (GetWindowRect(tWnd, out RECT rect))
                {
                    return rect.Bottom - rect.Top;
                }

                return 0;
            }
            catch (Exception ex)
            {
                // Tratar erro ao mostrar a barra de tarefas aqui
                return 0;
            }
        }
    }
}
