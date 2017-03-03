using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FileProcessor
{
    public class FileRepository : IDataRepository<IDataObject> 
    {
        private readonly ILogger _logger;
        private readonly DirectoryInfo _directoryInfo;
        
        public FileRepository(ILogger logger, string directoryPath)
        {
            _logger = logger;
            _directoryInfo = new DirectoryInfo(directoryPath);
        }
        
        public IEnumerable<IDataObject> GetNextItemsToProcess(int maxNumberOfItems)
        {
            int counter = 0;
            return _directoryInfo.GetFiles("*.txt").OrderBy(x => x.CreationTime)
                    .TakeWhile(x =>
                    {
                        if (counter == maxNumberOfItems) return false;

                        x.MoveTo(x.FullName + ".processing");
                        counter++;
                        return true;
                    })
                    .Select(f => new FileObject(f.CreationTime, f.Name, f.FullName, null, 0));
        }

        public void DisposeItems(IEnumerable<IDataObject> itemsToDispose)
        {
            foreach (var item in itemsToDispose)
            {
                File.Delete(item.FullName);
            }
        }
    }
}