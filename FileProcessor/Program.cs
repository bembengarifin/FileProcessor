using System;
using System.IO;
using System.Threading;

namespace FileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteSampleFiles();
            //return;

            // these can come from args or config files
            var inputFilesDirectoryPath = "InputFiles";
            var getFileLockKey = "PROCESSORGETLOCK";
            var lockMsTimeout = 1000;
            var itemsToFetchAtATime = 5;
            var milliSecondsBreakBetweenProcessing = 2000;

            // build dependencies
            var logger = new ConsoleLogger();
            var repository = new FileRepository(inputFilesDirectoryPath);
            var lockManager = new MutextLockManager(logger);
            var processor = new Processor(logger, repository, lockManager, getFileLockKey, lockMsTimeout, itemsToFetchAtATime);

            logger.Log("Press ESC to stop");
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                processor.RunProcess();

                logger.Log("Taking a {0} seconds break before processing again", milliSecondsBreakBetweenProcessing);
                Thread.Sleep(milliSecondsBreakBetweenProcessing);
            }
        }

        static void WriteSampleFiles()
        {
            Directory.CreateDirectory("InputFiles");

            //var arr = new string[] { "3s", "2s", "1s" };
            //var arr2 = new string[] { "A", "B", "C" };
            //for (int i = 0; i < 100; i++) using (var fs = File.Create(string.Format("InputFiles\\{0}-{1}-{2}.txt", i, arr2[i % arr.Length], arr[i % arr.Length]))) { }

            for (int i = 0; i < 100; i++) using (var fs = File.Create(string.Format("InputFiles\\{0}.txt", i))) { }
        }
    }
}