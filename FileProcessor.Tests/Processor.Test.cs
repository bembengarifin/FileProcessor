using System;
using Moq;
using TestStack.BDDfy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy.Configuration;

namespace FileProcessor.Tests
{
    [TestClass]
    [Story(
        AsA = "As a File Processor Instance",
        IWant = "I want to ensure I'm the only one picking up items",
        SoThat = "So that there is no race condition where multiple file processors are picking up same files")]
    public class File_Processor_Processes_Items
    {
        // Test Runners
        [TestMethod]
        public void Lock_Is_Available_Immediately_Test()
        {
            new Lock_Is_Available_Immediately().BDDfy<File_Processor_Processes_Items>();
        }

        [TestMethod]
        public void Lock_Is_Not_Available_Test()
        {
            new Lock_Is_Not_Available().BDDfy<File_Processor_Processes_Items>();
        }

        [TestMethod]
        public void Lock_Is_Not_Available_Immediately_But_Then_Available_Test()
        {
            this.Given(() => Console.WriteLine("test"), "Given_The_Lock_Is_Not_Available_Immediately")
                .When(() => _processor.RunProcess(), "When_The_Run_Process_Is_Executed")
                .Then(() => Console.WriteLine("verify"), "Then_Items_Should_Be_Returned_For_Further_Processing")
                .BDDfy();
        }

        protected Processor _processor;
        protected Mock<IDataRepository<IDataObject>> _mockedDataRepo;
        protected Mock<ILogger> _mockedLogger;
        protected Mock<ILockManager> _mockedLockManager;

        [TestInitialize]
        public void Setup()
        {
            _mockedLogger = new Mock<ILogger>();
            _mockedDataRepo = new Mock<IDataRepository<IDataObject>>();
            _mockedLockManager = new Mock<ILockManager>();
            _processor = new Processor(_mockedLogger.Object, _mockedDataRepo.Object);
        }
        
        class Lock_Is_Available_Immediately : File_Processor_Processes_Items
        {
            void Given_Lock_Is_Not_Being_Held_By_Any_Other_Process()
            {
            }

            void When_The_Run_Process_Is_Executed()
            {
                _processor.RunProcess();
            }

            void Then_Items_Should_Be_Returned_For_Further_Processing()
            {
                _mockedDataRepo.Verify(x => x.GetNextItemsToProcess(10));
            }

            void And_Lock_Should_Be_Released_Back()
            {
                _mockedLockManager.Verify();
            }
        }

        class Lock_Is_Not_Available : File_Processor_Processes_Items
        {
            void Given_Lock_Is_Being_Held_By_Other_Process()
            {
            }

            void And_Given_It_Is_Not_Released_Before_Time_Out()
            {

            }

            void When_The_Run_Process_Is_Executed()
            {
                _processor.RunProcess();
            }

            void Then_No_Item_Should_Be_Returned_For_Further_Processing()
            {
                _mockedDataRepo.Verify(x => x.GetNextItemsToProcess(10));
            }

            public void And_Lock_Should_Be_Released_Back()
            {

            }
        }
    }



}
