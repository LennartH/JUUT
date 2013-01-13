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
        public void AddReport() {
            var alphaReport = new Mock<Report>();
            alphaReport.Setup(alpha => alpha.TestClass).Returns(typeof(AlphaOwner));

            var nullReporter = new Mock<Report>();
            nullReporter.Setup(alpha => alpha.TestClass).Returns(default(Type));

            var reporterMock = new Mock<AbstractTestReporter>();
            TestReporter reporter = reporterMock.Object;

            reporter.AddReport(alphaReport.Object);
            AssertEx.That(reporter.Reports.Count, Is.EqualTo(1));

            AssertEx.That(() => reporter.AddReport(nullReporter.Object), Throws.An<ArgumentNullException>(),
                "The reporter has to throw an ArgumentNullException, when a report with a null test class type is added.\n");
        }

        [TestMethod]
        public void Reports() {
            //Setting up the reports
            var fooReport = new Mock<Report>();
            fooReport.Setup(foo => foo.TestClass).Returns(typeof (FooBarOwner));

            var barReport = new Mock<Report>();
            barReport.Setup(bar => bar.TestClass).Returns(typeof (FooBarOwner));

            var alphaReport = new Mock<Report>();
            alphaReport.Setup(alpha => alpha.TestClass).Returns(typeof (AlphaOwner));

            //Setting up the reporter
            var reporterMock = new Mock<AbstractTestReporter>();
            TestReporter reporter = reporterMock.Object;

            reporter.AddReport(fooReport.Object);
            fooReport.VerifyGet(foo => foo.TestClass, Times.Exactly(1));
            reporter.AddReport(barReport.Object);
            barReport.VerifyGet(bar => bar.TestClass, Times.Exactly(1));
            reporter.AddReport(alphaReport.Object);
            alphaReport.VerifyGet(alpha => alpha.TestClass, Times.Exactly(1));

            //Checking the structure of the reports
            Dictionary<Type, IList<Report>> reports = reporter.Reports;
            int count = 0;
            foreach (KeyValuePair<Type, IList<Report>> reportEntry in reports) {
                //Checking that the reports are mapped by their owner class
                foreach (Report report in reportEntry.Value) {
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