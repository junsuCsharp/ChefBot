using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Threading;
using System.Collections.Concurrent;
using devi;

namespace Cores
{
    /*
     * 2023.01.06
     * 
     * 00. Index : CurrentState_Action
     * 01. Sub   : CurrentState_Aborted_Action
     * 02. Sub   : CurrentState_Aborting_Action
     * 03. Sub   : CurrentState_Clearing_Action
     * 04. Sub   : CurrentState_Complete_Action
     * 05. Sub   : CurrentState_Completing_Action
     * 06. Sub   : CurrentState_Excute_Action
     * 07. Sub   : CurrentState_Hold_Action
     * 08. Sub   : CurrentState_Holding_Action
     * 09. Sub   : CurrentState_Idle_Action
     * 10. Sub   : CurrentState_Resetting_Action
     * 11. Sub   : CurrentState_Starting_Action
     * 12. Sub   : CurrentState_Stopped_Action
     * 13. Sub   : CurrentState_Stopping_Action
     * 14. Sub   : CurrentState_Suspended_Action
     * 15. Sub   : CurrentState_Suspending_Action
     * 16. Sub   : CurrentState_Unholding_Action
     * 17. Sub   : CurrentState_Unsuspend_Action
     * 
     * 
     * 
     * 
     * 
     * 2023.02.18
     * 항상 자동 레디 상태를 만들로 (IDLE) 스타트에서 에서 EXCUTE >> 자동 시작 되게 한다.
     * 
     * Idle_Action
     * Excute_Action
     * Starting_Action
     * Stopping_Action
     * 
     * 
     * 2023.03.06
     * - 그립상태 보고 에러 처리 해야 한다.
     * 2023.03.14
     * - 비상정지 발생시 재시작 처리
     * 
     * 
     * 
     * //2023.05.03 ::: 처음 배출 센서 감지시 사운드 없애기 용도
     * 
     * 
     * 
     * 
     * 
     */


    public class Core_StepModule
    {
        bool IsFirstInit = false; // 제일 처음 생성 될때,


        //디버그 및 초기화 하기 위한 변수        
        bool IsCobortPass = true;//starting action cobot pass
        
       
        bool IsDebugMove = false; //강제 디버깅 로봇 무빙하면서,

        ////2023.04.17 ::: Robot Debug Mode
        //bool IsCobotDebugMove = true;//resetting action cobot pass, 다트 플랫폼 등등 사용 할때
        ////===> Define 으로 이동

        public Core_StepModule()
        {
            for (int idx = 0; idx < (int)EFlag.Max; idx++)
            {
                iCurrFlagArray[idx] = 0;
                iPrevFlagArray[idx] = 0;
                dtFlagArray[idx] = DateTime.Now;
                tsFlagArray[idx] = new TimeSpan();
                IsBuffFlagArray[idx] = false;
            }
        }
       
        //로그 큐
        public ConcurrentQueue<Core_Data.LogData> GetLogs = new ConcurrentQueue<Core_Data.LogData>();
        public ConcurrentQueue<Core_Data.ChickenCounter> GC_Chiken_Logs = new ConcurrentQueue<Core_Data.ChickenCounter>();

        //Process Action Loading Module      
        Externs.Action_Loading actLoad;     
        string strDesc = null;//로딩 설명

        //Soft Hold used
        bool IsTaskPause = false;
        bool IsTaskResume = false;


        //2023.05.03 flag & timelap///////////////////////////////////
        enum EFlag
        { 
            Starting, Excute, Stopping, Resetting, Clearing, Max
        }

        int[] iCurrFlagArray = new int[(int)EFlag.Max];
        int[] iPrevFlagArray = new int[(int)EFlag.Max];

        DateTime[] dtFlagArray = new DateTime[(int)EFlag.Max];
        TimeSpan[] tsFlagArray = new TimeSpan[(int)EFlag.Max];
        bool[] IsBuffFlagArray = new bool[(int)EFlag.Max];
        //////////////////////////////////////////////////////////////

        int iStartingFlag = 101;
        int iPrevStartingFlag = -1;
        Stopwatch swStartingTime = new Stopwatch();

        int iExcuteFlag = 101;
        int iPrevExcuteFlag = -1;      
        Stopwatch swExcuteTime = new Stopwatch();


        int iStoppingFlag = 101;
        int iPrevStoppingFlag = -1;
        Stopwatch swStoppingTime = new Stopwatch();
        DateTime dtStoppingTime = DateTime.Now;
        TimeSpan tsStoppingTime = new TimeSpan(0, 0, 0);

        int iResettingFlag = 101;
        int iPrevResettingFlag = -1;
        Stopwatch swResettingTime = new Stopwatch();
        DateTime dtResettingTime = DateTime.Now;
        TimeSpan tsResettingTime = new TimeSpan(0, 0, 0);

        int iClearingFlag = 101;
        int iPrevClearingFlag = -1;
        Stopwatch swClearingTime = new Stopwatch();
        DateTime dtClearingTime = DateTime.Now;
        TimeSpan tsClearingTime = new TimeSpan(0, 0, 0);


        //Operation
        public Stopwatch swPauseTimer = new Stopwatch();
        public List<int> lstOilCheckdCount = new List<int>() { 0, 0, 0 };
        public LoderModule[] mLoders = new LoderModule[3];
        public List<ChikenModule> mChiken = new List<ChikenModule>() { new ChikenModule(), new ChikenModule(), new ChikenModule(), new ChikenModule(), new ChikenModule(), new ChikenModule() };
          

        //로봇의 위치 제어 및 엑스축 위치 이동 명령
        MyActionStepBuffer myActionPicknPlace = MyActionStepBuffer.None;
        MyActionXStepBuffer myActionXaxisMove = MyActionXStepBuffer.None;
        MySwitchBuffer myActionSwitch = MySwitchBuffer.None;
        const int XmoveOffset = 5;
        int iSeletedCooker = 0;
        MyActionCobotStepBuffer myActionCobot = MyActionCobotStepBuffer.None;


        //2023.03.10 ::: 기름 빠지는 대기 변수
        public Stopwatch swOutDelayTime = new Stopwatch();
        //public int iOutDelaySetTime = 7;//sec

        //2023.03.10 ::: 바로 투입 이동 대기 변수
        public Stopwatch swInDelayTime = new Stopwatch();
        //public int iInDelaySetTime = 3;//sec

        //2023.03.13 ::: 바로 배출 이동 대기 변수
        public Stopwatch swOutStartDelayTime = new Stopwatch();

        //투입 먼저 실행할 것 번호 생성
        int[] iInputNumber = new int[2];

        //배출 먼저 실행할 것 번호 생성
        int[] iOutputNumber = new int[2];

        //엑스 축 오버라이드 스피드 엣지 적용
        int iCurrOverrideSpeed = 0;
        int iPrevOverrideSpeed = 0;

        public ResetAlarmEventHandler ResetAlarmEvent;
        public delegate void ResetAlarmEventHandler();

        public ModBusSendEventHandler ModBusSendEvent;
        public delegate void ModBusSendEventHandler(int address, int value);
     

        //2023.03.06 ::: 스테이트 변경될때, EActionStep 초기화 하기 위한 변수
        private PackML.ECurrState EBuffState = PackML.ECurrState.CurrentState_None;
        private EActionStep eActionStep_Number = EActionStep.Function_State_Number_IDLE;

        //2023.03.07 ::: 조리 우선순위 변수
        List<int> lstPriority = new List<int>() { 1, 3, 5, 2, 4, 6 };
        List<bool> lstPriorityUsed = new List<bool>() { false, false, false, false, false, false };

        //2023.03.08 ::: 배출 우선 순위 정렬 변수
        public List<Core_Data.SortBuffer> mCompBuffer = new List<Core_Data.SortBuffer>();

        //2023.03.08 ::: 투입된 우선 순위로 진행 할 변수
        public List<int> iFirstInputs = new List<int>();

        //2023.03.10 ::: 투입부 배출부 확인 변수
        bool IsOutputChiken = false;
        bool IsInsertChiken = false;
        //2023.05.08 ::: 대기위치에서 팔 굽히기 확인 변수
        bool IsGripArm = false;

        //2023.04.11 ::: 중간 흔들기 및 산소 입히기 확인 변수
        bool IsOxzenChieken = false;
        bool IsShakingChieken = false;

        //2023.03.11 ::: 자동운전의 일시정지 조건 맞추기 위한 변수
        static bool IsCobotMoveAction = false;

        //2023.03.13 ::: 홀등 가능할때 변경 하기
        bool IsHoldingPostive = false;

        //2023.04.06 ::: LaserScanner Warning Area Speed Control
        //public int iSpeed
        public bool IsCurrWarningArea = false;
        public bool IsPrevWarningArea = false;

        //2023.04.06 ::: Robot Debug variable
        public int iCurrRobotAutoStepNumver = 0;
        public int iPrevRobotAutoStepNumver = 0;      
       
        public event EventHandler Action_Complted;
        public delegate void EventHandler();

        public event RobotEventHandler Cobot_Command;
        public delegate void RobotEventHandler(int resNumber, int data);

        public const int MOTOR = 0;//파스텍 보드 번호
        public const int CHEFX = 0;//프로그램 아이오 번호, 전장류
        public const int CHEFY = 1;//프로그램 아이오 번호, 인터페이스
        public const int CHEFZ = 2;//프로그램 아이오 번호, 안전
        public const int COBOT = 3;//프로그램 아이오 번호, 로봇

      

        //20023.04.11
        public bool cooking_shake = false;
        public bool cooking_oxzen = false;

        //2023.05.08 ::: 로봇 그리퍼 대기위치 가기위한 조건 확인용
        public bool IsGripWaitSensorCheck = true;

        /// <summary>
        /// 2023.05.03
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="OnOff"></param>
        /// <param name="sec"></param>
        bool TimeLapsFlag(EFlag flag, bool OnOff, double sec)
        {
            bool retBool = false;           

            if (OnOff == true && IsBuffFlagArray[(int)flag] == false)
            {
                dtFlagArray[(int)flag] = DateTime.Now;
            }
            else if (dtFlagArray[(int)flag].Ticks >= DateTime.Now.Ticks)
            {
                dtFlagArray[(int)flag] = DateTime.Now;
            }
            IsBuffFlagArray[(int)flag] = OnOff;

            tsFlagArray[(int)flag] = DateTime.Now - dtFlagArray[(int)flag];
            if (tsFlagArray[(int)flag].TotalSeconds >= sec)
            {
                //dtFlagArray[(int)flag] = DateTime.Now;
                retBool = true;
            }
            return retBool; 
        }

        public enum MyActionStepBuffer
        {
            //투입 시작 Pick_Input
            //배출 시작 Pick_Output
            //배출 완료 Place_Output
            //투입 완료 Place_Input

            None, Pick_Input, Place_Input, Pick_Output, Place_Output, Shaking, Oxzen
        }

        public enum MyActionXStepBuffer
        {
            //투입 시작 Pick_Input
            //배출 시작 Pick_Output
            //배출 완료 Place_Output
            //투입 완료 Place_Input

            None, Wait, LoadA, LoadB, LoadC, Cooker1, Cooker2, Cooker3, Cooker4, Cooker5, Cooker6, Clearing = 12,Max
        }

        public enum MyActionCobotStepBuffer
        {
            None = 0, ShakingCom = 17, ShakingExt = 18, OxzenCom = 19, OxzenExt = 20
        }

        public enum MySwitchBuffer
        { 
            None, SwitchA, SwitchB
        }     

        public enum CHEFX_INPUT
        {
            DC24VSMPS_CP = 0, ControlUnit_CP = 1, RobotPower_CP=2, XaxisPower_CP=3,

            LeftDoorOpen = 14, RightDoorOpen = 15, Max = 16
        }

        public enum CHEFX_OUTPUT
        {
            XaxisPower_CP = 3, RobotPower_CP = 4, XaxisPower_CP_Spare = 6, RobotPower_CP_Spare = 7, RobotPower_CP_Demo = 12,
        }

        public enum CHEFY_INPUT
        {
            Load_A = 0, Load_B = 1, Load_C = 2, LoadSensor_A = 4, LoadSensor_B = 5, LoadSensor_C = 6,

            L_EMC_SW = 8, R_EMC_SW = 9, L_PAUSE_SW = 10, R_PAUSE_SW = 11, X_AXIS_EDM = 12,
        }

        public enum CHEFY_OUTPUT
        {
            Load_A_Lamp = 0, Load_B_Lamp = 1, Load_C_Lamp = 2, Pause = 10, Pause_Spare = 11,
        }

        public enum CHEFZ_INPUT
        {
            EMO_1_STOP = 0, EMO_2_STOP = 1, Scanner_error = 3, Scanner_alarm = 4, Scanner_OSSD = 5, Scanner_Protect_area = 6,

            Scanner_Waring_area = 7, Scanner_Operation= 8, SafetyController_Operation = 11, SafetyController_Power = 12,
            
            SafetyController_Wire = 13, SafetyController_Signal = 14
        }

        public enum CHEFZ_OUTPUT
        {
            SafetyController_Reset = 0x0b, SafetyController_UnUsed = 0x0c,
        }

        public enum COBOT_INPUT
        {
            EMC_1 = 0, EMC_2 = 1, ABNOMAL_1 = 2, ABNOMAL_2 = 3, SafeTouque = 4, SafeStop = 5, Remote = 6, Manual = 7,
            Tasking = 8, Motion = 9, Home = 10, None = 11, DefWork = 12, NorWork = 13, CokWork = 14, Comp = 15, Max = 16
        }

        public enum COBOT_INPUT_COLL
        {
            EMC = 0, Emc = 1, ABNOMAL = 2, Abnomal = 3, SafeTouque = 4, SafeStop = 5, Remote = 6, Manual = 7,
            Tasking = 8, Motion = 9, Home = 10, ENC_Init_Alarm = 11, DefWork = 12, NorWork = 13, CokWork = 14, Move_Cmp = 15, Max = 16
        }

        public enum COBOT_OUTPUT
        { 
            Remmote_On1 = 0, Remmote_On2 = 1, Cobot_Reset1=2, Cobot_Reset2 = 3, Task_Start = 4, Task_Pause = 5, Task_Stop = 6, Task_Resume = 7,
            Serovo_On = 8, Cobot_On = 9, Cobot_Off = 10, Move_Pos1 = 11, Move_Pos2 = 12, Move_Pos3 = 13, Move_Pos4 = 14, Move_Cmd = 15,
        }

        public enum COBOT_OUTPUT_COLL
        {
            Remmote_On1 = 0, Remmote_On2 = 1, Cobot_Reset1 = 2, Cobot_Reset2 = 3, Task_Start = 4, Task_Pause = 5, Task_Stop = 6, Task_Resume = 7,
            Serovo_On = 8, Move_Cmd = 9, HGC_End = 12, HGC_Cmp = 13, HGC_Enable = 14, HGC_Start =15, Max = 16 
        }

        public enum IO_Board
        {
           None, EzEtherNetIO_1, EzEtherNetIO_2, EzEtherNetIO_3, EzEtherNetIO_4, RoBot_Digital_IO, Max//RoBot_Tool_IO, RoBot_IO, Max
        }

        #region Process Action

        public enum EActionStep
        {
            //Action Step
            Function_State_Number_IDLE = 0,
            Function_State_Number_READY = 1,
            Function_State_Number_ACTION = 2,
            Function_State_Number_COMPLETE = 3,
        }

        private void CurrentState_Aborted_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Aborting_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Clearing_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {

            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    iClearingFlag = 0;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    if (Clear_Action())
                    {
                        eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                        iClearingFlag = 101;
                    }
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Complete_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Completing_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Excute_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    if (IsTaskPause && !IsTaskResume)
                    {

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                       $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                       $" | iPrevExcuteFlag : {iPrevExcuteFlag:000} | >>> | iExcuteFlag : {iExcuteFlag:000}");

                        iExcuteFlag = iPrevExcuteFlag;

                        IsTaskPause = false;
                        IsTaskResume = true;
                    }
                    else
                    {
                        iExcuteFlag = 0;
                    }
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    Excute_Action();
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    iExcuteFlag = 101;
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Hold_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    if (Hold_Action())
                    {
                        Action_Complted();
                        eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    }
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Holding_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Idle_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    ResetAlarmEvent();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Resetting_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    iResettingFlag = 0;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    if (Idle_Action())
                    {
                        eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                        iResettingFlag = 101;
                    }
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Starting_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    iStartingFlag = 0;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    if (Starting_Action())
                    {
                        eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                        iStartingFlag = 101;
                    }
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Stopped_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Stopping_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    iStoppingFlag = 0;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    if (Stopping_Action())
                    {
                        eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                        iStoppingFlag = 101;
                    }
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Suspended_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Suspending_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Unholding_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    if (UnHold_Action())
                    {
                        Action_Complted();
                        eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    }
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        private void CurrentState_Unsuspend_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (eActionStep_Number)
            {
                case EActionStep.Function_State_Number_IDLE:
                    eActionStep_Number = EActionStep.Function_State_Number_READY;
                    break;
                case EActionStep.Function_State_Number_READY:
                    eActionStep_Number = EActionStep.Function_State_Number_ACTION;
                    break;
                case EActionStep.Function_State_Number_ACTION:
                    eActionStep_Number = EActionStep.Function_State_Number_COMPLETE;
                    break;
                case EActionStep.Function_State_Number_COMPLETE:
                    Action_Complted();
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
                default:
                    eActionStep_Number = EActionStep.Function_State_Number_IDLE;
                    break;
            }
        }
        public void CurrentState_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {
            switch (state)
            {
                case PackML.ECurrState.CurrentState_Aborted:
                    CurrentState_Aborted_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Aborting:
                    CurrentState_Aborting_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Clearing:
                    CurrentState_Clearing_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Complete:
                    CurrentState_Complete_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Completing:
                    CurrentState_Completing_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Excute:
                    CurrentState_Excute_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Hold:
                    CurrentState_Hold_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Holding:
                    CurrentState_Holding_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Idle:
                    CurrentState_Idle_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Resetting:
                    CurrentState_Resetting_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Starting:
                    CurrentState_Starting_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Stopped:
                    CurrentState_Stopped_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Stopping:
                    CurrentState_Stopping_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Suspended:
                    CurrentState_Suspended_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Suspending:
                    CurrentState_Suspending_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Unholding:
                    CurrentState_Unholding_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_Unsuspend:
                    CurrentState_Unsuspend_Action(state, mode);
                    break;
                case PackML.ECurrState.CurrentState_None:
                    break;
            }
        } 
        #endregion

        /// <summary>
        /// 상시 프로세스
        /// 스위치 이벤트 및 로더 배출 감시
        /// </summary>
        /// <param name="state"></param>
        /// <param name="mode"></param>
        public void Contiuse_Action(PackML.ECurrState state, PackML.EModeMatrix mode)
        {

            if (Cores.Core_Object.IsIntanceCredite == false)
                return;

            //2023.06.14 ::: 에러처리

            if (state == PackML.ECurrState.CurrentState_Aborted)
            {
                if (actLoad == null || actLoad.IsDispose())
                {
                 
                }
                else
                {
                    actLoad.Close();
                }
            }
            

            bool[] OutStateBuff = null;
            bool[] InStateBuff = null;

            //X축 저속 모드
            X_Axis_Override(); 

            #region 초기화
            //2023.03.10 ::: 인스턴스 에러로 인하여 처음 한번은 생성
            if (IsFirstInit == false)
            {
                IsFirstInit = true;

                for (int idx = 0; idx < mChiken.Count; idx++)
                {
                    //mChiken[idx].cookerIndex = idx;
                    mChiken[idx].chickenState = Core_Data.EB_State.None;
                    mChiken[idx].chickenType = Core_Data.EChickenType.None;
                }

                mChiken[0].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
                mChiken[1].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
                mChiken[2].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
                mChiken[3].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
                mChiken[4].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
                mChiken[5].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
                //mLoders[0] = new LoderModule(Core_Data.EChickenType.None);
                //mLoders[1] = new LoderModule(Core_Data.EChickenType.None);
                //mPlaceUnload = new LoderModule(Core_Data.EChickenType.None);


                //2023.04.24 ::: 여기다가 로딩한 데이터 넣어야 한다.
                int loaderCount = 0;
                for (int idx = 0; idx < Project_Main.FormMain.gui_file.type.Length; idx++)
                {

                    switch (Project_Main.FormMain.gui_file.type[idx])
                    {
                        case devJace.Files.ModuleType.Load:
                            mLoders[loaderCount] = new LoderModule(Core_Data.EChickenType.None);
                            mLoders[loaderCount].IsLoader = true;
                            loaderCount++;
                            break;

                        case devJace.Files.ModuleType.UnLoad:
                            mLoders[loaderCount] = new LoderModule(Core_Data.EChickenType.None);
                            mLoders[loaderCount].IsLoader = false;
                            loaderCount++;
                            break;

                        case devJace.Files.ModuleType.Cooker:
                            break;

                        case devJace.Files.ModuleType.None:
                            break;
                    }
                   
                }

                OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];

            }

            //2023.03.09 ::: 에러처리
            if (EBuffState != state)
            {
                //IsResume = false;
                //if (EBuffState == PackML.ECurrState.CurrentState_Hold
                //    && state == PackML.ECurrState.CurrentState_Excute)
                //{
                //    IsResume = true;
                //}
                EBuffState = state;

                //Init
                eActionStep_Number = EActionStep.Function_State_Number_IDLE;
            }
            #endregion

            #region 디버그 / 소프트웨어 검증용 / 시작
            if (Define.IsDebugPass == true || IsDebugMove == true)
            {
                for (int idx = 0; idx < mLoders.Length; idx++)
                {
                    if (mLoders[idx].IsLoader)
                    {
                        mLoders[idx].IsStartLatch = true;
                    }
                    
                }

                //mLoders[0].IsStartLatch = true;
                //mLoders[1].IsStartLatch = true;
            } 
            #endregion           

            #region 상태부 / 중간 흔들기 확인 / 중간 산소 입히기 확인
            TimeSpan tsMiddleTimeBuff;
            tsMiddleTimeBuff = new TimeSpan(0,
                Cores.Core_Object.GetObj_File.iBasketShakingSetMinTime,
                Cores.Core_Object.GetObj_File.iBasketShakingSetSecTime);
            //2023.04.11 ::: 중간 작업 흔들기 / 산소 입히기
            IsShakingChieken = false;
            for (int idx = 0; idx < mChiken.Count; idx++)
            {
                //배출 확인
                if (mChiken[idx].IsCurrentShaking(tsMiddleTimeBuff) == true && IsOutputChiken == false
                    && mChiken[idx].IsLatchShakingChikenComplted == false)
                {
                    IsShakingChieken = true;
                    break;
                }
            }

            tsMiddleTimeBuff = new TimeSpan(0,
               Cores.Core_Object.GetObj_File.iBasketOxzenSetMinTime,
               Cores.Core_Object.GetObj_File.iBasketOxzenSetSecTime);
            IsOxzenChieken = false;
            for (int idx = 0; idx < mChiken.Count; idx++)
            {
                //배출 확인
                if (mChiken[idx].IsCurrentOxzening(tsMiddleTimeBuff) == true && IsOutputChiken == false
                    && mChiken[idx].IsLatchOxzenChikenComplted == false
                    && IsShakingChieken == false)
                {
                    IsOxzenChieken = true;
                    break;
                }
            }
            #endregion

            #region 로더부
            //로더부
            //투입센서, 투입버튼입력 확인

            if (Define.IsDebugPass == false && IsDebugMove == false)
            {
                IsGripWaitSensorCheck = true;
                for (int idx = 0; idx < mLoders.Length; idx++)
                {
                    //투입 스위치 눌리면 넣기
                    //mLoders[idx].IsStartLatch = true;

                    //투입 바스켓 센서 입력시 넣기
                    //mLoders[idx].IsInputSensor = true;

                    mLoders[idx].IsCurrSensor = Cores.Fas_Data.lstIO_InState[CHEFY][idx + Define.iSensorLocateOffsset];
                    mLoders[idx].IsCurrSwitch = Cores.Fas_Data.lstIO_InState[CHEFY][idx];//switch

                    if (mLoders[idx].IsCurrSwitch == true //switch
                        && mLoders[idx].IsCurrSensor == true
                        && mLoders[idx].IsPrevSwitch == false)
                    {
                        mLoders[idx].IsStartLatch = true;

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                         devJace.Program.ELogLevel.Info,
                         $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                         $" | mLoders[idx].IsStartLatch : [{mLoders[idx].IsStartLatch}]");
                    }
                    else if (mLoders[idx].IsCurrSensor == false
                        && mLoders[idx].IsStartLatch == true)
                    {
                        mLoders[idx].IsStartLatch = false;

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                       devJace.Program.ELogLevel.Info,
                       $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                       $" | mLoders[idx].IsStartLatch : [{mLoders[idx].IsStartLatch}]");
                    }
                    mLoders[idx].IsPrevSwitch = mLoders[idx].IsCurrSwitch;
                    mLoders[idx].IsPrevSensor = mLoders[idx].IsCurrSensor;
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    //  devJace.Program.ELogLevel.Debug,
                    //  $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //  $" | Cores.Fas_Data.lstIO_InState[CHEFY][idx]: [{Cores.Fas_Data.lstIO_InState[CHEFY][idx]}] // {idx}");

                    if (mLoders[idx].IsLoader == false)
                    {
                        mLoders[idx].IsCurrSensor = Cores.Fas_Data.lstIO_InState[CHEFY][idx + Define.iSensorLocateOffsset];
                    }
                    else
                    {
                        //센서 입력 신호 두개 가 안들어 오고 있어야....
                        IsGripWaitSensorCheck &= !mLoders[idx].IsStartLatch;
                    }
                }

            }

            #endregion

            #region 배출부
            //배출부
            //2023.03.08 ::: 배출 근접 센서 확인  
            //2023.03.07 ::: 투입 가능 여부 확인
            bool IsInsertChikenBuff = false;
            for (int idx = 0; idx < mChiken.Count; idx++)
            {
                if (mChiken[idx].chickenState == Core_Data.EB_State.None
                    && mChiken[idx].IsCookerUsed)
                {
                    IsInsertChikenBuff = true;
                    break;
                }
            }
            IsInsertChiken = IsInsertChikenBuff;
            //2023.03.08 ::: 배출 가능 여부 확인
            bool IsOutputChikenBuff = false;
            for (int idx = 0; idx < mChiken.Count; idx++)
            {
                //배출 확인
                if (mChiken[idx].IsCookingComplted() == true || mChiken[idx].IsExist == true)
                {
                    IsOutputChiken = true;
                    IsInsertChiken = false;
                    //if (mPlaceUnload.IsCurrSensor == true)
                    //{
                    //
                    //}
                    break;
                }
            }
            IsOutputChiken = IsOutputChikenBuff;

            //2023.05.03 ::: 처음 배출 센서 감지시 사운드 없애기 용도
            if (IsOutputChiken == true && Define.IsFirstSoundOff == false)
            {
                Define.IsFirstSoundOff = true;
            }
            //2023.03.08 ::: 타이머 시간이 가장 오래된 것 부터 배출 해야 한다.          
            mCompBuffer.Clear();
            double[] dTempCookTimeSort = new double[mChiken.Count];
            for (int idx = 0; idx < mChiken.Count; idx++)
            {
                Core_Data.SortBuffer sortBuf = new Core_Data.SortBuffer();

                sortBuf.dCookingTime = mChiken[idx].tsCurTime.TotalSeconds;
                sortBuf.iCookingIndex = idx;
                sortBuf.IsCookingComplted = mChiken[idx].IsCookingComplted();

                dTempCookTimeSort[idx] = sortBuf.dCookingTime;

                ////2023.04.07 ;;; 배출 지연시간 로그
                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                //devJace.Program.ELogLevel.Info,
                //$"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                //$" | Exit Delay Time : {sortBuf.iCookingIndex},{sortBuf.dCookingTime}");

                mCompBuffer.Add(sortBuf);
                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                //                  devJace.Program.ELogLevel.Debug,
                //                  $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                //                  $" | CdTempCookTime : [{idx}][{dTempCookTimeSort[idx]:0000}]");
            }
            double dTimeMax = dTempCookTimeSort.Max();
            mCompBuffer = mCompBuffer.OrderByDescending(x => x.dCookingTime).ToList();

            ////2023.03.10 ::: 배출 강제 명령
            //for (int idx = 0; idx < mChiken.Count; idx++)
            //{
            //    if (mChiken[idx].IsExist == true)
            //    { 
            //        
            //    }
            //}
            #endregion

         
        }

        /// <summary>
        /// 알람 해제 동작 시퀀스
        /// </summary>
        /// <returns></returns>
        bool Clear_Action()
        {

            if (actLoad == null || actLoad.IsDispose())
            {
                swClearingTime.Restart();
                actLoad = new Externs.Action_Loading();
                actLoad.Open();
            }


            bool retBool = false;

            bool[] OutStateBuff = null;
            bool[] InStateBuff = null;

            OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];

            var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
            var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);

            bool[] resetBuff = null;
            resetBuff = Cores.Fas_Data.lstIO_OutState[CHEFZ];

            bool[] axisPowerBuff = null;
            axisPowerBuff = Cores.Fas_Data.lstIO_InState[CHEFX];

            double iSpeed = 0;
            int fastechSpeed = 0;
            int fastechAbsPos = 0;

            //2023.03.08 ::: 로봇 툴 입력 확인
            var toolInputs = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);

            //2023.03.09 ::: 로봇 조인트 값 확인
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);
            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;
            actLoad.UpdateProgress($"<Clearing : {iClearingFlag}>  {strDesc} [{swClearingTime.ElapsedMilliseconds} ms] {cobotState}");


            //2023.06.14 ::: 에러 처리 필요
            //태스킹이 안되는 경우 케이스 32번 이상에서,,,

            switch (iClearingFlag)
            {
                case 0:
                    if (Define.IsCobotDebugMove == false)
                    {
                        iClearingFlag = 10;
                        //2023.04.25 ::: 비상정지로 인하여 원점 복귀 해제
                        Fas_Data.IsOrgRetOk = false;

                        if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF)
                        {
                            for (int i = 0; i < OutStateBuff.Length; i++)
                            {
                                OutStateBuff[i] = false;
                            }
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        }
                       

                        if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.BACKDRIVE_HOLD)
                        {
                            iClearingFlag = 100;
                        }
                    }
                    else
                    {
                        iClearingFlag = 100;
                    }
                    break;

                //비상정지 발생시 로봇 리셋 조건
                #region MyRegion                
                case 10:
                    if (resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] == false)
                    {
                        resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);

                        iClearingFlag = 11;
                    }
                    break;

                case 11:
                    if (resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] == true)
                    {
                        resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);
                        iClearingFlag = 12;
                    }
                    break;

                case 12://모드버스 초기화
                    ModBusSendEvent(129, 0);
                    ModBusSendEvent(133, 0);
                    iClearingFlag = 20;
                    break;
                #endregion

               
                case 20:                   
                    iClearingFlag = 21;
                    break;

                case 21:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false
                       && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 22;
                    }
                  
                    break;

                case 22:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true
                        && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 23;
                    }
                    break;

                case 23:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] == false
                        && OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 24;
                        Thread.Sleep(500);
                    }                 
                    break;

                case 24:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] == true
                        && OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == true)
                    {
                        iClearingFlag = 25;
                        Thread.Sleep(500);
                    }
                    else
                    {
                        iClearingFlag = 23;
                    }
                    break;

                case 25:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == false
                    && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 26;
                        Thread.Sleep(500);
                    }
                    break;

                case 26:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == true
                    && OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == true)
                    {
                        iClearingFlag = 27;
                    }
                    else
                    {
                        iClearingFlag = 21;
                    }
                    break;

                case 27:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 28;
                        dtClearingTime = DateTime.Now;
                    }
                    break;

                case 28:
                    var robotServoOn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 260);
                    bool IsRobotServoOn = Convert.ToBoolean(robotServoOn.iData);
                    if (IsRobotServoOn)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 29;
                    }

                    tsClearingTime = DateTime.Now - dtClearingTime;
                    if (dtClearingTime.Ticks >= DateTime.Now.Ticks)
                    {
                        dtClearingTime = DateTime.Now;
                    }
                    else if (tsClearingTime.TotalSeconds >= 15)
                    {
                        iClearingFlag = 0;
                    }                 
                    break;

                case 29:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] == false)
                    {
                        iClearingFlag = 30;
                    }
                    break;

                case 30:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 31;
                    }
                    break;

                case 31:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iClearingFlag = 32;
                    }
                    break;

                case 32:
                    if (InStateBuff[(int)COBOT_INPUT.Tasking] == false)
                    {
                        iClearingFlag = 33;
                    }
                    break;

                case 33:
                    if (Define.IsRobotReCoverlyMode == false)
                    {
                        if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                        {
                            iClearingFlag = 40;
                        }
                        else
                        {
                            iClearingFlag = 34;
                        }
                        //로봇 원점 추가 해야 함.
                    }
                    else
                    {
                        iClearingFlag = 100;
                    }
                    break;

                case 34:
                    ModBusSendEvent(129, 13);
                    iClearingFlag = 35;
                    Thread.Sleep(500);
                    break;

                case 35:
                    if (manualCommand.iData != 0 && manualComplted.iData == 0)
                    {
                        iClearingFlag = 36;
                    }
                    else
                    {
                        iClearingFlag = 34;
                    }
                    break;

                case 36:
                    if (manualCommand.iData != 0 && manualComplted.iData != 0)
                    {
                        ModBusSendEvent(129, 0);
                        iClearingFlag = 37;
                        Thread.Sleep(500);
                    }
                    else if (manualCommand.iData == 0 && manualComplted.iData == 0)
                    {
                        iClearingFlag = 37;
                    }

                    if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                    {
                        ModBusSendEvent(129, 0);
                        iClearingFlag = 40;
                    }
                    break;

                case 37:
                    if (manualCommand.iData == 0 && manualComplted.iData == 0)
                    {
                        if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                        {
                            iClearingFlag = 40;
                        }
                        else
                        {
                            iClearingFlag = 34;
                        }
                    }                  
                    break;

                case 38:
                    //if (resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] == false)
                    //{
                    //    resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] = true;
                    //    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);
                    //
                    //    iClearingFlag = 39;
                    //}

                    break;

                case 39:
                    //if (resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] == true)
                    //{
                    //    Thread.Sleep(500);
                    //    resetBuff[(int)CHEFZ_OUTPUT.SafetyController_Reset] = false;
                    //    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);
                    //    iClearingFlag = 40;
                    //}
                    break;


                case 40:
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        Cores.Fas_Motion.SetServoOn(MOTOR, 0);
                    }
                    iClearingFlag = 41;
                    break;

                case 41:
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false)
                    {
                        Cores.Fas_Motion.SetServoOn(MOTOR, 1);
                        iClearingFlag = 42;
                    }
                    break;

                case 42:
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {   
                        iClearingFlag = 43;
                    }
                    break;

                case 43:
                    //iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                    //fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)20);
                    //Cores.Fas_Motion.MovePos(MOTOR, 1, 0, (uint)fastechSpeed, 0);
                    Fas_Data.IsOrgRetOk = false;
                    iClearingFlag = 44;
                    break;

                case 44:
                    if (Fas_Data.IsOrgRetOk == false
                           && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        Cores.Fas_Motion.OriginSearch(MOTOR);
                        iClearingFlag = 45;
                    }
                    else if (Fas_Data.IsOrgRetOk == true
                           && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        Cores.Fas_Motion.OriginSearch(MOTOR);
                        iClearingFlag = 45;
                    }

                    //Console.WriteLine($"{Fas_Data.IsOrgRetOk}");
                    //Console.WriteLine($"{Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On]}");
                    break;

                case 45:
                    if (Fas_Data.IsOrgRetOk == true
                          && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        
                        iClearingFlag = 50;
                    }
                    break;

                case 46:
                    break;

                case 47:
                    break;

                case 48:
                    break;

                case 49:
                    break;

                case 50:
                    iClearingFlag = 100;
                    break;

                case 100:
                    Define.IsRobotReCoverlyMode = false;
                    actLoad.Close();
                    swClearingTime.Stop();
                    retBool = true;
                    break;

                default:
                    iClearingFlag = 0;
                    break;

            }

            if (iPrevClearingFlag != iClearingFlag)
            {
                iPrevClearingFlag = iClearingFlag;

                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                       devJace.Program.ELogLevel.Debug,
                       $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                       $" | FLAG : [{iClearingFlag:000}]");
            }


            return retBool;
        }

        /// <summary>
        /// 준비 동작 시퀀스
        /// </summary>
        /// <returns></returns>
        bool Idle_Action()
        {
            bool retBool = false;
            double iSpeed = 0;
            int fastechSpeed = 0;
            int fastechAbsPos = 0;

            //일반
            //EMC 눌렸을 경우
            //
            
            try
            {
                if (actLoad == null || actLoad.IsDispose())
                {
                    swResettingTime.Restart();
                    actLoad = new Externs.Action_Loading();
                    actLoad.Open();
                }

                if (Define.IsDebugPass)
                {
                    iResettingFlag = 100;
                }

                var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
                Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

                actLoad.UpdateProgress($"<Resetting : {iResettingFlag}>  {strDesc} [{swResettingTime.ElapsedMilliseconds} ms] {cobotState}");

                //var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
                var robotServoOn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 260);
                bool IsRobotServoOn = Convert.ToBoolean(robotServoOn.iData);


                bool[] OutStateBuff = null;
                bool[] InStateBuff = null;

                //Remmote_On1 = 0, Remmote_On2 = 1, Cobot_Reset1 = 2, Cobot_Reset2 = 3, Task_Start = 4, Task_Pause = 5, Task_Stop = 6, Task_Resume = 7,
                //Serovo_On = 8, Cobot_On = 9, Cobot_Off = 10, Move_Pos1 = 11, Move_Pos2 = 12, Move_Pos3 = 13, Move_Pos4 = 14, Move_Cmd = 15,

                bool IsAllOff = true;

                //labelRobotState.Text = $"Robot State ::: {(Externs.Robot_Modbus_Table.RobotState_Ver_1_1)accessData.iData}";
                //2023.03.09 ::: temp
                //if (swResettingTime.Elapsed.TotalSeconds >= 20)
                //{
                //    actLoad.Close();
                //    swStartingTime.Stop();
                //    retBool = true;
                //}

                //2023.03.20 ::: 로봇 태스크 좌표 값 확인
                var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
                var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
                var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
                var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
                var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
                var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
                double[] TaskPos = new double[6];
                TaskPos[0] = double.Parse(task1.strData);
                TaskPos[1] = double.Parse(task2.strData);
                TaskPos[2] = double.Parse(task3.strData);
                TaskPos[3] = double.Parse(task4.strData);
                TaskPos[4] = double.Parse(task5.strData);
                TaskPos[5] = double.Parse(task6.strData);


                switch (iResettingFlag)
                {
                    case 0:
                        //if (IsRobotServoOn && cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING)
                        //{
                        //    iResettingFlag = 50;
                        //}
                        //else if (IsRobotServoOn && cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY)
                        //{
                        //    iResettingFlag = 11;
                        //}
                        //else
                        //{
                        //    iResettingFlag = 1;
                        //}
                        strDesc = "로봇 상태 확인 중 입니다.";
                        if (Define.IsCobotDebugMove == false)
                        {
                            //
                            if (IsRobotServoOn &&
                                (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                                || cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.COLLABORATIVE_RUNNING))
                            {
                                iResettingFlag = 50;
                            }
                            else
                            {
                                

                                OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                                InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                                for (int i = 0; i < OutStateBuff.Length; i++)
                                {
                                    OutStateBuff[i] = false;
                                }
                                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                                //Thread.Sleep(500);
                                iResettingFlag = 1;
                            }
                        }
                        else
                        {
                            iResettingFlag = 50;
                        }
                        break;

                    case 1:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false &&
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            //Thread.Sleep(500);
                            iResettingFlag = 2;
                        }
                        break;

                    case 2:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true &&
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                            OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            //Thread.Sleep(500);
                            iResettingFlag = 3;
                            dtResettingTime = DateTime.Now;
                        }
                        break;

                    case 3:
                        //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        //OutStateBuff[(int)COBOT_OUTPUT.Cobot_On] = true;
                        //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Off] = false;
                        //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        //Thread.Sleep(500);
                        //OutStateBuff[(int)COBOT_OUTPUT.Cobot_On] = false;
                        //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Off] = false;
                        //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iResettingFlag = 4;
                        break;

                    case 4:
                        iResettingFlag = 5;
                        break;

                    case 5:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] == false &&
                            OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == false)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
                            OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            iResettingFlag = 6;
                        }
                        break;

                    case 6:
                        iResettingFlag = 7;
                        break;

                    case 7:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == false &&
                            OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == false)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] = true;
                            OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] = true;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            iResettingFlag = 8;
                        }
                        break;

                    case 8:
                        iResettingFlag = 9;
                        break;

                    case 9:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] == false)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = true;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            iResettingFlag = 10;
                        }
                        break;
              
                    case 10:
                        strDesc = "로봇 서보 온 확인 중 입니다.";
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (IsRobotServoOn)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = false;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            iResettingFlag = 11;
                        }
                        tsResettingTime = DateTime.Now - dtResettingTime;
                        if (dtResettingTime.Ticks >= DateTime.Now.Ticks)
                        {
                            dtResettingTime = DateTime.Now;
                        }
                        else if (tsResettingTime.TotalSeconds >= 15)
                        {
                            dtResettingTime = DateTime.Now;
                            iResettingFlag = 0;
                        }
                        //else if()
                        break;

                    case 11:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (InStateBuff[(int)COBOT_INPUT.Tasking] == true
                            && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == false)
                        {
                            OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            //Thread.Sleep(500);
                            iResettingFlag = 12;
                        }
                        else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false)
                        {
                            iResettingFlag = 12;
                        }

                        tsResettingTime = DateTime.Now - dtResettingTime;
                        if (dtResettingTime.Ticks >= DateTime.Now.Ticks)
                        {
                            dtResettingTime = DateTime.Now;
                        }
                        else if (tsResettingTime.TotalSeconds >= 5)
                        {
                            //
                            if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY)
                            {
                                iResettingFlag = 0;

                                //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                                //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                                //OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] = false;
                                //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                                Thread.Sleep(1000);
                            }
                        }
                        break;

                    case 12:
                        strDesc = "로봇 실행 중 입니다.";
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        if (InStateBuff[(int)COBOT_INPUT.Tasking] == false
                            && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                        {
                            //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                            //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                            OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                            //low active
                            iResettingFlag = 13;
                        }
                        else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false
                            && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == false)
                        {
                            //iResettingFlag = 0;
                        }
                        else if (InStateBuff[(int)COBOT_INPUT.Tasking] == true)
                        {
                            iResettingFlag = 11;
                        }
                        break;

                    case 13:
                        if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                            || cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iResettingFlag = 20;
                            }
                            else
                            {
                                //iResettingFlag = 14;
                            }
                        }
                      
                        break;

                    case 14:
                        //위치 값이 다른 경우???
                        break;

                    case 15:
                        break;

                    case 16:
                        break;

                    case 17:
                        break;

                    case 18:
                        break;

                    case 19:
                        break;

                    case 20:
                        iResettingFlag = 30;
                        break;

                    case 30:
                        iResettingFlag = 40;
                        break;                 

                    case 40:                    
                        iResettingFlag = 50;
                        break;

                    case 50:
                        //OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
                        //InStateBuff = Cores.Fas_Data.lstIO_InState[CHEFX];
                        //if (Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == true)
                        //{
                        //    OutStateBuff[0x0D] = true;
                        //    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_1, OutStateBuff);
                        //    Thread.Sleep(500);
                        //    iResettingFlag = 51;
                        //}
                        //else
                        //{
                        //    iResettingFlag = 52;
                        //}

                        iResettingFlag = 52;
                        break;

                    case 51:
                        //OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
                        //InStateBuff = Cores.Fas_Data.lstIO_InState[CHEFX];
                        //OutStateBuff[0x0D] = false;
                        //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_1, OutStateBuff);
                        //Thread.Sleep(200);
                        iResettingFlag = 52;
                        break;

                    case 52:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == false 
                            && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false)
                        {
                            Cores.Fas_Motion.SetServoOn(MOTOR, 1);
                            Thread.Sleep(200);
                        }
                        iResettingFlag = 53;
                        break;

                    case 53:
                        if (Fas_Data.IsOrgRetOk == false
                            && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                        {
                            Cores.Fas_Motion.OriginSearch(MOTOR);
                            iResettingFlag = 54;
                        }
                        else
                        {
                            iResettingFlag = 55;
                        }
                        Thread.Sleep(200);
                        break;

                    case 54:
                        if (Fas_Data.IsOrgRetOk == true)
                        {
                            iResettingFlag = 60;
                        }
                        else if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true)
                        {
                            iResettingFlag = 55;
                        }
                        break;

                    case 55:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                        {
                            Cores.Fas_Motion.SetServoOn(MOTOR, 0);
                            iResettingFlag = 56;
                        }
                        break;

                    case 56:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false)
                        {
                            Cores.Fas_Motion.SetServoOn(MOTOR, 1);
                            iResettingFlag = 57;
                        }
                        break;

                    case 57:
                        OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
                        InStateBuff = Cores.Fas_Data.lstIO_InState[CHEFX];
                        if (Fas_Data.IsOrgRetOk == false)
                        {
                            iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                            fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)50);
                            //Cores.Fas_Motion.JogMove(MOTOR, 0, 250/*가감속 기본값*/, fastechSpeed);

                            //Cores.Fas_Motion.MovePos(MOTOR, 1, 0, (uint)fastechSpeed, 0);
                            Cores.Fas_Motion.MovePos(MOTOR, 1, 0, (uint)fastechSpeed, 0, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);

                            iResettingFlag = 58;
                        }
                        else
                        {
                            iResettingFlag = 60;
                        }
                        break;

                    case 58:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                           && Fas_Data.lstAxis_State[(int)EAxis_Status.HW_Miuns_Limit] == true)
                        {
                            Cores.Fas_Motion.OriginSearch(MOTOR);
                            iResettingFlag = 59;
                        }
                        else
                        {
                            Cores.Fas_Motion.OriginSearch(MOTOR);
                            iResettingFlag = 59;
                        }
                        Thread.Sleep(150);
                        break;

                    case 59:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                        && Fas_Data.IsOrgRetOk == true)
                        {
                            iResettingFlag = 60;
                        }
                        break;                    

                    case 60:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                              && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                Thread.Sleep(100);
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                Thread.Sleep(100);
                                iResettingFlag = 70;
                            }

                        }
                        
                        break;

                    case 70:
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                      && Fas_Data.IsOrgRetOk == true)
                        {
                            double dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                iResettingFlag = 80;
                            }
                            else
                            {
                                iResettingFlag = 60;
                            }
                        }
                        break;

                    case 80:

                        iResettingFlag = 90;
                        break;

                    case 90:
                        //for (int idx = 0; idx < mChiken.Count; idx++)
                        //{
                        //    //mChiken[idx].cookerIndex = idx;
                        //    mChiken[idx].chickenState = Core_Data.EB_State.None;
                        //    mChiken[idx].chickenType = Core_Data.EChickenType.None;
                        //}                     
                        //mLoders[0] = new LoderModule(Core_Data.EChickenType.None);
                        //mLoders[1] = new LoderModule(Core_Data.EChickenType.None);
                        //mPlaceUnload = new LoderModule(Core_Data.EChickenType.None);
                        iResettingFlag = 100;
                        break;

                    case 100:
                        actLoad.Close();
                        swStartingTime.Stop();
                        retBool = true;
                        break;

                    default:
                        break;
                
                }

                if (iPrevResettingFlag != iResettingFlag)
                {
                    iPrevResettingFlag = iResettingFlag;
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                        devJace.Program.ELogLevel.Debug,
                        $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | FLAG : [{iResettingFlag:000}]");
                }


            }
            catch
            { }
            return retBool;
        }     
      
        /// <summary>
        /// 반복 시퀀스
        /// </summary>
        void Excute_Action()
        {
            //System.Threading.Thread.Sleep(70);

            /*
             * 2023.02.15
             * 
             * 타이머 동작
             * 10개 포지션
             * 
             * 사용유무에 따라 
             * 배출 / 투입 /
             * 
             * 
             * 배출 센서 확인
             * 
             * 추후 로더부 하고 배출부하고 공용 되게 해야 합니다.
             * 
             * 2023.04.14 ::: 우선순위 변경
             * 1. 배출
             * 2. 흔들기
             * 3. 투입
             * 4. 산소 입히기
             * 
             */


            //2023.04.06 ::: safety 

            if (Define.IsDebugPass == false)
            {
                if (Cores.Core_Object.GetObj_File.iLaserScannerUse == 1
                  && Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area] == false)
                    return;
            }

            Cores.Core_Object.GetObj_File.iAutoMode = 1;


            bool retBool = false;
            double iSpeed = 0;
            int fastechSpeed = 0;
            int fastechAbsPos = 0;
            double dActualPos = 0;

            //2023.03.08 ::: 로봇 툴 입력 확인
            var toolInputs = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);

            //2023.03.09 ::: 로봇 조인트 값 확인
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);
            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);

            //다운 완료
            var cobotLiftDown = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 147);
            var autoomplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 149);
            var autoCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 133);
            var autoPickPlace = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 131);

            //var autoStepNumber = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 154);
            //iCurrRobotAutoStepNumver = autoStepNumber.iData;
            //if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Motion] && iCurrRobotAutoStepNumver != iPrevRobotAutoStepNumver)
            //{
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
            //                              devJace.Program.ELogLevel.Debug,
            //                              $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
            //                              $" | [ Cobot Auto Step Number : {iCurrRobotAutoStepNumver}]");
            //
            //    iPrevRobotAutoStepNumver = iCurrRobotAutoStepNumver;
            //}

            //2023.03.10 ::: Temp
            //Fas_Data.IsOrgRetOk = true;

            //2023.04.11 ::: 로봇 동작 명령 구간
            if (iExcuteFlag != 40 
                && iExcuteFlag != 44
                && iExcuteFlag != 52
                  && iExcuteFlag != 62
                && iExcuteFlag != 72
                 && iExcuteFlag != 77)
            {
                IsHoldingPostive = true;
            }
            else
            {
                IsHoldingPostive = false;
            }

            //팔 굽히기 / 팔 펴기
            //2023.05.08
            IsGripArm = true;
            IsGripArm &= !IsOutputChiken;
            IsGripArm &= !IsGripWaitSensorCheck;
            IsGripArm &= !IsShakingChieken;
            IsGripArm &= !IsOxzenChieken;
            //엑스 축 대기 위치 이고
            //로봇 도 대기 위치 이고
            if (IsGripArm)
            {
                //Cobot_Wait_Action();
            }
            else
            {
                //Cobot_Move_Action((int)MyActionXStepBuffer.Wait, MyActionStepBuffer.None);
            }

            switch (iExcuteFlag)
            {
                #region 조리 배출 확인 후 명령
                case 00://타이머 완료 확인
                    if (myActionXaxisMove == MyActionXStepBuffer.None
                        && myActionPicknPlace == MyActionStepBuffer.None
                        && UnLoaderPlace() == false)
                    {
                        for (int idx = 0; idx < mChiken.Count; idx++)
                        {
                            //배출 확인
                            if (mCompBuffer[idx].IsCookingComplted == true || mChiken[idx].IsExist == true)
                            {
                                if (mChiken[idx].IsExist == true)
                                {
                                    mChiken[idx].CookingForceComplte();
                                    myActionXaxisMove = (MyActionXStepBuffer)(idx + XmoveOffset);
                                }
                                else
                                {
                                    myActionXaxisMove = (MyActionXStepBuffer)(mCompBuffer[idx].iCookingIndex + XmoveOffset);
                                }


                                myActionPicknPlace = MyActionStepBuffer.Pick_Output;
                                retBool = true;
                                break;
                            }
                        }
                    }
                    if (retBool)
                    {
                        iExcuteFlag = 70;
                    }
                    else
                    {
                        iExcuteFlag = 6;
                    }
                    break;
                #endregion

                
                case 02:
                 
                    break;

                case 03:

                    break;

                case 04:
                 
                    break;

                case 05:
                    break;


                #region 조리 중간 흔들기 / 산소 입히기 명령
                case 06:
                    if (myActionXaxisMove == MyActionXStepBuffer.None
                     && myActionPicknPlace == MyActionStepBuffer.None
                     && IsShakingChieken == true)
                    {
                        for (int idx = 0; idx < mChiken.Count; idx++)
                        {
                            ////흔들기 확인
                            if (mChiken[idx].IsLatchShakingChiken == true && mChiken[idx].IsLatchShakingChikenComplted == false)
                            {
                                myActionXaxisMove = (MyActionXStepBuffer)(idx + XmoveOffset);
                                myActionPicknPlace = MyActionStepBuffer.Shaking;
                                iExcuteFlag = 50;
                                break;
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 10;
                    }
                    break;

                case 07:
                    if (myActionXaxisMove == MyActionXStepBuffer.None
                  && myActionPicknPlace == MyActionStepBuffer.None
                  && IsOxzenChieken == true)
                    {
                        for (int idx = 0; idx < mChiken.Count; idx++)
                        {
                            ////산소 입히기 확인
                            if (mChiken[idx].IsLatchOxzenChiken == true && mChiken[idx].IsLatchOxzenChikenComplted == false)
                            {
                                myActionXaxisMove = (MyActionXStepBuffer)(idx + XmoveOffset);
                                myActionPicknPlace = MyActionStepBuffer.Oxzen;
                                iExcuteFlag = 60;
                                break;
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 8;
                    }
                    break;

                case 08:
                    if (Define.IsDebugPass == false)
                    {
                       // if(IsOutputChiken == false && IsShakingChieken == false
                       //&& IsOxzenChieken == false)
                        if (IsOutputChiken == false && IsShakingChieken == false
                       && IsInsertChiken == true && IsOxzenChieken == false)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);Cores.Core_Object.GetObj_File.iXaxisAccDecTime
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime); 
                                Thread.Sleep(100);
                                iExcuteFlag = 9;
                            }
                        }
                        else
                        {
                            myActionPicknPlace = MyActionStepBuffer.None;
                            myActionXaxisMove = MyActionXStepBuffer.None;
                            myActionCobot = MyActionCobotStepBuffer.None;


                            iExcuteFlag = 0;
                        }

                    }
                    else
                    {
                        iExcuteFlag = 0;
                    }
                    break;

                case 09:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
               && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                myActionPicknPlace = MyActionStepBuffer.None;
                                myActionXaxisMove = MyActionXStepBuffer.None;
                                myActionCobot = MyActionCobotStepBuffer.None;
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                iExcuteFlag = 9;
                            }
                        }
                    }
                    else
                    {
                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 0;
                    }
                    break;
                #endregion

                #region 조리 투입 확인 후 명령
                case 10:
                    //투입
                    if (IsInsertChiken)
                    {
                        for (int idx = 0; idx < mLoders.Length; idx++)
                        {
                            if (mLoders[idx].IsStartLatch == true && mLoders[idx].IsLoader)
                            {
                                retBool = mLoders[idx].IsStartLatch;

                                switch (idx)
                                {
                                    case 0:
                                        myActionXaxisMove = MyActionXStepBuffer.LoadA;
                                        myActionSwitch = MySwitchBuffer.SwitchA;
                                        break;
                                    case 1:
                                        myActionXaxisMove = MyActionXStepBuffer.LoadB;
                                        myActionSwitch = MySwitchBuffer.SwitchB;
                                        break;
                                }
                                myActionPicknPlace = MyActionStepBuffer.Pick_Input;
                                break;
                            }
                        }
                        if (retBool)
                        {
                            iExcuteFlag = 11;
                        }
                        else
                        {
                            myActionXaxisMove = MyActionXStepBuffer.None;
                            myActionPicknPlace = MyActionStepBuffer.None;
                            myActionSwitch = MySwitchBuffer.None;
                            //2023.03.09 ::: 배출 확인
                            iExcuteFlag = 7;
                        }
                    }
                    else
                    {
                        iExcuteFlag = 7;
                    }
                    break;

                case 11:
                    //2023.03.10 ::: 바로 투입 방지 위험성 때문에 딜레이 추가

                    //2023.04.14 ::: 멀리 있는 경우 타이머 쓰지 않고, 생각해보기,
                    dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                    if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                       && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                    {

                        if (swInDelayTime.IsRunning == false)
                        {
                            swInDelayTime.Restart();
                        }
                        else if (swInDelayTime.Elapsed.TotalSeconds >= Cores.Core_Object.GetObj_File.iLoadDelayTime)
                        {
                            swInDelayTime.Stop();
                            swInDelayTime.Reset();
                            iExcuteFlag = 20;
                        }


                    }
                    else
                    {
                        iExcuteFlag = 20;
                    }
                    break;
                #endregion

                #region 조리 투입 / 조리 투입 완료 / 시퀀스
                case 20://엑스 축 무빙 가능 여부 및 동작
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                     && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);

                                Thread.Sleep(100);
                                iExcuteFlag = 22;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 41;
                    }
                    break;

                case 22:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                iExcuteFlag = 24;
                            }
                            else
                            {
                                iExcuteFlag = 20;
                            }

                        }
                    }
                    else
                    {

                    }
                    break;

                case 24:
                    if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Motion] == true
                  && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Comp] == false)
                    {
                        iExcuteFlag = 40;
                    }
                    //iExcuteFlag = 100;
                    break;
                
                case 40://로봇 무빙 명령 // pick up
                        //타임아웃 처리 필요
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                      && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //if (Cobot_Move_Action((int)myActionXaxisMove))
                                    //{
                                    //    iExcuteFlag = 41;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionXaxisMove, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 41;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Up);
                                    }
                                }
                            }
                            //else
                            //{
                            //    iExcuteFlag = 20;
                            //}
                        }
                    }
                    else
                    {

                    }
                    
                    break;
                case 41://빈 조리기구 우선 순위 찾아 넣기
                    int Cooker = CookerSelted();
                    if (Cooker != 0)
                    {
                        iSeletedCooker = Cooker;
                        if (mChiken[iSeletedCooker - 1].chickenState == Core_Data.EB_State.None)
                        {
                            mChiken[iSeletedCooker - 1].CookingReady();

                            switch (myActionSwitch)
                            {
                                case MySwitchBuffer.SwitchA:
                                    mChiken[iSeletedCooker - 1].SetTimeChanege(new TimeSpan(0,
                                        Core_Object.GetObj_File.iSwitch1SetTimeMin, Core_Object.GetObj_File.iSwitch1SetTimeSec));

                                    mChiken[iSeletedCooker - 1].chickenType = Core_Data.EChickenType.Fried;
                                    break;

                                case MySwitchBuffer.SwitchB:
                                    mChiken[iSeletedCooker - 1].SetTimeChanege(new TimeSpan(0,
                                        Core_Object.GetObj_File.iSwitch2SetTimeMin, Core_Object.GetObj_File.iSwitch2SetTimeSec));

                                    mChiken[iSeletedCooker - 1].chickenType = Core_Data.EChickenType.French;
                                    break;

                                default:
                                    break;
                            }


                            iExcuteFlag = 42;
                            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                                  devJace.Program.ELogLevel.Debug,
                                  $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                  $" | Cooker & X Move PosNumber : [{Cooker:0}][{Cores.Core_Object.GetObj_File.lstCookerName[iSeletedCooker - 1]}]");
                        }
                    }
                    else
                    {
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        iExcuteFlag = 0;
                    }
                    break;

                case 42://

                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[iSeletedCooker + 3]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                Thread.Sleep(100);
                                iExcuteFlag = 43;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 46;
                    }
                    break;

                case 43://로봇 모션 확인 해서 넣기
                        //X_Axis_Override();
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Motion] == true
                      && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Comp] == false)
                        {

                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[iSeletedCooker + 3] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[iSeletedCooker + 3] - 0.5)
                            {
                                //로봇이 해야 할 위치 결정
                                myActionXaxisMove = (MyActionXStepBuffer)(iSeletedCooker + 4);
                                myActionPicknPlace = MyActionStepBuffer.Place_Input;


                                iExcuteFlag = 44;
                            }
                            else
                            {
                                iExcuteFlag = 42;
                            }
                        }
                    }
                    else
                    {

                    }
                  
                    break;


                case 44://플레이스 투입
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                                && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[iSeletedCooker + 3] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[iSeletedCooker + 3] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //2023.04.12 ::: 정확한 시작 시간을 위하여 추가
                                    if (mChiken[iSeletedCooker - 1].chickenState == Core_Data.EB_State.Waiting
                                       && cobotLiftDown.iData == 1)
                                    {
                                        mChiken[iSeletedCooker - 1].CookingStart();
                                    }

                                    //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                                    //devJace.Program.ELogLevel.Debug,
                                    //$"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                    //$" | Cooker & iSeletedCooker : [{cobotLiftDown.iData:0}][{Cores.Core_Object.GetObj_File.lstCookerName[iSeletedCooker - 1]}]");


                                    //if (Cobot_Move_Action((int)myActionXaxisMove))
                                    //{
                                    //    iExcuteFlag = 45;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionXaxisMove, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 45;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Up);
                                    }
                                }



                            }
                          
                        }
                    }
                    else
                    {

                    }
               
                    break;

                case 45://바스켓 없는 것 확인용
                    if (toolInputs.iData != 0)
                    {
                        iExcuteFlag = 46;
                    }
                    break;

                case 46://조리 시작 업데이트                  
                    if (mChiken[iSeletedCooker - 1].chickenState == Core_Data.EB_State.Waiting)
                    {
                        mChiken[iSeletedCooker - 1].CookingStart();

                        //switch (myActionXaxisMove)
                        //{
                        //    case MyActionXStepBuffer.LoadA:
                        //        mLoders[0].IsStartLatch = false;
                        //        break;
                        //    case MyActionXStepBuffer.LoadB:
                        //        mLoders[1].IsStartLatch = false;
                        //        break;
                        //}
                        iExcuteFlag = 47;
                        //IsStartLog = true;

                        Core_Data.ChickenCounter chickenlog = new Core_Data.ChickenCounter(
                         DateTime.Now, iSeletedCooker - 1, mChiken[iSeletedCooker - 1].chickenType, 1,
                         mChiken[iSeletedCooker - 1].tsSetTime,
                         mChiken[iSeletedCooker - 1].tsCurTime);

                        GC_Chiken_Logs.Enqueue(chickenlog);
                    }
                    else if (mChiken[iSeletedCooker - 1].chickenState == Core_Data.EB_State.Cooking)
                    {
                        iExcuteFlag = 47;
                    }
                    break;

                case 47:
                    //2023.03.07 ::: 투입 가능 여부 확인                    
                    //2023.03.08 ::: 배출 가능 여부 확인
                    //2023.03.09 ::: 투입을 했으면 무조건 대기 위치로 가야 하니까. !!!

                    //2023.04.14 ;;; lsDebugPass가 참이면서 cooker 1-1 ~ 3-2가 사용 중이면 iStoppingFlag = 0;으로
                    if (Define.IsDebugPass == false)
                    {
                        if (IsOutputChiken == true)
                        {
                            iExcuteFlag = 0;
                        }
                        else if (IsShakingChieken == true)
                        {
                            iExcuteFlag = 0;
                        }
                        else if (IsInsertChiken == true)
                        {
                            iExcuteFlag = 0;
                        }
                        else if (IsOxzenChieken == true)
                        {
                            iExcuteFlag = 0;
                        }
                        else
                        {
                            iExcuteFlag = 48;
                        }
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionPicknPlace = MyActionStepBuffer.None;
                    }
                    else
                    {
                        iExcuteFlag = 0;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionPicknPlace = MyActionStepBuffer.None;
                    }

                    break;

                case 48:
                    //투입완료 후 복귀 동작
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                                && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                Thread.Sleep(100);
                                iExcuteFlag = 49;
                            }

                        }
                    }
                    else
                    { 
                    
                    }
                  
                    break;

                case 49:
                    //대기 위치 완료
                    //X_Axis_Override();
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                   && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                iExcuteFlag = 48;
                            }
                        }
                    }
                    else
                    {

                    }
                   
                    break;

                #endregion

                #region 조리 중간 흔들기 / 시퀀스
                case 50:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                     && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                cooking_shake = true;
                                if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                                {
                                    myActionCobot = MyActionCobotStepBuffer.ShakingExt;
                                }
                                else
                                {
                                    myActionCobot = MyActionCobotStepBuffer.ShakingCom;
                                }
                                Thread.Sleep(100);
                                iExcuteFlag = 51;
                            }

                        }
                    }
                    else
                    {
                        if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                        {
                            myActionCobot = MyActionCobotStepBuffer.ShakingExt;
                        }
                        else
                        {
                            myActionCobot = MyActionCobotStepBuffer.ShakingCom;
                        }
                        Thread.Sleep(100);
                        iExcuteFlag = 51;
                    }
                    break;

                case 51:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                iExcuteFlag = 52;
                            }
                            else
                            {
                                iExcuteFlag = 50;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 52;
                    }
                    break;
                case 52:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                      && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //if (Cobot_Move_Action((int)myActionCobot))
                                    //{
                                    //    iExcuteFlag = 53;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionCobot, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 53;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Shake);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 53;
                    }
                    break;
                case 53:
                    if (mChiken[(int)myActionXaxisMove - 5].chickenState == Core_Data.EB_State.Cooking)
                    {
                        mChiken[(int)myActionXaxisMove - 5].CookingShakingComplted();

                        int iCookNumber = (int)myActionXaxisMove - 5;
                        int iArrayNumber = iCookNumber % 3;

                        //lstOilCheckdCount[iArrayNumber]++;
                        //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                        //   devJace.Program.ELogLevel.Debug,
                        //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        //   $" | Oil Changed Count : [{iCookNumber}][{iArrayNumber}][{lstOilCheckdCount[iArrayNumber]}]");
                        //
                        //
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iTotal++;
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iMonth++;
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iDays++;

                        Core_Data.ChickenCounter chickenlog = new Core_Data.ChickenCounter(
                            DateTime.Now, iCookNumber, mChiken[(int)myActionXaxisMove - 5].chickenType, 2,
                            mChiken[(int)myActionXaxisMove - 5].tsSetTime,
                            mChiken[(int)myActionXaxisMove - 5].tsCurTime);

                        GC_Chiken_Logs.Enqueue(chickenlog);

                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.Wait;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 54;
                    }
                    else if (mChiken[(int)myActionXaxisMove - 5].chickenState == Core_Data.EB_State.Cooked)
                    {
                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 0;
                    }
                    break;
                case 54:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                   && Fas_Data.IsOrgRetOk == true)
                        {

                            if (IsOutputChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsShakingChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsInsertChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsOxzenChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                                {
                                    iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                    fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                    fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                    //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                    Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);

                                    Thread.Sleep(100);
                                    iExcuteFlag = 55;
                                }
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 55;
                    }
                    break;
                case 55:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
               && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                myActionPicknPlace = MyActionStepBuffer.None;
                                myActionXaxisMove = MyActionXStepBuffer.None;
                                myActionCobot = MyActionCobotStepBuffer.None;

                                iExcuteFlag = 0;
                            }
                            else
                            {
                                iExcuteFlag = 54;
                            }
                        }
                    }
                    else
                    {
                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 0;
                    }
                    break;
                case 56:
                    break;
                case 57:
                    break;
                case 58:
                    break;
                case 59:
                    break;
                #endregion

                #region 조리 중간 산소 입히기 / 시퀀스
                case 60:

                    //2023.04.18 ::: 임시 테스트 산소 입히기를 한번더 흔들기로

                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                     && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                cooking_oxzen = true;

                                //2023.04.18 ::: 임시 테스트 산소 입히기를 한번더 흔들기로
                                if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                                {
                                    myActionCobot = MyActionCobotStepBuffer.OxzenExt;
                                }
                                else
                                {
                                    myActionCobot = MyActionCobotStepBuffer.OxzenCom;
                                }
                                //if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                                //{
                                //    myActionCobot = MyActionCobotStepBuffer.ShakingExt;
                                //}
                                //else
                                //{
                                //    myActionCobot = MyActionCobotStepBuffer.ShakingCom;
                                //}
                                Thread.Sleep(100);
                                iExcuteFlag = 61;
                            }

                        }
                    }
                    else
                    {

                        //2023.04.18 ::: 임시 테스트 산소 입히기를 한번더 흔들기로
                        if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                        {
                            myActionCobot = MyActionCobotStepBuffer.OxzenExt;
                        }
                        else
                        {
                            myActionCobot = MyActionCobotStepBuffer.OxzenCom;
                        }
                        //if (myActionXaxisMove == MyActionXStepBuffer.Cooker6)
                        //{
                        //    myActionCobot = MyActionCobotStepBuffer.ShakingExt;
                        //}
                        //else
                        //{
                        //    myActionCobot = MyActionCobotStepBuffer.ShakingCom;
                        //}
                        Thread.Sleep(100);
                        iExcuteFlag = 61;
                    }
                    break;
                case 61:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                iExcuteFlag = 62;
                            }
                            else
                            {
                                iExcuteFlag = 60;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 62;
                    }
                    break;
                case 62:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                      && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //if (Cobot_Move_Action((int)myActionCobot))
                                    //{
                                    //    iExcuteFlag = 63;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionCobot, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 63;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Oxzen);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 63;
                    }
                    break;
                case 63:
                    if (mChiken[(int)myActionXaxisMove - 5].chickenState == Core_Data.EB_State.Cooking)
                    {
                        mChiken[(int)myActionXaxisMove - 5].CookingOxzenComplted();

                        int iCookNumber = (int)myActionXaxisMove - 5;
                        int iArrayNumber = iCookNumber % 3;

                        //lstOilCheckdCount[iArrayNumber]++;
                        //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                        //   devJace.Program.ELogLevel.Debug,
                        //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        //   $" | Oil Changed Count : [{iCookNumber}][{iArrayNumber}][{lstOilCheckdCount[iArrayNumber]}]");
                        //
                        //
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iTotal++;
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iMonth++;
                        //Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iDays++;

                        Core_Data.ChickenCounter chickenlog = new Core_Data.ChickenCounter(
                            DateTime.Now, iCookNumber, mChiken[(int)myActionXaxisMove - 5].chickenType, 3,
                            mChiken[(int)myActionXaxisMove - 5].tsSetTime,
                            mChiken[(int)myActionXaxisMove - 5].tsCurTime);

                        GC_Chiken_Logs.Enqueue(chickenlog);

                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.Wait;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 64;
                    }
                    else if (mChiken[(int)myActionXaxisMove - 5].chickenState == Core_Data.EB_State.Cooked)
                    {
                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionCobot = MyActionCobotStepBuffer.None;

                        iExcuteFlag = 0;
                    }
                    break;
                case 64:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                   && Fas_Data.IsOrgRetOk == true)
                        {
                            if (IsOutputChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsShakingChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsInsertChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsOxzenChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                                {
                                    iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                    fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                    fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                    //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                    Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                    Thread.Sleep(100);
                                    iExcuteFlag = 65;
                                }
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 65;
                    }
                    break;
                case 65:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
               && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                myActionPicknPlace = MyActionStepBuffer.None;
                                myActionXaxisMove = MyActionXStepBuffer.None;
                                myActionCobot = MyActionCobotStepBuffer.None;
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                iExcuteFlag = 64;
                            }
                        }
                    }
                    else
                    {
                        myActionPicknPlace = MyActionStepBuffer.None;
                        myActionXaxisMove = MyActionXStepBuffer.None;
                        myActionCobot = MyActionCobotStepBuffer.None;
                        iExcuteFlag = 0;
                    }
                    break;
                case 66:
                    break;
                case 67:
                    break;
                case 68:
                    break;
                case 69:
                    break; 
                #endregion

                #region 조리 배출 / 조리 배출 완료 / 시퀀스
                case 70://조리배출 자리 이동 명령
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                        && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1])
                                && swOutStartDelayTime.Elapsed.TotalSeconds >= Cores.Core_Object.GetObj_File.iUnLoadDelayTime)
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                Thread.Sleep(100);
                                iExcuteFlag = 71;

                                swOutStartDelayTime.Stop();
                                swOutStartDelayTime.Reset();

                                for (int idx = 0; idx < mCompBuffer.Count; idx++)
                                {
                                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                                            devJace.Program.ELogLevel.Debug,
                                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                            $" | Prioty : {Cores.Core_Object.GetObj_File.lstCookerName[mCompBuffer[idx].iCookingIndex]} [{mCompBuffer[idx].iCookingIndex}] [{mCompBuffer[idx].IsCookingComplted}] [{mCompBuffer[idx].dCookingTime}]");
                                }
                            }
                            else if (swOutStartDelayTime.IsRunning == false)
                            {
                                swOutStartDelayTime.Restart();
                            }

                            //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                            //            devJace.Program.ELogLevel.Debug,
                            //            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            //            $" | Prioty : [{mCompBuffer[idx].iCookingIndex}] [{mCompBuffer[idx].IsCookingComplted}] [{mCompBuffer[idx].dCookingTime}]");
                        }
                    }
                    else
                    {
                        iExcuteFlag = 71;
                    }
                    
                    break;

                case 71://조리배출 자리 이동 완료 확인
                        //X_Axis_Override();
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                    && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                iExcuteFlag = 72;
                            }
                            else
                            {
                                iExcuteFlag = 70;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 72;
                    }
                 
                    break;

                case 72://픽업, 아웃 명령
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //if (Cobot_Move_Action((int)myActionXaxisMove))
                                    //{
                                    //    iExcuteFlag = 73;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionXaxisMove, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 73;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Up);
                                        ActivatedCount(xui_type.Rb_Drain);
                                    }
                                }

                            }
                            //else
                            //{
                            //    iExcuteFlag = 70;
                            //}
                        }
                    }
                    else
                    {
                        iExcuteFlag = 73;
                    }
                   

                    //2023.03.08 ::: 갑자기 모션 링크가 끈어짐 발생 업데이트 안됨 확인 필요

                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    //          devJace.Program.ELogLevel.Debug,
                    //          $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //          $" | Inposition : [{Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition]}]");
                    //
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    //          devJace.Program.ELogLevel.Debug,
                    //          $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //          $" | Org_Ret_OK : [{Fas_Data.lstAxis_State[(int)EAxis_Status.Org_Ret_OK]}]");
                    //
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    //        devJace.Program.ELogLevel.Debug,
                    //        $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //        $" | Err_Sevo_Alarm : [{Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm]}]");


                    break;

                case 73://조리완료 업데이트 부분
                        //2023.03.08 ::: 기름 교체 회수 증가 하기
                    if (mChiken[(int)myActionXaxisMove - 5].chickenState == Core_Data.EB_State.Cooked)
                    {
                        mChiken[(int)myActionXaxisMove - 5].CookingComplted();

                        int iCookNumber = (int)myActionXaxisMove - 5;
                        //2023.05.23 ::: 오일교체 주기 카운트 에러 수정
                        //int iArrayNumber = iCookNumber % 3; //012 012
                        int iArrayNumber = iCookNumber / 2;

                        lstOilCheckdCount[iArrayNumber]++;
                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                           devJace.Program.ELogLevel.Debug,
                           $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                           $" | Oil Changed Count : [{iCookNumber}][{iArrayNumber}][{lstOilCheckdCount[iArrayNumber]}]");


                        Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iTotal++;
                        Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iMonth++;
                        Core_Object.GetCounters[(int)Core_Data.MainType.Chiken].iDays++;

                        Core_Data.ChickenCounter chickenlog = new Core_Data.ChickenCounter(
                            DateTime.Now,  iCookNumber, mChiken[(int)myActionXaxisMove - 5].chickenType, 4,
                            mChiken[(int)myActionXaxisMove - 5].tsSetTime,
                            mChiken[(int)myActionXaxisMove - 5].tsCurTime);

                        GC_Chiken_Logs.Enqueue(chickenlog);



                        myActionPicknPlace = MyActionStepBuffer.Place_Output;
                        if (UnLoaderPlace() == true)
                        {
                            myActionXaxisMove = MyActionXStepBuffer.Wait;
                            iExcuteFlag = 80;
                        }
                        else
                        {
                            UnLoaderPlace(out myActionXaxisMove);
                            //myActionXaxisMove = MyActionXStepBuffer.LoadC;
                            iExcuteFlag = 74;
                        }
                    }

                    break;

                case 74:
                    //2023.03.10 ::: 기름 좀 빠지게 대기
                    if (Define.IsDebugPass == false)
                    {
                        if (swOutDelayTime.IsRunning == false)
                        {
                            swOutDelayTime.Restart();
                        }
                        else if (swOutDelayTime.Elapsed.TotalSeconds >= Cores.Core_Object.GetObj_File.iBasketDelayTime)
                        {
                            swOutDelayTime.Stop();
                            swOutDelayTime.Reset();
                            iExcuteFlag = 75;
                        }
                    }
                    else
                    {
                        iExcuteFlag = 75;
                    }
                 
                    break;

                case 75:
                    //배출 위치 이동 명령
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                    && Fas_Data.IsOrgRetOk == true)
                        {
                            if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                            {
                                iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1]);
                                //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                Thread.Sleep(100);
                                iExcuteFlag = 76;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 76;
                    }
                 
                    break;

                case 76:
                    //배출 위치 이동 완료 확인
                    //X_Axis_Override();
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                      && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                iExcuteFlag = 77;
                            }
                            else
                            {
                                iExcuteFlag = 75;
                            }

                        }
                    }
                    else
                    {
                        iExcuteFlag = 77;
                    }
              

                    break;

                case 77:
                    //배출 로봇 명령
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
             && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)myActionXaxisMove - 1] - 0.5)
                            {
                                if (IsCobotMoveAction == false)
                                {
                                    //if (Cobot_Move_Action((int)myActionXaxisMove))
                                    //{
                                    //    iExcuteFlag = 78;
                                    //    IsCobotMoveAction = false;
                                    //}

                                    if (Cobot_Move_Action((int)myActionXaxisMove, myActionPicknPlace))
                                    {
                                        iExcuteFlag = 78;
                                        IsCobotMoveAction = false;

                                        ActivatedCount(xui_type.Gripper);
                                        ActivatedCount(xui_type.Rb_Down);
                                        ActivatedCount(xui_type.Rb_Up);
                                    }
                                }
                            }
                            //else
                            //{
                            //    iExcuteFlag = 75;
                            //}

                        }
                    }
                    else
                    {
                        iExcuteFlag = 78;
                    }
                
                    break;
                case 78:
                    //2023.03.07 ::: 투입 가능 여부 확인                    
                    //2023.03.08 ::: 배출 가능 여부 확인
                    //2023.03.09 ::: 조건 변경
                    //if (IsInsertChiken == false && IsOutputChiken == false)

                    if (IsInsertChiken == true && IsOutputChiken == false)
                    {
                        iExcuteFlag = 79;
                    }
                    else
                    {
                        iExcuteFlag = 0;
                    }
                    myActionXaxisMove = MyActionXStepBuffer.None;
                    myActionPicknPlace = MyActionStepBuffer.None;

                    break;
                case 79:
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                   && Fas_Data.IsOrgRetOk == true)
                        {
                            if (IsOutputChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsShakingChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsInsertChiken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else if (IsOxzenChieken == true)
                            {
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                                {
                                    iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                                    fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                                    fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                                    //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                                    Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                                    Thread.Sleep(100);
                                    iExcuteFlag = 80;
                                }
                            }

                           

                        }
                    }
                    else
                    {
                        iExcuteFlag = 80;
                    }
                
                    break;

                case 80:
                    //대기 위치 완료
                    //X_Axis_Override();
                    if (Define.IsDebugPass == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
               && Fas_Data.IsOrgRetOk == true)
                        {
                            dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                            if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                               && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                            {
                                iExcuteFlag = 0;
                            }
                            else
                            {
                                iExcuteFlag = 79;
                            }
                        }
                    }
                    else
                    {
                        iExcuteFlag = 0;
                    }
               
                    break;
                #endregion

                #region 조리 시퀀스 / 예비
                case 90:
                    break;

                case 100:
                    break;

                default:
                    iExcuteFlag = 0;
                    break; 
                    #endregion
            }

            if (iPrevExcuteFlag != iExcuteFlag)
            {   
                iPrevExcuteFlag = iExcuteFlag;

                if (iExcuteFlag >= 20)
                {
                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                        devJace.Program.ELogLevel.Debug,
                        $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                        $" | FLAG : [{iExcuteFlag:000}]");
                }
            }           

        }

        void ActivatedCount(xui_type type)
        {
            switch (type)
            {
                case xui_type.Gripper:
                    Project_Main.FormMain.gui_file.lGripperUseCount++;
                    break;

                case xui_type.Rb_Down:
                    Project_Main.FormMain.gui_file.lRobotDownUseCount++;
                    break;

                case xui_type.Rb_Up:
                    Project_Main.FormMain.gui_file.lRobotUpUseCount++;
                    break;

                case xui_type.Rb_Shake:
                    Project_Main.FormMain.gui_file.lRobotShakeUseCount++;
                    break;

                case xui_type.Rb_Oxzen:
                    Project_Main.FormMain.gui_file.lRobotOxzeneUseCount++;
                    break;

                case xui_type.Rb_Drain:
                    Project_Main.FormMain.gui_file.lRobotOilDrainUseCount++;
                    break;

                case xui_type.Rb_Clearing:
                    Project_Main.FormMain.gui_file.iRobotClerningUseCount++;
                    break;

            }
        }

        enum xui_type
        { 
            Gripper, Rb_Down, Rb_Up, Rb_Shake, Rb_Oxzen, Rb_Drain, Rb_Clearing
        }

        /// <summary>
        /// 조리기구 투입 우선 순위 명령부
        /// 2023.03.08 ::: 함수 테스트 완료
        /// </summary>
        /// <returns></returns>
        int CookerSelted()
        {
            int iNumber = 0;
            lstPriorityUsed[0] = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            lstPriorityUsed[1] = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            lstPriorityUsed[2] = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            lstPriorityUsed[3] = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            lstPriorityUsed[4] = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
            lstPriorityUsed[5] = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];

            mChiken[0].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            mChiken[1].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            mChiken[2].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            mChiken[3].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            mChiken[4].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
            mChiken[5].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];

            for (int idx = 0; idx < lstPriority.Count; idx++)
            {
                if (lstPriorityUsed[lstPriority[idx] - 1] == true && mChiken[lstPriority[idx] - 1].chickenState == Core_Data.EB_State.None)
                {
                    iNumber = lstPriority[idx];
                    break;
                }
            }

            return iNumber;
        }

        /// <summary>
        /// 코봇 자동 명령 함수
        /// 2023.03.09 확인 완료
        /// </summary>
        /// <param name="nPosNumber"></param>
        /// <returns></returns>
        public static bool Cobot_Move_Action(int nPosNumber)
        {
            bool retBool = false;

            bool[] OutBuff = Fas_Data.lstIO_OutState[COBOT];
            int OnOff = 0;

            if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Motion] == true
                 && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Comp] == false
                 && OutBuff[(int)COBOT_OUTPUT.Move_Cmd] == false && nPosNumber != 0)
            {
                //Move_Pos1 = 11, Move_Pos2 = 12, Move_Pos3 = 13, Move_Pos4 = 14, Move_Cmd = 15,

                OnOff = nPosNumber & 1;
                OutBuff[(int)COBOT_OUTPUT.Move_Pos1] = Convert.ToBoolean(OnOff);
                OnOff = (nPosNumber >> 1) & 1;
                OutBuff[(int)COBOT_OUTPUT.Move_Pos2] = Convert.ToBoolean(OnOff);
                OnOff = (nPosNumber >> 2) & 1;
                OutBuff[(int)COBOT_OUTPUT.Move_Pos3] = Convert.ToBoolean(OnOff);
                OnOff = (nPosNumber >> 3) & 1;
                OutBuff[(int)COBOT_OUTPUT.Move_Pos4] = Convert.ToBoolean(OnOff);
                OutBuff[(int)COBOT_OUTPUT.Move_Cmd] = true;

                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);
            }
            else if (OutBuff[(int)COBOT_OUTPUT.Move_Cmd] == true
                && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Comp] == true)
            {

                OutBuff[(int)COBOT_OUTPUT.Move_Pos1] = Convert.ToBoolean(OnOff);
                OutBuff[(int)COBOT_OUTPUT.Move_Pos2] = Convert.ToBoolean(OnOff);
                OutBuff[(int)COBOT_OUTPUT.Move_Pos3] = Convert.ToBoolean(OnOff);
                OutBuff[(int)COBOT_OUTPUT.Move_Pos4] = Convert.ToBoolean(OnOff);
                OutBuff[(int)COBOT_OUTPUT.Move_Cmd] = Convert.ToBoolean(OnOff);

                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);

                IsCobotMoveAction = true;
                retBool = true;
            }          

            return retBool;
        }

        /// <summary>
        /// 2023.05.04 코봇 자동 명령 함수, 얼라인 계산용 추가 되면서, 모드버스 쪽으로 옮긴것.
        /// </summary>
        /// <param name="nPosNumber"></param>
        /// <param name="pickplace"></param>
        /// <returns></returns>
        public bool Cobot_Move_Action(int nPosNumber, MyActionStepBuffer pickplace)
        {
            bool retBool = false;

            bool[] OutBuff = Fas_Data.lstIO_OutState[COBOT];
            //int OnOff = 0;

            var autoomplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 149);
            var autoCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 133);
            var autoPickPlace = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 131);


            if ((int)pickplace != autoPickPlace.iData)
            {
                ModBusSendEvent(131, (int)pickplace);
            }


            if (autoCommand.iData != nPosNumber)
            {
                ModBusSendEvent(133, nPosNumber);
            }
            
            
            if(Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT_COLL.Move_Cmp] == false
                 && OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] == false && nPosNumber != 0)
            {
                OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);
            }
            else if (OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] == true
                && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT_COLL.Move_Cmp] == true)
            {
                OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);
                IsCobotMoveAction = true;
                retBool = true;
            }

            return retBool;
        }

        /// <summary>
        /// 엑스 축 대기 위치이고, 아무 작업 안할때,
        /// </summary>
        /// <returns></returns>
        public bool Cobot_Wait_Action()
        {
            bool retBool = false;

            bool[] OutBuff = Fas_Data.lstIO_OutState[COBOT];
            //int OnOff = 0;

            var autoomplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 149);
            var autoCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 133);
            var autoPickPlace = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 131);


            //if ((int)pickplace != autoPickPlace.iData)
            //{
            //    ModBusSendEvent(131, (int)pickplace);
            //}

            int iGripperWait = 16;

            if (autoCommand.iData != iGripperWait)
            {
                ModBusSendEvent(133, iGripperWait);
            }


            if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT_COLL.Move_Cmp] == false
                 && OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] == false )
            {
                OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);
            }
            else if (OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] == true
                && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT_COLL.Move_Cmp] == true)
            {
                OutBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] = false;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutBuff);
                IsCobotMoveAction = true;
                retBool = true;
            }

            return retBool;
        }
             

        /// <summary>
        /// 2023.05.01 ::: 언로드 위치 확인 및 언로드 이동 위치 명령좌표
        /// </summary>
        /// <param name="unloadSpace"></param>
        /// <returns></returns>
        bool UnLoaderPlace(out MyActionXStepBuffer unloadSpace)
        {
            bool IsPlaceUnload = true;
            unloadSpace = MyActionXStepBuffer.None;
            for (int idx = 0; idx < mLoders.Length; idx++)
            {
                if (mLoders[idx].IsLoader == false)
                {
                    IsPlaceUnload &= mLoders[idx].IsCurrSensor;

                    if (mLoders[idx].IsCurrSensor == false)
                    {
                        //234
                        //012
                        unloadSpace = (MyActionXStepBuffer)(idx + 2);
                        break;
                    }
                }
            }

            return IsPlaceUnload;
        }

        /// <summary>
        /// 2023.05.01 ::: 언로드 위치 확인
        /// </summary>
        /// <returns></returns>
        bool UnLoaderPlace()
        {
            bool IsPlaceUnload = true;
            //unloadSpace = MyActionXStepBuffer.None;
            for (int idx = 0; idx < mLoders.Length; idx++)
            {
                if (mLoders[idx].IsLoader == false)
                {
                    IsPlaceUnload &= mLoders[idx].IsCurrSensor;

                    //if (mLoders[idx].IsCurrSensor)
                    //{
                    //    //234
                    //    //012
                    //    unloadSpace = (MyActionXStepBuffer)(idx + 2);
                    //}
                }
            }

            return IsPlaceUnload;
        }

        /// <summary>
        /// 로봇 위치 비교
        /// src ::: Cobot Pos, dst :: wait pos
        /// 2023.03.09 확인 완료
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool Cobot_Pos_Compare(double[] src, double[] dst)
        { 
            bool retBool = true;

            for (int idx= 0; idx < 6; idx++)
            {
                //2023.03.10 ::: 대기위치 값
                //2023.05.16 ::: 로버드 1번 축만 3미리 차리 난다. 홈은 조인트 인데, 대기 위치는 태스크이고, 고민 이고만.

                if (idx == 0)
                {
                    if (src[idx] <= dst[idx] + 5 && src[idx] >= dst[idx] - 5)
                    {

                    }
                    else
                    {
                        retBool &= false;
                    }
                }
                else
                {

                    if (src[idx] <= dst[idx] + 2 && src[idx] >= dst[idx] - 2)
                    {

                    }
                    else
                    {
                        retBool &= false;
                    }
                }

            }
            return  retBool;
        }

        /// <summary>
        /// 2023.06.06 ::: 엑스축 레이져 스캐너에 따른 속도 오버라이딩
        /// 2023.04.13 ::: 수동 음수이동이나 양수 이동이 안되는 경우 발생
        ///                   if (iPrevOverrideSpeed != iCurrOverrideSpeed)
        /// </summary>
        public void X_Axis_Override()
        {
            if (Cores.Core_Object.GetObj_File.iLaserScannerUse == 0)
                return;

            //if (Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Inposition] == true
            //        && Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] == true)
            //    return;

            //이렇게 하면 일시정지했다가 풀릴때 문제가 됨.
            //if (Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Inposition] == false
            //        && Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] == true)
            if (Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] == true)
            {
                bool IsChanged = false;

                double iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                int fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);
                double spd = Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iActVel);

                iCurrOverrideSpeed = Cores.Core_Object.GetObj_File.iXaxisSpeed;

                if (Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area] == false
                   && Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false)
                {
                    Cores.Core_Object.GetObj_File.iXaxisSpeed = 0;
                    Cores.Fas_Motion.VelocityOverride(MOTOR, 0);
                }
                else if (Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area] == true
                    && Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Waring_area] == false
                    && Cores.Core_Object.GetObj_File.iXaxisSpeed == 50)
                {
                    Cores.Fas_Motion.VelocityOverride(MOTOR, fastechSpeed);
                }
                else if (Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area] == true
                    && Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Waring_area] == true
                    && Cores.Core_Object.GetObj_File.iXaxisSpeed == 100)
                {
                    if (iPrevOverrideSpeed != iCurrOverrideSpeed)
                    {
                        Cores.Fas_Motion.VelocityOverride(MOTOR, fastechSpeed);
                    }

                }

                if (iPrevOverrideSpeed != iCurrOverrideSpeed)
                {
                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                    //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(0)}" +
                    //    $" | Axis Speed : {Cores.Core_Object.GetObj_File.iXaxisSpeed}");


                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                          $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(0)}" +
                          $" | Axis Speed : {iCurrOverrideSpeed:000}");
                }

                iPrevOverrideSpeed = iCurrOverrideSpeed;
            }
               

            //IsCurrWarningArea = Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Waring_area];
            //if (IsCurrWarningArea == false && IsCurrWarningArea != IsPrevWarningArea)
            //{
            //    Cores.Fas_Motion.VelocityOverride(MOTOR, fastechSpeed);
            //
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
            //       $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(0)}" +
            //       $" | Axis Speed : {Cores.Core_Object.GetObj_File.iXaxisSpeed}");
            //}
            //else if (IsCurrWarningArea == true && IsCurrWarningArea != IsPrevWarningArea )
            //{
            //    Cores.Fas_Motion.VelocityOverride(MOTOR, fastechSpeed);
            //
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
            //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(0)}" +
            //    $" | Axis Speed : {Cores.Core_Object.GetObj_File.iXaxisSpeed}");
            //}
            //IsPrevWarningArea = IsCurrWarningArea;
        }

        /// <summary>
        /// 요놈을 준비동작에 쓸지 시작동작에 쓸지 고민이고만....
        /// 2023.02.28
        /// </summary>
        /// <returns></returns>
        bool Starting_Action()
        {
            bool retBool = false;
                    
            if (actLoad == null || actLoad.IsDispose())
            {
                swStartingTime.Restart();
                actLoad = new Externs.Action_Loading();
                actLoad.Open();
            }

            if (Define.IsDebugPass)
            {
                iStartingFlag = 100;
            }

            actLoad.UpdateProgress($"<Starting : {iStartingFlag}>  {strDesc} [{swStartingTime.ElapsedMilliseconds} ms]");

            int alarm = Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0;
            int orging = Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0;
            int inpos = Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0;
            int servoOn = Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0;  
            
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);

            var cobotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            var cobotServoOn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 260);
            var cobotEmcStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 261);
            var cobotSafeStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 262);
            var cobotToolInput = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 021);

            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //0.
            bool IsConnection = true;
            //1.
            bool IsCobotAlarm = true;
            bool[] IsCobotState = new bool[5];
            //if(cobotState.iData == )

            //2.
            bool IsGripping = true;
            if ((cobotToolInput.iData & 1) != 0 && (cobotToolInput.iData >> 1 & 1) != 0)
            {
                IsGripping &= false;
            }
            //3.
            bool IsCookWaveUsed = false;
            foreach (bool use in Cores.Core_Object.GetObj_File.lstOilMeckUse)
            {
                if (use == true)
                {
                    IsCookWaveUsed = use;
                    break;
                }
            }

            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];

            switch (iStartingFlag)
            {
                case 0://이더넷 연결 확인           
                    strDesc = "조리 시작 합니다.";
                    IsConnection &= Cores.Core_Object.cobotConneted;
                    for (int i = 0; i <  Core_Object.fasTechConneted.Length; i++)
                    {
                        IsConnection &= Core_Object.fasTechConneted[i];
                    }
                    if (IsConnection)
                    {
                        iStartingFlag = 1;
                    }
                    //iStartingFlag = 1;
                    break;

                case 1://로봇 알람 상태 확인
                    strDesc = "로봇 상태 확인 합니다.";
                    if (
                        Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.EMC_1] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.EMC_2] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.ABNOMAL_1] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.ABNOMAL_2] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.SafeStop] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.SafeTouque] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Remote] == false
                        && Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Tasking] == false)
                    {
                        //EMC = 0, Emc = 1, ABNOMAL = 2, Abnomal = 3, SafeTouque = 4, SafeStop = 5, Remote = 6, Auto = 7, Comp = 8, Motion = 9, Tasking = 10, Home = 11, HomeAlarm = 12, DefWork = 13, NorWork = 14, CokWork = 15, Max = 16
                        iStartingFlag = 2;
                    }
                    iStartingFlag = 2;
                    break;

                case 2://엑스축 알람 상태 확인
                    strDesc = "모터 상태 확인 합니다.";
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == false
                       && Fas_Data.lstAxis_State[(int)EAxis_Status.Emg_Stop] == false
                       && Fas_Data.lstAxis_State[(int)EAxis_Status.Org_Ret_OK] == true
                       && Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true
                       && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == true)
                    {
                        //Err_Sevo_Alarm = 0,
                        //HW_Plus_Limit = 1,
                        //HW_Miuns_Limit = 2,
                        //SW_Plus_Limit = 3,
                        //SW_Miuns_Limit = 4,
                        //Reserved = 5,      

                        //Emg_Stop = 16,
                        //Slow_Stop = 17,
                        //Org_Returning = 18,
                        //Inposition = 19,
                        //Servo_On = 20,
                        //Alarm_Reset = 21,
                        //PT_Stopped = 22,
                        //Origin_Sensor = 23,

                        //Z_Pulse = 24,
                        //Org_Ret_OK = 25,
                        //Motion_DIR = 26,
                        //Motioning = 27,
                        //Motion_Pause = 28,
                        //Motion_Accel = 29,
                        //Motion_Decel = 30,
                        //Motion_Constant = 31,

                        //Max = 32

                        iStartingFlag = 3;
                    }
                    //if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false)
                    //{
                    //    Cores.Fas_Motion.SetServoOn(MOTOR, 1);
                    //}
                    iStartingFlag = 3;

                    //Console.WriteLine($"{EAxis_Status.Err_Sevo_Alarm} : {Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm]}");
                    //Console.WriteLine($"{EAxis_Status.Emg_Stop} : {Fas_Data.lstAxis_State[(int)EAxis_Status.Emg_Stop]}");
                    //Console.WriteLine($"{EAxis_Status.Org_Ret_OK} : {Fas_Data.lstAxis_State[(int)EAxis_Status.Org_Ret_OK]}");
                    //Console.WriteLine($"{EAxis_Status.Inposition} : {Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition]}");
                    //Console.WriteLine($"{EAxis_Status.Servo_On} : {Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On]}");

                    break;

                case 3://기타
                    strDesc = "전장 상태 확인 합니다.";
                    if ((Fas_Data.lstIO_InState[CHEFX][(int)CHEFX_INPUT.LeftDoorOpen] == false
                        && Fas_Data.lstIO_InState[CHEFX][(int)CHEFX_INPUT.RightDoorOpen] == false) 
                        || IsCobortPass == true)

                    {
                        //HighTempLimitA = 12, HighTempLimitB = 13, LeftDoorOpen = 14, RightDoorOpen = 15, Max = 16
                        iStartingFlag = 4;
                    }
                    break;

                case 4://출력 해야 할 것
                    //Remmote_On1 = 0, Remmote_On2 = 1, Cobot_Reset1 = 2, Cobot_Reset2 = 3, Task_Start = 4, Task_Pause = 5, Task_Stop = 6, Task_Resume = 7,
                    //Serovo_On = 8, Cobot_On = 9, Cobot_Off = 10, Move_Cmd = 11, Move_Pos1 = 12, Move_Pos2 = 13, Move_Pos3 = 14, Move_Pos4 = 15,
                    strDesc = "출력 제어 중 입니다.";

                    iStartingFlag = 5;
                    break;

                case 5://조리기구 3개다 사용
                    if (IsCookWaveUsed == true)
                    {
                        iStartingFlag = 10;
                    }
                    iStartingFlag = 10;
                    break;

                case 10://그리퍼 닫힘이면, 열림으로, 무게 확인(A 시리즈, E 시리즈 안됨 / M 시리즈 가능)
                    strDesc = "그리퍼 확인 중 입니다.";
                    if (IsGripping == false)
                    {
                        iStartingFlag = 20;
                    }
                    iStartingFlag = 20;
                    break;

                case 20://로봇 홈이 아니면 홈
                    strDesc = "코봇 호밍 중 입니다.";
                    if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Home] == false)
                    {
                        //Cobot_Command(128, 0x0E);
                        iStartingFlag = 21;
                        
                    }
                    else
                    {
                        iStartingFlag = 30;
                    }
                    break;

                case 21://로봇 홈 완료 확인
                    if (Fas_Data.lstIO_InState[COBOT][(int)COBOT_INPUT.Home] == true)
                    {
                        iStartingFlag = 30;
                    }
                    iStartingFlag = 30;
                    break;              

                case 30://로봇 대기 위치가 아니면 대기 위치로
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos1] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos2] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos3] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos4] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;

                    //2023.05.04 ::: 변경
                    OutStateBuff[(int)COBOT_OUTPUT_COLL.Move_Cmd] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    iStartingFlag = 31;
                    break;

                case 31://로봇 대기 위치 완료 확인
                    iStartingFlag = 40;
                    break;


                case 40://엑스 축 홈이 아니면 홈
                    strDesc = "X Axis 호밍 중 입니다.";
                    if (Fas_Data.IsOrgRetOk == false)
                    {
                        Cores.Fas_Motion.OriginSearch(MOTOR);
                        iStartingFlag = 50;
                    }
                    iStartingFlag = 50;
                    break;

                case 41://엑스축 홈 완료 확인
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Org_Ret_OK] == true
                        && Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true)
                    {   
                        iStartingFlag = 50;
                    }
                    break;

                case 50://엑스축 대기 위치가 아니면 대기 위치로
                    
                    iStartingFlag = 60;
                    break;

                case 51:
                    break;

                case 60:
                    iStartingFlag = 70;
                    break;

                case 70:
                    iStartingFlag = 80;
                    break;               

                case 80:
                    iStartingFlag = 90;
                    break;

                case 90:               
                    iStartingFlag = 100;
                    break;

                case 100:
                    actLoad.Close();
                    swStartingTime.Stop();
                    retBool = true;
                    break;


                default:
                    break;
            }

            if (iPrevStartingFlag != iStartingFlag)
            {
                iPrevStartingFlag = iStartingFlag;
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    devJace.Program.ELogLevel.Debug,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | FLAG : [{iStartingFlag:000}]");
            }
            return retBool;
        }

       /// <summary>
       /// 정지 동작 시퀀스 
       /// 2023.05.03 협동작업구역 추가에 대한 조건으로 인하여 
       /// </summary>
       /// <returns></returns>
        bool Stopping_Action()
        {
            /*
                * 2023.02.20
                * 
                * 그리퍼의 바스켓 유무 판단
                * 로봇 대기 위치 아니면 대기 위치 명령
                * 
                * 
                * 
                */

            if (Define.IsDebugPass == false)
            {
                if (Cores.Core_Object.GetObj_File.iLaserScannerUse == 1
                  && Fas_Data.lstIO_InState[Core_StepModule.CHEFZ][(int)CHEFZ_INPUT.Scanner_Protect_area] == false)
                    return false;
            }

            bool retBool = false;
            double iSpeed = 0;
            int fastechSpeed = 0;
            int fastechAbsPos = 0;
            double dActualPos = 0;

            if (actLoad == null || actLoad.IsDispose())
            {
                swStoppingTime.Restart();
                actLoad = new Externs.Action_Loading();
                actLoad.Open();
            }

            if (Define.IsDebugPass)
            {
                iStoppingFlag = 100;
            }

            
            //actLoad.UpdateProgress($"<Stopping : {iStoppingFlag}>  {strDesc} [{swStoppingTime.ElapsedMilliseconds} ms]");

            var cobotToolInput = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 021);
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;
            var cobotServoOn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 260);
            bool IsRobotServoOn = Convert.ToBoolean(cobotServoOn.iData);
            var cobotEmcStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 261);
            var cobotSafeStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 262);

            actLoad.UpdateProgress($"<Stopping : {iStoppingFlag}>  {strDesc} [{swResettingTime.ElapsedMilliseconds} ms] {cobotState}");


            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);



            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            bool[] InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];

            var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
            var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);

            switch (iStoppingFlag)
            {
                case 00://동작 중인 타이머 확인 하기, 동작 중이면 경고
                    strDesc = "로봇 정지 대기 중 입니다.";
                    Cores.Core_Object.GetObj_File.iAutoMode = 0;


                    if (Define.IsCobotDebugMove == false)
                    {
                        if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false)
                        {
                            Cores.Fas_Motion.StopMotion(MOTOR, false);
                        }

                        //jace
                        //if (OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == true)
                        //{
                        //    OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = false;
                        //    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        //    
                        //}

                        iStoppingFlag = 1;
                    }
                    else
                    {
                        iStoppingFlag = 100;
                    }
                    break;

                case 01:
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false)
                    {
                        Cores.Fas_Motion.StopMotion(MOTOR, false);
                    }
                    iStoppingFlag = 2;
                    break;

                case 02:
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true)
                    {
                        iStoppingFlag = 10;
                    }
                    break;

                case 10://
                    strDesc = "로봇 명령 초기화 중 입니다.";
                    if (OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] == true && InStateBuff[(int)COBOT_INPUT.Comp] == true)
                    {
                        //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos1] = false;
                        //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos2] = false;
                        //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos3] = false;
                        //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos4] = false;
                        OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] = false;
                        //OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 11;
                    }
                    else if (OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] == false && InStateBuff[(int)COBOT_INPUT.Comp] == false)
                    {
                        iStoppingFlag = 50;
                    }
                    break;

                case 11:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] == false && InStateBuff[(int)COBOT_INPUT.Comp] == false)
                    {
                        iStoppingFlag = 50;
                    }
                    break;

                case 12:
                    if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 20;
                    }
                    break;

                case 20:
                    iStoppingFlag = 30;
                    break;

                case 30:
                    for (int i = 0; i < OutStateBuff.Length; i++)
                    {
                        OutStateBuff[i] = false;
                    }
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    iStoppingFlag = 31;
                    break;

                case 31:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == false && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 32;
                        //Thread.Sleep(500);
                    }
                    else if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true)
                    {
                        iStoppingFlag = 32;
                    }
                    break;
                
                case 32:
                    if (OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] == true && OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] == true)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                        OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

                        iStoppingFlag = 33;
                    }                 
                    break;

                case 33:
                    OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] == false &&
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 34;
                    }
                    else if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                        && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                    {
                        iStoppingFlag = 42;
                    }
                    else if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY
                      && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                    {
                        iStoppingFlag = 42;
                    }
                    break;

                case 34:
                    OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                    if (OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] == false &&
                        OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] = true;
                        OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 35;
                    }
                    break;             

                case 35:
                    OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                    if (OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 36;
                    }
                    dtStoppingTime = DateTime.Now;
                    break;


                case 36:
                    strDesc = "로봇 서보 온 확인 중 입니다.";
                    OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                    if (IsRobotServoOn)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        iStoppingFlag = 40;
                    }
                    tsStoppingTime = DateTime.Now - dtStoppingTime;
                    if (dtStoppingTime.Ticks >= DateTime.Now.Ticks)
                    {
                        dtStoppingTime = DateTime.Now;
                    }
                    else if (tsStoppingTime.TotalSeconds >= 15)
                    {
                        iStoppingFlag = 20;
                    }

                    break;

                case 37:
                
                    
                    break;     

                case 40:
                    iStoppingFlag = 41;
                    break;

                case 41:
                    if (InStateBuff[(int)COBOT_INPUT.Tasking] == true
                        && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == false)
                    {
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        //Thread.Sleep(500);
                        iStoppingFlag = 42;
                    }
                    else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false)
                    {
                        iStoppingFlag = 42;
                    }
                    else
                    {
                        //iStoppingFlag = 20;
                    }
                    break;

                case 42:
                    if (InStateBuff[(int)COBOT_INPUT.Tasking] == false
                       && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                    {
                        //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                        //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                        //low active
                        iStoppingFlag = 43;
                    }
                    else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false
                        && OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == false)
                    {
                        //iResettingFlag = 0;
                    }
                    else if (InStateBuff[(int)COBOT_INPUT.Tasking] == true)
                    {
                        iStoppingFlag = 41;
                    }
                    break;

                case 43:
                    if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING
                      || cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
                    {
                        if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                        {
                            iStoppingFlag = 60;
                        }
                        else
                        {
                            iStoppingFlag = 50;
                        }
                    }
                    break; 



                case 50:
                    ModBusSendEvent(129, 13);
                    iStoppingFlag = 51;
                    Thread.Sleep(500);
                    break;

                case 51:
                    if (manualCommand.iData != 0 && manualComplted.iData == 0)
                    {
                        iStoppingFlag = 52;
                    }
                    else
                    {
                        iStoppingFlag = 50;
                    }
                    break;
                    //2023.05.22 ::: 52step 에러처리 필요 로봇이 죽어 있음
                case 52:
                    if (manualCommand.iData != 0 && manualComplted.iData != 0)
                    {
                        ModBusSendEvent(129, 0);
                        iStoppingFlag = 53;
                        Thread.Sleep(500);
                    }
                    else if (manualCommand.iData == 0 && manualComplted.iData == 0)
                    {
                        iStoppingFlag = 53;
                    }


                    if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.SAFE_OFF)
                    {
                        //2023.06.16
                        iStoppingFlag = 100;
                    }
                    break;

                case 53:
                    if (manualCommand.iData == 0 && manualComplted.iData == 0)
                    {
                        if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                        {
                            iStoppingFlag = 60;
                        }
                        else
                        {
                            iStoppingFlag = 50;
                        }
                    }
                    break;
              

                case 60:
                    strDesc = "로봇 대기 위치 이동 중 입니다.";
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true)
                    {
                        if (Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]))
                        {
                            iSpeed = 250 * Cores.Core_Object.GetObj_File.iXaxisSpeed * 0.01;
                            fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)50);
                            fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1]);
                            //Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                            Cores.Fas_Motion.MovePos(MOTOR, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                            Thread.Sleep(100);
                            //iStoppingFlag = 70;
                        }
                        iStoppingFlag = 70;
                    }
                    break;

                case 70:
                    //X_Axis_Override();
                    strDesc = "로봇 대기 위치 확인 중 입니다.";
                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == true)
                    {
                        dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                        if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                           && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                        {
                            iStoppingFlag = 80;
                            //iStoppingFlag = 80;
                        }
                    }
                    break;

                case 71:
                    //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                  
                    //else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false)
                    //{
                    //    iStoppingFlag = 72;
                    //}
                    //else if (OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                    //{
                    //    OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                    //    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    //    Thread.Sleep(100);
                    //}
                    //
                    //if (InStateBuff[(int)COBOT_INPUT.SafeTouque] == false)
                    //{
                    //    iStoppingFlag = 0;
                    //}
                    break;

                case 72:
                    strDesc = "로봇 실행 중 입니다.";
                    //OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                    //InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                 
                    //else if(InStateBuff[(int)COBOT_INPUT.Tasking] == true)
                    //{
                    //    iStoppingFlag = 71;
                    //}
                    //else if (InStateBuff[(int)COBOT_INPUT.Tasking] == false)
                    //{
                    //    iStoppingFlag = 73;
                    //}
                    break;

                case 73:
                 

                    break;



                //case 71:
                //    strDesc = "로봇 운전 준비 중 입니다.";
                //    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == false)
                //    {
                //        OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                //        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                //        iStoppingFlag = 72;
                //    }
                //    break;

                //case 72:
                //    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] == true)
                //    {
                //        iStoppingFlag = 73;
                //    }
                //    break;

                //case 73:
                //    if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_STANDBY)
                //    {
                //        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
                //        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                //        iStoppingFlag = 74;
                //        System.Threading.Thread.Sleep(500);
                //    }
                //    else if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING)
                //    {
                //        //OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                //        //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                //        iStoppingFlag = 75;
                //    }
                //    break;

                //case 74:
                //    strDesc = "로봇 운전 중 입니다.";
                //    if (OutStateBuff[(int)COBOT_OUTPUT.Task_Start] == true)
                //    {
                //        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                //        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                //        iStoppingFlag = 75;
                //        System.Threading.Thread.Sleep(500);
                //    }
                //    break;

                //case 75:
                //    if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING)
                //    {
                //        OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                //        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                //        iStoppingFlag = 80;
                //    }
                //    else
                //    {
                //        //System.Threading.Thread.Sleep(500);
                //        //iStoppingFlag = 73;
                //    }
                //    //iStoppingFlag = 80;
                //    break;

                case 80://로봇 대기 위치 확인
                    iStoppingFlag = 90;
                    break;

                case 90://엑스축 대기 위치 확인
                    iStoppingFlag = 100;
                    break;

                case 100:
                    iExcuteFlag = 0;
                    actLoad.Close();
                    swStoppingTime.Stop();
                    retBool = true;
                    //iStoppingFlag = 101;
                    break;


                default:
                    break;
            }

            if (iPrevStoppingFlag != iStoppingFlag)
            {
                iPrevStoppingFlag = iStoppingFlag;
                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    devJace.Program.ELogLevel.Debug,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | FLAG : [{iStoppingFlag:000}]");
            }


            return retBool;
        }

        /// <summary>
        /// 잠시 멈춤 동작 시퀀스
        /// </summary>
        /// <returns></returns>
        bool Hold_Action()
        {
            /*
             * 2023.03.08 ::: 일시정시 시퀀스 
             * 1. 전동작을 기억한다.
             * 2. 엑스 축이 이동 중이면 정지한다.
             * 3. 로봇이 움직이면???
             * 
             * 
             */

            bool retBool = false;

            IsTaskPause = true;
            IsTaskResume = false;
            iPrevExcuteFlag = iExcuteFlag;

            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];

            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false)
                          
            {
                Cores.Fas_Motion.StopMotion(MOTOR, false);
            }
            
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos1] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos2] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos3] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos4] = false;
            OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] = false;

            OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = false;

            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

            retBool = true;


            return retBool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool UnHold_Action()
        {
            /*
             * 2023.03.08 ::: 일시정시 시퀀스 
             * 1. 전동작을 기억한다.
             * 2. 엑스 축이 이동 중이면 정지한다.
             * 3. 로봇이 움직이면???
             * 
             * 
             */

            bool retBool = false;

            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos1] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos2] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos3] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Move_Pos4] = false;
            OutStateBuff[(int)COBOT_OUTPUT.Move_Cmd] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
            //OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = false;
            //OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;

            OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
            OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = true;
            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

            Thread.Sleep(1000);


            OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
            Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

            retBool = true;


            return retBool;
        }





    }//class
}//name