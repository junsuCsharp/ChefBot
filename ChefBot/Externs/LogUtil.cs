using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Externs
{
    public class LogUtil
	{
		//- 기록 디렉토리
		private string cur_dir = $"{Application.StartupPath}\\logs";
		//- 기록 파일 초기
		private string log_file = "";
		//- 누적 일자
		private int date_gap = 30;

		/// <summary>
		/// Log를 위한 객체 생성, Thread 실행
		/// </summary>
		public LogUtil(int Date_Gap)
		{
			//- 누적 일자
			this.date_gap = Date_Gap;
			//- 로그 기록 디렉토리
			this.cur_dir = System.IO.Directory.GetCurrentDirectory();
			this.cur_dir = Path.Combine(this.cur_dir, "logs");
			DirectoryInfo di = new DirectoryInfo(this.cur_dir);
			if (di.Exists == false) { Directory.CreateDirectory(this.cur_dir); }
			//- 삭제 갱신용 쓰레드
			Thread xTaskLog = new Thread(new ThreadStart(Task_Log_Delate));
			xTaskLog.Start();
		}

		/// <summary>
		/// 날짜에 따라 로그 파일을 생성
		/// </summary>
		private bool Log_file_Make()
		{
			this.log_file = DateTime.Today.ToString("yyyyMMdd") + ".log";
			this.log_file = Path.Combine(this.cur_dir, this.log_file);
			FileInfo fi = new FileInfo(this.log_file);
			return fi.Exists;
		}

		/// <summary>
		/// 로그 내용을 기록하는 함수, 원하는 내용을 기록하도록 유도
		/// </summary>
		public void Log_Make(string msg)
		{
			try
			{
				string d = DateTime.Now.ToString("yyyyMMdd-HHmmss");
				string str = string.Format("[{0}]{1}", d, msg);
				switch (this.Log_file_Make())
				{
					case false:
						using (StreamWriter w = new StreamWriter(this.log_file))
						{
							w.WriteLine(str);
							w.Close();
						}
						break;
					default:
						using (StreamWriter w = File.AppendText(this.log_file))
						{
							w.WriteLine(str);
							w.Close();
						}
						break;
				}
			}
			catch { }
		}

		/// <summary>
		/// 로그 삭제를 실행하는 쓰레드
		/// </summary>
		void Task_Log_Delate()
		{
			//for (int i = 0; i < 60; i++)
			//{ 
			//	Thread.Sleep(60000); 
			//}
			this.Log_Delete();
		}

		/// <summary>
		/// 로그 파일을 날자 조건에 따라 삭제 
		/// </summary>
		void Log_Delete()
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(this.cur_dir);
				DateTime log_limit_time = DateTime.Now.AddDays(this.date_gap);
				foreach (FileInfo f in di.GetFiles())
				{
					if (System.IO.Path.GetExtension(f.FullName) == ".log")
					{
						DateTime file_time = f.CreationTime;
						if (DateTime.Compare(file_time, log_limit_time) > 0)
						{
							File.Delete(f.FullName);
						}
					}
				}
			}
			finally { }
		}
	}
}
