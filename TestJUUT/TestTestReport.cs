using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT;
using JUUT.Core;
using JUUT.Core.Impl;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Throws = NHamcrest.Core.Throws;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {

    [TestClass]
    public class TestTestReport {

        [TestMethod]
        public void Creation() {
            MethodInfo testMethod = new DynamicMethod("TestName", null, null);
            Exception raisedException = new AssertException("Exception Text");

            TestReport report = new SimpleTestReport(testMethod, raisedException);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Failed));
            AssertEx.That(report.TestMethod, Is.EqualTo(testMethod));

            raisedException = new NullReferenceException();
            report = new SimpleTestReport(testMethod, raisedException);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Error));
            AssertEx.That(report.TestMethod, Is.EqualTo(testMethod));

            report = new SimpleTestReport(testMethod, null);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Passed));
            AssertEx.That(report.TestMethod, Is.EqualTo(testMethod));

            AssertEx.That(() => { new SimpleTestReport(null, raisedException); }, Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void Output() {
            MethodInfo testMethod = new DynamicMethod("TestName", null, null);
            Exception raisedException = new AssertException("Exception Text");

            TestReport report = new SimpleTestReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test failed: Exception Text"));

            raisedException = new NullReferenceException("Null reference");
            report = new SimpleTestReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test raised an unexpected exception: " + raisedException.Message));

            report = new SimpleTestReport(testMethod, null);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test passed successfully."));
        }

        [TestMethod]
        public void TestClassType() {
            MethodInfo testMethod = new DynamicMethod("TestName", null, null);
            Exception raisedException = new AssertException("Exception Text");
            
            TestReport report = new SimpleTestReport(testMethod, raisedException);
            AssertEx.That(report.TestClassType, Is.Null());

            report = new SimpleTestReport(typeof(TestOwner).GetMethod("TestName"), raisedException);
            AssertEx.That(report.TestClassType, Is.EqualTo(typeof(TestOwner)));
        }

        private class TestOwner {

            public void TestName() { }

        }

    }

}
