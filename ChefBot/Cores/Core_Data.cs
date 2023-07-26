using devJace.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace Cores
{
    public class Core_Data
    {
        public static List<int> lstCookerFriedCount = new List<int>() {0,0,0,0,0,0 };
        public static List<int> lstCookerFrenchCount = new List<int>() { 0, 0, 0, 0, 0, 0 };
        public static List<int> lstHourFriedCount = new List<int>() {0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0 };
        public static List<int> lstHourFrenchCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static Database.SQLiteDB m_db = new Database.SQLiteDB();
        // DB Product Table Info
        public static Database.SQProductDB m_ProductDB = new Database.SQProductDB("ProductDB");
        public static Database.SQHourDB m_HoutDB = new Database.SQHourDB("ProductDB");
        public static Database.SQMechineDB m_CobotDB = new Database.SQMechineDB("MechineDB");
        public static Database.SQMaintDB m_MaintDB = new Database.SQMaintDB("MaintDB");


        public static void CountUpdate(ChickenCounter chicken)
        {
            switch (chicken.chickenType)
            {
                case EChickenType.French:
                    lstHourFrenchCount[chicken.dateTime.Hour]++;
                    lstCookerFrenchCount[chicken.cookerIndex]++;
                    break;
                case EChickenType.Fried:
                    lstHourFriedCount[chicken.dateTime.Hour]++;
                    lstCookerFriedCount[chicken.cookerIndex]++;
                    break;
            }


        }

        public static void CountReset()
        {
            lstCookerFriedCount = new List<int>() { 0, 0, 0, 0, 0, 0 };
            lstCookerFrenchCount = new List<int>() { 0, 0, 0, 0, 0, 0 };
            lstHourFriedCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            lstHourFrenchCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

          
        }

        public partial class Chicken
        {
            public Chicken(int hour, int Fried, int French, int Unknown)
            {
                iHour = hour;
                iFried = Fried;
                iFrench = French;
                iUnknown = Unknown;
            }

            public int iHour;
            public int iFried;
            public int iFrench;
            public int iUnknown;

        }

        /// <summary>
        /// 데이터 베이스 참조
        /// </summary>
        public partial class ChickenCounter
        {
            //cooker = oilnumber
            public ChickenCounter(DateTime dt, int cooker, EChickenType type, int prioty,TimeSpan setTime, TimeSpan runTime)
            {
                dateTime = dt;
                chickenType = type;
                cookerIndex = cooker;

                tsSetTime = setTime;
                tsRunTime = runTime;

                priotyIndex = prioty;
            }

            public EChickenType chickenType = EChickenType.None;
            public DateTime dateTime = DateTime.Now;
            public int cookerIndex = -1;

            public TimeSpan tsSetTime;
            public TimeSpan tsRunTime;

            public int priotyIndex = -1;
        }

        public partial class MainCounter
        {
            public MainType mainType = MainType.Max;
            public double iTotal = 0;
            public double iMonth = 0;
            public double iDays = 0;
            public DateTime currTime = DateTime.Now;
            public DateTime prevTime = DateTime.Now;
            public DateTime MonthTime = DateTime.Now;
            public DateTime DayTime = DateTime.Now;

            public DateTime ProduceTime = DateTime.Now;
            public DateTime ProduceM_Time = DateTime.Now;
            public DateTime ProduceD_Time = DateTime.Now;

            public TimeSpan tsTotal = new TimeSpan();
            public TimeSpan tsMonth = new TimeSpan();
            public TimeSpan tsDay = new TimeSpan();
        }

        public enum MainType
        { 
            Chiken, Robot, OilChange, CookChange, AirFilter, Max
        }

        public partial class LogData
        { 
            public string strDate = null;
            public string strTime = null;
            public string strName = null;
            public string strDesc = null;
            public string strLevel = null;
        }

        public enum EChickenType
        {
            None, Fried, French, Unknown
        }

        public partial class SortBuffer
        {
            public double dCookingTime = -1;
            public int iCookingIndex = -1;
            public bool IsCookingComplted = false;
        }



        //public partial class Base_data
        //{
        //    //Info
        //    public string strBranchInfo = null;
        //    public string strBranchName = null;
        //    public int iMax = 6;


        //    public EB_State _State = EB_State.None;
        //    public List<int> iSetTimer = null;

        //    //public int iProduction_Quantity = 0;
        //    //public int iCumulative_Quantity = 0;

        //    public int iPriority_Input = 0;
        //    public int iPriority_Emission = 0;

        //    public bool IsManual_Emission = false;


        //    //Mechine Info
        //    public List<string> lstOilHeaterNumber = null;
        //    public List<int> lstInputOder = null;
        //    public List<int> lstInput_Priority = null;
        //    public List<int> lstOutput_Priority = null;

        //    public List<int> lstProduction_Quantity = null;
        //    public List<int> lstCumulative_Quantity = null;
        //}

        public enum EChicken
        { 
            Fried, Legs, Wings, Wings_Bongs, French, 
        }
        public enum EB_State
        {
            None, Waiting, Cooking, Cooked
        }
    }
}
