namespace FileProcessor
{
    interface ILogger
    {
        void Log(string format, params object[] msg);
    }
}