using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;
using JUUT.Core.Reports;
using JUUT.Core.Scanners;

namespace JUUT.Core.Runners {

    public class SimpleTestRunner : TestRunner {

        public Type TestClass { get; private set; }

        public ClassReport Report { get; private set; }

        public SimpleTestRunner(Type testClass) {
            if (testClass == null) {
                throw new ArgumentException("The test class of a TestRunner mustn't be null.");
            }
            if (testClass.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The test class of a TestRunner needs the JUUTTestClass-Attribute.");
            }

            TestClass = testClass;
            Report = new SimpleClassReport(TestClass);
        }

        /// <summary>
        /// Runs all tests and returns the reports of the runned tests.
        /// </summary>
        /// <returns>The reports of the runned tests.</returns>
        public void RunAll() {
            RunClassSetUp();
            foreach (MethodInfo test in TestClassScanner.GetSimpleTestMethodsOfClass(TestClass)) {
                RunTestSetUp();
                RunTest(test);
                RunTestTearDown();
            }
            RunClassTearDown();
        }

        /// <summary>TODO Was passiert wenns es keine Methode mit diesem namen gibt?
        /// Runs the test with the given name and returns the reports of the run.
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns>Report of the runned test.</returns>
        public void Run(MethodInfo testMethod) {
            //TODO If test not in class throw/return?

            RunClassSetUp();

            RunTestSetUp();
            RunTest(testMethod);
            RunTestTearDown();

            RunClassTearDown();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        private void RunTest(MethodInfo testMethod) {
            throw new NotImplementedException();
        }

        private void RunClassTearDown() {
            throw new NotImplementedException();
        }

        private void RunTestTearDown() {
            throw new NotImplementedException();
        }

        private void RunTestSetUp() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs the class set up method of the test class and adds adjusts the report, if an exception is thrown.
        /// </summary>
        private void RunClassSetUp() {
            MethodInfo classSetUp = TestClassScanner.GetClassSetUpOfTestClass(TestClass);
            if (classSetUp == null) {
                return;
            }

            try {
                classSetUp.Invoke(null, null);
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                Report.Add(new MethodReport(classSetUp, raisedException));
            }
        }

    }

}
