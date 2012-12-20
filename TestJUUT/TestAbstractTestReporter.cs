using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using JUUT;
using JUUT.Core;

using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestJUUT {

    [TestClass]
    public class TestAbstractTestReporter {

        [TestMethod]
        public void ReportManaging() {
            //Setting up the reports
            var fooReport = new Mock<TestReport>();
            MethodInfo fooMethodInfo = new DynamicMethod("Foo", null, null, typeof(FooBarOwner));
            fooReport.Setup(foo => foo.TestMethod).Returns(fooMethodInfo);
            fooReport.VerifyGet(foo => foo.TestMethod, Times.Exactly(1));

            var barReport = new Mock<TestReport>();
            MethodInfo barMethodInfo = new DynamicMethod("Bar", null, null, typeof(FooBarOwner));
            barReport.Setup(bar => bar.TestMethod).Returns(barMethodInfo);
            barReport.VerifyGet(bar => bar.TestMethod, Times.Exactly(1));

            var alphaReport = new Mock<TestReport>();
            MethodInfo alphaMethodInfo = new DynamicMethod("Alpha", null, null, typeof(AlphaOwner));
            alphaReport.Setup(alpha => alpha.TestMethod).Returns(alphaMethodInfo);
            alphaReport.VerifyGet(alpha => alpha.TestMethod, Times.Exactly(1));

            //Setting up the reporter
            var reporterMock = new Mock<AbstractTestReporter>();
            TestReporter reporter = reporterMock.Object;

            reporter.AddReport(fooReport);
            reporter.AddReport(barReport);
            reporter.AddReport(alphaReport);

            //Checking the structure of the reports
            Dictionary<Type, TestReport> reports = reporter.GetReports();
            foreach (KeyValuePair<Type, TestReport> reportEntry in reports) {
                //Checking that the reports are mapped by their owner class
                if (reportEntry.Key.Equals(typeof(FooBarOwner))) {
                    AssertEx.That(reportEntry.Value, Matches.AnyOf(Is.EqualTo(fooReport.Object), Is.EqualTo(barReport.Object)));
                } else if (reportEntry.Key.Equals(typeof(AlphaOwner))) {
                    AssertEx.That(reportEntry.Value, Is.EqualTo(alphaReport.Object));
                } else {
                    Assert.Fail("Unknown owner " + reportEntry.Key.Name);
                }
            }
        }

        private class FooBarOwner { }
        private class AlphaOwner { }

    }

}