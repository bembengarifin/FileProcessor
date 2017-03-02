using System;

namespace FileProcessor
{
    public class FileObject : IDataObject
    {
        public FileObject(DateTime creationTimeStamp, string fileName, string userId, int processingTimeInSeconds)
        {
            CreationTimeStamp = creationTimeStamp;
            FileName = fileName;
            UserId = userId;
            ProcessingTimeInSeconds = processingTimeInSeconds;
        }

        public DateTime CreationTimeStamp { get; private set; }

        public string FileName { get; private set; }

        public string UserId { get; private set; }

        public int ProcessingTimeInSeconds { get; private set; }

        public override string ToString()
        {
            return string.Format("File Name:{0}", FileName);
        }
    }
}