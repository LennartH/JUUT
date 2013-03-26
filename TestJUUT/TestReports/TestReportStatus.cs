using System;

using JUUT.Core;
using JUUT.Core.Reports.Status;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestReports {

    [TestClass]
    public class TestReportStatus {

        [TestMethod]
        public void Creation() {
            Exception raisedException = null;
            ReportStatus status = ReportStatus.Create(raisedException);
            AssertEx.That(status is ReportStatusSuccess, Is.True());

            raisedException = new AssertException("Assert Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status is ReportStatusFailed, Is.True());

            raisedException = new NullReferenceException("Other Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status is ReportStatusError, Is.True());
        }

        [TestMethod]
        public void Name() {
            Exception raisedException = null;
            ReportStatus status = ReportStatus.Create(raisedException);
            AssertEx.That(status.Name, Is.EqualTo("Success"));

            raisedException = new AssertException("Assert Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status.Name, Is.EqualTo("Failed"));

            raisedException = new NullReferenceException("Other Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status.Name, Is.EqualTo("Error"));
        }

        [TestMethod]
        public void TestToString() {
            Exception raisedException = null;
            ReportStatus status = ReportStatus.Create(raisedException);
            AssertEx.That(status.ToString(), Is.EqualTo(status.Name));
        }

    }

}
