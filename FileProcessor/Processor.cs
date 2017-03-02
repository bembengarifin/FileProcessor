using System;
using System.Collections.Generic;
using System.Threading;

namespace FileProcessor
{
    public class Processor
    {
        private readonly ILogger _logger;
        private readonly IDataRepository<IDataObject> _dataRepository;
        private readonly ILockManager _lockManager;
        private readonly string _getFileLockKey;
        private readonly int _lockMsTimeout;
        private readonly int _itemsToFetchAtATime;

        public Processor(ILogger logger, 
                        IDataRepository<IDataObject> dataRepository, 
                        ILockManager lockManager, 
                        string getFileLockKey, 
                        int lockMsTimeout, 
                        int itemsToFetchAtATime)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _lockManager = lockManager;
            _getFileLockKey = getFileLockKey;
            _lockMsTimeout = lockMsTimeout;
            _itemsToFetchAtATime = itemsToFetchAtATime;
        }

        public void RunProcess()
        {
            if (_lockManager.TryLockAndGet(_getFileLockKey, 
                                            _lockMsTimeout,
                                            () => _dataRepository.GetNextItemsToProcess(_itemsToFetchAtATime), 
                                            out IEnumerable<IDataObject> itemsToProcess))
            {
                // process
                foreach (var item in itemsToProcess)
                {
                    _logger.Log(item.ToString());
                }

                // clean up
                _dataRepository.DisposeItems(itemsToProcess);
            }
        }
    }
}