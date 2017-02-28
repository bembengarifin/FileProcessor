using System;

namespace FileProcessor
{
    public interface ILockManager<T>
    {
        Boolean TryLockAndGet(string lockKey, Func<T> get, int msTimeout, out T output);
    }
}