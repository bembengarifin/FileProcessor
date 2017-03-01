using System;

namespace FileProcessor
{
    public interface ILockManager
    {
        Boolean TryLockAndGet<T>(string lockKey, int msTimeout, Func<T> get, out T output);
    }
}