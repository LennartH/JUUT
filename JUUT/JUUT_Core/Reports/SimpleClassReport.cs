using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using JUUT_Core.Attributes;

namespace JUUT_Core.Reports {

    public class SimpleClassReport : ClassReport {

        public string Text {
            get {
                if (RunnedTests == 0) {
                    return "No tests have been runned.";
                }

                StringBuilder builder = new StringBuilder(RunnedTests + " test" + (RunnedTests > 1 ? "s" : "") + " runned: " +
                                                          FailedTests + " failed, " + SucceededTests + " succeeded");
                foreach (MethodReport report in Reports.Values) {
                    if (report.Status.IsWorseThan(new ReportStatus.Success())) {
                        builder.Append("\n\n");
                        builder.Append(report.ShortText);
                    }
                }
                return builder.ToString();
            }
        }

        public ICollection<MethodReport> MethodReports {
            get { return Reports.Values; }
        }

        public int RunnedTests {
            get { return FailedTests + SucceededTests; }
        }
        public int FailedTests { get; set; }
        public int SucceededTests { get; set; }

        public Type TestClass { get; private set; }
        private readonly Dictionary<MethodInfo, MethodReport> Reports;
        public ReportStatus Status { get; private set; }

        public SimpleClassReport(Type testClass) {
            if (testClass == null) {
                throw new ArgumentException("The test class of a SimpleClassReport mustn't be null.");
            }
            if (testClass.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The test class of a SimpleClassReport needs the JUUTTestClass-Attribute.");
            }

            Reports = new Dictionary<MethodInfo, MethodReport>();
            TestClass = testClass;
            Status = new ReportStatus.NotRunned();
        }

        public void Add(MethodReport report) {
            if (report.TestClass != TestClass) {
                throw new ArgumentException("The declaring type of the given report isn't equal to the declaring type of this class report.");
            }

            Status = report.Status.IsWorseThan(Status) ? report.Status : Status;
            AdjustCounters(report);
            Reports[report.Method] = report;
        }

        private void AdjustCounters(MethodReport newReport) {
            if (Reports.ContainsKey(newReport.Method)) {
                DecrementCountersFor(Reports[newReport.Method]);
            }
            IncrementCountersFor(newReport);
        }

        private void IncrementCountersFor(MethodReport report) {
            report.Status.IncrementCounterFor(this);
        }

        private void DecrementCountersFor(MethodReport report) {
            report.Status.DecrementCounterFor(this);
        }

        #region HashCode and Equals
        private bool Equals(SimpleClassReport other) {
            return TestClass.Equals(other.TestClass);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((SimpleClassReport) obj);
        }

        public override int GetHashCode() {
            return TestClass.GetHashCode();
        }
        #endregion

    }

}