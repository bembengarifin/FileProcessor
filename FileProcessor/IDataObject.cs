using System;

namespace FileProcessor
{
    public interface IDataObject
    {
        DateTime CreationTimeStamp { get; }

        string Name { get; }

        string FullName { get; }

        string UserId { get; }

        int ProcessingTimeInSeconds { get; }
    }
}