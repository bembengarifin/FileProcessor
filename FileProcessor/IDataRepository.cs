using System.Collections.Generic;

namespace FileProcessor
{
    public interface IDataRepository<T> where T : IDataObject
    {
        IEnumerable<T> GetNextItemsToProcess(int maxNumberOfItems);
    }
}