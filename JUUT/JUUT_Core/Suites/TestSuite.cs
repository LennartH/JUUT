using System;
using System.Reflection;

using JUUT_Core.Sessions;

namespace JUUT_Core.Suites {

    public interface TestSuite {

        /// <summary>
        /// Runs all contained tests and reports the results with it's reporter.
        /// </summary>
        /// <param name="session"></param>
        void Run(Session session);

    }

}
