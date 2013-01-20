using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.AttributeTests {

    [TestClass]
    public class TestClassSetUpAttribute {

        [TestMethod]
        public void Creation() {
            JUUTAttribute classSetUp = new ClassSetUpAttribute();
            AssertEx.That(classSetUp.Name, Is.EqualTo("ClassSetUp"));
            AssertEx.That(classSetUp.IsSetUpOrTearDown, Is.True());
        }

        [TestMethod]
        public void MemberValidation() {
            MemberInfo classSetUp = typeof(NotAttributedMock).GetMethod("Foo");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Is.False());
            classSetUp = typeof(TestClassMock).GetMethod("MockSetUp");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Is.True());

            classSetUp = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Throws.An<ArgumentException>());
            classSetUp = typeof(TestClassWithNonStaticClassSetUp).GetMethod("ClassSetUp");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Throws.An<ArgumentException>());
            classSetUp = typeof(TestClassWithOrganizeMethodsWithParameters).GetMethod("ClassSetUp");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Throws.An<ArgumentException>());
        }

    }
}
