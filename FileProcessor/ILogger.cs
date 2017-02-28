namespace FileProcessor
{
    public interface ILogger
    {
        void Log(string format, params object[] msg);
    }
}