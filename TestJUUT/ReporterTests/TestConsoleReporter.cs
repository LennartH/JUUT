using System;

using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;
using JUUT.Core.Reporters;
using JUUT.Core.Runners;

using NHamcrest.Core;

using TestJUUT.Util;

using Assert = JUUT.Core.Assert;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestJUUT.ReporterTests {

    [TestClass]
    public class TestConsoleReporter {

        private static string Text;

        [TestMethod]
        public void TextCreation() {
            TestReporter reporter = new ConsoleReporterMock();

            TestRunner runner = new SimpleTestRunner(typeof(TestMock));
            runner.AddAll();
            runner.Run();
            reporter.AddReport(runner.Report);

            runner = new SimpleTestRunner(typeof(AnotherTestMock));
            runner.AddAll();
            runner.Run();
            reporter.AddReport(runner.Report);

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
