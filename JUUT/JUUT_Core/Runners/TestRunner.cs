using System;
using System.Reflection;

using JUUT_Core.Reports;

namespace JUUT_Core.Runners {
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
        /// Adds all tests of the test class to the runner.
        /// </summary>
        void AddAll();

        /// <summary>
        /// Adds the given test to the runner. This test has to be a member of the test class and needs a TestMethod Attribute.
        /// </summary>
        void Add(MethodInfo test);

        /// <summary>
        /// Runs all tests added to the runner.
        /// </summary>
        void Run();

    }
}
