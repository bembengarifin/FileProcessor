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
        IWant = "I want to be able to create a lock",
        SoThat = "So that the synchronization is in place for concurrent processing")]
    public class Lock_With_Mutex
    {
        [TestMethod]
        public void Lock_Is_Successful_Test()
        {
            new Lock_Is_Successful().BDDfy<Lock_With_Mutex>();
        }

        [TestMethod]
        public void Lock_Is_Already_Held_By_Other_And_Longer_Than_Timeout_Test()
        {
            new Lock_Is_Already_Held_By_Other_And_Longer_Than_Timeout().BDDfy<Lock_With_Mutex>();
        }

        [TestMethod]
        public void Lock_Is_Already_Held_By_Other_And_Shorter_Than_Timeout_Test()
        {
            new Lock_Is_Already_Held_By_Other_And_Shorter_Than_Timeout().BDDfy<Lock_With_Mutex>();
        }
    }

    class Lock_Is_Successful
    {
        private MutextLockManager _lockManager;
        private Boolean _lockResult;

        void Given_Lock_Is_Available()
        {
            _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());
        }

        void When_Lock_Is_Attempted()
        {
            _lockResult = _lockManager.TryLock("abc", 10);
        }

        void Then_Return_Value_Should_Be_True()
        {
            Assert.IsTrue(_lockResult);
        }
    }

    class Lock_Is_Already_Held_By_Other_And_Longer_Than_Timeout
    {
        private MutextLockManager _lockManager;
        private Boolean _lockResult;
        private Mutex _otherMutex;

        void Given_Lock_Is_Already_Held_By_Other_Process()
        {
            _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());

            var t = new Thread(new ThreadStart(() =>
            {
                _otherMutex = new Mutex(false, "Global\\abc");
                _otherMutex.WaitOne();
                Thread.Sleep(3000);
                _otherMutex.ReleaseMutex();
            }));
            t.Start();
        }
        
        void When_Lock_Is_Attempted()
        {
            Thread.Sleep(1000);
            _lockResult = _lockManager.TryLock("abc", 100);
        }

        void Then_Return_Value_Should_Be_False()
        {
            Assert.IsFalse(_lockResult);
        }
    }

    class Lock_Is_Already_Held_By_Other_And_Shorter_Than_Timeout
    {
        private MutextLockManager _lockManager;
        private Boolean _lockResult;
        private Mutex _otherMutex;

        void Given_Lock_Is_Already_Held_By_Other_Process_And_Shorter_Than_Timeout()
        {
            _lockManager = new MutextLockManager(Moq.Mock.Of<ILogger>());

            var t = new Thread(new ThreadStart(() =>
            {
                _otherMutex = new Mutex(false, "Global\\def");
                _otherMutex.WaitOne();
                Thread.Sleep(100);
                _otherMutex.ReleaseMutex();
            }));
            t.Start();
        }

        void When_Lock_Is_Attempted()
        {
            _lockResult = _lockManager.TryLock("def", 500);
        }

        void Then_Return_Value_Should_Be_True()
        {
            Assert.IsTrue(_lockResult);
        }
    }
}
