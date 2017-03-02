using System;

namespace FileProcessor
{
    public interface ILockManager
    {
        bool CheckIfLockExists(string lockKey);
        Boolean TryLockAndGet<T>(string lockKey, int msTimeout, Func<T> get, out T output);
    }
}