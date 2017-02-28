using System.Collections.Generic;

namespace FileProcessor
{
    interface IDataRepository<T> where T : IDataObject
    {
        IEnumerable<T> GetNextItemsToProcess(int maxNumberOfItems);
    }
}