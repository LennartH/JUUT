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
    public class TestClassTearDownAttribute {

        [TestMethod]
        public void Creation() {
            JUUTAttribute classTearDown = new ClassTearDownAttribute();
            AssertEx.That(classTearDown.Name, Is.EqualTo("ClassTearDown"));
            AssertEx.That(classTearDown.IsSetUpOrTearDown, Is.True());
        }

        [TestMethod]
        public void MemberValidation() {
            MethodInfo classTearDown = typeof(NotAttributedMock).GetMethod("Foo");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(ClassTearDownAttribute), classTearDown), Is.False());
            classTearDown = typeof(TestClassMock).GetMethod("MockTearDown");
            AssertEx.That(JUUTAttribute.IsMemberValidFor(typeof(ClassTearDownAttribute), classTearDown), Is.True());

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassTearDownAttribute), classType), Throws.An<InvalidCastException>());
            classTearDown = typeof(TestClassWithNonStaticClassOrganizeMethods).GetMethod("ClassTearDown");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassTearDownAttribute), classTearDown), Throws.An<ArgumentException>());
            classTearDown = typeof(TestClassWithMethodsWithParameters).GetMethod("ClassTearDown");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassTearDownAttribute), classTearDown), Throws.An<ArgumentException>());
        }

    }
}
