using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JUUT_Core;
using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

using NHamcrest.Core;

namespace Assets.Tests {

    [JUUTTestClass]
    public class TestSample {

        [SimpleTestMethod]
        public void TestMethod1() {
            Assert.That(12, Is.EqualTo(10));
        }

        [SimpleTestMethod]
        public void TestMethod2() {
            throw new NullReferenceException("Error");
        }

        [SimpleTestMethod]
        public void TestMethod3() {
            Assert.That(10, Is.EqualTo(10));
        }

    }

}
