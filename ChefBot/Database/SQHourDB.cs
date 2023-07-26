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
    public class SQHourDB
    {
        private string strDBTable;

        public SQHourDB(string strTableName)
        {
            strDBTable = strTableName;
        }

        public bool ReadData(string strTableName, out List<Cores.Core_Data.Chicken> ListProductData)
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

            ListProductData = new List<Cores.Core_Data.Chicken>();

            for (int row = 0; row < readData.Rows.Count; row++)
            {
                ListProductData.Add(new Cores.Core_Data.Chicken(int.Parse(readData.Rows[row][0].ToString()),
                    int.Parse(readData.Rows[row][1].ToString()),
                    int.Parse(readData.Rows[row][2].ToString()),
                    int.Parse(readData.Rows[row][3].ToString())));
                
            }

            if (ListProductData.Count > 0)
                return true;

            return false;
        }

     
        /// <summary>
        /// 시간대별 데이터 업데이트 용
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateProductData(int hour, int firedCount, int frenchCount, int unknownCount)
        {
            DateTime curTime = DateTime.Now;
            string strName = string.Format("{0:s}_{1:d}{2:d2}{3:d2}", strDBTable, curTime.Year, curTime.Month, curTime.Day);

            //string strName = $"{strDBTable}";

            // Table 유무 Check
            if (Cores.Core_Data.m_db.IsExistTable(strName) == false)
            {
                CreateProductDB(strName);
            }

            string strQuery;

            strQuery = string.Format("UPDATE {0:s} SET FRIED={1:d}, FRENCH={2:d}, UNKNOWN={3:d} WHERE Time = '{4:s}'", strName, firedCount, frenchCount, unknownCount, hour.ToString());

            //int usWeekNumber = GetWeekOfYear(DateTime.Now, CultureInfo.CurrentCulture);
           
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

            strQuery = string.Format("CREATE TABLE IF NOT EXISTS {0:s} (TIME TEXT NOT NULL, FRIED TEXT NOT NULL, FRENCH TEXT NOT NULL, UNKNOWN TEXT NOT NULL)", strTableName);
          

            bRet = Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);

            if (!bRet)
            {
                return false;
            }

            // Table 초기화
            for (nRow = 0; nRow < 24; nRow++)
            {
                strQuery = string.Empty;
                strText = string.Empty;
            
                strQuery = string.Format("INSERT INTO {0:s} VALUES (", strTableName);
                strText = string.Format("'{0:s}','0','0','0')", nRow.ToString());
            
                strQuery += strText;
            
                Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);
            }

            return true;
        }
    }
}
