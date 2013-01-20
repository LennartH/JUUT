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

    }
}
