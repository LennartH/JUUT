using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core;
using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reports;
using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT.TestSessions {

    [TestClass]
    public class TestTestClassSession {

        [TestMethod]
        public void Creation() {
            TestClassSession runner = new TestClassSession(typeof(TestClassMock));
            AssertEx.That(runner.TestClass, Is.EqualTo(typeof(TestClassMock)));

            AssertEx.That(() => new SimpleClassReport(null), Throws.An<ArgumentException>());
            AssertEx.That(() => new SimpleClassReport(typeof(NotAttributedMock)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MethodAdding() {
            TestClassSession session = new TestClassSession(typeof(ThirdTestClassMock));
            AssertEx.That(() => session.Add(typeof(AnotherTestClassMock).GetMethod("TestMethod")), Throws.An<ArgumentException>());
            AssertEx.That(() => session.Add(typeof(ThirdTestClassMock).GetMethod("NotAttributedMethod")), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MethodManagement() {
            TestClassSession classSession = new TestClassSession(typeof(TestClassMock));

            classSession.Add(typeof(TestClassMock).GetMethod("FirstTestMethod"));
            HashSet<MethodInfo> expectedMethods = new HashSet<MethodInfo> { typeof(TestClassMock).GetMethod("FirstTestMethod") };
            Assert.IsTrue(expectedMethods.SetEquals(classSession.TestsToRun));

            classSession.AddAll();
            expectedMethods.Add(typeof(TestClassMock).GetMethod("SecondTestMethod"));
            Assert.IsTrue(expectedMethods.SetEquals(classSession.TestsToRun));
        }

        [TestMethod]
        public void EqualsAndHashCode() {
            TestClassSession session = new TestClassSession(typeof(TestClassMock));
            session.AddAll();

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(session.Equals(session), "An object should allways be equal to itself (reference).");
            AssertEx.That(session.GetHashCode(), Is.EqualTo(session.GetHashCode()), "Equal objects should have equal hashcodes.");

            TestClassSession equal = new TestClassSession(typeof(TestClassMock));
            equal.AddAll();
            Assert.IsTrue(session.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(session.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Not equal tests
            Assert.IsFalse(session.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(session.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(session.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new TestClassSession(typeof(AnotherTestClassMock));
            Assert.IsFalse(session.Equals(unequal), "An object shouldn't be equal to an object with different attributes.");
            AssertEx.That(session.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new TestClassSession(typeof(TestClassMock));
            ((TestClassSession) unequal).Add(typeof(TestClassMock).GetMethod("FirstTestMethod"));
            Assert.IsFalse(session.Equals(unequal), "An object shouldn't be equal to an object with different attributes.");
            AssertEx.That(session.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
        }

        [JUUTTestClass]
        private class AnotherTestClassMock {

            [SimpleTestMethod]
            public void TestMethod() { }

        }

        [JUUTTestClass]
        private class ThirdTestClassMock {

            public void NotAttributedMethod() { }

        }

    }

}
