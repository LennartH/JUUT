using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

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

    }
}
