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

            Report report = new ClassReport(type);
            AssertEx.That(report.ClassType, Is.EqualTo(typeof(TestClass)));
            AssertEx.That(report.Status is ReportStatus.NotRunned, Is.True());
            AssertEx.That(report.Text, Is.EqualTo("No tests have been runned."));

            AssertEx.That(() => new ClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new ClassReport(typeof(NotATestClass)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void AddingMethodReport() {
            Type type = typeof(TestClass);
            MethodInfo methodInfo = type.GetMethod("TestName");
            ClassReport report = new ClassReport(type);

            MethodReport methodReport1 = new MethodReport(methodInfo);
            report.Add(methodReport1);
            AssertEx.That(report.Status is ReportStatus.Success, Is.True());

            MethodReport methodReport2 = new MethodReport(methodInfo, new AssertException("Exception Text"));
            report.Add(methodReport2);
            AssertEx.That(report.Status is ReportStatus.Failed, Is.True());

            //This tests, that new reports of the same method will be replaced
            MethodReport methodReport3 = new MethodReport(methodInfo, new NullReferenceException());
            report.Add(methodReport3);
            AssertEx.That(report.Status is ReportStatus.Error, Is.True());

            methodInfo = typeof(AnotherTestClass).GetMethod("AnotherTest");
            MethodReport reportOfMethodNotInClass = new MethodReport(methodInfo);
            AssertEx.That(() => report.Add(reportOfMethodNotInClass), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void TextCreation() {
            
        }

        [JUUTTestClass]
        private class TestClass {

            [TestSetUp]
            public void SetUp() { }

            [SimpleTestMethod]
            public void TestName() { }

        }

        [JUUTTestClass]
        private class AnotherTestClass {

            [SimpleTestMethod]
            public void AnotherTest() { }

        }

        private class NotATestClass { }

    }

}
