using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JUUT_Core;
using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

namespace Assets.Tests {

    [JUUTTestClass]
    public class TestSample {

        [SimpleTestMethod]
        public void TestMethod() {
            Assert.That(12, Is.EqualtTo(10));
        }

    }

}
