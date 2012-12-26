using System;
using System.Reflection;
using System.Reflection.Emit;

using JUUT;
using JUUT.Core;
using JUUT.Core.Impl;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

using Throws = NHamcrest.Core.Throws;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {

    [TestClass]
    public class TestSimpleTestReport {

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

        [TestMethod]
        public void EqualsAndHashCode() {
            TestReport report = new SimpleTestReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Exception test."));

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(report.Equals(report), "An object should allways be equal to itself (reference).");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(report.GetHashCode()), "Equal objects should have equal hashcodes.");

            TestReport equal = new SimpleTestReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Exception test."));
            Assert.IsTrue(report.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(report.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Not equal tests
            Assert.IsFalse(report.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new SimpleTestReport(typeof(TestOwner).GetMethod("UnequalMethod"), new AssertException("Exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new SimpleTestReport(typeof(TestOwner).GetMethod("TestName"), new AssertException("Unequal exception test."));
            Assert.IsFalse(report.Equals(unequal), "An object shouldn't be equal to an object with different attributes.");
            AssertEx.That(report.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
        }

        private class TestOwner {

            public void TestName() { }
            public void UnequalMethod() { }

        }

    }

}
