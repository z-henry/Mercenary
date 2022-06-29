﻿using System;
using System.IO;
using System.Text;

namespace Mercenary
{
	// Token: 0x02000006 RID: 6
	public static class Out
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000417B File Offset: 0x0000237B
		public static void Log(string log)
		{
			string errorLogFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
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

		// Token: 0x06000032 RID: 50 RVA: 0x0000417D File Offset: 0x0000237D
		public static void UI(string log)
		{
			UIStatus.Get().AddInfo(log);
		}
	}
}
