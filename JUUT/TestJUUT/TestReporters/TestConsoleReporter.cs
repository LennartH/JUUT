using System;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reporters;
using JUUT_Core.Runners;
using JUUT_Core.Sessions;

using NHamcrest.Core;

using TestJUUT.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = JUUT_Core.Assert;

namespace TestJUUT.TestReporters {

    [TestClass]
    public class TestConsoleReporter {

        private static string Text;

        [TestMethod]
        public void TextCreation() {
            TestReporter reporter = new ConsoleReporterMock();
            TestRunner runner = new SimpleTestRunner();

            TestClassSession session = new TestClassSession(typeof(TestMock));
            session.AddAll();
            reporter.AddReport(runner.Run(session));

            session = new TestClassSession(typeof(AnotherTestMock));
            session.AddAll();
            reporter.AddReport(runner.Run(session));

            reporter.PresentReports();
            AssertEx.That(Text, Is.EqualTo(
                    "3 tests runned: 2 failed, 1 succeeded\n\n" +
                    "The Bar-Method failed: Expected is 2, but was 1.\n\n" +
                    "The Foo-Method threw an unexpected exception: Error Text."));
        }

        #region Helper Classes
        private class ConsoleReporterMock : ConsoleReporter {

            public override void PresentReports() {
                Text = CreateText();
            }

        }

        [JUUTTestClass]
        private class TestMock {

            [SimpleTestMethod]
            public void Foo() {

            }

            [SimpleTestMethod]
            public void Bar() {
                Assert.That(1, Is.EqualTo(2));
            }

        }

        [JUUTTestClass]
        private class AnotherTestMock {

            [SimpleTestMethod]
            public void Foo() {
                throw new NullReferenceException("Error Text.");
            }

        }
        #endregion

    }

}
