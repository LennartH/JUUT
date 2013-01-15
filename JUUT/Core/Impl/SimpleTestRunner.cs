using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using JUUT.Core.Impl.Reports;

namespace JUUT.Core.Impl {

    public class SimpleTestRunner : TestRunner {

        public Type TestClass { get; private set; }

        public SimpleTestRunner(Type testClass) {
            TestClass = testClass;
        }

        /// <summary>
        /// Runs all tests and returns the reports of the runned tests.
        /// </summary>
        /// <returns>The reports of the runned tests.</returns>
        public List<Report> RunAll() {
            List<Report> resultReports = new List<Report>();
            Report report = RunClassSetUp();
            if (report != null) {
                resultReports.Add(report);
                return resultReports;
            }

            return resultReports;
        }

        /// <summary>TODO Was passiert wenns es keine Methode mit diesem namen gibt?
        /// Runs the test with the given name and returns the reports of the run.
        /// </summary>
        /// <param name="testMethodName">The name of the testmethod, that should be runned.</param>
        /// <returns>Report of the runned test.</returns>
        public Report Run(string testMethodName) {
            Report report = RunClassSetUp();
            if (report != null) {
                return report;
            }

            return report;
        }

        /// <summary>
        /// Runs the class set up method of the test class and returns a report, if an exception is thrown.
        /// If no exception is thrown, null is returned.
        /// </summary>
        /// <returns>A report if an exception is thrown. Otherwhise null.</returns>
        private TestClassReport RunClassSetUp() {
            try {
                TestClassScanner.GetClassSetUpOfTest(TestClass).Invoke(null, null);
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                return new TestClassReport(TestClassScanner.GetClassSetUpOfTest(TestClass), raisedException);
            }

            return null;
        }

    }

}
