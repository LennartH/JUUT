using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.AttributeTests {

    [TestClass]
    public class TestTestTearDownAttribute {

        [TestMethod]
        public void Creation() {
            JUUTAttribute testTearDown = new TestTearDownAttribute();
            AssertEx.That(testTearDown.Name, Is.EqualTo("TestTearDown"));
            AssertEx.That(testTearDown.IsSetUpOrTearDown, Is.True());
        }

        [TestMethod]
        public void MemberValidation() {
            MethodInfo testTearDown = typeof(NotAttributedMock).GetMethod("Bar");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(TestTearDownAttribute), testTearDown), Is.False());
            testTearDown = typeof(TestClassMock).GetMethod("MockTestTearDown");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(TestTearDownAttribute), testTearDown), Is.True());

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestTearDownAttribute), classType), Throws.An<InvalidCastException>());
            testTearDown = typeof(TestClassWithMethodsWithParameters).GetMethod("TearDown");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestTearDownAttribute), testTearDown), Throws.An<ArgumentException>());
        }

    }
}
