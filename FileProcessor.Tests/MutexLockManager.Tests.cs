using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TestStack.BDDfy;

namespace FileProcessor.Tests
{
    [TestClass]
    [Story(
        AsA = "As a Lock Manager",
        IWant = "I want to be able to create a lock then perform some retrieval",
        SoThat = "So that the synchronization is in place for concurrent retrieval")]
    public abstract class Lock_And_Get
    {
        protected MutextLockManager _lockManager;
        protected Boolean _lockResult;
        protected string _lockKey;
        protected Func<string> _get;
        protected string _dummyGetOutput;
        protected string _actualOutput;

        [TestMethod]
        public void ExecuteTestScenario()
        {
            this.BDDfy<Lock_And_Get>(this.GetType().Name.Replace("_", " "));
        }

        [TestInitialize]
        public void Setup()
        {
            _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());
            _lockKey = Guid.NewGuid().ToString();
            _dummyGetOutput = "GetOutput";
            _get = () => { return _dummyGetOutput; };
            _actualOutput = null;
        }
        
        [TestClass]
        public class Lock_Is_Not_Held_By_Any_Other_Process : Lock_And_Get
        {   
            public void Given_Lock_Is_Available()
            {   
            }

            public void When_Lock_And_Get_Is_Attempted()
            {
                _lockResult = _lockManager.TryLockAndGet(_lockKey, 10, _get, out _actualOutput);
            }

            public void Then_Returned_Result_Should_Be_True()
            {
                Assert.IsTrue(_lockResult);
            }

            public void Then_Output_Should_Be_Returned()
            {
                Assert.AreEqual(_dummyGetOutput, _actualOutput);
            }
        }

        [TestClass]
        public class Lock_Is_Already_Held_By_Other_And_Longer_Than_Timeout : Lock_And_Get
        {
            private Mutex _otherMutex;

            public void Given_Lock_Is_Already_Held_By_Other_Process()
            {
                _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());

                var t = new Thread(new ThreadStart(() =>
                {
                    _otherMutex = new Mutex(false, "Global\\" + _lockKey);
                    _otherMutex.WaitOne();
                    Thread.Sleep(3000);
                    _otherMutex.ReleaseMutex();
                }));
                t.Start();
            }

            public void When_Lock_And_Get_Is_Attempted()
            {
                Thread.Sleep(1000); // delay so the the other thread will run first
                _lockResult = _lockManager.TryLockAndGet(_lockKey, 10, _get, out _actualOutput);
            }

            public void Then_Returned_Result_Should_Be_False()
            {
                Assert.IsFalse(_lockResult);
            }

            public void Then_Output_Should_Be_Null()
            {
                Assert.IsNull(_actualOutput);
            }
        }

        [TestClass]
        public class Lock_Is_Already_Held_By_Other_And_Shorter_Than_Timeout : Lock_And_Get
        {
            private Mutex _otherMutex;

            public void Given_Lock_Is_Already_Held_By_Other_Process_And_Shorter_Than_Timeout()
            {
                _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());

                var t = new Thread(new ThreadStart(() =>
                {
                    _otherMutex = new Mutex(false, "Global\\" + _lockKey);
                    _otherMutex.WaitOne();
                    Thread.Sleep(3000);
                    _otherMutex.ReleaseMutex();
                }));
                t.Start();
            }

            public void When_Lock_And_Get_Is_Attempted()
            {
                Thread.Sleep(1000); 
                _lockResult = _lockManager.TryLockAndGet(_lockKey, 4000, _get, out _actualOutput);
            }

            public void Then_Returned_Result_Should_Be_True()
            {
                Assert.IsTrue(_lockResult);
            }

            public void Then_Output_Should_Be_Returned()
            {
                Assert.AreEqual(_dummyGetOutput, _actualOutput);
            }
        }
    }

    [TestClass]
    [Story(
            AsA = "As a Lock Manager",
            IWant = "I want to be able to check any lock exists",
            SoThat = "So that the caller can make decision on further synchronization")]
    public abstract class Check_If_Lock_Exists
    {
        protected MutextLockManager _lockManager;
        protected Boolean _lockResult;
        protected string _lockKey;
        
        [TestMethod]
        public void ExecuteTestScenario()
        {
            this.BDDfy<Check_If_Lock_Exists>(this.GetType().Name.Replace("_", " "));
        }

        [TestInitialize]
        public void Setup()
        {
            _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());
            _lockKey = Guid.NewGuid().ToString();
        }

        [TestClass]
        public class Lock_Is_Not_Held_By_Any_Other_Process : Check_If_Lock_Exists
        {
            public void Given_Lock_Is_Available()
            {
            }

            public void When_Check_Lock_Is_Called()
            {
                _lockResult = _lockManager.CheckIfLockExists(_lockKey);
            }

            public void Then_Returned_Result_Should_Be_False()
            {
                Assert.IsFalse(_lockResult);
            }
        }

        [TestClass]
        public class Lock_Is_Already_Held_By_Other : Check_If_Lock_Exists
        {
            private Mutex _otherMutex;

            public void Given_Lock_Is_Already_Held_By_Other_Process()
            {
                _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());

                var t = new Thread(new ThreadStart(() =>
                {
                    _otherMutex = new Mutex(false, "Global\\" + _lockKey);
                    _otherMutex.WaitOne();
                    Thread.Sleep(3000);
                    _otherMutex.ReleaseMutex();
                }));
                t.Start();
            }

            public void When_Check_Lock_Is_Called()
            {
                Thread.Sleep(1000); // delay so the the other thread will run first
                _lockResult = _lockManager.CheckIfLockExists(_lockKey);
            }

            public void Then_Returned_Result_Should_Be_True()
            {
                Assert.IsTrue(_lockResult);
            }
        }
    }
}
