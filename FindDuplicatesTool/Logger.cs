using System;
using System.IO;
using System.Text;

namespace FindDuplicatesTool
{
	class Logger
    {
        private string LogFilePath { get; set; }
        public Logger(string fileName)
        {
            this.LogFilePath = fileName;
            return;
        }

        public void Print(string format, params object[] args)
        {
            using (TextWriter logfile = new StreamWriter(this.LogFilePath, true, Encoding.UTF8))
            {
                WriteLine(logfile, false, format, args);
            }
            return;
        }

        private void WriteLine(TextWriter writer, bool echoToConsole, string format, params object[] args)
        {
            string message = string.Format(format, args);
            DateTime dtNow = DateTime.Now;
            const string logFormat = "{0:yyyy-MM-dd hh:mm:ss.ttt}: {1}";
            writer.WriteLine(message);
            if (echoToConsole)
            {
                Console.WriteLine(logFormat, dtNow, message);
            }
            return;
        }

        public void ExitAndOut(string format, params object[] args)
        {
            using (TextWriter logfile = new StreamWriter("", true, Encoding.UTF8))
            {
                WriteLine(logfile, true, format, args);
                WriteLine(logfile, true, "Program Exit");
            }
            Environment.Exit(1);
            return; // unreachable code
        }
    }
}
