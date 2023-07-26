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
    public class SQMaintDB
    {
        private string strDBTable;

        public SQMaintDB(string strTableName)
        {
            strDBTable = strTableName;
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
                string[] dat = new string[] { readData.Rows[row][0].ToString(),
                                              readData.Rows[row][1].ToString(),
                                              readData.Rows[row][2].ToString(),
                                              readData.Rows[row][3].ToString(),
                                              readData.Rows[row][4].ToString(),
                                              readData.Rows[row][5].ToString(),
                                              readData.Rows[row][6].ToString(),
                                              readData.Rows[row][7].ToString()};
                
                ListData.Add(dat);
            }

            if (ListData.Count > 0)
                return true;

            return false;
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

        /// <summary>
        /// 시간대별 데이터 업데이트 용
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateProductData(ulong[] countArray)
        {
            DateTime curTime = DateTime.Now;
            string strName = string.Format("{0:s}", strDBTable);

            string strQuery;

            string strData = null;

            for (int idx = 0; idx < countArray.Length; idx++)
            {
                if (idx == countArray.Length - 1)
                {
                    strData += $"'{countArray[idx]}'";
                }
                else
                {
                    strData += $"'{countArray[idx]}', ";
                }

            }

            // Table 유무 Check
            if (Cores.Core_Data.m_db.IsExistTable(strName) == false)
            {
                CreateProductDB(strName);

                strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', {strData} )";
            }
            else
            {
                strQuery = string.Format("SELECT * FROM {0:s} ORDER BY ROWID DESC LIMIT 1", strName);
                SQLiteDataReader sqlReader = Cores.Core_Data.m_db.ReadDBQuery(strQuery);

                if (sqlReader == null || sqlReader.FieldCount <= 0)
                {
                    strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', {strData} )";
                }
                else
                {
                    DataTable readData = new DataTable();
                    readData.Load(sqlReader);

                    string datetime = readData.Rows[readData.Rows.Count - 1][0].ToString();

                    string compDate = DateTime.Now.ToString("yyyy-MM-dd");

                    if (datetime != compDate)
                    {
                        strQuery = $"INSERT INTO {strName} VALUES ('{DateTime.Now:yyyy-MM-dd}', {strData})";
                    }
                    else
                    {
                        string[] head = new string[] { "GRIP=", "DOWN=", "UP=", "SHAKE=", "OXZEN=", "DRAIN=", "CLEAN=" };
                        string[] pars = strData.Split(',');

                        //strQuery = string.Format("UPDATE {0:s} SET Date='{1:s}', ", strName, datetime);
                        //strQuery = string.Format("UPDATE {0:s} where Date='{1:s}', ", strName, datetime);
                        strQuery = string.Format("UPDATE {0:s} set ", strName);

                        for (int idx = 0; idx < head.Length; idx++)
                        {
                            if (idx == head.Length - 1)
                            {
                                strQuery += head[idx] + pars[idx];
                            }
                            else
                            {
                                strQuery += head[idx] + pars[idx] + ",";
                            }
                            
                        }
                        strQuery += $" where date={datetime}";
                        //Console.WriteLine();
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
            //"GRIP=", "DOWN=", "UP=", "SHAKE=", "OXZEN=", "DRAIN=", "CLEAN="
            strQuery = string.Format("CREATE TABLE IF NOT EXISTS {0:s} (DATE TEXT NOT NULL" +
                ", GRIP TEXT NOT NULL , DOWN TEXT NOT NULL, UP TEXT NOT NULL, SHAKE TEXT NOT NULL" +
                ", OXZEN TEXT NOT NULL, DRAIN TEXT NOT NULL, CLEAN TEXT NOT NULL)", strTableName);


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
