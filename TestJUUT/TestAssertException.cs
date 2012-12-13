using System;

using JUUT;

using Is = NHamcrest.Core.Is;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT {
    [TestClass]
    public class TestAssertException {
        [TestMethod]
        public void Creation() {
            AssertException e = new AssertException();
            AssertEx.That(e.Message, Is.EqualTo(""));

            e = new AssertException("Test");
            AssertEx.That(e.Message, Is.EqualTo("Test"));
            try {
                throw e;
                Assert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException) {
                // Do nothing
            }
        }
    }
}
