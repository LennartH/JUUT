using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Reporters;
using JUUT_Core.Runners;

namespace JUUT_Core.Suites {

    public abstract class AbstractTestSuite : TestSuite {

        private readonly TestReporter Reporter;
        private readonly Dictionary<Type, TestRunner> Runners; 

        protected AbstractTestSuite(TestReporter reporter) {
            if (reporter == null) {
                throw new ArgumentException("The reporter of a test suite mustn't be null.");
            }

            Reporter = reporter;
            Runners = new Dictionary<Type, TestRunner>();
        }

        public void AddAll(Type testClass) {
            if (!Runners.ContainsKey(testClass)) {
                Runners.Add(testClass, new SimpleTestRunner(testClass));
            }
            Runners[testClass].AddAll();
        }

        public void Add(MethodInfo test) {
            if (test.DeclaringType == null) {
                return;
            }

            if (!Runners.ContainsKey(test.DeclaringType)) {
                Runners.Add(test.DeclaringType, new SimpleTestRunner(test.DeclaringType));
            }
            Runners[test.DeclaringType].Add(test);
        }

        public void Run() {
            foreach (TestRunner runner in Runners.Values) {
                runner.Run();
                Reporter.AddReport(runner.Report);
            }
            Reporter.PresentReports();
        }

    }

}