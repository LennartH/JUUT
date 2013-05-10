using JUUT_Core.Reports;
using JUUT_Core.Sessions;

namespace JUUT_Core.Runners {

    public interface TestRunner {

        /// <summary>
        /// Runs all tests added to the runner.
        /// </summary>
        /// <param name="session"></param>
        ClassReport Run(TestClassSession session);

    }

}
