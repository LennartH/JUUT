using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core {

    public abstract class AbstractTestReporter : TestReporter {

        public Dictionary<Type, IList<Report>> Reports { get; private set; }

        protected AbstractTestReporter() {
            Reports = new Dictionary<Type, IList<Report>>();
        }

        public void AddReport(Report report) {
            Type testClassType = report.TestClass;
            if (!Reports.ContainsKey(testClassType)) {
                Reports[testClassType] = new List<Report>();
            }
            Reports[testClassType].Add(report);
        }

        public abstract void PresentReports();

    }

}
