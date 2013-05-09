using System.Text;

using JUUT_Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest;
using NHamcrest.Core;

using TestJUUT.Util;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT.TestCore {

    [TestClass]
    public class TestAssertException {

        [TestMethod]
        public void Creation() {
            IDescription description = new StringDescription(new StringBuilder("description"));
            IDescription missmatchDescription = new StringDescription(new StringBuilder("was missmatch"));

            AssertException e = new AssertException();
            AssertEx.That(e.Message, Is.EqualTo(""));

            e = new AssertException("Test");
            AssertEx.That(e.Message, Is.EqualTo("Test"));

            e = new AssertException(description, missmatchDescription);
            AssertEx.That(e.Message, Is.EqualTo("Expected is description, but was missmatch."));

            e = new AssertException(description, missmatchDescription, "Custom message.");
            AssertEx.That(e.Message, Is.EqualTo("Custom message.\nExpected: description\nBut was missmatch"));
        }

        [TestMethod]
        public void EqualsAndHashCode() {
            AssertException exception = new AssertException("Exception text.");

            //Using IsTrue/IsFalse to cover all paths (aren't covered, when using Equals)
            //Equal tests
            Assert.IsTrue(exception.Equals(exception), "An object should allways be equal to itself (reference).");
            AssertEx.That(exception.GetHashCode(), Is.EqualTo(exception.GetHashCode()), "Equal objects should have equal hashcodes.");

            AssertException equal = new AssertException("Exception text.");
            Assert.IsTrue(exception.Equals(equal), "An object should be equal to an object with the same attributes.");
            AssertEx.That(exception.GetHashCode(), Is.EqualTo(equal.GetHashCode()), "Equal objects should have equal hashcodes.");

            //Unequal tests
            Assert.IsFalse(exception.Equals(null), "An object shouldn't be equal to null.");

            object unequal = new object();
            Assert.IsFalse(exception.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(exception.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");

            unequal = new AssertException("Unequal exception text.");
            Assert.IsFalse(exception.Equals(unequal), "An object shouldn't be equal to an object of an other type.");
            AssertEx.That(exception.GetHashCode(), Is.Not(unequal.GetHashCode()), "Unequal objects shouldn't have equal hashcodes.");
        }

    }

}
