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
            try {
                RunClassSetUp();
            } catch (Exception) {
                return;
            }

            object testInstance = Activator.CreateInstance(TestClass);
            foreach (MethodInfo test in TestClassScanner.GetSimpleTestMethodsOfClass(TestClass)) {
                try {
                    RunTestSetUp(testInstance);
                } catch (Exception) {
                    return;
                }

                RunTest(testInstance, test);

                try {
                    RunTestTearDown(testInstance);
                } catch (Exception) {
                    return;
                }
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

            object testInstance = Activator.CreateInstance(TestClass);

            try {
                RunClassSetUp();
                RunTestSetUp(testInstance);
            } catch (Exception) {
                return;
            }

            RunTest(testInstance, testMethod);

            try {
                RunTestTearDown(testInstance);
                RunClassTearDown();
            } catch (Exception) {
                return;
            }
        }

        /// <summary>
        /// Runs the class set up method of the test class and adjusts the report, if an exception is thrown.
        /// </summary>
        private void RunClassSetUp() {
            MethodInfo classSetUp = TestClassScanner.GetClassSetUpOfTestClass(TestClass);
            if (classSetUp == null) {
                return;
            }

            RunStaticOrganizeMethod(classSetUp);
        }

        /// <summary>
        /// Runs the test set up method for the given test class instance and adjusts the report, if an exception is thrown.
        /// </summary>
        private void RunTestSetUp(object testInstance) {
            MethodInfo testSetUp = TestClassScanner.GetTestSetUpOfTestClass(TestClass);
            if (testSetUp == null) {
                return;
            }

            RunInstanceMethod(testInstance, testSetUp, false);
        }

        /// <summary>
        /// Runs the test method for the given test class instance and adjusts the report.
        /// </summary>
        private void RunTest(object testInstance, MethodInfo testMethod) {
            RunInstanceMethod(testInstance, testMethod, true);
        }

        /// <summary>
        /// Runs the test tear down method for the given test class instance and adjusts the report, if an exception is thrown.
        /// </summary>
        private void RunTestTearDown(object testInstance) {
            MethodInfo testTearDown = TestClassScanner.GetTestTearDownOfTestClass(TestClass);
            if (testTearDown == null) {
                return;
            }

            RunInstanceMethod(testInstance, testTearDown, false);
        }

        /// <summary>
        /// Runs the class tear down method of the test class and adjusts the report, if an exception is thrown.
        /// </summary>
        private void RunClassTearDown() {
            MethodInfo classTearDown = TestClassScanner.GetClassTearDownOfClass(TestClass);
            if (classTearDown == null) {
                return;
            }

            RunStaticOrganizeMethod(classTearDown);
        }

        /// <summary>
        /// Runs the given static organize method and adjusts the report, if an exception is thrown.
        /// </summary>
        /// <param name="staticOrganizeMethod"></param>
        private void RunStaticOrganizeMethod(MethodInfo staticOrganizeMethod) {
            try {
                staticOrganizeMethod.Invoke(null, null);
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                Report.Add(new MethodReport(staticOrganizeMethod, raisedException));
                throw;
            }
        }

        /// <summary>
        /// Runs the method (test, test set up or test tear down) for the given test class instance and
        /// adjusts the report, if an exception is thrown or allwaysReport is true.
        /// </summary>
        private void RunInstanceMethod(object testInstance, MethodInfo method, bool allwaysReport) {
            try {
                method.Invoke(testInstance, null);
                if (allwaysReport) {
                    Report.Add(new MethodReport(method));
                }
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                Report.Add(new MethodReport(method, raisedException));
                throw;
            }

        }

    }

}
