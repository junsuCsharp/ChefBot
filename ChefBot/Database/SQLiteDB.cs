using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Drawing;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Database
{
    public class SQLiteDB
    {
        private static SQLiteConnection DBConnect;

        DateTime Tomorrow = DateTime.Today.AddDays(1);

        string DB_PATH = $"{Application.StartupPath}\\";
        string SQLITE_DB = $"Chef_Bot.db";

        //public static Database.SQLiteDB liteDB = new Database.SQLiteDB();

        private static bool DisplayMessageBox(string strText)
        {
            Common.FormMessageBox MsgBox = new Common.FormMessageBox(strText);

            if (MsgBox.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return false;
            }

            return true;
        }

        public void Connect()
        {
            try
            {
                DBConnect = new SQLiteConnection("Data Source=" + DB_PATH + SQLITE_DB);
                DBConnect.Open();
            }
            catch
            {
                return;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (DBConnect.State != ConnectionState.Open)
                {
                    return false;
                }

                DBConnect.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConnect()
        {
            try
            {
                if (DBConnect == null)
                    return false;

                if (DBConnect.State != ConnectionState.Open)
                {
                    return false;
                }

                return true;
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("IsConnect : " + se.Message);
                return false;
            }
        }

        public void DropTable(string strTableName)
        {
            if (IsConnect() == false)
            {
                return;
            }

            try
            {
                string strQuery;
                strQuery = string.Format("DROP TABLE IF EXISTS {0:s};", strTableName);

                ExecuteDBQuery(strQuery);
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("DropTable : " + se.Message);
                return;
            }
        }      

        public bool ExecuteDBQuery(string strQuery)
        {
            if (IsConnect() == false)
            {
                return false;
            }

            try
            {
                SQLiteCommand sqlCMD = new SQLiteCommand(strQuery, DBConnect);
                sqlCMD.ExecuteNonQuery();
                sqlCMD.Dispose();
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("ExecuteDBQuery : " + se.Message);
                return false;
            }

            return true;
        }

        public SQLiteDataReader ReadDBQuery(string strQuery)
        {
            if (IsConnect() == false)
            {
                return null;
            }

            try
            {
                SQLiteCommand sqlCMD = new SQLiteCommand(strQuery, DBConnect);

                SQLiteDataReader SQLReader = sqlCMD.ExecuteReader();

                sqlCMD.Dispose();

                return SQLReader;
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("ReadDBQuery : " + se.Message);
                return null;
            }
        }

        

        public bool IsExistTable(string strTableName)
        {
            string strQuery = string.Empty;
            strQuery = string.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{0:s}'", strTableName);

            try
            {
                SQLiteDataReader reader = ReadDBQuery(strQuery);

                if (reader.Read() == false)
                {
                    reader.Close();
                    reader.Dispose();

                    return false;
                }

                reader.Close();
                reader.Dispose();

                return true;
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("IsExistTable : " + se.Message);
                return false;
            }
        }

        public bool SearchDBTable(string strTableFormat, ref List<string> ListData)
        {
            string strQuery = string.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '%{0:s}%'", strTableFormat);

            try
            {
                SQLiteDataReader reader = ReadDBQuery(strQuery);

                if (reader.Read() == false)
                {
                    reader.Close();
                    reader.Dispose();

                    return false;
                }

                ListData.Add(reader[0].ToString());
                while (reader.Read())
                {
                    ListData.Add(reader["name"].ToString());
                }

                reader.Close();
                reader.Dispose();

                return true;
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("SearchDBTable : " + se.Message);
                return false;
            }
        }

        public object ExecuteScalar(string strQuery)
        {
            if (IsConnect() == false)
            {
                return null;
            }

            try
            {
                SQLiteCommand sqlCMD = new SQLiteCommand(strQuery, DBConnect);

                object obj = sqlCMD.ExecuteScalar();

                sqlCMD.Dispose();

                return obj;
            }
            catch (SQLiteException se)
            {
                DisplayMessageBox("ReadDBQuery : " + se.Message);
                return null;
            }
        }

    }
}




