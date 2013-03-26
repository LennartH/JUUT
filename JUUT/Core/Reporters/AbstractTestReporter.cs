using System;
using System.Collections.Generic;

using JUUT.Core.Reports;

namespace JUUT.Core.Reporters {

    public abstract class AbstractTestReporter : TestReporter {

        public Dictionary<Type, IList<Report>> Reports { get; private set; }

        protected AbstractTestReporter() {
            Reports = new Dictionary<Type, IList<Report>>();
        }

        public void AddReport(Report report) {
            Type testClassType = report.ClassType;
            if (!Reports.ContainsKey(testClassType)) {
                Reports[testClassType] = new List<Report>();
            }
            Reports[testClassType].Add(report);
        }

        public abstract void PresentReports();

    }

}
