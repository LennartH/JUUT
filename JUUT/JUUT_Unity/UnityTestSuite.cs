using System.Collections.Generic;

using JUUT_Core.Reports;
using JUUT_Core.Suites;

namespace JUUT_Unity {

    public class UnityTestSuite : AbstractTestSuite {

        public UnityTestSuite() : base(new UnityTestReporter()) { }

        public ICollection<MethodReport> FailedTests {
            get { return ((UnityTestReporter) Reporter).FailedReports; }
        }

    }

}
