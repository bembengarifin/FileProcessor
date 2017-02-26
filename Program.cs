using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace FileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Log("Hello World!");
            //WriteSampleFiles();
            //ProcessFile();

            while (true)
            {
                TakeATurn<string>("abc", () =>
                {
                    Thread.Sleep(3000);
                    return "dummy";
                });
            }

            //Console.Read();
        }

        static void WriteSampleFiles()
        {
            Log("Writing some sample files");

            Directory.CreateDirectory("InputFiles");

            var arr = new string[] { "3s", "2s", "1s" };
            for (int i = 0; i < 10; i++) using (var fs = File.Create(string.Format("InputFiles\\{0}-{1}.txt", i, arr[i % arr.Length]))) { }
        }

        static void ProcessFile()
        {
            while (true)
            {
                var fileInfo = GetNextFileToProcess();

                if (fileInfo != null)
                {
                    var fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
                    var processingTime = Convert.ToInt32(fileName.Split('-')[1].TrimEnd('s'));

                    Log("{0} - Start processing for {1} sec processing", fileInfo.Name, processingTime);
                    Thread.Sleep(processingTime * 1000);

                    fileInfo.Delete();
                    Log("{0} - Completed processing", fileInfo.Name);
                }
                else
                {
                    Log("Sleeping for 3 seconds, then try to get file again");
                    Thread.Sleep(3000);
                }
            }
        }

        static FileInfo GetNextFileToProcess()
        {
            var di = new DirectoryInfo("InputFiles");
            return TakeATurn<FileInfo>("GetFile", () => di.GetFiles().OrderBy(x => x.CreationTime).FirstOrDefault());
        }

        static T TakeATurn<T>(string lockKey, Func<T> get)
        {
            Mutex m = null;
            T result = default(T);
            try
            {
                m = new Mutex(false, "Global\\" + lockKey);
                Log("Waiting for a turn");
                if (m.WaitOne(1000))
                {
                    Log("Got the lock, working on something now");
                    result = get();

                    Log("Done, releasing the lock");
                    m?.ReleaseMutex();
                }
            }
            catch(Exception ex)
            {
                Log("Error: {0}", ex.Message);
            }
            finally
            {
                m?.Dispose();
            }

            return result;
        }

        static void Log(string format, params object[] msg)
        {
            var pid = Process.GetCurrentProcess().Id;
            var tid = Thread.CurrentThread.ManagedThreadId;

            Console.Write("PID:{0}, ThreadId:{1}, {2}, ", pid, tid, DateTime.Now.ToString("dd-MMM-yy HH:mm:ss.fff"));
            Console.WriteLine(format, msg);
        }
    }
}