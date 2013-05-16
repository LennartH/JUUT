using System.Collections.Generic;
using System.Text;

using JUUT_Core.Reporters;
using JUUT_Core.Reports;

using UnityEngine;

namespace JUUT_Unity {

    public class UnityTestReporter : AbstractTestReporter {

        public ICollection<MethodReport> FailedReports { get; private set; }

        public UnityTestReporter() {
            FailedReports = new HashSet<MethodReport>();
        }

        public override void PresentReports() {
            Debug.Log(CreateSummaryText() + "\n\n");

            foreach (ClassReport classReport in Reports) {
                foreach (MethodReport report in classReport.MethodReports) {
                    if (report.Status.IsWorseThan(new ReportStatus.Success())) {
                        Debug.LogError(CreateReportText(report) + "\n\n");
                        FailedReports.Add(report);
                    }
                }
            }
        }

        private string CreateSummaryText() {
            int runnedTestsNumber = 0;
            int failedTestsNumber = 0;
            int succededTestsNumber = 0;

            foreach (ClassReport report in Reports) {
                runnedTestsNumber += report.RunnedTests;
                succededTestsNumber += report.SucceededTests;
                failedTestsNumber += report.FailedTests;
            }

            return runnedTestsNumber + " test" + (runnedTestsNumber > 1 ? "s" : "") + " runned: " +
                   failedTestsNumber + " failed, " + succededTestsNumber + " succeeded";
        }

        private static string CreateReportText(MethodReport report) {
            StringBuilder builder = new StringBuilder(report.Text);
            builder.Append("\n\n");
            builder.Append(report.RaisedException.StackTrace);
            return builder.ToString();
        }

    }

}
