using System.Collections.Generic;

using JUUT_Core.Reports;

namespace JUUT_Core.Reporters {

    public abstract class AbstractTestReporter : TestReporter {

        public HashSet<ClassReport> Reports { get; private set; }

        protected AbstractTestReporter() {
            Reports = new HashSet<ClassReport>();
        }

        public void AddReport(ClassReport report) {
            Reports.Add(report);
        }

        public abstract void PresentReports();

    }

}
