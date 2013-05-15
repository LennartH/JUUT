using System;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Runners;
using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestRunners {

    [TestClass]
    public class TestCollectingTestRunner {

        [TestMethod]
        public void RunTestAfterMethodCall() {
            TestClassSession session = new TestClassSession(typeof(TestClassMock));
            session.AddAll();

            TestRunner runner = new CollectingTestRunner();
            AssertEx.That(runner.Run(session), Is.Null());

            //TODO Other runner is needed
        }

        #region Helper Classes
        [JUUTTestClass]
        private class TestClassMock {

            [TestAfter(typeof(TestClassTarget), "TargetMethod")]
            public void TestAfterMethod() { }

        }
        #endregion

    }

}