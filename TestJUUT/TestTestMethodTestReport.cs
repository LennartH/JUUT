using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT.Core;
using JUUT.Core.Attributes;
using JUUT.Core.Impl;
using JUUT.Core.Impl.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

using Throws = NHamcrest.Core.Throws;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {

    [TestClass]
    public class TestTestMethodTestReport {

        [TestMethod]
        public void Creation() {
            MethodInfo testMethod = typeof(TestOwner).GetMethod("TestName");
            Exception raisedException = new AssertException("Exception Text");

            Report report = new TestMethodReport(testMethod, raisedException);
            AssertEx.That(report.TestClass, Is.EqualTo(typeof(TestOwner)));
            AssertEx.That(report.Range, Is.EqualTo(ReportRange.TestMethod));

            //This allows to create a test method report without a raised exception
            report = new TestMethodReport(testMethod);
            AssertEx.That(report.TestClass, Is.EqualTo(typeof(TestOwner)));
            AssertEx.That(report.Range, Is.EqualTo(ReportRange.TestMethod));

            testMethod = new DynamicMethod("Foo", null, null);
            AssertEx.That(() => { new TestMethodReport(testMethod, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new TestMethodReport(testMethod); }, Throws.An<ArgumentException>());

            AssertEx.That(() => { new TestMethodReport(null, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new TestMethodReport(null); }, Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MessageCreation() {
            MethodInfo testMethod = typeof(TestOwner).GetMethod("TestName");
            Exception raisedException = new AssertException("Exception Text");

            Report report = new TestMethodReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test failed: Exception Text"));

            raisedException = new NullReferenceException("Null reference");
            report = new TestMethodReport(testMethod, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test raised an unexpected exception: " + raisedException.Message));

            report = new TestMethodReport(testMethod);
            AssertEx.That(report.Text, Is.EqualTo("The TestName-Test passed successfully."));
        }

        [TestMethod]
        public void ReportToString() {
            Exception raisedException = new NullReferenceException("Exception text");
            Report report = new TestMethodReport(typeof(TestOwner).GetMethod("TestName"), raisedException);
            AssertEx.That(report.ToString(), Is.EqualTo("Test wide report of " + typeof(TestOwner).Name + ": " + report.Text));
        }

        [TestMethod]
        public void EqualsAndHashCode() {
            Report report = new TestMethodReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Exception test."));

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(report.Equals(report), "An object should allways be equal to itself (reference).");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(report.GetHashCode()), "Equal objects should have equal hashcodes.");

            Report equal = new TestMethodReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Exception test."));
            Assert.IsTrue(report.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Not equal tests
            Assert.IsFalse(report.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new TestMethodReport(typeof(TestOwner).GetMethod("UnequalMethod"), new AssertException("Exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new TestMethodReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Unequal exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object with different attributes.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
        }

        private class TestOwner {

            [SimpleTestMethod]
            public void TestName() { }
            [SimpleTestMethod]
            public void UnequalMethod() { }

        }

    }

}
