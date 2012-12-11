using System;
using System.Reflection;

using JUUT.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {
    [TestClass]
    public class TestAttributes {

        [TestMethod]
        public void JUUTTest() {
            Attribute JUUTTest = typeof (JUUTTestAttributedMock).GetCustomAttribute(typeof (JUUTTestAttribute), true);
            AssertEx.That(JUUTTest, Is.NotNull());

            JUUTTest = typeof(NotAttributedMock).GetCustomAttribute(typeof(JUUTTestAttribute), true);
            AssertEx.That(JUUTTest, Is.Null());
        }

        [JUUTTest]
        private class JUUTTestAttributedMock {
            
        }

        private class NotAttributedMock {}
    }
}
