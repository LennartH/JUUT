using System;

using JUUT_Core;
using JUUT_Core.Reports;

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
            AssertEx.That(status is ReportStatus.Success, Is.True());

            raisedException = new AssertException("Assert Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status is ReportStatus.Failed, Is.True());

            raisedException = new NullReferenceException("Other Exception");
            status = ReportStatus.Create(raisedException);
            AssertEx.That(status is ReportStatus.Error, Is.True());
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

            status = new ReportStatus.NotRunned();
            AssertEx.That(status.Name, Is.EqualTo("Not Runned"));
        }

        [TestMethod]
        public void DescribingText() {
            ReportStatus status = new ReportStatus.NotRunned();
            AssertEx.That(status.DescribingText, Is.EqualTo("wasn't run"));

            status = new ReportStatus.Success();
            AssertEx.That(status.DescribingText, Is.EqualTo("passed successfully"));

            status = new ReportStatus.Failed();
            AssertEx.That(status.DescribingText, Is.EqualTo("failed"));

            status = new ReportStatus.Error();
            AssertEx.That(status.DescribingText, Is.EqualTo("threw an unexpected exception"));
        }

        [TestMethod]
        public void BadnessOrder() {
            ReportStatus success = new ReportStatus.Success();
            ReportStatus failed = new ReportStatus.Failed();
            ReportStatus error = new ReportStatus.Error();
            ReportStatus notRunned = new ReportStatus.NotRunned();

            //Tests for Not Runned
            AssertEx.That(notRunned.IsWorseThan(notRunned), Is.False());
            AssertEx.That(notRunned.IsWorseThan(success), Is.False());
            AssertEx.That(notRunned.IsWorseThan(failed), Is.False());
            AssertEx.That(notRunned.IsWorseThan(error), Is.False());

            //Tests for Success
            AssertEx.That(success.IsWorseThan(notRunned), Is.True());
            AssertEx.That(success.IsWorseThan(success), Is.False());
            AssertEx.That(success.IsWorseThan(failed), Is.False());
            AssertEx.That(success.IsWorseThan(error), Is.False());

            //Tests for Failed
            AssertEx.That(failed.IsWorseThan(notRunned), Is.True());
            AssertEx.That(failed.IsWorseThan(success), Is.True());
            AssertEx.That(failed.IsWorseThan(failed), Is.False());
            AssertEx.That(failed.IsWorseThan(error), Is.False());

            //Tests for Error
            AssertEx.That(error.IsWorseThan(notRunned), Is.True());
            AssertEx.That(error.IsWorseThan(success), Is.True());
            AssertEx.That(error.IsWorseThan(failed), Is.True());
            AssertEx.That(error.IsWorseThan(error), Is.False());
        }

        [TestMethod]
        public void TestToString() {
            Exception raisedException = null;
            ReportStatus status = ReportStatus.Create(raisedException);
            AssertEx.That(status.ToString(), Is.EqualTo(status.Name));
        }

    }

}
