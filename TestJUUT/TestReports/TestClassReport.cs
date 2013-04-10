using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT.Core;
using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;
using JUUT.Core.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestReports {

    [TestClass]
    public class TestClassReport {

        [TestMethod]
        public void Creation() {
            Type type = typeof(TestClass);

            Report report = new SimpleClassReport(type);
            AssertEx.That(report.ClassType, Is.EqualTo(typeof(TestClass)));
            AssertEx.That(report.Status is ReportStatus.NotRunned, Is.True());
            AssertEx.That(report.Text, Is.EqualTo("No tests have been runned."));

            AssertEx.That(() => new SimpleClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new SimpleClassReport(typeof(NotATestClass)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void AddingMethodReport() {
            Type type = typeof(TestClass);
            MethodInfo succeededMethod = type.GetMethod("TestName");
            MethodInfo failedMethod = type.GetMethod("SecondTest");
            ClassReport report = new SimpleClassReport(type);
            AssertEx.That(report.NotRunnedTests, Is.EqualTo(2));

            MethodReport methodReport1 = new MethodReport(succeededMethod);
            report.Add(methodReport1);
            AssertEx.That(report.Status is ReportStatus.Success, Is.True());
            AssertEx.That(report.SucceededTests, Is.EqualTo(1));
            AssertEx.That(report.RunnedTests, Is.EqualTo(1));
            AssertEx.That(report.NotRunnedTests, Is.EqualTo(1));

            MethodReport methodReport2 = new MethodReport(failedMethod, new AssertException("Exception Text"));
            report.Add(methodReport2);
            AssertEx.That(report.Status is ReportStatus.Failed, Is.True());
            AssertEx.That(report.FailedTests, Is.EqualTo(1));
            AssertEx.That(report.RunnedTests, Is.EqualTo(2));
            AssertEx.That(report.NotRunnedTests, Is.EqualTo(0));

            //This tests, that new reports of the same method will be replaced
            MethodReport methodReport3 = new MethodReport(succeededMethod, new NullReferenceException());
            report.Add(methodReport3);
            AssertEx.That(report.Status is ReportStatus.Error, Is.True());
            AssertEx.That(report.FailedTests, Is.EqualTo(2));
            AssertEx.That(report.RunnedTests, Is.EqualTo(2));
            AssertEx.That(report.NotRunnedTests, Is.EqualTo(0));

            //Reports for methods of another class aren't allowed
            MethodInfo methodInfo = typeof(AnotherTestClass).GetMethod("AnotherTest");
            MethodReport reportOfMethodNotInClass = new MethodReport(methodInfo);
            AssertEx.That(() => report.Add(reportOfMethodNotInClass), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void TextCreation() {
            throw new NotImplementedException();
        }

        [JUUTTestClass]
        private class TestClass {

            [TestSetUp]
            public void SetUp() { }

            [SimpleTestMethod]
            public void TestName() { }

            [SimpleTestMethod]
            public void SecondTest() { }

        }

        [JUUTTestClass]
        private class AnotherTestClass {

            [SimpleTestMethod]
            public void AnotherTest() { }

        }

        private class NotATestClass { }

    }

}
