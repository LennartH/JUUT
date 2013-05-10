using System;

using JUUT_Core.Attributes;

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

    }
}
