using System;

namespace FileProcessor
{
    public interface IDataObject
    {
        DateTime CreationTimeStamp { get; }

        string FileName { get; }

        string UserId { get; }

        int ProcessingTimeInSeconds { get; }
    }
}