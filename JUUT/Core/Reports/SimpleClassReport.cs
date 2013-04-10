using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;
using JUUT.Core.Scanners;

namespace JUUT.Core.Reports {

    public class SimpleClassReport : ClassReport {

        public string Text {
            get {
                if (FailedTests == 0 && SucceededTests == 0 && NotRunnedTests == 0) {
                    return "No tests have been runned.";
                }
                return "";
            }
        }

        private readonly Dictionary<MethodInfo, MethodReport> Reports;
        public int RunnedTests {
            get { return FailedTests + SucceededTests; }
        }
        public int FailedTests { get; set; }
        public int SucceededTests { get; set; }
        public int NotRunnedTests { get; set; }

        public Type ClassType { get; private set; }

        public ReportStatus Status { get; private set; }

        public SimpleClassReport(Type classType) {
            if (classType == null) {
                throw new ArgumentException("The class type of a SimpleClassReport mustn't be null.");
            }
            if (classType.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The class needs the JUUTTestClass-Attribute.");
            }

            Reports = new Dictionary<MethodInfo, MethodReport>();
            ClassType = classType;
            Status = new ReportStatus.NotRunned();
            InitializeReports();
        }

        private void InitializeReports() {
            foreach (MethodInfo test in TestClassScanner.GetSimpleTestMethodsOfClass(ClassType)) {
                Add(new MethodReport(test) {Status = new ReportStatus.NotRunned()});
            }
        }

        public void Add(MethodReport report) {
            if (report.ClassType != ClassType) {
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

    }

}