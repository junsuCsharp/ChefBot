using devi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cores
{
    public class Fas_Data
    {
        public int iAxisNumber = 0;

        public static int iCmdPos = 0;
        public static int iActPos = 0; 
        public static int iActVel = 0;
        public static int iPosError = 0;
        public static bool IsOrgRetOk = false;

        public static bool[] lstAxis_State = new bool[(int)EAxis_Status.Max];
        public static bool[] lstInput_State = new bool[(int)EFas_UsrInput.Max];
        public static bool[] lstOutput_State = new bool[(int)EFas_UsrOutput.Max];

        //public int iCmdVel = 0;
        public static int iHomeOffset = 0;


        //계속
        public static List<bool[]> lstIO_InState = new List<bool[]>() {new bool[16], new bool[16], new bool[16], new bool[16] };
        public static List<bool[]> lstIO_OutState = new List<bool[]>() { new bool[16], new bool[16], new bool[16], new bool[16] };

        //public static List<bool[]> lstIO_OutBuffer = new List<bool[]>() { new bool[16], new bool[16], new bool[16], new bool[16] };

        //요거는 계속 유지하고 있어야 함.
        public static uint[] usInputState = new uint[4];
        public static uint[] usOutputState = new uint[4];

        //2023.05.09 ::: 가감속 S 커브
        public static int nAccDecTime = 750;

     
    }

    public static class Fas_Func
    {
        //Pos Convert ::: PPS -> msec

        //Spd Convert ::: PPS -> msec

        //Jog Speed Max ::: 2,500,000, default 5,000

        //Hom Speed Max ::: 500,000, default 5,000

        //파나소닉 샘플 25mm 0.0025 2500um ::: 1pulse & 2.5um 4000 10mm

        //res(모터 분해능) = 25mm / 10000pulse = 0.0025mm/pulse
        //pps(Plus Command) = tagetPos / res

        const int iPulseRef = 2500;
        const double dMiliterRef = 25;

        //2023.05.24 엑스축 파나소닉 앰프 400W -> 750W 변경 및 감속기 제거로 인하여 변경
        const int iReduction = 5;
        //const int iReduction = 1;

        /// <summary>
        /// 사용자에 받은 미리미터를 pps로 변환하여 파스텍 전송
        /// </summary>
        /// <param name="mmTaget"></param>
        /// <returns></returns>
        public static int PPS_To_mm(double mmTaget)
        {
            int iRet = -99999;
            int iEnc = 4; //4체베
            int um = 1000;
            double ppr = dMiliterRef / iReduction * um / (iPulseRef * iEnc) ;
            iRet = (int)(mmTaget * um / ppr);
            return iRet;
        }

        /// <summary>
        /// 사용자에 받은 미리미터를 pps로 변환하여 파스텍 전송
        /// 2022.12.23 ::: 소수점 처리를 위한 함수, 사용하지 않아도 됨, 
        /// </summary>
        /// <param name="mmTaget"></param>
        /// <param name="IsPosition"></param>
        /// <returns></returns>
        public static int PPS_To_mm(double mmTaget, bool IsPosition)
        {
            int iRet = -99999;
            int iEnc = 4; //4체베
            int um = 1000;
            double ppr = dMiliterRef * um / (iPulseRef * iEnc);
            double dConData = (mmTaget * um / ppr);
            iRet = (int)dConData;
            return iRet;
        }

        /// <summary>
        /// 파스텍에서 받은 위치 정보를 사용자에 맞게 미리미터 변환
        /// </summary>
        /// <param name="getPPS"></param>
        /// <returns></returns>
        public static double PPR_To_mm(int getPPS)
        {
            int iEnc = 4; //4체베
            int um = 1000;
            double ppr = dMiliterRef / iReduction * um / (iPulseRef * iEnc) ;

            double dRet = (getPPS * ppr * 0.001) ;
            dRet = Math.Round(dRet, 2);
            //dRet = Math.Truncate(dRet);
            return dRet;
        }
    }

    public enum EAxis_Status
    {
        /*
        None,

        Error_All,
        HW_Plus_Limit,
        HW_Miuns_Limit,
        SW_Plus_Limit,
        SW_Miuns_Limit,
        Reserved,
        Pos_Counrt_Over,
        Err_Sevo_Alarm,

        Err_Over_Current,
        Err_Over_Speed,
        Err_Step_Out,
        Err_Over_Load,
        Err_Over_Heat,
        Err_Back_EMF,
        Err_Motor_Power,
        Err_Inposition,

        Emg_Stop,
        Slow_Stop,
        Org_Returning,
        Inposition,
        Servo_On,
        Alarm_Reset,
        PT_Stopped,
        Origin_Sensor,

        Z_Pulse,
        Org_Ret_OK,
        Motion_DIR,
        Motioning,
        Motion_Pause,
        Motion_Accel,
        Motion_Decel,
        Motion_Constant,

        Max
        */

        /*
        None=0,

        Err_Sevo_Alarm=1,
        HW_Plus_Limit=2,
        HW_Miuns_Limit=3,
        SW_Plus_Limit=4,
        SW_Miuns_Limit=5,
        Reserved=6,
        //Reserved,
        //Reserved,

        //Reserved,
        //Reserved,
        //Reserved,
        //Reserved,
        //Reserved,
        //Reserved,
        //Reserved,
        //Reserved,

        Emg_Stop=17,
        Slow_Stop=18,
        Org_Returning=19,
        Inposition=20,
        Servo_On=21,
        Alarm_Reset=22,
        PT_Stopped=23,
        Origin_Sensor=24,

        Z_Pulse=25,
        Org_Ret_OK=26,
        Motion_DIR=27,
        Motioning=28,
        Motion_Pause=29,
        Motion_Accel=30,
        Motion_Decel=31,
        Motion_Constant=32,

        Max=33
        */

        Err_Sevo_Alarm  = 0,
        HW_Plus_Limit   = 1,
        HW_Miuns_Limit  = 2,
        SW_Plus_Limit   = 3,
        SW_Miuns_Limit  = 4,
        Reserved        = 5,      

        Emg_Stop        = 16,
        Slow_Stop       = 17,
        Org_Returning   = 18,
        Inposition      = 19,
        Servo_On        = 20,
        Alarm_Reset     = 21,
        PT_Stopped      = 22,
        Origin_Sensor   = 23,

        Z_Pulse         = 24,
        Org_Ret_OK      = 25,
        Motion_DIR      = 26,
        Motioning       = 27,
        Motion_Pause    = 28,
        Motion_Accel    = 29,
        Motion_Decel    = 30,
        Motion_Constant = 31,

        Max = 32

    }

    public enum EFas_Pin
    { 
        None, Voltage_24, Voltage_Gnd, Vdc_24, Gnd_24, Limit_Plus, Limit_Minus, Origin, Input, Brake, Output, Max
    }

    public enum EFas_UsrInput
    {
        //Clear_Pos, Soft_Stop, Jog_Plus, Jog_Minus, Alarm_Reset, Servo_On, Pause, Origin_Search, Teaching, E_Stop, User_In, Max
        Plus_Limit = 0, Miuns_Limit = 1, Orighin = 2, Clear_Pos = 3, Stop = 13, Jog_Plus = 14, Jog_Minus = 15, Alarm_Reset = 16, Servo_On = 17, Pause = 18, Origin_Search = 19, E_Stop = 21, User_In = 26, Max = 32
    }

    public enum EFas_UsrOutput
    { 
       Compare_Out = 0, InPosition = 1, Alarm = 2, Moving = 3, Acc_Dec = 4, ACK = 5, END = 6, OriginSerchOK = 8, ServoReady =9, User_Out = 15, Max = 16
    }

    public enum EActvie
    { 
       None, Low, High, ActvieLow, ActvieHigh, Max
    }

    public enum EOrgSearch
    {
        //Org Method :
        //원점 복귀 명령의 종류를 선택합니다.
        //♦0 : ‘Org Speed’값에 의해 원점 센서 지점까지 이동 후, 저속의 ‘Org Search Speed’값으로 정밀 원점 복귀를 실시합니다.
        //♦1 : ‘Org Speed’ 값에 의해 원점 센서 지점까지 이동 후, 저속의 ‘Org Search Speed’값으로 Z-pulse 원점 복귀를 실시합니다.
        //♦2 : ‘Org Speed’값에 의해 Limit 센서 감지 지점까지 이동 후 즉시 정지합니다.
        //♦3 : ‘Org Speed’값에 의해 Limit 센서 감지 지점까지 이동 후, 저속의 ‘Org Search Speed’값으로 Z-pulse 원점 복귀를 실시합니다.
        //♦4 :   저속의 ‘Org Search Speed’값으로 Z-pulse 원점 복귀를 실시합니다.        
        //♦5 :   현재의 위치를 원점으로 설정할 때 사용됩니다.
    }

}
