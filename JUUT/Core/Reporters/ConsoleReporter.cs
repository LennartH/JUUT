using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JUUT.Core.Reports;

namespace JUUT.Core.Reporters {

    public abstract class ConsoleReporter : AbstractTestReporter {

        protected string CreateText() {
            List<MethodReport> failedTests = new List<MethodReport>();
            int runnedTestsNumber = 0;
            int failedTestsNumber = 0;
            int succededTestsNumber = 0;

            foreach (ClassReport report in Reports) {
                runnedTestsNumber += report.RunnedTests;
                succededTestsNumber += report.SucceededTests;
                failedTestsNumber += report.FailedTests;
                foreach (MethodReport test in report.MethodReports) {
                    if (test.Status.IsWorseThan(new ReportStatus.Success())) {
                        failedTests.Add(test);
                    }
                }
            }

            StringBuilder builder = new StringBuilder(
                runnedTestsNumber + " test" + (runnedTestsNumber > 1 ? "s" : "") + " runned: " +
                failedTestsNumber + " failed, " + succededTestsNumber + " succeeded");
            foreach (MethodReport report in failedTests) {
                builder.Append("\n\n");
                builder.Append(report.Text);
            }
            return builder.ToString();
        }

    }

}