using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reports;
using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestSessions {

    [TestClass]
    public class TestTestClassSession {

        [TestMethod]
        public void Creation() {
            TestClassSession runner = new TestClassSession(typeof(TestClassMock));
            AssertEx.That(runner.TestClass, Is.EqualTo(typeof(TestClassMock)));

            AssertEx.That(() => new SimpleClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new SimpleClassReport(typeof(NotAttributedMock)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MethodAdding() {
            TestClassSession session = new TestClassSession(typeof(ThirdTestClassMock));
            AssertEx.That(() => session.Add(typeof(AnotherTestClassMock).GetMethod("TestMethod")), Throws.An<ArgumentException>());
            AssertEx.That(() => session.Add(typeof(ThirdTestClassMock).GetMethod("NotAttributedMethod")), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MethodManagement() {
            TestClassSession classSession = new TestClassSession(typeof(TestClassMock));

            classSession.Add(typeof(TestClassMock).GetMethod("FirstTestMethod"));
            HashSet<MethodInfo> expectedMethods = new HashSet<MethodInfo> { typeof(TestClassMock).GetMethod("FirstTestMethod") };
            Assert.IsTrue(expectedMethods.SetEquals(classSession.TestsToRun));

            classSession.AddAll();
            expectedMethods.Add(typeof(TestClassMock).GetMethod("SecondTestMethod"));
            Assert.IsTrue(expectedMethods.SetEquals(classSession.TestsToRun));
        }

        [JUUTTestClass]
        private class AnotherTestClassMock {

            [SimpleTestMethod]
            public void TestMethod() { }

        }

        [JUUTTestClass]
        private class ThirdTestClassMock {

            public void NotAttributedMethod() { }

        }

    }

}
