using System;

using JUUT_Core.Reporters;
using JUUT_Core.Runners;
using JUUT_Core.Sessions;

namespace JUUT_Core.Suites {

    public abstract class AbstractTestSuite : TestSuite {

        private readonly TestReporter Reporter;

        protected AbstractTestSuite(TestReporter reporter) {
            if (reporter == null) {
                throw new ArgumentException("The reporter of a test suite mustn't be null.");
            }

            Reporter = reporter;
        }

        public void RunSimpleTests(Session session) {
            TestRunner runner = new SimpleTestRunner();
            foreach (TestClassSession testClassSession in session.TestClassSessions) {
                Reporter.AddReport(runner.Run(testClassSession));
            }
            Reporter.PresentReports();
        }

    }

}