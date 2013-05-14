using System;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Runners;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestAttributes {

    [TestClass]
    public class TestJUUTTestClass {

        [TestMethod]
        public void Creation() {
            JUUTAttribute JUUTTest = new JUUTTestClassAttribute();
            AssertEx.That(JUUTTest.Name, Is.EqualTo("JUUTTestClass"));
            AssertEx.That(JUUTTest.IsSetUpOrTearDown, Is.False());
        }

        [TestMethod]
        public void MemberValidation() {
            Type testClassAttribute = typeof(JUUTTestClassAttribute);
            AssertEx.That(JUUTAttribute.IsMemberValidFor(testClassAttribute, typeof(TestClassMock)), Is.True());

            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(testClassAttribute, typeof(NotAttributedMock)), Throws.An<ArgumentException>());
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(testClassAttribute, typeof(NotAttributedMock).GetMethod("Foo")), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void RunnerCreation() {
            AssertEx.That(JUUTTestClassAttribute.CreateRunner(typeof(TestClassWithOnlySimpleTests)), Is.InstanceOf(typeof(SimpleTestRunner)));
            AssertEx.That(JUUTTestClassAttribute.CreateRunner(typeof(TestClassWithTestAfterMethod)), Is.InstanceOf(typeof(CollectingTestRunner)));
        }

        [JUUTTestClass]
        private class TestClassWithOnlySimpleTests {

            [SimpleTestMethod]
            public void TestMethod() { }

        }

        [JUUTTestClass]
        private class TestClassWithTestAfterMethod {

            [SimpleTestMethod]
            public void TestMethod() { }
            [TestAfter]
            public void TestAfterMethod() { }

        }

    }
}
