using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Reporters;
using JUUT_Core.Runners;
using JUUT_Core.Sessions;

namespace JUUT_Core.Suites {

    public abstract class AbstractTestSuite : TestSuite {

        private readonly TestReporter Reporter;
        private readonly Dictionary<Type, TestClassSession> Runners; 

        protected AbstractTestSuite(TestReporter reporter) {
            if (reporter == null) {
                throw new ArgumentException("The reporter of a test suite mustn't be null.");
            }

            Reporter = reporter;
            Runners = new Dictionary<Type, TestClassSession>();
        }

        public void AddAll(Type testClass) {
            if (!Runners.ContainsKey(testClass)) {
                Runners.Add(testClass, new TestClassSession(testClass));
            }
            Runners[testClass].AddAll();
        }

        public void Add(MethodInfo test) {
            if (test.DeclaringType == null) {
                return;
            }

            if (!Runners.ContainsKey(test.DeclaringType)) {
                Runners.Add(test.DeclaringType, new TestClassSession(test.DeclaringType));
            }
            Runners[test.DeclaringType].Add(test);
        }

        public void Run() {
            TestRunner runner = new SimpleTestRunner();
            foreach (TestClassSession session in Runners.Values) {
                Reporter.AddReport(runner.Run(session));
            }
            Reporter.PresentReports();
        }

    }

}