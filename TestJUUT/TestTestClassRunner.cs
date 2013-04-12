using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core;
using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;
using JUUT.Core.Reports;
using JUUT.Core.Runners;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestSimpleTestClassRunner {

        private static List<string> MethodCallOrder { get; set; }

        [TestInitialize]
        public void InitializeMethodCountersAndTheMethodCallOrder() {
            ResetMethodCountersAndTheMethodCallOrder();
        }

        private static void ResetMethodCountersAndTheMethodCallOrder() {
            MethodCallOrder = new List<string>();
        }

        [TestMethod]
        public void Creation() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            AssertEx.That(runner.TestClass, Is.EqualTo(typeof(TestClassMock)));

            AssertEx.That(() => new SimpleClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new SimpleClassReport(typeof(NotAttributedMock)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void RunAllTestsOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            runner.RunAll();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests();

            //Checking the created test reports
            ICollection<MethodReport> resultReports = runner.Report.MethodReports;
            AssertEx.That(resultReports.Count, Is.EqualTo(2));
            foreach (Report report in resultReports) {
                AssertThatTheReportIsEqualToFooOrBar(report);
            }
        }

        [TestMethod]
        public void RunSpecificTestOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            runner.Run(typeof(TestClassMock).GetMethod("Foo"));
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest();

            //Checking the returned test report
            Report report = runner.Report.MethodsReport[0];
            Report expectedReport = new MethodReport(typeof(TestClassMock).GetMethod("Foo"));
            AssertEx.That(report, Is.EqualTo(expectedReport));

            //Checking the reaction for a method name, that doesn't exist
            runner.Run(typeof(OtherTestClassMock).GetMethod("TestMethod"));
            //TODO What happens if you give the name of a method that doesn't exist in the test class.
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingClassSetUp() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMockWithFailingClassSetUp));

            //Testing the run of a specific testMethod
            runner.Run(typeof(TestClassMockWithFailingClassSetUp).GetMethod("Bar"));
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test report
            Report report = runner.Report.MethodsReport[0];
            Exception raisedException = new NullReferenceException("Failing class set up.");
            Report expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(report, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            ResetMethodCountersAndTheMethodCallOrder();
            runner.RunAll();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test reports
            List<Report> reports = runner.Report.MethodsReport;
            raisedException = new NullReferenceException("Failing class set up.");
            expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(reports[0], Is.EqualTo(expectedReport));
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingTestSetUp() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMockWithFailingTestSetUp));

            //Testing the run of a specific testMethod
            runner.Run(typeof(TestClassMockWithFailingTestSetUp).GetMethod("Foo"));
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test report
            Report returnedReport = runner.Report.MethodsReport[0];
            Exception raisedException = new NullReferenceException("Failing class set up.");
            Report expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(returnedReport, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            ResetMethodCountersAndTheMethodCallOrder();
            runner.RunAll();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test reports
            List<Report> reports = runner.Report.MethodsReport;
            raisedException = new NullReferenceException("Failing class set up.");
            expectedReport = new MethodReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(reports[0], Is.EqualTo(expectedReport));
        }

        //TODO Implement tests for test classes which fail in the other methods

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

        private void AssertThatTheReportIsEqualToFooOrBar(Report report) {
            Report fooReport = new MethodReport(typeof(TestClassMock).GetMethod("Foo"));
            Report barReport = new MethodReport(typeof(TestClassMock).GetMethod("Bar"));

            AssertEx.That(report, Matches.AnyOf(Is.EqualTo(fooReport), Is.EqualTo(barReport)));
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

        }

        [JUUTTestClass]
        private class OtherTestClassMock {

            [SimpleTestMethod]
            public void TestMethod() { }

        }
        #endregion

    }

}
