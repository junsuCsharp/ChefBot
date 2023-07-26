using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using System.Threading;
using Forms;
using System.Windows.Media;
using Cores;

using System.Media;
using WMPLib;
using static Cores.Core_StepModule;

using System.Text.RegularExpressions;
using ChefBot;
using static Externs.Robot_Modbus_Table;
using Externs;
using PackML;
using System.Collections.Concurrent;
using devJace.Files;
using System.Reflection;


namespace Project_Main
{
    /*
     * 2022.12.29
     * 1366 x 768, 1920 x 1080
     * 
     * 2023.02.17 ::: 1920 x 1080
     * 
     * Worker_Elapsed 1ms
     * Work_TickTime 1000ms //사용안함.
     * Work_DateTime 1000ms
     * FormMain_FormClosing
     * asyncPingCheck
     * 
     * 
     * 2023.03.06
     * 이슈리스트
     *   -- 보호된 메모리 엑세스 에러
     * 
     * 메트로 폼 사용하지 말 것.
     * 해상도 읽기
     * 
     * 2023.03.07
     * 알람 파일 / 알람 업데이트
     * 데이터 베이스 읽기 및 오래된 것 지우기
     * 
     * 서페이스 고 1920 x 1280
     * 
     * 자동 조건 잡기
     * 정보 화면 업데이트 하기
     * 
     * 
     * 2023.03.10 ::: 서피스고, 퍼포먼스 카운터 에러
     * 
     * 프로그램 시작 위치
     * 
     * 2023.06.14 ::: 레이져스캐너 경고창 띄우는 곳
     * Work_AlarmForm_Update
     * 
     * 
     * 
     * 
     * 
     */


    //public partial class FormMain : MetroFramework.Forms.MetroForm
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        //private PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //private PerformanceCounter ram = new PerformanceCounter("Memory", "Available MBytes");
        //string process_name = Process.GetCurrentProcess().ProcessName;
        //private PerformanceCounter prcess_cpu = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

        delegate void EventFiredDelegate();

        List<Form> _forms = new List<Form>();
        Forms.FormExcute automatics;
        Forms.FormConfigs configs;        
        Forms.FormDashBorad dashBorad;
        Forms.FormDIO formDIO;
        Forms.PageMain pageMain;
        Forms.FormExcute formExcute;
        Forms.FormDebug formDebug;
        Forms.FormXaxis formXaxis;
        Forms.FormAlarm formAlarm;
        Forms.FormInfo formInfo;
        Forms.FormAborting formLaser;
        FormTutorial frmTutorial;
        Forms.FormRobotBus formRobot;
        Forms.FormPause formPause;
        Forms.FormRobotEmc formRobotEmc;

        public Cores.Core_Object core_Object;//main process
        public Forms.FormErrorBox alramMeassage;

        List<RadioButton> rbButtons = new List<RadioButton>();        
        List<LBSoft.IndustrialCtrls.Leds.LBLed> Leds = new List<LBSoft.IndustrialCtrls.Leds.LBLed>();

        //Random r = new Random();
        //int iRcount = 0;
        //int iTickCount = 0;
        //int iReversCount = 0;

        System.Drawing.Color colorLime = System.Drawing.Color.Lime;
        System.Drawing.Color colorRed = System.Drawing.Color.Red;

        DateTime processTime = DateTime.Now;
        Thread asyncThread;

        DateTime dtcurrentTime = DateTime.Now; //12시를 기준으로 날짜가 변경 될때, 자동 데이터베이스 삭제
        DateTime nextDayTime = DateTime.Now.AddDays(1); //12시를 기준으로 날짜가 변경 될때, 자동 데이터베이스 삭제


        WindowsMediaPlayer wmp = new WindowsMediaPlayer();
        SoundPlayer player;// = new SoundPlayer($"{Application.StartupPath}\\Sound\\warning.mp3");
        int iCurrSoundPlayerNumver = 1000;
        int iPrevSoundPlayerNumver = 1000;
        int iPrevProcesStateBuffer = 0;
        int iCurrProcesStateBuffer = 0;

        PackML.ECurrState eCurrStateWatchdog = PackML.ECurrState.CurrentState_None;

        public static devJace.Files.Gui_File gui_file = new devJace.Files.Gui_File();

        //2023.04.12 ::: 레이져 스캐너 엣지 변수
        public bool IsCurrentProtectArea = true;
        public bool IsPreviousProtectArea = true;
        public bool IsCurrentWarningArea = true;
        public bool IsPreviousWarningArea = true;

        //2023.04.17 ::: 자동, 로봇 인터페이스 로그, 
        //public bool Is
        //DateTime dtUpdateTime = DateTime.Now;
        //TimeSpan tsUpdateTime = new TimeSpan();

        ConcurrentStack<int> SoundQue = new ConcurrentStack<int>();
        DateTime dateTimeSoundTime = DateTime.Now;
        TimeSpan timeSpanSoundTime = new TimeSpan();
        int iBeforeSoundPlayerNumver = 1000;

        //2023.05.01 ::: 코봇 상태 초기 메세지 구동준비
        int iCobotPrevState = -1;
        bool IsOperationReady = false;

        bool IsPauseLatch = false;
        bool IsPauseBuff = false;
        bool IsResumePolice = false;

        bool IsPauseDoubleLatch = false;

        Externs.Robot_Modbus_Table.RobotState_Ver_1_1 IsPauseDoubleLatchState = RobotState_Ver_1_1.SAFE_OFF;

        void SoundWaveEnQue(int value)
        {
            try
            {
                if (iBeforeSoundPlayerNumver != value)
                {
                    SoundQue.Push(value);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
            }
            
        }       

        enum EFrm
        {
            DashBoard, Excute, Manual, Config, DIO, Info, Alarm, Debug, Exit, Max
        }

        enum Ethernet
        {
            Cobot, Pc, X_Axis, EIO_1, EIO_2, EIO_3, EIO_4, Max
        }

        /// <summary>
        /// 폼 로딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                  $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | App Start");

            this.DoubleBuffered = true;
            this.Size = new Size(1920, 1040);
            this.Location = new Point(3, 3);

            //프로그램 시작 위치
            this.StartPosition = FormStartPosition.CenterScreen;
            if (devi.Define.IsSupervisor)
            {
                //this.Location = new Point(0, 0);

                //서피스 고 해상도 변경 후
                //this.Location = new Point(0, -3);

                //2층 사무실
                //this.Location = new Point(-1920, 3);    


              

                //노트북
                this.Location = new Point(0, 0);

                //서피스 고 해상도 변경 후
                this.Location = new Point(0, -3);

                //this.Location = new Point(3, 3);


                //2층 사무실
                this.Location = new Point(-1919, 9);
            }
            else
            {
                //서페이스 고 
                //this.Location = new Point(-1920, 198);
                //this.Location = new Point(0, 3);
                //내꺼 노트북
                //this.Location = new Point(-1920, 503);

                this.Location = new Point(3, 3);
            }

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                 $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Location : {this.Location}");



            string pathfile = $"{Application.StartupPath}\\whdmd_xui.xml";
            if (devJace.Files.Xml.Load(pathfile, ref gui_file))
            {
                for (int idx = 0; idx < gui_file.type.Length; idx++)
                {
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //$"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //$" | xui : {gui_file.iCookIndex[idx]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | xui name : {gui_file.strName[idx]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | xui type : {gui_file.type[idx]}");
                }
            }
            else
            {
                gui_file.strName[0] = "Load A";
                gui_file.type[0] = devJace.Files.ModuleType.Load;
                gui_file.strName[1] = "Load B";
                gui_file.type[1] = devJace.Files.ModuleType.Load;

                gui_file.strName[2] = "UnLoad C";
                gui_file.type[2] = devJace.Files.ModuleType.UnLoad;

                gui_file.strName[3] = "Cooker 1-1";
                gui_file.strName[4] = "Cooker 1-2";
                gui_file.strName[5] = "Cooker 2-1";
                gui_file.strName[6] = "Cooker 2-2";
                gui_file.strName[7] = "Cooker 3-1";
                gui_file.strName[8] = "Cooker 3-2";
                
                gui_file.type[3] = devJace.Files.ModuleType.Cooker;
                gui_file.type[4] = devJace.Files.ModuleType.Cooker;
                gui_file.type[5] = devJace.Files.ModuleType.Cooker;
                gui_file.type[6] = devJace.Files.ModuleType.Cooker;
                gui_file.type[7] = devJace.Files.ModuleType.Cooker;
                gui_file.type[8] = devJace.Files.ModuleType.Cooker;

                devJace.Files.Xml.Save(pathfile, gui_file);

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | whdmd_xui");
            }           

            pictureBoxLogo.Image = global::ChefBot.Properties.Resources.Doosan_Robotics_Partner_logo_light;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, 
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Logo View");

            Externs.Action_Loading actLoad = new Externs.Action_Loading();
            actLoad.Open();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Loading View");

            Externs.LogUtil delLog = new Externs.LogUtil(-1);
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, 
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Log Delete");

            //database connect           
            Cores.Core_Data.m_db.Connect();
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Database Init");

            core_Object = new Cores.Core_Object();
            //core_Object.MotorStatusEvent += new Cores.Core_Object.MotionDataEventHandler(FasTech_MotorData_Update);

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Object Init");

            //for (int idx = 0; idx <64; idx++)
            //{
            //    int iBit16 = idx % 16;
            //    int iBoard = idx % 4;
            //    int iNumber = (iBoard + 1) * 4096 + iBit16;
            //    Console.WriteLine($"GPIO LABEL L ::: {idx:00} / {iBoard} / {iBit16:X2} / {iNumber:X4}");
            //}
            //Console.WriteLine();

            configs = new Forms.FormConfigs(this);
            //formRobot.RobotSenderEvent += new Forms.FormRobotBus.EventHandler(Robot_Send_Event);
            //formDebug = new Forms.FormDebug(this);
            formExcute = new Forms.FormExcute(this);
            formExcute.RobotSoundEventEvent += new Forms.FormExcute.RobotSoundEventHandler(fromManualSound);
            formDIO = new Forms.FormDIO(this);
            formXaxis = new Forms.FormXaxis(this);
            formXaxis.RobotSoundEventEvent += new Forms.FormXaxis.RobotSoundEventHandler(fromManualSound);

            pageMain = new PageMain();
            formInfo = new FormInfo();
            formInfo.SuperVisorChangeEvent += new Forms.FormInfo.SuperVisorChangeEventHandler(SuperVisiorChanged);
            formAlarm = new FormAlarm();
            formRobot = new FormRobotBus();

            _forms = new List<Form>();
            _forms.Add(pageMain);
            _forms.Add(formExcute);
            _forms.Add(formXaxis);
            _forms.Add(configs);
            _forms.Add(formDIO);
            _forms.Add(formInfo);
            _forms.Add(formAlarm);
            _forms.Add(formDebug);//TEMP
            _forms.Add(formRobot);//TEMP

            //_forms.Add(formBase);            
            //_forms.Add(formRobot);//info            
            //_forms.Add(dashBorad);
            //_forms.Add(automatics);

            //DashBoard, Excute, Manual, Config, DIO, Base, Robot, Debug, Exit, Max

            metroPanelMain.Controls.Clear();
            foreach (Form frm in _forms)
            {
                devJace.Program.Performance(frm, false);
                metroPanelMain.Controls.Add(frm);
            }

            foreach (Control frm in metroPanelMain.Controls)
            {
                frm.Show();
                frm.Hide();
            }

            metroPanelMain.Controls[(int)EFrm.DashBoard].Show();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Display Init");

            rbButtons.Add(radioButton1);//main
            rbButtons.Add(radioButton2);//auto
            rbButtons.Add(radioButton3);//manual
            rbButtons.Add(radioButton4);//config
            rbButtons.Add(radioButton5);//monitor            
            rbButtons.Add(radioButton8);//alarm
            rbButtons.Add(radioButton7);//info
            rbButtons.Add(radioButton9);//debug
            rbButtons.Add(radioButton6);//exit

            foreach (RadioButton rb in rbButtons)
            {
                rb.BackColor = devi.Define.colorMainButtonLightColor;
            }
            rbButtons[(int)EFrm.DashBoard].Checked = true;
            radioButtonLightMode.Checked = true; //default light mode
            //DashBoard, Excute, Manual, Config, DIO, Robot, Debug, Exit, Max

            Leds.Add(lbLed1);
            Leds.Add(lbLed2);
            Leds.Add(lbLed3);
            Leds.Add(lbLed4);
            Leds.Add(lbLed5);
            Leds.Add(lbLed6);
            Leds.Add(lbLed7);

            for (int idx = 0; idx < (int)Ethernet.Max; idx++)
            {
                Leds[idx].LedColor = System.Drawing.Color.Lime;
            }

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | List Button Init");

            DefaultGUI();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | GUI Init");

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 500;
            timer.SynchronizingObject = this;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Start();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Timer Worker");


            asyncThread = new Thread(new ThreadStart(asyncPingCheck)) { IsBackground = true };
            asyncThread.Start();

         

            //System.Timers.Timer worker = new System.Timers.Timer();
            //worker.AutoReset = true;
            //worker.Interval = 950;
            //worker.SynchronizingObject = this;
            //worker.Elapsed += Worker_Elapsed;
            //worker.Start();
            //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Timer Tick");

            //for (int i = 0; i < 500; i++)
            //{
            //    double taget = 1 + (i * 0.0025);
            //
            //    Console.WriteLine($"{taget} ::: {Cores.Fas_Func.PPS_To_mm(taget, 2500, 25)}");
            //    
            //}

            //for (int i = 0; i < 500; i++)
            //{
            //    //double taget = 1 + (i * 0.0025);
            //
            //    Console.WriteLine($"{i} ::: {Cores.Fas_Func.PPR_To_mm(i)}");
            //
            //}

            //Console.WriteLine();


            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Default Location");

            actLoad.Close();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Load Complted");

            devJace.Program.VisibleConsole(!devJace.Program.IsWindowVisible());

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Console Init");


            DatabaseRead();

            //Setup_Easy_Mesh(FormTutorial.ETryal.Automode);            
        }

        void Setup_Easy_Mesh(FormTutorial.ETryal tryal)
        {
            frmTutorial = new FormTutorial(this.Location);
            frmTutorial.Show();
            frmTutorial.SetTutorial(0.8d, tryal);
        }

        void fromManualSound(int index)
        {
            //SoundWave(false, index);
            iCurrSoundPlayerNumver = index;
            SoundWaveEnQue(iCurrSoundPlayerNumver);
        }

        void SuperVisiorChanged(bool super)
        {
            formXaxis.AlignVisible(super);
            formDIO.AlignVisible(super);
        }

        /// <summary>
        /// 비동기 핑 체크
        /// </summary>
        void asyncPingCheck()
        {
            DateTime asyncTime = DateTime.Now;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
            while (true)
            {
                Thread.Sleep(1);
                //비동기 핑체크
                try
                {
                    foreach (string ip in Cores.Core_Object.GetObj_File.Device_IP)
                    {
                        Networks.NetAsyncPing.PingCheckAsync(ip);
                        //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} |" +
                        //$" IP Address : {ip}");
                    }

                    foreach (KeyValuePair<string, bool> lst in Networks.NetAsyncPing.dicReConnectIps)
                    {
                        for (int idx = 0; idx < Cores.Core_Object.GetObj_File.Device_IP.Count; idx++)
                        {
                            if (lst.Key == Cores.Core_Object.GetObj_File.Device_IP[idx])
                            {

                            }
                        }
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    TimeSpan ts = DateTime.Now - asyncTime;
                    asyncTime = DateTime.Now;
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(31)} |" +
                    //    $" asyncTime {ts.TotalMilliseconds:0000.0000}ms");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (ts.TotalMilliseconds > 3500)
                    {
                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(31)} |" +
                            $" asyncTime {ts.TotalMilliseconds:0000.0000}ms");
                    }

                }
                catch (Exception ex)
                {
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                        $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | Async Ping {ex.Message}");
                }
            }

        }

        /// <summary>
        /// 컨피그 화면에서 오는 두산로봇 모드버스 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Robot_Send_Event(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string name = btn.Name;
            int index = -1;

            string toolName = "buttonToolOutput";
            string gpioName = "buttonGpioOutput";
            string regsName = "buttonRegsitorSet";
            string testName = "buttonTestSet";

            if (name.Contains(toolName))
            {
                index = int.Parse(name.Replace(toolName, null));

                core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, index);
            }
            else if (name.Contains(gpioName))
            {
                index = int.Parse(name.Replace(gpioName, null));
                core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.GPIO, index);
            }
            else if (name.Contains(regsName))
            {
                //index = 16;
                //int data = 0xff;
                //core_Object.Modbus_Sender(index, data);
            }
            else if (name.Contains(testName))
            {
                index = 22;
                core_Object.Modbus_Sender(index);
            }

        }

        /// <summary>
        /// 컨피크 화면으로 데이터 업데이트
        /// 2023.03.02 인보크가 느려서 삭제
        /// </summary>
        /// <param name="data"></param>
        //void FasTech_MotorData_Update(Cores.Fas_Data data)
        //{
        //    //configs.UpdateInformation(data);
        //    formDIO.UpdateInformation(data);
        //}

        private void Worker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                BeginInvoke(new EventFiredDelegate(Work_TickTime));
            }
            catch
            {

            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MaintCountTEST();


            //throw new NotImplementedException();
            try
            {
                BeginInvoke(new EventFiredDelegate(Work_DateTime));
            }
            catch
            { }

        }

        /// <summary>
        /// 전체 화면 업데이트 전용
        /// </summary>
        void Work_DateTime()
        {
            //2023.04.24
            DefaultGUI();


            try
            {
                //2023.03.10 ::: 서피스 고 퍼모먼스 모니터 에러로 인하여 임시 삭제
                //this.labelCpuAsage.Text = "cpu 사용 : " + cpu.NextValue().ToString("00.000") + " %";
                //this.labelMemoryUsed.Text = "ram 사용가능량 : " + ram.NextValue().ToString() + " MB";            
                //this.labelCpuUsed.Text = "program cpu 사용 : " + prcess_cpu.NextValue().ToString("00.000") + " %";

                //this.labelCpuUsed.Text = process_name + " cpu 사용 : " + prcess_cpu.NextValue().ToString() + " %";


                PowerStatus psStatus = SystemInformation.PowerStatus;

                this.labelCpuAsage.Text = $"Battery State : {psStatus.BatteryChargeStatus}";

                this.labelMemoryUsed.Text = $"Cable : {psStatus.PowerLineStatus}";

                this.labelCpuUsed.Text = $"Time : {TimeSpan.FromSeconds(psStatus.BatteryLifeRemaining)} / {psStatus.BatteryLifePercent*100}%";
            }
            catch { }

            //Default Update
            //this.labelDateTime.Text = $"{DateTime.Now:tt HH:mm:ss}\n{DateTime.Now:yyyy-MM-dd}";
            //this.labelDateTime.Text = $"{DateTime.Now:tt HH:mm:ss yyyy-MM-dd}";
            this.labelDateTime.Text = $"{DateTime.Now:HH:mm:ss yyyy-MM-dd}";

            //Database TEST
            //TimeSpan tsSetTime = new TimeSpan(0, 0, 7, 1);
            //TimeSpan tsRusTime = new TimeSpan(0, 0, 4, 1);
            //Cores.Core_Data.ChickenCounter itemData = new Cores.Core_Data.ChickenCounter(DateTime.Now, (Cores.Core_Data.EChickenType)r.Next(1, 3), r.Next(1, 7), tsSetTime, tsRusTime);
            //Cores.Core_Data.m_ProductDB.UpdateInsertData(itemData);
            //
            //Cores.Core_Data.Chicken chicken = new Cores.Core_Data.Chicken(DateTime.Now.Hour, r.Next(10), r.Next(10), r.Next(10));
            //Cores.Core_Data.m_HoutDB.UpdateProductData(chicken.iHour, chicken.iFried, chicken.iFrench, chicken.iUnknown);

            //iReversCount++;
            //iReversCount = iReversCount % 2;
            //Leds[(int)Ethernet.Pc].State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)iReversCount;


            ConnectionUpdate();

            //Robot Information          

            //2023.04.06 ::: 모션 이나 로봇 움직일때 로그 저장
            string logRobot = $"\r\n";
            foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            {
                if (dat.iPrevData != dat.iData || dat.IsFirst == false)
                {
                    dat.iPrevData = dat.iData;
                    dat.IsFirst = true;

                    if (dat.strData == null)
                    {
                        dat.strData = "0";
                    }
                    //formRobot.Robot_Update(dat);
                    formDIO.Robot_Update(dat);
                    formXaxis.Robot_Update(dat);                   
                }
                if (dat.iCurrData != dat.iData)
                {
                    dat.iCurrData = dat.iData;

                    switch (dat.Address)
                    {
                        case 155:
                            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                               $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(0)}" +
                               $" | {dat.Address} {dat.strDesc} {dat.iData}");
                            break;

                        case 156:
                            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                               $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(10)}" +
                               $" | {dat.Address} {dat.strDesc} {dat.iData}");
                            break;

                        case 259:
                            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(10)}" +
                            $" | {dat.Address} {dat.strDesc} {dat.iData}");
                            break;

                        default:
                            break;
                    }
                }

                if (dat.strData == null)
                {
                    dat.strData = "0";
                }

                if (dat.IsUsed)
                {
                    logRobot += $"[Address : {dat.Address:000} | Value :{dat.strData.PadRight(10)} | Desc : {dat.strDesc.TrimEnd().PadRight(35)}], \r\n";

                }
            }   

            if (Cores.Core_Object.cobotConneted == true
                && core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Excute)
            {
                //int myAutoBit = 0;
                //int iTmpBit = 0;
                //iTmpBit = Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_OUTPUT.Move_Pos1] ? 1 : 0;
                //myAutoBit |= iTmpBit;
                //iTmpBit = Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_OUTPUT.Move_Pos2] ? 1 : 0;
                //myAutoBit |= iTmpBit << 1 ;
                //iTmpBit = Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_OUTPUT.Move_Pos3] ? 1 : 0;
                //myAutoBit |= iTmpBit << 2;
                //iTmpBit = Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_OUTPUT.Move_Pos4] ? 1 : 0;
                //myAutoBit |= iTmpBit << 3;
                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                //  devJace.Program.ELogLevel.Info, $" | {autoStep.strDesc.Replace(" ", null)} : {autoStep.iData:000} / PC : {myAutoBit}");

                var autoomplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 149);
                var autoCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 133);
                var autoPickPlace = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 131);
                var autoStep = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 154);

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                  devJace.Program.ELogLevel.Info, $" | AUTO DEBUG : {autoStep.iData:000}" +
                  $" / PC CMD: {Fas_Data.lstIO_OutState[Core_StepModule.COBOT][(int)COBOT_OUTPUT_COLL.Move_Cmd]}" +
                  $" / RB CMP: {Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)COBOT_INPUT_COLL.Move_Cmp]}" +
                  $" / Pick & Place : {(Core_StepModule.MyActionStepBuffer)autoPickPlace.iData}" +
                  $" / Pos Number : {(Core_StepModule.MyActionXStepBuffer)autoCommand.iData}");

            }

            if (Cores.Core_Object.cobotConneted == true
                && core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Stopped)
            {
                var manualStep = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 153);
                if (manualStep.iCurrData != manualStep.iData)
                {
                    
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                      devJace.Program.ELogLevel.Info, $" | {manualStep.strDesc.Replace(" ", null)} : {manualStep.iData:000}");
                }
                   
            }

            //2023.04.07 ;;; 로그 저장 조건
            //if ((Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_INPUT.Motion] == false)
            //    && Cores.Core_Object.cobotConneted==true
            //    )

            //{
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
            //          //devJace.Program.ELogLevel.Info, $" | {logRobot}");
            //    devJace.Program.ELogLevel.Info, $" | ");
            //}

            //

            if (Cores.Core_Object.cobotConneted == true
                && Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)COBOT_INPUT.Motion] == false)
            
            {
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                devJace.Program.ELogLevel.Info, $" | {logRobot}");
                //devJace.Program.ELogLevel.Info, $" | ");
            }

            //2023.04.14 ;;; (cooker 1-1 ~ 3-2)의 (조리시작 ~ 조리완료)까지의 시간 로그 저장 (현재 시간 - 각 동작 명령수행시 시간)



            //Robot Pos Update
            configs.Coobt_Pos_Update();

            //Robot Version
            if (Externs.Robot_Modbus_Table.strRobot_Version == null)
            {
                var major = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 256);
                var minor = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 257);
                var patch = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 258);

                if (major.IsUsed == false && minor.IsUsed == false && patch.IsUsed == false)
                {
                    Externs.Robot_Modbus_Table.strRobot_Version = $"{major.iData}.{minor.iData}.{patch.iData}";
                    //Console.WriteLine($"{DateTime.Now} >>> Externs.Robot_Modbus_Table.strRobot_Version ::: {Externs.Robot_Modbus_Table.strRobot_Version}");

                    formDIO.Robot_Verstion(Externs.Robot_Modbus_Table.strRobot_Version);
                    formXaxis.Robot_Verstion(Externs.Robot_Modbus_Table.strRobot_Version);
                }
            }

            //2023.02.28
            formXaxis.Xaxis_Update();
            formXaxis.UpdateInformation();

            //2023.03.02 ::: 이벤트 받기 위한 호출
            formDIO.UpdateInformation();

            //2023.03.08 ::: 경고창 / 알람창 띠울 것

            //2023.03.09 ::: 로봇 이미지 변경으로 자동운전 상태에 따라서 이미지 업데이트  
            formExcute.RobotLocation();
            formExcute.BasketImageUpdate();

            //2023.02.21 ::: AlarmUpdate
            //formAlarm.Update_Alarm("dataadf", "dasdadf", r.Next(4));                                                         
            //formExcute.RobotLocation(r.Next(2), r.Next(2), iRcount++);

            //2023.02.23 ::: 자동화면 업데이트
            //formExcute.Update_State(core_Object.processCore.eCurrState,
            //    core_Object.processCore.eCurrModeMatrix,
            //    Cores.Core_Object.GetObj_File.tsPauseSetMinTime,
            //    Cores.Core_Object.GetObj_File.tsPauseSetSecTime,
            //    ref core_Object.stepModuleCore.swPauseTimer);

            formExcute.Update_State();

            SoundPlayerUpdate();
            SoundWave();
            CookLogSave();
            LogDelete();
            PageMainCount();
            pageMain.CurrentUpdate();
            CurrentWatchdogUpdate();
            Work_AlarmForm_Update();
            OperationReady();
            //CurrentTimeChanged();
            Pause_State();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            TimeSpan tsProcessTime = DateTime.Now - processTime;
            processTime = DateTime.Now;

            //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
            //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(42)}" +
            //    $" | {tsProcessTime.TotalMilliseconds:000.0000}ms");

            if (tsProcessTime.TotalMilliseconds >= 1000)
            {
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(42)}" +
                    $" | {tsProcessTime.TotalMilliseconds:000.0000}ms");
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        void CookLogSave()
        {
            if (core_Object.stepModuleCore.GC_Chiken_Logs.Count != 0)
            {
                int cnt = core_Object.stepModuleCore.GC_Chiken_Logs.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (core_Object.stepModuleCore.GC_Chiken_Logs.TryDequeue(out Cores.Core_Data.ChickenCounter dat))
                    {
                        Cores.Core_Data.m_ProductDB.UpdateInsertData(dat);

                        Core_Data.CountUpdate(dat);
                        Cores.Core_Data.m_HoutDB.UpdateProductData(dat.dateTime.Hour,
                            Core_Data.lstHourFriedCount[dat.dateTime.Hour],
                             Core_Data.lstHourFrenchCount[dat.dateTime.Hour], 0);

                        string log = $"{dat.dateTime:yyyy-MM-dd HH:mm:ss} | {dat.cookerIndex}         | " +
                            $"{(int)dat.chickenType}        | " +
                            $"{dat.priotyIndex}           | " +
                            $"{dat.tsSetTime}    | " +
                            $"{dat.tsRunTime}    |";
                        Common.ClsLogFile.Write($"{DateTime.Now:yyyy-MM-dd_}Cook.log", log);
                    }
                }
            }
        }

        void LogDelete()
        {
            if (dtcurrentTime.Day == nextDayTime.Day)
            {
                dtcurrentTime = nextDayTime;
                nextDayTime = DateTime.Now.AddDays(1);

                Core_Data.CountReset();

                //2023.03.15 ::: 데이터 베이스 지난일 삭제
            }
        }

        void OperationReady()
        {

            if (devi.Define.IsCobotDebugMove)
                return;

            bool IsConn = true;

            IsConn &= Core_Object.cobotConneted;
            for (int idx =0; idx < Core_Object.fasTechConneted.Length; idx++)
            {
                IsConn &= Core_Object.fasTechConneted[idx];
            }


            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;


            if (cobotState == RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
                return;

            if (cobotState == RobotState_Ver_1_1.STANDALONE_RUNNING)
                return;

            var procState = core_Object.processCore.eCurrState;

            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_Pause])
            {
                return;
            }

                if (IsConn && cobotState != RobotState_Ver_1_1.STANDALONE_RUNNING && iCobotPrevState != robotState.iData
                && procState == ECurrState.CurrentState_Stopped
                && rbButtons[(int)EFrm.Manual].Checked == false
                && IsOperationReady == false
                && cobotState != RobotState_Ver_1_1.MANUAL_STANDBY)
            {
                IsOperationReady = true;
                iCobotPrevState = robotState.iData;
                Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.Normal, $"로봇 운전 준비를 진행 하시겠습니까?", 10);
                //RobotSoundEventEvent(17);
                //msg.Show();

                if (msg.ShowDialog() == DialogResult.OK)
                {
                    PackML.Command cmd = PackML.Command.CurrentState_None;

                    switch (core_Object.processCore.eCurrState)
                    {
                        //case PackML.ECurrState.CurrentState_Idle:
                        //    cmd = PackML.Command.CurrentState_Starting;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Excute:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Hold:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        case PackML.ECurrState.CurrentState_Stopped:
                            cmd = PackML.Command.CurrentState_Resetting;
                            break;
                            //case PackML.ECurrState.CurrentState_Starting:
                            //    cmd = PackML.Command.CurrentState_Stopping;
                            //    break;
                    }
                    if (cmd != PackML.Command.CurrentState_None)
                    {
                        core_Object.processCore.Set_State_Matrix(cmd);
                    }
                }
                IsOperationReady = false;
            }
            else if (IsConn && cobotState != RobotState_Ver_1_1.STANDALONE_RUNNING && iCobotPrevState != robotState.iData
              && procState == ECurrState.CurrentState_Stopped
              && rbButtons[(int)EFrm.Excute].Checked == true
              && IsOperationReady == false
              && cobotState != RobotState_Ver_1_1.MANUAL_STANDBY)
            {
                IsOperationReady = true;
                iCobotPrevState = robotState.iData;
                Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.Normal, $"로봇 운전 준비를 진행 하시겠습니까?", 10);
                //RobotSoundEventEvent(17);
                //msg.Show();

                if (msg.ShowDialog() == DialogResult.OK)
                {
                    PackML.Command cmd = PackML.Command.CurrentState_None;

                    switch (core_Object.processCore.eCurrState)
                    {
                        //case PackML.ECurrState.CurrentState_Idle:
                        //    cmd = PackML.Command.CurrentState_Starting;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Excute:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Hold:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        case PackML.ECurrState.CurrentState_Stopped:
                            cmd = PackML.Command.CurrentState_Resetting;
                            break;
                            //case PackML.ECurrState.CurrentState_Starting:
                            //    cmd = PackML.Command.CurrentState_Stopping;
                            //    break;
                    }
                    if (cmd != PackML.Command.CurrentState_None)
                    {
                        core_Object.processCore.Set_State_Matrix(cmd);
                    }
                }
                IsOperationReady = false;
            }
            //iCobotPrevState = robotState.iData;

        }

        void OperationReady(bool operAuto)
        {
            if (devi.Define.IsCobotDebugMove)
                return;


            bool IsConn = true;

            IsConn &= Core_Object.cobotConneted;
            for (int idx = 0; idx < Core_Object.fasTechConneted.Length; idx++)
            {
                IsConn &= Core_Object.fasTechConneted[idx];
            }


            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;


            if (cobotState == RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
                return;

            if (cobotState == RobotState_Ver_1_1.STANDALONE_RUNNING)
                return;

            var procState = core_Object.processCore.eCurrState;

            if (procState == ECurrState.CurrentState_Aborted)
                return;

            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_Pause])
            {
                return;
            }
            if (IsConn  && operAuto == true
                && procState == ECurrState.CurrentState_Stopped)
            {
                iCobotPrevState = robotState.iData;
                Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.Normal, $"로봇 운전 준비를 진행 하시겠습니까?", 10);
                //RobotSoundEventEvent(17);
                //msg.Show();

                if (msg.ShowDialog() == DialogResult.OK)
                {
                    PackML.Command cmd = PackML.Command.CurrentState_None;

                    switch (core_Object.processCore.eCurrState)
                    {
                        //case PackML.ECurrState.CurrentState_Idle:
                        //    cmd = PackML.Command.CurrentState_Starting;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Excute:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        //case PackML.ECurrState.CurrentState_Hold:
                        //    cmd = PackML.Command.CurrentState_Stopping;
                        //    break;
                        case PackML.ECurrState.CurrentState_Stopped:
                            cmd = PackML.Command.CurrentState_Resetting;
                            break;
                            //case PackML.ECurrState.CurrentState_Starting:
                            //    cmd = PackML.Command.CurrentState_Stopping;
                            //    break;
                    }
                    if (cmd != PackML.Command.CurrentState_None)
                    {
                        core_Object.processCore.Set_State_Matrix(cmd);
                    }
                }
            }
            //iCobotPrevState = robotState.iData;

        }

        void CurrentWatchdogUpdate()
        {

            bool IsDisplay = false;
            List<string> names = new List<string>();
            List<string> desc = new List<string>();
            List<int> level = new List<int>();
            int iMax = 0;

            if (devi.Define.IsCobotDebugMove == true)
                return;

            bool IsHigist = false;
            if (eCurrStateWatchdog != core_Object.processCore.eCurrState &&  core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Aborted)
            {
             
                foreach (Cores.Core_BlackBox.AttributesEventArgs alarm in Cores.Core_BlackBox.lstAlarmTotal)
                {
                    IsDisplay |= alarm.IsAlarmOn;
                    if (alarm.IsAlarmOn)
                    {
                        names.Add(alarm.strName);
                        desc.Add(alarm.strDescription);
                        level.Add((int)alarm.EType);
                        iMax++;
                    }
                }

                if (IsDisplay)
                {
                    if (alramMeassage == null || alramMeassage.IsDisposed)
                    {
                        alramMeassage = new FormErrorBox(this);
                        alramMeassage.ClearEvent += new FormErrorBox.ClearEventHandler(CommandClear); 
                        alramMeassage.Show();
                    }
                    else
                    {
                        alramMeassage.Focus();
                    }

                    try
                    {
                        if (formRobotEmc != null)
                        {
                            formRobotEmc.Close();
                            formRobotEmc = null;
                        }
                    }
                    catch
                    { 
                    
                    }
                    
                    
                    for (int idx = 0; idx < iMax; idx++)
                    {
                        alramMeassage.Update_Alarm(names[idx], desc[idx], level[idx]);
                        formAlarm.Update_Alarm(names[idx], desc[idx], level[idx]);
                    }



                    IsHigist = true;
                }
            }

            eCurrStateWatchdog = core_Object.processCore.eCurrState;



            //2023.04.13 ::: 경고 표시

            IsDisplay = true;
            names = new List<string>();
            desc = new List<string>();
            level = new List<int>();
            iMax = 0;
            foreach (Cores.Core_BlackBox.AttributesEventArgs alarm in Cores.Core_BlackBox.lstAlarmTotal)
            {
               
                if (alarm.IsAlarmOn)
                {
                    IsDisplay &= alarm.IsDisplay;
                    alarm.IsDisplay = true;
                    names.Add(alarm.strName);
                    desc.Add(alarm.strDescription);
                    level.Add((int)alarm.EType);
                    iMax++;
                }
            }

            if (IsDisplay == false && IsHigist == false)
            {
                if (alramMeassage == null || alramMeassage.IsDisposed)
                {
                    alramMeassage = new FormErrorBox(this);
                    alramMeassage.ClearEvent += new FormErrorBox.ClearEventHandler(CommandClear);
                    alramMeassage.Show();
                }
                else
                {
                    alramMeassage.Focus();
                }

                for (int idx = 0; idx < iMax; idx++)
                {
                    alramMeassage.Update_Alarm(names[idx], desc[idx], level[idx]);
                    formAlarm.Update_Alarm(names[idx], desc[idx], level[idx]);
                }
            }

        }

        void CommandClear()
        {
            if (core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Aborted)
            {
                PackML.Command cmd = PackML.Command.CurrentState_Clearing;
                core_Object.processCore.Set_State_Matrix(cmd);

                core_Object.alarmCore.Reset_Alarm();
            }
        }

        void ConnectionUpdate()
        {
            #region 접속 상태 표시, PC 상태 표시

            if (Leds[(int)Ethernet.Pc].State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
            {
                Leds[(int)Ethernet.Pc].LedColor = colorLime;
                Leds[(int)Ethernet.Pc].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            }
            
            
            //Connection
            if (Cores.Core_Object.cobotConneted)
            {
                if (Leds[(int)Ethernet.Cobot].LedColor != colorLime)
                {
                    Leds[(int)Ethernet.Cobot].LedColor = colorLime;
                }

                if (Leds[(int)Ethernet.Cobot].State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                {
                    Leds[(int)Ethernet.Cobot].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                }
                
            }
            else
            {
                if (Leds[(int)Ethernet.Cobot].LedColor != colorRed)
                {
                    Leds[(int)Ethernet.Cobot].LedColor = colorRed;
                }
                
                if (Leds[(int)Ethernet.Cobot].State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                {
                    Leds[(int)Ethernet.Cobot].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                }
            }

            for (int idx = 0; idx < Cores.Core_Object.fasTechConneted.Length; idx++)
            {
                if (Leds[idx + 2].State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                {
                    Leds[idx + 2].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                }

                if (Cores.Core_Object.fasTechConneted[idx])
                {
                    if (Leds[idx + 2].LedColor != colorLime)
                    {
                        Leds[idx + 2].LedColor = colorLime;
                    }
                }
                else
                {
                    if (Leds[idx + 2].LedColor != colorRed)
                    {
                        Leds[idx + 2].LedColor = colorRed;
                    }
                }
            }


            //if (Cores.Core_Object.fasTechConneted[0])
            //{
            //    Leds[(int)Ethernet.X_Axis].LedColor = colorLime;
            //    Leds[(int)Ethernet.X_Axis].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //else
            //{
            //    Leds[(int)Ethernet.X_Axis].LedColor = colorRed;
            //    Leds[(int)Ethernet.X_Axis].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //
            //if (Cores.Core_Object.fasTechConneted[1])
            //{
            //    Leds[(int)Ethernet.EIO_1].LedColor = colorLime;
            //    Leds[(int)Ethernet.EIO_1].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //else
            //{
            //    Leds[(int)Ethernet.EIO_1].LedColor = colorRed;
            //    Leds[(int)Ethernet.EIO_1].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //
            //if (Cores.Core_Object.fasTechConneted[2])
            //{
            //    Leds[(int)Ethernet.EIO_2].LedColor = colorLime;
            //    Leds[(int)Ethernet.EIO_2].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //else
            //{
            //    Leds[(int)Ethernet.EIO_2].LedColor = colorRed;
            //    Leds[(int)Ethernet.EIO_2].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //
            //if (Cores.Core_Object.fasTechConneted[3])
            //{
            //    Leds[(int)Ethernet.EIO_3].LedColor = colorLime;
            //    Leds[(int)Ethernet.EIO_3].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //else
            //{
            //    Leds[(int)Ethernet.EIO_3].LedColor = colorRed;
            //    Leds[(int)Ethernet.EIO_3].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //
            //if (Cores.Core_Object.fasTechConneted[4])
            //{
            //    Leds[(int)Ethernet.EIO_4].LedColor = colorLime;
            //    Leds[(int)Ethernet.EIO_4].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            //else
            //{
            //    Leds[(int)Ethernet.EIO_4].LedColor = colorRed;
            //    Leds[(int)Ethernet.EIO_4].State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            //}
            #endregion
        }

        void SoundPlayerUpdate()
        {
            //2023.03.11 ::: Sound wave player

            bool IsSoundCooking = false;

            for (int idx = 0; idx < core_Object.stepModuleCore.mChiken.Count; idx++)
            {
                if (core_Object.stepModuleCore.mChiken[idx].chickenState == Core_Data.EB_State.Waiting && core_Object.stepModuleCore.mChiken[idx].IsCookingRedySound == false)
                {
                    IsSoundCooking = true;
                    iCurrSoundPlayerNumver = 7;
                    core_Object.stepModuleCore.mChiken[idx].IsCookingRedySound = true;
                    SoundWaveEnQue(iCurrSoundPlayerNumver);
                    break;
                }
                else if (core_Object.stepModuleCore.mChiken[idx].chickenState == Core_Data.EB_State.Cooked && core_Object.stepModuleCore.mChiken[idx].IsCookingCompSound == false)
                {
                    IsSoundCooking = true;
                    iCurrSoundPlayerNumver = 8;
                    core_Object.stepModuleCore.mChiken[idx].IsCookingCompSound = true;
                    SoundWaveEnQue(iCurrSoundPlayerNumver);
                    break;
                }
                else if (core_Object.stepModuleCore.mChiken[idx].IsExist == true && core_Object.stepModuleCore.mChiken[idx].IsCookingForcSound == false)
                {
                    IsSoundCooking = true;
                    iCurrSoundPlayerNumver = 5;
                    core_Object.stepModuleCore.mChiken[idx].IsCookingForcSound = true;
                    SoundWaveEnQue(iCurrSoundPlayerNumver);
                    break;
                }
            }


            iCurrProcesStateBuffer = (int)core_Object.processCore.eCurrState;
            //if (core_Object.stepModuleCore.mPlaceUnload.IsCurrSensor == true)
            //{
            //    //iCurrSoundPlayerNumver = 9;
            //    //SoundWaveEnQue(iCurrSoundPlayerNumver);
            //}
            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false && Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_DIR] == true)
            {
                iCurrSoundPlayerNumver = 4;
                SoundWaveEnQue(iCurrSoundPlayerNumver);
            }
            else if (IsSoundCooking)
            {

            }
            else if (core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Resetting)
            {
                iPrevProcesStateBuffer = iCurrProcesStateBuffer;
                iCurrSoundPlayerNumver = 2;
                SoundWaveEnQue(iCurrSoundPlayerNumver);
            }
            else if (core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Idle)
            {
                iPrevProcesStateBuffer = iCurrProcesStateBuffer;
                iCurrSoundPlayerNumver = 3;
                SoundWaveEnQue(iCurrSoundPlayerNumver);
            }

            if(core_Object.stepModuleCore.cooking_shake)
            {
                core_Object.stepModuleCore.cooking_shake = false;
                iCurrSoundPlayerNumver = 11;
                SoundWaveEnQue(iCurrSoundPlayerNumver);
            }

            if (core_Object.stepModuleCore.cooking_oxzen)
            {
                core_Object.stepModuleCore.cooking_oxzen = false;
                iCurrSoundPlayerNumver = 10; 
                SoundWaveEnQue(iCurrSoundPlayerNumver);
            }

         

            //2023.04.26
            //SoundWave(false, iCurrSoundPlayerNumver);
        }

        string currentTime;
        string calTime;
        string startTime;
        /// <summary>
        /// 2023.03.20
        /// 메인 대시 보드 업데이트
        /// </summary>
        void PageMainCount()
        {
            //currentTime = System.DateTime.Now.ToString("h:mm:ss.ff");
            //System.DateTime start = DateTime.Now;
            //System.DateTime current = System.Convert.ToDateTime(currentTime);
            //System.TimeSpan timeCal = TimeSpan.FromHours( Environment.TickCount);
            //calTime = timeCal.ToString();


            for (int idx = 0; idx < (int)Core_Data.MainType.Max; idx++)
            {
                Core_Object.GetCounters[idx].currTime = DateTime.Now;

                if (Core_Object.GetCounters[idx].currTime.Day >= Core_Object.GetCounters[idx].DayTime.Day)
                {
                    Core_Object.GetOperNAs[idx].lstDays.Add(Core_Object.GetCounters[idx].DayTime.Day);
                    Core_Object.GetOperNAs[idx].lstOpers.Add((int)Core_Object.GetCounters[idx].iDays);

                    Core_Object.GetCounters[idx].iDays = 0;
                    Core_Object.GetCounters[idx].DayTime = Core_Object.GetCounters[idx].currTime.AddDays(1);
                    Core_Object.GetCounters[idx].ProduceD_Time = Core_Object.GetCounters[idx].currTime;

                    Cores.Core_Object.GetCounters[idx].tsDay = new TimeSpan();
                }

                if (Core_Object.GetCounters[idx].currTime.Month >= Core_Object.GetCounters[idx].MonthTime.Month)
                {
                    Core_Object.GetCounters[idx].iMonth = 0;
                    Core_Object.GetCounters[idx].MonthTime = Core_Object.GetCounters[idx].currTime.AddMonths(1);
                    Core_Object.GetCounters[idx].ProduceM_Time = Core_Object.GetCounters[idx].currTime;

                    Cores.Core_Object.GetCounters[idx].tsMonth = new TimeSpan();
                }

                if (Core_Object.GetCounters[idx].currTime != Core_Object.GetCounters[idx].prevTime)
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0);
                    ts = Core_Object.GetCounters[idx].currTime - Core_Object.GetCounters[idx].prevTime;
                    //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iTotal += ts.TotalMilliseconds;
                    //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].tsTotal += ts;
                    //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].tsMonth += ts;
                    //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].tsDay += ts;

                    Core_Object.GetCounters[idx].tsTotal += ts;
                    Core_Object.GetCounters[idx].tsMonth += ts;
                    Core_Object.GetCounters[idx].tsDay += ts;
                    Core_Object.GetCounters[idx].prevTime = Core_Object.GetCounters[idx].currTime;
                    //Console.WriteLine(ts);
                }
            }

            //Console.WriteLine();

            //TimeSpan ts = new TimeSpan(0, 0, 0);
            ///////////////////////////////////////////////////////////////////////////////////////////
            //ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.Robot].ProduceTime;
            ////ts = TimeSpan.FromMilliseconds(Environment.TickCount);
            //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iTotal += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.Robot].ProduceM_Time ;
            //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iMonth += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.Robot].ProduceD_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iDays += ts.TotalMilliseconds;
            ///////////////////////////////////////////////////////////////////////////////////////////
            //ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].ProduceTime;
            //Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].iTotal += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].ProduceM_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].iMonth += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].ProduceD_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.AirFilter].iDays += ts.TotalMilliseconds;
            ///////////////////////////////////////////////////////////////////////////////////////////
            //ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].ProduceTime;
            //Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].iTotal += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].ProduceM_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].iMonth += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].ProduceD_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.CookChange].iDays += ts.TotalMilliseconds;
            ///////////////////////////////////////////////////////////////////////////////////////////
            //ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].ProduceTime;
            //Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].iTotal += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].ProduceM_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].iMonth += ts.TotalMilliseconds;
            //
            ////ts = DateTime.Now - Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].ProduceD_Time;
            //Core_Object.GetCounters[(int)Core_Data.MainType.OilChange].iDays += ts.TotalMilliseconds;
            ///////////////////////////////////////////////////////////////////////////////////////////


            if (Core_Object.GetOperNAs.Count != 0)
            {
                for (int idx = 0; idx < (int)Core_Data.MainType.Max; idx++)
                {
                    if (Core_Object.GetOperNAs[idx].lstDays.Count > 31)
                    {
                        Core_Object.GetOperNAs[idx].lstDays.RemoveAt(0);
                    }
                    if (Core_Object.GetOperNAs[idx].lstOpers.Count > 31)
                    {
                        Core_Object.GetOperNAs[idx].lstOpers.RemoveAt(0);
                    }
                }
            }
        }

        void Pause_State()
        {
            if (Core_Object.GetObj_File.iLaserScannerUse == 1)
                return;

            IsPauseBuff = Fas_Data.lstIO_InState[CHEFY][(int)CHEFY_INPUT.L_PAUSE_SW] | Fas_Data.lstIO_InState[CHEFY][(int)CHEFY_INPUT.R_PAUSE_SW];
            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;
           
            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];

            if (IsPauseBuff == true && IsPauseLatch == false)
            {
                IsPauseLatch = true;
                //경고창
                IsPauseDoubleLatch = true;
                if (formPause == null || formPause.IsDisposed)
                {
                    formPause = new FormPause();
                    formPause.Show();
                }
                else
                {
                    formPause.Focus();
                }

            }

            if (cobotState == RobotState_Ver_1_1.INTERRUPTED && IsPauseDoubleLatchState != cobotState && IsPauseBuff == false)
            {
                if (formRobotEmc == null || formRobotEmc.IsDisposed)
                {
                    formRobotEmc = new FormRobotEmc(this);
                    formRobotEmc.ClearEvent += new FormRobotEmc.ClearEventHandler(ModBusCommandClear);
                    formRobotEmc.Show();
                }
                else
                {
                    formRobotEmc.Focus();
                }

            }
            IsPauseDoubleLatchState = cobotState;

            if (formRobotEmc != null && formRobotEmc.IsDisposed == false)
            {
                formRobotEmc.Status();
            }


            if (formPause != null && formPause.IsDisposed == false)
            {
                formPause.SwitchState(Cores.Fas_Data.lstIO_InState[CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_PAUSE_SW] ? 1 : 0,
                    Cores.Fas_Data.lstIO_OutState[CHEFY][(int)Core_StepModule.CHEFY_OUTPUT.Pause] ? 1 : 0,
                     Cores.Fas_Data.lstIO_InState[CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_PAUSE_SW] ? 1 : 0);
            }

            if (IsPauseBuff == false)
            {
                IsPauseLatch = false;


                if (formPause != null)
                {
                    formPause.Close();
                    //formPause = null;
                }
                //경고창
            }

            if (IsPauseDoubleLatch == true && cobotState == RobotState_Ver_1_1.STANDALONE_RUNNING)
            {
                IsPauseDoubleLatch = false;
            }

            if (IsPauseDoubleLatch == true)
            {

                if (IsPauseBuff == false && cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.INTERRUPTED
                    && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false
                    && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false)
                {
                    Thread.Sleep(100);
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    IsResumePolice = true;


                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset1} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset1]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset2} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset2]}");
                }
                else if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true
                     && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true
                     && IsResumePolice == true)
                {
                    Thread.Sleep(100);
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset1} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset1]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset2} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset2]}");
                }
                else if (IsPauseBuff == false && cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY
                    && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == true && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == true
                    && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false
                    && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == false
                    && OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == true
                    && IsResumePolice == true)
                {
                    Thread.Sleep(100);
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                  $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Task_Resume} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Task_Resume]}");
                }
                else if (cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                    && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true)
                {

                    IsResumePolice = false;

                    OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Task_Resume} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Task_Resume]}");


                }
                else if (cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY
                    && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true
                    )
                {
                    Thread.Sleep(100);

                    IsResumePolice = false;
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                  $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Task_Resume} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Task_Resume]}");
                }

            }

            

        }

        void ModBusCommandClear()
        {
            core_Object.Modbus_Sender(129, 0);
            core_Object.Modbus_Sender(133, 0);
            IsPauseDoubleLatch = true;
        }

        /// <summary>
        /// 레이져 스캐너 경고 창 띄우는 곳
        /// </summary>
        void Work_AlarmForm_Update()
        {
            ////2023.03.08 ::: 배출 근접 센서 확인
            //bool IsPlaceSensor = Cores.Fas_Data.lstIO_InState[1][8];
            //
            //bool IsOutputChiken = false;
            //for (int idx = 0; idx < core_Object.stepModuleCore.mChiken.Count; idx++)
            //{
            //    //배출 확인
            //    if (core_Object.stepModuleCore.mChiken[idx].IsCookingComplted() == true && )
            //    {
            //        IsOutputChiken = true;
            //        break;
            //    }
            //}

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            if (core_Object.processCore.eCurrState == ECurrState.CurrentState_Resetting)
                return;

            //2023.06.14 ::: 핸드가이딩 지금 안하니까 해제
            //if (cobotState != RobotState_Ver_1_1.HANDGUIDING_CONTROL_RUNNING)
            //    return;

            if (Cores.Core_Object.GetObj_File.iLaserScannerUse == 0)
            {
                if (formLaser != null)
                {
                    formLaser.Close();
                }
                return;
            }
            bool[] InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            if (InStateBuff[(int)COBOT_INPUT.Motion] == true && Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false)
            {
                return;
            }
            
            // fasTechConneted 신호가 전부 true인 경우에만 레이저 스캐너 알람 작동
            bool isconnected = true;
            for (int idx = 0; idx < Core_Object.fasTechConneted.Length; idx++)
            {
                isconnected &= Core_Object.fasTechConneted[idx];
            }
            if (isconnected==true)
            {
                //public bool IsCurrentProtectArea = false;
                //public bool IsPreviousProtectArea = false;
                //public bool IsCurrentWarningArea = false;
                //public bool IsPreviousWarningArea = false;
                IsCurrentProtectArea = Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area];
                IsCurrentWarningArea = Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Waring_area];

                if (IsCurrentProtectArea == false && IsCurrentProtectArea != IsPreviousProtectArea)
                {
                    if (formLaser == null || formLaser.IsDisposed == true)
                    {
                        formLaser = new FormAborting();
                        formLaser.Show();
                    }
                    else
                    {
                        formLaser.Focus();
                    }
                    formLaser.meassage("코봇 위험 지점 입니다.", 1);
                    iCurrSoundPlayerNumver = 16;
                    SoundWaveEnQue(iCurrSoundPlayerNumver);
                }
                else if (IsCurrentWarningArea == false && IsCurrentWarningArea != IsPreviousWarningArea)
                {
                    if (formLaser == null || formLaser.IsDisposed == true)
                    {
                        formLaser = new FormAborting();
                        formLaser.Show();
                    }
                    else
                    {
                        formLaser.Focus();
                    }
                    formLaser.meassage("코봇 주의 지점 입니다.", 0);
                    iCurrSoundPlayerNumver = 15;
                    SoundWaveEnQue(iCurrSoundPlayerNumver);
                }
                //else
                //{
                //    if (formLaser != null)
                //    {
                //        formLaser.Close();
                //        formLaser = null;
                //    }
                //}
                if(IsCurrentProtectArea == true && IsCurrentWarningArea == true && formLaser != null)
                {
                    formLaser.Close();
                }

                IsPreviousProtectArea = IsCurrentProtectArea;
                IsPreviousWarningArea = IsCurrentWarningArea;

            }
            

           

        }

        /// <summary>
        /// 빨리 업데이트 필요한 것들 사용 // 현재 사용하지 않음
        /// </summary>
        void Work_TickTime()
        {
            //iTickCount++;
            //iTickCount = iTickCount % 10;

            ////formBase.TEST();

            ////_forms[(int)EFrm.Base].test



            ////System.Threading.Thread.Sleep(200);


            //if (iTickCount == 0)
            //{
            //    //formExcute.RobotLocation(1, 1, iRcount++);
            //    formExcute.RobotLocation(r.Next(2), r.Next(2), iRcount++);

            //    formExcute.Update_State(core_Object.processCore.eCurrState, core_Object.processCore.eCurrModeMatrix, core_Object.GetObj_File.tsPauseSetMinTime, core_Object.GetObj_File.tsPauseSetSecTime, ref core_Object.stepModuleCore.swPauseTimer);

            //    //Robot Information
            //    foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            //    {
            //        if (dat.iPrevData != dat.iData || dat.IsFirst == false)
            //        {
            //            dat.iPrevData = dat.iData;
            //            dat.IsFirst = true;

            //            if (dat.strData == null)
            //            {
            //                dat.strData = "0";
            //            }
            //        }
            //        //formRobot.Robot_Update(dat);
            //        formDIO.Robot_Update(dat);
            //        formXaxis.Robot_Update(dat);
            //    }

            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
            //}

            //if (iRcount >= 100)
            //{
            //    iRcount = 0;
            //}

        }

        /// <summary>
        /// 폼 핸들러 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
             RadioButton rb = sender as RadioButton;
            //int index = int.Parse(rb.Name.Substring(rb.Name.Length - 1));
            //index = index - 1;
            //index = (int)(EFrm)index;

       

            if (rb.Checked == false)
            {
                var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
                var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);
                if (manualCommand.iData == 0)
                {
                    return;
                }
                
            }
            

            if (rb != rbButtons[(int)EFrm.Exit])
            {


                //var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
                //var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);

                var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
                var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);

                if (manualCommand.iData != 0)
                {
                    //Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,  "수동 조작 중 입니다.");
                    //if (msg.ShowDialog() == DialogResult.OK)
                    //{
                    //    //rbButtons[(int)EFrm.Manual].Checked = true;
                    //    return;
                    //}
                }



                for (int idx = 0; idx < (int)EFrm.Exit; idx++)
                {
                    if (rb == rbButtons[idx])
                    {
                        if (rb != rbButtons[(int)EFrm.Debug])
                        {
                            metroPanelMain.Controls[idx].Show();
                            rbButtons[idx].BackColor = devi.Define.colorSubButtonColor;
                        }

                        EFrm AutoManual = (EFrm)idx;
                        PackML.Command cmd = PackML.Command.CurrentState_None;
                        PackML.EModeMatrix mode = PackML.EModeMatrix.None_Matrix;
                        string strModeName = null;
                        if (rb.Checked)
                        {
                            switch (AutoManual)
                            {
                                case EFrm.Excute://auto

                                    //strModeName = "Auto";
                                    //mode = PackML.EModeMatrix.AutomaticMode_Matrix;
                                    //cmd = PackML.Command.CurrentState_Resetting;
                                    //if (core_Object.processCore.eCurrState != PackML.ECurrState.CurrentState_Idle)
                                    //{
                                    //    //core_Object.processCore.Set_State_Matrix(cmd);
                                    //}
                                    //if (core_Object.processCore.eCurrModeMatrix != PackML.EModeMatrix.AutomaticMode_Matrix)
                                    //{
                                    //    core_Object.processCore.Set_StateMode(mode);
                                    //}
                                    OperationReady(true);
                                    break;

                                case EFrm.Manual://Manual
                                    strModeName = "Manu";
                                    mode = PackML.EModeMatrix.ManualMode_Matrix;
                                    cmd = PackML.Command.CurrentState_Stopping;

                                   


                                    if (core_Object.processCore.eCurrState != PackML.ECurrState.CurrentState_Stopped)
                                    {
                                        Common.FormMessageBox msg = new Common.FormMessageBox("조리를 정지 하시겠습니까?");
                                        if(msg.ShowDialog() == DialogResult.OK) 
                                        {
                                            core_Object.processCore.Set_State_Matrix(cmd);
                                            if (core_Object.processCore.eCurrModeMatrix != PackML.EModeMatrix.ManualMode_Matrix)
                                            {
                                                core_Object.processCore.Set_StateMode(mode);
                                            }
                                        }

                                       
                                    }


                                
                                    break;

                                case EFrm.Debug:
                                    break;
                            }

                         

                            //Console.WriteLine($"{DateTime.Now} >>> Process Cmd {strModeName} [ {core_Object.processCore.eCurrState } / {cmd}  ] [  {core_Object.processCore.eCurrModeMatrix} / {mode} ]");

                            //TEST
                            //if (alramMeassage == null || alramMeassage.IsDisposed)
                            //{
                            //    alramMeassage = new Forms.FormErrorBox(Forms.FormErrorBox.AlarmLevel.Warning, "TEST", "desc");
                            //    alramMeassage.Show();
                            //}
                            //else
                            //{
                            //    alramMeassage.Update_Alarm("dafdsf", "dasd", (int)Forms.FormErrorBox.AlarmLevel.Warning);
                            //    alramMeassage.Focus();
                            //}

                            formXaxis.iMyPageNumber = idx;

                        }

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                             $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Event(RB) {((EFrm)formXaxis.iMyPageNumber)}");

                    }
                    else
                    {
                        if (rb != rbButtons[(int)EFrm.Debug])
                        {
                            metroPanelMain.Controls[idx].Hide();
                            rbButtons[idx].BackColor = devi.Define.colorMainButtonLightColor;
                        }
                        
                    }
                }

               
            }
            else
            {
                if (rb.Checked)
                {
                    Common.FormMessageBox msg = new Common.FormMessageBox("프로그램 종료 하시겠습니까?");
                    if (msg.ShowDialog() == DialogResult.OK)
                    {
                        //정상 종료
                        bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[Cores.Core_StepModule.COBOT];
                        for (int idx = 0; idx < OutStateBuff.Length; idx++)
                        {
                            OutStateBuff[idx] = false;
                        }
                        Cores.Fas_Motion.SetOutput((int)Cores.Core_StepModule.IO_Board.EzEtherNetIO_4, OutStateBuff);

                        
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[Cores.Core_StepModule.CHEFY];
                        if (OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause] == true
                            || OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause_Spare] == true)
                        {
                            OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause] = false;
                            OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause_Spare] = false;

                            Cores.Fas_Motion.SetOutput((int)Cores.Core_StepModule.IO_Board.EzEtherNetIO_2, OutStateBuff);
                        }
                        

                        //2023.04.24 ::: 엑스축 모터 구동 중이면 정지
                        Cores.Fas_Motion.StopMotion(MOTOR, false);

                        this.Close();
                    }
                    else
                    {
                        rb.Checked = false;
                    }
                }

               
            }
        }

        /// <summary>
        /// 두산로봇 로고 라이트모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonLightMode_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxLogo.Image = global::ChefBot.Properties.Resources.Doosan_Robotics_Partner_logo_light;
        }

        /// <summary>
        /// 두산로봇 로고 다크모드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxLogo.Image = global::ChefBot.Properties.Resources.Doosan_Robotics_Partner_logo_lightblue;
        }

        /// <summary>
        /// 폼 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Program Closed");

            devi.Define.IsDisposed = true;

            Application.Exit();
        }

        /// <summary>
        /// 폼 종료 중 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //2023.03.01
            //core_Object.ObjectFIle_Save(); 위로 수정

            string path = null;


            for (int idx = 0; idx < Core_Object.GetCounters.Count; idx++)
            {
                Core_Object.GetCounters[idx].ProduceM_Time = DateTime.Now;
                Core_Object.GetCounters[idx].ProduceD_Time = DateTime.Now;
            }

            for (int idx = 0; idx < Core_Object.GetCounters.Count; idx++)
            {

                Core_Object.GetCounters[idx].iTotal = Core_Object.GetCounters[idx].tsTotal.TotalSeconds;
                Core_Object.GetCounters[idx].iMonth = Core_Object.GetCounters[idx].tsMonth.TotalSeconds;
                Core_Object.GetCounters[idx].iDays = Core_Object.GetCounters[idx].tsDay.TotalSeconds;

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                $" | GetCounters [{(Core_Data.MainType)idx}] :  {Core_Object.GetCounters[idx].tsTotal}");

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                $" | GetCounters [{(Core_Data.MainType)idx}] :  {Core_Object.GetCounters[idx].tsMonth}");

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                $" | GetCounters [{(Core_Data.MainType)idx}] :  {Core_Object.GetCounters[idx].tsDay}");
            }

            gui_file.dtDateTime = DateTime.Now;

            List<ulong> countBuff = new List<ulong>();
            countBuff.Add(gui_file.lGripperUseCount);
            countBuff.Add(gui_file.lRobotDownUseCount);
            countBuff.Add(gui_file.lRobotUpUseCount);
            countBuff.Add(gui_file.lRobotShakeUseCount);
            countBuff.Add(gui_file.lRobotOxzeneUseCount);
            countBuff.Add(gui_file.lRobotOilDrainUseCount);
            countBuff.Add(gui_file.iRobotClerningUseCount);
            Core_Data.m_MaintDB.UpdateProductData(countBuff.ToArray());

            Core_Data.m_CobotDB.UpdateProductData(Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iDays);

            path = $"{Application.StartupPath}\\whdmd_res.xml";
            for (int idx = 0; idx < core_Object.stepModuleCore.mChiken.Count; idx++)
            {
                devJace.Files.Xml.Save(path, Core_Object.mExternLoadChiken);
            }

            path = $"{Application.StartupPath}\\whdmd_cnt.xml";
            devJace.Files.Xml.Save(path, Core_Object.GetCounters);

            core_Object.ObjectFIle_Save();

            path = $"{Application.StartupPath}\\whdmd_srv.xml";
            devJace.Files.Xml.Save(path, Core_Object.GetOperNAs);

            path = $"{Application.StartupPath}\\whdmd_xui.xml";
            devJace.Files.Xml.Save(path, gui_file);


            core_Object.Dispose();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | Program Closing");


        }

        /// <summary>
        /// 화면 초기 값
        /// </summary>
        void DefaultGUI()
        {
            progressBar1.Value = 100;
            progressBar1.Style = ProgressBarStyle.Marquee;
            
            int fontSize = 12;
            foreach (RadioButton rb in rbButtons)
            {
                rb.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            }

            this.labelDateTime.Font = new Font(Fonts.FontLibrary.Families[0], 20);
            radioButtonLightMode.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            radioButtonDarkMode.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            //fontSize = 12;
            for (int idx = 0; idx < (int)Ethernet.Max; idx++)
            {
                Leds[idx].Font = new Font("Arial", fontSize);
            }

            fontSize = 9;
            this.labelCpuAsage.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelMemoryUsed.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelCpuUsed.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            labelCpuAsage.ForeColor = System.Drawing.Color.White;
            labelMemoryUsed.ForeColor = System.Drawing.Color.White;
            labelCpuUsed.ForeColor = System.Drawing.Color.White;



            //List<Size> lstFormSize = new List<Size>();
            //lstFormSize.Add(this.Size);
            //for (int i = 0; i < _forms.Count; i++)
            //{
            //    lstFormSize.Add(_forms[i].Size);
            //}
            //Console.WriteLine();

        }

        /// <summary>
        /// 디버그 콘솔창 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton9_Click(object sender, EventArgs e)
        {
            devJace.Program.VisibleConsole(!devJace.Program.IsWindowVisible());

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                $" | Debug Show {devJace.Program.IsWindowVisible()}");
        }

        /// <summary>
        /// 데이터베이스 초기 동기화
        /// </summary>
        void DatabaseRead()
        {
            int limitDisplayCount = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            bool IsSucess = false;
            IsSucess = Cores.Core_Data.m_CobotDB.LimitData(limitDisplayCount);
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                           $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | m_CobotDB Database Delete ::: {IsSucess}");
            IsSucess = Cores.Core_Data.m_MaintDB.LimitData(limitDisplayCount);
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                          $"{System.Reflection.MethodBase.GetCurrentMethod().Name} | m_MaintDB Database Delete ::: {IsSucess}");

            if (Cores.Core_Data.m_ProductDB.ReadTotalCount(out int totalCount))
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iTotal = totalCount;
            }
            else
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iTotal = 0;
            }

            if (Cores.Core_Data.m_ProductDB.ReadMonthCount(out int monthCount))
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iMonth = monthCount;
            }
            else
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iMonth = 0;
            }

            if (Cores.Core_Data.m_ProductDB.ReadDayCount(out int dayCount))
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iDays = dayCount;
            }
            else
            {
                Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iDays = 0;
            }


            //if (Cores.Core_Data.m_CobotDB.ReadData(out List<string[]> dat))
            //{
            //    //Core_Object.strArryRobotUsedTime = new string[dat.Count];
            //
            //    for (int idx = 0; idx < dat.Count; idx++)
            //    {
            //        Core_Object.strArryRobotUsedTime.Add(dat[idx]);
            //    }
            //}
        }

        void CurrentTimeChanged()
        {
            if (DateTime.Now.Day > dtcurrentTime.Day)
            {
                dtcurrentTime = DateTime.Now.AddDays(7);

                //현재 날짜 기준으로 -7일것 찾아 지우기.

            }
        }

        void SoundWave()
        {
            if (SoundQue.Count >= 100)
            {
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Error,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(42)}" +
                $" | Sound Que Count : {SoundQue.Count}");

                iBeforeSoundPlayerNumver = 0;
            }


            if (SoundQue.IsEmpty)
                return;

            timeSpanSoundTime = DateTime.Now - dateTimeSoundTime;
            if (timeSpanSoundTime.TotalSeconds <= 3)
            {
                return;
            }
            dateTimeSoundTime = DateTime.Now;

            SoundQue.TryPop(out int Category);

            if (iBeforeSoundPlayerNumver == Category)
            {
                return;
            }
            iBeforeSoundPlayerNumver = Category;
            //SoundQue.TryPopRange(out int[] soundBuffs);

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
              $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(42)}" +
              $" | Sound Que Count : {SoundQue.Count}");
            SoundQue.Clear();

            //player.
            switch (Category)
            {

                default:
                    //player = new SoundPlayer($"{Application.StartupPath}\\Sound\\warning.mp3");
                    break;

                case 0:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\warning.wav");
                    break;

                case 1:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\error.wav");
                    break;

                case 2:
                    //2023.05.04
                    if (core_Object.processCore.eCurrState == ECurrState.CurrentState_Idle)
                    {
                        player = new SoundPlayer($"{Application.StartupPath}\\Sound\\ready_cook.wav");
                    }
                    
                    break;

                case 3:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\ready_cook_complete.wav");
                    break;

                case 4:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\robot_move_warning.wav");
                    //iPrevSoundPlayerNumver = 1000;
                    break;

                case 5:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\force_out_command.wav");
                    break;

                case 6:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\force_out_complete.wav");
                    break;

                case 7:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cook_command.wav");
                    break;

                case 8:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cook_complete.wav");
                    break;

                case 9:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cook_comp_warning.wav");
                    break;

                case 10:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Cooking_Oxyzen.wav");
                    break;

                case 11:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Cooking_Shaking.wav");
                    break;

                case 12:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Robot_UnUse_Position.wav");
                    break;

                case 13:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Robot_Use_Position.wav");
                    break;

                case 14:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Robot_Home_Moving.wav");
                    break;

                case 15:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Warning_Area.wav");
                    break;

                case 16:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\Protective_Area.wav");
                    break;

                //2023.04.26 음성 추가
                case 17://오일 교체를 진행 하겠습니다.
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\oil_change.wav");
                    break;
                case 18:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_use_1.wav");
                    break;
                case 19:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_use_2.wav");
                    break;
                case 20:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_use_3.wav");
                    break;
                case 21:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_unuse_1.wav");
                    break;
                case 22:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_unuse_2.wav");
                    break;
                case 23:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\cooker_unuse_3.wav");
                    break;
                case 24://배출 바스켓 가져가는 것 지연시 발생 음.
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\unload_check.wav");
                    break;
                case 25://일시정지 버튼 사용시
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\robot_pause.wav");
                    break;
                case 26://일시정지 버튼 해제시
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\robot_unpause.wav");
                    break;
                case 27:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\x-axis_homing.wav");
                    break;
                case 28:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\x-alarm_on.wav");
                    break;
                case 29:
                    player = new SoundPlayer($"{Application.StartupPath}\\Sound\\x-alarm_off.wav");
                    break;
            }

            if (player != null)
            {
                player.Play();
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
               $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(42)}" +
               $" | {Category}");
            }

            //if (IsRepeat)
            //{
            //    player.PlayLooping();
            //}
            //else
            //{
            //    player.Play();
            //}

        }

        void SoundWaveStop()
        {
            //SoundPlayer player = new SoundPlayer($"{Application.StartupPath}\\Sound\\warning.mp3");
            //player.Play();
            //player.PlayLooping();
            //wmp.URL = $"{Application.StartupPath}\\Sound\\warning.wav";

            player.Stop();

        }

        void MaintCountTEST()
        {
            //Random r = new Random();
            //gui_file.lGripperUseCount = (ulong)r.Next(120);
            //gui_file.lRobotDownUseCount = (ulong)r.Next(120);
            //gui_file.lRobotUpUseCount = (ulong)r.Next(120);
            //gui_file.lRobotShakeUseCount = (ulong)r.Next(120);
            //gui_file.lRobotOxzeneUseCount = (ulong)r.Next(120);
            //gui_file.lRobotOilDrainUseCount = (ulong)r.Next(120);
            //gui_file.iRobotClerningUseCount = (ulong)r.Next(120);

            if (gui_file.dtDateTime.Day != DateTime.Now.Day)
            {
                Project_Main.FormMain.gui_file.lGripperUseCount = 0;
                Project_Main.FormMain.gui_file.lRobotDownUseCount = 0;
                Project_Main.FormMain.gui_file.lRobotUpUseCount = 0;
                Project_Main.FormMain.gui_file.lRobotShakeUseCount = 0;
                Project_Main.FormMain.gui_file.lRobotOxzeneUseCount = 0;
                Project_Main.FormMain.gui_file.lRobotOilDrainUseCount = 0;
                Project_Main.FormMain.gui_file.iRobotClerningUseCount = 0;
            }

            List<ulong> countBuff = new List<ulong>();
            countBuff.Add(gui_file.lGripperUseCount);
            countBuff.Add(gui_file.lRobotDownUseCount);
            countBuff.Add(gui_file.lRobotUpUseCount);
            countBuff.Add(gui_file.lRobotShakeUseCount);
            countBuff.Add(gui_file.lRobotOxzeneUseCount);
            countBuff.Add(gui_file.lRobotOilDrainUseCount);
            countBuff.Add(gui_file.iRobotClerningUseCount);

            Core_Data.m_MaintDB.UpdateProductData(countBuff.ToArray());

            Core_Data.m_CobotDB.UpdateProductData(Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iDays);

        }

    }
}
