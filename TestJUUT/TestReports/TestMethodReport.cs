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

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT.TestReports {

    [TestClass]
    public class TestMethodReport {

        [TestMethod]
        public void Creation() {
            MethodInfo testMethod = typeof(TestClass).GetMethod("TestName");
            Exception raisedException = new AssertException("Exception Text");

            Report report = new MethodReport(testMethod, raisedException);
            AssertEx.That(report.ClassType, Is.EqualTo(typeof(TestClass)));
            AssertEx.That(report.Status is ReportStatus.Failed, Is.True());

            //Check that a report can be created with other JUUTMethodAttributes
            report = new MethodReport(typeof(TestClass).GetMethod("SetUp"), raisedException);

            //This allows to create a test method report without a raised exception
            report = new MethodReport(testMethod);
            AssertEx.That(report.ClassType, Is.EqualTo(typeof(TestClass)));
            AssertEx.That(report.Status is ReportStatus.Success, Is.True());

            testMethod = new DynamicMethod("Foo", null, null);
            AssertEx.That(() => { new MethodReport(testMethod, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new MethodReport(testMethod); }, Throws.An<ArgumentException>());

            AssertEx.That(() => { new MethodReport(null, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new MethodReport(null); }, Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MessageCreation() {
            MethodInfo testMethod = typeof(TestClass).GetMethod("TestName");
            Exception raisedException = new AssertException("Exception Text");

            Report report = new MethodReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Method failed: Exception Text"));

            raisedException = new NullReferenceException("Null reference");
            report = new MethodReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Method raised an unexpected exception: " + raisedException.Message));

            report = new MethodReport(testMethod);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Method passed successfully."));
        }

        [TestMethod]
        public void ReportToString() {
            Exception raisedException = new NullReferenceException("Exception text");
            Report report = new MethodReport(typeof(TestClass).GetMethod("TestName"), raisedException);
            AssertEx.That(report.ToString(), Is.EqualTo(report.Text));
        }

        [TestMethod]
        public void EqualsAndHashCode() {
            Report report = new MethodReport(typeof(TestClass).GetMethod("TestName"), new AssertException("Exception test."));

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(report.Equals(report), "An object should allways be equal to itself (reference).");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(report.GetHashCode()), "Equal objects should have equal hashcodes.");

            Report equal = new MethodReport(typeof(TestClass).GetMethod("TestName"), new AssertException("Exception test."));
            Assert.IsTrue(report.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Not equal tests
            Assert.IsFalse(report.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new MethodReport(typeof(TestClass).GetMethod("UnequalMethod"), new AssertException("Exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new MethodReport(typeof(TestClass).GetMethod("TestName"), new AssertException("Unequal exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object with different attributes.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
        }

        private class TestClass {

            [TestSetUp]
            public void SetUp() { }

            [SimpleTestMethod]
            public void TestName() { }
            [SimpleTestMethod]
            public void UnequalMethod() { }

        }

    }

}
