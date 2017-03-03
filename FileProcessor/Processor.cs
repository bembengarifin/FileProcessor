using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_lockManager.TryLockAndGet(lockKey: _getFileLockKey,
                                            msTimeout: _lockMsTimeout,
                                            get: () => _dataRepository.GetNextItemsToProcess(_itemsToFetchAtATime),
                                            output: out IEnumerable<IDataObject> itemsToProcess))
            {
                if (itemsToProcess.Any())
                {
                    // materialize the items
                    var items = itemsToProcess.ToList();

                    // process
                    foreach (var item in items)
                    {
                        _logger.Log(item.ToString());
                    }
                    //Thread.Sleep(3000); // simulate some work

                    // clean up
                    _logger.Log("Finished processing, removing the data");
                    _dataRepository.DisposeItems(items);
                }
            }
        }
    }
}