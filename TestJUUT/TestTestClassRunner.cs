using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core;
using JUUT.Core.Attributes;
using JUUT.Core.Reports;
using JUUT.Core.Runners;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestSimpleTestClassRunner {

        public static int ClassSetUpCount { get; private set; }
        public static int SetUpCount { get; private set; }

        public static int FooRunnedCount { get; private set; }
        public static int BarRunnedCount { get; private set; }

        public static int TearDownCount { get; private set; }
        public static int ClassTearDownCount { get; private set; }

        public static List<string> MethodCallOrder { get; private set; }

        [TestInitialize]
        public void InitializeMethodCountersAndTheMethodCallOrder() {
            ResetMethodCountersAndTheMethodCallOrder();
        }

        private static void ResetMethodCountersAndTheMethodCallOrder() {
            ClassSetUpCount = 0;
            SetUpCount = 0;
            FooRunnedCount = 0;
            BarRunnedCount = 0;
            TearDownCount = 0;
            ClassTearDownCount = 0;

            MethodCallOrder = new List<string>();
        }

        [TestMethod]
        public void Creation() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            AssertEx.That(runner.TestClass, Is.EqualTo(typeof(TestClassMock)));
        }

        [TestMethod]
        public void RunAllTestsOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            List<Report> resultReports = runner.RunAll();

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningAllTests();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests();

            //Checking the returned test reports
            AssertEx.That(resultReports.Count, Is.EqualTo(2));
            foreach (Report report in resultReports) {
                AssertThatTheReportIsEqualToFooOrBar(report);
            }
        }

        [TestMethod]
        public void RunSpecificTestOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            Report report = runner.Run(typeof(TestClassMock).GetMethod("Foo"));

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningASpecificTest();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest();

            //Checking the returned test report
            Report expectedReport = new TestMethodReport(typeof(TestClassMock).GetMethod("Foo"));
            AssertEx.That(report, Is.EqualTo(expectedReport));

            //Checking the reaction for a method name, that doesn't exist
            runner.Run(typeof(OtherTestClassMock).GetMethod("TestMethod"));
            //TODO What happens if you give the name of a method that doesn't exist in the test class.
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingClassSetUp() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMockWithFailingClassSetUp));
            
            //Testing the run of a specific testMethod
            Report returnedReport = runner.Run(typeof(TestClassMockWithFailingClassSetUp).GetMethod("Bar"));

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingClassSetUp();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test report
            Exception raisedException = new InvalidOperationException("Failing class set up.");
            Report expectedReport = new TestClassReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(returnedReport, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            ResetMethodCountersAndTheMethodCallOrder();
            List<Report> reports = runner.RunAll();

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingClassSetUp();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test reports
            raisedException = new InvalidOperationException("Failing class set up.");
            expectedReport = new TestClassReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(reports[0], Is.EqualTo(expectedReport));
        }

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingTestSetUp() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMockWithFailingTestSetUp));

            //Testing the run of a specific testMethod
            Report returnedReport = runner.Run(typeof(TestClassMockWithFailingTestSetUp).GetMethod("Foo"));

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingTestSetUp();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test report
            Exception raisedException = new InvalidOperationException("Failing class set up.");
            Report expectedReport = new TestClassReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(returnedReport, Is.EqualTo(expectedReport));

            //Testing the run of all tests
            ResetMethodCountersAndTheMethodCallOrder();
            List<Report> reports = runner.RunAll();

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingTestSetUp();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp();

            //Checking the returned test reports
            raisedException = new InvalidOperationException("Failing class set up.");
            expectedReport = new TestClassReport(typeof(TestClassMockWithFailingClassSetUp).GetMethod("ClassSetUp"), raisedException);
            AssertEx.That(reports.Count, Is.EqualTo(1));
            AssertEx.That(reports[0], Is.EqualTo(expectedReport));
        }

        //TODO Implement tests for test classes which fail in the other methods

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp", "SetUp", "Foo", "TearDown", "SetUp", "Bar", "TearDown", "ClassTearDown" };
            AssertEx.That(MethodCallOrder, Is.EqualTo(expectedMethodCallOrder));
        }

        private static void AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningAllTests() {
            AssertEx.That(ClassSetUpCount, Is.EqualTo(1));
            AssertEx.That(SetUpCount, Is.EqualTo(2));
            AssertEx.That(FooRunnedCount, Is.EqualTo(1));
            AssertEx.That(BarRunnedCount, Is.EqualTo(1));
            AssertEx.That(TearDownCount, Is.EqualTo(2));
            AssertEx.That(ClassTearDownCount, Is.EqualTo(1));
        }

        private void AssertThatTheReportIsEqualToFooOrBar(Report report) {
            Report fooReport = new TestMethodReport(typeof(TestClassMock).GetMethod("Foo"));
            Report barReport = new TestMethodReport(typeof(TestClassMock).GetMethod("Bar"));

            AssertEx.That(report, Matches.AnyOf(Is.EqualTo(fooReport), Is.EqualTo(barReport)));
        }

        private static void AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningASpecificTest() {
            AssertEx.That(ClassSetUpCount, Is.EqualTo(1));
            AssertEx.That(SetUpCount, Is.EqualTo(1));
            AssertEx.That(FooRunnedCount, Is.EqualTo(1));
            AssertEx.That(BarRunnedCount, Is.EqualTo(0));
            AssertEx.That(TearDownCount, Is.EqualTo(1));
            AssertEx.That(ClassTearDownCount, Is.EqualTo(1));
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp", "SetUp", "Foo", "TearDown", "ClassTearDown" };
            AssertEx.That(MethodCallOrder, Is.EqualTo(expectedMethodCallOrder));
        }

        private static void AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingClassSetUp() {
            AssertEx.That(ClassSetUpCount, Is.EqualTo(1));
            AssertEx.That(SetUpCount, Is.EqualTo(0));
            AssertEx.That(FooRunnedCount, Is.EqualTo(0));
            AssertEx.That(BarRunnedCount, Is.EqualTo(0));
            AssertEx.That(TearDownCount, Is.EqualTo(0));
            AssertEx.That(ClassTearDownCount, Is.EqualTo(0));
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp() {
            AssertEx.That(MethodCallOrderToString(MethodCallOrder), Is.EqualTo(MethodCallOrderToString(new List<string> { "ClassSetUp" })));
        }

        private static void AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingTestSetUp() {
            AssertEx.That(ClassSetUpCount, Is.EqualTo(1));
            AssertEx.That(SetUpCount, Is.EqualTo(1));
            AssertEx.That(FooRunnedCount, Is.EqualTo(0));
            AssertEx.That(BarRunnedCount, Is.EqualTo(0));
            AssertEx.That(TearDownCount, Is.EqualTo(0));
            AssertEx.That(ClassTearDownCount, Is.EqualTo(0));
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingTestSetUp() {
            AssertEx.That(MethodCallOrderToString(MethodCallOrder), Is.EqualTo(MethodCallOrderToString(new List<string> { "ClassSetUp", "SetUp" })));
        }

        private static string MethodCallOrderToString(List<string> methodCallOrder) {
            bool first = true;
            
            string result = "empty";
            foreach (string method in methodCallOrder) {
                if (first) {
                    result = method;
                    first = false;
                } else {
                    result += " " + method;
                }
            }
            return result;
        }

        [JUUTTestClass]
        private class TestClassMock {

            [ClassSetUp]
            public static void ClassSetUp() {
                ClassSetUpCount++;
                MethodCallOrder.Add("ClassSetUp");
            }
            [TestSetUp]
            public void SetUp() {
                SetUpCount++;
                MethodCallOrder.Add("SetUp");
            }

            [SimpleTestMethod]
            public void Foo() {
                FooRunnedCount++;
                MethodCallOrder.Add("Foo");
            }
            [SimpleTestMethod]
            public void Bar() {
                BarRunnedCount++;
                MethodCallOrder.Add("Bar");
            }

            [TestTearDown]
            public void TearDown() {
                TearDownCount++;
                MethodCallOrder.Add("TearDown");
            }
            [ClassTearDown]
            public static void ClassTearDown() {
                ClassTearDownCount++;
                MethodCallOrder.Add("ClassTearDown");
            }

        }

        [JUUTTestClass]
        private class TestClassMockWithFailingClassSetUp {

            [ClassSetUp]
            public static void ClassSetUp() {
                ClassSetUpCount++;
                MethodCallOrder.Add("ClassSetUp");
                throw new InvalidOperationException("Failing class set up.");
            }

        }

        [JUUTTestClass]
        private class TestClassMockWithFailingTestSetUp {

            [ClassSetUp]
            public static void ClassSetUp() {
                ClassSetUpCount++;
                MethodCallOrder.Add("ClassSetUp");
            }

            [TestSetUp]
            public void SetUp() {
                SetUpCount++;
                MethodCallOrder.Add("SetUp");
                throw new InvalidOperationException("Failing test set up.");
            }

        }

        [JUUTTestClass]
        private class OtherTestClassMock {

            [SimpleTestMethod]
            public void TestMethod() { }

        }

    }

}
