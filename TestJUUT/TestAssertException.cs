using System;
using System.Text;

using JUUT;

using NHamcrest;

using Is = NHamcrest.Core.Is;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT {
    [TestClass]
    public class TestAssertException {
        [TestMethod]
        public void Creation() {
            IDescription description = new StringDescription(new StringBuilder("description"));
            IDescription missmatchDescription = new StringDescription(new StringBuilder("was missmatch"));

            AssertException e = new AssertException("Test");
            AssertEx.That(e.Message, Is.EqualTo("Test"));

            e = new AssertException(description, missmatchDescription);
            AssertEx.That(e.Message, Is.EqualTo("Expected is description, but was missmatch."));

            e = new AssertException(description, missmatchDescription, "Custom message.");
            AssertEx.That(e.Message, Is.EqualTo("Custom message.\nExpected: description\nBut was missmatch"));
        }
    }
}
