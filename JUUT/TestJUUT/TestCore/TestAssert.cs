using JUUT.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

using Assert = JUUT.Core.Assert;

namespace TestJUUT.TestCore {

    [TestClass]
    public class TestAssert {

        [TestMethod]
        public void Fail() {
            try {
                Assert.Fail();
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo(""));
            }

            const string failText = "A forced fail.";
            try {
                Assert.Fail(failText);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo(failText));
            }
        }

        [TestMethod]
        public void That() {
            // Call this method to check that it doesn't throw an exception.
            Assert.That(2, Is.LessThan(3));
            Assert.That("test", Is.EqualTo("test"));
            Assert.That(4, Is.EqualTo(4), "4 is allways equal to 4.");

            // Checking the exceptions thrown by Assert.
            try {
                Assert.That("test", Is.EqualTo("Test"));
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo("Expected is Test, but was test."));
            }

            try {
                Assert.That("test", Is.EqualTo("Test"), "This has to fail.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo("This has to fail.\nExpected: Test\nBut was test"));
            }
        }
    }

}
