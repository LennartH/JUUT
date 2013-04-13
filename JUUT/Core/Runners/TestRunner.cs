using System;
using System.Reflection;

using JUUT.Core.Reports;

namespace JUUT.Core.Runners {
    public interface TestRunner {

        /// <summary>
        /// The test class of the tests this runner will execute.
        /// </summary>
        Type TestClass { get; }

        /// <summary>
        /// The class report for the executed tests.<para />
        /// Use the run methods to fill the method.
        /// </summary>
        ClassReport Report { get; }

        /// <summary>
        /// Runs all tests of the test class.
        /// </summary>
        void RunAll();

        /// <summary>
        /// Runs the the given test method.<para />
        /// Has to be a member of the test class.
        /// </summary>
        void Run(MethodInfo testMethod);

    }
}
