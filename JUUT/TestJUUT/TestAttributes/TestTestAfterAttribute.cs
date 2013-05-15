using System;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestAttributes {

    [TestClass]
    public class TestTestAfterAttribute {

        [TestMethod]
        public void Creation() {
            JUUTTestMethodAttribute testAfter = new TestAfterAttribute(typeof(TestClassTarget), "TargetMethod");
            AssertEx.That(testAfter.Name, Is.EqualTo("TestAfter"));
            AssertEx.That(testAfter.IsSetUpOrTearDown, Is.False());
            AssertEx.That(testAfter.IsTestReadyToRun, Is.False());
        }

        [TestMethod]
        public void MemberValidation() {
            MethodInfo simpleTestMethod = typeof(NotAttributedMock).GetMethod("Foo");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(TestAfterAttribute), simpleTestMethod), Is.False());
            simpleTestMethod = typeof(TestClassWithTestAfterMethod).GetMethod("TestAfterMethod");
            bool t = JUUTAttribute.IsMemberValidFor(typeof(TestAfterAttribute), simpleTestMethod);
            AssertEx.That(t, Is.True());

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestAfterAttribute), classType), Throws.An<InvalidCastException>());
            simpleTestMethod = typeof(TestClassWithMethodsWithParameters).GetMethod("TestAfterMethod");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestAfterAttribute), simpleTestMethod), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void ReadyToRunAfterTargetHasBeenCalled() {
            JUUTTestMethodAttribute testAfter = new TestAfterAttribute(typeof(TestClassTarget), "TargetMethod");
            TestClassTarget target = new TestClassTarget();
            target.TargetMethod();
            AssertEx.That(testAfter.IsTestReadyToRun, Is.True());
        }

    }
}
