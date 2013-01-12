using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core;
using JUUT.Core.Impl;
using JUUT.Core.Impl.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;

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
            AssertEx.That(runner.TestClassInfo, Is.EqualTo(typeof(TestClassMock)));
        }

        [TestMethod]
        public void RunAllTestsOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            List<TestReport> resultReports = runner.RunAll();

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningAllTests();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningAllTests();

            //Checking the returned test reports
            AssertEx.That(resultReports.Count, Is.EqualTo(2));
            foreach (TestReport report in resultReports) {
                AssertThatTheReportIsEqualToFooOrBar(report);
            }
        }

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

        private void AssertThatTheReportIsEqualToFooOrBar(TestReport report) {
            TestReport fooReport = new SimpleTestReport(typeof(TestClassMock).GetMethod("Foo"), null);
            TestReport barReport = new SimpleTestReport(typeof(TestClassMock).GetMethod("Bar"), null);

            AssertEx.That(report, Matches.AnyOf(Is.EqualTo(fooReport), Is.EqualTo(barReport)));
        }

        [TestMethod]
        public void RunSpecificTestOfJUUTTestClass() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMock));
            TestReport testReport = runner.Run("Foo");

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningASpecificTest();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningASpecificTest();

            //Checking the returned test report
            TestReport expectedReport = new SimpleTestReport(typeof(TestClassMock).GetMethod("Foo"), null);
            AssertEx.That(testReport, Is.EqualTo(expectedReport));
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

        [TestMethod]
        public void RunTestsOfJUUTTestClassWithFailingClassSetUp() {
            TestRunner runner = new SimpleTestRunner(typeof(TestClassMockWithFailingClassSetUp));
            //Testing the run of a specific method
            runner.Run("Bar");

            //Checking that the methods of the test class mock are called correctly
            AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingClassSetUp();
            AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp();

            //Checking the returned test report
            //TODO A new TestReport is needed for this case

            //TODO Implement the same check for running all tests (reset the static variables before)
        }

        //TODO Implement tests for test classes which fail in the other methods

        private static void AssertThatTheMethodsAreCalledForTheCorrectTimesAfterRunningATestWithFailingClassSetUp() {
            AssertEx.That(ClassSetUpCount, Is.EqualTo(1));
            AssertEx.That(SetUpCount, Is.EqualTo(0));
            AssertEx.That(FooRunnedCount, Is.EqualTo(0));
            AssertEx.That(BarRunnedCount, Is.EqualTo(0));
            AssertEx.That(TearDownCount, Is.EqualTo(0));
            AssertEx.That(ClassTearDownCount, Is.EqualTo(0));
        }

        private static void AssertThatTheMethodsAreCalledInTheCorrectOrderAfterRunningATestWithFailingClassSetUp() {
            List<string> expectedMethodCallOrder = new List<string> { "ClassSetUp" };
            AssertEx.That(MethodCallOrder, Is.EqualTo(expectedMethodCallOrder));
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
        private class TestClassMockWithFailingClassSetUp : TestClassMock {

            [ClassSetUp]
            public new static void ClassSetUp() {
                ClassSetUpCount++;
                MethodCallOrder.Add("ClassSetUp");
                throw new InvalidOperationException();
            }

        }

    }

}
