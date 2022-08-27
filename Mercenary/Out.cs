using System;
using System.IO;
using System.Text;

namespace Mercenary
{
	
	public static class Out
	{
		
		public static void Log(string log)
		{
			string errorLogFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepinEx/Log");
			if (Main.hsUnitID.Length > 0)
				errorLogFilePath = System.IO.Path.Combine(errorLogFilePath, Main.hsUnitID);
			if (!Directory.Exists(errorLogFilePath))
			{
				Directory.CreateDirectory(errorLogFilePath);
			}
			string logFile = System.IO.Path.Combine(errorLogFilePath, "mercenarylog" + "@" + DateTime.Today.ToString("yyyy-MM-dd") + ".log");
			bool writeBaseInfo = System.IO.File.Exists(logFile);
			StreamWriter swLogFile = new StreamWriter(logFile, true, Encoding.Unicode);
			swLogFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + log);
			swLogFile.Close();
			swLogFile.Dispose();
		}
		public static void LogGameRecord(string log)
		{
			string errorLogFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepinEx/Log");
			if (Main.hsUnitID.Length > 0)
				errorLogFilePath = System.IO.Path.Combine(errorLogFilePath, Main.hsUnitID);
			if (!Directory.Exists(errorLogFilePath))
			{
				Directory.CreateDirectory(errorLogFilePath);
			}
			string logFile = System.IO.Path.Combine(errorLogFilePath, "gamerecord" + "@" + DateTime.Today.ToString("yyyy-MM-dd") + ".log");
			bool writeBaseInfo = System.IO.File.Exists(logFile);
			StreamWriter swLogFile = new StreamWriter(logFile, true, Encoding.Unicode);
			swLogFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + log);
			swLogFile.Close();
			swLogFile.Dispose();
		}


		public static void UI(string log)
		{
			UIStatus.Get().AddInfo(log);
		}
	}
}
