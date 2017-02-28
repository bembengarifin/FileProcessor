using System.Threading;

namespace FileProcessor
{
    public class Processor
    {
        private readonly ILogger _logger;
        private readonly IDataRepository<IDataObject> _dataRepository;
        public Processor(ILogger logger, IDataRepository<IDataObject> dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        public void RunProcess()
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