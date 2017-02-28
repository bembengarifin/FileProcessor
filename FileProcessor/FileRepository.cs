using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FileProcessor
{
    class FileRepository : IDataRepository<FileObject>
    {
        private readonly DirectoryInfo _directoryInfo;
        public FileRepository(string directoryPath)
        {
            _directoryInfo = new DirectoryInfo(directoryPath);
        }
        
        public IEnumerable<FileObject> GetNextItemsToProcess(int maxNumberOfItems)
        {
            return (from fileInfo in _directoryInfo.GetFiles().OrderBy(x => x.CreationTime)
                    let fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."))
                    let arr = fileName.Split('-')
                    let userId = arr[1]
                    let processingTime = Convert.ToInt32(arr[2].TrimEnd('s'))
                   select new FileObject(fileInfo.CreationTime, fileInfo.Name, userId, processingTime)).Take(maxNumberOfItems);
        }
    }
}