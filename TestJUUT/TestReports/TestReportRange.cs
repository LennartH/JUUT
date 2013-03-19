using JUUT.Core.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestReports {

    [TestClass]
    public class TestReportRange {

        [TestMethod]
        public void EnumLikeBehaviourButWithProperties() {
            AssertEx.That(ReportRange.TestMethod.Name, Is.EqualTo("TestMethod"));
            AssertEx.That(ReportRange.TestClass.Name, Is.EqualTo("TestClass"));
        }

        [TestMethod]
        public void ReportRangeToString() {
            AssertEx.That(ReportRange.TestMethod.ToString(), Is.EqualTo("TestMethodRange"));
            AssertEx.That(ReportRange.TestClass.ToString(), Is.EqualTo("TestClassRange"));
        }

    }

}
