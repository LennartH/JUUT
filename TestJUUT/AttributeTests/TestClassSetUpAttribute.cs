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

    }
}
