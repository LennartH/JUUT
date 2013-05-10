using System;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestAttributes {

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

            Type classType = typeof(TestClassMock);
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classType), Throws.An<InvalidCastException>());
            classSetUp = typeof(TestClassWithNonStaticClassOrganizeMethods).GetMethod("ClassSetUp");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Throws.An<ArgumentException>());
            classSetUp = typeof(TestClassWithMethodsWithParameters).GetMethod("ClassSetUp");
            AssertEx.That(() => JUUTAttribute.IsMemberValidFor(typeof(ClassSetUpAttribute), classSetUp), Throws.An<ArgumentException>());
        }

    }
}
