using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Runtime.Serialization;

namespace devJace
{
    /*
     * 2022.10.18 ::: 정명재 
     * Program.cs
     * 
     * 
     * 
     * 
     * 
     */


    static class Program
    {

        //static bool IsDebug = true;


        //=========================================================================
        // 콘솔창 관련 시작
        //=========================================================================
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();              //콘솔할당하기위해...

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();               //할당된 콘솔제거하기위해...

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);  //콘솔을 연결하기위해...

        [DllImport("user32.dll", SetLastError = true)]  //프로세스ID를 알기위해...
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll", SetLastError = true)]      //현재활성화된 창을 구하기 위해...
        static extern IntPtr GetForegroundWindow();

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_CLOSE = 0xF060;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        public static void VisibleConsole(bool visible)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, visible ? SW_SHOW : SW_HIDE);
        }
        public static bool IsWindowVisible()
        {
            var handle = GetConsoleWindow();
            return IsWindowVisible(handle);
        }
        //=========================================================================
        // 콘솔창 관련 종료
        //=========================================================================


        //=========================================================================
        // 모니터 해상도 관련 시작
        //=========================================================================

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        public enum DeviceCap
        {
            VERTRES = 10,
            HORZRES = 11,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
        }

        public static float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int LogicalScreenWeight = GetDeviceCaps(desktop, (int)DeviceCap.HORZRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);
            int PhysicalScreenWeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPHORZRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            //float ScreenScalingFactor = (float)LogicalScreenHeight / (float)PhysicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

        [DllImport("Shcore.dll")]
        public static extern int SetProcessDpiAwareness(int processDpiAwareness);
        /// <summary>
        /// According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        /// </summary>
        public enum AWARENESS
        {
            //None = 0,
            //SystemAware = 1,
            //PerMonitorAware = 2

            INVALID = -1,
            UNAWARE = 0,
            SYSTEM_AWARE = 1,
            PER_MONITOR_AWARE = 2
        }
        //=========================================================================
        // 모니터 해상도 관련 종료
        //=========================================================================

        public static Project_Main.FormMain m_MainUI;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ChartDirector.Chart.setLicenseCode("DEVP-2M4Y-J2K9-HZ5F-75E7-197C");

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            //Operating System
            OperatingSystem os = System.Environment.OSVersion;
            //if(System.Environment.Is64BitOperatingSystem)
            Console.WriteLine("Platform :" + os.Platform);
            Console.WriteLine("ServicePack :" + os.ServicePack);
            Console.WriteLine("Version :" + os.Version);
            Console.WriteLine("VersionString :" + os.VersionString);
            Console.WriteLine("CLR Version :" + System.Environment.OSVersion);
            var vers = getWindowVersionInWin32NT(os.Version);

            Assembly assembly = Assembly.GetExecutingAssembly();

            //string resourceName = $"{typeof(Program).Namespace}.NLog.config";
            //string resourceName = $"Kyuchon_Robot.NLog.config";
            //string resourceName = $"NLog.config";
            string resourceName = $"ChefBot.NLog.config";
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(reader, null);
                }
            }

            //Monitor Pixel
            if (vers == "Windows 8")
            {
                //Dpi.SetProcessDpiAwareness((int)Dpi.DpiAwareness.None);
                //SetProcessDpiAwareness((int)AWARENESS.PER_MONITOR_AWARE);
                SetProcessDpiAwareness((int)AWARENESS.UNAWARE);
                //SetProcessDpiAwareness((int)AWARENESS.SYSTEM_AWARE);
                //SetProcessDpiAwareness((int)AWARENESS.PER_MONITOR_AWARE);
            }

            //Application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Project_Main.FormMain());

            bool State;
            Mutex mutex = new Mutex(true, "GONGMAN", out State);

            if (State)
            {
                //2023.01.16
                //On_Rockey();

                //DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                //2022.03.22
                //UsrDisplay.SetProcessDpiAwareness((int)UsrDisplay.DpiAwareness.None);
                m_MainUI = new Project_Main.FormMain();
                Application.Run(m_MainUI);
            }
            else
            {
                MessageBox.Show("This Program is already running");
            }
        }
        

        /// <summary>
        /// 서브 폼 설정
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="IsParents"></param>
        public static void Performance(this Form frm, bool IsParents)
        {
            if (frm == null)
                return;
            if (IsParents == false)
            {
                frm.TopLevel = IsParents;
                frm.Size = frm.PreferredSize;
                frm.Dock = DockStyle.Fill;
                frm.MaximizeBox = false;
                frm.Text = null;
            }

            frm.AutoScaleMode = AutoScaleMode.None;

            const bool IsUserFontUse = true;
            const float fontDepth = 9;
            const int fontIndex = (int)Fonts.FontLibrary.ENotoSans.Normal;

            //Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | IsUserFontUse {IsUserFontUse} ");

            if (IsUserFontUse)
            {
                frm.Font = new Font(Fonts.FontLibrary.Families[fontIndex], fontDepth);                
            }

            //Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | frmName {frm.Name} ");
            //Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | frmSize {frm.Size} ");

            Control[] controls = GetAllControlsUsingRecursive(frm);
            foreach (Control control in controls)
            {
                DoubleBuffered(control);
                if (IsUserFontUse)
                {
                    control.Font = new Font(Fonts.FontLibrary.Families[fontIndex], fontDepth);
                }
                //Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | ConName {control.Name} ");
                //Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | conSize {control.Size} ");
            }
        }

        private static Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();
            foreach (Control control in containerControl.Controls)
            {
                allControls.Add(control);
                if (control.Controls.Count > 0)
                {
                    allControls.AddRange(GetAllControlsUsingRecursive(control));
                }

            }
            return allControls.ToArray();

        }

        private static void DoubleBuffered(this Control control)
        {
            Type dgvType = control.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, true, null);
        }

        public static string getWindowVersion()
        {
            OperatingSystem os = Environment.OSVersion;

            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                    return getWindowVersionInWin32NT(os.Version);
                default:
                    return "UnKnown or not more than Window 98";
            }
        }

        private static string getWindowVersionInWin32NT(Version version)
        {
            switch (version.Major)
            {
                case 4:
                    return "Windows NT 4.0";
                case 5:
                    if (version.Minor == 0)
                        return "Windows 2000";
                    else if (version.Minor == 1)
                        return "Windows XP";
                    else
                        return "Windows 2003";
                case 6:
                    if (version.Minor == 0)
                        return "Windows Vista";
                    else if (version.Minor == 1)
                        return "Windows 7";
                    else
                        return "Windows 8";
                case 10:
                    if (version.Minor == 0)
                        return "Windows 10";
                    else
                        return "Windows 10";
                default:
                    return "UnKnown Version";
            }
        }

        public static void FormKeyEventAbort(KeyEventArgs e)
        {
            //단축키 폼 종료 무시
            if (e.Alt && e.KeyCode == Keys.F4)
                e.Handled = true;
        }

        //static Logger logger = LogManager.GetCurrentClassLogger();
        static int logIndex = 0;
        static string logString = null;
        static ELogLevel eLogLevel;
        public static void LogSave(NLog.Logger logger, ELogLevel logLevel, string msg)
        {
            try
            {
                if (eLogLevel != logLevel)
                {
                    eLogLevel = logLevel;
                    logIndex = 0;
                }
                if (logString != msg)
                {
                    logString = msg;
                    logIndex = 0;
                }

                //switch (logLevel)
                //{
                //    case ELogLevel.Debug:
                //        logger.Debug($"[{logIndex++}] {msg} |");
                //        break;
                //
                //    case ELogLevel.Info:
                //        logger.Info($"[{logIndex++}] {msg} |");
                //        break;
                //
                //    case ELogLevel.Warn:
                //        logger.Warn($"[{logIndex++}] {msg} |");
                //        break;
                //
                //    case ELogLevel.Error:
                //        logger.Error($"[{logIndex++}] {msg} |");
                //        break;
                //
                //    case ELogLevel.Fatal:
                //        logger.Fatal($"[{logIndex++}] {msg} |");
                //        break;
                //}


                switch (logLevel)
                {
                    case ELogLevel.Debug:
                        logger.Debug($"{msg} |");
                        break;

                    case ELogLevel.Info:
                        logger.Info($"{msg} |");
                        break;

                    case ELogLevel.Warn:
                        logger.Warn($"{msg} |");
                        break;

                    case ELogLevel.Error:
                        logger.Error($"{msg} |");
                        break;

                    case ELogLevel.Fatal:
                        logger.Fatal($"{msg} |");
                        break;
                }

            }
            catch
            {

            }
        }

        public enum ELogLevel
        {
            Debug, Info, Warn, Error, Fatal
        }
    }
   
}
