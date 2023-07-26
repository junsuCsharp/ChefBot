using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class SQProductDB
    {
        private string strDBTable;

        public SQProductDB(string strTableName)
        {
            strDBTable = strTableName;
        }

        public bool ReadDayData(out int[][] getCount)
        {
            getCount = null;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;


            //count(CASE WHEN Year='2023' AND Month='02' AND Week='7' AND type='0' THEN 1 END) as getFried,
            strQuery += $"SELECT date, day,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}' AND type='{0}' THEN 1 END) as getFried,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}' AND type='{1}' THEN 1 END) as getFrench ";
            strQuery += $"FROM {strDBTable} ";
            strQuery += $"where Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}'";
            strQuery += $"GROUP by Day ";
            strQuery += $"ORDER by date ASC";

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = new int[DateTime.DaysInMonth((int)DateTime.Now.Year, (int)DateTime.Now.Month)][];

            for (int row = 0; row < getCount.Length; row++)
            {
                getCount[row] = new int[3];
            }
            for (int row = 0; row < readData.Rows.Count; row++)
            {
                int day = int.Parse(readData.Rows[row][1].ToString());
                getCount[day][0] = day;
                getCount[day][1] = int.Parse(readData.Rows[row][2].ToString());
                getCount[day][2] = int.Parse(readData.Rows[row][3].ToString());
            }

            return true;
        }
        public bool ReadWeekData(out int[][] getCount)
        {
            getCount = null;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;


            //count(CASE WHEN Year='2023' AND Month='02' AND Week='7' AND type='0' THEN 1 END) as getFried,
            strQuery += $"SELECT date, day, Week,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}' AND type='{0}' THEN 1 END) as getFried,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}' AND type='{1}' THEN 1 END) as getFrench ";
            strQuery += $"FROM {strDBTable} ";
            strQuery += $"where Year='{DateTime.Now.Year:0000}' AND Month='{DateTime.Now.Month:00}' AND Week='{GetWeekOfYear(DateTime.Now, CultureInfo.CurrentCulture):00}' ";
            strQuery += $"GROUP by Day ";
            strQuery += $"ORDER by date ASC";

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = new int[7][];

            for (int row = 0; row < getCount.Length; row++)
            {
                getCount[row] = new int[3];
            }
            for (int row = 0; row < readData.Rows.Count; row++)
            {
                int day = int.Parse(readData.Rows[row][1].ToString());
                getCount[row][0] = day;
                getCount[row][1] = int.Parse(readData.Rows[row][3].ToString());
                getCount[row][2] = int.Parse(readData.Rows[row][4].ToString());
            }

            return true;
        }
        public bool ReadMonthData(out int[][] getCount)
        {
            getCount = null;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;

            strQuery += $"SELECT Year, Month,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND type='{0}' THEN 1 END) as getFried,";
            strQuery += $"count (CASE WHEN Year='{DateTime.Now.Year:0000}' AND type='{1}' THEN 1 END) as getFrench ";
            strQuery += $"FROM {strDBTable} ";
            strQuery += $"where Year='{DateTime.Now.Year:0000}' ";
            strQuery += $"GROUP by Month ";
            strQuery += $"ORDER by date ASC";

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = new int[12][];

            for (int row = 0; row < getCount.Length; row++)
            {
                getCount[row] = new int[3];
            }
            for (int row = 0; row < readData.Rows.Count; row++)
            {
                int month = int.Parse(readData.Rows[row][1].ToString());
                getCount[month][0] = month;
                getCount[month][1] = int.Parse(readData.Rows[row][2].ToString());
                getCount[month][2] = int.Parse(readData.Rows[row][3].ToString());
            }

            return true;
        }
        public bool ReadTotalCount(out int getCount)
        {
            getCount = 0;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;

            strQuery += $"SELECT count(*) FROM {strDBTable}";
            

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = int.Parse(readData.Rows[0][0].ToString());

            //for (int row = 0; row < getCount.Length; row++)
            //{
            //    getCount[row] = new int[3];
            //}

            //for (int row = 0; row < readData.Rows.Count; row++)
            //{
            //    int month = int.Parse(readData.Rows[row][1].ToString());
            //    getCount[month][0] = month;
            //    getCount[month][1] = int.Parse(readData.Rows[row][2].ToString());
            //    getCount[month][2] = int.Parse(readData.Rows[row][3].ToString());
            //}

            return true;
        }
        public bool ReadMonthCount(out int getCount)
        {
            getCount = 0;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;

            strQuery += $"SELECT count(*) FROM {strDBTable} where year='{DateTime.Now.Year:0000}' and month='{DateTime.Now.Month:00}'";


            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = int.Parse(readData.Rows[0][0].ToString());

            //for (int row = 0; row < getCount.Length; row++)
            //{
            //    getCount[row] = new int[3];
            //}

            //for (int row = 0; row < readData.Rows.Count; row++)
            //{
            //    int month = int.Parse(readData.Rows[row][1].ToString());
            //    getCount[month][0] = month;
            //    getCount[month][1] = int.Parse(readData.Rows[row][2].ToString());
            //    getCount[month][2] = int.Parse(readData.Rows[row][3].ToString());
            //}

            return true;
        }
        public bool ReadDayCount(out int getCount)
        {
            getCount = 0;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = null;

            strQuery += $"SELECT count(*) FROM {strDBTable} where year='{DateTime.Now.Year:0000}' and month='{DateTime.Now.Month:00}' and day='{DateTime.Now.Day:00}'";


            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            getCount = int.Parse(readData.Rows[0][0].ToString());

            //for (int row = 0; row < getCount.Length; row++)
            //{
            //    getCount[row] = new int[3];
            //}

            //for (int row = 0; row < readData.Rows.Count; row++)
            //{
            //    int month = int.Parse(readData.Rows[row][1].ToString());
            //    getCount[month][0] = month;
            //    getCount[month][1] = int.Parse(readData.Rows[row][2].ToString());
            //    getCount[month][2] = int.Parse(readData.Rows[row][3].ToString());
            //}

            return true;
        }



        public bool ReadData(string strTableName, out List<Cores.Core_Data.ChickenCounter> ListProductData)
        {
            ListProductData = null;

            if (Cores.Core_Data.m_db.IsExistTable(strTableName) == false)
                return false;

            string strQuery = string.Format("SELECT * FROM {0:s} ORDER BY Time ASC", strTableName);

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            ListProductData = new List<Cores.Core_Data.ChickenCounter>();

            for (int row = 0; row < readData.Rows.Count; row++)
            {

                DateTime dtDate = DateTime.Parse(readData.Rows[row][0].ToString());
                DateTime dtTime = DateTime.Parse(readData.Rows[row][1].ToString());
                DateTime dateTime = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, dtTime.Hour, dtTime.Minute, dtTime.Second);
                int type = Convert.ToInt16(readData.Rows[row][2]);
                int cooker = Convert.ToInt16(readData.Rows[row][3]);             
                TimeSpan tsSetTime = TimeSpan.Parse(readData.Rows[row][9].ToString());
                TimeSpan tsRunTime = TimeSpan.Parse(readData.Rows[row][10].ToString());
                
                ListProductData.Add(new Cores.Core_Data.ChickenCounter(dateTime, cooker, (Cores.Core_Data.EChickenType)type, 0,tsSetTime, tsRunTime));
            }

            if (ListProductData.Count > 0)
                return true;

            return false;
        }

        public bool GetCount(int year, out int[] cout)
        {
            cout = new int[] {};

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;
                

            string strQuery = string.Format("SELECT * FROM {0:s} ORDER BY Time ASC", strDBTable);

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// 전체 데이터
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateInsertData(Cores.Core_Data.ChickenCounter count)
        {
            //DateTime curTime = DateTime.Now;
            //string strName = string.Format("{0:s}_{1:d}{2:d2}{3:d2}", strDBTable, curTime.Year, curTime.Month, curTime.Day);

            string strName = $"{strDBTable}";

            // Table 유무 Check
            if (Cores.Core_Data.m_db.IsExistTable(strName) == false)
            {
                CreateProductDB(strName);
            }

            string strQuery;

            //strQuery = string.Format("UPDATE {0:s} SET OK={1:d}, NG={2:d}, DOUBLE={3:d} WHERE Time = '{4:s}'", strName, nOKCount, nNGCount, nDoubleCount, nHour.ToString());

            int usWeekNumber = GetWeekOfYear(count.dateTime, CultureInfo.CurrentCulture);

            //strQuery = $"UPDATE {strName} SET DATE='{count.dateTime:yyyy-MM-dd}', TIME='{count.dateTime:HH:mm:ss.fff}', TYPE={(int)count.chickenType}," +
            //    $"COOKER={count.cookerIndex}, HOUR={count.dateTime:HH}, DAY={count.dateTime:dd}, WEEK={usWeekNumber}, MONTH={count.dateTime.Month:00}, YEAR={count.dateTime.Year:0000}  WHERE TIME='{count.dateTime:HH:mm:ss.fff}'";

            string settime = count.tsSetTime.ToString();
            string runtime = count.tsRunTime.ToString();

            //INSERT INTO ProductDB VALUES ('2023-02-13', '18:10:33.084', '1', '4', '18', '13', '7', '02', '2023')
            strQuery = $"INSERT INTO {strName} VALUES ('{count.dateTime:yyyy-MM-dd}', '{count.dateTime:HH:mm:ss.fff}', '{(int)count.chickenType}', " +
                $"'{count.cookerIndex}', '{count.dateTime:HH}', '{count.dateTime:dd}', '{usWeekNumber}', '{count.dateTime.Month:00}', '{count.dateTime.Year:0000}', '{settime}', '{runtime}' )";
                

            Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);

            return true;
        }       

        /// <summary>
        /// 연도 주차 구하기
        /// </summary>
        /// <param name="sourceDate">소스 일자</param>
        /// <param name="cultureInfo">문화 정보</param>
        /// <returns>연도 주차</returns>
        public int GetWeekOfYear(DateTime sourceDate, CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;

            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            return cultureInfo.Calendar.GetWeekOfYear(sourceDate, calendarWeekRule, firstDayOfWeek);
        }


        public bool CreateProductDB(string strTableName)
        {
            bool bRet = false;
            int nRow = 0;

            string strQuery, strText;

            //strQuery = string.Format("CREATE TABLE IF NOT EXISTS {0:s} (Time TEXT NOT NULL, OK INT NOT NULL, NG INT NOT NULL, DOUBLE INT NOT NULL)", strTableName);
            strQuery = string.Format("CREATE TABLE IF NOT EXISTS {0:s}" +
                " (DATE TEXT NOT NULL, TIME TEXT NOT NULL, TYPE TEXT NOT NULL, COOKER TEXT NOT NULL" +
                ", HOUR TEXT NOT NULL, DAY TEXT NOT NULL, WEEK TEXT NOT NULL, MONTH TEXT NOT NULL, YEAR TEXT NOT NULL" +
                ", SETTIME TEXT NOT NULL, RUNTIME TEXT NOT NULL)", strTableName);

            bRet = Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);

            if (!bRet)
            {
                return false;
            }

            // Table 초기화
            //for (nRow = 0; nRow < 24; nRow++)
            //{
            //    strQuery = string.Empty;
            //    strText = string.Empty;
            //
            //    strQuery = string.Format("INSERT INTO {0:s} VALUES (", strTableName);
            //    strText = string.Format("'{0:s}','0','0','0')", nRow.ToString());
            //
            //    strQuery += strText;
            //
            //    Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);
            //}

            return true;
        }
    }
}
