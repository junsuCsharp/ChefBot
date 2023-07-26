using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace devJace.Files
{
    class Xml_Serializer
    {
    }


    /// <summary>
    /// app default file
    /// </summary>
    public partial class Xml_File
    {
        public string FilePath { get; } = $"{Application.StartupPath}";
        public string FileName { get; } = $"trace";

        public DateTime AppStartTime = DateTime.Now;
        public DateTime AppExistTime = DateTime.Now;

#if(DEBUG)
        public bool IsConsole { get; } = true;
#elif (TRACE)
        public bool IsConsole { get; } = false;
#endif

        public Xml_File()
        {
            //FilePath = $"{Application.StartupPath}";
            //Local_IP = new List<string>();
        }
    }

    public partial class Obj_File
    {
        //Operation
        public int iMaxOil = 6;
        public int iMaxMeck = 3;
        public int iMaxDevice = 6;
        public List<string> Device_IP = new List<string>();
        public List<string> Device_Name = new List<string>();
        public List<string> Local_IP = new List<string>();

        //public TimeSpan tsPauseSetTime = new TimeSpan(0, 0, 10);
        //public List<TimeSpan> tsChikenSetTime = new List<TimeSpan>() { new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0), new TimeSpan(0, 7, 0) };
        public int tsPauseSetMinTime = 0;
        public int tsPauseSetSecTime = 0;

        public List<int> tsChikenSetMinTime = new List<int>();
        public List<int> tsChikenSetSecTime = new List<int>();

        public List<bool> lstOilMeckUse = new List<bool>();
        public List<int> lstOilCheckdCount = new List<int>();
        public List<int> lstOilTemperature = new List<int>();

        public List<string> lstLoaderName = new List<string>();
        public List<string> lstFryerName = new List<string>();
        public List<string> lstLoaderSetIO = new List<string>();
        public List<string> lstCookerName = new List<string>();


        //Operation Recipe
        public int iMaxOperation = 8;
        public int iCobotSpeed = 100;
        public int iXaxisSpeed = 100;
        public int iXaxisAccDecTime = 750;

        public int iLoadDelayTime = 3;
        public int iUnLoadDelayTime = 3;
        public int iBasketDelayTime = 7;

        public int iSwitch1SetTimeMin = 12;
        public int iSwitch1SetTimeSec = 0;

        public int iSwitch2SetTimeMin = 5;
        public int iSwitch2SetTimeSec = 0;


        public List<string> lstOperRecipeName = new List<string>();
        public List<string> lstOperRecipeRange = new List<string>();
        public List<string> lstOperRecipeUnit = new List<string>();


        //Operation Option
        public int iMaxOption = 11;
        public int iLanguage = 0;
        public int iXaxisControl = 1;        
        public int iBasketShaking = 0;
        public int iBasketShakingCount = 5;
        public double dBasketWeight = 0; //소수점 3자리  
        public int iMidShakingCount = 3;
        public int iMidOtwoShower = 3;
        public int iGripperUse = 1;
        public int iAutoMode = 0;
        public int iManualMode = 0;
        public int iLaserScannerUse = 0;
        public int iAutoClear = 0;
        public int iRecipeDownload = 0;
        public int iSyncModbus = 0;
        public int iOilDrainUse = 0;
        public int iOilDrainCount = 5;

        //2023.05.04 ::: 티치 옵세 사용유무, 초기화 추가
        public int iTechingUse = 0;
        public int iTechingInt = 0;

        public int iGripperModel = 0;

        //2023.05.25
        public int iCustomUser = 1;

        

        public List<string> lstOptionName = new List<string>();
        public List<string> lstOptionRange = new List<string>();
        public List<string> lstOptionUnit = new List<string>();



        public int iMaxExOption = 11;
        public int iBasketShakingSetMinTime = 1;
        public int iBasketShakingSetSecTime = 0;
        public int iBasketOxzenSetMinTime = 4;
        public int iBasketOxzenSetSecTime = 0;

        public int inputdelayMintime = 2;
        public int inputdelaySectime = 0;

        public string[] strExOptionNames = new string[] {
                "조리 중 흔들기 시간 (분)",
                "조리 중 흔들기 시간 (초)",
                "조리 중 산소 입히기 시간 (분)",
                "조리 중 산소 입히기 시간 (초)",
                "투입 지연 시간 (분)",
                "투입 지연 시간 (초)",
                "1번 투입구 조리 시간(분)",
                "1번 투입구 조리 시간(초)",
                "2번 투입구 조리 시간(분)",
                "2번 투입구 조리 시간(초)",
                "조리 중 산소 입히기 시간(초)"
        };

        public string[] strExOptionRanges = new string[] {
                "1 ~ 5",
                "0 ~ 60",
                "1 ~ 5",
                "0 ~ 60",
                "1 ~ 5",
                "0 ~ 60",
                "1 ~ 15",
                "0 ~ 60",
                "1 ~ 15",
                "0 ~ 60",
                "0 / 10"
        };

        public string[] strExOptionUnits = new string[] {
                "분",
                "초",
                "분",
                "초",
                "분",
                "초",
                "분",
                "초",
                "분",
                "초",
                "초"
        };

        //2023.04.13 ::: 배출 및 중간 실행 옵셋 시간 추가
        public int[] iOffsetTime = new int[] { 2, 3, 4, 5, 6, 7};

        //2023.03.09 ::: 로봇 위치 비교를 위한 변수
        //2023.03.17 ::: 코봇 위치 레시피로 이동
        //public double[] dArrayCobotWaitPos = new double[6];

        //2023.03.09 ::: 디버깅 상태 변수
        public string[] strArrayExcuteFlagDesc = new string[100];
        public string[] strArrayIdleFlagDesc = new string[100];
        public string[] strArrayStartFlagDesc = new string[100];
        public string[] strArrayStopFlagDesc = new string[100];
        public string[] strArrayAbortFlagDesc = new string[100];

    }

    public partial class Pos_File
    {
        //Xaxis
        public int iMaxPosition = 10;
        public List<int> lstPositions = new List<int>() {};//Fastech
        public List<double> lstRealPositions = new List<double> { };//mm change

        public int iPlusLimit = 1000;
        public int iMinusLimit = -200;
        //public double iOrgOffset = 0;

        public double dPlusLimitPos = 0;
        public double dMinusLimitPos = 0;

        public string[] strXposNames = new string[] {
                "X-Axis Teaching Position 01 ( Wait   Unit 0 )",
                "X-Axis Teaching Position 02 ( Loader Unit 1 )",
                "X-Axis Teaching Position 03 ( Loader Unit 2 )",
                "X-Axis Teaching Position 04 ( Loader Unit 3 )",
                "X-Axis Teaching Position 05 ( Flyer  Unit 1 )",
                "X-Axis Teaching Position 06 ( Flyer  Unit 2 )",
                "X-Axis Teaching Position 07 ( Flyer  Unit 3 )",
                "X-Axis Teaching Position 08 ( Flyer  Unit 4 )",
                "X-Axis Teaching Position 09 ( Flyer  Unit 5 )",
                "X-Axis Teaching Position 10 ( Flyer  Unit 6 )",
        };

        public string[] strXposRanges = new string[] {
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
                "-10 ~ 1600",
        };

        public string[] strXposUnit = new string[] {
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
                "mm",
        };
        //x axis Setup

        //단축 ::: 리미트 좌우 30mm
        //-50 -30 0 1335(1305)  1363
        //조리기구
        //0 133.5 321 508.5 779 965 1235 1421 1691 1877 2016


        //Cobot

    } 

    public partial class DIO_File
    {
        public int iMaxIO = 0;
        
        //public List<bool> Inputs = new List<bool>();
        //public List<bool> Outputs = new List<bool>();

        public List<string> InNames = new List<string>();
        public List<string> OutNames = new List<string>();

        public List<string> InLabels = new List<string>();
        public List<string> OutLabels = new List<string>();
    }

    public partial class Cobot_File
    {
        public int iMaxPos = 6;
        public int iMaxLength = 20;

        //레시피별 각도
        public List<double[]> Joint = new List<double[]>() {};

        //레시피 이름
        public List<string> strName = new List<string>();
    }

    public partial class Alarm_File
    {
        public Alarm_File()
        {
            strName = new List<string>();
            strMessage = new List<string>();
            strDescription = new List<string>();
            AlarmType = new List<Cores.Core_BlackBox.ECode>();
        }

        public int iAlarmMax = 30;
        public List<string> strDescription;
        public List<string> strMessage;
        public List<string> strName;
        public List<Cores.Core_BlackBox.ECode> AlarmType;
    }

    public partial class Etc_File
    {
        public List<int> lstDays = new List<int>();
        public List<int> lstOpers = new List<int>();
    }

    public partial class Gui_File
    {
        public const int iMax = 9;
        public string[] strName = new string[Gui_File.iMax];
        public int[] iSetMin = new int[Gui_File.iMax];
        public int[] iSetSec = new int[Gui_File.iMax];
        public ModuleType[] type = new ModuleType[Gui_File.iMax];
        public int[] iCookIndex = new int[Gui_File.iMax];

        public ulong lGripperUseCount = 0;
        public ulong lRobotDownUseCount = 0;
        public ulong lRobotUpUseCount = 0;
        public ulong lRobotShakeUseCount = 0;
        public ulong lRobotOxzeneUseCount = 0;
        public ulong lRobotOilDrainUseCount = 0;
        public ulong iRobotClerningUseCount = 0;

        public DateTime dtDateTime = DateTime.Now;
    }

    public enum ModuleType
    { 
        None, Load, UnLoad, Cooker
    }

}
