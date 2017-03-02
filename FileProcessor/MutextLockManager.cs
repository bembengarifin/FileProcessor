using System;
using System.Threading;

namespace FileProcessor
{
    public class MutextLockManager : ILockManager
    {
        private const string GLOBAL_KEYWORD = "Global\\";
        private readonly ILogger _logger;
        public MutextLockManager(ILogger logger)
        {
            _logger = logger;
        }

        public bool CheckIfLockExists(string lockKey)
        {
            return Mutex.TryOpenExisting(GLOBAL_KEYWORD + lockKey, out Mutex m);
        }

        public bool TryLockAndGet<T>(string lockKey, int msTimeout, Func<T> get, out T output)
        {
            Mutex m = null;
            output = default(T);
            try
            {
                // Global/Local Mutex - https://msdn.microsoft.com/en-us/library/f55ddskf(v=vs.110).aspx
                m = new Mutex(false, GLOBAL_KEYWORD + lockKey);
                _logger.Log("Waiting for a turn");
                if (m.WaitOne(msTimeout))
                {
                    if (get != null)
                    {
                        _logger.Log("Got the lock, working on something now");
                        output = get();
                    }

                    _logger.Log("Done, releasing the lock");
                    m?.ReleaseMutex();
                    
                    return true; // lock was acquired successfully
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error: {0}", ex.Message);
            }
            finally
            {
                m?.Dispose();
            }

            return false; // lock was not acquired
        }
    }
}