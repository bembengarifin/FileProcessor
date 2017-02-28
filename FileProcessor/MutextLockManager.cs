using System;
using System.Threading;

namespace FileProcessor
{
    public class MutextLockManager<T> : ILockManager<T>
    {
        private readonly ILogger _logger;
        public MutextLockManager(ILogger logger)
        {
            _logger = logger;
        }

        public bool TryLockAndGet(string lockKey, Func<T> get, int msTimeout, out T output)
        {
            Mutex m = null;
            output = default(T);
            try
            {
                m = new Mutex(false, "Global\\" + lockKey);
                _logger.Log("Waiting for a turn");
                if (m.WaitOne(1000))
                {
                    _logger.Log("Got the lock, working on something now");
                    output = get();

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