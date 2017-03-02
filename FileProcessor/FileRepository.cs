using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FileProcessor
{
    public class FileRepository : IDataRepository<IDataObject> 
    {
        private readonly DirectoryInfo _directoryInfo;

        public FileRepository(string directoryPath)
        {
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
                    .Select(f => new FileObject(f.CreationTime, f.Name, null, 0));
        }

        public void DisposeItems(IEnumerable<IDataObject> itemsToDispose)
        {
            foreach (var item in itemsToDispose)
            {   
                File.Delete(Path.Combine(_directoryInfo.FullName, item.FileName) + ".processing");
            }
        }
    }
}