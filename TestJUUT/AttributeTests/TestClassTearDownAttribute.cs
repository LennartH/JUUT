using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

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

    }
}
