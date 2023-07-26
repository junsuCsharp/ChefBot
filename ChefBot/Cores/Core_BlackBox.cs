using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using PackML;
using static Forms.FormErrorBox;

namespace Cores
{
    
    public class Core_BlackBox
    {
        /*
         * 2022.12.20 ::: 알람 모듈 설계
         * 
         * 
         * 
         * 
         */
     
        const int SET = 1;
        const int RESET = 0;

        private int iAlarmStatus = (int)General.NormalAlarm;
        private int iAlarmEvent = (int)ECode.Normal;
        private int iAlarmReset = SET;

        private ulong uLowBuffer = 0;
        private ulong uMiddleBuffer = 0;
        private ulong uHighBuffer = 0;

        public event EventHandler OnAlarm;
        public delegate void EventHandler(object sender, EventArgs e);

        public static List<AttributesEventArgs> lstAlarmTotal = new List<AttributesEventArgs>();
        static int iAlarmLevel = 0;

        public static int CurrentAlarmLevel()
        {
            return iAlarmLevel;
        }

        public Core_BlackBox()
        {
            AlarmInstance();


            Thread threadProcessor = new Thread(new ThreadStart(Progress)) { IsBackground = true };
            threadProcessor.Start();
        }

        void AlarmInstance()
        {
            AttributesEventArgs attributes;

            //int alarmIndex = 0;

            attributes = new AttributesEventArgs();
            attributes.Index = 0;
            attributes.IsAlarmUse = true;
            attributes.strName = "DC24V 전원_CP";
            attributes.strMessage = "DC 24전원 CP가 차단되었습니다.";
            attributes.strDescription = "DC24V 전원 CP를 올려주세요.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 1;
            attributes.IsAlarmUse = true;
            attributes.strName = "컨트롤 유닛_CP";
            attributes.strMessage = "컨트롤 유닛 CP가 차단되었습니다.";
            attributes.strDescription = "컨트롤 유닛 CP를 올려주세요.";
            lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.Index = 2;
            //attributes.IsAlarmUse = true;
            //attributes.strName = "환풍팬_CP";
            //attributes.strMessage = "환풍팬 전원 CP가 차단되었습니다.";
            //attributes.strDescription = "환풍팬 전원 CP를 올려주세요.";
            //lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 2;
            attributes.IsAlarmUse = true;
            attributes.strName = "로봇 전원_CP";
            attributes.strMessage = "로봇전원 CP가 차단되었습니다.";
            attributes.strDescription = "로봇 전원 CP를 올려주세요.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 3;
            attributes.IsAlarmUse = true;
            attributes.strName = "x축 전원_CP";
            attributes.strMessage = "x축 전원 CP가 차단되었습니다.";
            attributes.strDescription = "x축 전원 CP를 올려주세요.";
            lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.Index = 5;
            //attributes.IsAlarmUse = true;
            //attributes.strName = "컨트롤 전원_CP";
            //attributes.strMessage = "비상 정지 상태입니다.";
            //attributes.strDescription = "컨트롤 전원 CP를 올려주세요.";
            //lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 4;
            attributes.IsAlarmUse = true;
            attributes.strName = "도어 열림";
            attributes.strMessage = "전장함 왼쪽 도어 열렸습니다.";
            attributes.strDescription = "전장함 왼쪽 도어 센서 및 문열림을 확인하여 주세요.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 5;
            attributes.IsAlarmUse = true;
            attributes.strName = "도어 열림";
            attributes.strMessage = "전장함 오른쪽 도어 열렸습니다.";
            attributes.strDescription = "전장함 오른쪽 도어 센서 및 문열림을 확인하여 주세요.";
            lstAlarmTotal.Add(attributes);


            attributes = new AttributesEventArgs();
            attributes.Index = 6;
            attributes.IsAlarmUse = true;
            attributes.strName = "EMC(1)";
            attributes.strMessage = "로봇 비상정지.";
            attributes.strDescription = "로봇 비상 정지 해제 후 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 7;
            attributes.IsAlarmUse = true;
            attributes.strName = "EMC(2)";
            attributes.strMessage = "로봇 비상정지.";
            attributes.strDescription = "로봇 비상 정지 해제 후 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 8;
            attributes.IsAlarmUse = false;
            attributes.strName = "HOME_ALARM";
            attributes.strMessage = "로봇이 원점 위치가 아닙니다.";
            attributes.strDescription = "로봇 원점 위치를 확인해 주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 9;
            attributes.IsAlarmUse = false;
            attributes.strName = "ABNOMAL(1)";
            attributes.strMessage = "비정상 동작 입니다.";
            attributes.strDescription = "비정상 동작입니다. 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 10;
            attributes.IsAlarmUse = false;
            attributes.strName = "ABNOMAL(2)";
            attributes.strMessage = "비정상 동작 입니다.";
            attributes.strDescription = "비정상 동작입니다. 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 11;
            attributes.IsAlarmUse = true;
            attributes.strName = "SAFETORQUE";
            attributes.strMessage = "로봇에 충돌이 감지 되었습니다.";
            attributes.strDescription = "로봇 충돌 원인 확인 후 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 12;
            attributes.IsAlarmUse = false;
            attributes.strName = "SAFESTOP";
            attributes.strMessage = "안전 정지.";
            attributes.strDescription = "로봇 안전 정지 해제 후 원점을 잡아주세요.";
            lstAlarmTotal.Add(attributes);

            //2023.03.28 레이저 스캐너 알람 추가
            attributes = new AttributesEventArgs();
            attributes.Index = 13;
            attributes.IsAlarmUse = true;
            attributes.strName = "EMO_1_STOP";
            attributes.strMessage = "EMO버튼 1 비상정지";
            attributes.strDescription = "비상 정지 상태를 해제 해주세요.";
            lstAlarmTotal.Add(attributes);
            
            attributes = new AttributesEventArgs();
            attributes.Index = 14;
            attributes.IsAlarmUse = true;
            attributes.strName = "EMO_2_STOP";
            attributes.strMessage = "EMO버튼 2 비상정지";
            attributes.strDescription = "비상 정지 상태를 해제 해주세요.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 15;
            attributes.IsAlarmUse = true;
            attributes.strName = "로봇 엔코더";
            attributes.strMessage = "엔코더 초기화 알람";
            attributes.strDescription = "-.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 16;
            attributes.IsAlarmUse = true;
            attributes.strName = "ETHERNET";
            attributes.strMessage = "네트워크 알람";
            attributes.strDescription = "디바이스 장치 연결을 확인 하여 주세요";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 17;
            attributes.IsAlarmUse = true;
            attributes.strName = "X-Axis Following";
            attributes.strMessage = "단축 로봇 위치 알람";
            attributes.strDescription = "단축 로봇을 원점 검색 바랍니다.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 18;
            attributes.IsAlarmUse = true;
            attributes.strName = "배출 바스켓";
            attributes.strMessage = "배출 바스켓 알람";
            attributes.strDescription = "배출 바스켓을 모두 제거하여 주시기 바랍니다.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 19;
            attributes.IsAlarmUse = true;
            attributes.strName = "왼쪽버튼 일시 정지";
            attributes.strMessage = "긴급 정지 중 일시정지 알람";
            attributes.strDescription = "일시정지 버튼 누름 상태 해제 후 사용 바랍니다.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 20;
            attributes.IsAlarmUse = true;
            attributes.strName = "SW 일시 정지";
            attributes.strMessage = "긴급 정지 중 일시정지 알람";
            attributes.strDescription = "일시정지 버튼 누름 상태 해제 후 사용 바랍니다.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 21;
            attributes.IsAlarmUse = true;
            attributes.strName = "오른쪽버튼 일시 정지";
            attributes.strMessage = "긴급 정지 중 일시정지 알람";
            attributes.strDescription = "일시정지 버튼 누름 상태 해제 후 사용 바랍니다.";
            lstAlarmTotal.Add(attributes);

            attributes = new AttributesEventArgs();
            attributes.Index = 22;
            attributes.IsAlarmUse = true;
            attributes.strName = "단축 로봇 알람";
            attributes.strMessage = "외력 발생 중 모션 알람";
            attributes.strDescription = "-.";
            lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_error";
            //attributes.strMessage = "레이저 스캐너(에러)";
            //attributes.strDescription = "에러를 해제 해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_alarm";
            //attributes.strMessage = "레이저 스캐너(알람)";
            //attributes.strDescription = "알람을 해제 해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_OSSD";
            //attributes.strMessage = "레이저 스캐너(OSSD)";
            //attributes.strDescription = "???????????????.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_Protect_area";
            //attributes.strMessage = "보호 영역 검출";
            //attributes.strDescription = "보호 영역에서 벗어나세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_Waring_area";
            //attributes.strMessage = "경고 영역 검출";
            //attributes.strDescription = "경고 영역에서 벗어나세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "Scanner_Operation";
            //attributes.strMessage = "레이저 스캐너(구동상태)";
            //attributes.strDescription = "레이저 스캐너 구동상태를 확인해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "SafetyController_Operation";
            //attributes.strMessage = "안전 컨트롤러(구동상태)";
            //attributes.strDescription = "안전 컨트롤러 구동상태를 확인해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "SafetyController_Power";
            //attributes.strMessage = "안전 컨트롤러(전원불량)";
            //attributes.strDescription = "안전 컨트롤러 전원을 확인해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "SafetyController_Wire";
            //attributes.strMessage = "안전 컨트롤러(배선불량)";
            //attributes.strDescription = "안전 컨트롤러 배선을 확인해주세요.";
            //lstAlarmTotal.Add(attributes);

            //attributes = new AttributesEventArgs();
            //attributes.IsAlarmUse = true;
            //attributes.strName = "SafetyController_Signal";
            //attributes.strMessage = "안전 컨트롤러(신호불량)";
            //attributes.strDescription = "안전 컨트롤러 신호를 확인해주세요.";
            //lstAlarmTotal.Add(attributes);


        }

        public void Dispose()
        {
            
        }

        public enum ECode
        {
            Normal, Low, Middle, High, Max
        }

        public enum General
        {
            NormalAlarm, UnacknowledgedAlarm, AcknowledgedAlarm, RTNunacknowledged, SuppressedAlarm, Max
        }

        public enum EActive
        { 
            Low, High
        }

        void Progress()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (iAlarmStatus == (int)General.NormalAlarm)
                {
                    Event_Alarm();//알람 이벤트 발생 확인
                }
                if (iAlarmEvent >= (int)ECode.Normal)
                {
                    switch ((General)iAlarmStatus)
                    {
                        case General.NormalAlarm://정상상태 프로세스
                            iAlarmStatus = (int)General.UnacknowledgedAlarm;                            
                            break;
                        case General.UnacknowledgedAlarm://알람처리 프로세스
                            AlarmProcess(iAlarmEvent);//프로세스 코딩 부분
                            break;
                        case General.AcknowledgedAlarm://알람 리셋을 해야 만 해제 프로세스
                            if (iAlarmReset == RESET)//프로세스 코딩 부분
                            {
                                iAlarmStatus = (int)General.NormalAlarm;
                                iAlarmEvent = RESET; //초기화
                            }
                            break;
                        case General.RTNunacknowledged://알람이 문제가 없으면 응답없이 해제 프로세스
                            if (iAlarmReset == RESET)//프로세스 코딩 부분
                            {
                                iAlarmStatus = (int)General.NormalAlarm;
                                iAlarmEvent = RESET; //초기화
                            }
                            break;
                        case General.SuppressedAlarm://비상정지 프로세스
                            if (iAlarmReset == RESET)//프로세스 코딩 부분
                            {
                                iAlarmStatus = (int)General.NormalAlarm;
                                iAlarmEvent = RESET; //초기화
                            }
                            break;
                        default:
                            break;
                    }
                }

                iAlarmLevel = iAlarmEvent;

            }//while
        }

        /// <summary>
        /// 2023.03.22
        /// 기본 알람 매니져
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="eActive"></param>
        /// <param name="msecDelay"></param>
        /// <param name="eCode"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        public void Alarm_Manager(int Category, EActive eActive, int msecDelay, ECode eCode, int bitIndex, int value)
        {
            //iAlarmEvent = (int)attributes.EType;
            //신호 감시
            //알람확인하여 이벤트 발생
            if (lstAlarmTotal.Count == 0)
                return;
            if (lstAlarmTotal[Category].IsAlarmOn == false)
            {
                lstAlarmTotal[Category].dateTime = DateTime.Now;
                lstAlarmTotal[Category].EType = eCode;
                if (lstAlarmTotal[Category].IsTimerSet == false)
                {
                    lstAlarmTotal[Category].IsTimerSet = true;
                    //lstAlarmTotal[Category].timeSpan = lstAlarmTotal[Category].dateTime.AddMilliseconds(msecDelay)
                    lstAlarmTotal[Category].delayTime = lstAlarmTotal[Category].dateTime.AddMilliseconds(msecDelay);
                }
                if ((int)eActive == ((value >> bitIndex) & 1) && msecDelay == 0)
                {
                    lstAlarmTotal[Category].IsAlarmOn = true;
                    OnAlarmReached(lstAlarmTotal[Category]);
                }
                else if ((int)eActive != ((value >> bitIndex) & 1) && msecDelay > 0)
                {
                    lstAlarmTotal[Category].IsTimerSet = false;
                    //lstAlarmTotal[Category].IsAlarmOn = false;
                }
                else if ((int)eActive == ((value >> bitIndex) & 1) && msecDelay > 0)
                {
                    if (lstAlarmTotal[Category].dateTime.Ticks >= lstAlarmTotal[Category].delayTime.Ticks)
                    {
                        lstAlarmTotal[Category].IsAlarmOn = true;
                        OnAlarmReached(lstAlarmTotal[Category]);
                    }
                }
            }
            else if (lstAlarmTotal[Category].IsAlarmOn == true)
            {

                switch (eCode)
                {
                    case ECode.Normal:
                        break;
                    case ECode.Low:
                        uLowBuffer++;
                        break;
                    case ECode.Middle:
                        uMiddleBuffer++;
                        break;
                    case ECode.High:
                        uHighBuffer++;
                        break;
                }
            }

            //AttributesEventArgs attributes = new AttributesEventArgs();
            //attributes.strDescription = "TEST";            
            //attributes.EType = eCode;
            //attributes.dateTime = DateTime.Now;            
            
        }

        /// <summary>
        /// GPIO 
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="currState"></param>
        /// <param name="setState"></param>
        /// <param name="currMode"></param>
        /// <param name="setMode"></param>
        /// <param name="eActive"></param>
        /// <param name="msecDelay"></param>
        /// <param name="eCode"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        public void Alarm_Manager(int Category, ECurrState currState, ECurrState setState, EModeMatrix currMode, EModeMatrix setMode , EActive eActive, int msecDelay, ECode eCode, int bitIndex, int value)
        {
            //iAlarmEvent = (int)attributes.EType;
            //신호 감시
            //알람확인하여 이벤트 발생
            if (lstAlarmTotal.Count == 0)
                return;

            if (lstAlarmTotal[Category].IsAlarmUse == false)
                return;

            if (currState == setState && currMode == setMode)
            {

                if (lstAlarmTotal[Category].IsAlarmOn == false)
                {
                    lstAlarmTotal[Category].dateTime = DateTime.Now;
                    lstAlarmTotal[Category].EType = eCode;
                    if (lstAlarmTotal[Category].IsTimerSet == false)
                    {
                        lstAlarmTotal[Category].IsTimerSet = true;
                        //lstAlarmTotal[Category].timeSpan = lstAlarmTotal[Category].dateTime.AddMilliseconds(msecDelay)
                        lstAlarmTotal[Category].delayTime = lstAlarmTotal[Category].dateTime.AddMilliseconds(msecDelay);
                    }
                    if ((int)eActive == ((value >> bitIndex) & 1) && msecDelay == 0)
                    {
                        lstAlarmTotal[Category].IsAlarmOn = true;
                        OnAlarmReached(lstAlarmTotal[Category]);
                    }
                    else if ((int)eActive != ((value >> bitIndex) & 1) && msecDelay > 0)
                    {
                        lstAlarmTotal[Category].IsTimerSet = false;
                        //lstAlarmTotal[Category].IsAlarmOn = false;
                    }
                    else if ((int)eActive == ((value >> bitIndex) & 1) && msecDelay > 0)
                    {
                        if (lstAlarmTotal[Category].dateTime.Ticks >= lstAlarmTotal[Category].delayTime.Ticks)
                        {
                            lstAlarmTotal[Category].IsAlarmOn = true;
                            OnAlarmReached(lstAlarmTotal[Category]);
                        }
                    }
                }
                else if (lstAlarmTotal[Category].IsAlarmOn == true)
                {

                    switch (eCode)
                    {
                        case ECode.Normal:
                            break;
                        case ECode.Low:
                            uLowBuffer++;
                            break;
                        case ECode.Middle:
                            uMiddleBuffer++;
                            break;
                        case ECode.High:
                            uHighBuffer++;
                            break;
                    }

                    //2023.04.25 ::: 이벤트 발생 했는데도, 재 이벤트 발생 시켜본다

                    if (uHighBuffer != 0 &&
                        currState != ECurrState.CurrentState_Aborted)
                    {
                        OnAlarmReached(lstAlarmTotal[Category]);
                    }
                }
            }                    

        }

        /// <summary>
        /// 모드 버스, 특정 데이터 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="tagetValue"></param>
        /// <param name="currState"></param>
        /// <param name="setState"></param>
        /// <param name="currMode"></param>
        /// <param name="setMode"></param>
        /// <param name=""></param>
        public void Alarm_Manager(int address, double tagetValue, ECurrState currState, ECurrState setState, EModeMatrix currMode, EModeMatrix setMode, EActive eActive, int msecDelay, ECode eCode)
        {
            //2023.04.06
            //foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            //{
            //    //온도가, 70이상 
            //    var Joint_Motor_Temperature_1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 300);
            //    var Joint_Motor_Temperature_2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 301);
            //    var Joint_Motor_Temperature_3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 302);
            //    var Joint_Motor_Temperature_4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 303);
            //    var Joint_Motor_Temperature_5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 304);
            //    var Joint_Motor_Temperature_6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 305);
            //
            //    int Joint_Motor_Temperature_1_int = Convert.ToInt32(Joint_Motor_Temperature_1.strData);
            //    int Joint_Motor_Temperature_2_int = Convert.ToInt32(Joint_Motor_Temperature_2.strData);
            //    int Joint_Motor_Temperature_3_int = Convert.ToInt32(Joint_Motor_Temperature_3.strData);
            //    int Joint_Motor_Temperature_4_int = Convert.ToInt32(Joint_Motor_Temperature_4.strData);
            //    int Joint_Motor_Temperature_5_int = Convert.ToInt32(Joint_Motor_Temperature_5.strData);
            //    int Joint_Motor_Temperature_6_int = Convert.ToInt32(Joint_Motor_Temperature_6.strData);
            //
            //    string logFatal = $"\r\n";
            //
            //    //조건 : 해당 어드레스, 값이 70이상/ 로그 저장
            //    if (Joint_Motor_Temperature_1_int>=70 && Joint_Motor_Temperature_2_int >= 70 && Joint_Motor_Temperature_3_int >= 70 && Joint_Motor_Temperature_4_int >= 70 && Joint_Motor_Temperature_5_int >= 70 && Joint_Motor_Temperature_6_int >= 70)
            //    {
            //        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal, $" | {logFatal}");
            //
            //        if (dat.Address>=300 && dat.Address<=305)
            //        {
            //            logFatal += $"[Address : {dat.Address:000} | Value :{dat.strData.PadRight(10)} | Desc : {dat.strDesc.TrimEnd().PadRight(35)}], \r\n";
            //        }
            //    }
            //}

            var buffData = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == address);
            if (string.IsNullOrEmpty(buffData.strData))
                return;

            double val = double.Parse(buffData.strData);

            switch (eActive)
            {
                case EActive.Low:
                    if (val <= tagetValue)
                    { 
                    
                    }
                    break;
                case EActive.High:
                    if (val >= tagetValue)
                    {

                    }
                    break;
            }

        }

        public void EX_Alarm_Manager()
        { 
        
        }
             

        public void Reset_Alarm()
        {
            iAlarmReset = RESET;

            uLowBuffer = 0;
            uMiddleBuffer = 0;
            uHighBuffer = 0;

            foreach (AttributesEventArgs args in lstAlarmTotal)
            {
                args.IsDisplay = false;
                args.IsAlarmOn = false;
                args.dateTime = DateTime.Now;
            }
        }

        private void Event_Alarm()
        {
            if (uLowBuffer != 0)
            {
                iAlarmEvent = (int)ECode.Low;
            }
            if (uMiddleBuffer != 0)
            {
                iAlarmEvent = (int)ECode.Middle;
            }
            if (uHighBuffer != 0)
            {
                iAlarmEvent = (int)ECode.High;
            }
        }

        private void AlarmProcess(int AlarmCode)
        {
            switch ((ECode)AlarmCode)
            {
                case ECode.Low: iAlarmStatus = (int)General.RTNunacknowledged; break;
                case ECode.Middle: iAlarmStatus = (int)General.AcknowledgedAlarm; break;
                case ECode.High: iAlarmStatus = (int)General.AcknowledgedAlarm; break;
                default:break;
            }
        }

        protected virtual void OnAlarmReached(AttributesEventArgs e)
        {
            EventHandler handler = OnAlarm;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public partial class AttributesEventArgs : EventArgs
        {
            public bool IsAlarmOn = false;
            public bool IsTimerSet = false;
            public bool IsAlarmUse = false;
            public DateTime dateTime;
            public DateTime delayTime;

            public string strDescription;                               
            public string strMessage;
            public string strName;
            public ECode EType;

            //2023.04.17 ::: 표시 한번 한 경우
            public bool IsDisplay = false;

            //2023.04.24 ::: 알람 인덱스
            public int Index = -1;

        }
    }

  

   

    
}
