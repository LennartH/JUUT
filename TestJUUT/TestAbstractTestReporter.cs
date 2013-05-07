using System.Collections.Generic;

using JUUT.Core.Reporters;
using JUUT.Core.Reports;

using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestAbstractTestReporter {

        [TestMethod]
        public void AddReport() {
            ClassReport alphaReport = new Mock<ClassReport>().Object;
            TestReporter reporter = new Mock<AbstractTestReporter>().Object;

            reporter.AddReport(alphaReport);
            AssertEx.That(reporter.Reports.Count, Is.EqualTo(1));
        }

        [TestMethod]
        public void Reports() {
            //Setting up the reports
            ClassReport fooReport = new Mock<ClassReport>().Object;
            ClassReport barReport = new Mock<ClassReport>().Object;
            ClassReport alphaReport = new Mock<ClassReport>().Object;

            //Setting up the reporter
            var reporterMock = new Mock<AbstractTestReporter>();
            TestReporter reporter = reporterMock.Object;

            reporter.AddReport(fooReport);
            reporter.AddReport(barReport);
            reporter.AddReport(alphaReport);

            //Checking the structure of the reports
            HashSet<ClassReport> reports = reporter.Reports;
            AssertEx.That(reports.Count, Is.EqualTo(3));
            AssertEx.That(reports.SetEquals(new List<ClassReport> {fooReport, barReport, alphaReport}), Is.True());
        }

        private class FooBarOwner { }
        private class AlphaOwner { }

    }

}