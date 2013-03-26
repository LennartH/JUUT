using System;
using System.Reflection;
using System.Reflection.Emit;

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

            AssertEx.That(() => new ClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new ClassReport(typeof(NotATestClass)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void AddingMethodReport() {
            
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

        private class NotATestClass { }

    }

}
