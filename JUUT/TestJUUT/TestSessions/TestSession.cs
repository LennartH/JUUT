using System;

using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestSessions {

    [TestClass]
    public class TestSession {

        [TestMethod]
        public void AddingTests() {
            Session session = new Session();
            AssertEx.That(() => session.AddAll(typeof(NotAttributedMock)), Throws.An<ArgumentException>());
            AssertEx.That(() => session.Add(typeof(NotAttributedMock).GetMethod("Foo")), Throws.An<ArgumentException>());
        }

    }

}
