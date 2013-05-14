using System;
using System.Reflection;

using JUUT_Core.Sessions;

namespace JUUT_Core.Suites {

    public interface TestSuite {

        /// <summary>
        /// Runs all simple tests contained by the session and reports the results with it's reporter.
        /// </summary>
        /// <param name="session"></param>
        void RunSimpleTests(Session session);

    }

}
