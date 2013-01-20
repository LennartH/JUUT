using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.AttributeTests {

    [TestClass]
    public class TestTestSetUpAttribute {

        [TestMethod]
        public void Creation() {
            JUUTAttribute testSetUp = new TestSetUpAttribute();
            AssertEx.That(testSetUp.Name, Is.EqualTo("TestSetUp"));
            AssertEx.That(testSetUp.IsSetUpOrTearDown, Is.True());
        }

        [TestMethod]
        public void MemberValidation() {
            MethodInfo testSetUp = typeof(NotAttributedMock).GetMethod("Foo");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(TestSetUpAttribute), testSetUp), Is.False());
            testSetUp = typeof(TestClassMock).GetMethod("MockTestSetUp");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(TestSetUpAttribute), testSetUp), Is.True());

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestSetUpAttribute), classType), Throws.An<InvalidCastException>());
            testSetUp = typeof(TestClassWithOrganizeMethodsWithParameters).GetMethod("SetUp");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(TestSetUpAttribute), testSetUp), Throws.An<ArgumentException>());
        }

    }
}
