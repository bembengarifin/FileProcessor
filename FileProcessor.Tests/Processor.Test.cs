using System;
using Xunit;
using Moq;

namespace FileProcessor.Tests
{
    public class When_Run_Process_Is_Called
    {
        [Fact]
        public void There_Should_Be_Called_To_Retrieve_Data()
        {
            var mockLogger = new Mock<ILogger>();
            var mockDataRepo = new Mock<IDataRepository<IDataObject>>();
            Processor p = new Processor(mockLogger.Object, mockDataRepo.Object);

            p.RunProcess();

            mockDataRepo.Verify(x => x.GetNextItemsToProcess(10));
        }
    }
}
