using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {
    [TestClass]
    public class TestAttributes {
        [TestMethod]
        public void JUUTTest() {
            AssertEx.That(1, Is.LessThan(2));
        }
    }
}
