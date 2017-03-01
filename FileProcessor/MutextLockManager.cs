using System;
using System.Threading;

namespace FileProcessor
{
    public class MutextLockManager : ILockManager
    {
        private readonly ILogger _logger;
        public MutextLockManager(ILogger logger)
        {
            _logger = logger;
        }

        public bool TryLock(string lockKey, int msTimeout)
        {
            return TryLockAndGet(lockKey, msTimeout, null, out string dummy);
        }

        public bool TryLockAndGet<T>(string lockKey, int msTimeout, Func<T> get, out T output)
        {
            Mutex m = null;
            output = default(T);
            try
            {
                m = new Mutex(false, "Global\\" + lockKey);
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
                    
                    return true; // object was acquired
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

            return false; // no object was acquired
        }
    }
}