using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core {

    public abstract class AbstractTestReporter : TestReporter {

        public Dictionary<Type, IList<TestReport>> Reports { get; private set; }

        protected AbstractTestReporter() {
            Reports = new Dictionary<Type, IList<TestReport>>();
        }

        public void AddReport(TestReport report) {
            Type testClassType = report.TestClassType;
            if (!Reports.ContainsKey(testClassType)) {
                Reports[testClassType] = new List<TestReport>();
            }
            Reports[testClassType].Add(report);
        }

        public abstract void PresentReports();

    }

}
