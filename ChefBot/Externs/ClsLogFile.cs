using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Common
{
    public static class ClsLogFile
    {
        private static object LockObj = new object();

        public static void MakeFolder(string path)
        {
            try
            {
                lock (LockObj)
                {
                    //string currentdirectory = Directory.GetCurrentDirectory();
                    //string s_LogFilePath = currentdirectory;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 쿡 로그
        /// </summary>
        /// <param name="file"></param>
        /// <param name="logMsg"></param>
        public static void Write(string file, string logMsg)
        {
            try
            {
                lock (LockObj)
                {
                    string currentdirectory = Directory.GetCurrentDirectory();
                    //string s_LogFilePath = currentdirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    //string s_LogFilePath = "D:\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    string s_LogFilePath = $"{Application.StartupPath}\\CookLogs\\";
                    if (!Directory.Exists(s_LogFilePath))
                    {
                        Directory.CreateDirectory(s_LogFilePath);
                    }

                    string sFile = s_LogFilePath + file;

                    if (!File.Exists(sFile))
                    {
                        using (StreamWriter sw = File.CreateText(sFile))
                        {
                            sw.WriteLine("DateTime            | CookIndex | CookType | CookPriorty | CookSetTime | CookRunTime |");
                        }
                    }
                    using (StreamWriter sw = File.AppendText(sFile))
                    {   
                        sw.WriteLine(logMsg);
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 로봇 로그
        /// </summary>
        /// <param name="file"></param>
        /// <param name="logMsg"></param>
        public static void Write(string file, string[] logMsg)
        {
            try
            {
                lock (LockObj)
                {
                    string currentdirectory = Directory.GetCurrentDirectory();
                    string s_LogFilePath = currentdirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    if (!Directory.Exists(s_LogFilePath))
                    {
                        Directory.CreateDirectory(s_LogFilePath);
                    }

                    string sFile = s_LogFilePath +  file;

                    if (!File.Exists(sFile))
                    {
                        using (StreamWriter sw = File.CreateText(sFile))
                        {
                        }
                    }
                    using (StreamWriter sw = File.AppendText(sFile))
                    {
                        for (int i = 0; i < logMsg.Length; i++)
                        {
                            sw.WriteLine(logMsg[i]);
                        }
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }

        public static void Write(string file, List<string> logMsg)
        {
            try
            {
                lock (LockObj)
                {
                    string currentdirectory = Directory.GetCurrentDirectory();
                    //string s_LogFilePath = currentdirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    string s_LogFilePath = "D:\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    if (!Directory.Exists(s_LogFilePath))
                    {
                        Directory.CreateDirectory(s_LogFilePath);
                    }

                    string sFile = s_LogFilePath + file;

                    if (!File.Exists(sFile))
                    {
                        using (StreamWriter sw = File.CreateText(sFile))
                        {
                        }
                    }
                    using (StreamWriter sw = File.AppendText(sFile))
                    {
                        for (int i = 0; i < logMsg.Count; i++)
                        {
                            sw.WriteLine(logMsg[i]);
                        }
                        logMsg.Clear();
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }

        public static void Write(string path, string file, List<string> logMsg)
        {
            try
            {
                lock (LockObj)
                {
                    //string currentdirectory = Directory.GetCurrentDirectory();
                    //string s_LogFilePath = currentdirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    //string s_LogFilePath = file;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string sFile = path + file;

                    if (!File.Exists(sFile))
                    {
                        using (StreamWriter sw = File.CreateText(sFile))
                        {
                        }
                    }
                    else
                    {
                        File.Delete(sFile);
                        using (StreamWriter sw = File.CreateText(sFile))
                        {
                        }
                    }
                    
                    using (StreamWriter sw = File.AppendText(sFile))
                    {
                        for (int i = 0; i < logMsg.Count; i++)
                        {
                            sw.WriteLine(logMsg[i]);
                        }
                        //logMsg.Clear();
                        sw.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("LOG SAVE :::" + ex.Message);
            }
        }

        #region READ File
        public static string[] ReadLog(string Url)
        {
            if (!File.Exists(Url))
            {
                return null;
            }
            else
            {
                try
                {
                    string[] strTxtValue = File.ReadAllLines(Url);
                    if (strTxtValue.Length > 0)
                    {
                        return strTxtValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch(Exception ex)
                {
                    return null;
                }
                
            }
        }
        #endregion

    }
}
