using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT_Core;
using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT.TestReports {

    [TestClass]
    public class TestClassReport {

        [TestMethod]
        public void Creation() {
            Type type = typeof(TestClass);

            Report report = new SimpleClassReport(type);
            AssertEx.That(report.TestClass, Is.EqualTo(typeof(TestClass)));
            AssertEx.That(report.Status is ReportStatus.NotRunned, Is.True());

            AssertEx.That(() => new SimpleClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new SimpleClassReport(typeof(NotATestClass)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void AddingMethodReport() {
            Type type = typeof(TestClass);
            MethodInfo succeededMethod = type.GetMethod("TestName");
            MethodInfo failedMethod = type.GetMethod("SecondTest");
            ClassReport report = new SimpleClassReport(type);

            MethodReport methodReport1 = new MethodReport(succeededMethod);
            report.Add(methodReport1);
            AssertEx.That(report.Status is ReportStatus.Success, Is.True());
            AssertEx.That(report.SucceededTests, Is.EqualTo(1));
            AssertEx.That(report.RunnedTests, Is.EqualTo(1));

            MethodReport methodReport2 = new MethodReport(failedMethod, new AssertException("Exception Text"));
            report.Add(methodReport2);
            AssertEx.That(report.Status is ReportStatus.Failed, Is.True());
            AssertEx.That(report.FailedTests, Is.EqualTo(1));
            AssertEx.That(report.RunnedTests, Is.EqualTo(2));

            //This tests, that new reports of the same method will be replaced
            MethodReport methodReport3 = new MethodReport(succeededMethod, new NullReferenceException());
            report.Add(methodReport3);
            AssertEx.That(report.Status is ReportStatus.Error, Is.True());
            AssertEx.That(report.FailedTests, Is.EqualTo(2));
            AssertEx.That(report.RunnedTests, Is.EqualTo(2));

            //Reports for methods of another class aren't allowed
            MethodInfo methodInfo = typeof(AnotherTestClass).GetMethod("AnotherTest");
            MethodReport reportOfMethodNotInClass = new MethodReport(methodInfo);
            AssertEx.That(() => report.Add(reportOfMethodNotInClass), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void TextCreation() {
            Type type = typeof(TestClass);
            MethodInfo succeededMethod = type.GetMethod("TestName");
            MethodInfo failedMethod = type.GetMethod("SecondTest");
            ClassReport report = new SimpleClassReport(type);

            AssertEx.That(report.Text, Is.EqualTo("No tests have been runned."));

            MethodReport methodReport1 = new MethodReport(succeededMethod);
            report.Add(methodReport1);
            AssertEx.That(report.Text, Is.EqualTo("1 test runned: 0 failed, 1 succeeded"));

            MethodReport methodReport2 = new MethodReport(failedMethod, new AssertException("Exception Text"));
            report.Add(methodReport2);
            AssertEx.That(report.Text, Is.EqualTo("2 tests runned: 1 failed, 1 succeeded\n" +
                                                  "\n" +
                                                  "The SecondTest-Method failed."));

            MethodReport methodReport3 = new MethodReport(succeededMethod, new NullReferenceException());
            report.Add(methodReport3);
            AssertEx.That(report.Text, Is.EqualTo("2 tests runned: 2 failed, 0 succeeded\n" +
                                                  "\n" +
                                                  "The TestName-Method threw an unexpected exception.\n" +
                                                  "\n" +
                                                  "The SecondTest-Method failed."));
        }

        [TestMethod]
        public void EqualsAndHashCode() {
            Report report = new SimpleClassReport(typeof(TestClass));

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(report.Equals(report), "An object should allways be equal to itself (reference).");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(report.GetHashCode()), "Equal objects should have equal hashcodes.");

            Report equal = new SimpleClassReport(typeof(TestClass));
            Assert.IsTrue(report.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Not equal tests
            Assert.IsFalse(report.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new SimpleClassReport(typeof(AnotherTestClass));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
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
