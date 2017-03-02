using System;
using Moq;
using TestStack.BDDfy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy.Configuration;
using System.Collections.Generic;

namespace FileProcessor.Tests
{
    [TestClass]
    [Story(
        AsA = "As a File Processor Instance",
        IWant = "I want to retrieve files",
        SoThat = "So that there is no race condition where multiple file processors are picking up same files")]
    public abstract class File_Processor_Processes_Items 
    {
        protected Processor _processor;
        protected Mock<IDataRepository<IDataObject>> _mockedDataRepo;
        protected Mock<ILogger> _mockedLogger;
        protected Mock<ILockManager> _mockedLockManager;
        private string _getFileLockKey;
        private int _lockMsTimeout;
        private int _itemsToFetchAtATime;

        [TestInitialize]
        public void Setup()
        {
            _mockedLogger = new Mock<ILogger>();
            _mockedDataRepo = new Mock<IDataRepository<IDataObject>>();
            _mockedLockManager = new Mock<ILockManager>();
            _getFileLockKey = "PROCESSORGETLOCK";
            _lockMsTimeout = 1000;
            _itemsToFetchAtATime = 20;
            _processor = new Processor(_mockedLogger.Object, _mockedDataRepo.Object, _mockedLockManager.Object, _getFileLockKey, _lockMsTimeout, _itemsToFetchAtATime);
        }

        [TestMethod]
        public void ExecuteTestScenario()
        {
            this.BDDfy<File_Processor_Processes_Items>(this.GetType().Name.Replace("_", " "));
        }

        [TestClass]
        public class Lock_Is_Available_Immediately : File_Processor_Processes_Items
        {
            public void Given_Lock_Is_Not_Being_Held_By_Any_Other_Process()
            {
                _mockedDataRepo.Setup(x => x.GetNextItemsToProcess(_itemsToFetchAtATime));

                IEnumerable<IDataObject> result;
                Func<IEnumerable<IDataObject>> get = () => _mockedDataRepo.Object.GetNextItemsToProcess(_itemsToFetchAtATime);
                _mockedLockManager.Setup(x => x.TryLockAndGet(_getFileLockKey, _lockMsTimeout, get, out result)).Returns(true);
            }

            public void When_The_Run_Process_Is_Executed()
            {
                _processor.RunProcess();
            }

            public void Then_Lock_Manager_Was_Called()
            {
                _mockedLockManager.Verify();
            }

            public void And_Data_Should_Be_Retrieved_For_Further_Processing()
            {
                _mockedDataRepo.Verify();
            }
        }

        [TestClass]
        public class Lock_Is_Not_Available : File_Processor_Processes_Items
        {
            public void Given_Lock_Is_Being_Held_By_Other_Process()
            {
                IEnumerable<IDataObject> result;
                Func<IEnumerable<IDataObject>> get = () => _mockedDataRepo.Object.GetNextItemsToProcess(_itemsToFetchAtATime);
                _mockedLockManager.Setup(x => x.TryLockAndGet(_getFileLockKey, _lockMsTimeout, get, out result)).Returns(false);
            }
            
            public void When_The_Run_Process_Is_Executed()
            {
                _processor.RunProcess();
            }

            public void Then_Lock_Manager_Was_Called()
            {
                _mockedLockManager.Verify();
            }

            public void And_No_Data_Should_Be_Retrieved_For_Further_Processing()
            {
                _mockedDataRepo.Verify(x => x.GetNextItemsToProcess(_itemsToFetchAtATime), Times.Never);
            }
        }

        
    }



}
