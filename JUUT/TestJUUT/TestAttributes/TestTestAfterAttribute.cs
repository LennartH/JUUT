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
            JUUTAttribute testTearDown = new TestAfterAttribute();
            AssertEx.That(testTearDown.Name, Is.EqualTo("TestAfter"));
            AssertEx.That(testTearDown.IsSetUpOrTearDown, Is.False());
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

    }
}
