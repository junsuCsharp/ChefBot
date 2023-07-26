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
    public class SQMechineDB
    {
        private string strDBTable;

        public SQMechineDB(string strTableName)
        {
            strDBTable = strTableName;
        }

        public bool LimitData(int limitCount)
        {
            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strSQL = "";
            DateTime dt = DateTime.Now.AddDays(limitCount * -1);
            //strSQL = $"delete from MaintDB where date < '{dt:yyyy-MM-dd}'";
            strSQL = $"DELETE FROM {strDBTable} WHERE date in ( SELECT * FROM ( SELECT date FROM {strDBTable} order by date desc limit {limitCount}, {limitCount}) as P)";

            return Cores.Core_Data.m_db.ExecuteDBQuery(strSQL);
        }

        public bool ReadData(out List<string[]> ListData)
        {
            ListData = null;

            if (Cores.Core_Data.m_db.IsExistTable(strDBTable) == false)
                return false;

            string strQuery = string.Format("SELECT * FROM {0:s} ORDER BY DATE ASC", strDBTable);

            SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

            if (sqlReader == null || sqlReader.FieldCount <= 0)
                return false;

            DataTable readData = new DataTable();
            readData.Load(sqlReader);

            ListData = new List<string[]>();

            for (int row = 0; row < readData.Rows.Count; row++)
            {
                string[] dat = new string[] { readData.Rows[row][0].ToString() , readData.Rows[row][1].ToString() };
                
                ListData.Add(dat);
            }

            if (ListData.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// 시간대별 데이터 업데이트 용
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateProductData(double hour)
        {
            DateTime curTime = DateTime.Now;
            string strName = string.Format("{0:s}", strDBTable);

            string strQuery;

            // Table 유무 Check
            if (Cores.Core_Data.m_db.IsExistTable(strName) == false)
            {
                CreateProductDB(strName);

                strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', '{0}' )";
            }
            else
            {
                strQuery = string.Format("SELECT * FROM {0:s} ORDER BY ROWID DESC LIMIT 1", strName);
                SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

                if (sqlReader == null || sqlReader.FieldCount <= 0)
                {
                    strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', '{0}' )";
                }
                else
                {
                    DataTable readData = new DataTable();
                    readData.Load(sqlReader);

                    if (readData.Rows.Count != 0)
                    {
                        string datetime = readData.Rows[readData.Rows.Count - 1][0].ToString();

                        string compDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //string compDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                        if (datetime != compDate)
                        {
                            strQuery = $"INSERT INTO {strName} VALUES ('{compDate}', '{0}' )";
                        }
                        else
                        {
                            //2023.06.14
                            //UPDATE MechineDB set Robot='4' where date='2023-06-16'
                            strQuery = string.Format("UPDATE {0:s} SET Robot='{2:d}' where Date='{1:s}'", strName, datetime, hour.ToString());
                        }
                    }
                    else
                    {
                        strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', '{0}' )";
                    }
                    
                }

            }
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

            strQuery = string.Format("CREATE TABLE IF NOT EXISTS {0:s} (DATE TEXT NOT NULL, ROBOT TEXT NOT NULL)", strTableName);


            bRet = Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);

            if (!bRet)
            {
                return false;
            }

            Cores.Core_Data.m_db.ExecuteDBQuery(strQuery);

            return true;
        }

    }
}
