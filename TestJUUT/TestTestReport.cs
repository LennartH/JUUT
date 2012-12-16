using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT;

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

            TestReport report = new TestReport(testMethod, raisedException);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Failed));

            raisedException = new NullReferenceException();
            report = new TestReport(testMethod, raisedException);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Error));

            report = new TestReport(testMethod, null);
            AssertEx.That(report.TestStatus, Is.EqualTo(TestStatus.Passed));

            AssertEx.That(() => { new TestReport(null, raisedException); }, Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void Output() {
            MethodInfo testMethod = new DynamicMethod("TestName", null, null);
            Exception raisedException = new AssertException("Exception Text");

            TestReport report = new TestReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test failed: Exception Text"));

            raisedException = new NullReferenceException("Null reference");
            report = new TestReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test raised an unexpected exception: " + raisedException.Message));

            report = new TestReport(testMethod, null);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test passed successfully."));
        }

    }

}
