using System.IO;

namespace FileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Log("Hello World!");
            //WriteSampleFiles();
            //ProcessFile();

            //while (true)
            //{
            //    TakeATurn<string>("abc", () =>
            //    {
            //        Thread.Sleep(3000);
            //        return "dummy";
            //    });
            //}

            //Console.Read();
        }

        static void WriteSampleFiles()
        {
            //Log("Writing some sample files");

            Directory.CreateDirectory("InputFiles");

            var arr = new string[] { "3s", "2s", "1s" };
            for (int i = 0; i < 10; i++) using (var fs = File.Create(string.Format("InputFiles\\{0}-{1}.txt", i, arr[i % arr.Length]))) { }
        }
    }
}