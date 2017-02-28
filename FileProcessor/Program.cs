using System.IO;
using System.Threading;

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

    class FileProcessor
    {
        private readonly ILogger _logger;
        private readonly IDataRepository<IDataObject> _dataRepository;
        public FileProcessor(ILogger logger, IDataRepository<IDataObject> dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        void RunProcess()
        {
            var fileInfo = _dataRepository.GetNextItemsToProcess(10);

            if (fileInfo != null)
            {
                //_logger.Log("{0} - Start processing for {1} sec processing", fileInfo.Name, processingTime);
                //Thread.Sleep(processingTime * 1000);

                //fileInfo.Delete();
                //_logger.Log("{0} - Completed processing", fileInfo.Name);
            }
            else
            {
                _logger.Log("Sleeping for 3 seconds, then try to get file again");
                Thread.Sleep(3000);
            }

            
        }
    }
}