﻿using System;
using System.Linq;
using System.Collections.Generic;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reporters;
using JUUT_Core.Sessions;
using JUUT_Core.Suites;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestAbstractTestSuite {

        private static List<string> MethodCallOrder { get; set; }

        [TestInitialize]
        public void InitializeMethodCountersAndTheMethodCallOrder() {
            MethodCallOrder = new List<string>();
        }

        [TestMethod]
        public void Creation() {
            AssertEx.That(() => new TestSuiteMock(null), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void RunningTheSuite() {
            Mock<AbstractTestReporter> reporterMock = new Mock<AbstractTestReporter>();
            TestSuite suite = new TestSuiteMock(reporterMock.Object);

            Session session = new Session();
            session.AddAll(typeof(TestClass));
            session.Add(typeof(AnotherTestClass).GetMethod("Blub"));
            suite.Run(session);
            AssertThatTheMethodsAreCalledCorrectly();
            reporterMock.Verify(rep => rep.PresentReports(), Times.Once());
        }

        #region Helpers
        private void AssertThatTheMethodsAreCalledCorrectly() {
            List<string> possibleOrder1 = new List<string> {"Foo", "Bar", "Blub"};
            List<string> possibleOrder2 = new List<string> {"Blub", "Foo", "Bar"};
            AssertEx.That(
                MethodCallOrder.SequenceEqual(possibleOrder1) || MethodCallOrder.SequenceEqual(possibleOrder2),
                Is.True());
        }

        private class TestSuiteMock : AbstractTestSuite {

            public TestSuiteMock(TestReporter reporter) : base(reporter) {
            }

        }

        [JUUTTestClass]
        private class TestClass {

            [SimpleTestMethod]
            public void Foo() {
                MethodCallOrder.Add("Foo");
            }

            [SimpleTestMethod]
            public void Bar() {
                MethodCallOrder.Add("Bar");
            }

        }

        [JUUTTestClass]
        private class AnotherTestClass {

            [SimpleTestMethod]
            public void Blub() {
                MethodCallOrder.Add("Blub");
            }

            [SimpleTestMethod]
            public void Bla() {
                MethodCallOrder.Add("Bla");
            }

        }

        private class NotATestClass {

            public void NotATest() {
            }

        }
        #endregion

    }

}