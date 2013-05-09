using System;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.AttributeTests {

    [TestClass]
    public class TestSimpleTestMethodAttribute {

        [TestMethod]
        public void Creation() {
            JUUTAttribute testTearDown = new SimpleTestMethodAttribute();
            AssertEx.That(testTearDown.Name, Is.EqualTo("SimpleTestMethod"));
            AssertEx.That(testTearDown.IsSetUpOrTearDown, Is.False());
        }

        [TestMethod]
        public void MemberValidation() {
            MethodInfo simpleTestMethod = typeof(NotAttributedMock).GetMethod("Foo");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(SimpleTestMethodAttribute), simpleTestMethod), Is.False());
            simpleTestMethod = typeof(TestClassMock).GetMethod("FirstTestMethod");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(SimpleTestMethodAttribute), simpleTestMethod), Is.True());

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(SimpleTestMethodAttribute), classType), Throws.An<InvalidCastException>());
            simpleTestMethod = typeof(TestClassWithMethodsWithParameters).GetMethod("TestMethod");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(SimpleTestMethodAttribute), simpleTestMethod), Throws.An<ArgumentException>());
        }

    }
}
