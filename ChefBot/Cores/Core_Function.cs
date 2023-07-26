using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cores
{
    public class Core_Function
    {
        public void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }
            return;
        }
    }

    public partial class ChikenModule
    {
        public ChikenModule()
        {

        }

        /// <summary>
        /// 조리기구에 투입할때 발생
        /// </summary>
        /// <param name="cooker"></param>
        /// <param name="setTime"></param>
        public void SetCooker(int cooker, TimeSpan setTime)
        {
            cookerIndex = cooker;

            //TimeSpan tsBuff = new TimeSpan(0, 0, (int)setTime.TotalSeconds - Core_Object.GetObj_File.iOffsetTime[cookerIndex]);
            //tsSetTime = tsBuff;

            tsSetTime = setTime;
        }

        /// <summary>
        /// 설정된 타이머를 변경할 때 사용하고, 조리시작하여도, 다시 날짜시간 변수에 넣는다.
        /// </summary>
        /// <param name="setTime"></param>
        public void SetTimeChanege(TimeSpan setTime)
        {
            //타이머 시간 변경
            //TimeSpan tsBuff = new TimeSpan(0, 0, (int)setTime.TotalSeconds - Core_Object.GetObj_File.iOffsetTime[cookerIndex]);
            //tsSetTime = tsBuff;

            tsSetTime = setTime;
            dateTimeCooking = dateTimeCooker.Add(tsSetTime);
        }

        /// <summary>
        /// 투입 완료시 발생
        /// </summary>
        public void CookingStart()
        {
            //투입완료
            dateTimeCooker = DateTime.Now;
            dateTimeCooking = dateTimeCooker.Add(tsSetTime);
            chickenState = Core_Data.EB_State.Cooking;
            //stopwatch.Restart();
            //stopwatchOutput.Stop();
            IsCookingCompSound = false;

            tsCurTime = new TimeSpan(0, 0, 0);
            IsLatchShakingChiken = false;
            IsLatchOxzenChiken = false;
            IsLatchShakingChikenComplted = false;
            IsLatchOxzenChikenComplted = false;
        }

        public void CookingReady()
        {
            chickenState = Core_Data.EB_State.Waiting;
        }

        /// <summary>
        /// 투입 후 상시 비교 용
        /// </summary>
        /// <returns></returns>
        public bool IsCookingComplted()
        {
            //if (DateTime.Now >= dateTimeCooking && stopwatch.IsRunning)
            //{
            //    chickenState = Core_Data.EB_State.Cooked;
            //    return true;
            //}
            //else if (chickenState == Core_Data.EB_State.Waiting)
            //{   
            //    return false;
            //}
            //else if (stopwatch.IsRunning == false) 
            //{
            //    //chickenState = Core_Data.EB_State.None;
            //    return false;
            //}
            //chickenState = Core_Data.EB_State.Cooking;
            //return false;

            tsCurTime = DateTime.Now.AddSeconds((double)Core_Object.GetObj_File.iOffsetTime[cookerIndex]) - dateTimeCooker;
            TimeSpan tsNowRes = dateTimeCooking - dateTimeCooker;
            if (tsNowRes.TotalSeconds == 0)
            {
                return false;
            }
            else if (tsCurTime.TotalMilliseconds >= tsNowRes.TotalMilliseconds
                && (chickenState == Core_Data.EB_State.Cooking || chickenState == Core_Data.EB_State.Cooked))
            {
                chickenState = Core_Data.EB_State.Cooked;
                if (chickenPreviousState != chickenState)
                {
                    chickenPreviousState = chickenState;

                    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                    devJace.Program.ELogLevel.Debug,
                    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $" | Exist Time : [{cookerIndex}][{tsCurTime.TotalSeconds}][{tsNowRes.TotalSeconds}]");
                }
              

               
                return true;
            }


          

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tsShakingTime"></param>
        /// <returns></returns>
        public bool IsCurrentShaking(TimeSpan tsShakingTime)
        {
            tsCurTime = DateTime.Now - dateTimeCooker;

            if (tsShakingTime.TotalSeconds == 0 || Core_Object.GetObj_File.iBasketShaking == 0
                || Core_Object.GetObj_File.iBasketShakingCount == 0)
                return false;

            if (chickenState == Core_Data.EB_State.Cooking && IsLatchShakingChiken == false
              && tsCurTime.TotalSeconds >= tsShakingTime.TotalSeconds)
            {
                IsLatchShakingChiken = true;
            }
            else if (chickenState != Core_Data.EB_State.Cooking && IsLatchShakingChiken == true)
            {
                IsLatchShakingChiken = false;
            }

            return IsLatchShakingChiken;
        }

        public bool IsCurrentOxzening(TimeSpan tsOxzenTime)
        {
            tsCurTime = DateTime.Now - dateTimeCooker;

            if (tsOxzenTime.TotalSeconds == 0)
                return false;

            if (chickenState == Core_Data.EB_State.Cooking && IsLatchOxzenChiken == false
              && tsCurTime.TotalSeconds >= tsOxzenTime.TotalSeconds)
            {
                IsLatchOxzenChiken = true;
            }
            else if (chickenState != Core_Data.EB_State.Cooking && IsLatchOxzenChiken == true)
            {
                IsLatchOxzenChiken = false;
            }

            return IsLatchOxzenChiken;
        }

        /// <summary>
        /// 조리 완료 후 발생
        /// </summary>
        public void CookingComplted()
        {
            IsExist = false;
            chickenState = Core_Data.EB_State.None;
            //stopwatch.Stop();
            //stopwatchOutput.Restart();
            IsCookingRedySound = false;
        }

        /// <summary>
        /// 중간에 치킨 흔들기 완료 되면 발생
        /// </summary>
        public void CookingShakingComplted()
        {
            IsLatchShakingChikenComplted = true;
        }

        /// <summary>
        /// 중간에
        /// </summary>
        public void CookingOxzenComplted()
        {
            IsLatchOxzenChikenComplted = true;
        }

        public void CookingForceComplte()
        {
            //IsExist = false;
            chickenState = Core_Data.EB_State.Cooked;
            //stopwatch.Stop();
            //stopwatchOutput.Restart();
            IsCookingForcSound = false;
        }

        public Cores.Core_Data.EChickenType chickenType = Cores.Core_Data.EChickenType.None;
        public Cores.Core_Data.EB_State chickenState = Core_Data.EB_State.None;

        private Cores.Core_Data.EB_State chickenPreviousState = Core_Data.EB_State.None;
        public DateTime dateTimeCooker = DateTime.Now;//쿠킹 시작 시간
        public DateTime dateTimeCooking = DateTime.Now;//쿠킹 완료 비교 시간
        public DateTime dateTimeCooked = DateTime.Now;//쿠킹 완료 시간
        public int cookerIndex = -1;
        //public Stopwatch stopwatch = new Stopwatch();//화면 표시용
        public TimeSpan tsSetTime = new TimeSpan(0, 0, 0);//화면 표시용 설정 시간
        public TimeSpan tsCurTime = new TimeSpan(0, 0, 0);//화면 표시용 경과 시간
        //public Stopwatch stopwatchOutput = new Stopwatch();//요리 완료하고, 경과시간
        public bool IsExist = false;

        public bool IsCookingForcSound = false;
        public bool IsCookingCompSound = false;
        public bool IsCookingRedySound = false;

        public bool IsLatchShakingChiken = false;
        public bool IsLatchOxzenChiken = false;

        public bool IsLatchShakingChikenComplted = false;
        public bool IsLatchOxzenChikenComplted = false;

        public bool IsCookerUsed = false;
    }

    public partial class exChiken
    {
        public Cores.Core_Data.EChickenType chickenType = Cores.Core_Data.EChickenType.None;
        public Cores.Core_Data.EB_State chickenState = Core_Data.EB_State.None;
        public DateTime dateTimeCooker = DateTime.Now;//쿠킹 시작 시간
        public DateTime dateTimeCooking = DateTime.Now;//쿠킹 완료 비교 시간
        public DateTime dateTimeCooked = DateTime.Now;//쿠킹 완료 시간
        public int cookerIndex = -1;
        
        public TimeSpan tsSetTime = new TimeSpan(0, 0, 0);//화면 표시용 설정 시간
        public TimeSpan tsCurTime = new TimeSpan(0, 0, 0);//화면 표시용 경과 시간
        
        public bool IsExist = false;

        public bool IsCookingForcSound = false;
        public bool IsCookingCompSound = false;
        public bool IsCookingRedySound = false;
    }

    public partial class LoderModule
    {
        public LoderModule(Core_Data.EChickenType eChicken)
        {
            dateTimeLoader = DateTime.Now;
            dateTimeBlinkTime = dateTimeLoader.AddMilliseconds(500);
            chickenType = eChicken;
            chickenState = Core_Data.EB_State.Waiting;
        }      
        public DateTime dateTimeLoader = DateTime.Now;//로더 이벤트 타임
        public DateTime dateTimeBlinkTime = DateTime.Now;//로더 이벤트 타임
        public Cores.Core_Data.EB_State chickenState = Core_Data.EB_State.None;
        public Cores.Core_Data.EChickenType chickenType = Cores.Core_Data.EChickenType.None;

        public bool IsLoader = false;

        public bool IsStartLatch = false;
        
        public bool IsCurrSensor = false;
        public bool IsPrevSensor = false;

        public bool IsCurrSwitch = false;
        public bool IsPrevSwitch = false;

        public string strSwitch = "";

        public bool IsLoading()
        {
            return IsStartLatch & IsCurrSensor;
        }
    }

    
}
