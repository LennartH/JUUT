using System;
using System.Linq;
using System.Collections.Generic;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reports;
using JUUT_Core.Runners;
using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestSimpleTestClassRunner {

        private static List<string> MethodCallOrder { get; set; }

        [TestInitialize]
        public void InitializeMethodCountersAndTheMethodCallOrder() {
            MethodCallOrder = new List<string>();
        }

        [TestMethod]
        public void RunAllTestsOfJUUTTestClass() {
            TestClassSession session = new TestClassSession(typeof(TestClassMock));
            session.AddAll();

            TestRunner runner = new SimpleTestRunner();
            ClassReport classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests();

            //Checking the created test reports
            ICollection<MethodReport> resultReports = classReport.MethodReports;
            AssertEx.That(resultReports.Count, Is.EqualTo(2));
            foreach (MethodReport report in resultReports) {
                AssertThatTheReportIsEqualToFooOrBar(report);
            }
        }

        [TestMethod]
        public void RunSpecificTestOfJUUTTestClass() {
            TestClassSession session = new TestClassSession(typeof(TestClassMock));
            session.Add(typeof(TestClassMock).GetMethod("Foo"));

            TestRunner runner = new SimpleTestRunner();
            ClassReport classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest();

            //Checking the returned test report
            Report report = GetFirstMethodReportFrom(classReport.MethodReports);
            Report expectedReport = new MethodReport(typeof(TestClassMock).GetMethod("Foo"));
            AssertEx.That(report, Is.EqualTo(expectedReport));
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingClassSetUp() {
            TestClassSession session = new TestClassSession(typeof(TestClassMockWithFailingClassSetUp));
            session.Add(typeof(TestClassMockWithFailingClassSetUp).GetMethod("Bar"));

            //Testing the run of a specific testMethod
            TestRunner runner = new SimpleTestRunner();
            ClassReport classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test report
            Report report = GetFirstMethodReportFrom(classReport.MethodReports);
            Exception raisedException = new NullReferenceException("Failing class set up.");
            Report expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(report, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            MethodCallOrder = new List<string>();
            session.AddAll();
            classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test reports
            ICollection<MethodReport> reports = classReport.MethodReports;
            raisedException = new NullReferenceException("Failing class set up.");
            expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(GetFirstMethodReportFrom(reports), Is.EqualTo(expectedReport));
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingTestSetUp() {
            TestClassSession session = new TestClassSession(typeof(TestClassMockWithFailingTestSetUp));
            session.Add(typeof(TestClassMockWithFailingTestSetUp).GetMethod("Foo"));

            //Testing the run of a specific testMethod
            TestRunner runner = new SimpleTestRunner();
            ClassReport classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test report
            Report returnedReport = GetFirstMethodReportFrom(classReport.MethodReports);
            Exception raisedException = new NullReferenceException("Failing test set up.");
            Report expectedReport = new MethodReport(typeof(TestClassMockWithFailingTestSetUp).GetMethod("SetUp"), raisedException);
            AssertEx.That(returnedReport, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            MethodCallOrder = new List<string>();
            session.AddAll();
            classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test reports
            ICollection<MethodReport> reports = classReport.MethodReports;
            raisedException = new NullReferenceException("Failing test set up.");
            expectedReport = new MethodReport(typeof(TestClassMockWithFailingTestSetUp).GetMethod("SetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(GetFirstMethodReportFrom(reports), Is.EqualTo(expectedReport));
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingTestMethod() {
            TestClassSession session = new TestClassSession(typeof(TestClassMockWithFailingTestMethod));
            session.AddAll();

            TestRunner runner = new SimpleTestRunner();
            ClassReport classReport = runner.Run(session);
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestMethod();

            //Checking the returned test reports
            ICollection<MethodReport> reports = classReport.MethodReports;

            ICollection<MethodReport> expectedReports = new HashSet<MethodReport>();
            expectedReports.Add(new MethodReport(typeof(TestClassMockWithFailingTestMethod).GetMethod("FailingTest"),
                new NullReferenceException("Failing test method.")));
            expectedReports.Add(new MethodReport(typeof(TestClassMockWithFailingTestMethod).GetMethod("WorkingTest")));

            AssertEx.That(reports.Count, Is.EqualTo(2));
            Assert.IsTrue(expectedReports.SequenceEqual(reports));
        }

        [TestMethod]
        public void RunTestAfterMethodCall() {
            TestClassSession session = new TestClassSession(typeof(TestClassWithTestAfterMethod));
            session.AddAll();

            TestRunner runner = new SimpleTestRunner();
            AssertEx.That(runner.Run(session), Is.Null());

            //TODO Other runner is needed
        }

        #region HelperMethods
        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp", "SetUp", "Foo", "TearDown", "SetUp", "Bar", "TearDown", "ClassTearDown" };
            AssertEx.That(MethodCallOrder.SequenceEqual(expectedMethodCallOrder), Is.True(), "Methods weren't called in the correct order.");
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp", "SetUp", "Foo", "TearDown", "ClassTearDown" };
            AssertEx.That(MethodCallOrder.SequenceEqual(expectedMethodCallOrder), Is.True(), "Methods weren't called in the correct order.");
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp" };
            AssertEx.That(MethodCallOrder.SequenceEqual(expectedMethodCallOrder), Is.True(), "Methods weren't called in the correct order.");
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp", "SetUp" };
            AssertEx.That(MethodCallOrder.SequenceEqual(expectedMethodCallOrder), Is.True(), "Methods weren't called in the correct order.");
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestMethod() {
            List<string> expectedMethodCallOrder = new List<string> { "Failing", "Working" };
            AssertEx.That(MethodCallOrder.SequenceEqual(expectedMethodCallOrder), Is.True(), "Methods weren't called in the correct order.");
        }

        private void AssertThatTheReportIsEqualToFooOrBar(Report report) {
            Report fooReport = new MethodReport(typeof(TestClassMock).GetMethod("Foo"));
            Report barReport = new MethodReport(typeof(TestClassMock).GetMethod("Bar"));

            AssertEx.That(report, Matches.AnyOf(Is.EqualTo(fooReport), Is.EqualTo(barReport)));
        }

        private static Report GetFirstMethodReportFrom(IEnumerable<MethodReport> reportCollection) {
            IEnumerator<MethodReport> enumerator = reportCollection.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion

        #region TestClassMocks
        [JUUTTestClass]
        private class TestClassMock {

            [ClassSetUp]
            public static void ClassSetUp() {
                MethodCallOrder.Add("ClassSetUp");
            }
            [TestSetUp]
            public void SetUp() {
                MethodCallOrder.Add("SetUp");
            }

            [SimpleTestMethod]
            public void Foo() {
                MethodCallOrder.Add("Foo");
            }
            [SimpleTestMethod]
            public void Bar() {
                MethodCallOrder.Add("Bar");
            }

            [TestTearDown]
            public void TearDown() {
                MethodCallOrder.Add("TearDown");
            }
            [ClassTearDown]
            public static void ClassTearDown() {
                MethodCallOrder.Add("ClassTearDown");
            }

        }

        [JUUTTestClass]
        private class TestClassMockWithFailingClassSetUp {

            [ClassSetUp]
            public static void ClassSetUp() {
                MethodCallOrder.Add("ClassSetUp");
                throw new NullReferenceException("Failing class set up.");
            }

            [SimpleTestMethod]
            public void Bar() { }

        }

        [JUUTTestClass]
        private class TestClassMockWithFailingTestSetUp {

            [ClassSetUp]
            public static void ClassSetUp() {
                MethodCallOrder.Add("ClassSetUp");
            }

            [TestSetUp]
            public void SetUp() {
                MethodCallOrder.Add("SetUp");
                throw new NullReferenceException("Failing test set up.");
            }

            [SimpleTestMethod]
            public void Foo() { }

        }

        [JUUTTestClass]
        private class OtherTestClassMock {

            [SimpleTestMethod]
            public void TestMethod() { }

        }

        [JUUTTestClass]
        private class TestClassMockWithFailingTestMethod {

            [SimpleTestMethod]
            public void FailingTest() {
                MethodCallOrder.Add("Failing");
                throw new NullReferenceException("Failing test method.");
            }

            [SimpleTestMethod]
            public void WorkingTest() {
                MethodCallOrder.Add("Working");
            }

        }
        #endregion

    }

}
