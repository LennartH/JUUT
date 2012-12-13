using System;

using JUUT;
using JUUTAssert = JUUT.Assert;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Is = NHamcrest.Core.Is;
using Throws = NHamcrest.Core.Throws;

namespace TestJUUT {

    [TestClass]
    public class TestAssert {

        [TestMethod]
        public void Fail() {
            try {
                JUUTAssert.Fail();
                MSTestAssert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo(""));
            }

            const string failText = "A forced fail.";
            try {
                JUUTAssert.Fail(failText);
                MSTestAssert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo(failText));
            }
        }

        [TestMethod]
        public void That() {
            // Call this method to check that it doesn't throw an exception.
            JUUTAssert.That(2, Is.LessThan(3));
            JUUTAssert.That("test", Is.EqualTo("test"));
            JUUTAssert.That(4, Is.EqualTo(4), "4 is allways equal to 4.");

            // Checking the exceptions thrown by Assert.
            try {
                JUUTAssert.That("test", Is.EqualTo("Test"));
                MSTestAssert.Fail("This line shouldn't be reached. An exception should be thrown.");
            } catch (AssertException e) {
                AssertEx.That(e.Message, Is.EqualTo("Expected was test, but is Test."));
            }
        }
    }

}
