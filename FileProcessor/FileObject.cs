using System;

namespace FileProcessor
{
    public class FileObject : IDataObject
    {
        public FileObject(DateTime creationTimeStamp, string name, string fullName, string userId, int processingTimeInSeconds)
        {
            CreationTimeStamp = creationTimeStamp;
            Name = name;
            FullName = fullName;
            UserId = userId;
            ProcessingTimeInSeconds = processingTimeInSeconds;
        }

        public DateTime CreationTimeStamp { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public string UserId { get; private set; }

        public int ProcessingTimeInSeconds { get; private set; }

        public override string ToString()
        {
            return string.Format("File Name:{0}", Name);
        }
    }
}