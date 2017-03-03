using System;
using System.Threading;
using System.Diagnostics;

namespace FileProcessor
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string format, params object[] msg)
        {
            var pid = Process.GetCurrentProcess().Id;
            var tid = Thread.CurrentThread.ManagedThreadId;

            Console.Write("PID:{0}, ThreadId:{1}, {2}, ", pid, tid, DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.WriteLine(format, msg);
        }
    }
}