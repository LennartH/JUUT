using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

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

    }
}
