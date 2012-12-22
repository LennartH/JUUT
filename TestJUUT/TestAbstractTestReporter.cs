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
        public void Reports() {
            //Setting up the reports
            var fooReport = new Mock<TestReport>();
            fooReport.Setup(foo => foo.TestClassType).Returns(typeof (FooBarOwner));

            var barReport = new Mock<TestReport>();
            barReport.Setup(bar => bar.TestClassType).Returns(typeof (FooBarOwner));

            var alphaReport = new Mock<TestReport>();
            alphaReport.Setup(alpha => alpha.TestClassType).Returns(typeof (AlphaOwner));

            //Setting up the reporter
            var reporterMock = new Mock<AbstractTestReporter>();
            TestReporter reporter = reporterMock.Object;

            reporter.AddReport(fooReport.Object);
            fooReport.VerifyGet(foo => foo.TestClassType, Times.Exactly(1));
            reporter.AddReport(barReport.Object);
            barReport.VerifyGet(bar => bar.TestClassType, Times.Exactly(1));
            reporter.AddReport(alphaReport.Object);
            alphaReport.VerifyGet(alpha => alpha.TestClassType, Times.Exactly(1));

            //Checking the structure of the reports
            Dictionary<Type, IList<TestReport>> reports = reporter.Reports;
            int count = 0;
            foreach (KeyValuePair<Type, IList<TestReport>> reportEntry in reports) {
                //Checking that the reports are mapped by their owner class
                foreach (TestReport report in reportEntry.Value) {
                    if (reportEntry.Key.Equals(typeof (FooBarOwner))) {
                        AssertEx.That(
                            report, Matches.AnyOf(Is.EqualTo(fooReport.Object), Is.EqualTo(barReport.Object)),
                            "Unexpected reporter for FooBarOwner.");
                    } else if (reportEntry.Key.Equals(typeof (AlphaOwner))) {
                        AssertEx.That(report, Is.EqualTo(alphaReport.Object), "Unexpected reporter for AlphaOwner.");
                    } else {
                        Assert.Fail("Unknown owner " + reportEntry.Key.Name);
                    }
                    count++;
                }
            }
            AssertEx.That(count, Is.EqualTo(3));
        }

        private class FooBarOwner { }

        private class AlphaOwner { }

    }

}