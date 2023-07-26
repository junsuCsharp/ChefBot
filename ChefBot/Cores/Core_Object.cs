using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using FASTECH;
using System.IO;
using Externs;
using System.Collections.Concurrent;
using PackML;
using static Cores.Core_StepModule;
using static Externs.Robot_Modbus_Table;
using System.Security.Cryptography;
using System.Reflection;
using devi;

namespace Cores
{
    /*
     *  2022.11.23 ::: 파스텍 X 축 / IO 인터페이스
     *  파스텍 모터 및 아이오 접속
     *  로봇 접속
     *  모드 버스 상시 읽고 쓰기
     *  설정파일 저장 및 불러오기
     *  
     *  
     *  
     *  
     *  
     *  INDEX
     *  Adaptive_GPIO()
     */


    public class Core_Object : IDisposable
    {
        Thread objThread;
        public Core_BlackBox alarmCore;
        public PackML.ClsPackML processCore;
        public Core_StepModule stepModuleCore;        
        public Core_Adapter adapterCore;
        public Core_Robot core_Robot;
        public Fas_Data fasTechData = new Fas_Data();

        //Setting FIles
        public static devJace.Files.DIO_File GetDIO_File = new devJace.Files.DIO_File();
        public static devJace.Files.Obj_File GetObj_File = new devJace.Files.Obj_File();
        public static devJace.Files.Pos_File GetPos_File = new devJace.Files.Pos_File();
        public static devJace.Files.Cobot_File GetCos_File = new devJace.Files.Cobot_File();

        //2023.04.14 ::: 인스턴스 전체 생성 완료
        public static bool IsIntanceCredite = false;

        public MotionDataEventHandler MotorStatusEvent;
        public delegate void MotionDataEventHandler(Fas_Data data);

        public EventHandler AckNomalActionEvent;
        public delegate void EventHandler(object sender, EventArgs e);        

        //디바이스 접속 상태 변수
        public static bool[] fasTechConneted = new bool[] { false, false, false, false, false};
        public static bool cobotConneted = false;

        //데이터 베이스 동기화 전용
        public static List<Core_Data.MainCounter> GetCounters = new List<Core_Data.MainCounter>();

        //데이터 베이스 동기화 전용 로봇 구동시간
        public static List<string[]> strArryRobotUsedTime = new List<string[]>();


        //설비가동현황 / 유지보수 현황
        public static List<devJace.Files.Etc_File> GetOperNAs = new List<devJace.Files.Etc_File>();

        private int[] iBit = new int[] {0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80, 
                                        0x100, 0x200, 0x400, 0x800, 0x1000, 0x2000, 0x4000, 0x8000};

        //치킨 모듈 재접속시 데이터 저장용
        public static List<exChiken> mExternLoadChiken = new List<exChiken>();

        //2023.04.06 ::: LaserScanner Warning Area Speed Control
        //public int iSpeed
        public bool IsCurrWarningArea = false;
        public bool IsPrevWarningArea = false;
        public bool IsCurrProtectArea = false;
        public bool IsPrevProtectArea = false;
        public bool IsResumePolice = false;
        Stopwatch swResumeOffTime = new Stopwatch();


        //laser scanner
        Externs.Robot_Modbus_Table.RobotState_Ver_1_1 LaserRobotCurrState = Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF;
        //Externs.Robot_Modbus_Table.RobotState_Ver_1_1 RobotPrevState = Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF;

        //Collaborativ_Hand
        Externs.Robot_Modbus_Table.RobotState_Ver_1_1 SemiRobotCurrState = Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF;
        //Externs.Robot_Modbus_Table.RobotState_Ver_1_1 SemiRobotPrevState = Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF;

        public int iCurrLaserScannerUse = -1;
        public int iPrevLaserScannerUse = -1;


        public Core_Object()
        {
            //for (int idx = 0; idx < (int)Core_Data.MainType.Max; idx++)
            //{
            //    Core_Data.MainCounter instance = new Core_Data.MainCounter();
            //    instance.mainType = (Core_Data.MainType)idx;
            //    GetCounters.Add(instance);
            //}

            for (int idx = 0; idx < (int)Core_Data.MainType.Max; idx++)
            {
                //Core_Data.MainCounter instance = new Core_Data.MainCounter();
                //instance.mainType = (Core_Data.MainType)idx;
                //GetCounters.Add(instance);
            
                devJace.Files.Etc_File instance = new devJace.Files.Etc_File();
                instance.lstDays = new List<int>();
                instance.lstOpers = new List<int>();

                for (int i = 0; i < 30; i++)
                {
                    instance.lstDays.Add(i);
                    instance.lstOpers.Add(0);
                }

                GetOperNAs.Add(instance);
            }

            //Prioty 0
            processCore = new PackML.ClsPackML();
            processCore.CurrentState_Action += ProcessCore_CurrentState_Action;
            processCore.Contiused_Action += ProcessCore_Contiused_Action;
            processCore.AckNormal_Action += ProcessCore_AckNormal_Action;

            alarmCore = new Core_BlackBox();
            alarmCore.OnAlarm += AlarmCore_OnAlarm;          

            stepModuleCore = new Core_StepModule();
            stepModuleCore.Action_Complted += StepModuleCore_Action_Complted;
            stepModuleCore.Cobot_Command += StepModuleCore_Cobot_Command;
            stepModuleCore.ResetAlarmEvent += SteModuleCore_Action_ResetAlarm;
            stepModuleCore.ModBusSendEvent += SteModuleCore_Action_ModbusSend;


            adapterCore = new Core_Adapter();
            adapterCore.OnProcessCommand += AdapterCore_OnProcessCommand;

            //core_Robot = new Core_Robot("127.0.0.1", "192.168.137.100");
            core_Robot = new Core_Robot("127.0.0.1", "192.168.0.110");

            if (objThread != null)
            {
                objThread.Abort();
            }
            else
            {
                objThread = new Thread(new ThreadStart(Run)) { IsBackground = true };
                objThread.Start();
            }
        }

        private void SteModuleCore_Action_ResetAlarm()
        {
            alarmCore.Reset_Alarm();
        }

        private void SteModuleCore_Action_ModbusSend(int address, int value)
        {
            Modbus_Sender(address, value);
        }

        private void ProcessCore_Contiused_Action(ECurrState state, EModeMatrix mode)
        {
            //throw new NotImplementedException();
            if (stepModuleCore == null)
                return;
            stepModuleCore.Contiuse_Action(state, mode);
        }

        private void StepModuleCore_Cobot_Command(int resNumber, int data)
        {
            //throw new NotImplementedException();
            Modbus_Sender(resNumber, data);
        }

        public void ObjectFIle_Save()
        {
            string path = null;
            path = $"{Application.StartupPath}\\whdmd_xbj.xml";

            if (File.Exists(path))
            {
                try
                {
                    //File.Delete(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The deletion failed: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("Specified file doesn't exist");
            }

            devJace.Files.Xml.Save(path, GetObj_File);
        }

        public static void ObjectFile_Save_Extern()
        {
            string path = null;
            path = $"{Application.StartupPath}\\whdmd_xbj.xml";

            if (File.Exists(path))
            {
                try
                {
                    //File.Delete(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The deletion failed: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("Specified file doesn't exist");
            }

            devJace.Files.Xml.Save(path, GetObj_File);
        }

        private void ProcessCore_AckNormal_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            //eCurrState = state;
            //eModeMatrix = mode;

            //Console.WriteLine($"{DateTime.Now} >>> {System.Reflection.MethodBase.GetCurrentMethod().Name} {state} {mode}");


            //응답 받았을때 초기화 하자
            //if (state == PackML.ECurrState.CurrentState_Starting && mode == PackML.EModeMatrix.AutomaticMode_Matrix)
            //{
            //    stepModuleCore.Init_Action(GetObj_File);
            //}
        }

        /// <summary>
        /// 프로세스 명령
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdapterCore_OnProcessCommand(object sender, Core_Adapter.ProcessReachedEventArgs e)
        {
            //확인 필요
            processCore.Set_State_Matrix((PackML.Command)e.Command);
        }

        /// <summary>
        /// 프로세스 명령 확인
        /// </summary>
        private void StepModuleCore_Action_Complted()
        {   
            processCore.Recv_Action_Complted();

           
            //업데이트 부분
            if (processCore.eCurrState == PackML.ECurrState.CurrentState_Idle && processCore.eCurrModeMatrix == PackML.EModeMatrix.ManualMode_Matrix)
            {
                //processCore.Set_State_Matrix(PackML.Command.CurrentState_Resetting);
                processCore.Set_StateMode(PackML.EModeMatrix.AutomaticMode_Matrix);
            }
            //if (processCore.eCurrState == PackML.ECurrState.CurrentState_Stopped && processCore.eCurrModeMatrix == PackML.EModeMatrix.AutomaticMode_Matrix)
            //{
            //    processCore.Set_State_Matrix(PackML.Command.CurrentState_Resetting);
            //    //processCore.Set_StateMode(PackML.EModeMatrix.AutomaticMode_Matrix);
            //}
            //Console.WriteLine($"{DateTime.Now} >>> {System.Reflection.MethodBase.GetCurrentMethod().Name} {processCore.eCurrState} {processCore.eCurrModeMatrix}");
            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                 $" | {processCore.eCurrModeMatrix} | {processCore.eCurrState}");
        }

        /// <summary>
        /// 프로세스 실행
        /// </summary>
        /// <param name="state"></param>
        /// <param name="mode"></param>
        private void ProcessCore_CurrentState_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            if (stepModuleCore == null)
                return;
            stepModuleCore.CurrentState_Action(state, mode);        
        }        

        /// <summary>
        /// 알람 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmCore_OnAlarm(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Core_BlackBox.AttributesEventArgs attributes = e as Core_BlackBox.AttributesEventArgs;
            //Console.WriteLine(attributes.dateTime);


            PackML.Command cmd = PackML.Command.CurrentState_None;
            

            switch (attributes.EType)
            {
                case Core_BlackBox.ECode.Low:
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                        $"| {attributes.strName} | {attributes.strMessage}" +
                        $"| {attributes.EType}");
                    break;
                case Core_BlackBox.ECode.Middle:
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Error,
                      $"| {attributes.strName} | {attributes.strMessage}" +
                      $"| {attributes.EType}");
                    cmd = PackML.Command.CurrentState_Aborting;
                    break;
                case Core_BlackBox.ECode.High:
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
                      $"| {attributes.strName} | {attributes.strMessage}" +
                      $"| {attributes.EType}");
                    cmd = PackML.Command.CurrentState_Aborting;
                    break;
            }

            if (cmd != PackML.Command.CurrentState_None)
            {
                processCore.Set_State_Matrix(cmd);
            }

        }

      
        /// <summary>
        /// 오브젝트
        /// </summary>
        void Run()
        {
            bool IsDone = false;

            #region 설정 파일들 불러오기 및 저장
            //load
            string path = null;
            path = $"{Application.StartupPath}\\whdmd_xos.xml";
            if (devJace.Files.Xml.Load(path, ref GetPos_File))
            {
                if (GetPos_File.lstRealPositions.Count != GetPos_File.iMaxPosition)
                {
                    //Console.WriteLine();
                    //에러처리
                }
            }
            else
            {

                devJace.Files.Xml.Save(path, GetPos_File);
            }

            //load
            path = $"{Application.StartupPath}\\whdmd_xio.xml";

            if (devJace.Files.Xml.Load(path, ref GetDIO_File))
            {
                //Set_Initalize_IO();
            }
            else
            {
                Set_Default_IO();
                devJace.Files.Xml.Save(path, GetDIO_File);
            }

            //load
            path = $"{Application.StartupPath}\\whdmd_xbj.xml";
            if (devJace.Files.Xml.Load(path, ref GetObj_File))
            {
                Define.eGripper = (EGRIPS)GetObj_File.iGripperModel;

                Define.eCUSTOM = (ECUSTOM)GetObj_File.iCustomUser;

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                  $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                  $" | eGripper {Define.eGripper}");

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                $" | eCUSTOM {Define.eCUSTOM}");
            }
            else
            {
                Set_Default_Obj();
                devJace.Files.Xml.Save(path, GetObj_File);
            }

            //My IP Search
            List<string> lstCurrFindIp = Networks.NetFunc.GetLocalIP();
            if (lstCurrFindIp.Count != 0)
            {
                GetObj_File.Local_IP = new List<string>();
                GetObj_File.Local_IP.AddRange(lstCurrFindIp);
                devJace.Files.Xml.Save(path, GetObj_File);
            }

            //load
            path = $"{Application.StartupPath}\\whdmd_cos.xml";
            if (devJace.Files.Xml.Load(path, ref GetCos_File))
            {

            }
            else
            {
                Set_Default_Cos();
                devJace.Files.Xml.Save(path, GetCos_File);
            }

            //치킨 모듈 불러오기
            path = $"{Application.StartupPath}\\whdmd_res.xml";
            if (devJace.Files.Xml.Load(path, ref mExternLoadChiken))
            {

            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    exChiken exChiken = new exChiken();
                    exChiken.chickenState = Core_Data.EB_State.None;
                    exChiken.chickenType = Core_Data.EChickenType.Unknown;
                    exChiken.cookerIndex = i;
                    exChiken.dateTimeCooker = DateTime.Now;
                    exChiken.dateTimeCooking = DateTime.Now;
                    exChiken.tsCurTime = new TimeSpan(0, 5, 0);
                    exChiken.tsSetTime = new TimeSpan(0, 12, 0);
                    mExternLoadChiken.Add(exChiken);
                }

                //mExternLoadChiken[idx].stopwatch = new Stopwatch();
                //mExternLoadChiken[idx].stopwatch.
                devJace.Files.Xml.Save(path, mExternLoadChiken);
            }

            path = $"{Application.StartupPath}\\whdmd_cnt.xml";
            if (devJace.Files.Xml.Load(path, ref GetCounters))
            {
                for (int idx = 1; idx < GetCounters.Count; idx++)
                {
                    Core_Object.GetCounters[idx].tsTotal = TimeSpan.FromSeconds(Core_Object.GetCounters[idx].iTotal);
                    Core_Object.GetCounters[idx].tsMonth = TimeSpan.FromSeconds(Core_Object.GetCounters[idx].iMonth);
                    Core_Object.GetCounters[idx].tsDay = TimeSpan.FromSeconds(Core_Object.GetCounters[idx].iDays);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | GetCounters [{(Core_Data.MainType)idx}] :  {GetCounters[idx].tsTotal}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | GetCounters [{(Core_Data.MainType)idx}] :  {GetCounters[idx].tsMonth}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | GetCounters [{(Core_Data.MainType)idx}] :  {GetCounters[idx].tsDay}");

                }
            }
            else
            {
                devJace.Files.Xml.Save(path, GetCounters);
            }
            #endregion

            //connnection
            cobotConneted = core_Robot.IsConneted;

            if (cobotConneted)
            {
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
               $" | ROBOT : DOOSAN | IP : {GetObj_File.Device_IP[GetObj_File.Device_IP.Count - 1]} | IsConnectFasMotion : {cobotConneted}");
            }
            else
            {
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                  $" | ROBOT : DOOSAN | IP : {GetObj_File.Device_IP[GetObj_File.Device_IP.Count - 1]} | IsConnectFasMotion : {cobotConneted}");
            }

            

            bool IsConnectFasMotion = true;
            for (int idx = 0; idx < GetObj_File.Device_IP.Count -1; idx++)
            {
                IsConnectFasMotion &= Fas_Motion.Connect(GetObj_File.Device_IP[idx], idx);
                fasTechConneted[idx] = IsConnectFasMotion;
                bool IsCheckDrvInfo = Fas_Motion.CheckDriveInfo(idx, out string fastechInfo);

                if (IsCheckDrvInfo)
                {
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | FASTECH : {fastechInfo} | IP : {GetObj_File.Device_IP[idx]} | IsConnectFasMotion : {IsConnectFasMotion}");
                }
                else
                {
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                           $" | FASTECH ::: {fastechInfo} | IP : {GetObj_File.Device_IP[idx]} | IsConnectFasMotion : {IsConnectFasMotion}");
                }

                Fas_Motion.SetReConnect(idx, IsConnectFasMotion);
            } 

           

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            //2023.02.10 ::: 모드버스 동기화 시간 조절
            TimeSpan tsModbusGetTime = new TimeSpan(0, 0, 0, 0, 700);
            DateTime dtModbusGetTime = DateTime.Now + tsModbusGetTime;
            //TimeSpan tsModbusDelayTime = new TimeSpan(0, 0, 0, 0, 100);

            processCore.Set_StateMode(PackML.EModeMatrix.ManualMode_Matrix);
            //processCore.Set_State_Matrix(PackML.Command.CurrentState_Resetting);

            DateTime processTime = DateTime.Now;


            for (int idx = 0; idx < stepModuleCore.mChiken.Count; idx++)
            {
                stepModuleCore.mChiken[idx].SetCooker(idx, new TimeSpan(0, GetObj_File.tsChikenSetMinTime[idx], GetObj_File.tsChikenSetSecTime[idx]));
            }

            cobotConneted = core_Robot.IsConneted;
            //stepModuleCore.Connected(cobotConneted, fasTechConneted);          
            IsDone = true;
            IsIntanceCredite = true;
            while (IsDone)
            {
                Thread.Sleep(1);
                //System.Windows.Forms.Application.DoEvents();
                //dtNow = DateTime.Now;

                cobotConneted = core_Robot.IsConneted;
                Get_Robot_Lost(ref cobotConneted);

                ///50ms                
                //bool TEST = true;
                Get_Fastech_Lost(ref IsConnectFasMotion);
                Get_Fastech_Motion(ref IsConnectFasMotion);

                WatchDogAlarm(alarmCore, processCore.eCurrState, processCore.eCurrModeMatrix);     
                
                Adaptive_GPIO();
                
                ////모드버스 동기화 보내는 메세지 패킷
                ///보낼때 200ms 소요
                if (DateTime.Now.Ticks >= dtModbusGetTime.Ticks)
                {
                    Modbus_Sender();
                    dtModbusGetTime = DateTime.Now.AddMilliseconds(tsModbusGetTime.TotalMilliseconds);
                }             

                TimeSpan tsProcessTime = DateTime.Now - processTime;
                processTime = DateTime.Now;
                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                //    $" | {processCore.eCurrModeMatrix} | {processCore.eCurrState} | {tsProcessTime.TotalMilliseconds:000.0000}ms");

                if (tsProcessTime.TotalMilliseconds >= 1200)
                {
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | {processCore.eCurrModeMatrix} | {processCore.eCurrState} | {tsProcessTime.TotalMilliseconds:000.0000}ms");
                }

            }
        }
        
        void Get_Robot_Lost(ref bool IsConnectRobot)
        {
            if (IsConnectRobot == false)
            {
                core_Robot.ReConnection();
            }
        }

        /// <summary>
        /// 알람 모듈 검색 조건 설정 및 찾기
        /// </summary>
        /// <param name="box"></param>
        /// <param name="state"></param>
        /// <param name="mode"></param>
        void WatchDogAlarm(Core_BlackBox box, ECurrState state, EModeMatrix mode)
        {
            bool IsConnect = true;
            int resValue = 1;

            IsConnect &= Cores.Core_Object.cobotConneted;

            for (int idx = 0; idx < Cores.Core_Object.fasTechConneted.Length; idx++)
            {
                IsConnect &= Cores.Core_Object.fasTechConneted[idx];
            }   
            
            if(IsConnect == false)
            {
                resValue = IsConnect ? 1 : 0;
                box.Alarm_Manager(16, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
                return;
            }


           
            int L_EMC_Number = 13;
            int R_EMC_Number = 14;
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_EMC_SW] ? 1 : 0;

            box.Alarm_Manager(L_EMC_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(L_EMC_Number, state, ECurrState.CurrentState_Idle, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(L_EMC_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(L_EMC_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(L_EMC_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_EMC_SW] ? 1 : 0;

            box.Alarm_Manager(R_EMC_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC_Number, state, ECurrState.CurrentState_Idle, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.High, 0, resValue);


            int M_SMPS_Number = 00;
            int M_UNIT_Number = 01;
            int R_POWS_Number = 02;
            int X_POWS_Number = 03;
            int L_OPEN_Number = 04;
            int R_OPEN_Number = 05;

            int R_EMC1_Number = 06;
            int R_EMC2_Number = 07;

            int R_HOME_Number = 08;
            int R_ABN1_Number = 09;
            int R_ABM2_Number = 10;
            int R_TORQ_Number = 11;
            int R_STOP_Number = 12;
            int R_ENCI_Number = 15;


            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.EMC_1] ? 1 : 0;
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Idle, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC1_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);;

            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.EMC_2] ? 1 : 0;
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Idle, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);
            box.Alarm_Manager(R_EMC2_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.High, 0, resValue);

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.DC24VSMPS_CP] ? 1 : 0;
            box.Alarm_Manager(M_SMPS_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.ControlUnit_CP] ? 1 : 0;
            box.Alarm_Manager(M_UNIT_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.RobotPower_CP] ? 1 : 0;
            box.Alarm_Manager(R_POWS_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.XaxisPower_CP] ? 1 : 0;
            box.Alarm_Manager(X_POWS_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.LeftDoorOpen] ? 1 : 0;
            box.Alarm_Manager(L_OPEN_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFX][(int)Core_StepModule.CHEFX_INPUT.RightDoorOpen] ? 1 : 0;
            box.Alarm_Manager(R_OPEN_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            
            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.Home] ? 1 : 0;
            box.Alarm_Manager(R_HOME_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.ABNOMAL_1] ? 1 : 0;
            box.Alarm_Manager(R_ABN1_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.ABNOMAL_2] ? 1 : 0;
            box.Alarm_Manager(R_ABM2_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.SafeTouque] ? 1 : 0;
            box.Alarm_Manager(R_TORQ_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);
            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.SafeStop] ? 1 : 0;
            box.Alarm_Manager(R_STOP_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);


            int L_Pause_Number = 19;
            int M_Pause_Number = 20;
            int R_Pause_Number = 21;

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_PAUSE_SW] ? 1 : 0;
            box.Alarm_Manager(L_Pause_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);            
            box.Alarm_Manager(L_Pause_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);            
            box.Alarm_Manager(L_Pause_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_PAUSE_SW] ? 1 : 0;
            box.Alarm_Manager(R_Pause_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);
            box.Alarm_Manager(R_Pause_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);
            box.Alarm_Manager(R_Pause_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);


            //2023.05.22
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_OUTPUT.Pause] ? 1 : 0;
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);            
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);            
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_OUTPUT.Pause_Spare] ? 1 : 0;
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Clearing, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Stopping, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);
            box.Alarm_Manager(M_Pause_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Middle, 0, resValue);



            //resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT_COLL.ENC_Init_Alarm] ? 1 : 0;
            //box.Alarm_Manager(R_ENCI_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Low, 0, resValue);
            //resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT_COLL.ENC_Init_Alarm] ? 1 : 0;
            //box.Alarm_Manager(R_ENCI_Number, state, ECurrState.CurrentState_Resetting, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Low, 0, resValue);
            //resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT_COLL.ENC_Init_Alarm] ? 1 : 0;
            //box.Alarm_Manager(R_ENCI_Number, state, ECurrState.CurrentState_Idle, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 0, Core_BlackBox.ECode.Low, 0, resValue);


            resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT_COLL.ENC_Init_Alarm] ? 1 : 0;
            box.Alarm_Manager(R_ENCI_Number, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.Low, 0, Core_BlackBox.ECode.Low, 0, resValue);


            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            //if (cobotState == RobotState_Ver_1_1.HANDGUIDING_CONTROL_RUNNING)
            if (cobotState == RobotState_Ver_1_1.SAFE_OFF)
            {
                resValue = Fas_Data.lstIO_InState[Core_StepModule.COBOT][(int)Core_StepModule.COBOT_INPUT.SafeTouque] ? 1 : 0;
                box.Alarm_Manager(R_TORQ_Number, state, ECurrState.CurrentState_Stopped, mode, EModeMatrix.ManualMode_Matrix, Core_BlackBox.EActive.Low, 100, Core_BlackBox.ECode.High, 0, resValue);
            }


            //2023.05.23
            //로봇 외력 발생으로 상태가 인터럽트 발생인데, 엑스축 움직이는 경우 알람 으로
            if(cobotState == RobotState_Ver_1_1.INTERRUPTED) 
            {
                int X_Motion = 22;
                resValue = Fas_Data.lstAxis_State[(int)EAxis_Status.Motioning] ? 1 : 0;
                box.Alarm_Manager(X_Motion, state, ECurrState.CurrentState_Excute, mode, EModeMatrix.AutomaticMode_Matrix, Core_BlackBox.EActive.High, 1000, Core_BlackBox.ECode.High, 0, resValue);
            }


        }

        /// <summary>
        /// Borad 4, Input16/Output16, 
        /// </summary>
        void Set_Default_IO()
        {
            GetDIO_File.iMaxIO = 64;

            //GetDIO_File.Inputs = new List<bool>();
            //GetDIO_File.Outputs = new List<bool>();

            GetDIO_File.InLabels = new List<string>();
            GetDIO_File.OutLabels = new List<string>();

            GetDIO_File.InNames = new List<string>();
            GetDIO_File.OutNames = new List<string>();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                  devJace.Program.ELogLevel.Info,
                  $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                  $" | GPIO Index Instance");

            //X1000 ~ X4000
            //Y1000 ~ Y4000
            for (int idx = 0; idx < GetDIO_File.iMaxIO; idx++)
            {
                int iBit16 = idx % 16;
                int iBit04 = ((idx / 16) + 1) * 10;
                GetDIO_File.InLabels.Add($"X{iBit04}{iBit16:X2}");
                GetDIO_File.OutLabels.Add($"Y{iBit04}{iBit16:X2}");

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    devJace.Program.ELogLevel.Info,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | IO Index ::: {iBit04}{iBit16:X2}");

                //GetDIO_File.Inputs.Add(false);
                //GetDIO_File.Outputs.Add(false);

                GetDIO_File.InNames.Add("NULL");
                GetDIO_File.OutNames.Add("NULL");
            }

            //Console.WriteLine();
        }     

        /// <summary>
        /// 
        /// </summary>
        void Set_Default_Obj()
        {
            GetObj_File.Device_IP = new List<string>();
            GetObj_File.Device_IP.Add("192.168.0.105");
            GetObj_File.Device_IP.Add("192.168.0.106");
            GetObj_File.Device_IP.Add("192.168.0.107");
            GetObj_File.Device_IP.Add("192.168.0.108");
            GetObj_File.Device_IP.Add("192.168.0.109");
            GetObj_File.Device_IP.Add("192.168.0.110");

            GetObj_File.Device_Name = new List<string>();
            GetObj_File.Device_Name.Add("X-Axis Motor");
            GetObj_File.Device_Name.Add("EzEtherNetIO_1");
            GetObj_File.Device_Name.Add("EzEtherNetIO_2");
            GetObj_File.Device_Name.Add("EzEtherNetIO_3");
            GetObj_File.Device_Name.Add("EzEtherNetIO_4");            
            GetObj_File.Device_Name.Add("Doosan Robot");

            GetObj_File.lstOilCheckdCount = new List<int>() { 50, 50, 50 };
            GetObj_File.lstOilTemperature = new List<int>() { 180, 180, 180 };
            GetObj_File.lstOilMeckUse = new List<bool>() { true, true, true };

            //GetObj_File.tsPauseSetTime = new TimeSpan(0, 0, 10);
            //GetObj_File.tsChikenSetTime = new List<TimeSpan>() { new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0) };
            GetObj_File.lstFryerName = new List<string>() { "Cookware A", "Cookware B", "Cookware C" };
            GetObj_File.lstLoaderName = new List<string>();
            GetObj_File.lstLoaderName.Add("Load A");
            GetObj_File.lstLoaderName.Add("Load B");
            GetObj_File.lstLoaderName.Add("Load C");
            GetObj_File.lstLoaderSetIO = new List<string>();
            GetObj_File.lstLoaderSetIO.Add("IN");
            GetObj_File.lstLoaderSetIO.Add("IN");
            GetObj_File.lstLoaderSetIO.Add("OUT");
            GetObj_File.lstCookerName = new List<string>();
            GetObj_File.lstCookerName.Add("Cooker 1-1");
            GetObj_File.lstCookerName.Add("Cooker 1-2");
            GetObj_File.lstCookerName.Add("Cooker 2-1");
            GetObj_File.lstCookerName.Add("Cooker 2-2");
            GetObj_File.lstCookerName.Add("Cooker 3-1");
            GetObj_File.lstCookerName.Add("Cooker 3-2");

            GetObj_File.tsChikenSetMinTime = new List<int>() { 7, 7, 7, 7, 7, 7 };
            GetObj_File.tsChikenSetSecTime = new List<int>() { 0, 0, 0, 0, 0, 0 };

            GetObj_File.lstOperRecipeName = new List<string>() { "Cobot Speed", "X-Axis Speed", "X-Axis AccDecTime",
                                                            "Load Delay", "UnLoad Delay", "UnLoad Basket Delay"};
            GetObj_File.lstOperRecipeRange = new List<string>() { "1 ~ 100", "1 ~ 100", "200 ~ 750", "0 ~ 9", "0 ~ 9", "0 ~ 9"};
            GetObj_File.lstOperRecipeUnit = new List<string>() { "%", "%", "msec","Sec", "Sec", "Sec"};
            GetObj_File.lstOilMeckUse = new List<bool>() { false, false, false };

            GetObj_File.lstOptionName = new List<string>() { "Display Language", "X-Axis Control", "Safety Laser Scanner", "Basket Shake", "Basket Weight", "MeterialWeight", "ChikenWeight" };
            //GetObj_File.lstOptionName = new List<string>() { "언어 설정", "x축 컨트롤", "안전 레이저 스캐너", "바스켓 흔들기", "바스켓 무게", "재료 무게", "치킨 무게" };
            GetObj_File.lstOptionRange = new List<string>() { "1 / 0", "1 / 0", "1 / 0", "1 / 0", "0 ~ 1.5", "0 ~ 1.5", "0 ~ 1.5" };
            GetObj_File.lstOptionUnit = new List<string>() { "English / Korea", "Yes / No", "Use / UnUse", "Use / UnUse", "Kg", "Kg", "Kg" };


        }

        /// <summary>
        /// 
        /// </summary>
        void Set_Default_Cos()
        {
            GetCos_File.Joint = new List<double[]>();
            GetCos_File.strName = new List<string>();
            for (int i = 0; i < GetCos_File.iMaxLength; i++)
            {
                double[] buff = new double[GetCos_File.iMaxPos];
                for (int j = 0; j < GetCos_File.iMaxPos; j++)
                {
                    buff[j] = 0;
                }
                GetCos_File.Joint.Add(buff);
                GetCos_File.strName.Add("NULL");
            }
        }

        /// <summary>
        /// 파스텍 단축 모터 상태 가져오기
        /// </summary>
        /// <param name="IsConnected"></param>
        void Get_Fastech_Motion(ref bool IsConnected)
        {
            if (IsConnected == false)
                return;
            //Console.WriteLine($"{methodName} New Fas_Data ");

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int nBdID = 0;
            uint dwAxisStatus = 0;
            uint dwInStatus = 0;
            uint dwOutStatus = 0;

            ushort wPosItemNo = 0;

            //Fas_Data _item = new Fas_Data();
            //if (EziMOTIONPlusELib.FAS_GetAllStatus(nBdID, ref dwInStatus, ref dwOutStatus, ref dwAxisStatus,
            //                                   ref _item.iCmdPos, ref _item.iActPos, ref _item.iPosError, ref _item.iActVel,
            //                                 ref wPosItemNo) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Error
            //}

            //for (int bitIndex = 0; bitIndex < (int)EAxis_Status.Max; bitIndex++)
            //{
            //    _item.lstAxis_State[bitIndex] = (dwAxisStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;

            //    //Console.WriteLine($"{methodName} {((EAxis_Status)bitIndex).ToString().PadRight(20)} >>> {_item.lstAxis_State[bitIndex].ToString().PadRight(5)} {bitIndex:00}:Success");
            //}

            //for (int bitIndex = 0; bitIndex < (int)EFas_UsrInput.Max; bitIndex++)
            //{
            //    _item.lstInput_State[bitIndex] = (dwInStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;
            //}

            //for (int bitIndex = 0; bitIndex < (int)EFas_UsrOutput.Max; bitIndex++)
            //{
            //    _item.lstOutput_State[bitIndex] = (dwOutStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;
            //}

            //if (Fas_Motion.GetOriginOffset(nBdID, out _item.iHomeOffset))
            //{ }


            //Fas_Data _item = new Fas_Data();
            if (EziMOTIONPlusELib.FAS_GetAllStatus(nBdID, ref dwInStatus, ref dwOutStatus, ref dwAxisStatus,
                                               ref Fas_Data.iCmdPos, ref Fas_Data.iActPos, ref Fas_Data.iPosError, ref Fas_Data.iActVel,
                                             ref wPosItemNo) != EziMOTIONPlusELib.FMM_OK)
            {
                //Error

                IsConnected = false;
            }

            for (int bitIndex = 0; bitIndex < (int)EAxis_Status.Max; bitIndex++)
            {
                Fas_Data.lstAxis_State[bitIndex] = (dwAxisStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;

                //Console.WriteLine($"{methodName} {((EAxis_Status)bitIndex).ToString().PadRight(20)} >>> {_item.lstAxis_State[bitIndex].ToString().PadRight(5)} {bitIndex:00}:Success");
            }

            for (int bitIndex = 0; bitIndex < (int)EFas_UsrInput.Max; bitIndex++)
            {
                Fas_Data.lstInput_State[bitIndex] = (dwInStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;
            }

            for (int bitIndex = 0; bitIndex < (int)EFas_UsrOutput.Max; bitIndex++)
            {
                Fas_Data.lstOutput_State[bitIndex] = (dwOutStatus >> bitIndex & 1 /**/ ) != 0 ? true : false;
            }

            if (Fas_Motion.GetOriginOffset(nBdID, out Fas_Data.iHomeOffset))
            { }


            //try
            //{
            //    MotorStatusEvent(_item);
            //}
            //catch(Exception ex)
            //{
            //    //Console.WriteLine($"{methodName} ::: {ex.Message}");
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
            //       $" | MotorStatusEvent");
            //}

            //2023.03.02 ::: 아이오보드 읽기
            uint[] inputData = new uint[4];
            uint[] outputData = new uint[4];
            for (int idx = 1; idx < 5; idx++)
            {
                if (Fas_Motion.GetInput(idx, out inputData[idx - 1]))
                {
                    for (int i = 0; i < 16; i++)
                    {
                        bool bON = ((inputData[idx - 1] & (0x01 << i)) != 0);
                        Fas_Data.lstIO_InState[idx-1][i] = bON;
                        //Console.WriteLine("Input bit {0} is {1}.", i, ((bON) ? "ON" : "OFF"));
                    }

                    //if (Fas_Data.usInputState[idx - 1] != inputData[idx - 1])
                    //{
                    //    Fas_Data.usInputState[idx - 1] = inputData[idx - 1];
                    //
                    //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //        $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //        $" | IP {GetObj_File.Device_IP[idx]} | GPIO Input Read Sucess | 0x{inputData[idx - 1]:X4}");
                    //}
                    
                }

                if (Fas_Motion.GetOutput(idx, out outputData[idx - 1]))
                {
                    //상위 비트 MSB 데이터 입니다.
                    outputData[idx - 1] = outputData[idx - 1] >> 16;

                    for (int i = 0; i < 16; i++)
                    {
                        bool bON = ((outputData[idx - 1] & (0x01 << i)) != 0);
                        Fas_Data.lstIO_OutState[idx - 1][i] = bON;
                        //Console.WriteLine("Input bit {0} is {1}.", i, ((bON) ? "ON" : "OFF"));
                    }

                    //DEBUG
                    //if (idx == 4)
                    //{
                    //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //     $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //     $" | IP {GetObj_File.Device_IP[idx]} | GPIO Output Read Sucess | 0x{Fas_Data.usOutputState[idx - 1]:X4}");
                    //}

                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //   $" | IP {GetObj_File.Device_IP[idx]} | GPIO Output Read Sucess | 0x{outputData[idx - 1]:X4}");

                    //if (Fas_Data.usOutputState[idx - 1] != outputData[idx - 1])
                    //{
                    //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //   $" | IP {GetObj_File.Device_IP[idx]} | GPIO Output Read Sucess | 0x{Fas_Data.usOutputState[idx - 1]:X4}");
                    //
                    //    Fas_Data.usOutputState[idx - 1] = outputData[idx - 1];
                    //
                    //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //    $" | IP {GetObj_File.Device_IP[idx]} | GPIO Output Read Sucess | 0x{Fas_Data.usOutputState[idx - 1]:X4}");
                    //}
                    
                }

            }

            //fasTechData = _item;
            //Console.WriteLine($"{methodName} Success");


            //FASTECH IO Monitor Debug
            //string strBitView = Convert.ToString(dwInStatus, 2).PadLeft(32,'0');            
            //string strBitViewUser = null;
            //for (int i = 0; i < strBitView.Length; i++)
            //{
            //    if (i % 4 == 3 && i < strBitView.Length -1)
            //    {
            //        strBitViewUser += strBitView[i] + ",";
            //    }
            //    else
            //    {
            //        strBitViewUser += strBitView[i];
            //    }                
            //}
            //Console.WriteLine($"{methodName} dwInStatus : {strBitViewUser}");

            //strBitView = Convert.ToString(dwOutStatus, 2).PadLeft(16, '0');
            //strBitViewUser = null;
            //for (int i = 0; i < strBitView.Length; i++)
            //{
            //    if (i % 4 == 3 && i < strBitView.Length - 1)
            //    {
            //        strBitViewUser += strBitView[i] + ",";
            //    }
            //    else
            //    {
            //        strBitViewUser += strBitView[i];
            //    }
            //}
            //Console.WriteLine($"{methodName} dwOutStatus : {strBitViewUser}");

            #region MyRegion
            //if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Console.WriteLine($"DEBUG ::: Board Number : {nBdID} Function(FAS_GetAxisStatus) was failed.");
            //}

            //if (EziMOTIONPlusELib.FAS_GetIOAxisStatus(nBdID, ref dwInStatus, ref dwOutStatus, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Error
            //}
            //if (EziMOTIONPlusELib.FAS_GetCommandPos(nBdID, ref _item.iCmdPos) != EziMOTIONPlusELib.FMM_OK)
            //{ 
            //    //Error
            //}
            //
            //if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref _item.iActPos) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Error
            //}
            //
            //if (EziMOTIONPlusELib.FAS_GetActualVel(nBdID, ref _item.iActVel) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Error
            //}           
            //
            //if (EziMOTIONPlusELib.FAS_GetPosError(nBdID, ref _item.iPosError) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    //Error
            //}            

            //2022.12.02 ::: 이값이 틀어지면은 팔로윙 에러 입니다.
            //_item.iPosError = (_item.iCmdPos) - (_item.iActPos);
            //_item.iPosError = Math.Abs(_item.iCmdPos) - Math.Abs(_item.iActPos);

            //Console.WriteLine($"{methodName} iCmdPos {_item.iCmdPos}");
            //Console.WriteLine($"{methodName} iActPos {_item.iActPos}");
            //Console.WriteLine($"{methodName} iActVel {_item.iActVel}");
            //Console.WriteLine($"{methodName} iPosError {_item.iPosError}"); 
            #endregion
        }

        /// <summary>
        /// 파스텍 모션 아이오 접속 확인
        /// </summary>
        /// <param name="IsConnected"></param>
        void Get_Fastech_Lost(ref bool IsConnected)
        {
            for (int i = 0; i < GetObj_File.Device_IP.Count; i++)
            {
                IsConnected = Fas_Motion.IsConnLost(i, GetObj_File.Device_IP[i]);
            }
                        
            if (IsConnected == false)
            {

            }
        }

        enum EBus_Func
        { 
            ManualMode=1, AutoMode=2, None_3, None_4, None_5, None_6, None_7, None_8, None_9,
            None_10, None_11, None_12, None_13, AutoClear=14, RecipeDownload=16
        }

        /// <summary>
        /// IO Function
        /// 
        /// </summary>
        void Adaptive_GPIO()
        {
            if (adapterCore == null)
                return;


            //2023.03.08 :::스위치 LED 기능
            bool[] OutStateBuff = null;
            bool[] InStateBuff = null;

          
            //2023.05.01
            //OutStateBuff[2] = stepModuleCore.mPlaceUnload.IsCurrSensor;
            //2023.05.10 ::: 디버그 아이오 체크 아닌 경우에 동작 하기 위함.
            if (devi.Define.IsXaxisDebugMove == false)
            {
                OutStateBuff = Fas_Data.lstIO_OutState[CHEFY];
                for (int idx = 0; idx < stepModuleCore.mLoders.Length; idx++)
                {
                    if (stepModuleCore.mLoders[idx] != null)
                    {
                        if (stepModuleCore.mLoders[idx].IsStartLatch == true)
                        {
                            if (DateTime.Now.Ticks >= stepModuleCore.mLoders[idx].dateTimeBlinkTime.Ticks)
                            {
                                stepModuleCore.mLoders[idx].dateTimeBlinkTime = DateTime.Now.AddMilliseconds(500);
                                OutStateBuff[idx] = !OutStateBuff[idx];
                            }
                        }
                        else
                        {
                            OutStateBuff[idx] = Fas_Data.lstIO_InState[CHEFY][idx];
                        }
                    }
                }


                for (int idx = 0; idx < stepModuleCore.mLoders.Length; idx++)
                {
                    if (stepModuleCore.mLoders[idx].IsLoader == false)
                    {
                        OutStateBuff[idx] = stepModuleCore.mLoders[idx].IsCurrSensor;
                    }
                }
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_2, OutStateBuff);
            }
            

            var cobotExCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 128);
            var cobotExComplte = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 144);

            //2023.03.10 ::: 파스텍 모터 원점 변수 재선언
            InStateBuff = Cores.Fas_Data.lstIO_InState[CHEFX];
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
            Fas_Data.IsOrgRetOk = Fas_Data.lstAxis_State[(int)EAxis_Status.Org_Ret_OK];
            Fas_Data.IsOrgRetOk &= !Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm];
            Fas_Data.IsOrgRetOk &= !OutStateBuff[(int)Core_StepModule.CHEFX_OUTPUT.XaxisPower_CP];
            
            //2023.03.13 ::: AutoClear Reset
            if(((cobotExCommand.iData >> 13) & 1) != 0 && ((cobotExComplte.iData >> 13) & 1) != 0)
            {
                GetObj_File.iAutoClear = 0;
            }      

            if (((cobotExCommand.iData >> 15) & 1) != 0 && ((cobotExComplte.iData >> 15) & 1) != 0)
            {
                GetObj_File.iSyncModbus = 0;
            }         
            else
            {
                GetObj_File.iSyncModbus = 1;
            }

            //2023.03.13 ::: RecipeDownload Reset
            if (((cobotExCommand.iData >> 14) & 1) != 0 && ((cobotExComplte.iData >> 14) & 1) != 0)
            {
                GetObj_File.iRecipeDownload = 0;
            }

            //2023.03.13 ::: 비상정지시 로봇 출력 끄기            
            var cobotManualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);
            var cobotManualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);

            //var cobotAutoPos

            InStateBuff = Fas_Data.lstIO_InState[CHEFY];
            OutStateBuff = Fas_Data.lstIO_OutState[COBOT];

            //이머전시 발생시.
            bool IsEmergency = true;
            IsEmergency &= !Fas_Data.lstIO_InState[CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_EMC_SW];
            IsEmergency &= !Fas_Data.lstIO_InState[CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_EMC_SW];
            IsEmergency &= Fas_Data.lstIO_InState[COBOT][(int)Core_StepModule.COBOT_INPUT.EMC_1];
            IsEmergency &= Fas_Data.lstIO_InState[COBOT][(int)Core_StepModule.COBOT_INPUT.EMC_2];

            //if (InStateBuff[(int)Core_StepModule.CHEFY_INPUT.L_EMC_SW] || InStateBuff[(int)Core_StepModule.CHEFY_INPUT.R_EMC_SW])
            if(IsEmergency == false)
            {

                bool IsAllOff = true;
                for (int i = 0; i < 16; i++)
                {
                    if (OutStateBuff[i] == true)
                    {
                        IsAllOff = false;
                        break;
                    }
                }

                if (IsAllOff == false)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        OutStateBuff[i] = false;
                    }
                    Cores.Fas_Motion.SetOutput((int)Core_StepModule.IO_Board.EzEtherNetIO_4, OutStateBuff);
                }

                if (Define.IsXaxisDebugMove == false)
                {
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        Cores.Fas_Motion.SetServoOn(MOTOR, 0);
                    }
                }
               

                if (cobotManualCommand.iData != 0)
                {
                    core_Robot.Modbus_Sender(129, 0);
                }

                //2023.05.22 ::: 화면 일시정지 출력이 나가고 있는 상황에서 출력 끄기
                if (Cores.Fas_Data.lstIO_OutState[CHEFY][(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause] == true
                    || Cores.Fas_Data.lstIO_OutState[CHEFY][(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause_Spare] == true)
                {
                    OutStateBuff = Fas_Data.lstIO_OutState[CHEFY];
                    OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause] = false;
                    OutStateBuff[(int)Cores.Core_StepModule.CHEFY_OUTPUT.Pause_Spare] = false;
                    Cores.Fas_Motion.SetOutput((int)Core_StepModule.IO_Board.EzEtherNetIO_2, OutStateBuff);
                }
            }

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            OutStateBuff = Fas_Data.lstIO_OutState[COBOT];
            if (cobotState == RobotState_Ver_1_1.STANDALONE_RUNNING)
            {
                if (OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset1] == true && OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset2] == true)
                {
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset1] = false;
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset2] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
               $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset1} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset1]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Cobot_Reset2} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Cobot_Reset2]}");
                }
            }

            //2023.03.20 ::: 파스텍 모션 접속 에러 인경우 알람 필요


            //2023.04.06 ::: 레이져 스캐너 사용일 경우
            LaserScannerHold();

            //2023.04.25 ::: 레이져 스캐너 사용유무에 따라 IO 사용 유무 출력
            iCurrLaserScannerUse = GetObj_File.iLaserScannerUse;
            if (iCurrLaserScannerUse == 1 && iPrevLaserScannerUse == 0)
            {
                //사용 할때
                OutStateBuff = Fas_Data.lstIO_OutState[CHEFZ];
                OutStateBuff[(int)Core_StepModule.CHEFZ_OUTPUT.SafetyController_UnUsed] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, OutStateBuff);

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
             $" | ROBOT : {cobotState} {CHEFZ_OUTPUT.SafetyController_UnUsed} | {OutStateBuff[(int)CHEFZ_OUTPUT.SafetyController_UnUsed]}");
            }
            if (iCurrLaserScannerUse == 0 && iPrevLaserScannerUse == 1)
            {
                //사용 안 할때
                OutStateBuff = Fas_Data.lstIO_OutState[CHEFZ];
                OutStateBuff[(int)Core_StepModule.CHEFZ_OUTPUT.SafetyController_UnUsed] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, OutStateBuff);

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
      $" | ROBOT : {cobotState} {CHEFZ_OUTPUT.SafetyController_UnUsed} | {OutStateBuff[(int)CHEFZ_OUTPUT.SafetyController_UnUsed]}");
            }

            iPrevLaserScannerUse = iCurrLaserScannerUse;
            
            //커맨드 출력 막기 인터락
            if (processCore.eCurrState != ECurrState.CurrentState_Excute &&
                Fas_Data.lstIO_OutState[COBOT][(int)Core_StepModule.COBOT_OUTPUT_COLL.Move_Cmd] == true
                && devi.Define.IsXaxisDebugMove == false)
            {
                OutStateBuff = Fas_Data.lstIO_OutState[COBOT];
                OutStateBuff[(int)Core_StepModule.COBOT_OUTPUT_COLL.Move_Cmd] = false;
                Cores.Fas_Motion.SetOutput((int)Core_StepModule.IO_Board.EzEtherNetIO_4, OutStateBuff);

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
              $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.Move_Cmd} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd]}");
            }

            Collaborativ_Hand();         
        }

        /// <summary>
        /// 2023.04.06 ::: 레이져 스캐너 홀드 기능
        /// </summary>
        void LaserScannerHold()
        {
            if (GetObj_File.iLaserScannerUse == 0)
                return;

            if (processCore.eCurrState == ECurrState.CurrentState_Resetting)
                return;

            if (processCore.eCurrState == ECurrState.CurrentState_Clearing)
                return;

            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];

            IsCurrProtectArea = Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area];
            IsCurrWarningArea = Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Waring_area];           

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;         


            if (IsCurrProtectArea == true && cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.INTERRUPTED)
            {
                Thread.Sleep(100);
                OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);                
                IsResumePolice = true;


            }
            else if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true
                && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true)
            {
                Thread.Sleep(100);
                OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }
            else  if (IsCurrProtectArea == true && cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY
                && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == true && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == true
                && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false
                && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == false
                && OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == true
                && IsResumePolice == true)
            {
                Thread.Sleep(100);
                OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }
            else if (cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true)
            {
                OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }
            else if (cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY
                && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true
                )
            {
                Thread.Sleep(100);
                OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);           
           
            }

            if (cobotState == RobotState_Ver_1_1.STANDALONE_RUNNING
                && IsResumePolice == true)
            {
                IsResumePolice = false;
            }

            //별도
            if (cobotState == Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF
               && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true)
            {
                OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }

            if (OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == false && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == false
              && OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] == true)
            {
                OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }

            LaserRobotCurrState = cobotState;  

            if (IsCurrWarningArea == false)
            {
                Cores.Core_Object.GetObj_File.iCobotSpeed = 50;
                Cores.Core_Object.GetObj_File.iXaxisSpeed = 50;
                Modbus_Sender(143, Cores.Core_Object.GetObj_File.iCobotSpeed);
            }
            else if (IsCurrWarningArea == true)
            {
                Cores.Core_Object.GetObj_File.iCobotSpeed = 100;
                Cores.Core_Object.GetObj_File.iXaxisSpeed = 100;
                Modbus_Sender(143, Cores.Core_Object.GetObj_File.iCobotSpeed);
            }     

            IsPrevProtectArea = IsCurrProtectArea;
            IsPrevWarningArea = IsCurrWarningArea;

        }

        /// <summary>
        /// 2023.05.03 ::: 협동작업구역내의 얼라인바 옵셋 하기 위한 제어
        /// </summary>
        void Collaborativ_Hand()
        {
            //2023.05.09 ::: 얼라인 사용유무에 대해서 인터락 할지 말지 고민 해보자.


            //1. 협동
            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            bool[] OutStateBuff = null;
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];


            if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.HANDGUIDING_CONTROL_STANDBY)
            {

                if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable] == true && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start] == true)
                {
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable] = false;
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.HGC_Enable} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.HGC_Start} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start]}");
                }
            }

            if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
            {

                if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp] == true && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End] == true)
                {
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp] = false;
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                      $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.HGC_Cmp} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp]}");

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | ROBOT : {cobotState} {COBOT_OUTPUT_COLL.HGC_End} | {OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End]}");
                }
            }

            SemiRobotCurrState = cobotState;
        }    

        //const byte STX = 2;
        //const byte DEV = 5;
        const byte DUM = 0;
        const byte LEN = 6;
        const byte SLV = 255;
        const byte DAT = 1;

        /// <summary>
        /// 2023.02.10 모드버스 데이터 동기화, ALL
        /// </summary>
        void Modbus_Sender()
        {
            foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            {
                //Trans id 2byte, Protocal id 2byte(Modbus TCP : 0), Length 2byte, slave address 1byte, fucntion code 1byte, data 2byte)

                if (dat.IsUsed)
                {
                    byte[] paket = null;


                    if (dat.Address == 143)
                    {
                        if (dat.iData != GetObj_File.iCobotSpeed)
                        {
                            dat.iData = GetObj_File.iCobotSpeed;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 128)
                    {

                        int iBuff = 0;                   

                        int[] iData = new int[16];
                        //iData[00] = GetObj_File.iManualMode;
                        //iData[01] = GetObj_File.iAutoMode;
                        iData[00] = 0;
                        iData[01] = 0;
                        iData[02] = GetObj_File.iBasketShaking;
                        iData[03] = GetObj_File.iGripperUse;
                        iData[04] = GetObj_File.iOilDrainUse;
                        iData[05] = GetObj_File.iTechingUse;
                        iData[06] = GetObj_File.iTechingInt;

                        iData[12] = GetObj_File.iXaxisControl;                        
                        iData[13] = GetObj_File.iAutoClear;
                        iData[14] = GetObj_File.iRecipeDownload;
                        iData[15] = GetObj_File.iSyncModbus;
                        

                        for (int i = 0; i < 16; i++)
                        {
                            if (iData[i] == 1)
                            {
                                iBuff |= iBit[i];
                            }
                            else
                            {
                                iBuff &= 0xffff - iBit[i];
                            }
                        }

                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 130)
                    {
                        //바스켓 무게:: 1800, 2800 3300, 3800, 4300, 3400, 4000, 4600
                        int iBuff = 3800;
                        iBuff = (int)((GetObj_File.dBasketWeight) * 1000);
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 132)
                    {
                        //엑스축 포지션 위치 데이터
                        int iBuff = Fas_Data.iActPos;                       
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 141)
                    {
                        //드레인 카운트                        
                        int iBuff = 0;
                        iBuff = GetObj_File.iOilDrainCount;
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 142)
                    {
                        //바스켓 흔들기 카운트                        
                        int iBuff = 0;
                        iBuff = GetObj_File.iBasketShakingCount;
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 139)
                    {
                        //조리중 바스켓 흔들기 개수                      
                        int iBuff = 0;
                        iBuff = GetObj_File.iMidShakingCount;
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else if (dat.Address == 140)
                    {
                        //조리중 바스켓 산소 입히기 시간                  
                        int iBuff = 0;
                        iBuff = GetObj_File.iMidOtwoShower;
                        if (dat.iData != iBuff)
                        {
                            dat.iData = iBuff;
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)Robot_Modbus_Table.Func.Write_Register, (byte)(dat.Address >> 8), (byte)dat.Address, (byte)(dat.iData >> 8), (byte)dat.iData };
                        }
                        else
                        {
                            paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        }
                    }
                    else
                    {
                        paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                    }

                    //TEST                    
                    //string log = null;
                    //foreach (byte buf in paket)
                    //{
                    //    log += $"0x{buf:X2} ";
                    //}                    
                    //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {dat.Address:000} {dat.strDesc} [{dat.ptDecimal}]");
                    if (paket != null)
                    {
                        core_Robot.SendMeassage(paket);
                    }
                  

                    //if (dat.Address == 0)
                    //{
                    //    core_Robot.SendMeassage(paket);
                    //}
                }
            }

            //Console.WriteLine();
        }

        /// <summary>
        /// 디지털 출력, 툴 출력
        /// </summary>
        /// <param name="manu"></param>
        /// <param name="bitNumber"></param>
        public void Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write manu, int bitNumber)
        {
            int address = -1;
            int writeAddress = -1;
            if (manu == Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO)
            {
                address = 22; //read address
                var item = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == address);

                int buff = 1;
                int dat = item.iData >> (bitNumber - 1);
                dat = (~dat) & 1;
                
                writeAddress = 38 + bitNumber - 1;
                byte[] paket = new byte[] { (byte)(writeAddress >> 8), (byte)writeAddress, 
                    DUM, DUM,
                    DUM, LEN,
                    SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Coil, 
                    (byte)(writeAddress >> 8), (byte)writeAddress,
                    Externs.Robot_Modbus_Table.SetBit(dat) , DUM };


                //TEST                
                //string log = null;
                //foreach (byte buf in paket)
                //{
                //    log += $"0x{buf:X2} ";
                //}
                //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log} {writeAddress} {dat}");
                

                core_Robot.SendMeassage(paket);
            }

            if (manu == Externs.Robot_Modbus_Table.Robot_Write.GPIO)
            {
                address = 1;
                var item = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == address);

                int dat = item.iData >> (bitNumber - 1);
                dat = (~dat) & 1;
                writeAddress = 16 + bitNumber - 1;
                byte[] paket = new byte[] { (byte)(writeAddress >> 8), (byte)writeAddress,
                    DUM, DUM,
                    DUM, LEN,
                    SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Coil,
                    (byte)(writeAddress >> 8), (byte)writeAddress,
                    Externs.Robot_Modbus_Table.SetBit(dat) , DUM };

                //TEST                
                //string log = null;
                //foreach (byte buf in paket)
                //{
                //    log += $"0x{buf:X2} ";
                //}
                //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log} {writeAddress} {dat}");

                core_Robot.SendMeassage(paket);
            }
         
        }

        /// <summary>
        /// 16bit 레지스터 128 ~ 255까지 
        /// </summary>
        /// <param name="regNumber"></param>
        /// <param name="data"></param>
        public void Modbus_Sender(int regNumber, int data)
        {
          
            byte[] paket = new byte[] { (byte)(regNumber >> 8), (byte)regNumber, DUM, DUM, DUM, LEN, SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Register, (byte)(regNumber >> 8), (byte)regNumber, (byte)(data >> 8), (byte)data };

            //TEST
            //string log = null;
            //foreach (byte buf in paket)
            //{
            //    log += $"0x{buf:X2} ";
            //}
            //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log} {number}");

            core_Robot.SendMeassage(paket);
        }
        

        /// <summary>
        /// 수동 1개만
        /// </summary>
        /// <param name="regNumber"></param>
        public void Modbus_Sender(int regNumber)
        {
            foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            {
                //Trans id 2byte, Protocal id 2byte(Modbus TCP : 0), Length 2byte, slave address 1byte, fucntion code 1byte, data 2byte)
                if (dat.IsUsed)
                {
                    if (dat.Address == regNumber)
                    {
                        byte[] paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };
                        //TEST                    
                        //string log = null;
                        //foreach (byte buf in paket)
                        //{
                        //    log += $"0x{buf:X2} ";
                        //}
                        //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {dat.Address:000} {dat.strDesc} [{dat.ptDecimal}]");
                        //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log}");
                        core_Robot.SendMeassage(paket);
                        break;
                    }
                }
            }

            //Console.WriteLine();
        }

        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private System.Runtime.InteropServices.SafeHandle _safeHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);
        
        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();

                    alarmCore.Dispose();  
                    processCore.Dispose();
                    stepModuleCore = null;
                    adapterCore = null;
                    core_Robot.Dispose();
                }

                _disposedValue = true;
            }
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (_disposed)
        //    {
        //        return;
        //    }
        //
        //    if (disposing)
        //    {
        //        // TODO: dispose managed state (managed objects).
        //    }
        //
        //    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        //    // TODO: set large fields to null.
        //
        //    _disposed = true;
        //}

    }//class
}//name
