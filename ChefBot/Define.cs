using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace devi
{
    public static class Define
    {
        //개발자 전용 ::: 
        public static bool IsSupervisor = false;
        //public static bool IsSupervisor = true;


        public static bool IsDisposed = false;

        public static List<long> lDaysCount = new List<long>();
        public static List<long> lMonthCount = new List<long>();
        public static List<long> lWeekCount = new List<long>();       


        //information
        public static Color colorMainBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
        public static Color colorSubButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(56)))), ((int)(((byte)(100)))));
        public static Color colorMainButtonBoldColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(114)))), ((int)(((byte)(196)))));
        public static Color colorMainButtonLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
        public static Color colorChartMainBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(255)))), ((int)(((byte)(246)))));

               

        public static EGRIPS eGripper = EGRIPS.ZIMMER;
        public static EUSER eUSER = EUSER.Oper;
        //public static ECUSTOM eCUSTOM = ECUSTOM.Demo;
        public static ECUSTOM eCUSTOM = ECUSTOM.KongMan;


        //초기 로딩시 소리 없애는 용도
        public static bool IsFirstSoundOff = false;

        ////2023.04.17 ::: Robot Debug Mode
        public static bool IsCobotDebugMove = false;//resetting action cobot pass, 다트 플랫폼 등등 사용 할때
        ////===> Define 으로 이동
        ///

        //2023.05.06 ::: 셋업용 IO 모션 체크 용
        public static bool IsXaxisDebugMove = false;//인터락

        public static bool IsAdministrator = false;

        public static bool IsDebugPass = false; //소프트웨어만을 위한 디버그 변수    

        //2023.05.23 로봇 리커버리 모드
        public static bool IsRobotReCoverlyMode = false;


        //2023.05.25 ::: 입력센서 공만,데모 동일하게 하드웨어 배선 변경 함.
        public const int iSensorLocateOffsset = 4;

        //public const int iSensorLocateOffsset = 6;
    }


    public enum EGRIPS
    { 
        ZIMMER, DH, OnRobot 
    }

    public enum EUSER
    { 
        None, Oper, Admin
    }

    public enum ECUSTOM
    { 
        Demo, KongMan, Kyuchon
    }
}
